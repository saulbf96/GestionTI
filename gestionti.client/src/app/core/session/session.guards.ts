import { HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { catchError, map, of } from 'rxjs';
import { AppRole } from './session.model';
import { SessionService } from './session.service';

/**
 * Carga la sesión antes de mostrar la aplicación.
 * Si la API responde 401/403 (sin cuenta válida o sin grupo de la aplicación),
 * manda a la pantalla "Sin acceso".
 */
export const sessionGuard: CanActivateFn = () => {
  const session = inject(SessionService);
  const router = inject(Router);

  if (session.session()) {
    return true;
  }

  return session.load().pipe(
    map(() => true),
    catchError((error: HttpErrorResponse) =>
      of(router.createUrlTree(['/sin-acceso'], { queryParams: { motivo: error.status || 'red' } })),
    ),
  );
};

/**
 * Oculta pantallas según el rol (solo interfaz; la API vuelve a validar).
 * Si el usuario no tiene el rol, regresa al Resumen.
 */
export function roleGuard(...roles: AppRole[]): CanActivateFn {
  return () => {
    const session = inject(SessionService);
    return session.hasAnyRole(roles) ? true : inject(Router).createUrlTree(['/']);
  };
}
