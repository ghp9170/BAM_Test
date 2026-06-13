import { TestBed } from '@angular/core/testing';

import { AstronautListService } from './astronaut-list-service';

describe('AstronautListService', () => {
  let service: AstronautListService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AstronautListService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
