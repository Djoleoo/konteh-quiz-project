import { Component, inject } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-submit-dialog',
  templateUrl: './submit-dialog.component.html',
  styleUrl: './submit-dialog.component.css'
})
export class SubmitDialogComponent {
  readonly dialogRef = inject(MatDialogRef<SubmitDialogComponent>)

  onSubmit(): void {
    this.dialogRef.close(true); 
  }
}
