import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from '@angular/router';
import { forkJoin, Observable } from 'rxjs';
import { UserService } from './shared/services/user.service';
import { AuthService } from './core/auth/auth.service';

@Injectable({
    providedIn: 'root'
})
export class UserDataResolver implements Resolve<any>
{
    /**
     * Constructor
     */
    constructor(
        private _userService: UserService,
        private _authService: AuthService
    )
    {
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Use this resolver to resolve initial mock-api for the application
     *
     * @param route
     * @param state
     */
    resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<any>
    {
        // Fork join multiple API endpoint calls to wait all of them to finish
        return forkJoin([
            this._userService.getUser(this._authService.userValue.Id)
        ]);
    }
}
