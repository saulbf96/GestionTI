import { Component, computed, inject, output } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { SessionService } from '../../core/session/session.service';
import { NAVIGATION } from '../navigation';

/** Barra lateral oscura con el menú filtrado por rol. */
@Component({
  selector: 'app-sidebar',
  imports: [RouterLink, RouterLinkActive, MatIconModule],
  templateUrl: './sidebar.html',
  styleUrl: './sidebar.scss',
})
export class Sidebar {
  private readonly session = inject(SessionService);

  /** Se emite al elegir una opción (en pantallas chicas cierra el menú). */
  readonly navigated = output<void>();

  protected readonly domain = this.session.domain;

  protected readonly sections = computed(() =>
    NAVIGATION.map((section) => ({
      ...section,
      items: section.items.filter((item) => !item.roles || this.session.hasAnyRole(item.roles)),
    })).filter((section) => section.items.length > 0),
  );
}
