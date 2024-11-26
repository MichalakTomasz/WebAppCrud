import { ControllerService } from './../../Services/controller.service';
import { Component } from '@angular/core';
import { ReactiveFormsModule, FormGroup, FormControl } from '@angular/forms';
import { Product } from '../../Models/Product';
import { MaterialModule } from '../../material/material.module';

@Component({
    selector: 'app-add.product',
    imports: [ReactiveFormsModule, MaterialModule],
    templateUrl: './add.product.component.html',
    styleUrl: './add.product.component.css'
})
export class AddProductComponent {
  baseUrl = 'https://localhost:7174/product/';
  namePlaceholder = 'Name';
  codePlaceholder = 'Code';
  descriptionPlaceholder = 'Description';
  urlPicturePlaceholder = 'Url Picture';
  pricePlaceholder = 'Price';
  addResult = ''

  constructor(private controllerService: ControllerService){ }

  addProductFormGroup = new FormGroup({
    name : new FormControl(''),
    code : new FormControl(''),
    description: new FormControl(''),
    urlPicture : new FormControl(''),
    price : new FormControl(0)
  });

  onSubmit(){
    const product : Product = {
      name : this.addProductFormGroup.controls.name.value ?? '',
      code : this.addProductFormGroup.controls.code.value ?? '',
      description : this.addProductFormGroup.controls.description.value ?? '',
      urlPicture : this.addProductFormGroup.controls.urlPicture.value ?? "",
      price : +(this.addProductFormGroup.controls.price.value ?? 0)
    };

    this.controllerService.addProduct(this.baseUrl, product)
    .subscribe({
      next : () => this.addResult = 'Add product successful',
      error : e => {
        if (e.error){
          if (Array.isArray(e.error)){
            let errorMessage = e.error.map((er: { errorMessage: string; }) => er.errorMessage).join(',');
            this.addResult = errorMessage;
          }
          if (typeof e.error === 'string'){
            this.addResult = e.error
          }
        }
      }
    });
  }
}
