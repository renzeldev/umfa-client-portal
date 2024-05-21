import {Injectable} from '@angular/core';
import {CanActivate, Router} from '@angular/router';
import { RoleType } from '@core/models';
import { UserService } from '@shared/services';


@Injectable()
export class UmfaAdministratorAuthGuard implements CanActivate {

  constructor(private router: Router, private userService: UserService) {
  }

  canActivate(): boolean {
    let roleId = this.userService.userValue ?  this.userService.userValue.RoleId : JSON.parse(localStorage.getItem('user')).RoleId;
    
    if(roleId == RoleType.UMFAAdministrator ) {
        return true;
    } else if (roleId == RoleType.UMFAOperator) {
        this.router.navigate(['/admin/meterMapping']);
        return false; 
    } else {
        this.router.navigate(['/project']);
        return false;
    }
    
  }
}