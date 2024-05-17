import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SiteSelectorComponent } from './site-selector.component';

describe('SiteSelectorComponent', () => {
  let component: SiteSelectorComponent;
  let fixture: ComponentFixture<SiteSelectorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SiteSelectorComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(SiteSelectorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
