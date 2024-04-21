import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DeleteLockComponent } from './delete-lock.component';

describe('DeleteLockComponent', () => {
  let component: DeleteLockComponent;
  let fixture: ComponentFixture<DeleteLockComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DeleteLockComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DeleteLockComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
