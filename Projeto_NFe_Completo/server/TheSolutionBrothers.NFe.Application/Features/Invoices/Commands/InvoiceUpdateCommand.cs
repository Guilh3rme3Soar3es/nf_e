using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheSolutionBrothers.NFe.Domain.Features.Invoices;

namespace TheSolutionBrothers.NFe.Application.Features.Invoices.Commands
{
	public class InvoiceUpdateCommand
	{
		public long Id { get; set; }
		public string NatureOperation { get; set; }
		public int Number { get; set; }
        public InvoiceStatus Status { get; set; }
		public DateTime EntryDate { get; set; }
		public long SenderId { get; set; }
		public long ReceiverId { get; set; }
		public long CarrierId { get; set; }
		public IList<InvoiceItemRegisterCommand> unattachedItems { get; set; }
        public IList<InvoiceItemUpdateCommand> attachedItems { get; set; }

        public InvoiceUpdateCommand()
		{
		}

		public ValidationResult Validate()
		{
			ValidationResult InvoiceItemsValidationResult = null;
			if (this.attachedItems != null && this.attachedItems.Count > 0)
			{
				foreach (var item in attachedItems)
				{
					InvoiceItemsValidationResult = item.Validate();

					if (!InvoiceItemsValidationResult.IsValid)
						return InvoiceItemsValidationResult;
				}
			}

            InvoiceItemsValidationResult = null;
            if (this.unattachedItems != null && this.unattachedItems.Count > 0)
            {
                foreach (var item in unattachedItems)
                {
                    InvoiceItemsValidationResult = item.Validate();

                    if (!InvoiceItemsValidationResult.IsValid)
                        return InvoiceItemsValidationResult;
                }
            }

            return new Validator().Validate(this);
		}

		class Validator : AbstractValidator<InvoiceUpdateCommand>
		{
			public Validator()
			{
                RuleFor(r => r.Id)
                   .GreaterThan(0).WithMessage("Nota fiscal com identificador inválido.");
                
                RuleFor(c => c.ReceiverId)
					.GreaterThan(0).WithMessage("Nota fiscal com identificador inválido.");

				RuleFor(r => r.NatureOperation)
					.NotEmpty().WithMessage("Nota fiscal com natureza de operação não informado.")
					.MaximumLength(70).WithMessage("Nota fiscal com natureza de operação maior que 70 caracteres.");

				RuleFor(r => r.Number)
					.GreaterThan(0).WithMessage("Nota fiscal com numero inválido.");

				RuleFor(r => r.EntryDate)
					.NotNull().WithMessage("Nota fiscal com data de entrada nula.")
                    .LessThan(DateTime.Now).WithMessage("Nota fiscal com data de entrada maior que a data atual.");

				RuleFor(c => c.ReceiverId)
					.GreaterThan(0).WithMessage("Nota fiscal com destinatário com identificador inválido.");

				RuleFor(c => c.CarrierId)
					.GreaterThan(0).WithMessage("Nota fiscal com transportador com identificador inválido.");

				RuleFor(c => c.SenderId)
					.GreaterThan(0).WithMessage("Nota fiscal com emitente com identificador inválido.");

			}
		}
	}
}
