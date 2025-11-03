import { Component } from '@angular/core';
import { MatIcon } from '@angular/material/icon';

interface Feature {
  icon: string;
  title: string;
  description: string;
}

@Component({
  selector: 'app-about',
  imports: [
    MatIcon
  ],
  templateUrl: './about.component.html',
  styleUrl: './about.component.scss'
})
export class AboutComponent {
  features: Feature[] = [
    {
      icon: 'schedule',
      title: 'Automated Booking',
      description: 'Streamline appointment scheduling, reducing no-shows and eliminating the need for manual logs and paper files.',
    },
    {
      icon: 'pets',
      title: 'Digital Service Tracker',
      description: 'Maintain comprehensive, digital service records for every pet, ensuring transparency and professional care history management.',
    },
    {
      icon: 'contact_support',
      title: 'Enhanced Communication',
      description: 'Move away from outdated SMS apps to provide fast, reliable, and transparent client communication.',
    },
  ];
}
