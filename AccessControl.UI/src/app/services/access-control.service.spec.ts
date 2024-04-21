import { TestBed } from '@angular/core/testing';
import { AccessControlService } from './access-control.service';

describe('SitesService', () => {
  let service: AccessControlService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AccessControlService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
