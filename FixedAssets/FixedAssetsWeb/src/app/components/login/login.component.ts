import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { catchError } from 'rxjs/operators';
import { throwError } from 'rxjs';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent {

  email = '';
  password = '';
  errorMessage: string | null = null;

  constructor(private router: Router, private http: HttpClient) { }

  login(): void {
    this.errorMessage = null; 

    const loginData = { email: this.email, password: this.password };

    this.http.post<{ id: number }>(`${environment.apiUrl}/user/login`, loginData)
      .pipe(
        catchError((error: HttpErrorResponse) => {
          this.handleError(error);
          return throwError(() => new Error('Erro na requisição de login.'));
        })
      )
      .subscribe({
        next: (response) => {
          const userId = response?.id;
          if (userId) {
            localStorage.setItem('userId', userId.toString()); 
            this.router.navigate(['/produtos', userId]);

          } else {
            this.errorMessage = 'ID do usuário não encontrado.';
          }
        },
      });

  }

  private handleError(error: HttpErrorResponse): void {
    if (error.status === 401) {
      this.errorMessage = 'E-mail ou senha inválidos.';
    } else {
      this.errorMessage = 'Erro ao tentar fazer login. Tente novamente mais tarde.';
    }
    console.error('Erro de login:', error);
  }
}
