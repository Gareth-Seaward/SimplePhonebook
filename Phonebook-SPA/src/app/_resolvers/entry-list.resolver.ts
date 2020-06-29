import { Resolve, ActivatedRouteSnapshot, Router } from '@angular/router';
import { Entry } from '../_models/entry';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { EntryService } from '../_services/entry.service';
import { catchError, flatMap } from 'rxjs/operators';
import { AlertifyService } from '../_services/alertify.service';
import { PhonebookService } from '../_services/phonebook.service';
import { AuthService } from '../_services/auth.service';
import { Phonebook } from '../_models/phonebook';

@Injectable({providedIn: 'root'})
export class EntryListResolver implements Resolve<Entry[]>{
  pageNumber = 1;
  pageSize = 10;

  constructor(
    private entryService: EntryService,
    private phonebookService: PhonebookService,
    private authService: AuthService,
    private router: Router,
    private alertify: AlertifyService){

  }

  resolve(route: ActivatedRouteSnapshot): Observable<Entry[]> {

    return this.phonebookService.getPhonebook(this.authService.decodedToken.nameid)
    .pipe(
      flatMap(data => {
        console.log(data);
        return this.entryService.getEntries(data.id, this.pageNumber, this.pageSize)
      .pipe(catchError(error => {
          this.alertify.error('Problem retrieving data');
          this.router.navigate(['/home']);
          return of(null); // return to get out.
        }));
      }
      )
    );
  }
}
