using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SuperEsperanzaApi;
using SuperEsperanzaApi.Dao;
using SuperEsperanzaApi.Data;
using SuperEsperanzaApi.Services;
using SuperEsperanzaApi.Services.Interfaces;
using SuperEsperanzaApi.Mappings;
using SuperEsperanzaApi.Models;
using System.Text;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    // Agregar el filtro de autorización personalizado globalmente
    options.Filters.Add<SuperEsperanzaApi.Filters.AuthorizationErrorHandler>();
})
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.TypeInfoResolverChain.Add(AppJsonSerializerContext.Default);
});
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "SuperEsperanza API", Version = "v1" });
    
    // Configuración de seguridad JWT para Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header usando el esquema Bearer. Ejemplo: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });
    
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Configuración de AutoMapper
builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddSingleton<ConexionDB>();
builder.Services.AddScoped<UsuarioDAO>();
builder.Services.AddScoped<RolDAO>();
builder.Services.AddScoped<CategoriaDAO>();
builder.Services.AddScoped<ProductoDAO>();
builder.Services.AddScoped<ProveedorDAO>();
builder.Services.AddScoped<ClienteDAO>();
builder.Services.AddScoped<CompraDAO>();
builder.Services.AddScoped<LoteDAO>();
builder.Services.AddScoped<SesionDAO>();
builder.Services.AddScoped<FacturaDAO>();
builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IUsuarioCrudService, UsuarioCrudService>();
builder.Services.AddScoped<IRolService, RolService>();
builder.Services.AddScoped<IService<Categoria>, CategoriaService>();
builder.Services.AddScoped<IService<Producto>, ProductoService>();
builder.Services.AddScoped<IService<Proveedor>, ProveedorService>();
builder.Services.AddScoped<IService<Cliente>, ClienteService>();
builder.Services.AddScoped<ICompraService, CompraService>();
builder.Services.AddScoped<ILoteService, LoteService>();
builder.Services.AddScoped<ISesionService, SesionService>();
builder.Services.AddScoped<IFacturaService, FacturaService>();

var jwtSection = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSection["Key"]!);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = jwtSection["Issuer"],
        ValidateAudience = true,
        ValidAudience = jwtSection["Audience"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        NameClaimType = ClaimTypes.Name,
        RoleClaimType = ClaimTypes.Role
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "SuperEsperanza API v1");
    c.RoutePrefix = "swagger";
});

app.UseAuthentication();
app.UseAuthorization();

// Agregar middleware para manejar errores de autorización
app.UseMiddleware<SuperEsperanzaApi.Middleware.AuthorizationErrorHandlerMiddleware>();

app.MapControllers();

app.Run();
