import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { environment } from '../../../environments/environment';


@Component({
  selector: 'app-saldo',
  template: `
    <div class="saldo">
      <span>Saldo: </span>
      <span *ngIf="!saldoVisivel">****</span>
      <span *ngIf="saldoVisivel">{{ saldo | currency:'BRL':'symbol':'1.2-2' }}</span>
      <button (click)="toggleSaldoVisivel()">
        <i [class]="saldoVisivel ? 'fas fa-eye-slash' : 'fas fa-eye'"></i>
      </button>
      <span class="usuario">Olá, {{ userName }}!</span> &nbsp;
    </div>
  `,
  styleUrls: ['./saldo.component.css'],
  standalone: true,
  imports: [CommonModule]
})
export class SaldoComponent implements OnInit {
  saldo = 0;
  saldoVisivel = false;
  userName = '';

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    this.getUserInfo(); // Obter informações do usuário e saldo
  }

  // Método para obter o nome e o saldo do usuário
  getUserInfo(): void {
    const userId = localStorage.getItem('userId'); // Obter o userId do localStorage

    if (userId) {
      this.http.get<{ name: string, balance: number }>(`${environment.apiUrl}/user/user/${userId}/balance`)
        .subscribe(
          response => {
            this.saldo = response.balance;
            this.userName = response.name; // Armazena o nome do usuário
          },
          error => console.error('Erro ao buscar informações do usuário:', error)
        );
    } else {
      console.error('Usuário não autenticado.');
    }
  }

  toggleSaldoVisivel(): void {
    this.saldoVisivel = !this.saldoVisivel;
  }
}
