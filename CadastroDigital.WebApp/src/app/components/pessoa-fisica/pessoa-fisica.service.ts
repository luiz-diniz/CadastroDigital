import { HttpClient } from "@angular/common/http";
import { inject, Injectable, OnInit } from "@angular/core";
import { Observable } from "rxjs";
import { PessoaFisicaModel } from "./models/pessoa-fisica.model";
import { environment } from "../../../environments/environment";

@Injectable()
export class PessoaFisicaService{

    private httpClient = inject(HttpClient);
    private baseUrl: string = `${environment.url}fisicas`

    criar(pessoa: PessoaFisicaModel) : Observable<any>{
        return this.httpClient.post<any>(this.baseUrl, pessoa);
    }

    listar() : Observable<PessoaFisicaModel[]>{
        return this.httpClient.get<any>(this.baseUrl);
    }

    excluir(id: number) : Observable<any>{
        return this.httpClient.delete<any>(`${this.baseUrl}/${id}`);
    }
}