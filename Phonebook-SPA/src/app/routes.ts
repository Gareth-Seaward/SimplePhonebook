import {Routes} from '@angular/router';
import { HomeComponent } from './home/home.component';
import { PhonebookComponent } from './phonebook/phonebook.component';
import { EntiryListComponent } from './entries/entiry-list/entiry-list.component';
import { AuthGuard } from './_guards/auth.guard';
import { PhonebookResolver } from './_resolvers/phonebook.resolver';
import { EntryListResolver } from './_resolvers/entry-list.resolver';
import { EntryItemComponent } from './Entries/Entry-Item/Entry-Item.component';
import { EntryItemResolver } from './_resolvers/entry-item.resolver';

export const appRoutes: Routes = [
  { path: '', component: HomeComponent},
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate:[AuthGuard],
    children:[
      { path: 'phonebook', component: PhonebookComponent, resolve: {phonebook: PhonebookResolver}},
      { path: 'entries/:id', component: EntryItemComponent},
      { path: 'entries', component: EntiryListComponent, resolve:{entries: EntryListResolver}}]
  },
  { path: '**', redirectTo: '', pathMatch: 'full'}
];
