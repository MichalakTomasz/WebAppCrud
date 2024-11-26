import { Product } from '../../Models/Product';
import { ControllerService } from '../../Services/controller.service';
import { Component, OnInit } from '@angular/core';
import { MaterialModule } from '../../material/material.module';
import { BehaviorSubject } from 'rxjs';

@Component({
    selector: 'app-get.products',
    imports: [MaterialModule],
    templateUrl: './products.component.html',
    styleUrl: './products.component.css'
})
export class ProductsComponent implements OnInit {

  baseUrl = 'https://localhost:7174/product'
  products = <Array<Product>>{}
  productsObs = new BehaviorSubject<Array<Product>>(this.products);
  displayedColumns: string[] = ['id', 'code', 'name', 'description', 'urlPicture', 'price'];
  constructor(private controllerService: ControllerService){ }

  ngOnInit(){
    this.controllerService.getProducts(this.baseUrl)
    .subscribe(d => {
      this.products = d;
      this.productsObs.next(this.products)
    });
  }
}


