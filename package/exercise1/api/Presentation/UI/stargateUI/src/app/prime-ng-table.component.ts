import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common'; // Required for *ngFor and other common directives
import { TableModule } from 'primeng/table'; // PrimeNG Table Module

interface MyData {
  value: number;
}

@Component({
  selector: 'app-prime-ng-table',
  standalone: true,
  imports: [CommonModule, TableModule], // Import necessary modules for standalone component
  template: `
    <div class="card">
      <h3>List of Numbers</h3>
      <p-table [value]="data" [tableStyle]="{ 'min-width': '20rem' }">
        <ng-template pTemplate="header">
          <tr>
            <th>Number</th>
          </tr>
        </ng-template>
        <ng-template pTemplate="body" let-rowData>
          <tr>
            <td>{{ rowData.value }}</td>
          </tr>
        </ng-template>
        <ng-template pTemplate="emptymessage">
          <tr>
            <td colspan="1">No data found.</td>
          </tr>
        </ng-template>
      </p-table>
    </div>
  `,
  styles: [`
    .card {
      margin: 20px auto;
      padding: 20px;
      border: 1px solid #ddd;
      border-radius: 8px;
      box-shadow: 0 2px 4px rgba(0,0,0,0.1);
      max-width: 400px; /* Limit width for better presentation */
    }
  `]
})
export class PrimeNgTableComponent implements OnInit {
  data: MyData[] = [];

  ngOnInit() {
    // Initialize the data for the table
    this.data = [
      { value: 1 },
      { value: 2 },
      { value: 34 }
    ];
  }
}