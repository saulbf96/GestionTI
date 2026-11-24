import { BreakpointObserver } from '@angular/cdk/layout';
import { Component, inject, viewChild } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { MatSidenav, MatSidenavModule } from '@angular/material/sidenav';
import { RouterOutlet } from '@angular/router';
import { map } from 'rxjs';
import { Sidebar } from '../sidebar/sidebar';
import { Topbar } from '../topbar/topbar';

/** Estructura de la aplicación: barra lateral, barra superior y contenido. */
@Component({
  selector: 'app-shell',
  imports: [MatSidenavModule, RouterOutlet, Sidebar, Topbar],
  template: `
    <mat-sidenav-container class="container">
      <mat-sidenav
        class="sidenav"
        [mode]="isCompact() ? 'over' : 'side'"
        [opened]="!isCompact()"
        [fixedInViewport]="true"
      >
        <app-sidebar (navigated)="isCompact() && sidenav().close()" />
      </mat-sidenav>
      <mat-sidenav-content>
        <app-topbar (menuToggle)="sidenav().toggle()" />
        <main class="content">
          <router-outlet />
        </main>
      </mat-sidenav-content>
    </mat-sidenav-container>
  `,
  styles: `
    .container {
      height: 100vh;
    }
    .sidenav {
      width: var(--gti-sidebar-width);
      border: 0;
    }
    .content {
      max-width: 1440px;
      margin: 0 auto;
      padding: 32px;
    }
    @media (width < 720px) {
      .content {
        padding: 20px 16px;
      }
    }
  `,
})
export class Shell {
  protected readonly sidenav = viewChild.required(MatSidenav);

  /** Menos de 1024 px: el menú se abre encima del contenido. */
  protected readonly isCompact = toSignal(
    inject(BreakpointObserver)
      .observe('(max-width: 1023.98px)')
      .pipe(map((state) => state.matches)),
    { initialValue: false },
  );
}
