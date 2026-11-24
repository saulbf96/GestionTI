import { Component, ElementRef, inject, output, viewChild } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { SessionService } from '../../core/session/session.service';

/** Barra superior: menú, buscador (Ctrl K), notificaciones y usuario. */
@Component({
  selector: 'app-topbar',
  imports: [MatButtonModule, MatIconModule],
  templateUrl: './topbar.html',
  styleUrl: './topbar.scss',
  host: {
    '(document:keydown.control.k)': 'focusSearch($event)',
    '(document:keydown.meta.k)': 'focusSearch($event)',
  },
})
export class Topbar {
  private readonly session = inject(SessionService);
  private readonly searchInput = viewChild.required<ElementRef<HTMLInputElement>>('search');

  readonly menuToggle = output<void>();

  protected readonly accountName = this.session.accountName;
  protected readonly initials = this.session.initials;
  protected readonly roleLabel = this.session.roleLabel;

  protected focusSearch(event: Event): void {
    event.preventDefault();
    this.searchInput().nativeElement.focus();
  }
}
