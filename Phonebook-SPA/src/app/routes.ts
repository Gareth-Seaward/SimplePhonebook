import {Routes} from '@angular/router';
import { HomeComponent } from './home/home.component';
import { PhonebookComponent } from './phonebook/phonebook.component';
import { EntiryListComponent } from './entiry-list/entiry-list.component';
import { AuthGuard } from './_guards/auth.guard';

export const appRoutes: Routes = [
  { path: '', component: HomeComponent},
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate:[AuthGuard],
    children:[
      { path: 'phonebook', component: PhonebookComponent},
      { path: 'entries', component: EntiryListComponent}]
  },
  { path: '**', redirectTo: '', pathMatch: 'full'}
];
