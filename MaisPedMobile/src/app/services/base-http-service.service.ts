import { Injectable } from '@angular/core';
import {AuthenticationService} from './authentication.service';
import wretch, {Wretcher} from 'wretch';

@Injectable({
  providedIn: 'root'
})
export class BaseHttpService {

  constructor(protected authenticationService: AuthenticationService) { }

  protected getAuthenticatedWretch(): Promise<Wretcher> {
    return this.authenticationService.getToken().then(x => {
      return wretch().auth(`Bearer ${x}`);
    });
  }
}
