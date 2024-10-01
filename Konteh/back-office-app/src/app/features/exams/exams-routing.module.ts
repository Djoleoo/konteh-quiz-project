import { Route, RouterModule } from "@angular/router";
import { ExamsOverviewComponent } from "./exams-overview/exams-overview.component";
import { NgModule } from "@angular/core";

const routes: Route[] = [
    {
        path: '',
        component: ExamsOverviewComponent
    }
]

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class ExamsRoutingModule { }