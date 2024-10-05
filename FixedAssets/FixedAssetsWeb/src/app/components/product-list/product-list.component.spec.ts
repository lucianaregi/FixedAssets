import { TestBed, ComponentFixture } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { ProductListComponent } from './product-list.component';
import { ProductService } from '../../services/product.service';
import { Router } from '@angular/router';
import { Location } from '@angular/common';
import { Component } from '@angular/core';

// Componente Dummy para testes de roteamento
@Component({
  template: ''
})
class DummyComponent { }

describe('Routing Test', () => {
  let router: Router;
  let location: Location;
  let fixture: ComponentFixture<ProductListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        RouterTestingModule.withRoutes([
          { path: 'produtos', component: ProductListComponent },
          { path: 'dummy', component: DummyComponent }
        ]),
        HttpClientTestingModule // Importa o HttpClientTestingModule para simular HTTP
      ],
      declarations: [
        ProductListComponent,
        DummyComponent
      ],
      providers: [ProductService] // Adiciona o ProductService como provider
    }).compileComponents();

    // Injetando o Router e o Location
    router = TestBed.inject(Router);
    location = TestBed.inject(Location);
    fixture = TestBed.createComponent(ProductListComponent);
    router.initialNavigation();  // Inicializa a navegação
  });

  it('should navigate to "products" route and print URL', async () => {
    await router.navigate(['/produtos']);
    fixture.detectChanges();
    console.log('Rota atual:', location.path());
    expect(location.path()).toBe('/produtos');
  });
});

describe('ProductListComponent', () => {
  let component: ProductListComponent;
  let fixture: ComponentFixture<ProductListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        RouterTestingModule,  // Simula as rotas
        HttpClientTestingModule  // Substitui HttpClientModule por HttpClientTestingModule
      ],
      declarations: [ProductListComponent],
      providers: [ProductService]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ProductListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

describe('ProductService', () => {
  let service: ProductService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],  // Usa o HttpClientTestingModule
      providers: [ProductService]
    });

    service = TestBed.inject(ProductService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should fetch products', () => {
    const dummyProducts = [
      { id: 1, name: 'Product 1' },
      { id: 2, name: 'Product 2' }
    ];

    service.getProducts().subscribe(products => {
      expect(products.length).toBe(2);
      expect(products).toEqual(dummyProducts);
    });

    // Simula a resposta HTTP esperada
    const req = httpMock.expectOne('http://localhost:5212/api/product');
    expect(req.request.method).toBe('GET');
    req.flush(dummyProducts);
  });
});
