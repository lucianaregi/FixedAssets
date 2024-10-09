import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MostTradedAssetComponent } from './most-traded-asset.component';

describe('MostTradedAssetComponent', () => {
  let component: MostTradedAssetComponent;
  let fixture: ComponentFixture<MostTradedAssetComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MostTradedAssetComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MostTradedAssetComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
