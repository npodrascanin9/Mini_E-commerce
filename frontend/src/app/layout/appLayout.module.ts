import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MaterialModule } from '../shared/material.module';
import { HeaderLayoutComponent } from './header/header-layout.component';
import { BodyLayoutComponent } from './body/body-layout.component';

@NgModule({
  declarations: [
    HeaderLayoutComponent,
    BodyLayoutComponent
  ],
  imports: [
    CommonModule,
    RouterModule,
    MaterialModule
  ],
  exports: [
    HeaderLayoutComponent,
    BodyLayoutComponent
  ]
})
export class AppLayoutModule {}
