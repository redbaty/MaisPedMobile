import {Injectable} from '@angular/core';
import {BaseHttpService} from './base-http-service.service';
import {AuthenticationService} from './authentication.service';
import {AddressDictionaryService} from './address-dictionary.service';
import {Order} from '../models/order';

@Injectable({
                providedIn: 'root'
            })
export class OrderService extends BaseHttpService {
    constructor(protected authenticationService: AuthenticationService, private address: AddressDictionaryService) {
        super(authenticationService);
    }

    add(order: Order) {
        return this.getAuthenticatedWretch()
                   .then(wretch => {
                       return wretch.url(`${this.address.apiAddress}order`)
                                    .post(order);
                   });
    }
}
