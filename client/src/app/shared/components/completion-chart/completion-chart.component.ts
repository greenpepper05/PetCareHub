import { Component, computed, input, Input } from '@angular/core';

const STROKE_WIDTH = 12;
const RADIUS = 60;
const CIRCUMFERENCE = 2 * Math.PI * RADIUS;

@Component({
  selector: 'app-completion-chart',
  imports: [

  ],
  templateUrl: './completion-chart.component.html',
  styleUrl: './completion-chart.component.scss'
})
export class CompletionChartComponent {
  
  readonly STROKE_WIDTH = STROKE_WIDTH;
  readonly RADIUS = RADIUS;
  readonly CIRCUMFERENCE = CIRCUMFERENCE;

  percentage = input.required<number>(); 
  
  // Computed property to calculate the SVG stroke offset
  strokeDashoffset = computed(() => {
    // Access the input as a signal function call
    const progress = this.percentage(); 
    // Calculation: Total circumference minus the length of the completed arc
    const offset = this.CIRCUMFERENCE - (progress / 100) * this.CIRCUMFERENCE;
    return offset;
  });
}
