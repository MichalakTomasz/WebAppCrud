import { AuthResult } from './../Models/AuthResult';
import { Injectable } from '@angular/core';
import { ControllerService } from './controller.service';
import { Credentials } from '../Models/Credentials';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private authData = <AuthResult>{};
  authDataObs = new BehaviorSubject<AuthResult>(this.authData);

  private baseUrl = 'https://localhost:7174/auth/';

  constructor(private controllerService: ControllerService) { }

  setUser(authResult: AuthResult){
    Object.assign(this.authData, authResult)
    this.authDataObs.next(this.authData)
  }

  getUserObservable() : Observable<AuthResult>{
    return this.authDataObs;
  }

  async getUser(){
    if (this.authData?.token){
      return this.authData;
    }

    let currentUser =  await this.controllerService.auth(this.baseUrl, new Credentials());
    this.setUser(currentUser);
    return this.authData
  }

  checkUser(){
    return this.authData;
  }
}
