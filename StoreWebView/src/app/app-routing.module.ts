import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';  
import { DepartamentComponent } from './departament/departament.component';
import { EmployeeComponent } from './employee/employee.component';

const routes: Routes = [
  {path:'home',component:HomeComponent},
  {path:'departament',component:DepartamentComponent},
  {path:'employee',component:EmployeeComponent}  
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
