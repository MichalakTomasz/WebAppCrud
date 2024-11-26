import { Component, OnInit } from '@angular/core';
import { RouterModule, RouterOutlet } from '@angular/router';
import { ControllerService } from './Services/controller.service';
import { UserService } from './Services/user.service';
import { MenuComponent } from "./Components/menu/menu.component";
import { AuthResult } from './Models/AuthResult';
import { UserComponent } from "./Components/user/user.component";
import { MaterialModule } from './material/material.module';

@Component({
    selector: 'app-root',
    imports: [
      RouterOutlet,
      RouterModule,
      MenuComponent,
      UserComponent,
      MaterialModule],
    templateUrl: './app.component.html',
    styleUrl: './app.component.css',
    providers: [
        ControllerService
    ]
})
export class AppComponent implements OnInit{
  title = 'Cloud Academy - Backend developer';
  baseUrl = 'https://localhost:7174/auth'
  currentUser = <AuthResult>{};

  constructor(private userService: UserService){ }

  async ngOnInit() {
    this.currentUser = await this.userService.getUser();
  }
}
