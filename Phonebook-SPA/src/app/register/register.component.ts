import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import {
  FormGroup,
  FormControl,
  Validators,
  FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
})
export class RegisterComponent implements OnInit {
  @Output() cancelRegister = new EventEmitter();
  model: any = {};
  registerForm: FormGroup;

  constructor(
    private authService: AuthService,
    private alertify: AlertifyService,
    private fb: FormBuilder,
    private router: Router
  ) {}

  ngOnInit() {
    this.createRegisterForm();
  }

  createRegisterForm() {
    this.registerForm = this.fb.group({
      username: ['',Validators.required],
      password: ['',[Validators.required, Validators.minLength(4), Validators.maxLength(12)]],
      confirmPassword: ['', Validators.required],
      phonebookname: ['',[Validators.required, Validators.minLength(4), Validators.maxLength(24)]]
    }, {validator: this.passwordMatchValidator});
  }

  passwordMatchValidator(g: FormGroup) {
    // tslint:disable-next-line: object-literal-key-quotes
    return g.get('password').value === g.get('confirmPassword').value
      ? null
      : { mismatch: true };
  }

  register() {
    if (this.registerForm.valid) {
      this.model = Object.assign({}, this.registerForm.value);
    this.authService.register(this.model).subscribe(
      () => {
        this.alertify.success('registration Successful');
      },
      (error) => {
        this.alertify.error(error);
      },
      () => {
        this.authService.login(this.model).subscribe(() => {
          this.router.navigate(['/entries']);
        });
    })}
  }

  cancel() {
    this.cancelRegister.emit(false);
    this.alertify.error('Cancelled');
  }
}
