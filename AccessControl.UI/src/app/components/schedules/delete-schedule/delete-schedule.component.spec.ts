import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DeleteScheduleComponent } from './delete-schedule.component';

describe('DeleteScheduleComponent', () => {
  let component: DeleteScheduleComponent;
  let fixture: ComponentFixture<DeleteScheduleComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DeleteScheduleComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DeleteScheduleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
