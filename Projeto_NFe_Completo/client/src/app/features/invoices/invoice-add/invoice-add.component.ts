import { OnInit } from '@angular/core/src/metadata/lifecycle_hooks';
import { InvoiceRegisterCommand } from './../shared/invoice.model';
import { Router, ActivatedRoute } from '@angular/router';
import { InvoiceService } from './../shared/invoice.service';
import { FormGroup, Validators, FormBuilder, FormControl, FormArray } from '@angular/forms';
import { Component } from '@angular/core';

@Component({
    templateUrl: './invoice-add.component.html',
})

export class InvoiceAddComponent implements OnInit {

    public title: string = 'Adicionar Nota Fiscal';
    public isLoading: boolean = false;

    public invoiceItemFormGroup: FormGroup;

    public form: FormGroup = this.fb.group({
        natureOperation: ['', Validators.required],
        number: ['', Validators.required],
        receiver: new FormControl(null, [Validators.required]),
        carrier: new FormControl(null, []),
        sender: new FormControl(null, [Validators.required]),
        invoiceItems: this.fb.array([]),
    }, {});

    constructor(
        private fb: FormBuilder,
        private invoiceService: InvoiceService,
        private router: Router,
        private route: ActivatedRoute,
    ) {}

    public ngOnInit(): void {
        this.invoiceItemFormGroup = this.createInvoiceItem();
    }

    public onSubmit(formModel: FormGroup): void {
        this.isLoading = true;

        const registerCommand: InvoiceRegisterCommand = new InvoiceRegisterCommand(formModel.value);

        this.invoiceService
            .post(registerCommand)
            .take(1)
            .subscribe((id: number) => {
                this.isLoading = false;

                if (id > 0) {
                    this.redirect();
                } else {
                    alert('Não foi possível cadastrar o registro');
                }
            });
    }

    public createInvoiceItem(): FormGroup {
        return this.fb.group({
            product: new FormControl(null, [Validators.required]),
            amount: ['', [Validators.required]],
        });
    }

    public onAddProduct(): void {
        const items: FormArray = this.form.get('invoiceItems') as FormArray;
        items.push(this.invoiceItemFormGroup);

        this.invoiceItemFormGroup = this.createInvoiceItem();
    }

    public redirect(): void {
        this.router.navigate(['../'],  { relativeTo: this.route });
    }

}
