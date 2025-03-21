import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';
import { MenuComponent } from "./components/layout/menu/menu.component";
import { HeaderComponent } from "./components/layout/header/header.component";
import { FooterComponent } from './components/layout/footer/footer.component';
import { ButtonModule } from 'primeng/button';
import { SharedModule } from 'primeng/api';
import { ReactiveFormsModule } from '@angular/forms';
import { PessoaFisicaService } from './components/pessoa-fisica/pessoa-fisica.service';
import { HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    CommonModule, 
    RouterOutlet,
    MenuComponent, 
    HeaderComponent, 
    FooterComponent,
    ButtonModule,
    SharedModule,
    ReactiveFormsModule,
    HttpClientModule
  ],
  providers: [
    PessoaFisicaService
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent implements OnInit {

  title = 'CadastroDigital.WebApp';

  ngOnInit(): void {
    
  }

}
