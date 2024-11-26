import { ControllerService } from './../../Services/controller.service';
import { Component } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MaterialModule } from '../../material/material.module';

@Component({
    selector: 'app-delete.product',
    imports: [ReactiveFormsModule, MaterialModule],
    templateUrl: './delete.product.component.html',
    styleUrl: './delete.product.component.css'
})
export class DeleteProductComponent {
  baseUrl = 'https://localhost:7174/product/';
  deleteResult = '';
  idPlaceholder = 'Id';
  deleteFormGroup = new FormGroup({
    id : new FormControl(0)
  });
  constructor(private controllerService : ControllerService){}
  onSubmit(){
    this.controllerService.deleteProduct(this.baseUrl, this.deleteFormGroup.controls.id.value ?? 0)
    .subscribe({
      next : () => this.deleteResult = 'Update product successful',
      error : e => {
        if (e.error){
          if (Array.isArray(e.error)){
            let errorMessage = e.error.map((er: { errorMessage: string; }) => er.errorMessage).join(',');
            this.deleteResult = errorMessage;
          }
          if (typeof e.error === 'string'){
            this.deleteResult = e.error
          }
        }
      }
    });
  }

}
