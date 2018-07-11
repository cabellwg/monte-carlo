import {Router} from 'aurelia-router';

export class App {
  router: Router;
  routerConfig(config, router){
    config.title = 'Aurelia Router';
    config.map([
      {route: '', name: 'home', moduleId: 'form/form', nav: true, title: 'Home'},
      {route: 'results', name: 'results', moduleId: 'results/results', nav: true, title: 'Results'}
    ]);
    this.router = router;
  }
}
