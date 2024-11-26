import { Credentials } from './../../Models/Credentials';
import { Component } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ControllerService } from '../../Services/controller.service';
import { UserService } from '../../Services/user.service';
import { MaterialModule } from '../../material/material.module';

@Component({
    selector: 'app-auth',
    imports: [ReactiveFormsModule, MaterialModule],
    templateUrl: './auth.component.html',
    styleUrl: './auth.component.css'
})
export class AuthComponent
{
  constructor(
    private controllerService : ControllerService,
    private userService : UserService)
    {

    }

  baseUrl = 'https://localhost:7174/auth/'

  emailPlaceholder = "email"
  passwordPlaceholder = "password"
  credentials = <Credentials>{};

  loginFormGroup = new FormGroup({
    emailForm : new FormControl('',[Validators.email]),
    passwordForm : new FormControl('')
  });

  async onSubmit(){
    this.credentials.email = this.loginFormGroup.controls.emailForm.value?.toString();
    this.credentials.password = this.loginFormGroup.controls.passwordForm.value?.toString();

    let authResult = await this.controllerService.auth(this.baseUrl, this.credentials);
    this.userService.setUser(authResult);
  }
}
