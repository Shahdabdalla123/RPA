import { EmployeeService } from './../ApiServices/employee-service';
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { AuthenticationService } from '../ApiServices/authentication-service';


@Component({
  selector: 'app-header',
  standalone: true,
  imports: [CommonModule,RouterModule],
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent  {
 
  constructor(protected authService:AuthenticationService,private router:Router){}
   
 logout():void{
  this.authService.logout() ;  
 }
  
}
