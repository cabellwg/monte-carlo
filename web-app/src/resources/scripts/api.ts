import {json} from 'aurelia-fetch-client';
import {Inputs} from './inputs';
import {Data} from './data';
import {HttpService} from './http-service';

export class APIRequest{
  static async postInputs(inputs: Inputs){
    let client = new HttpService();
    client.configureClient();
    

    return await client.fetch(json(inputs));
  }
}
