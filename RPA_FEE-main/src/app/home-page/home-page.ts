import { EmployeeService } from './../ApiServices/employee-service';
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

interface User {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
  isactive: boolean;
}

@Component({
  selector: 'app-user-management',
  standalone: true,
  imports: [CommonModule, FormsModule,RouterModule],
  templateUrl: './home-page.html',
  styleUrls: ['./home-page.css']
})
export class HomePage implements OnInit {
  searchText: string = '';
  users !: any;

  constructor(private employeeService:EmployeeService) {}
  ngOnInit(): void {
   this.search() ; 
  }

  search(): void {
    this.employeeService.searchEmployee(this.searchText).subscribe((res)=>{
      if (res.success) {
        this.users = res.users; 
      } else {
        console.error('Search failed:', res.message);
      }
      
    },(err) => {
        console.error('Error during search:', err);
      }) ; 
    if (!this.searchText) {
      return;
    }
    
  }

  clearFilter(): void {
    this.searchText = '';
  }

  disableUser(userId: string): void {
        // alert(`Are you sure you want to deactivate user with ID: ${userId}?`);
        this.employeeService.disableEmployee(userId).subscribe((res)=>{
            this.search() ; 
        },(err)=>{
          console.log(err) ; 
        }) ;
    // const user = this.users.find(u => u.id === userId);
    // if (user) {
    //   user.isactive = !user.isactive;
    //   // this.applyFilter(); // Reapply filter to update the view
    // }
  }
 
}
