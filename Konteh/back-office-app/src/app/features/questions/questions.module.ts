import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { ReactiveFormsModule, FormsModule } from "@angular/forms";
import { MatButtonModule } from "@angular/material/button";
import { MatFormFieldModule, MatLabel } from "@angular/material/form-field";
import { MatIconModule } from "@angular/material/icon";
import { MatInputModule } from "@angular/material/input";
import { MatMenuModule } from "@angular/material/menu";
import { MatPaginator } from "@angular/material/paginator";
import { MatSortModule } from "@angular/material/sort";
import { MatTableModule } from "@angular/material/table";
import { QuestionFormComponent } from "./question-form/question-form.component";
import { QuestionsTableComponent } from "./questions-table/questions-table.component";
import { MatSelectModule } from "@angular/material/select";
import { FormErrorsComponent } from "../../shared/form-errors/form-errors.component";
import {MatCardModule} from '@angular/material/card';
import { QuestionsRoutingModule } from "./questions-routing.module";
import { CanvasJSAngularChartsModule } from '@canvasjs/angular-charts';
import { MatCheckboxModule } from "@angular/material/checkbox";
import { QuestionStatisticComponent } from './question-statistic/question-statistic.component';
import { QuestionCategoryStatisticsComponent } from './question-category-statistics/question-category-statistics.component';

@NgModule({
    declarations: [
        QuestionsTableComponent,
        QuestionFormComponent,
        QuestionStatisticComponent,
        QuestionCategoryStatisticsComponent
    ],
    imports: [
        CommonModule,
        ReactiveFormsModule,
        MatFormFieldModule,
        MatLabel,
        MatPaginator,
        MatTableModule,
        MatInputModule,
        FormsModule,
        MatIconModule,
        MatMenuModule,
        MatSortModule,
        MatButtonModule,
        MatSelectModule,
        FormErrorsComponent,
        MatCardModule,
        QuestionsRoutingModule,
        CanvasJSAngularChartsModule,
        MatCheckboxModule
    ]
})
export class QuestionsModule { }