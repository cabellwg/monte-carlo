import {PLATFORM} from 'aurelia-pal';
import {Router} from 'aurelia-router';

export class App {
  router: Router;
  routerConfig(config, router){
    config.title = 'Aurelia Router';
    config.options.pushState = true;
    config.map([
      {route: ['', 'form'], name: 'home', moduleId: PLATFORM.moduleName('./components/input-formfield/form.html'), nav: true, title: 'Home'},
      {route: 'results', name: 'results', moduleId: PLATFORM.moduleName('./components/results/results.html'), nav: true, title: 'Results'}
    ]);
    this.router = router;
  }
}
