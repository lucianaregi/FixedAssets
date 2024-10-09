import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { ProductListComponent } from './product-list.component';
import { ProductService } from '../../services/product.service';
import { Product } from '../../models/product.model';

describe('ProductListComponent', () => {
  let component: ProductListComponent;
  let fixture: ComponentFixture<ProductListComponent>;
  let httpMock: HttpTestingController;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        HttpClientTestingModule, // Importando o módulo de teste HTTP
        ProductListComponent
      ],
      providers: [ProductService]
    })
      .compileComponents();

    fixture = TestBed.createComponent(ProductListComponent);
    component = fixture.componentInstance;
    httpMock = TestBed.inject(HttpTestingController); // Injetando o controlador HTTP de teste
    fixture.detectChanges();
  });

  afterEach(() => {
    httpMock.verify(); // Verificando se todas as requisições HTTP foram atendidas
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should set loading to true initially', () => {
    expect(component.loading).toBeTrue();
  });

  it('should fetch products and update the list', () => {
    const mockProducts: Product[] = [
      { id: 1, name: 'CDB', indexer: 'IPCA', tax: 5, unitPrice: 1000, stock: 100, orderItems: [], userAssets: [] },
      { id: 2, name: 'LCI', indexer: 'CDI', tax: 7, unitPrice: 2000, stock: 50, orderItems: [], userAssets: [] }
    ];

    // Simulando a requisição HTTP e retornando dados mockados
    component.ngOnInit();
    const req = httpMock.expectOne('/api/products');
    expect(req.request.method).toBe('GET');
    req.flush(mockProducts);

    // Validando que os produtos foram carregados e o estado de loading atualizado
    expect(component.products.length).toBe(2);
    expect(component.products).toEqual(mockProducts);
    expect(component.loading).toBeFalse();
  });

  it('should handle HTTP errors gracefully', () => {
    component.ngOnInit();
    const req = httpMock.expectOne('/api/products');

    // Simulando um erro HTTP
    req.flush('Error', { status: 500, statusText: 'Internal Server Error' });

    // Verificando que o estado de loading foi atualizado e a lista de produtos permanece vazia
    expect(component.loading).toBeFalse();
    expect(component.products.length).toBe(0);
  });
});
