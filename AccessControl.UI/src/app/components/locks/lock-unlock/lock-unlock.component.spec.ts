import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LockUnlockComponent } from './lock-unlock.component';

describe('LockUnlockComponent', () => {
  let component: LockUnlockComponent;
  let fixture: ComponentFixture<LockUnlockComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LockUnlockComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(LockUnlockComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
