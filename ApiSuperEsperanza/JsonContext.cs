using System.Collections.Generic;
using System.Text.Json.Serialization; // <--- ESTA ES LA L�NEA CR�TICA QUE FALTABA
using Microsoft.AspNetCore.Mvc;       // <--- Necesario para ProblemDetails
using SuperEsperanzaApi.Dto;
using SuperEsperanzaApi.Models;

namespace SuperEsperanzaApi
{
    [JsonSourceGenerationOptions(GenerationMode = JsonSourceGenerationMode.Default, PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
    
    // --- Autenticaci�n y General ---
    [JsonSerializable(typeof(LoginRequest))]
    [JsonSerializable(typeof(LoginResponse))]
    [JsonSerializable(typeof(ApiStatusResponse))]
    [JsonSerializable(typeof(ErrorResponse))]
    [JsonSerializable(typeof(ErrorDetailResponse))]
    [JsonSerializable(typeof(MensajeResponse))]
    [JsonSerializable(typeof(AuthorizationErrorResponse))]
    [JsonSerializable(typeof(Usuario))]
    [JsonSerializable(typeof(Rol))]                  // <-- A�adido: metadata para Rol
    [JsonSerializable(typeof(IEnumerable<Rol>))]     // <-- A�adido: colecciones de Rol
    [JsonSerializable(typeof(List<Rol>))]
    [JsonSerializable(typeof(Rol[]))]

    // --- Errores Est�ndar de ASP.NET Core ---
    [JsonSerializable(typeof(ProblemDetails))]
    [JsonSerializable(typeof(ValidationProblemDetails))]
    [JsonSerializable(typeof(Dictionary<string, string[]>))]

    // ... otros registros ...

    // --- M�DULO CATEGORIA ---
    [JsonSerializable(typeof(Categoria))]
    [JsonSerializable(typeof(IEnumerable<Categoria>))]
    [JsonSerializable(typeof(List<Categoria>))]

    [JsonSerializable(typeof(CategoriaDto))]
    [JsonSerializable(typeof(IEnumerable<CategoriaDto>))]
    [JsonSerializable(typeof(List<CategoriaDto>))]

    [JsonSerializable(typeof(CategoriaCreateDto))]
    [JsonSerializable(typeof(CategoriaUpdateDto))]

    // --- MÓDULO PRODUCTO ---
    [JsonSerializable(typeof(Producto))]
    [JsonSerializable(typeof(IEnumerable<Producto>))]
    [JsonSerializable(typeof(List<Producto>))]
    [JsonSerializable(typeof(ProductoDto))]
    [JsonSerializable(typeof(IEnumerable<ProductoDto>))]
    [JsonSerializable(typeof(List<ProductoDto>))]
    [JsonSerializable(typeof(ProductoCreateDto))]
    [JsonSerializable(typeof(ProductoUpdateDto))]

    // --- MÓDULO PROVEEDOR ---
    [JsonSerializable(typeof(Proveedor))]
    [JsonSerializable(typeof(IEnumerable<Proveedor>))]
    [JsonSerializable(typeof(List<Proveedor>))]
    [JsonSerializable(typeof(ProveedorDto))]
    [JsonSerializable(typeof(IEnumerable<ProveedorDto>))]
    [JsonSerializable(typeof(List<ProveedorDto>))]
    [JsonSerializable(typeof(ProveedorCreateDto))]
    [JsonSerializable(typeof(ProveedorUpdateDto))]

    // --- MÓDULO CLIENTE ---
    [JsonSerializable(typeof(Cliente))]
    [JsonSerializable(typeof(IEnumerable<Cliente>))]
    [JsonSerializable(typeof(List<Cliente>))]
    [JsonSerializable(typeof(ClienteDto))]
    [JsonSerializable(typeof(IEnumerable<ClienteDto>))]
    [JsonSerializable(typeof(List<ClienteDto>))]
    [JsonSerializable(typeof(ClienteCreateDto))]
    [JsonSerializable(typeof(ClienteUpdateDto))]

    // --- MÓDULO USUARIO (CRUD) ---
    [JsonSerializable(typeof(IEnumerable<Usuario>))]
    [JsonSerializable(typeof(List<Usuario>))]
    [JsonSerializable(typeof(UsuarioDto))]
    [JsonSerializable(typeof(IEnumerable<UsuarioDto>))]
    [JsonSerializable(typeof(List<UsuarioDto>))]
    [JsonSerializable(typeof(UsuarioCreateDto))]
    [JsonSerializable(typeof(UsuarioUpdateDto))]

    // --- MÓDULO COMPRA ---
    [JsonSerializable(typeof(Compra))]
    [JsonSerializable(typeof(IEnumerable<Compra>))]
    [JsonSerializable(typeof(List<Compra>))]
    [JsonSerializable(typeof(CompraDto))]
    [JsonSerializable(typeof(IEnumerable<CompraDto>))]
    [JsonSerializable(typeof(List<CompraDto>))]
    [JsonSerializable(typeof(CompraCreateDto))]
    [JsonSerializable(typeof(DetalleCompra))]
    [JsonSerializable(typeof(DetalleCompraDto))]
    [JsonSerializable(typeof(DetalleCompraCreateDto))]
    [JsonSerializable(typeof(DetalleCompraUpdateDto))]

    // --- MÓDULO LOTE ---
    [JsonSerializable(typeof(Lote))]
    [JsonSerializable(typeof(IEnumerable<Lote>))]
    [JsonSerializable(typeof(List<Lote>))]
    [JsonSerializable(typeof(LoteDto))]
    [JsonSerializable(typeof(IEnumerable<LoteDto>))]
    [JsonSerializable(typeof(List<LoteDto>))]
    [JsonSerializable(typeof(LoteCreateDto))]

    // --- MÓDULO SESION ---
    [JsonSerializable(typeof(Sesion))]
    [JsonSerializable(typeof(IEnumerable<Sesion>))]
    [JsonSerializable(typeof(List<Sesion>))]
    [JsonSerializable(typeof(SesionDto))]
    [JsonSerializable(typeof(IEnumerable<SesionDto>))]
    [JsonSerializable(typeof(List<SesionDto>))]
    [JsonSerializable(typeof(SesionCreateDto))]
    [JsonSerializable(typeof(SesionCerrarDto))]

    // --- MÓDULO FACTURA ---
    [JsonSerializable(typeof(Factura))]
    [JsonSerializable(typeof(IEnumerable<Factura>))]
    [JsonSerializable(typeof(List<Factura>))]
    [JsonSerializable(typeof(FacturaDto))]
    [JsonSerializable(typeof(IEnumerable<FacturaDto>))]
    [JsonSerializable(typeof(List<FacturaDto>))]
    [JsonSerializable(typeof(FacturaCreateDto))]
    [JsonSerializable(typeof(DetalleFactura))]
    [JsonSerializable(typeof(DetalleFacturaDto))]
    [JsonSerializable(typeof(DetalleFacturaCreateDto))]
    [JsonSerializable(typeof(FacturaAnularDto))]

    internal partial class AppJsonSerializerContext : JsonSerializerContext
    {
    }
}