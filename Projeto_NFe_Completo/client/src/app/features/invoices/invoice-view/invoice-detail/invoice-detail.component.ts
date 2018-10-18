import { Router, ActivatedRoute } from '@angular/router';
import { InvoiceResolveService } from './../../shared/invoice.service';
import { Subject } from 'rxjs/Subject';
import { Invoice } from './../../shared/invoice.model';
import { Component, OnInit, OnDestroy } from '@angular/core';

@Component({
    templateUrl: './invoice-detail.component.html',
})

export class InvoiceDetailComponent implements OnInit, OnDestroy{
    public isLoading: boolean = false;

    private invoice: Invoice;
    private ngUnsubscribe: Subject<void> = new Subject<void>();

    constructor(private resolver: InvoiceResolveService,
        private router: Router,
        private route: ActivatedRoute) {}

    public ngOnInit(): void {
        this.isLoading = true;
        this.resolver.onChanges
            .takeUntil(this.ngUnsubscribe)
            .subscribe((invoice: Invoice) => {
                this.isLoading = false;
                this.invoice = Object.assign(new Invoice(), invoice);
            });
    }

    public ngOnDestroy(): void {
        this.ngUnsubscribe.next();
        this.ngUnsubscribe.complete();
    }

    public formatDate(dateString: string): string {

        return new Date(dateString).toISOString();
    }

    public toStringStatus(status: string): string {

        if (status == '0') {
            return 'Aberta';
        }

        return 'Emitida';
    }

    public toStringNumber(numeral: number): string {

        return numeral.toString();
    }

    public onEdit(): void {
        this.router.navigate(['./editar'], { relativeTo: this.route });
    }

    public redirect(): void {
        this.router.navigate(['/notas'], { relativeTo: this.route });
    }
}
