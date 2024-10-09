import { Component, OnInit, OnDestroy, TemplateRef } from '@angular/core';
import { ActivatedRoute } from '@angular/router';  
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { HttpClientModule, HttpClient, HttpErrorResponse } from '@angular/common/http';
import { ProductService } from '../../services/product.service';
import { Product } from '../../models/product.model';
import { Subscription } from 'rxjs';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { environment } from '../../../environments/environment';
import { FormsModule } from '@angular/forms';
import { OrderProcessingResult } from '../../models/order-processing-result.model';
import { SaldoComponent } from '../../components/saldo/saldo.component';

@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [CommonModule, RouterLink, HttpClientModule, FormsModule, SaldoComponent],
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.css']
})
export class ProductListComponent implements OnInit, OnDestroy {
  products: Product[] = [];
  selectedProduct: Product | null = null;
  quantity = 1;
  message: string | undefined;
  loading = true;
  error: string | null = null;
  private subscription: Subscription = new Subscription();
  private modalRef: NgbModalRef | null = null;
  isSuccess: boolean | null = null;
  userId: number | null = null;  

  constructor(
    private productService: ProductService,
    private http: HttpClient,
    private modalService: NgbModal,
    private route: ActivatedRoute  
  ) { }

  ngOnInit(): void {
    // Obter o userId da rota
    this.route.paramMap.subscribe(params => {
      const id = params.get('id');
      this.userId = id ? +id : null;
    });

    this.loadProducts();
  }

  loadProducts(): void {
    this.loading = true;
    const productSub = this.productService.getProducts().subscribe({
      next: (data: Product[]) => {
        this.products = data;
        this.loading = false;
      },
      error: (err: Error) => {
        this.error = 'Falha ao carregar os produtos. Tente novamente mais tarde.';
        this.loading = false;
        console.error('Erro ao carregar produtos:', err);
      }
    });

    this.subscription.add(productSub);
  }

  openPurchaseModal(product: Product, content: TemplateRef<Element>): void {
    this.selectedProduct = product;
    this.quantity = 1;
    this.message = undefined;
    this.isSuccess = null;
    this.modalRef = this.modalService.open(content);

    this.modalRef.result.finally(() => {
      this.message = undefined;
      this.isSuccess = null;
    });
  }

  async purchaseProduct(): Promise<void> {
    if (!this.selectedProduct || !this.userId) {
      this.message = 'Usuário ou produto não selecionado.';
      this.isSuccess = false;
      return;
    }

    const order = {
      userId: this.userId,  // Utiliza o userId capturado da rota
      orderItems: [
        {
          productId: this.selectedProduct.id,
          productName: this.selectedProduct.name,
          quantity: this.quantity,
          unitPrice: this.selectedProduct.unitPrice
        }
      ]
    };

    try {
      const result = await this.http.post<OrderProcessingResult>(
        `${environment.apiUrl}/order/order`,
        order
      ).toPromise();

      if (result && result.success) {
        this.message = result.message || 'Compra realizada com sucesso!';
        this.isSuccess = true;

        setTimeout(() => {
          if (this.modalRef) {
            this.modalRef.close();
          }
        }, 3000);
      } else {
        this.message = result?.message || 'Erro ao processar a compra.';
        this.isSuccess = false;
      }
    } catch (error) {
      if (error instanceof HttpErrorResponse) {
        this.message = error.error.message || 'Erro ao processar a compra.';
        this.isSuccess = false;
        console.error('Erro ao processar a compra:', error.error);
      } else {
        this.message = 'Erro desconhecido ao processar a compra.';
        this.isSuccess = false;
        console.error('Erro desconhecido ao processar a compra:', error);
      }
    }
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }
}
