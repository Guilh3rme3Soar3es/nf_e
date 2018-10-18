import { NgModule } from '@angular/core';

import { DropDownsModule } from '@progress/kendo-angular-dropdowns';
import { GridModule } from '@progress/kendo-angular-grid';

import { SharedModule } from '../../../shared/shared.module';

import { InvoiceItemFormComponent } from './invoice-item-form/invoice-item-form.component';
import { InvoiceItemListComponent } from './invoice-item-list/invoice-item-list.component';
import { ProductSharedModule } from '../../products/shared/product-shared.module';

@NgModule({
    declarations: [
        InvoiceItemFormComponent,
        InvoiceItemListComponent,
    ],
    imports: [
        SharedModule,
        DropDownsModule,
        GridModule,
        ProductSharedModule,
    ],
    exports: [
        InvoiceItemFormComponent,
        InvoiceItemListComponent,
    ],
    providers: [

    ],
})
export class InvoiceItemModule {}
