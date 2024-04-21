import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditLockComponent } from './edit-lock.component';

describe('EditLockComponent', () => {
  let component: EditLockComponent;
  let fixture: ComponentFixture<EditLockComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EditLockComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EditLockComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
