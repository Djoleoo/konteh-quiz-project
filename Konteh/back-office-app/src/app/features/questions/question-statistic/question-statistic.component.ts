import { Component, OnInit } from '@angular/core';
import { QuestionClient } from '../../../api/api-reference';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-question-statistic',
  templateUrl: './question-statistic.component.html',
  styleUrl: './question-statistic.component.css'
})
export class QuestionStatisticComponent implements OnInit{

  questionId?: number;
  chartOptions: any;
  
  constructor(
    private questionClient: QuestionClient,
    private route: ActivatedRoute
  ) { }

  ngOnInit(): void {
    this.chartOptions = {
      animationEnabled: true,
      theme: "light",
      exportEnabled: true,
      title: {
        text: ""
      },
      subtitles: [{
        text: "Candidates answered correctly"
      }],
      data: [{
        type: "pie",
        indexLabel: "{name}: {y}%",
        dataPoints: [] 
      }]
    }

    this.route.paramMap.subscribe(param => {
      const paramId = param.get('id');
      if (paramId) {
        this.questionId = Number(paramId)
        this.getQuestionStatistics(this.questionId);
      }
    });
    
  }

  getQuestionStatistics(id: number) {
    this.questionClient.getQuestionStatistic(id).subscribe({
      next: (response) => {
        const correctPercentage = response.percentage || 0;
        const wrongPercentage = 100 - correctPercentage;
        this.chartOptions.title.text = response.text;
        this.chartOptions.data[0].dataPoints = [
          { name: "Correct", y: correctPercentage , color: '#0FFF50'},
          { name: "Wrong", y: wrongPercentage, color: '#FF0000'}
        ];
  
        this.chartOptions = { ...this.chartOptions };
        this.chartOptions.render(); 

      }
    });
  }
  
}
