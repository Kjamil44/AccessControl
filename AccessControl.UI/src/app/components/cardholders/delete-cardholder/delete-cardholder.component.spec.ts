import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DeleteCardholderComponent } from './delete-cardholder.component';

describe('DeleteCardholderComponent', () => {
  let component: DeleteCardholderComponent;
  let fixture: ComponentFixture<DeleteCardholderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DeleteCardholderComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DeleteCardholderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
