import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CardholdersListComponent } from './cardholders-list.component';

describe('CardholdersListComponent', () => {
  let component: CardholdersListComponent;
  let fixture: ComponentFixture<CardholdersListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CardholdersListComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CardholdersListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
