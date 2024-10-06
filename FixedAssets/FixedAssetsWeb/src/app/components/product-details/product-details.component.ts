import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ProductService } from '../../services/product.service';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  standalone: true,  
  imports: [CommonModule],
  styleUrls: ['./product-details.component.css']
})
export class ProductDetailsComponent implements OnInit {

  product: any;
  message: string | undefined;

  constructor(
    private route: ActivatedRoute, 
    private productService: ProductService , 
    private http: HttpClient 
  ) { }

  ngOnInit(): void {
    // Captura o ID da URL
    const productId = this.route.snapshot.paramMap.get('id');

    // Chama o serviço para buscar os detalhes do produto pelo ID
    if (productId) {
      this.productService.getProductById(productId).subscribe(product => {
        this.product = product;
      });
    }
  }

  buyProduct(): void {
    const order = {
      userId: 1,           // ID do usuário
      productId: this.product.id,
      quantity: 1          // Quantidade desejada
    };

    this.http.post('http://localhost:5212/api/user/order', order).subscribe(
      response => {
        this.message = 'Compra realizada com sucesso!';
      },
      error => {
        if (error.status === 400 && error.error === 'Saldo insuficiente.') {
          this.message = 'Saldo insuficiente para realizar a compra.';
        } else {
          this.message = 'Erro ao processar a compra.';
        }
      }
    );
  }



}
