import { Data } from './resources/scripts/data';
import { EventAggregator} from 'aurelia-event-aggregator';
import {PLATFORM} from 'aurelia-pal';
import {Router, RouterConfiguration} from 'aurelia-router';
import { inject } from 'aurelia-framework';


@inject(EventAggregator)
export class App {

  router: Router;
  configureRouter(config: RouterConfiguration, router: Router){
    config.title = 'Aurelia Router';
    config.options.pushState = true;
    config.map([
      {route: ['', 'form'], name: 'home', moduleId: PLATFORM.moduleName('components/input-formfield/form'), nav: true, title: 'Home'},
      {route: 'results', name: 'results', moduleId: PLATFORM.moduleName('components/results/results'), nav: true, title: 'Results'}
    ]);
    this.router = router;
  }

  


}
