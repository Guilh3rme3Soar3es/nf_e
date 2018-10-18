import { InvoiceItemCommand } from './../invoice-product/shared/item.model';
import { InvoiceTax } from './invoice-tax.model';
import { Receiver } from './../../receivers/shared/receiver.model';
import { Sender } from 'src/app/features/senders/shared/sender.model';
import { Carrier } from 'src/app/features/carriers/shared/carrier.model';
import { InvoiceItem, InvoiceItemRegisterCommand } from '../invoice-product/shared/invoice-item.model';

export class Invoice{
    public id: number;
    public natureOperation: string;
    public keyAccess: number;
    public number: number;
    public status: string;
    public entryDate: Date;
    public issueDate: Date;
    public invoiceTax: InvoiceTax;
    public sender: Sender;
    public receiver: Receiver;
    public carrier: Carrier;
    public invoiceItems: InvoiceItem[];
}

export class InvoiceRemoveCommand {

    public invoiceIds: number[];

    constructor(invoice: Invoice[]) {
        this.map(invoice);
    }

    private map(invoice: Invoice[]): void {
        this.invoiceIds = invoice.map((invoice: Invoice) => {
            return invoice.id;
        });
    }

}

export class InvoiceRegisterCommand {
    public natureOperation: string;
    public number: number;
    public status: string;
    public entryDate: Date;
    public senderId: number;
    public receiverId: number;
    public carrierId: number;
    public invoiceItems: InvoiceItemRegisterCommand[];

    constructor(value: Invoice) {
        this.map(value);
    }

    private map(value: Invoice): void {
        this.natureOperation = value.natureOperation;
        this.number = value.number;
        this.status = 'OPEN';
        this.entryDate = new Date('10/10/2018');
        this.senderId = value.sender ? value.sender.id : null;
        this.receiverId = value.receiver ? value.receiver.id : null;
        this.carrierId = value.carrier ? value.carrier.id : null;
        this.invoiceItems = [];

        if (value.invoiceItems) {
            value.invoiceItems.forEach((invoiceItem: any) => {
                this.invoiceItems.push(new InvoiceItemRegisterCommand(invoiceItem));
            });
        }
    }
}
export class InvoiceUpdateCommand {
    public id: number;
    public natureOperation: string;
    public keyAccess: number;
    public invoiceNumber: number;
    public status: string;
    public entryDate: Date;
    public issueDate: Date;
    public invoiceTax: InvoiceTax;
    public sender: Sender;
    public receiver: Receiver;
    public carrier: Carrier;
    public invoiceItems: InvoiceItem[];

    constructor(id: number, value: Invoice) {
        this.id = id;
        this.map(value);
    }

    private map(value: Invoice): void {
    this.natureOperation = value.natureOperation;
    this.keyAccess = value.keyAccess;
    this.invoiceNumber = value.invoiceNumber;
    this.status = value.status;
    this.entryDate = value.entryDate;
    this.issueDate = value.issueDate;
    this.invoiceTax = value.invoiceTax;
    this.sender = value.sender;
    this.receiver = value.receiver;
    this.carrier = value.carrier;
    this.invoiceItems = value.invoiceItems;
    }
}
