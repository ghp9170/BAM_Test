import { Routes } from '@angular/router';
import { PersonList } from './components/person-list/person-list';

export const routes: Routes = [
  { path: '', component: PersonList },

  // You can add a wildcard route here later for 404 pages:
  // { path: '**', redirectTo: '' }
];
