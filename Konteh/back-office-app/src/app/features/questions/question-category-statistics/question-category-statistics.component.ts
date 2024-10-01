import { Component, OnInit } from '@angular/core';
import { GetQuestionCategoryStatisticResponse, QuestionClient } from '../../../api/api-reference';

@Component({
  selector: 'app-question-category-statistics',
  templateUrl: './question-category-statistics.component.html',
  styleUrl: './question-category-statistics.component.css'
})
export class QuestionCategoryStatisticsComponent implements OnInit{

  chartCategoryOptions: any;
    
  constructor(
    private questionClient: QuestionClient,
  ) { }

  ngOnInit(): void {
    this.chartCategoryOptions = {
      animationEnabled: true,
      theme: "light",
      exportEnabled: true,
      title: {
        text: "Questions statistics by category"
      },
      subtitles: [{
        text: "Candidates answered correctly"
      }],
      data: [{
        type: "column",
        indexLabel: "{label}: {y}%",
        dataPoints: [] 
      }]
    }
    this.getQuestionCategoryStatistics();
  }

  getQuestionCategoryStatistics(){

    this.questionClient.getCategoryQuestionStatistic().subscribe({
      next: (response: GetQuestionCategoryStatisticResponse[]) => {
        
        this.chartCategoryOptions.data[0].dataPoints = response.map(element => {
          return { label: element.categoryName, y: element.correctPercentage };
        });
        this.chartCategoryOptions = { ...this.chartCategoryOptions };
        this.chartCategoryOptions.render(); 
      }
    });   
  }
}
