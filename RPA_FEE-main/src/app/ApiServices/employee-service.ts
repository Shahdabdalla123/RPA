import { Injectable } from '@angular/core';
import { API } from './enviroment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthenticationService } from './authentication-service';
import { Router } from '@angular/router';
export interface EmployeeDTo {
  fName: string;
  lName: string;
  email: string;
  password: string;
  role: string;
}
@Injectable({
  providedIn: 'root'
})
export class EmployeeService {
  private apiUrl = `${API}/Auth`;

  constructor(private http: HttpClient,private auth:AuthenticationService,private router:Router) { }

  disableEmployee(employeeId: string): Observable<any> {
    return this.http.put(`${this.apiUrl}`,{
      id : employeeId
    });
  };

  createEmployee(Employee: EmployeeDTo) :Observable<any> {
    return this.http.post(`${this.apiUrl}/register `, Employee);
  }

  searchEmployee(searchTextEmail: any):Observable<any>   {
    return this.http.get(`${this.apiUrl}/?email=${searchTextEmail}`);
  }
  
}
