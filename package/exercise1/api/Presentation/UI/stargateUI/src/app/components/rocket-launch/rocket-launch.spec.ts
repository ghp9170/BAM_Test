import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RocketLaunch } from './rocket-launch';

describe('RocketLaunch', () => {
  let component: RocketLaunch;
  let fixture: ComponentFixture<RocketLaunch>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RocketLaunch],
    }).compileComponents();

    fixture = TestBed.createComponent(RocketLaunch);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
