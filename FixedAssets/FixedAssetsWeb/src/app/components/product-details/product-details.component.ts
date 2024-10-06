import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ProductService } from '../../services/product.service';
//import { CurrencyPipe } from '@angular/common';
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

  constructor(
    private route: ActivatedRoute, // Para acessar os parâmetros da rota
    private productService: ProductService  // Para buscar os detalhes do produto
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
}
