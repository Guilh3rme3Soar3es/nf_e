import { Component, Input, EventEmitter, Output, OnInit, OnDestroy } from '@angular/core';
import { FormGroup, FormArray } from '@angular/forms';
import { Subject } from 'rxjs/Subject';

import { ProductService } from '../../../products/shared/product.service';
import { Product } from './../../../products/shared/product.model';

@Component({
    selector: 'ndd-invoice-item-form',
    templateUrl: './invoice-item-form.component.html',
})
export class InvoiceItemFormComponent implements OnInit, OnDestroy {

    @Input() public formModelItem: FormGroup;
    @Input() public invoiceItems: FormArray;

    @Output() public submit: EventEmitter<FormGroup> = new EventEmitter<FormGroup>();

    public productData: Product[] = [];

    private onProductFilterChange: Subject<string> = new Subject<string>();
    private ngUnsubscribeProduct: Subject<void> = new Subject<void>();

    //public invoiceItems: InvoiceItem[];

    constructor(public productService: ProductService) {

    }

    public ngOnInit(): void {
        const timeDelay: number = 300;

        this.onProductFilterChange
            .takeUntil(this.ngUnsubscribeProduct)
            .debounceTime(timeDelay)
            .switchMap((value: string) => this.productService.queryByDescription(value))
            .subscribe((response: any) => {
                this.productData = response;
            });
    }

    public ngOnDestroy(): void {
        this.ngUnsubscribeProduct.next();
        this.ngUnsubscribeProduct.complete();
    }

    public onProductAutoCompleteChange(value: string): void {
        this.onProductFilterChange.next(value);
    }

    public onSubmit(event: Event): void {
        event.stopPropagation();
        this.submit.emit(this.formModel);
    }

    public deleteProduct(): void {}

}
