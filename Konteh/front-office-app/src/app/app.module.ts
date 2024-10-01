import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { ReactiveFormsModule } from '@angular/forms';
import { HTTP_INTERCEPTORS, HttpClient, HttpClientModule } from '@angular/common/http';
import { MatError, MatFormField, MatLabel } from '@angular/material/form-field';
import { CandidateInfoFormComponent } from './start-info/candidate-info-form.component';
import { MatInput } from '@angular/material/input';
import { TakingTestComponent } from './taking-test/taking-test.component';
import { FormsModule } from '@angular/forms';
import { MatRadioModule } from '@angular/material/radio';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { SubmitDialogComponent } from './submit-dialog/submit-dialog.component';
import {MatDialogModule} from '@angular/material/dialog';
import { HttpErrorInterceptor } from './services/http-error-interceptor.service';
import { NotFoundComponent } from './not-found/not-found.component';
import { MatIconModule } from '@angular/material/icon';
import { CompletedTestComponent } from './completed-test/completed-test.component';

@NgModule({
  declarations: [
    AppComponent,
    CandidateInfoFormComponent,
    TakingTestComponent,
    SubmitDialogComponent,
    NotFoundComponent,
    CompletedTestComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    ReactiveFormsModule,
    HttpClientModule,
    MatFormField,
    MatLabel,
    MatInput,
    MatError,
    FormsModule,
    MatRadioModule,
    MatCheckboxModule,
    MatDialogModule,
    MatIconModule
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: HttpErrorInterceptor,
      multi: true
    },
    provideAnimationsAsync()
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
