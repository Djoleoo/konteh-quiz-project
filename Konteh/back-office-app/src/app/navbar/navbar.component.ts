import { Component, OnInit } from '@angular/core';
import { MsalService } from '@azure/msal-angular';
import { GetAllQuestionsResponse, QuestionClient } from '../api/api-reference';
import { Router } from '@angular/router';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent implements OnInit{
  username: string | undefined;

  constructor(private authService: MsalService, private questionClient: QuestionClient, private router: Router){}

  ngOnInit(): void {
    this.authService.initialize();
    this.setLoginDisplay();
  }

  login(){
    this.authService.loginPopup()
    .subscribe({
      next: result => {
        this.setLoginDisplay();
      },
      error: (error) => {
        
      }
    });
  }

  logout() {
    this.authService.logoutPopup({
      mainWindowRedirectUri: "/"
    }).subscribe(_ => {
      this.username = undefined;
    });
  }

  setLoginDisplay() {
    const accounts = this.authService.instance.getAllAccounts();
    if(accounts && accounts[0]){
      this.authService.instance.setActiveAccount(accounts[0]);
      this.username = accounts[0].name;
    }else{
      this.username = undefined;
      this.authService.instance.setActiveAccount(null);
    }
  }

  addQuestion() {
    this.router.navigate(['questions/add'])
  }
  questionsTable() {
    this.router.navigate(['questions'])
  }
  examOverview() {
    this.router.navigate(['exams'])
  }

}
