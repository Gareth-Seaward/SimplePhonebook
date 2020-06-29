import {Routes} from '@angular/router';
import { HomeComponent } from './home/home.component';
import { PhonebookComponent } from './phonebook/phonebook.component';
import { EntiryListComponent } from './entries/entiry-list/entiry-list.component';
import { AuthGuard } from './_guards/auth.guard';
import { PhonebookResolver } from './_resolvers/phonebook.resolver';

export const appRoutes: Routes = [
  { path: '', component: HomeComponent},
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate:[AuthGuard],
    children:[
      { path: 'phonebook', component: PhonebookComponent, resolve: {phonebook: PhonebookResolver}},
      { path: 'entries', component: EntiryListComponent}]
  },
  { path: '**', redirectTo: '', pathMatch: 'full'}
];
