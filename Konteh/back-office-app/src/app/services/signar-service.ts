import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { GetExamsResponse } from '../api/api-reference';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SignalRService {
  private hubConnection!: signalR.HubConnection;
  private messageSubject = new Subject<GetExamsResponse>();
  message$ = this.messageSubject.asObservable();

  startConnection(hubUrl: string): void {
    this.hubConnection = new signalR.HubConnectionBuilder()
    .withUrl(hubUrl)
    .build();

    this.hubConnection
    .start()
    .catch(err => console.log('Error while starting connection' + err))

    this.hubConnection.on('ReceiveExamRequest', (message: GetExamsResponse) => {
      this.messageSubject.next(message);
    });

    this.hubConnection.on('RecieveExamSubmit', (message: GetExamsResponse) => {
      this.messageSubject.next(message);
    });
  }
}
