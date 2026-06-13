import { Routes } from '@angular/router';
import { AstronautList } from './components/astronaut-list/astronaut-list';

export const routes: Routes = [
  { path: '', component: AstronautList },
  
  // You can add a wildcard route here later for 404 pages:
  // { path: '**', redirectTo: '' }
];
