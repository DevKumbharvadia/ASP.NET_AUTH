import { Component, inject, OnInit } from '@angular/core';
import { Router, RouterLink, RouterOutlet } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-page-layout',
  standalone: true,
  imports: [RouterLink, RouterOutlet],
  templateUrl: './page-layout.component.html',
  styleUrl: './page-layout.component.css'
})
export class PageLayoutComponent {
  authServices = inject(AuthService);
  router = inject(Router);

  userRoles:any;

  ngOnInit(): void {
    //roles ok
    this.authServices.getUserRoles()
    this.userRoles = '';
    this.userRoles = this.authServices.USER_ROLES;
    if(!this.userRoles){
      this.userRoles = sessionStorage.getItem("UserRoles");
    }
  }

  onLogOff() {
    // Call the logout method from AuthService
    this.authServices.logout(); // No need to pass userId, logout handles it internally
  }


}
