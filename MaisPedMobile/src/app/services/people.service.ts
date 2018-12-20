import {Injectable} from '@angular/core';
import {BaseHttpService} from './base-http-service.service';
import {AuthenticationService} from './authentication.service';
import {Person} from '../models/person';
import {AddressDictionaryService} from './address-dictionary.service';

@Injectable({
                providedIn: 'root'
            })
export class PeopleService extends BaseHttpService {
    constructor(protected authenticationService: AuthenticationService, private address: AddressDictionaryService) {
        super(authenticationService);
    }

    getAll(): Promise<Person[]> {
        return new Promise<Person[]>((resolve, reject) => {
            this.getAuthenticatedWretch()
                .then(wretcher => {
                    wretcher.url(`${this.address.apiAddress}people`)
                            .get()
                            .json(i => i as Person[])
                            .then(x => resolve(x));
                })
                .catch(e => reject(e));
        });
    }
}
