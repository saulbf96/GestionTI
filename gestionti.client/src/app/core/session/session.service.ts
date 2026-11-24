import { HttpClient } from '@angular/common/http';
import { computed, inject, Injectable, signal } from '@angular/core';
import { Observable, tap } from 'rxjs';
import { AppRole, ROLE_LABELS, Session } from './session.model';

/**
 * Sesión del usuario actual (cuenta de Windows y roles).
 * Solo sirve para mostrar u ocultar menús y botones: los permisos reales
 * se validan en la API en cada petición.
 */
@Injectable({ providedIn: 'root' })
export class SessionService {
  private readonly http = inject(HttpClient);
  private readonly current = signal<Session | null>(null);

  readonly session = this.current.asReadonly();
  readonly roles = computed<readonly AppRole[]>(() => this.current()?.roles ?? []);

  /** "UNOCONTACT\SBASILIO" → "UNOCONTACT". */
  readonly domain = computed(() => {
    const userName = this.current()?.userName ?? '';
    return userName.includes('\\') ? userName.split('\\')[0].toUpperCase() : '';
  });

  /** "UNOCONTACT\SBASILIO" → "sbasilio". */
  readonly accountName = computed(() => {
    const userName = this.current()?.userName ?? '';
    return (userName.split('\\').pop() ?? '').toLowerCase();
  });

  readonly initials = computed(() => this.accountName().slice(0, 2).toUpperCase());

  /** Rol principal para mostrar (Admin > Técnico > Consulta). */
  readonly roleLabel = computed(() => {
    const roles = this.roles();
    const main = (['Admin', 'Technician', 'ReadOnly'] as const).find((r) => roles.includes(r));
    return main ? ROLE_LABELS[main] : '';
  });

  load(): Observable<Session> {
    return this.http.get<Session>('/api/session').pipe(tap((session) => this.current.set(session)));
  }

  hasAnyRole(roles: readonly AppRole[]): boolean {
    return roles.some((role) => this.roles().includes(role));
  }
}
