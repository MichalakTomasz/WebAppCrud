import { ControllerService } from './../../Services/controller.service';
import { Component } from '@angular/core';
import { ReactiveFormsModule, FormGroup, FormControl } from '@angular/forms';
import { Product } from '../../Models/Product';
import { MaterialModule } from '../../material/material.module';

@Component({
    selector: 'app-update.product',
    imports: [ReactiveFormsModule, MaterialModule],
    templateUrl: './update.product.component.html',
    styleUrl: './update.product.component.css'
})
export class UpdateProductComponent {
  baseUrl = 'https://localhost:7174/product/';
  idPlaceholder = 'Id';
  namePlaceholder = 'Name';
  codePlaceholder = 'Code';
  descriptionPlaceholder = 'Description';
  urlPicturePlaceholder = 'Url Picture';
  pricePlaceholder = 'Price';
  updateResult = ''

  constructor(private controllerService: ControllerService){ }

  updateProductFormGroup = new FormGroup({
    id : new FormControl(0),
    name : new FormControl(''),
    code : new FormControl(''),
    description: new FormControl(''),
    urlPicture : new FormControl(''),
    price : new FormControl(0)
  });

  onSubmit(){
    const product : Product = {
      id: +(this.updateProductFormGroup.controls.id.value ?? 0),
      name : this.updateProductFormGroup.controls.name.value ?? '',
      code : this.updateProductFormGroup.controls.code.value ?? '',
      description : this.updateProductFormGroup.controls.description.value ?? '',
      urlPicture : this.updateProductFormGroup.controls.urlPicture.value ?? "",
      price : +(this.updateProductFormGroup.controls.price.value ?? 0)
    };

    this.controllerService.updateProduct(this.baseUrl, product)
    .subscribe({
      next : () => this.updateResult = 'Update product successful',
      error : e => {
        if (e.error){
          if (Array.isArray(e.error)){
            let errorMessage = e.error.map((er: { errorMessage: string; }) => er.errorMessage).join(',');
            this.updateResult = errorMessage;
          }
          if (typeof e.error === 'string'){
            this.updateResult = e.error
          }
        }
      }
    });
  }
}
