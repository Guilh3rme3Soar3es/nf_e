import { FormArray } from '@angular/forms';
import { Component, Input } from '@angular/core';

@Component({
    templateUrl: './invoice-item-list.component.html',
    selector: 'ndd-invoice-item-list',
})
export class InvoiceItemListComponent {

    @Input() public invoiceItems: FormArray;

}
