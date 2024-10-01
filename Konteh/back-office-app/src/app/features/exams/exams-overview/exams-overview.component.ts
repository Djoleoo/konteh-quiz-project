import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { ExamClient, GetExamsResponse, IGetExamsResponse } from '../../../api/api-reference';
import { ActivatedRoute, Router } from '@angular/router';
import { SignalRService } from '../../../services/signar-service';

@Component({
  selector: 'app-exams-overview',
  templateUrl: './exams-overview.component.html',
  styleUrls: ['./exams-overview.component.css']
})
export class ExamsOverviewComponent implements OnInit {
  displayedColumns: string[] = ['candidate', 'status', 'score', 'startTime'];
  dataSource = new MatTableDataSource<IGetExamsResponse>([]);
  searchText: string = ''; 
  examList: IGetExamsResponse[] = [];
  
  constructor(
    private examClient: ExamClient,
    private route: ActivatedRoute,
    private router: Router,
    private signalRService: SignalRService
  ) {}

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      this.searchText = params['searchText'] || '';  
      this.loadExams();
    });

    this.signalRService.message$.subscribe((message: GetExamsResponse) => {
      this.handleMessage(message);
    });
  }

  loadExams(): void {
    this.examClient.getAllExams(this.searchText).subscribe((response: IGetExamsResponse[]) => {
      this.examList = response || [];
      this.dataSource =  new MatTableDataSource<IGetExamsResponse>(this.examList);
    });
  }

  onSearchChange(searchText: string): void {
    this.searchText = searchText;
    this.updateUrlParams();
    this.loadExams();
  }

  updateUrlParams(): void {
    this.router.navigate([], {
      relativeTo: this.route,
      queryParams: {
        searchText: this.searchText,
      },
      queryParamsHandling: 'merge'  
    });
  }

  handleMessage(message: GetExamsResponse): void{
    const foundExam = this.examList.find(exam => exam.id === message.id);
    console.log("found exam: ", foundExam)
    if (foundExam) {
      foundExam.examStatus = message.examStatus;
      foundExam.score = message.score;
    } else {
      this.examList.unshift(message);
      this.dataSource.data = [...this.examList];
    }
  }
}