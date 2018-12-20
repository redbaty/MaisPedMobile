import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {FormsModule} from '@angular/forms';
import {Routes, RouterModule} from '@angular/router';

import {IonicModule} from '@ionic/angular';

import {OrderPage} from './order.page';
import {Ng2BRPipesModule} from 'ng2-brpipes';

const routes: Routes = [
    {
        path: '',
        component: OrderPage
    }
];

@NgModule({
              imports: [
                  CommonModule,
                  FormsModule,
                  IonicModule,
                  Ng2BRPipesModule,
                  RouterModule.forChild(routes)
              ],
              declarations: [OrderPage]
          })
export class OrderPageModule {
}
