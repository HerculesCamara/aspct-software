using ASPCTS.Context;
using ASPCTS.Mappings;
using ASPCTS.Repositories;
using ASPCTS.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

//DB Conection 

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConexaoPadrao")));


//Repostories
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IPaiRepository, PaiRepository>();
builder.Services.AddScoped<IPsicologoRepository, PsicologoRepository>();
builder.Services.AddScoped<ICriancaRepository, CriancaRepository>();
builder.Services.AddScoped<IAtividadeRepository, AtividadeRepository>();


// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(x =>
    {
        x.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
        x.JsonSerializerOptions.WriteIndented = true;
    }); ;
builder.Services.AddScoped<IAtividadeService, AtividadeService>();
builder.Services.AddScoped<ICriancaService, CriancaService>();
builder.Services.AddScoped<IPaiService, PaiService>();
builder.Services.AddScoped<IPsicologoService, PsicologoService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Version = "v1",
        Title = "API para ASPCTS",
        Description = "API para acompanhamento de criancas com desordem do espectro autista",
        Contact = new OpenApiContact
        {
            Name = "Gustavo Maia",
            Email = "gustavogomesmaia@gmail.com"
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
        c.RoutePrefix = string.Empty; // Exibe Swagger na raiz (opcional)
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
