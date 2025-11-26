import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CardUnlockAnimationComponent } from './card-unlock-animation.component';

describe('CardUnlockAnimationComponent', () => {
  let component: CardUnlockAnimationComponent;
  let fixture: ComponentFixture<CardUnlockAnimationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CardUnlockAnimationComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(CardUnlockAnimationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
