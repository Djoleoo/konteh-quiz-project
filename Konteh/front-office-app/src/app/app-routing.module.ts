import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CandidateInfoFormComponent } from './start-info/candidate-info-form.component';
import { TakingTestComponent } from './taking-test/taking-test.component'
import { NotFoundComponent } from './not-found/not-found.component';
import { CompletedTestComponent } from './completed-test/completed-test.component';

const routes: Routes = [
  {
    path:'',
    component: CandidateInfoFormComponent
  },
  {
    path:'test',
    component: TakingTestComponent
  },
  {
    path:'not-found',
    component: NotFoundComponent
  },
  {
    path:'complete',
    component: CompletedTestComponent
  },
  {
    path: '**',
    redirectTo: 'not-found',
  },
  
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
