using ASPCTS.Context;
using ASPCTS.Mappings;
using ASPCTS.Repositories;
using ASPCTS.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Authentication.JwtBearer;

using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

//DB Conection 

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConexaoPadrao")));


//Repostories
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IResponsavelRepository, ResponsavelRepository>();
builder.Services.AddScoped<IPsicologoRepository, PsicologoRepository>();
builder.Services.AddScoped<ICriancaRepository, CriancaRepository>();
builder.Services.AddScoped<IAtividadeRepository, AtividadeRepository>();
builder.Services.AddScoped<IRelatorioRepository, RelatorioRepository>();



// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(x =>
    {
        x.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
        x.JsonSerializerOptions.WriteIndented = true;
    }); ;

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "ASPCTS",
        ValidAudience = "ASPCTSUsers",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MihaChaveSuperSeguraJWTcom32Bytes!!!")),
        RoleClaimType = ClaimTypes.Role,
        NameClaimType = ClaimTypes.NameIdentifier
    };
});
builder.Services.AddScoped<IAtividadeService, AtividadeService>();
builder.Services.AddScoped<ICriancaService, CriancaService>();
builder.Services.AddScoped<IResponsavelService, ResponsavelService>();
builder.Services.AddScoped<IPsicologoService, PsicologoService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IRelatorioService, RelatorioService>();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "ASPCTS API", Version = "v1" });

    // Configura o esquema de segurança para JWT
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Insira o token JWT desta forma: Bearer {seu_token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            new string[] {}
        }
    });
}
);

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ASPCTS API v1");
        c.RoutePrefix = string.Empty; // Exibe Swagger na raiz
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
