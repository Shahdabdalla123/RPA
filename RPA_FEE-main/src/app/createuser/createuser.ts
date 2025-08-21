import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormControl, FormGroup, FormsModule, FormSubmittedEvent, ReactiveFormsModule, Validators } from '@angular/forms';
import { EmployeeDTo, EmployeeService } from '../ApiServices/employee-service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-createuser',
  imports: [CommonModule, FormsModule,ReactiveFormsModule],
  templateUrl: './createuser.html',
  styleUrl: './createuser.css'
})
export class Createuser {
   
  CreateuserForm: FormGroup;
  isSubmitting = false;
  Employee:any;

  constructor( private EmployeeService: EmployeeService,private router: Router) {

    this.CreateuserForm = new FormGroup({
      fName: new FormControl('', [
        Validators.required,
        Validators.pattern('^[a-zA-Z ]+$')
      ]),
      lName: new FormControl('', [
        Validators.required,
        Validators.pattern('^[a-zA-Z ]+$')
      ]),
      email: new FormControl('', 
      [
       Validators.required, Validators.email
      ]),
      password:new FormControl('', [
        Validators.required,
        Validators.minLength(6),
        Validators.maxLength(100),
        Validators.pattern(/^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*?&#]).+$/)
      ]),
      role: new FormControl('', [
        Validators.required,
        Validators.pattern('^[a-zA-Z ]+$')
      ]),
    });
  }
  showPassword: boolean = false;
  togglePasswordVisibility(): void {
    this.showPassword = !this.showPassword;
  }
  onSubmit() {
    if (this.CreateuserForm.valid) 
    {
      this.isSubmitting = true;
      const formValue = this.CreateuserForm.value;
      const Employee: EmployeeDTo = {
        fName: formValue.fName,
        lName: formValue.lName,
        email: formValue.email,
        password: formValue.password,
        role:formValue.role
      };
       this.EmployeeService.createEmployee(Employee).subscribe((res)=>{
            if(res.success) {
              this.router.navigate(['/home']) ; 
            }
            console.log('Employee created successfully:', Employee);
        },(err) => {
           console.error('Error creating employee:', err);
        }
      );
  }
}
}