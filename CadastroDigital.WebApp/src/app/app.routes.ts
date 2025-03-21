import { Routes } from '@angular/router';
import { PessoaFisicaComponent } from './components/pessoa-fisica/pessoa-fisica.component';
import { PessoaJuridicaComponent } from './components/pessoa-juridica/pessoa-juridica.component';

export const routes: Routes = [
    {
        path: 'pessoa-fisica',
        component: PessoaFisicaComponent
    },
    {
        path: 'pessoa-juridica',
        component: PessoaJuridicaComponent
    },
    {
        path: '',
        redirectTo: '/pessoa-fisica',
        pathMatch: 'full'   
    }
];
