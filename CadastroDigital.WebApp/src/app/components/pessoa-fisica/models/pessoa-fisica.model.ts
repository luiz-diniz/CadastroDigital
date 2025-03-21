import { EnderecoModel } from "./endereco.model";

export class PessoaFisicaModel{
    id?: number;
    cpf?: string;
    nome?: string;
    dataNascimento?: Date;
    endereco?: EnderecoModel;
}