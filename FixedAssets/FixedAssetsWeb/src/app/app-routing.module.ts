import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProductListComponent } from './components/product-list/product-list.component';
import { PurchaseHistoryComponent } from './components/purchase-history/purchase-history.component';

export const routes: Routes = [
  { path: 'produtos', component: ProductListComponent },
  { path: 'produtos/:id', loadComponent: () => import('./components/product-details/product-details.component').then(m => m.ProductDetailsComponent) },
  { path: 'historico-compras', component: PurchaseHistoryComponent }, 
  { path: '', redirectTo: '/produtos', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
