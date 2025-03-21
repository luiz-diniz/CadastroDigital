import { Component } from '@angular/core';

import { MenubarModule } from 'primeng/menubar';
import { ButtonModule } from 'primeng/button';
import { MenuItem, SharedModule } from 'primeng/api'; 

@Component({
  selector: 'app-menu',
  standalone: true,
  imports: [ 
    MenubarModule,
    ButtonModule,
    SharedModule
  ],
  templateUrl: './menu.component.html',
  styleUrl: './menu.component.scss'
})
export class MenuComponent {

  items: MenuItem[] = [];
  
  ngOnInit() {
    this.items = [
      { label: 'Pessoa Física', routerLink: ['/pessoa-fisica'] },
      { label: 'Pessoa Jurídica', routerLink: ['/pessoa-juridica'] }     
    ];
  }
}
