import { GuidRequest } from './../Models/GuidRequest';
import { firstValueFrom, Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Credentials } from '../Models/Credentials';
import { AuthModel } from '../Models/AuthModel';
import { AuthType } from '../Models/AuthType';
import { AuthResult } from '../Models/AuthResult';
import { Product } from '../Models/Product';
import { PasswordPolicy } from '../Models/PasswordPolicy';

@Injectable({
  providedIn: 'root'
})
export class ControllerService
{
  constructor(private http: HttpClient) { }

  getAuthModel(credentials : Credentials): AuthModel{
    let authModel = new AuthModel();
    authModel.credentials = credentials;
    authModel.authType = credentials.email && credentials.password ?
    AuthType.LogIn : AuthType.Guest;

    return authModel;
  }

  async auth(url : string, credentials : Credentials) : Promise<AuthResult>{
    let authModel = this.getAuthModel(credentials);
    let authResult = await firstValueFrom(this.http.post<AuthResult>(url, authModel))

    return authResult;
  }

   getProducts(url : string) : Observable<Array<Product>>{
    return this.http.get<Array<Product>>(url);
  }

  getProduct(url : string, id: number) : Observable<Product>{
    return this.http.get<Product>(url + id);
  }

  addProduct(url: string, product: Product) : Observable<Product>{
    return this.http.post<Product>(url, product);
  }

  updateProduct(url: string, product: Product) : Observable<Product>{
    return this.http.put<Product>(url, product);
  }

  deleteProduct(url: string, id : number) : Observable<boolean>{
    return this.http.delete<boolean>(url + id);
  }

  registerUser(url: string, credentials : Credentials) : Observable<boolean>{
    return this.http.post<boolean>(url, credentials);
  }

  deleteUser(url: string, guidRequest : GuidRequest) : Observable<boolean>{
    return this.http.delete<boolean>(url, { body : guidRequest});
  }

  getPasswordPolicy(url: string) : Observable<PasswordPolicy>{
    return this.http.get<PasswordPolicy>(url);
  }
}
