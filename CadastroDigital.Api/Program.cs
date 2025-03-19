using CadastroDigital.Application.Services;
using CadastroDigital.Domain.Ports.Repository;
using CadastroDigital.Domain.Ports.Services;
using CadastroDigital.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging();

var connectionString = builder.Configuration.GetConnectionString("Default");

if(string.IsNullOrEmpty(connectionString))
    throw new ArgumentNullException("Connection string null");

builder.Services.AddSingleton<IPessoaFisicaRepository>(new PessoaFisicaRepository(connectionString));
builder.Services.AddSingleton<IPessoaJuridicaRepository>(new PessoaJuridicaRepository(connectionString));
builder.Services.AddSingleton<IEnderecoRepository>(new EnderecoRepository(connectionString));

builder.Services.AddSingleton<IPessoaFisicaService, PessoaFisicaService>();
builder.Services.AddSingleton<IPessoaJuridicaService, PessoaJuridicaService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.MapControllers();
app.Run();