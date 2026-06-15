import { Component, OnInit, HostListener, ViewChild, ChangeDetectionStrategy } from '@angular/core';
import { Observable } from 'rxjs';
import { PersonService } from './person.service';
import { CommonModule } from '@angular/common';
import { TableModule } from 'primeng/table';
import { PersonCard } from '../person-card/person-card';
import { RocketLaunch } from '../rocket-launch/rocket-launch';
import { PersonResponse } from '../../models/PersonResponse';

@Component({
  selector: 'app-person-list',
  imports: [CommonModule, TableModule, PersonCard, RocketLaunch],
  templateUrl: './person-list.html',
  changeDetection: ChangeDetectionStrategy.Eager,
  styleUrl: './person-list.css',
})
export class PersonList implements OnInit {
  astronauts!: Observable<PersonResponse>;
  @ViewChild(RocketLaunch) rocketLaunch!: RocketLaunch;

  constructor(private service: PersonService) {}

  ngOnInit(): void {
    this.astronauts = this.service.getPeople();
  }

  onCardClick(person: any) {
    if (person.careerStartDate) {
      this.rocketLaunch.launchRocket('modify', person);
    } else {
      this.rocketLaunch.launchRocket('create', person);
    }
  }

  refreshRoster() {
    this.astronauts = this.service.getPeople();
  }

  @HostListener('window:mousemove', ['$event'])
  onMouseMove(event: MouseEvent) {
    // Calculate cursor distance from the center of the window
    const mouseX = event.clientX - window.innerWidth / 2;
    const mouseY = event.clientY - window.innerHeight / 2;
    document.body.style.setProperty('--mouse-x', `${mouseX}px`);
    document.body.style.setProperty('--mouse-y', `${mouseY}px`);
  }
}
