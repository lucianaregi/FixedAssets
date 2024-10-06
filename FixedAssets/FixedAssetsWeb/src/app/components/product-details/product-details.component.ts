import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ProductService } from '../../services/product.service';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { Product } from '../../models/product.model'; 

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  standalone: true,
  imports: [CommonModule],
  styleUrls: ['./product-details.component.css']
})
export class ProductDetailsComponent implements OnInit {

  product: Product | null = null;  
  message: string | undefined;

  constructor(
    private route: ActivatedRoute,
    private productService: ProductService,
    private http: HttpClient
  ) { }

  ngOnInit(): void {
    // Captura o ID da URL
    const productId = Number(this.route.snapshot.paramMap.get('id'));

    // Verifica se o ID do produto é válido antes de chamar o serviço
    if (productId) {
      this.productService.getProductById(productId).subscribe(
        (product: Product) => {
          this.product = product;
        },
        error => {
          console.error('Erro ao carregar detalhes do produto:', error);
          this.message = 'Erro ao carregar detalhes do produto.';
        }
      );
    } else {
      this.message = 'Produto não encontrado.';
    }
  }

  buyProduct(): void {
    if (!this.product) {
      this.message = 'Produto não encontrado.';
      return;
    }

    const order = {
      userId: 1,           // ID do usuário fixo por enquanto
      productId: this.product.id,
      quantity: 1          // Quantidade desejada
    };

    this.http.post('http://localhost:5212/api/user/order', order).subscribe(
      () => {
        this.message = 'Compra realizada com sucesso!';
      },
      error => {
        if (error.status === 400 && error.error === 'Saldo insuficiente.') {
          this.message = 'Saldo insuficiente para realizar a compra.';
        } else {
          console.error('Erro ao processar a compra:', error);
          this.message = 'Erro ao processar a compra.';
        }
      }
    );
  }
}
