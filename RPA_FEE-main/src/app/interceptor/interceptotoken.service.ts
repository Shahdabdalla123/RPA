import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, Observable, throwError } from 'rxjs';
import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { AuthenticationService } from '../ApiServices/authentication-service';
import { EmployeeService } from '../ApiServices/employee-service';

@Injectable({
  providedIn: 'root'
})
@Injectable()
export class InterceptorTokenService implements HttpInterceptor {
  constructor(private auth: AuthenticationService, private router: Router,private account:EmployeeService) {}

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const token = this.auth.getToken();
    if (token) {
      request = request.clone({
        setHeaders: {
          Authorization: `Bearer ${token}`
        }
      });
    }

    return next.handle(request).pipe(
      catchError((error) => {
        if (error.status === 401) {
          this.auth.logout();
          this.router.navigate(['/login'], { queryParams: { message: 'Unauthorized' } });
        }
        return throwError(() => error);
      })
    );
  }
}
