import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AccessControlService } from 'src/app/services/access-control.service';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  registerForm: FormGroup;

  constructor(private authService: AuthService,
    private accessService: AccessControlService,
    private fb: FormBuilder,
    private router: Router) {
    this.registerForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      username: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  onSubmit() {
    const credentials = {
      "email": this.registerForm.value.email,
      "password": this.registerForm.value.password,
      "username": this.registerForm.value.username
    }

    if (this.registerForm.valid) {
      this.authService.register(credentials)
        .subscribe({
          next: data => {
            this.authService.storeToken(data.token);
            this.router.navigate([`/dashboard`]);
          },
          error: error => {
            console.log(error)
            this.accessService.createErrorNotification(error.message);
            //navigate
          }
        });;
    }

  }
}