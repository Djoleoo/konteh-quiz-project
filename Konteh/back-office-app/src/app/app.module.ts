import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { MatToolbarModule } from '@angular/material/toolbar';

import {
  MsalModule,
  MsalRedirectComponent,
  MsalInterceptor,
} from "@azure/msal-angular";

import {
  InteractionType,
  PublicClientApplication,
} from "@azure/msal-browser";

import { NavbarComponent } from './navbar/navbar.component';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { HomeComponent } from './home/home.component';
import { environment } from '../environments/environment';
import { MAT_DATE_FORMATS } from '@angular/material/core';
import { HttpErrorInterceptor } from './services/http-error-interceptor.service';

@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    HomeComponent,
  ],
  imports: [
    HttpClientModule,
    AppRoutingModule,
    BrowserModule,
    AppRoutingModule,
    MatToolbarModule,
    MsalModule.forRoot(
      new PublicClientApplication({
        auth: {
          clientId: environment.msalConfig.clientId,
          authority: environment.msalConfig.authority,
          redirectUri: environment.msalConfig.redirectUri,
        },
        cache: {
          cacheLocation: "localStorage",
          storeAuthStateInCookie: false,
        },
      }),
      null!,
      {
        interactionType: InteractionType.Popup,
        protectedResourceMap: new Map([

          ["https://localhost:7221/", ['api://dbf7f51e-d046-435b-88ee-c4f9ee872967/to-do-lists.read',
            'api://dbf7f51e-d046-435b-88ee-c4f9ee872967/to-do-lists.write']],
        ]),
      }
    ),
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: MsalInterceptor,
      multi: true,
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: HttpErrorInterceptor,
      multi: true
    },
    {
      provide: MAT_DATE_FORMATS,
      useValue: {
        parse: {
          dateInput: 'LL',
        },
        display: {
          dateInput: 'dd-MM-yyyy HH:mm:ss',
          monthYearLabel: 'MMM YYYY',
          dateA11yLabel: 'LL',
          monthYearA11yLabel: 'MMMM YYYY',
        }
      }
    },
    provideAnimationsAsync()
  ],
  bootstrap: [AppComponent, MsalRedirectComponent]
})
export class AppModule { }
