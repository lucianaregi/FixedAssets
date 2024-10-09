import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ProductService } from '../../services/product.service';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { Product } from '../../models/product.model';
import { environment } from '../../../environments/environment';  // Importa o ambiente

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

  async buyProduct(): Promise<void> {
    if (!this.product) {
      this.message = 'Produto não encontrado.';
      return;
    }

    const order = {
      userId: 3, 
      productId: this.product.id,
      quantity: 1 
    };

    try {
      // Fazer a requisição POST para o backend
      const response = await this.http.post<string>(`${environment.apiUrl}/user/order`, order).toPromise();

      // Exibe a mensagem recebida do backend (sucesso ou erro)
      this.message = response;
    } catch (error) {
      console.error('Erro ao processar a compra:', error);
      this.message = 'Erro ao processar a compra. Por favor, tente novamente.';
    }
  }

}
