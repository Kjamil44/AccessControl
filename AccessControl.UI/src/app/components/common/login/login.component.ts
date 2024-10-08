import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { AccessControlService } from 'src/app/services/access-control.service';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  loginForm: FormGroup;

  constructor(private authService: AuthService, private accessService: AccessControlService, private router: Router) {
    this.loginForm = new FormGroup({
      email: new FormControl(),
      password: new FormControl()
    });
  }

  onLogIn() {
    const credentials = {
      "email": this.loginForm.value.email,
      "password": this.loginForm.value.password
    }

    this.authService.login(credentials)
      .subscribe({
        next: data => {
          this.authService.storeToken(data.token);
          this.router.navigate([`/dashboard`]);
        },
        error: error => {
          this.accessService.createErrorNotification(error.message);
        }
      });;
  }

  navigateToRegister() {
    this.router.navigate([`/register`]);
  }

}
