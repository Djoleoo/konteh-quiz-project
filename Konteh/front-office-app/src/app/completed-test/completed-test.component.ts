import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-completed-test',
  templateUrl: './completed-test.component.html',
  styleUrl: './completed-test.component.css'
})
export class CompletedTestComponent {
  constructor(private router: Router){
    
  }
  goHome() {
    this.router.navigate([""])
  }

}
