import { Routes } from '@angular/router';
import { roleGuard, sessionGuard } from './core/session/session.guards';
import { OPERATOR_ROLES } from './core/session/session.model';
import { Shell } from './layout/shell/shell';
import { PageData } from './shared/page-header/page-header';

const pending = () => import('./features/pending/pending-page').then((m) => m.PendingPage);

/** Datos que lee la pantalla provisional (título, subtítulo y ruta de navegación). */
function page(data: PageData): { data: PageData; title: string } {
  return { data, title: data.title };
}

export const routes: Routes = [
  {
    path: 'sin-acceso',
    title: 'Sin acceso',
    loadComponent: () => import('./features/no-access/no-access-page').then((m) => m.NoAccessPage),
  },
  {
    path: '',
    component: Shell,
    canActivate: [sessionGuard],
    children: [
      {
        path: '',
        pathMatch: 'full',
        loadComponent: pending,
        ...page({ title: 'Resumen', subtitle: 'Estado del inventario y de las asignaciones.', module: 'Dashboard' }),
      },

      // Colaboradores
      {
        path: 'usuarios',
        loadComponent: pending,
        ...page({ title: 'Colaboradores', subtitle: 'Registro y control de colaboradores.', module: 'Usuarios / Altas' }),
      },
      {
        path: 'usuarios/nuevo',
        loadComponent: pending,
        canActivate: [roleGuard(...OPERATOR_ROLES)],
        ...page({
          title: 'Alta de colaborador',
          breadcrumbs: [{ label: 'Colaboradores', link: '/usuarios' }],
          module: 'Usuarios / Altas',
        }),
      },
      {
        path: 'usuarios/:id',
        loadComponent: pending,
        ...page({
          title: 'Ficha del colaborador',
          breadcrumbs: [{ label: 'Colaboradores', link: '/usuarios' }],
          module: 'Usuarios / Altas',
        }),
      },

      // Inventario
      {
        path: 'equipos',
        loadComponent: pending,
        ...page({ title: 'Inventario de equipos', breadcrumbs: [], module: 'Inventario de equipos' }),
      },
      {
        path: 'equipos/:kc',
        loadComponent: pending,
        ...page({
          title: 'Detalle de equipo',
          breadcrumbs: [{ label: 'Inventario', link: '/equipos' }],
          module: 'Inventario de equipos',
        }),
      },

      // Asignaciones, responsivas y devoluciones
      {
        path: 'asignaciones',
        loadComponent: pending,
        ...page({ title: 'Asignaciones', module: 'Asignaciones' }),
      },
      {
        path: 'asignaciones/nueva',
        loadComponent: pending,
        canActivate: [roleGuard(...OPERATOR_ROLES)],
        ...page({
          title: 'Asignar equipo',
          breadcrumbs: [{ label: 'Asignaciones', link: '/asignaciones' }],
          module: 'Asignaciones',
        }),
      },
      {
        path: 'responsivas',
        loadComponent: pending,
        ...page({ title: 'Responsivas', module: 'Responsivas' }),
      },
      {
        path: 'devoluciones',
        loadComponent: pending,
        ...page({ title: 'Devoluciones', module: 'Devoluciones' }),
      },
      {
        path: 'devoluciones/nueva',
        loadComponent: pending,
        canActivate: [roleGuard(...OPERATOR_ROLES)],
        ...page({
          title: 'Devolución de equipo',
          breadcrumbs: [{ label: 'Devoluciones', link: '/devoluciones' }],
          module: 'Devoluciones',
        }),
      },

      // Bajas
      {
        path: 'bajas',
        loadComponent: pending,
        ...page({ title: 'Bajas', module: 'Bajas' }),
      },
      {
        path: 'bajas/:folio',
        loadComponent: pending,
        ...page({ title: 'Baja de colaborador', breadcrumbs: [{ label: 'Bajas', link: '/bajas' }], module: 'Bajas' }),
      },

      // Control
      {
        path: 'historial',
        loadComponent: pending,
        ...page({
          title: 'Historial de movimientos',
          subtitle: 'Bitácora de solo lectura. Los registros no se pueden editar ni eliminar.',
          module: 'Historial / Trazabilidad',
        }),
      },
      {
        path: 'catalogos',
        loadComponent: pending,
        canActivate: [roleGuard('Admin')],
        ...page({ title: 'Catálogos', module: 'Campañas / Áreas y catálogos' }),
      },
      {
        path: 'roles',
        loadComponent: pending,
        canActivate: [roleGuard('Admin')],
        ...page({
          title: 'Roles y permisos',
          subtitle: 'El rol de cada persona se toma de su grupo en Active Directory. Los permisos se validan en la API.',
          module: 'Administración',
        }),
      },
    ],
  },
  { path: '**', redirectTo: '' },
];
