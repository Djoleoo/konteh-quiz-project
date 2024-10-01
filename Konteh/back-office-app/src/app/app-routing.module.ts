import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BrowserUtils } from "@azure/msal-browser";
import { HomeComponent } from './home/home.component';
import { authGuard } from './auth.guard';
import { NotFoundComponent } from './not-found/not-found.component';

const routes: Routes = [
  {
    path:'',
    component: HomeComponent
  },
  { 
    path: 'questions', 
    canActivate: [authGuard],
    loadChildren: () => import('./features/questions/questions.module').then(m=>m.QuestionsModule)
  },
  {
    path: 'exams',
    canActivate: [authGuard],
    loadChildren: () => import('./features/exams/exams.module').then(m=> m.ExamsModule)
  },
  {
    path: 'not-found',
    component: NotFoundComponent
  },
  {
    path: '**',
    redirectTo: 'not-found',
  },
];

const isIframe = window !== window.parent && !window.opener;


@NgModule({
  imports: [
    RouterModule.forRoot(routes, {
      initialNavigation:
        !BrowserUtils.isInIframe() && !BrowserUtils.isInPopup()
          ? "enabledNonBlocking"
          : "disabled", 
    }),
  ],
  exports: [RouterModule],
})
export class AppRoutingModule {}
