import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { User } from '../models/user.model';
import { environment } from '../../environments/environment';
import { Order } from '../models/order.model';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private apiUrl = `${environment.apiUrl}/user`;  

  constructor(private http: HttpClient) { }

  // Método para obter os detalhes de um usuário
  getUserById(id: number): Observable<User> {
    return this.http.get<User>(`${this.apiUrl}/${id}`);
  }

  // Método para obter o saldo do usuário 
  getUserBalance(id: number): Observable<number> {
    return this.http.get<User>(`${this.apiUrl}/${id}`)
      .pipe(map(user => user.balance));  // Mapeia o objeto User para retornar o saldo
  }

  // Método para obter as ordens de um usuário
  getUserOrders(id: number): Observable<Order[]> {
    return this.http.get<Order[]>(`${this.apiUrl}/${id}/orders`);
  }
}
