import { InvoiceResolveService } from './../shared/invoice.service';
import { Subject } from 'rxjs/Subject';
import { Invoice } from './../shared/invoice.model';
import { Component, OnInit, OnDestroy } from '@angular/core';

@Component({
    templateUrl: './invoice-view.component.html',
})

export class InvoiceViewComponent implements OnInit, OnDestroy {

    public invoice: Invoice;
    public infoItems: object[];
    public title: number;
    private ngUnsubscribe: Subject<void> = new Subject<void>();

    constructor(private resolver: InvoiceResolveService) {}

    public ngOnInit(): void {
        this.resolver.onChanges
            .takeUntil(this.ngUnsubscribe)
            .subscribe((invoice: Invoice) => {
                this.invoice = invoice;
                this.createProperty();
            });
    }

    public ngOnDestroy(): void {
        this.ngUnsubscribe.next();
        this.ngUnsubscribe.complete();
    }

    private createProperty(): void {
        this.title = this.invoice.invoiceNumber;
        const natureOperation: string = this.invoice.natureOperation;

        this.infoItems = [
            {
                value: natureOperation,
                description: natureOperation,
            },
        ];
    }

}
