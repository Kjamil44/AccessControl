import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LocksListComponent } from './locks-list.component';

describe('LocksListComponent', () => {
  let component: LocksListComponent;
  let fixture: ComponentFixture<LocksListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LocksListComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LocksListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
