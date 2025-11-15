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
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioCrudService _service;
        private readonly IMapper _mapper;

        public UsuarioController(IUsuarioCrudService service, IMapper mapper)
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
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult<IEnumerable<UsuarioDto>>> GetAll()
        {
            var lista = await _service.ListarAsync();
            return Ok(_mapper.Map<IEnumerable<UsuarioDto>>(lista));
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult<UsuarioDto>> GetById(int id)
        {
            var obj = await _service.ObtenerPorIdAsync(id);
            if (obj == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<UsuarioDto>(obj));
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult<UsuarioDto>> Create(UsuarioCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var entity = _mapper.Map<Usuario>(dto);
            entity.Id_UsuarioCreacion = GetUserId();

            var (ok, error) = await _service.CrearAsync(entity, dto.Clave);
            if (!ok)
            {
                return BadRequest(new ErrorResponse { Error = error });
            }

            var dtoCreado = _mapper.Map<UsuarioDto>(entity);
            return CreatedAtAction(nameof(GetById), new { id = entity.Id_Usuario }, dtoCreado);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrador")]
        public async Task<ActionResult> Update(int id, UsuarioUpdateDto dto)
        {
            var entity = await _service.ObtenerPorIdAsync(id);
            if (entity == null)
            {
                return NotFound();
            }

            _mapper.Map(dto, entity);
            entity.Id_UsuarioModificacion = GetUserId();

            var (ok, error) = await _service.ActualizarAsync(entity, dto.Clave);
            if (!ok)
            {
                return BadRequest(new ErrorResponse { Error = error });
            }

            return Ok(new MensajeResponse { Mensaje = "Usuario actualizado" });
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

