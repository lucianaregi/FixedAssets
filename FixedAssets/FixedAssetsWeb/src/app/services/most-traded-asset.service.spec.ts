import { TestBed } from '@angular/core/testing';

import { MostTradedAssetService } from './most-traded-asset.service';

describe('MostTradedAssetService', () => {
  let service: MostTradedAssetService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(MostTradedAssetService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
