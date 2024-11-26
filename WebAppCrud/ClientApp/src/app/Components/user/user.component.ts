import { MaterialModule } from '../../material/material.module';
import { AuthResult } from '../../Models/AuthResult';
import { UserService } from './../../Services/user.service';
import { Component, OnInit } from '@angular/core';

@Component({
    selector: 'app-user',
    imports: [MaterialModule],
    templateUrl: './user.component.html',
    styleUrl: './user.component.css'
})
export class UserComponent implements OnInit {
  currentUser = <AuthResult>{};

  constructor (private userService: UserService) { }

  async ngOnInit() {
    this.currentUser = await this.userService.getUser();
  }
}
