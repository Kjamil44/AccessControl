import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateLockComponent } from './create-lock.component';

describe('CreateLockComponent', () => {
  let component: CreateLockComponent;
  let fixture: ComponentFixture<CreateLockComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CreateLockComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreateLockComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
