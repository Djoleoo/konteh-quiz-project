import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AnswerDto, ExamClient, GetExamExamQuestionDto, GetExamQuestionDto, SubmitExamCommand } from '../api/api-reference';
import { MatDialog } from '@angular/material/dialog';
import { SubmitDialogComponent } from '../submit-dialog/submit-dialog.component';
import { environment } from '../../environments/environments';

@Component({
  selector: 'app-taking-test',
  templateUrl: './taking-test.component.html',
  styleUrl: './taking-test.component.css'
})
export class TakingTestComponent implements OnInit{
  id : number = 0;
  page: number = 0;
  display: string = '';
  isHidden = false;
  isCompleted = false;
  readonly dialog = inject(MatDialog)
  endTime : Date = new Date();
  examQuestions: GetExamExamQuestionDto[] = [];
  question: GetExamQuestionDto = new GetExamQuestionDto();

  constructor (private route: ActivatedRoute, private examClient: ExamClient, private router:Router){
  }

  loadExam(){
    this.examClient.getExam(this.id).subscribe({
      next:(res) =>{
        this.examQuestions = res.examQuestionDtos!;
        this.question = res.examQuestionDtos![0].questionDto!;
        this.endTime = new Date(res.startTime?.getTime()!)
        this.endTime.setSeconds(this.endTime.getSeconds() + environment.examDuration.seconds)
      } 
    })
  }

  ngOnInit(): void{
    this.route.queryParamMap.subscribe(param => {
      this.id = param.get('id')!==null ? Number(param.get('id')) : 0;
    })
    this.loadExam();
    this.timer();
  }

  isChecked(answer: AnswerDto){
    return this.examQuestions[this.page].selectedAnswers?.includes(answer, 0)
  }
  onCheckboxChange(answer: AnswerDto) {
    if(this.isChecked(answer)){
      this.examQuestions[this.page].selectedAnswers = this.examQuestions[this.page].selectedAnswers?.filter((a) => a.answerId !== answer.answerId);
      return;
    }
    this.examQuestions[this.page].selectedAnswers?.push(answer)
  }

  onRadioButtonChange(answer: AnswerDto) {
    this.examQuestions[this.page].selectedAnswers = this.examQuestions[this.page].selectedAnswers?.filter((a) => a.answerId === answer.answerId);
    this.examQuestions[this.page].selectedAnswers?.push(answer);
  }

  next() {
    if(this.page < this.examQuestions.length-1 ){
      this.page++;
      this.question = this.examQuestions[this.page].questionDto!;
    }
  }
  previous(){
    if(this.page > 0 ){
      this.page--;
      this.question = this.examQuestions[this.page].questionDto!;
    }
  }
  submit() {
    var request = new SubmitExamCommand({id : this.id, examQuestions: this.examQuestions});
    this.examClient.submit(request).subscribe({
      next:(res) =>{
        this.isCompleted = true;
        this.timer()
        this.router.navigate(["complete"])
      }
    })
  }
  timer() {
    let textSecond: any = '0';

    const timer = setInterval(() => {   
      let currentTime = new Date();
      currentTime.setHours(currentTime.getHours()-2);
      
      let seconds = Math.floor((this.endTime.getTime() - currentTime.getTime())/1000);
      let second: number = seconds % 60;
      let minute: number = seconds / 60;
      const prefix = minute < 10 ? '0' : '';
      
      if (second < 10) {
        textSecond = '0' + second;
      } else textSecond = second;

      this.display = `${prefix}${Math.floor(minute)}:${textSecond}`;

      if (minute === 0 && second === 0) {
        this.display = 'Time is up!';
        this.openDialog();
        clearInterval(timer);
      }
      if(this.isCompleted){
        clearInterval(timer);
      }
    }, 1000);
  }

  openDialog(){
    var dialogRef = this.dialog.open(SubmitDialogComponent, {
      width: '450px'
    });
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.submit()
      }
      else{
        this.router.navigate([""])
      }
    });
  }
  hideTimer() {
    this.isHidden = !this.isHidden;
  }
}
