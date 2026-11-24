import { Component, input } from '@angular/core';
import { RouterLink } from '@angular/router';

export interface Breadcrumb {
  label: string;
  link?: string;
}

/** Datos de una pantalla: título, subtítulo, ruta de navegación y módulo al que pertenece. */
export interface PageData {
  title: string;
  subtitle?: string;
  breadcrumbs?: Breadcrumb[];
  module?: string;
}

/**
 * Encabezado de página: ruta (Inicio / …), título, subtítulo y acciones.
 * Las acciones se proyectan: <app-page-header title="…"><button>…</button></app-page-header>
 */
@Component({
  selector: 'app-page-header',
  imports: [RouterLink],
  templateUrl: './page-header.html',
  styleUrl: './page-header.scss',
})
export class PageHeader {
  readonly title = input.required<string>();
  readonly subtitle = input<string>();
  /** Sin valor no se muestra la ruta; [] muestra solo "Inicio / título". */
  readonly breadcrumbs = input<Breadcrumb[]>();
}
