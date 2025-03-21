import { Component, inject, OnInit } from '@angular/core';
import { SharedModule } from 'primeng/api';
import { FormGroup, FormControl, Validators, ReactiveFormsModule } from '@angular/forms';

import { InputTextModule } from 'primeng/inputtext';
import { FieldsetModule } from 'primeng/fieldset';
import { ButtonModule } from 'primeng/button';
import { CalendarModule } from 'primeng/calendar';
import { FloatLabelModule } from 'primeng/floatlabel';
import { TableModule } from 'primeng/table';
import { CommonModule } from '@angular/common';
import { ToastrService } from 'ngx-toastr';

import { PessoaFisicaService } from './pessoa-fisica.service';
import { PessoaFisicaModel } from './models/pessoa-fisica.model';
import { switchMap } from 'rxjs';
@Component({
  selector: 'app-pessoa-fisica',
  standalone: true,
  imports: [
    CommonModule,
    ButtonModule,
    SharedModule,
    InputTextModule,
    FieldsetModule,
    CalendarModule,
    ReactiveFormsModule,
    FloatLabelModule,
    TableModule 
  ],
  templateUrl: './pessoa-fisica.component.html',
  styleUrl: './pessoa-fisica.component.scss'
})
export class PessoaFisicaComponent implements OnInit {

  form!: FormGroup;
  novoRegistro: boolean = false;
  pessoas: PessoaFisicaModel[] = [];

  pessoaFisicaService = inject(PessoaFisicaService);
  toastr = inject(ToastrService);

  ngOnInit(): void {
    this.carregarLista();

    this.form = new FormGroup({
      cpf: new FormControl('', [
        Validators.required,
        Validators.pattern('^[0-9]{3}([.-]?[0-9]{3}){2}-[0-9]{2}$')
      ]),
      nome: new FormControl('', [Validators.required, Validators.maxLength(100)]),
      dataNascimento: new FormControl(null, [Validators.required]),

      cep: new FormControl('', [
        Validators.required,
        Validators.pattern('^[0-9]{5}-[0-9]{3}$')
      ]),
      logradouro: new FormControl('', [Validators.required, Validators.maxLength(255)]),
      numero: new FormControl('', [Validators.required]),
      complemento: new FormControl('', [Validators.maxLength(255)]),
      bairro: new FormControl('', [Validators.required, Validators.maxLength(100)]),
      cidade: new FormControl('', [Validators.required, Validators.maxLength(100)]),
      estado: new FormControl('', [Validators.required, Validators.maxLength(50)]),
    });
  }

  onSubmit(): void {
    const pessoa: PessoaFisicaModel = this.form.getRawValue();

    pessoa.endereco = {
      cep: this.form.get('cep')?.value.replace('-', ''),
      logradouro: this.form.get('logradouro')?.value, 
      numero: this.form.get('numero')?.value,
      complemento: this.form.get('complemento')?.value,
      bairro: this.form.get('bairro')?.value,
      cidade: this.form.get('cidade')?.value,
      estado: this.form.get('estado')?.value
    };

      this.pessoaFisicaService.criar(pessoa).pipe(
        switchMap(() => this.pessoaFisicaService.listar())
      ).subscribe({
        next: (pessoas) => {
          this.pessoas = pessoas;
          this.toastr.success('Pessoa física criada com sucesso!');
          this.novoRegistro = false;
        },
        error: (error) => {
          console.error('Erro ao criar pessoas físicas', error);
          this.toastr.error('Erro ao criar pessoa física', 'Error');         
        }
      });
    }

  excluir(id: number) {
    this.pessoaFisicaService.excluir(id).pipe(
      switchMap(() => this.pessoaFisicaService.listar())
    ).subscribe({
      next: (pessoas) => {
        this.pessoas = pessoas;
        this.toastr.success('Pessoa física excluída com sucesso!');
      },
      error: (error) => {
        console.error('Erro ao excluir pessoa física', error);
        this.toastr.error('Erro ao excluir pessoa física', 'Error');         
      }
    });
  }

  carregarLista(){
    this.pessoaFisicaService.listar().subscribe({
      next: (pessoas) => {
        this.pessoas = pessoas;
      },
      error: (error) => {
        console.error('Erro ao listar pessoas físicas', error);
        this.toastr.error('Erro o ao listar pessoas físicas', 'Error');         
      }
    })
  }
}