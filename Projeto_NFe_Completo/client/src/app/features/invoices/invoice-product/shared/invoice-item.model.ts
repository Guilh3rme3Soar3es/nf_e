import { Invoice } from '../../shared/invoice.model';
import { Product } from 'src/app/features/products/shared/product.model';

export class InvoiceItem {

    public id: number;
    public invoice: Invoice;
    public product: Product;
    public amount: number;

}

export class InvoiceItemRegisterCommand {

    public productId: number;
    public amount: number;

    constructor(value: any) {
        this.productId = value.product ? value.product.id : null;
        this.amount = value.amount;
    }
}
