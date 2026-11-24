import { registerLocaleData } from '@angular/common';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import localeEsMx from '@angular/common/locales/es-MX';
import { LOCALE_ID, NgModule, provideBrowserGlobalErrorListeners } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing-module';
import { App } from './app';

// Fechas, números y moneda con formato de México (24/11/2026, 1,850).
registerLocaleData(localeEsMx);

@NgModule({
  declarations: [App],
  imports: [BrowserModule, AppRoutingModule],
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideHttpClient(withInterceptorsFromDi()),
    { provide: LOCALE_ID, useValue: 'es-MX' },
  ],
  bootstrap: [App],
})
export class AppModule {}
