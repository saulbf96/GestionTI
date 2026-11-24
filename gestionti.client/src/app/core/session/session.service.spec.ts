import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { SessionService } from './session.service';

describe('SessionService', () => {
  let service: SessionService;
  let http: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({ providers: [provideHttpClient(), provideHttpClientTesting()] });
    service = TestBed.inject(SessionService);
    http = TestBed.inject(HttpTestingController);
  });

  afterEach(() => http.verify());

  it('carga la sesión y deriva cuenta, dominio, iniciales y rol', () => {
    service.load().subscribe();
    http.expectOne('/api/session').flush({ userName: 'UNOCONTACT\\SBASILIO', roles: ['Technician'] });

    expect(service.accountName()).toBe('sbasilio');
    expect(service.domain()).toBe('UNOCONTACT');
    expect(service.initials()).toBe('SB');
    expect(service.roleLabel()).toBe('Técnico de TI');
  });

  it('muestra el rol de mayor nivel cuando tiene varios', () => {
    service.load().subscribe();
    http.expectOne('/api/session').flush({ userName: 'UNOCONTACT\\jperez', roles: ['ReadOnly', 'Admin'] });

    expect(service.roleLabel()).toBe('Administrador de TI');
  });

  it('hasAnyRole respeta los roles de la sesión', () => {
    service.load().subscribe();
    http.expectOne('/api/session').flush({ userName: 'UNOCONTACT\\rsalinas', roles: ['ReadOnly'] });

    expect(service.hasAnyRole(['ReadOnly'])).toBe(true);
    expect(service.hasAnyRole(['Admin', 'Technician'])).toBe(false);
  });
});
