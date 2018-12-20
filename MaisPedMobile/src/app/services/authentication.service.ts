import {Injectable} from '@angular/core';
import wretch from 'wretch';
import {AddressDictionaryService} from './address-dictionary.service';

@Injectable({
                providedIn: 'root'
            })
export class AuthenticationService {
    constructor(private address: AddressDictionaryService) {
    }

    public getToken(): Promise<string> {
        // TODO: Build device hash.
        const hash = 'browser';
        return wretch(`${this.address.apiAddress}Authentication/phone/hash/${hash}`)
            .get()
            .text();
    }
}
