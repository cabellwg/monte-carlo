import {HttpClient} from 'aurelia-fetch-client';

export class HttpService{
  endpoint: string = "http://localhost:54126/api";
  client: HttpClient;

  configureClient(){
    let client = new HttpClient;
    client.configure(config => {
      config
        .withBaseUrl(this.endpoint)
        .withDefaults({
          mode: 'cors',
          headers: {
            'Access-Control-Allow-Headers': '*',
            'Access-Control-Allow-Origin': '*',
            'Content-Type': 'application/json; charset=utf-8'
          }
        })
    });

    this.client = client;
  }

  async fetch(body){
    return await this.client.fetch(this.endpoint,{
      method: "POST",
      body: body
    }).then(r =>{
      let results = r.json();
      return results;
    })
  }

}
