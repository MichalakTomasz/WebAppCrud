import { RouterModule } from '@angular/router';
import { UserService } from './../../Services/user.service';
import { Component, OnInit } from '@angular/core';
import { MenuItem } from '../../Models/MenuItem';
import { CommonModule } from '@angular/common';
import { MaterialModule } from '../../material/material.module';

@Component({
    selector: 'app-menu',
    imports: [RouterModule, CommonModule, MaterialModule],
    templateUrl: './menu.component.html',
    styleUrl: './menu.component.css'
})
export class MenuComponent implements OnInit {
  menuItems? :Array<MenuItem>;

  constructor(private userService : UserService){ }

  async ngOnInit() {
    this.userService.getUserObservable().subscribe(u =>{
      if (u.isAuthorized){
        if (u.roles?.includes('Admin')){
          this.menuItems =
          [
            { name: 'Home', path: 'home'},
            { name: 'Login', path: 'auth'},
            { name: 'Products', path: "products" },
            { name: 'Product', path: "product" },
            { name: 'Add product', path: "addProduct" },
            { name: 'Update product', path: 'updateProduct'},
            { name: 'Delete Product', path: "deleteProduct" },
            { name: 'Register user', path: 'registerUser' },
            { name: 'Delete user', path: 'deleteUser' }
          ]
        }
        else if (u.roles?.includes('Guest')){
          this.menuItems =
          [
            { name: 'Home', path: 'home' },
            { name: 'Login', path: 'auth'},
            { name: 'Products', path: 'products' },
            { name: 'Product', path: 'product' },
          ]
        }
      }
    });
  }
}
