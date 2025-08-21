import { EmployeeService } from './../ApiServices/employee-service';
import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { AuthenticationService } from '../ApiServices/authentication-service';
import { take } from 'rxjs/operators';

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule, CommonModule, RouterModule],
  templateUrl: './login.html',
  styleUrls: ['./login.css']
})
export class Login implements OnInit {
  loginForm!: FormGroup;
  showPassword = false;
  errorMessage: string | null = null;

  constructor(
    private router: Router,
    private employeeService: EmployeeService,
    private authService: AuthenticationService,
    private fb: FormBuilder
  ) {}

  ngOnInit(): void {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [
        Validators.required,
        Validators.minLength(6),
        Validators.maxLength(100),
        Validators.pattern(/^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*?&#]).+$/)
      ]]
    });
  }

  togglePasswordVisibility(): void {
    this.showPassword = !this.showPassword;
  }

  get email() { return this.loginForm.get('email'); }
  get password() { return this.loginForm.get('password'); }

  onSubmit() {
    if (this.loginForm.invalid) return;

    this.authService.login(this.email?.value!, this.password?.value!)
      // .pipe(take(1))
      .subscribe((res) => {
          console.log(res.token);
          if (res.success) {
            this.authService.setAuthState(res.token);
            this.router.navigate(['/home']);
          } else {
            this.errorMessage = 'Login failed: No token received';
          }
        },
        (err) => {
          this.errorMessage = 'Invalid email or password';
          console.log(err);
          
        }
      );
  }
}
