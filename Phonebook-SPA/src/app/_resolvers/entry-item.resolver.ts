import { Resolve, ActivatedRouteSnapshot, Router } from '@angular/router';
import { Entry } from '../_models/entry';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { EntryService } from '../_services/entry.service';
import { catchError } from 'rxjs/operators';
import { AlertifyService } from '../_services/alertify.service';

@Injectable({ providedIn: 'root' })
export class EntryItemResolver implements Resolve<Entry> {

  constructor(
    private entryService: EntryService,
    private alertify: AlertifyService,
    private router: Router){
  }

  resolve(route: ActivatedRouteSnapshot): Observable<Entry> {
    const user = JSON.parse(localStorage.getItem('user'));
    return this.entryService.getEntry(user.phonebookId, route.params.id)
    .pipe(catchError(error => {
      this.alertify.error('Problem fetching data');
      this.router.navigate(['/home']);
      return of(null);
    }))
  }
}
