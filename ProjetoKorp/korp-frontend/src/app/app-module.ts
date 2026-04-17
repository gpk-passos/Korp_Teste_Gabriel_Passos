import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http'; 
import { FaturamentoService } from './services/faturamento';
import { EstoqueService } from './services/estoque';

import { AppRoutingModule } from './app-routing-module';
import { App } from './app';

@NgModule({
  declarations: [
    App 
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule
  ],
  providers: [EstoqueService,
    FaturamentoService,],
  bootstrap: [App]
})
export class AppModule { }