import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AccessControlService } from 'src/app/services/access-control.service';
import { AuthService } from 'src/app/services/auth.service';
import { SpinnerService } from 'src/app/services/spinner.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
})
export class RegisterComponent {
  registerForm: FormGroup;

  roleOptions = [
    { label: 'System Admin', value: 'SystemAdmin' },
    { label: 'Site Admin', value: 'SiteAdmin' },
    { label: 'Security Operator', value: 'SecurityOperator' },
    { label: 'Cardholder Manager', value: 'CardholderManager' },
    { label: 'Auditor', value: 'Auditor' },
  ];

  constructor(
    private authService: AuthService,
    private accessService: AccessControlService,
    public spinner: SpinnerService,
    private fb: FormBuilder,
    private router: Router
  ) {
    this.registerForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      username: ['', Validators.required],
      role: [null, [Validators.required]],
      password: ['', Validators.required],
    });
  }

  onSubmit() {
    const credentials = {
      email: this.registerForm.value.email,
      password: this.registerForm.value.password,
      username: this.registerForm.value.username,
    };

    if (this.registerForm.valid) {
      this.spinner.with(this.authService.register(credentials)).subscribe({
        next: (data) => {
          this.authService.storeToken(data.token);
          this.router.navigate([`/dashboard`]);
        },
        error: (err: Error) => {
          this.accessService.createErrorNotification(err.message);
        },
      });
    }
  }

  isInvalid(controlName: string): boolean {
    const c = this.registerForm.get(controlName);
    return !!c && c.invalid && (c.dirty || c.touched);
  }

  goToLogin() {
    this.router.navigate([`/`]);
  }
}
