import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DeleteSiteComponent } from './delete-site.component';

describe('DeleteSiteComponent', () => {
  let component: DeleteSiteComponent;
  let fixture: ComponentFixture<DeleteSiteComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DeleteSiteComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DeleteSiteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
