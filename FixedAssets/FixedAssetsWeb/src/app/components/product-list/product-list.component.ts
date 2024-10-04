import { Component, OnInit } from '@angular/core';
import { ProductService } from '../../services/product.service';

@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.css']
})
export class ProductListComponent implements OnInit {
  products: any[] = [];  // Armazena a lista de produtos

  constructor(private productService: ProductService) { }

  ngOnInit(): void {
    // Busca os produtos quando o componente for carregado
    this.productService.getProducts().subscribe((data) => {
      this.products = data;
    });
  }
}
