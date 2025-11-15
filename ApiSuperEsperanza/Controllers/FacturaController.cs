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
            return claim != null && int.TryParse(claim.Value, out int id) ? id : 0;
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

            var factura = _mapper.Map<Factura>(dto);
            factura.Id_UsuarioCreacion = GetUserId();

            // Mapear los detalles
            if (dto.Detalles != null)
            {
                factura.Detalles = dto.Detalles.Select(d => new DetalleFactura
                {
                    Id_Lote = d.Id_Lote,
                    Cantidad = d.Cantidad,
                    PrecioUnitario = d.PrecioUnitario,
                    Id_UsuarioCreacion = GetUserId()
                }).ToList();
            }

            var (ok, error, idFactura) = await _service.CrearFacturaAsync(factura);
            if (!ok)
            {
                return BadRequest(new ErrorResponse { Error = error });
            }

            factura.Id_Factura = idFactura ?? 0;
            var dtoCreado = _mapper.Map<FacturaDto>(factura);
            return CreatedAtAction(nameof(Create), new { id = factura.Id_Factura }, dtoCreado);
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

