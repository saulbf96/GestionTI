import { Component, inject } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { MatIconModule } from '@angular/material/icon';
import { ActivatedRoute } from '@angular/router';
import { map } from 'rxjs';
import { PageData, PageHeader } from '../../shared/page-header/page-header';

/**
 * Pantalla provisional: muestra el encabezado real de la ruta y avisa
 * en qué módulo se construye. Se reemplaza por la pantalla definitiva.
 */
@Component({
  selector: 'app-pending-page',
  imports: [PageHeader, MatIconModule],
  template: `
    @let page = data();
    <app-page-header [title]="page.title" [subtitle]="page.subtitle" [breadcrumbs]="page.breadcrumbs" />
    <section class="card">
      <mat-icon aria-hidden="true">construction</mat-icon>
      <div>
        <h2>Pantalla en construcción</h2>
        <p>
          Se construye con el módulo <strong>{{ page.module }}</strong>. El diseño de referencia está en las
          pantallas del prototipo.
        </p>
      </div>
    </section>
  `,
  styles: `
    .card {
      display: flex;
      gap: 16px;
      align-items: flex-start;
      padding: 24px;
      background: var(--gti-surface);
      border-radius: var(--gti-radius-card);
      box-shadow: var(--gti-shadow-card);
    }
    mat-icon {
      color: var(--gti-accent);
    }
    h2 {
      margin: 0 0 4px;
      font-size: 16px;
      font-weight: 600;
    }
    p {
      margin: 0;
      color: var(--gti-text-2);
    }
  `,
})
export class PendingPage {
  protected readonly data = toSignal(inject(ActivatedRoute).data.pipe(map((d) => d as PageData)), {
    requireSync: true,
  });
}
