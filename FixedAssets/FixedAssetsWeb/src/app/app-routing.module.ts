import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProductListComponent } from './components/product-list/product-list.component';
import { PurchaseHistoryComponent } from './components/purchase-history/purchase-history.component';
import { LoginComponent } from './components/login/login.component';
import { MostTradedAssetComponent } from './components/most-traded-asset/most-traded-asset.component';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'produtos/:id', component: ProductListComponent },  // Aceita o parâmetro id
  { path: 'produtos', component: ProductListComponent },
  { path: 'historico-compras/:userId', component: PurchaseHistoryComponent },
  { path: 'most-traded-assets', component: MostTradedAssetComponent },
  { path: '', redirectTo: '/login', pathMatch: 'full' }  // Redireciona para login por padrão
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
