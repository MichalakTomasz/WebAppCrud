import { inject } from '@angular/core';
import {
  HttpRequest,
  HttpHandlerFn,
  HttpInterceptorFn
} from '@angular/common/http';

import { UserService } from './Services/user.service';  


export const authInterceptor: HttpInterceptorFn = (req: HttpRequest<unknown>, next:HttpHandlerFn) => {
  const userService = inject(UserService);
  const currentUser = userService.checkUser();
  if (currentUser?.token){
    req = req.clone({
      headers: req.headers.set('Authorization', `Bearer ${currentUser.token}`),
    });
  }

  return next(req);
};
