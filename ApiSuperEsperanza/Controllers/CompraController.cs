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
    public class CompraController : ControllerBase
    {
        private readonly ICompraService _service;
        private readonly IMapper _mapper;

        public CompraController(ICompraService service, IMapper mapper)
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
        public async Task<ActionResult<IEnumerable<DetalleCompraDto>>> GetDetalles(int id)
        {
            var detalles = await _service.ListarDetallesPorCompraAsync(id);
            return Ok(_mapper.Map<IEnumerable<DetalleCompraDto>>(detalles));
        }

        [HttpPut("detalles/{id}")]
        [Authorize(Roles = "Administrador,Bodeguero")]
        public async Task<ActionResult> UpdateDetalle(int id, DetalleCompraUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var detalle = _mapper.Map<DetalleCompra>(dto);
            detalle.Id_DetalleCompra = id;

            var userId = GetUserId();
            var (ok, error) = await _service.ActualizarDetalleCompraAsync(detalle, userId);

            if (!ok)
            {
                return BadRequest(new ErrorResponse { Error = error });
            }

            return Ok(new MensajeResponse { Mensaje = "Detalle de compra actualizado correctamente" });
        }

        [HttpDelete("detalles/{id}")]
        [Authorize(Roles = "Administrador")] // Bodeguero no puede eliminar según el script SQL
        public async Task<ActionResult> DeleteDetalle(int id)
        {
            var (ok, error) = await _service.EliminarDetalleCompraAsync(id);

            if (!ok)
            {
                return BadRequest(new ErrorResponse { Error = error });
            }

            return Ok(new MensajeResponse { Mensaje = "Detalle de compra eliminado correctamente" });
        }

        [HttpPost]
        [Authorize(Roles = "Administrador,Bodeguero")]
        public async Task<ActionResult<CompraDto>> Create(CompraCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // Validar que la fecha esté en el rango válido de SQL Server
            var minDate = new DateTime(1753, 1, 1, 0, 0, 0, DateTimeKind.Unspecified);
            var maxDate = new DateTime(9999, 12, 31, 23, 59, 59, DateTimeKind.Unspecified);
            
            // Normalizar la fecha recibida (puede venir con zona horaria desde JSON)
            var fechaRecibida = dto.FechaCompra;
            
            // Log para debugging
            System.Diagnostics.Debug.WriteLine($"Fecha recibida en API: {fechaRecibida:yyyy-MM-dd HH:mm:ss}, Kind: {fechaRecibida.Kind}");
            
            // Extraer solo la parte de fecha, ignorando la hora y zona horaria
            // Si la fecha viene como UTC o Local, convertirla a Unspecified
            DateTime fechaSoloFecha;
            if (fechaRecibida.Kind == DateTimeKind.Utc || fechaRecibida.Kind == DateTimeKind.Local)
            {
                // Si viene con zona horaria, tomar solo la fecha sin convertir
                fechaSoloFecha = new DateTime(fechaRecibida.Year, fechaRecibida.Month, fechaRecibida.Day, 0, 0, 0, DateTimeKind.Unspecified);
            }
            else
            {
                fechaSoloFecha = fechaRecibida.Date;
            }
            
            // Validar rango
            if (fechaSoloFecha < minDate.Date || fechaSoloFecha > maxDate.Date)
            {
                return BadRequest(new ErrorResponse 
                { 
                    Error = $"La fecha de compra debe estar entre {minDate:yyyy-MM-dd} y {maxDate:yyyy-MM-dd}. Fecha recibida: {fechaRecibida:yyyy-MM-dd HH:mm:ss} (Kind: {fechaRecibida.Kind})" 
                });
            }

            // Crear una nueva fecha sin zona horaria para evitar problemas de serialización y SQL Server
            // Asegurar que sea exactamente medianoche sin zona horaria
            var fechaNormalizada = new DateTime(
                fechaSoloFecha.Year,
                fechaSoloFecha.Month,
                fechaSoloFecha.Day,
                0, 0, 0,
                DateTimeKind.Unspecified
            );
            
            // Validación final antes de asignar
            if (fechaNormalizada < minDate || fechaNormalizada > maxDate)
            {
                return BadRequest(new ErrorResponse 
                { 
                    Error = $"La fecha normalizada está fuera del rango válido. Fecha: {fechaNormalizada:yyyy-MM-dd HH:mm:ss}" 
                });
            }
            
            System.Diagnostics.Debug.WriteLine($"Fecha normalizada: {fechaNormalizada:yyyy-MM-dd HH:mm:ss}, Kind: {fechaNormalizada.Kind}");
            
            dto.FechaCompra = fechaNormalizada;

            // Validar que haya detalles
            if (dto.Detalles == null || !dto.Detalles.Any())
            {
                return BadRequest(new ErrorResponse { Error = "La compra debe tener al menos un detalle." });
            }

            var compra = _mapper.Map<Compra>(dto);
            compra.Id_UsuarioCreacion = GetUserId();
            
            // Asegurar que la fecha en el modelo también esté normalizada
            compra.FechaCompra = fechaNormalizada;

            // Mapear los detalles
            compra.Detalles = dto.Detalles.Select(d => new DetalleCompra
            {
                Id_Producto = d.Id_Producto,
                Cantidad = d.Cantidad,
                PrecioUnitario = d.PrecioUnitario,
                Id_UsuarioCreacion = GetUserId()
            }).ToList();

            var (ok, error, idCompra) = await _service.CrearCompraAsync(compra);
            if (!ok)
            {
                return BadRequest(new ErrorResponse { Error = error });
            }

            compra.Id_Compra = idCompra ?? 0;
            var dtoCreado = _mapper.Map<CompraDto>(compra);
            return CreatedAtAction(nameof(Create), new { id = compra.Id_Compra }, dtoCreado);
        }
    }
}

