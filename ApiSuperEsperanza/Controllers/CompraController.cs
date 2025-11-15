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
            return claim != null && int.TryParse(claim.Value, out int id) ? id : 0;
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

            var compra = _mapper.Map<Compra>(dto);
            compra.Id_UsuarioCreacion = GetUserId();

            // Mapear los detalles
            if (dto.Detalles != null)
            {
                compra.Detalles = dto.Detalles.Select(d => new DetalleCompra
                {
                    Id_Producto = d.Id_Producto,
                    Cantidad = d.Cantidad,
                    PrecioUnitario = d.PrecioUnitario,
                    Id_UsuarioCreacion = GetUserId()
                }).ToList();
            }

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

