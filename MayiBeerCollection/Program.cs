global using MayiBeerCollection;
global using MayiBeerCollection.Models;
global using Microsoft.EntityFrameworkCore;
global using MayiBeerCollection.DTO;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//ADD MAPERS
builder.Services.AddAutoMapper(config =>
{
    config.CreateMap<Cerveza, CervezaDTO>();
    config.CreateMap<CervezaDTO, Cerveza>();

    config.CreateMap<Ciudad, CiudadDTO>();
    config.CreateMap<CiudadDTO, Ciudad>();

    config.CreateMap<Estilo, EstiloDTO>();
    config.CreateMap<EstiloDTO, Estilo>();

    config.CreateMap<Marca, MarcaDTO>();
    config.CreateMap<MarcaDTO, Marca>();

    config.CreateMap<Pai, PaisDTO>();
    config.CreateMap<PaisDTO, Pai>();

    config.CreateMap<Usuario, UsuarioDTO>();
    config.CreateMap<UsuarioDTO, Usuario>();

}, typeof(Program));

//ADD CORS
builder.Services.AddCors(options => options.AddPolicy("AllowWebApp",
    builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

//JWT
builder.Configuration.AddJsonFile("appsettings.json");
var secretKey = builder.Configuration.GetSection("settings").GetSection("secretkey").ToString();
var keyBytes = Encoding.UTF8.GetBytes(secretKey);

builder.Services.AddAuthentication(config =>
{
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(config =>
{
    config.RequireHttpsMetadata = false;
    config.SaveToken = true;
    config.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
    };
});
//

//ADD CONTROLLERS
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddDbContext<MayiBeerCollectionContext>(x => x.UseSqlServer("Server=localhost; Database=MayiBeerCollection; Trusted_Connection=True; TrustServerCertificate=True"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//USE CORS
app.UseCors("AllowWebApp");

app.UseHttpsRedirection();

//JWT
app.UseAuthentication();
//

app.UseAuthorization();

app.MapControllers();

app.Run();
