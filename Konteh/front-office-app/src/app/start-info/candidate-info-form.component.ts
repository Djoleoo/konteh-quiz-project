import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ExamClient, GenerateExamCommand, ExamQuestionDto } from '../api/api-reference'; 
import { Router } from '@angular/router';

@Component({
  selector: 'app-candidate-info-form',
  templateUrl: './candidate-info-form.component.html',
  styleUrls: ['./candidate-info-form.component.css']
})
export class CandidateInfoFormComponent {
  examQuestions: ExamQuestionDto[] = []; 
  error = '';
  examForm = new FormGroup({
    email: new FormControl('', [
      Validators.required, 
      Validators.pattern('^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$') 
    ]),
    name: new FormControl('', Validators.required),
    surname: new FormControl('', Validators.required),
  });
  constructor(private examClient: ExamClient, private router: Router) {
  }

 getErrorMessage(controlName: any): string {
    const control = this.examForm.get(controlName);
    if(control === null)
      return '';
    if (control.hasError('required') && control.touched) {
      return `${controlName.charAt(0).toUpperCase() + controlName.slice(1)} is required`;
    }
    if (control.hasError('pattern') && control.touched) {
      return `Please enter a valid ${controlName}`;
    }
    return '';
  }

  onSubmit() {
    if(!this.examForm.valid)
      return;
    
    const formValue = this.examForm.value;
      const query = new GenerateExamCommand({
        email: formValue.email!,
        name: formValue.name!,
        surname: formValue.surname!
      });
    
      console.log(query);
    this.examClient.createExam(query).subscribe({
      next:(res) => {
        console.log(res.id)
        this.router.navigate(['test'], { queryParams: { id: res.id } });
        this.error = '';
      },
      error:() => {
        this.error = 'You have already taken a test!';
      }
    })
  }
}
