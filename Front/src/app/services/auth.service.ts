import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable, of } from 'rxjs';
import { catchError, map, retry, tap } from 'rxjs/operators';
import { Token } from '@angular/compiler';
import { IRole } from '../model/interface';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private apiUrl = 'https://localhost:7250/api/Audit';
  private refreshTokenKey = 'refreshToken';
  private jwtTokenKey = 'jwtToken';
  private userIdKey = 'UserId';
  private userRolesKey = 'UserRoles';
  USER_ROLES:any;
  USER_ID:any;

  constructor(private http: HttpClient, private router: Router) {}

  // Get JWT token from storage
  getJwtToken() {
    return sessionStorage.getItem(this.jwtTokenKey);
  }

  // Get refresh token from storage
  getRefreshToken() {
    return sessionStorage.getItem(this.refreshTokenKey);
  }

  // Set JWT token in session storage
  setJwtToken(token: string) {
    sessionStorage.setItem(this.jwtTokenKey, token);
  }

  setUserId(data: any){
    sessionStorage.setItem(this.userIdKey, data)
    this.USER_ID = data;
  }

  setUserRoles(data: any){
    //ok
    sessionStorage.setItem(this.userRolesKey,data);
    this.USER_ROLES = data
  }

  getUserId(){
    //ok
    return sessionStorage.getItem(this.userIdKey);
  }

  // Set refresh token in session storage
  setRefreshToken(token: string) {
    sessionStorage.setItem(this.refreshTokenKey, token);
  }

  // Remove tokens on logout
  clearTokens() {
    sessionStorage.removeItem(this.jwtTokenKey);
    sessionStorage.removeItem(this.refreshTokenKey);
    sessionStorage.removeItem(this.userRolesKey);
    sessionStorage.removeItem(this.userIdKey);
  }


  // Refresh token function
  refreshToken(): Observable<any> {
    const refreshToken = this.getRefreshToken();
    if (!refreshToken) {
      this.logout(); // No refresh token, log out user
      return of(null);
    }

    return this.http.post(`${this.apiUrl}/refresh-token`, { refreshToken }).pipe(
      tap((response: any) => {
        if (response && response.success) {
          this.setJwtToken(response.data.JwtToken); // Update JWT token
          this.setRefreshToken(response.data.RefreshToken); // Update refresh token
        } else {
          this.logout(); // Token refresh failed, log out user
        }
      }),
      catchError(error => {
        console.error('Error refreshing token', error);
        this.logout(); // Log out in case of any error
        return of(null);
      })
    );
  }

  // Check if the token is expired or about to expire
  isTokenExpired(token: string): boolean {
    const expiry = JSON.parse(atob(token.split('.')[1])).exp;
    const now = Math.floor(new Date().getTime() / 1000);
    return now >= expiry;
  }

  // Automatically refresh token if it's about to expire
  autoRefreshToken(): Observable<boolean> {
    const token = this.getJwtToken();
    if (!token || this.isTokenExpired(token)) {
      // If the token is expired or missing, try to refresh
      return this.refreshToken().pipe(
        map(() => true),
        catchError(() => {
          this.logout();
          return of(false);
        })
      );
    }

    return of(true); // Token is valid
  }

  // Logout function
  logout() {
    const userId = this.getUserId();
    if (userId) {
      this.http.post(`https://localhost:7250/api/Audit/Logout?userId=${userId}`, {})
        .subscribe({
          next: (res: any) => {
            console.log('Logout successful:', res);  // Successful response handling
            this.clearTokens();  // Clear tokens upon successful logout
            this.router.navigateByUrl('/login');  // Redirect to login page
          },
          error: (err: any) => {
            console.error('Logout failed:', err);  // Handle any errors that occurred during logout

            // Optional: Display error message to the user (you can use a toast, modal, or alert)
            alert('An error occurred during logout. Please try again.');
          },
          complete: () => {
            console.log('Logout request completed');  // Optional: Log when the request is complete
          }
        });
    } else {
      console.log('No user ID found, clearing tokens and navigating to login');
      this.clearTokens();
      this.router.navigateByUrl('/login');
    }
  }


  login(username: string, password: string): Observable<any> {
    const loginObj = { username, password };
    return this.http.post(`https://localhost:7250/api/Audit/LoginUser`, loginObj).pipe(
      tap((response: any) => {
        if (response && response.success) {
          this.setJwtToken(response.data.jwtToken); // Assuming new JWT is in response.data.JwtToken
          this.setRefreshToken(response.data.RefreshToken); // Assuming refresh token is in response.data.RefreshToken
          this.setUserId(response.data.userId);
          this.getUserRoles();
        }
      }),
      catchError(error => {
        console.error('Login failed', error);
        alert("login failed check username or password")
        return of(null); // Handle error gracefully
      })
    );
  }

  onRegister(username: string, password: string, email: string, roleId: string): Observable<any>{
    const registerObj = {username, password, email, roleId}
    return this.http.post("https://localhost:7250/api/User/register", registerObj)
  }

  // Check if the user is logged in
  isLoggedIn(): boolean {
    return !!this.getJwtToken(); // Check if JWT token exists
  }

  getAllRoles(): Observable<any>{
    return this.http.get<any>("https://localhost:7250/api/Role/GetAllRoles");
  }

  getUserRoles(){
    this.http.get<any>(`https://localhost:7250/api/Role/GetUserRoles?userId=${sessionStorage.getItem(this.userIdKey)}`,{}).subscribe((res:any)=>{
      this.setUserRoles(res.data)
    })
  }
}
