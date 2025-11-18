using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuperEsperanzaApi.Dto;
using SuperEsperanzaApi.Services;

namespace SuperEsperanzaApi.Controlador
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ReporteController : ControllerBase
    {
        private readonly IReporteService _service;

        public ReporteController(IReporteService service)
        {
            _service = service;
        }

        /// <summary>
        /// Obtiene el reporte de ventas por fecha
        /// </summary>
        [HttpGet("ventas")]
        [Authorize(Roles = "Administrador,Supervisor,Gerente,Contador")]
        public async Task<ActionResult<List<ReporteVentasDto>>> GetReporteVentas([FromQuery] DateTime fecha)
        {
            try
            {
                var reporte = await _service.ObtenerReporteVentasAsync(fecha);
                return Ok(reporte);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse { Error = ex.Message });
            }
        }

        /// <summary>
        /// Obtiene el inventario general
        /// </summary>
        [HttpGet("inventario")]
        [Authorize(Roles = "Administrador,Bodeguero,Supervisor,Gerente,Contador")]
        public async Task<ActionResult<List<InventarioGeneralDto>>> GetInventarioGeneral()
        {
            try
            {
                var inventario = await _service.ObtenerInventarioGeneralAsync();
                return Ok(inventario);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse { Error = ex.Message });
            }
        }
    }
}

