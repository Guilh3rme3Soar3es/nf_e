using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheSolutionBrothers.NFe.Application.Features.Invoices.Commands
{
	public class InvoiceDeleteCommand
	{
		public virtual long[] InvoiceIds { get; set; }



		public InvoiceDeleteCommand()
		{
		}

		public ValidationResult Validate()
		{
			return new Validator().Validate(this);
		}

		class Validator : AbstractValidator<InvoiceDeleteCommand>
		{
			public Validator()
			{
				RuleFor(c => c.InvoiceIds).NotNull();
				RuleFor(c => c.InvoiceIds.Length).GreaterThan(0);
			}
		}
	}
}
