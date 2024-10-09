import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { MostTradedAsset } from '../models/most-traded-asset.model';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class MostTradedAssetService {
  private apiUrl = `${environment.apiUrl}/mosttradedassets`;

  constructor(private http: HttpClient) { }

  // Função para buscar os 5 ativos mais negociados
  getMostTradedAssets(): Observable<MostTradedAsset[]> {
    return this.http.get<MostTradedAsset[]>(this.apiUrl).pipe(
      catchError(this.handleError)
    );
  }

  // Função para atualizar o valor de um ativo
  updateMostTradedAsset(asset: MostTradedAsset): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}`, asset).pipe(
      catchError(this.handleError)
    );
  }

  // Função para lidar com erros HTTP
  private handleError(error: HttpErrorResponse): Observable<never> {
    console.error('Ocorreu um erro:', error);
    return throwError(() => new Error('Erro na comunicação com a API.'));
  }
}
