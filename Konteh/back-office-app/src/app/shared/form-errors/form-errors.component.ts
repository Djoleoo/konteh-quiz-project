import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { UntypedFormGroup } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';

@Component({
  selector: 'app-form-errors',
  templateUrl: './form-errors.component.html',
  standalone: true,
  imports: [MatFormFieldModule, CommonModule]
})
export class FormErrorsComponent {
  @Input() form!: UntypedFormGroup;

  get generalErrors() {
    return this.form.errors?.['general'];
  }
}
