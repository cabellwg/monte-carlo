import {PLATFORM} from 'aurelia-pal';
import {RouterConfiguration, Router} from 'aurelia-router';
import {Script} from 'vm';

export class App {
  router:Router;
  routerConfiguration(config, router){
    config.title= 'Monte Carlo';
    config.options.pushState = true;
    config.map([
      {route: ['','form'], name: 'form', moduleId: PLATFORM.moduleName('form/form'), nav: true, title: 'Home'},
      {route: 'results', name: 'results', moduleId: PLATFORM.moduleName('results/results'), nav: true, title: 'Results'}
    ]);
    this.router = router;
  }
}
