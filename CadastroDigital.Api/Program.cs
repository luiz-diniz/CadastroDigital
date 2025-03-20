using CadastroDigital.Application.Services;
using CadastroDigital.Domain.Entities;
using CadastroDigital.Domain.Ports.Repository;
using CadastroDigital.Domain.Ports.Services;
using CadastroDigital.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging();
builder.Services.AddHttpClient<IIntegracaoCep, IntegracaoViaCepService>();

var connectionString = builder.Configuration.GetConnectionString("Default");

if(string.IsNullOrEmpty(connectionString))
    throw new ArgumentNullException("Connection string null");

builder.Services.AddSingleton<IPessoaRepository<PessoaFisica>>(new PessoaFisicaRepository(connectionString));
builder.Services.AddSingleton<IPessoaRepository<PessoaJuridica>>(new PessoaJuridicaRepository(connectionString));
builder.Services.AddSingleton<IEnderecoRepository>(new EnderecoRepository(connectionString));

builder.Services.AddSingleton(typeof(IPessoaService<>), typeof(PessoaService<>));
builder.Services.AddSingleton<IEnderecoService, EnderecoService>();
builder.Services.AddSingleton<IIntegracaoCep, IntegracaoViaCepService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();
app.Run();