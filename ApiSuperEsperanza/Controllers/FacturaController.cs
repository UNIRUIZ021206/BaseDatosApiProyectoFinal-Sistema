using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuperEsperanzaApi.Dto;
using SuperEsperanzaApi.Models;
using SuperEsperanzaApi.Services;
using System.Security.Claims;

namespace SuperEsperanzaApi.Controlador
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class FacturaController : ControllerBase
    {
        private readonly IFacturaService _service;
        private readonly IMapper _mapper;

        public FacturaController(IFacturaService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        private int GetUserId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null || !int.TryParse(claim.Value, out int id) || id <= 0)
            {
                throw new UnauthorizedAccessException("Usuario no autenticado o ID de usuario inválido.");
            }
            return id;
        }

        [HttpGet("{id}/detalles")]
        public async Task<ActionResult<IEnumerable<DetalleFacturaDto>>> GetDetalles(int id)
        {
            var detalles = await _service.ListarDetallesPorFacturaAsync(id);
            return Ok(_mapper.Map<IEnumerable<DetalleFacturaDto>>(detalles));
        }

        [HttpPost]
        [Authorize(Roles = "Administrador,Cajero")]
        public async Task<ActionResult<FacturaDto>> Create(FacturaCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // Validar que haya detalles
            if (dto.Detalles == null || !dto.Detalles.Any())
            {
                return BadRequest(new ErrorResponse { Error = "La factura debe tener al menos un detalle." });
            }

            var userId = GetUserId();
            System.Diagnostics.Debug.WriteLine($"[FacturaController.Create] Iniciando creación de factura");
            System.Diagnostics.Debug.WriteLine($"  - Usuario ID: {userId}");
            System.Diagnostics.Debug.WriteLine($"  - Código Factura: {dto.CodigoFactura}");
            System.Diagnostics.Debug.WriteLine($"  - Cantidad de detalles recibidos: {dto.Detalles.Count}");
            
            foreach (var det in dto.Detalles)
            {
                System.Diagnostics.Debug.WriteLine($"    Detalle recibido: Lote ID {det.Id_Lote}, Cantidad {det.Cantidad}, Precio {det.PrecioUnitario}");
                
                // Validar cada detalle antes de procesar
                if (det.Cantidad <= 0)
                {
                    return BadRequest(new ErrorResponse { Error = $"La cantidad debe ser mayor a cero para el lote ID {det.Id_Lote}" });
                }
                
                if (det.PrecioUnitario <= 0)
                {
                    return BadRequest(new ErrorResponse { Error = $"El precio unitario debe ser mayor a cero para el lote ID {det.Id_Lote}" });
                }
                
                if (det.Id_Lote <= 0)
                {
                    return BadRequest(new ErrorResponse { Error = $"El ID de lote es inválido: {det.Id_Lote}" });
                }
            }

            var factura = _mapper.Map<Factura>(dto);
            factura.Id_UsuarioCreacion = userId;

            // Mapear los detalles y calcular subtotal
            factura.Detalles = dto.Detalles.Select(d => new DetalleFactura
            {
                Id_Lote = d.Id_Lote,
                Cantidad = d.Cantidad,
                PrecioUnitario = d.PrecioUnitario,
                Subtotal = d.Cantidad * d.PrecioUnitario, // Calcular subtotal inmediatamente
                Id_UsuarioCreacion = userId
            }).ToList();

            System.Diagnostics.Debug.WriteLine($"[FacturaController.Create] Detalles mapeados: {factura.Detalles.Count}");
            foreach (var det in factura.Detalles)
            {
                System.Diagnostics.Debug.WriteLine($"    Detalle mapeado: Lote ID {det.Id_Lote}, Cantidad {det.Cantidad}, Precio {det.PrecioUnitario}, Usuario {det.Id_UsuarioCreacion}");
            }

            try
            {
                var (ok, error, idFactura) = await _service.CrearFacturaAsync(factura);
                if (!ok)
                {
                    System.Diagnostics.Debug.WriteLine($"[FacturaController.Create] Error del servicio: {error}");
                    return BadRequest(new ErrorResponse { Error = error });
                }

                factura.Id_Factura = idFactura ?? 0;
                var dtoCreado = _mapper.Map<FacturaDto>(factura);
                System.Diagnostics.Debug.WriteLine($"[FacturaController.Create] Factura creada exitosamente con ID: {factura.Id_Factura}");
                return CreatedAtAction(nameof(Create), new { id = factura.Id_Factura }, dtoCreado);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[FacturaController.Create] Excepción capturada: {ex.GetType().Name} - {ex.Message}");
                if (ex.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine($"[FacturaController.Create] Excepción interna: {ex.InnerException.Message}");
                }
                return BadRequest(new ErrorResponse { Error = $"Error al procesar la factura: {ex.Message}" });
            }
        }

        [HttpPost("anular")]
        [Authorize(Roles = "Administrador,Supervisor")] // Supervisor puede anular facturas
        public async Task<ActionResult> Anular(FacturaAnularDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userId = GetUserId();
            var (ok, error) = await _service.AnularFacturaAsync(dto.Id_Factura, dto.Motivo, userId);

            if (!ok)
            {
                return BadRequest(new ErrorResponse { Error = error });
            }

            return Ok(new MensajeResponse { Mensaje = "Factura anulada correctamente" });
        }
    }
}

