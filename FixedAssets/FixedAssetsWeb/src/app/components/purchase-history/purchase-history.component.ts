import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-purchase-history',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './purchase-history.component.html',
  styleUrl: './purchase-history.component.css'
})

export class PurchaseHistoryComponent implements OnInit {
  purchases: any[] = [];
  message: string | undefined;

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    // Simulação: Id do usuário logado
    const userId = 1; // Substitua pelo ID real

    this.http.get(`http://localhost:5212/api/user/${userId}/orders`).subscribe(
      (data: any) => {
        this.purchases = data;
      },
      error => {
        this.message = 'Erro ao carregar o histórico de compras.';
      }
    );
  }
}
