import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AstronautList } from './astronaut-list';

describe('AstronautList', () => {
  let component: AstronautList;
  let fixture: ComponentFixture<AstronautList>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AstronautList],
    }).compileComponents();

    fixture = TestBed.createComponent(AstronautList);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
