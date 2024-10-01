import { Route, RouterModule } from "@angular/router";
import { QuestionsTableComponent } from "./questions-table/questions-table.component";
import { QuestionFormComponent } from "./question-form/question-form.component";
import { NgModule } from "@angular/core";
import { QuestionStatisticComponent } from "./question-statistic/question-statistic.component";
import { QuestionCategoryStatisticsComponent } from "./question-category-statistics/question-category-statistics.component";

const routes: Route[] = [
    {
        path: '',
        component: QuestionsTableComponent
    },
    {
        path: 'add',
        component: QuestionFormComponent
    },
    {
        path: ':id',
        component: QuestionFormComponent
    },
    {
        path: 'statistic/:id',
        component: QuestionStatisticComponent
    },
    {
        path: 'category/statistics',
        component: QuestionCategoryStatisticsComponent
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class QuestionsRoutingModule { }