import { Component, OnInit } from '@angular/core';
import { UserService } from '../../services/user.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-users',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './users.component.html'
})
export class UsersComponent implements OnInit {
  users: any[] = [];
  isAdmin = false;

  constructor(private userService: UserService) {
    this.isAdmin = JSON.parse(sessionStorage.getItem('user') || '{}').role === 'Admin';
  }

  ngOnInit() {
    if (this.isAdmin) {
      this.loadUsers();
    }
  }

  loadUsers() {
    this.userService.getUsers().subscribe(res => {
      this.users = res;
    });
  }

  approveUser(id: string) {
    this.userService.approveUser(id).subscribe(() => {
      this.loadUsers();
    });
  }

  updateRole(id: string, event: Event) {
    const role = (event.target as HTMLSelectElement).value;
    this.userService.updateRole(id, role).subscribe(() => {
      this.loadUsers();
    });
  }
}
