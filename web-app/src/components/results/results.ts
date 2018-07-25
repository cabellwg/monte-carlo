import { Data } from 'resources/scripts/data';
import { inject } from 'aurelia-framework';
import { EventAggregator } from 'aurelia-event-aggregator';
import { Router } from 'aurelia-router';

@inject(EventAggregator, Router)
export class Results {
  data: Data = Data.instance;
}
