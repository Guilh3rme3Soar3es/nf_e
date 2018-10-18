import { Router, ActivatedRoute } from '@angular/router';
import { InvoiceService, InvoiceResolveService } from './../../shared/invoice.service';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { Subject } from 'rxjs/Subject';
import { Invoice, InvoiceUpdateCommand } from './../../shared/invoice.model';
import { Component, OnInit } from '@angular/core';

@Component({
    templateUrl: './invoice-edit.component.html',
})

export class InvoiceEditComponent implements OnInit {
    private static CODE_MAX_LENGTH_STATE: number = 14;
    private static DESCRIPTION_MAX_LENGTH_STATE: number = 60;
    private static CURRENT_VALUE_MIN_VALUE_STATE: number = 1;
    public isLoading: boolean = false;

    private invoice: Invoice;
    private ngUnsubscribe: Subject<void> = new Subject<void>();

    private form: FormGroup = this.fb.group({
        code: ['', [Validators.required, Validators.maxLength(InvoiceEditComponent.CODE_MAX_LENGTH_STATE)]],
        description: ['', [Validators.required, Validators.maxLength(InvoiceEditComponent.DESCRIPTION_MAX_LENGTH_STATE)]],
        currentValue: ['', [Validators.required, Validators.min(InvoiceEditComponent.CURRENT_VALUE_MIN_VALUE_STATE)]],
    });

    constructor(
        private fb: FormBuilder,
        private invoiceService: InvoiceService,
        private resolver: InvoiceResolveService,
        private router: Router,
        private route: ActivatedRoute,
    ) {}

    public ngOnInit(): void {
        this.resolver.onChanges
            .takeUntil(this.ngUnsubscribe)
            .subscribe((invoice: Invoice) => {
                this.invoice = Object.assign(new Invoice(), invoice);
                this.form.setValue({
                    code: this.invoice.invoiceNumber,
                    natureOperation: this.invoice.natureOperation,
                    receiver: {
                        id: this.invoice.receiver.id,
                        name: this.invoice.receiver.name,
                    },
                    carrier: {
                        id: this.invoice.carrier.id,
                        name: this.invoice.carrier.name,
                    },
                    sender: {
                        id: this.invoice.sender.id,
                        fancyName: this.invoice.sender.fancyName,
                    },
                });
            });
    }

    public onSubmit(formModel: FormGroup): void {
        this.isLoading = true;

        const updateCommand: InvoiceUpdateCommand = new InvoiceUpdateCommand(this.invoice.id, formModel.value);

        this.invoiceService
            .put(updateCommand)
            .take(1)
            .subscribe((success: boolean) => {
                this.isLoading = false;

                if (success) {
                    this.resolver.resolveFromRouteAndNotify();
                    this.redirect();
                } else {
                    alert('Não foi possível atualizar o registro');
                }
            });
    }

    public redirect(): void {
        this.router.navigate(['../'],  { relativeTo: this.route });
    }

}
