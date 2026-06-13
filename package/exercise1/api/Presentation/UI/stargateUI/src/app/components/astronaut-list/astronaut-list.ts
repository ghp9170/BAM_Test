import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { AstronautService, PersonResponse } from '../../astronaut.service';
import { CommonModule } from '@angular/common';
import { TableModule } from 'primeng/table';

@Component({
  selector: 'app-astronaut-list',
  imports: [CommonModule, TableModule],
  templateUrl: './astronaut-list.html',
  styleUrl: './astronaut-list.css',
})
export class AstronautList implements OnInit {
  astronauts! : Observable<PersonResponse> ;

  constructor(private service: AstronautService) {
  }
  ngOnInit(): void {
    this.astronauts = this.service.getPeople();
  }
}
