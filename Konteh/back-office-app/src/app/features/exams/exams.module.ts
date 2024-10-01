import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatFormField, MatLabel } from '@angular/material/form-field';
import { MatPaginatorModule } from '@angular/material/paginator';
import { ExamsOverviewComponent } from './exams-overview/exams-overview.component';
import { MatTableModule } from '@angular/material/table';
import {  MatInputModule } from '@angular/material/input';
import { ExamsRoutingModule } from './exams-routing.module';



@NgModule({
  declarations: [ExamsOverviewComponent],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormField,
    MatLabel,
    MatPaginatorModule,
    MatTableModule,
    MatInputModule,
    FormsModule,
    ExamsRoutingModule
  ],
  exports:
  [
    ExamsOverviewComponent
  ]
})
export class ExamsModule { }
