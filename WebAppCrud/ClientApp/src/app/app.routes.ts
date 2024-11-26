import { Routes } from '@angular/router';
import { MainComponent } from "./Components/main/main.component";
import { AuthComponent } from "./Components/auth/auth.component";
import { PageNotFoundComponent } from "./Components/page.not.found/page.not.found.component";
import { AboutComponent } from './Components/about/about.component';
import { ProductsComponent } from './Components/products/products.component';
import { UpdateProductComponent } from './Components/update.product/update.product.component';
import { DeleteProductComponent } from './Components/delete.product/delete.product.component';
import { RegisterUserComponent } from './Components/register.user/register.user.component';
import { DeleteUserComponent } from './Components/delete.user/delete.user.component';
import { ProductComponent } from './Components/product/product.component';
import { AddProductComponent } from './Components/add.product/add.product.component';

export const routes: Routes =
[
    { path: '', component: MainComponent, pathMatch: 'full' },
    { path: 'home', component: MainComponent },
    { path: 'auth', component: AuthComponent },
    { path: 'about', component: AboutComponent },
    { path: 'products', component: ProductsComponent },
    { path: 'product', component: ProductComponent },
    { path: 'addProduct', component: AddProductComponent },
    { path: 'updateProduct', component: UpdateProductComponent },
    { path: 'deleteProduct', component: DeleteProductComponent },
    { path: 'registerUser', component: RegisterUserComponent },
    { path: 'deleteUser', component: DeleteUserComponent },

    { path: '**', component: PageNotFoundComponent }
];
