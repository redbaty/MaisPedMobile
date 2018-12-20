import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AddressDictionaryService {

  public serverAddress = 'http://192.168.0.33:6580/';
  public apiAddress = `${this.serverAddress}api/`;

  constructor() { }
}
