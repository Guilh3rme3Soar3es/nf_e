using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheSolutionBrothers.NFe.Application.Features.Invoices.Commands;
using TheSolutionBrothers.NFe.Application.Features.Invoices.ViewModels;
using TheSolutionBrothers.NFe.Domain.Features.InvoiceItems;
using TheSolutionBrothers.NFe.Domain.Features.Invoices;

namespace TheSolutionBrothers.NFe.Application.Features.Invoices
{
    public class InvoiceItemCustomResolver : IValueResolver<InvoiceUpdateCommand, Invoice, IList<InvoiceItem>>
    {
        public IList<InvoiceItem> Resolve(InvoiceUpdateCommand source, Invoice destination, IList<InvoiceItem> destMember, ResolutionContext context)
        {
            List<InvoiceItem> items = new List<InvoiceItem>();
            foreach (var item in source.attachedItems)
            {
                items.Add(Mapper.Map<InvoiceItem>(item));
            }
            foreach (var item in source.unattachedItems)
            {
                items.Add(Mapper.Map<InvoiceItem>(item));
            }
            return items;
        }
    }
}
