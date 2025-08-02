import { Component } from '@angular/core';
import { MatCard } from "@angular/material/card";

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [MatCard],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent {

}
