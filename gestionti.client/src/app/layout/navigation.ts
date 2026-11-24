import { AppRole } from '../core/session/session.model';

export interface NavItem {
  label: string;
  /** Nombre del ícono de Material Symbols. */
  icon: string;
  route: string;
  /** Si se indica, solo esos roles ven la opción. */
  roles?: readonly AppRole[];
  /** Marca activa solo con la ruta exacta (para "/"). */
  exact?: boolean;
}

export interface NavSection {
  title: string;
  items: readonly NavItem[];
}

/** Menú lateral, igual al de las pantallas de diseño. */
export const NAVIGATION: readonly NavSection[] = [
  {
    title: 'Operación',
    items: [
      { label: 'Resumen', icon: 'space_dashboard', route: '/', exact: true },
      { label: 'Colaboradores', icon: 'group', route: '/usuarios' },
      { label: 'Inventario', icon: 'laptop_windows', route: '/equipos' },
      { label: 'Asignaciones', icon: 'swap_horiz', route: '/asignaciones' },
      { label: 'Responsivas', icon: 'description', route: '/responsivas' },
      { label: 'Devoluciones', icon: 'undo', route: '/devoluciones' },
      { label: 'Bajas', icon: 'logout', route: '/bajas' },
    ],
  },
  {
    title: 'Control',
    items: [
      { label: 'Historial', icon: 'history', route: '/historial' },
      { label: 'Catálogos', icon: 'list', route: '/catalogos', roles: ['Admin'] },
      { label: 'Roles y permisos', icon: 'shield', route: '/roles', roles: ['Admin'] },
    ],
  },
];
