import { MatButtonModule } from '@angular/material/button';
import { NgModule } from '@angular/core';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatInputModule } from '@angular/material/input';
import { MatCardModule } from '@Angular/material/card';
import { MatTableModule } from '@angular/material/table';
import { MatGridListModule } from '@angular/material/grid-list'

const material = [
  MatButtonModule,
  MatProgressSpinnerModule,
  MatToolbarModule,
  MatInputModule,
  MatCardModule,
  MatTableModule,
  MatGridListModule
];

@NgModule({
  imports: [material],
  exports: [material]
})
export class MaterialModule { }
