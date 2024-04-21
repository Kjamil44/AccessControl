import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateCardholderComponent } from './create-cardholder.component';

describe('CreateCardholderComponent', () => {
  let component: CreateCardholderComponent;
  let fixture: ComponentFixture<CreateCardholderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CreateCardholderComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreateCardholderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
