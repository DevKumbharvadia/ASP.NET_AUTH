import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, RouterLink, RouterLinkActive],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
})
export class LoginComponent {
  loginObj: any = {
    username: '',
    password: '',
  };

  authService = inject(AuthService);
  router = inject(Router);

  onLogin() {
    this.authService.login(this.loginObj.username, this.loginObj.password).subscribe(
      (res: any) => {
        if (res && res.success) {
          alert('Login Success');
          this.router.navigateByUrl('layout/home').then(() => 
            window.location.reload()
          );
        } else {
          alert(res.message || 'Login failed');
        }
      },
      (error) => {
        console.error('Error:', error); // Log error for debugging
        alert('Error in login request');
      }
    );
  }
}
