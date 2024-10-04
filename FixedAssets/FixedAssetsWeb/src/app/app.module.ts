import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';

import { AppComponent } from './app.component';
import { ProductListComponent } from './components/product-list/product-list.component';

@NgModule({
  declarations: [
    AppComponent,
    ProductListComponent  // Componentes que vamos declarar
  ],
  imports: [
    BrowserModule,
    HttpClientModule  // Importa o HttpClientModule para chamadas HTTP
  ],
  providers: [],
  bootstrap: [AppComponent]  // Inicializa o AppComponent no início
})
export class AppModule { }
