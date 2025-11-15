using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuperEsperanzaApi.Dto;
using SuperEsperanzaApi.Models;
using SuperEsperanzaApi.Services.Interfaces;
using System.Security.Claims;

namespace SuperEsperanzaApi.Controlador
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProductoController : ControllerBase
    {
        private readonly IService<Producto> _service;
        private readonly IMapper _mapper;

        public ProductoController(IService<Producto> service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        private int GetUserId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            return claim != null && int.TryParse(claim.Value, out int id) ? id : 0;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductoDto>>> GetAll()
        {
            var lista = await _service.ListarAsync();
            return Ok(_mapper.Map<IEnumerable<ProductoDto>>(lista));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductoDto>> GetById(int id)
        {
            var obj = await _service.ObtenerPorIdAsync(id);
            if (obj == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<ProductoDto>(obj));
        }

        [HttpPost]
        [Authorize(Roles = "Administrador,Bodeguero")]
        public async Task<ActionResult<ProductoDto>> Create(ProductoCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var entity = _mapper.Map<Producto>(dto);
            entity.Id_UsuarioCreacion = GetUserId();

            var (ok, error) = await _service.CrearAsync(entity);
            if (!ok)
            {
                return BadRequest(new ErrorResponse { Error = error });
            }

            var dtoCreado = _mapper.Map<ProductoDto>(entity);
            return CreatedAtAction(nameof(GetById), new { id = entity.Id_Producto }, dtoCreado);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrador,Bodeguero,Supervisor")]
        public async Task<ActionResult> Update(int id, ProductoUpdateDto dto)
        {
            var entity = await _service.ObtenerPorIdAsync(id);
            if (entity == null)
            {
                return NotFound();
            }

            _mapper.Map(dto, entity);
            entity.Id_UsuarioModificacion = GetUserId();

            var (ok, error) = await _service.ActualizarAsync(entity);
            if (!ok)
            {
                return BadRequest(new ErrorResponse { Error = error });
            }

            return Ok(new MensajeResponse { Mensaje = "Producto actualizado" });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> Delete(int id)
        {
            var userId = GetUserId();
            var (ok, error) = await _service.EliminarAsync(id, userId);

            if (!ok)
            {
                return BadRequest(new ErrorResponse { Error = error });
            }
            return NoContent();
        }
    }
}

