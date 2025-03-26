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

builder.Services.AddScoped<DbSession>();
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

builder.Services.AddTransient<IPessoaRepository<PessoaFisica>, PessoaFisicaRepository>();
builder.Services.AddTransient<IPessoaRepository<PessoaJuridica>, PessoaJuridicaRepository>();
builder.Services.AddTransient<IEnderecoRepository, EnderecoRepository>();

builder.Services.AddTransient(typeof(IPessoaService<>), typeof(PessoaService<>));
builder.Services.AddTransient<IEnderecoService, EnderecoService>();
builder.Services.AddTransient<IIntegracaoCep, IntegracaoViaCepService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(p =>
{
    p.AllowAnyHeader();
    p.AllowAnyMethod();
    p.AllowAnyOrigin();
    p.SetIsOriginAllowed(o => true);
});

app.MapControllers();
app.Run();