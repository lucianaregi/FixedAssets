//import { Injectable } from '@angular/core';
//import { HttpClient } from '@angular/common/http';
//import { Observable } from 'rxjs';

//@Injectable({
//  providedIn: 'root'
//})
//export class ProductService {
//  private apiUrl = 'http://localhost:5212/api/product';

//  constructor(private http: HttpClient) { }

//  getProducts(): Observable<any[]> {
//    return this.http.get<any[]>(this.apiUrl);
//  }
//}

import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  private apiUrl = 'http://localhost:5212/api/product'; 

  constructor(private http: HttpClient) { }

  // Método para buscar a lista de produtos
  getProducts(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl);
  }

  // Método para buscar um produto pelo ID
  getProductById(id: string): Observable<any> {
    const url = `${this.apiUrl}/${id}`; // Construa a URL com o ID do produto
    return this.http.get<any>(url); // Faça uma requisição GET para a URL
  }


}
