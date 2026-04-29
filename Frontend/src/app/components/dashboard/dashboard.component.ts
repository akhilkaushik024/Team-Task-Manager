import { Component, OnInit } from '@angular/core';
import { DashboardService } from '../../services/dashboard.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './dashboard.component.html'
})
export class DashboardComponent implements OnInit {
  stats: any;
  constructor(private dashboardService: DashboardService) {}
  ngOnInit() {
    this.dashboardService.getStats().subscribe(res => this.stats = res);
  }
  getCompletionRate(): number {
    if (!this.stats || !this.stats.totalTasks) return 0;
    return Math.round((this.stats.completed / this.stats.totalTasks) * 100);
  }
}
