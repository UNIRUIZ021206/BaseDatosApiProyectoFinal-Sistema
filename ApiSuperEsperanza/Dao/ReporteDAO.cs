using Microsoft.Data.SqlClient;
using SuperEsperanzaApi.Data;
using SuperEsperanzaApi.Dto;
using System.Data;

namespace SuperEsperanzaApi.Dao
{
    public class ReporteDAO
    {
        private readonly ConexionDB _conexion;

        public ReporteDAO(ConexionDB conexion)
        {
            _conexion = conexion;
        }

        /// <summary>
        /// Obtiene el reporte de ventas por fecha usando el procedimiento almacenado sp_ReporteVentas
        /// </summary>
        public async Task<List<ReporteVentasDto>> ObtenerReporteVentasAsync(DateTime fecha)
        {
            var lista = new List<ReporteVentasDto>();
            using var cn = await GetOpenConnectionAsync();
            using var cmd = new SqlCommand(Procedimientos.SP_REPORTE_VENTAS, cn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@Fecha", fecha.Date);

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                lista.Add(new ReporteVentasDto
                {
                    Producto = reader["Producto"].ToString() ?? string.Empty,
                    CantidadVendida = Convert.ToInt32(reader["CantidadVendida"]),
                    TotalVenta = Convert.ToDecimal(reader["TotalVenta"])
                });
            }
            return lista;
        }

        /// <summary>
        /// Obtiene el inventario general usando la vista vw_InventarioGeneral
        /// </summary>
        public async Task<List<InventarioGeneralDto>> ObtenerInventarioGeneralAsync()
        {
            var lista = new List<InventarioGeneralDto>();
            using var cn = await GetOpenConnectionAsync();
            using var cmd = new SqlCommand($"SELECT * FROM {Procedimientos.VW_INVENTARIO_GENERAL}", cn)
            {
                CommandType = CommandType.Text
            };

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                lista.Add(new InventarioGeneralDto
                {
                    Id_Producto = Convert.ToInt32(reader["Id_Producto"]),
                    CodigoProducto = reader["CodigoProducto"].ToString() ?? string.Empty,
                    NombreProducto = reader["NombreProducto"].ToString() ?? string.Empty,
                    NombreCategoria = reader["NombreCategoria"].ToString() ?? string.Empty,
                    PrecioVenta = Convert.ToDecimal(reader["PrecioVenta"]),
                    StockActual = Convert.ToInt32(reader["StockActual"]),
                    StockDisponibleEnLotes = Convert.ToInt32(reader["StockDisponibleEnLotes"]),
                    EstadoProducto = Convert.ToBoolean(reader["EstadoProducto"])
                });
            }
            return lista;
        }

        private async Task<SqlConnection> GetOpenConnectionAsync()
        {
            var cn = _conexion.ObtenerConexion();
            await cn.OpenAsync();
            return cn;
        }
    }
}

