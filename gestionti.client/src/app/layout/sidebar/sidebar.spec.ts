import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { AppRole } from '../../core/session/session.model';
import { SessionService } from '../../core/session/session.service';
import { Sidebar } from './sidebar';

function renderSidebarAs(roles: AppRole[]): string[] {
  TestBed.configureTestingModule({
    imports: [Sidebar],
    providers: [provideRouter([]), provideHttpClient(), provideHttpClientTesting()],
  });
  TestBed.inject(SessionService).load().subscribe();
  TestBed.inject(HttpTestingController)
    .expectOne('/api/session')
    .flush({ userName: 'UNOCONTACT\\usuario', roles });

  const fixture = TestBed.createComponent(Sidebar);
  fixture.detectChanges();
  const links = fixture.nativeElement.querySelectorAll('nav a span') as NodeListOf<HTMLElement>;
  return Array.from(links, (span) => span.textContent?.trim() ?? '');
}

describe('Sidebar', () => {
  it('el Administrador ve Catálogos y Roles y permisos', () => {
    const labels = renderSidebarAs(['Admin']);

    expect(labels).toContain('Catálogos');
    expect(labels).toContain('Roles y permisos');
  });

  it('Consulta no ve Catálogos ni Roles y permisos', () => {
    const labels = renderSidebarAs(['ReadOnly']);

    expect(labels).toContain('Resumen');
    expect(labels).toContain('Historial');
    expect(labels).not.toContain('Catálogos');
    expect(labels).not.toContain('Roles y permisos');
  });
});
