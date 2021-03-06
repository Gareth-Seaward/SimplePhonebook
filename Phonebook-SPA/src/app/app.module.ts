import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import {HttpClientModule} from '@angular/common/http';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';

import { AppComponent } from './app.component';
import { PhonebookComponent } from './phonebook/phonebook.component';
import { NavComponent } from './nav/nav.component';
import { AuthService } from './_services/auth.service';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';
import { ErrorInterceptorsProvider } from './_services/error.interceptor';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { ButtonsModule } from 'ngx-bootstrap/buttons';
import { EntiryListComponent } from './entries/entiry-list/entiry-list.component';
import { RouterModule } from '@angular/router';
import { appRoutes } from './routes';
import { JwtModule } from '@auth0/angular-jwt';
import { EntryItemComponent } from './Entries/Entry-Item/Entry-Item.component';
import { EntryNewComponent } from './Entries/entry-new/entry-new.component';

export function tokenGetter() {
  return localStorage.getItem('token');
}
@NgModule({
   declarations: [
      AppComponent,
      PhonebookComponent,
      NavComponent,
      HomeComponent,
      RegisterComponent,
      EntiryListComponent,
      EntryItemComponent,
      EntryNewComponent
   ],
   imports: [
      BrowserModule,
      HttpClientModule,
      FormsModule,
      ReactiveFormsModule,
      BrowserAnimationsModule,
      BsDropdownModule.forRoot(),
      PaginationModule.forRoot(),
      ButtonsModule.forRoot(),
      RouterModule.forRoot(appRoutes),
      JwtModule.forRoot({
        config: {
          // tslint:disable-next-line: object-literal-shorthand
          tokenGetter: tokenGetter,
          whitelistedDomains: ['localhost:5000'],
          blacklistedRoutes: ['localhost:5000/v1/auth']
        }
      })
   ],
   providers: [
      AuthService,
      ErrorInterceptorsProvider
   ],
   bootstrap: [
      AppComponent
   ]
})
export class AppModule { }
