import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Product } from '../models/product.model';

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  private apiUrl = '/api/product'; 

  constructor(private http: HttpClient) { }

  // Método para buscar a lista de produtos
  getProducts(): Observable<Product[]> {
    return this.http.get<Product[]>(this.apiUrl);
  }

  // Método para buscar um produto pelo ID
  getProductById(id: number): Observable<Product> {
    return this.http.get<Product>(`http://localhost:5212/api/products/${id}`);
  }


}
