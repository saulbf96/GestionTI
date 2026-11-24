/** Roles que devuelve la API (/api/session). Deben coincidir con AppRoles en .NET. */
export type AppRole = 'Admin' | 'Technician' | 'ReadOnly';

/** Respuesta de GET /api/session. */
export interface Session {
  userName: string;
  roles: AppRole[];
}

/** Nombre del rol para mostrar en pantalla. */
export const ROLE_LABELS: Record<AppRole, string> = {
  Admin: 'Administrador de TI',
  Technician: 'Técnico de TI',
  ReadOnly: 'Consulta',
};

/** Roles que pueden modificar datos (política Operate en la API). */
export const OPERATOR_ROLES: readonly AppRole[] = ['Admin', 'Technician'];
