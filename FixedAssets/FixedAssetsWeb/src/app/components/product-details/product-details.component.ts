import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.css']
})
export class ProductDetailsComponent implements OnInit {

  product = {
    id: 1,
    name: 'Product 1',
    description: 'Description of product 1',
    price: 100
  };

  constructor() { }

  ngOnInit(): void {
    // Lógica adicional pode ser inserida aqui
  }

}
