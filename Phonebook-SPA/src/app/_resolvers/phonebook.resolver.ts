import { Phonebook } from '../_models/phonebook';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { Observable, of } from 'rxjs';
import { PhonebookService } from '../_services/phonebook.service';
import { catchError } from 'rxjs/operators';
import { AlertifyService } from '../_services/alertify.service';
import { Injectable } from '@angular/core';
import { AuthService } from '../_services/auth.service';

@Injectable({providedIn: 'root'})
export class PhonebookResolver implements Resolve<Phonebook>{

  constructor(
    private router: Router,
    private authService: AuthService,
    private phonebookService: PhonebookService,
    private alertify: AlertifyService){}

  resolve(router: ActivatedRouteSnapshot): Observable<Phonebook> {
    return this.phonebookService.getPhonebook(this.authService.decodedToken.nameid)
    .pipe(catchError(error => {
      this.alertify.error('Problem retrieving your data');
      this.router.navigate(['/home']);
      return of(null);
    }))
  }
}
