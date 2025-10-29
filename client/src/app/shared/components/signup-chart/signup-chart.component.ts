import { NgClass, NgStyle } from '@angular/common';
import { Component, computed, Input } from '@angular/core';

interface MonthlySignupData {
  month: string;
  users: number;
}

interface CalculatedBar extends MonthlySignupData {
  barHeight: number;
  barX: number;
  barY: number;
  barWidth: number;
}

@Component({
  selector: 'app-signup-chart',
  imports: [

  ],
  templateUrl: './signup-chart.component.html',
  styleUrl: './signup-chart.component.scss'
})
export class SignupChartComponent {
  @Input({ required: true }) signupsData: { month: string, users: number }[] = [];
  
  readonly width = 400;
  readonly height = 200;
  readonly padding = 20;

  // Computed properties for scaling
  maxUsers = computed(() => Math.max(...this.signupsData.map(d => d.users), 100) * 1.1); // Add a small buffer
  barSpacing = computed(() => this.width / this.signupsData.length);

  /**
   * Calculates the SVG pixel height for a given user count.
   * @param users The user count for a month.
   * @returns The height in pixels.
   */
  calculateHeight(users: number): number {
    return (users / this.maxUsers()) * this.height;
  }

  calculatedBars = computed<CalculatedBar[]>(() => {
    const data = this.signupsData;
    const maxUsers = this.maxUsers();
    const barSpacing = this.barSpacing();
    const height = this.height;
    const padding = this.padding;

    return data.map((d, index) => {
      // 1. Calculate height of the bar based on user count
      const barHeight = (d.users / maxUsers) * height;
      
      // 2. Calculate horizontal position and width
      const barWidth = barSpacing * 0.6; // 60% of the spacing for the bar
      const barX = padding + index * barSpacing;
      
      // 3. Calculate vertical position (SVG Y-origin is top-left, so we subtract height)
      const barY = height + padding - barHeight;

      return {
        ...d,
        barHeight,
        barX: barX + barSpacing * 0.2, // Offset to center the bar
        barY,
        barWidth,
      };
    });
  });
}
