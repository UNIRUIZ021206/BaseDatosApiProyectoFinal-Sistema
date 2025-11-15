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
    public class SesionController : ControllerBase
    {
        private readonly ISesionService _service;
        private readonly IMapper _mapper;

        public SesionController(ISesionService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        private int GetUserId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            return claim != null && int.TryParse(claim.Value, out int id) ? id : 0;
        }

        [HttpGet("activas")]
        public async Task<ActionResult<IEnumerable<SesionDto>>> GetActivas()
        {
            var lista = await _service.ListarSesionesActivasAsync();
            return Ok(_mapper.Map<IEnumerable<SesionDto>>(lista));
        }

        [HttpPost("abrir")]
        [Authorize(Roles = "Administrador,Cajero")]
        public async Task<ActionResult<SesionDto>> Abrir(SesionCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var sesion = _mapper.Map<Sesion>(dto);
            sesion.Id_Usuario = GetUserId();
            sesion.Id_UsuarioCreacion = GetUserId();

            var (ok, error, idSesion) = await _service.AbrirSesionAsync(sesion);
            if (!ok)
            {
                return BadRequest(new ErrorResponse { Error = error });
            }

            sesion.Id_Sesion = idSesion ?? 0;
            var dtoCreado = _mapper.Map<SesionDto>(sesion);
            return CreatedAtAction(nameof(Abrir), new { id = sesion.Id_Sesion }, dtoCreado);
        }

        [HttpPost("cerrar")]
        [Authorize(Roles = "Administrador,Cajero,Supervisor")] // Supervisor puede cerrar sesiones
        public async Task<ActionResult> Cerrar(SesionCerrarDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userId = GetUserId();
            var (ok, error) = await _service.CerrarSesionAsync(dto.Id_Sesion, userId);

            if (!ok)
            {
                return BadRequest(new ErrorResponse { Error = error });
            }

            return Ok(new MensajeResponse { Mensaje = "Sesión cerrada correctamente" });
        }
    }
}

