Projeto com finalidade de cadastrar Pessoas físicas e jurídicas seguindo a arquitetura hexagonal.

O projeto conta com os seguintes módulos:

CadastroDigital.Domain - Contém as entidades da aplicação e os ports de repositório e serviços.
CadastroDigital.Application - Módulo reponsável por implementar os services e fazer a orquestração da lógica com uso dos services e repositórios.
CadastroDigital.Infrastructure - Implementação da camada de dados em SQL Server, usando ADO.NET.
CadastroDigital.Api - Projeto de API que serve como porta de entrada da aplicação. 
