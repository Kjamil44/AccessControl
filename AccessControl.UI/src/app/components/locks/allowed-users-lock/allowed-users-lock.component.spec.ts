import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AllowedUsersLockComponent } from './allowed-users-lock.component';

describe('AllowedUsersLockComponent', () => {
  let component: AllowedUsersLockComponent;
  let fixture: ComponentFixture<AllowedUsersLockComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AllowedUsersLockComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AllowedUsersLockComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
