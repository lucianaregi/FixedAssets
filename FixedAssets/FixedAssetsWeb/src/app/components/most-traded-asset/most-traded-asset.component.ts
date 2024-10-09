import { Component, OnInit } from '@angular/core';
import { MostTradedAssetService } from '../../services/most-traded-asset.service'; 
import { MostTradedAsset } from '../../models/most-traded-asset.model';
import { ChartData } from '../../models/chart-data.model';
import { HttpClientModule } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
//import { throwError } from 'rxjs';
import { DecimalPipe } from '@angular/common';
import { Color, NgxChartsModule, ScaleType } from '@swimlane/ngx-charts';

@Component({
  selector: 'app-most-traded-asset',
  standalone: true,
  imports: [CommonModule, HttpClientModule, FormsModule, NgxChartsModule],
  providers: [DecimalPipe],
  templateUrl: './most-traded-asset.component.html',
  styleUrls: ['./most-traded-asset.component.css'],
})
export class MostTradedAssetComponent implements OnInit {
  mostTradedAssets: MostTradedAsset[] = [];
  chartData: ChartData[] = [];
  errorMessage: string | null = null;
   
  view: [number, number] = [700, 400];

  showXAxis = true;
  showYAxis = true;
  gradient = false;
  showLegend = true;
  showXAxisLabel = true;
  xAxisLabel = 'Ativos';
  showYAxisLabel = true;
  yAxisLabel = 'Volume Negociado';
  colorScheme: Color = {
    name: 'myScheme',
    selectable: true,
    group: ScaleType.Ordinal,
    domain: ['#5AA454', '#A10A28', '#C7B42C', '#AAAAAA']
  };

  constructor(private mostTradedAssetService: MostTradedAssetService, private decimalPipe: DecimalPipe) { }

  ngOnInit(): void {
    this.loadMostTradedAssets();

  }

  // Função para carregar os ativos mais negociados
  //loadMostTradedAssets(): void {
  //  this.mostTradedAssetService.getMostTradedAssets().subscribe({
  //    next: (data) => {
  //      this.mostTradedAssets = data;
  //    },
  //    error: (error) => {
  //      console.error('Erro ao carregar os ativos mais negociados:', error);
  //      this.errorMessage = 'Erro ao carregar os ativos mais negociados.';
  //    },
  //  });
  //}

  loadMostTradedAssets(): void {
    this.mostTradedAssetService.getMostTradedAssets().subscribe({
      next: (data: MostTradedAsset[]) => {
        this.mostTradedAssets = data;
        this.chartData = data.map(asset => ({
          name: asset.name,
          value: asset.totalTrades
        }));
      },
      error: (error) => {
        console.error('Erro ao carregar os ativos mais negociados:', error);
        this.errorMessage = 'Erro ao carregar os ativos mais negociados.';
      },
    });
  }


  onSelect(event: { name: string; value: number; series: string }): void {
    console.log(`Item selecionado: Nome = ${event.name}, Valor = ${event.value}`);
  }

  // Função que formata o valor quando o usuário digita
  onValueChange(value: string, asset: MostTradedAsset): void {
    const formattedValue = value.replace(',', '.');  // Substitui a vírgula por ponto
    const parsedValue = parseFloat(formattedValue);  // Converte para número

    // Verifica se o valor é um número válido antes de atribuir
    if (!isNaN(parsedValue)) {
      asset.currentValue = parsedValue;
    }
  }


  // Função para atualizar um ativo negociado
  updateAsset(asset: MostTradedAsset): void {
    this.mostTradedAssetService.updateMostTradedAsset(asset).subscribe({
      next: () => {
        console.log('Ativo atualizado com sucesso.');
      },
      error: (error) => {
        console.error('Erro ao atualizar o ativo:', error);
        this.errorMessage = 'Erro ao atualizar o ativo.';
      },
    });
  }
}
