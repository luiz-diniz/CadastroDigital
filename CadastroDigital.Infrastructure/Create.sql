IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'CadastroDigital')
BEGIN
    CREATE DATABASE CadastroDigital;
END

GO
USE CadastroDigital;
GO

CREATE TABLE Enderecos (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Cep VARCHAR(10) NOT NULL,
    Logradouro NVARCHAR(255) NOT NULL,
    Numero INT NOT NULL,
    Complemento NVARCHAR(255) NULL,
    Bairro NVARCHAR(100) NOT NULL,
    Cidade NVARCHAR(100) NOT NULL,
	Estado NVARCHAR(50) NOT NULL,
	UF CHAR(2) NULL,
	Localidade NVARCHAR(100) NULL,
	DDD NVARCHAR(14) NULL,
	IBGE CHAR(7) NULL
);

CREATE TABLE PessoasFisicas (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Cpf VARCHAR(14) NOT NULL UNIQUE,
    Nome NVARCHAR(100) NOT NULL,
    DataNascimento DATE NOT NULL,
    EnderecoId INT NULL,

    CONSTRAINT FK_PessoaFisica_Endereco FOREIGN KEY (EnderecoId) REFERENCES Enderecos(Id) ON DELETE SET NULL
);

CREATE TABLE PessoasJuridicas (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Cnpj VARCHAR(18) NOT NULL UNIQUE,
    RazaoSocial NVARCHAR(150) NOT NULL,
    NomeFantasia NVARCHAR(150) NOT NULL,
    DataAbertura DATE NOT NULL,
    SituacaoCadastral INT NOT NULL,
    EnderecoId INT NULL,

    CONSTRAINT FK_PessoaJuridica_Endereco FOREIGN KEY (EnderecoId) REFERENCES Enderecos(Id) ON DELETE SET NULL,
);