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
    public class ProveedorController : ControllerBase
    {
        private readonly IService<Proveedor> _service;
        private readonly IMapper _mapper;

        public ProveedorController(IService<Proveedor> service, IMapper mapper)
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
        public async Task<ActionResult<IEnumerable<ProveedorDto>>> GetAll()
        {
            var lista = await _service.ListarAsync();
            return Ok(_mapper.Map<IEnumerable<ProveedorDto>>(lista));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProveedorDto>> GetById(int id)
        {
            var obj = await _service.ObtenerPorIdAsync(id);
            if (obj == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<ProveedorDto>(obj));
        }

        [HttpPost]
        [Authorize(Roles = "Administrador,Bodeguero")]
        public async Task<ActionResult<ProveedorDto>> Create(ProveedorCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var entity = _mapper.Map<Proveedor>(dto);
            entity.Id_UsuarioCreacion = GetUserId();

            var (ok, error) = await _service.CrearAsync(entity);
            if (!ok)
            {
                return BadRequest(new ErrorResponse { Error = error });
            }

            var dtoCreado = _mapper.Map<ProveedorDto>(entity);
            return CreatedAtAction(nameof(GetById), new { id = entity.Id_Proveedor }, dtoCreado);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrador,Bodeguero")]
        public async Task<ActionResult> Update(int id, ProveedorUpdateDto dto)
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

            return Ok(new MensajeResponse { Mensaje = "Proveedor actualizado" });
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

