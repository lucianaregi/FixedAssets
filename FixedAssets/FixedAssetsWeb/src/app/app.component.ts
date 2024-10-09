import { Component, OnInit } from '@angular/core';
import { RouterOutlet, RouterLink, Router } from '@angular/router';
import { SaldoComponent } from './components/saldo/saldo.component';
import { CommonModule } from '@angular/common';


@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, RouterLink, SaldoComponent, CommonModule], 
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'FixedAssetsWeb';
  isLoggedIn = false;

  ngOnInit(): void {
    this.checkLoginStatus();
  }

  // Método para verificar o status de login do usuário
  checkLoginStatus(): void {
    const userId = localStorage.getItem('userId'); 
    this.isLoggedIn = !!userId; 
  }
  constructor(private router: Router) { }
  logout(): void {
    // Remover o userId do localStorage
    localStorage.removeItem('userId');

    // Redirecionar para a tela de login
    this.router.navigate(['/login']);
  }
}
