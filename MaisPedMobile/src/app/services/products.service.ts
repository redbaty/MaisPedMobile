import { Injectable } from '@angular/core';
import {AuthenticationService} from './authentication.service';
import {AddressDictionaryService} from './address-dictionary.service';
import {Person} from '../models/person';
import {BaseHttpService} from './base-http-service.service';
import {Product} from '../models/product';

@Injectable({
  providedIn: 'root'
})
export class ProductsService extends BaseHttpService {
  constructor(protected authenticationService: AuthenticationService, private address: AddressDictionaryService) {
    super(authenticationService);
  }

  getAll(): Promise<Person[]> {
    return new Promise<Person[]>((resolve, reject) => {
      this.getAuthenticatedWretch()
          .then(wretcher => {
            wretcher.url(`${this.address.apiAddress}products`)
                    .get()
                    .json(i => i as Product[])
                    .then(x => resolve(x));
          })
          .catch(e => reject(e));
    });
  }
}
