import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { Purchase } from '../../models/purchase.model'; 

@Component({
  selector: 'app-purchase-history',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './purchase-history.component.html',
  styleUrl: './purchase-history.component.css'
})

export class PurchaseHistoryComponent implements OnInit {
  purchases: Purchase[] = [];
  message: string | undefined;

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    // Simulação: Id do usuário logado
    const userId = 1;

    // Fazendo uma chamada HTTP GET para obter o histórico de compras do usuário
    this.http.get<Purchase[]>(`http://localhost:5212/api/user/${userId}/orders`).subscribe(
      (data: Purchase[]) => {
        this.purchases = data;  // Agora "data" é tipado como um array de "Purchase"
      },
      () => {
        this.message = 'Erro ao carregar o histórico de compras.';  // Erro tratado genericamente
      }
    );

  }
 
}
