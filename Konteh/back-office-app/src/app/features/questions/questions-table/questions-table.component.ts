import { Component, ViewChild } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import {  PageEvent } from '@angular/material/paginator';
import { FormControl } from '@angular/forms';
import { debounceTime, map } from 'rxjs/operators';
import { fromEvent } from 'rxjs';
import { ISearchQuestionsResponse, QuestionCategory, QuestionClient, QuestionType, SearchQuestionsQuery, SearchQuestionsResponse } from '../../../api/api-reference';
import { Router } from '@angular/router';

@Component({
  selector: 'app-questions-table',
  templateUrl: './questions-table.component.html',
  styleUrl: './questions-table.component.css'
})
export class QuestionsTableComponent{
  searchControl = new FormControl('');
  pageIndex : number = 0;
  pageSize : number = 0;
  length : number = 0;
  displayedColumns = ['category', 'type', 'text', 'edit'];

  dataSource: MatTableDataSource<ISearchQuestionsResponse> = new MatTableDataSource<ISearchQuestionsResponse>();
  questionList !: SearchQuestionsResponse[];
  
  constructor(private client: QuestionClient, private router:Router) {
    this.pageSize = 5;
    this.pageIndex=0;
    this.search();
  }

  ngOnInit() {
    this.searchControl.valueChanges
      .pipe(debounceTime(500))
      .subscribe(() => {
        this.pageIndex=0;
        this.search();
      })
  }

  deleteQuestion(id: number) {
    this.client.delete(id).subscribe(res => {
      this.pageIndex=0;
      this.search();
    })
  }
  editQuestion(id: number) {
    this.router.navigate(['questions', id]);
  }

  questionStatistic(id: number){
    this.router.navigate(['questions/statistic/', id]);
  }
  getQuestionCategoryStatistics(){
    this.router.navigate(['questions/category/statistics']);
  }

  getQuestionCategoryName(value: number): string {
    const entry = Object.entries(QuestionCategory).find(([key, val]) => val === value);
    return entry ? entry[0] : 'Unknown';
  }
  
  getQuestionTypeName (value: number) {
    const entry = Object.entries(QuestionType).find(([key, val]) => val === value);
    return entry ? entry[0] : 'Unknown';
  }

  search(){
    let query = new SearchQuestionsQuery({
      text: this.searchControl.value ?? undefined,
      page: this.pageIndex,
      pageSize: this.pageSize
    });        
    this.client.search(query).subscribe(res => {
      this.questionList = res.questions === undefined ? [] : res.questions;
      this.length = res.length === undefined ? 0 : res.length;
      
      this.dataSource = new MatTableDataSource<ISearchQuestionsResponse>(this.questionList);
    })
  }
  handlePageEvent(e: PageEvent) {
    this.pageSize = e.pageSize;
    this.pageIndex = e.pageIndex;
    this.search();
  }
}

