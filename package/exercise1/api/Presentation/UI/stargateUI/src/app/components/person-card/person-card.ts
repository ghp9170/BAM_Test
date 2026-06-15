import { Component, Input, OnInit, inject, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CreateAstronautDetail } from '../../models/CreateAstronautDetail';

@Component({
  selector: 'app-person-card',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './person-card.html',
  changeDetection: ChangeDetectionStrategy.Eager,
  styleUrls: ['./person-card.scss'],
})
export class PersonCard {
  @Input() astronaut: any;

  ngOnInit() {}
}
