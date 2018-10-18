import { NDDTabsbarModule } from '../../shared/ndd-ng-tabsbar/component/ndd-ng-tabsbar.module';
import { NDDTitlebarModule } from '../../shared/ndd-ng-titlebar/component/ndd-ng-titlebar.module';
import { SharedModule } from '../../shared/shared.module';
import { NgModule } from '@angular/core';

import { GridModule } from '@progress/kendo-angular-grid';
import { DropDownsModule } from '@progress/kendo-angular-dropdowns';

import { InvoiceRoutingModule } from './invoice-routing.module';
import { InvoiceSharedModule } from './shared/invoice-shared.module';
import { InvoiceResolveService } from './shared/invoice.service';
import { InvoiceAddComponent } from './invoice-add/invoice-add.component';
import { InvoiceListComponent } from './invoice-list/invoice-list.component';
import { InvoiceViewComponent } from './invoice-view/invoice-view.component';
import { InvoiceDetailComponent } from './invoice-view/invoice-detail/invoice-detail.component';
import { InvoiceEditComponent } from './invoice-view/invoice-edit/invoice-edit.component';
import { InvoiceFormComponent } from './invoice-form/invoice-form.component';
import { InvoiceItemModule } from './invoice-product/invoice-item.module';
import { SenderSharedModule } from '../senders/shared/sender-shared.module';
import { CarrierSharedModule } from '../carriers/shared/carrier-shared.module';
import { ReceiverSharedModule } from '../receivers/shared/receiver-shared.module';

@NgModule({
    declarations: [
        InvoiceListComponent,
        InvoiceDetailComponent,
        InvoiceViewComponent,
        InvoiceFormComponent,
        InvoiceEditComponent,
        InvoiceAddComponent,
    ],
    imports: [
        InvoiceRoutingModule,
        SharedModule,
        GridModule,
        DropDownsModule,
        NDDTabsbarModule,
        NDDTitlebarModule,
        InvoiceSharedModule,
        InvoiceItemModule,
        ReceiverSharedModule,
        CarrierSharedModule,
        SenderSharedModule,
    ],
    exports: [],
    providers: [InvoiceResolveService],
})
export class InvoiceModule {}
