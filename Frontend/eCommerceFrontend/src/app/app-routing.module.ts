import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './Components/login/login.component';
import { ProductListComponent } from './Components/product-list/product-list.component';

const routes: Routes = [
   { path: 'login', component: LoginComponent },
   { path: '', component: ProductListComponent } 
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
