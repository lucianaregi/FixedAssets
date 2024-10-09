import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { Purchase } from '../../models/purchase.model';
import { environment } from '../../../environments/environment';
import { catchError } from 'rxjs/operators';
import { throwError, Observable } from 'rxjs';
import { ActivatedRoute } from '@angular/router';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-purchase-history',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './purchase-history.component.html',
  styleUrls: ['./purchase-history.component.css']
})
export class PurchaseHistoryComponent implements OnInit {
  purchases: Purchase[] = [];
  message: string | undefined;
  userId: number | null = null; 

  constructor(private http: HttpClient, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      this.userId = Number(params.get('userId')) || this.getUserIdFromLocalStorage();
      if (this.userId !== null) {
        this.loadPurchaseHistory();
      } else {
        this.message = 'Usuário não autenticado.';
      }
    });
  }

  // Função para carregar o histórico de compras
  loadPurchaseHistory(): void {
    if (this.userId === null) {
      this.message = 'Usuário não autenticado.';
      return;
    }

    // Fazendo uma chamada HTTP GET para obter o histórico de compras do usuário
    this.http.get<Purchase[]>(`${environment.apiUrl}/user/${this.userId}/orders`)
      .pipe(
        catchError((error: HttpErrorResponse) => this.handleError(error))
      )
      .subscribe({
        next: (data: Purchase[]) => {
          this.purchases = data;  // Armazena os dados recebidos
        },
        error: () => {
          this.message = 'Erro ao carregar o histórico de compras.';  // Mensagem de erro
        }
      });
  }

  // Função para obter o userId do localStorage
  getUserIdFromLocalStorage(): number | null {
    const userId = localStorage.getItem('userId');
    return userId ? Number(userId) : null;
  }

  // Função para tratar erros de requisição
  private handleError(error: HttpErrorResponse): Observable<never> {
    console.error('Erro ao carregar o histórico de compras:', error);
    return throwError(() => new Error('Erro ao tentar carregar o histórico.'));
  }
}
