import { Component, computed, input } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

/**
 * Se muestra cuando la API no reconoce al usuario (401), cuando su cuenta no
 * pertenece a ningún grupo de la aplicación (403) o cuando la API no responde.
 */
@Component({
  selector: 'app-no-access-page',
  imports: [MatButtonModule, MatIconModule],
  template: `
    <main class="wrapper">
      <section class="card">
        <mat-icon aria-hidden="true">lock</mat-icon>
        <h1>{{ message().title }}</h1>
        <p>{{ message().detail }}</p>
        <a mat-flat-button href="/">Intentar de nuevo</a>
      </section>
    </main>
  `,
  styles: `
    .wrapper {
      display: grid;
      place-items: center;
      min-height: 100vh;
      padding: 24px;
    }
    .card {
      max-width: 440px;
      padding: 40px;
      text-align: center;
      background: var(--gti-surface);
      border-radius: var(--gti-radius-card);
      box-shadow: var(--gti-shadow-raised);
    }
    mat-icon {
      color: var(--gti-primary);
      font-size: 40px;
      width: 40px;
      height: 40px;
    }
    h1 {
      margin: 16px 0 8px;
      font-size: 22px;
    }
    p {
      margin: 0 0 24px;
      color: var(--gti-text-2);
    }
  `,
})
export class NoAccessPage {
  /** Viene de ?motivo= (código HTTP o "red"). */
  readonly motivo = input<string>();

  protected readonly message = computed(() => {
    switch (this.motivo()) {
      case '403':
        return {
          title: 'No tienes acceso a Gestión de TI',
          detail:
            'Tu cuenta de red no pertenece a ningún grupo de la aplicación. Solicita acceso al área de TI.',
        };
      case '401':
        return {
          title: 'No se pudo validar tu cuenta',
          detail: 'Abre la aplicación desde un equipo del dominio con tu sesión de Windows iniciada.',
        };
      default:
        return {
          title: 'No hay conexión con el servidor',
          detail: 'Revisa tu conexión a la red. Si continúa, comunícate con el área de TI.',
        };
    }
  });
}
