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
    public class LoteController : ControllerBase
    {
        private readonly ILoteService _service;
        private readonly IMapper _mapper;

        public LoteController(ILoteService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        private int GetUserId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            return claim != null && int.TryParse(claim.Value, out int id) ? id : 0;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LoteDto>> GetById(int id)
        {
            var lote = await _service.ObtenerLotePorIdAsync(id);
            if (lote == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<LoteDto>(lote));
        }

        [HttpGet("producto/{idProducto}")]
        public async Task<ActionResult<IEnumerable<LoteDto>>> GetByProducto(int idProducto)
        {
            var lista = await _service.ListarLotesPorProductoAsync(idProducto);
            return Ok(_mapper.Map<IEnumerable<LoteDto>>(lista));
        }

        [HttpPost]
        [Authorize(Roles = "Administrador,Bodeguero")]
        public async Task<ActionResult<LoteDto>> Create(LoteCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var lote = _mapper.Map<Lote>(dto);
            lote.Id_UsuarioCreacion = GetUserId();

            var (ok, error, idLote) = await _service.CrearLoteAsync(lote);
            if (!ok)
            {
                return BadRequest(new ErrorResponse { Error = error });
            }

            lote.Id_Lote = idLote ?? 0;
            var dtoCreado = _mapper.Map<LoteDto>(lote);
            return CreatedAtAction(nameof(Create), new { id = lote.Id_Lote }, dtoCreado);
        }
    }
}

