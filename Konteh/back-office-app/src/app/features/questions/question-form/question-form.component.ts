import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormArray, Validators, FormControl } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { setServerSideValidationErrors } from '../../../validation';
import { QuestionType, QuestionCategory, QuestionClient, CreateOrUpdateQuestionAnswerRequest, CreateOrUpdateQuestionCommand } from '../../../api/api-reference';

@Component({
  selector: 'app-question-form',
  templateUrl: './question-form.component.html',
  styleUrls: ['./question-form.component.css']
})
export class QuestionFormComponent implements OnInit {

  questionForm = new FormGroup({
    type: new FormControl(QuestionType.Checkbox, { validators: Validators.required, nonNullable: true }),
    category: new FormControl(QuestionCategory.Oop, { validators: Validators.required, nonNullable: true }),
    text: new FormControl('', { validators: Validators.required, nonNullable: true }),
    answers: new FormArray([]),
    answerText: new FormControl('', { nonNullable: true }),
    isCorrect: new FormControl(false, { nonNullable: true })
  });
  questionType = QuestionType;
  questionCategory = QuestionCategory;
  questionId?: number;
  isEditingQuestion: boolean = false;
  isEditingAnswer: boolean = false;
  editIndex?: number;

  constructor(
    private questionClient: QuestionClient,
    private route: ActivatedRoute,
    private router : Router
  ) { }

  ngOnInit(): void {
    this.route.paramMap.subscribe(param => {
      const paramId = param.get('id');
      if (paramId) {
        this.isEditingQuestion = true;
        this.isEditingAnswer = false;
        this.questionId = Number(paramId)
        this.getQuestion(this.questionId);
      }
    });
  }


  getQuestion(id: number) {
    this.questionClient.getQuestionById(id).subscribe(question => {
      this.questionForm.patchValue({
        text: question.text,
        type: question.type,
        category: question.category,
        answerText: '',
        isCorrect: false
      });

      this.answersArray.clear();
      if (question.answers) {
        question.answers.forEach(answer => {
          this.answersArray.push(this.createAnswerGroup(answer));
        });
      }
    });
  }

  get questionTypeOptions() {
    return Object.keys(this.questionType).filter(key => isNaN(Number(key)));
  }

  get questionCategoryOptions() {
    return Object.keys(this.questionCategory).filter(key => isNaN(Number(key)));
  }

  getEnumValueForType(type: string): QuestionType {
    return this.questionType[type as keyof typeof QuestionType];
  }
  
  getEnumValueForCategory(category: string): QuestionCategory {
    return this.questionCategory[category as keyof typeof QuestionCategory];
  }
  
  get answersArray(): FormArray {
    return this.questionForm.get('answers') as FormArray;
  }

  createAnswerGroup(answer: CreateOrUpdateQuestionAnswerRequest): FormGroup {
    return new FormGroup({
      text: new FormControl(answer.text, { validators: Validators.required, nonNullable: true }),
      isCorrect: new FormControl(answer.isCorrect, { nonNullable: true })
    });
  }

  createAnswer() {
    const newAnswer = new CreateOrUpdateQuestionAnswerRequest({
      text: this.questionForm.get('answerText')?.value,
      isCorrect: this.questionForm.get('isCorrect')?.value
    });
    this.answersArray.push(this.createAnswerGroup(newAnswer));
    this.clearAnswerFormValues();
  }

  onEditAnswerFormClick() {
    const newAnswer = new CreateOrUpdateQuestionAnswerRequest({
      text: this.questionForm.get('answerText')?.value,
      isCorrect: this.questionForm.get('isCorrect')?.value
    });

    if (this.isEditingAnswer) {
      this.answersArray.at(this.editIndex!).patchValue(newAnswer);
      this.editIndex = undefined;
      this.clearAnswerFormValues();
      this.isEditingAnswer = false;
    }
  }

  onEditAnswerCardClick(index: number) {
    const answer = this.answersArray.at(index).value;
    this.questionForm.patchValue({
      answerText: answer.text,
      isCorrect: answer.isCorrect
    });
    this.isEditingAnswer = true
    this.editIndex = index;
  }

  deleteAnswer(index: number) {
    this.answersArray.removeAt(index);
  }

  clearAnswerFormValues() {
    this.questionForm.patchValue({
      isCorrect: false,
      answerText: ''
    });
  }

  convertAnswerArray(): CreateOrUpdateQuestionAnswerRequest[] {
    return this.answersArray.controls.map(control =>
      new CreateOrUpdateQuestionAnswerRequest(control.value)
    );
  }

  onCreateQuestion() {
    if (!this.questionForm.valid) {
      alert("Please fill out all required fields.");
      return;
    }
    if (!this.hasMinimumAnswers()) {
      alert("You must provide at least two answers.");
      return;
    }
    if (!this.hasMinimumCorrectAnswers()) {
      alert("For Checkbox type questions, there must be at least two correct answers.");
      return;
    }

    const createQuestionCommand = new CreateOrUpdateQuestionCommand({
      id: this.questionId,
      text: this.questionForm.get('text')?.value,
      type: this.questionForm.get('type')?.value,
      category: this.questionForm.get('category')?.value,
      answers: this.convertAnswerArray()
    });


    this.questionClient.createOrUpdate(createQuestionCommand).subscribe({
      next: _ => this.router.navigate(['questions']),
      error: errors => setServerSideValidationErrors(errors, this.questionForm)
    })
  }

  clearForm() {
    this.questionForm.reset({
      type: QuestionType.Checkbox,
      category: QuestionCategory.Oop,
      text: '',
      answerText: '',
      isCorrect: false
    });

    this.answersArray.clear();
  }

  hasMinimumAnswers(): boolean {
    return this.answersArray.length >= 2;
  }

  hasMinimumCorrectAnswers(): boolean {
    if (this.questionForm.get('type')?.value === QuestionType.Checkbox) {
      const correctAnswers = this.answersArray.controls.filter(answer => answer.get('isCorrect')?.value === true);
      return correctAnswers.length >= 2;
    }
    return true;
  }
}