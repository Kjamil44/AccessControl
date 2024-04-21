import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditCardholderComponent } from './edit-cardholder.component';

describe('EditCardholderComponent', () => {
  let component: EditCardholderComponent;
  let fixture: ComponentFixture<EditCardholderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EditCardholderComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EditCardholderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
