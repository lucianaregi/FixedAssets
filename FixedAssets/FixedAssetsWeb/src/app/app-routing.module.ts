//import { NgModule } from '@angular/core';
//import { RouterModule, Routes } from '@angular/router';
//import { ProductListComponent } from './components/product-list/product-list.component';
//import { ProductDetailsComponent } from './components/product-details/product-details.component';

//export const routes: Routes = [
//  { path: 'produtos', component: ProductListComponent },
//  { path: 'produtos/:id', component: ProductDetailsComponent },
//  { path: '', redirectTo: '/produtos', pathMatch: 'full' }
//];

//@NgModule({
//  imports: [RouterModule.forRoot(routes)],
//  exports: [RouterModule]
//})
//export class AppRoutingModule { }


import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProductListComponent } from './components/product-list/product-list.component';

export const routes: Routes = [
  { path: 'produtos', component: ProductListComponent },
  { path: 'produtos/:id', loadComponent: () => import('./components/product-details/product-details.component').then(m => m.ProductDetailsComponent) },
  { path: '', redirectTo: '/produtos', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
