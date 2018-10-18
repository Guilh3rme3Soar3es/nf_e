using System;
using System.Linq;
using TheSolutionBrothers.NFe.Domain.Features.InvoiceCarriers;
using TheSolutionBrothers.NFe.Domain.Features.InvoiceReceivers;
using TheSolutionBrothers.NFe.Domain.Features.Invoices;
using TheSolutionBrothers.NFe.Domain.Features.InvoiceSenders;
using TheSolutionBrothers.NFe.Domain.Exceptions;
using TheSolutionBrothers.NFe.Domain.Features.InvoiceItems;
using TheSolutionBrothers.NFe.Domain.Features.InvoiceTaxes;
using TheSolutionBrothers.NFe.Infra.XML.Features.Invoices;
using TheSolutionBrothers.NFe.Infra.PDF.Features.Invoices;
using TheSolutionBrothers.NFe.Application.Features.Invoices.Commands;
using TheSolutionBrothers.NFe.Application.Features.Invoices.Queries;
using TheSolutionBrothers.NFe.Application.Features.Invoices.ViewModels;
using AutoMapper;
using TheSolutionBrothers.NFe.Domain.Features.Carriers;
using TheSolutionBrothers.NFe.Domain.Features.Receivers;
using TheSolutionBrothers.NFe.Domain.Features.Senders;
using TheSolutionBrothers.NFe.Domain.Features.Products;
using System.Collections.Generic;

namespace TheSolutionBrothers.NFe.Application.Features.Invoices
{
    public class InvoiceService : IInvoiceService
    {
        private IInvoiceRepository _invoiceRepository;
        private ICarrierRepository _carrierRepository;
        private IReceiverRepository _receiverRepository;
        private ISenderRepository _senderRepository;
        private IProductRepository _productRepository;
        private IInvoiceItemRepository _invoiceItemRepository;
        private readonly IMapper _mapper;

        public InvoiceService(IInvoiceRepository invoiceRepository, ICarrierRepository carrierRepository, IReceiverRepository receiverRepository, ISenderRepository senderRepository, IProductRepository productRepository, IInvoiceItemRepository invoiceItemRepository, IMapper mapper)
        {
            _invoiceRepository = invoiceRepository;
            _carrierRepository = carrierRepository;
            _receiverRepository = receiverRepository;
            _senderRepository = senderRepository;
            _productRepository = productRepository;
            _invoiceItemRepository = invoiceItemRepository;
            _mapper = mapper;

        }

        //      public Invoice Update(Invoice entity)
        //      {
        //          if (entity.Id <= 0)
        //              throw new IdentifierUndefinedException();
        //          if (entity.Status.Equals(InvoiceStatus.ISSUED))
        //              throw new InvoiceUpdateIssuedInvoiceException();
        //          entity.Validate();
        //          Invoice invoice = _repositoryInvoice.GetByNumber(entity.Number);
        //          if (invoice != null && !invoice.Id.Equals(entity.Id))
        //              throw new InvoiceExistentNumberException();

        //          foreach (var item in entity.InvoiceItems)
        //          {
        //              if (item.Id <= 0)
        //                  _repositoryInvoiceItem.Add(item);
        //              else 
        //                  _repositoryInvoiceItem.Update(item);
        //          }

        //          _repositoryInvoice.Update(entity);

        //          return entity;
        //      }

        //      public Invoice Issue(Invoice entity, double freightValue)
        //      {
        //          if (entity.Id <= 0)
        //              throw new IdentifierUndefinedException();
        //          if (entity.Status.Equals(InvoiceStatus.ISSUED))
        //              throw new InvoiceIssueIssuedInvoiceException();
        //          entity.Validate();
        //          Invoice invoice = _repositoryInvoice.GetByNumber(entity.Number);
        //          if (invoice != null && !invoice.Id.Equals(entity.Id))
        //              throw new InvoiceExistentNumberException();
        //          entity.Issue(freightValue);
        //          invoice = _repositoryInvoice.GetByKeyAccess(entity.KeyAccess.Value);
        //          if (invoice != null && !invoice.Id.Equals(entity.Id))
        //              throw new InvoiceKeyAccessExistentException();
        //          foreach (var item in entity.InvoiceItems)
        //          {
        //              _repositoryInvoiceItem.Update(item);
        //          }
        //          _repositoryInvoiceTax.Add(entity.InvoiceTax);
        //          _repositoryInvoiceCarrier.Add(entity.InvoiceCarrier);
        //          _repositoryInvoiceReceiver.Add(entity.InvoiceReceiver);
        //          _repositoryInvoiceSender.Add(entity.InvoiceSender);
        //          _repositoryInvoice.Update(entity);
        //          return entity;
        //      }

        //public void ExportToXML(Invoice entity, string path)
        //{
        //	if (entity.Id <= 0)
        //	{
        //		throw new IdentifierUndefinedException();
        //	}
        //          if (String.IsNullOrEmpty(path))
        //          {
        //              throw new InvoiceExportInvalidPathException();
        //          }

        //	Invoice invoiceToExport = Get(entity.Id);

        //	if (entity.Status != InvoiceStatus.ISSUED)
        //	{
        //		throw new InvoiceExportOpenInvoiceException();
        //	}

        //	entity.Validate();
        //	_repositoryXML.Export(entity, path);
        //}

        //      public void ExportToPDF(Invoice entity, string path)
        //      {
        //	if (entity.Id <= 0)
        //	{
        //		throw new IdentifierUndefinedException();
        //	}
        //	if (String.IsNullOrEmpty(path))
        //	{
        //		throw new InvoiceExportInvalidPathException();
        //	}

        //	Invoice invoiceToExport = Get(entity.Id);

        //	if (entity.Status != InvoiceStatus.ISSUED)
        //	{
        //		throw new InvoiceExportOpenInvoiceException();
        //	}

        //	entity.Validate();
        //	_repositoryPDF.Export(entity, path);
        //}

        public long Add(InvoiceRegisterCommand command)
        {
            if (command.Status.Equals(InvoiceStatus.ISSUED))
                throw new InvoiceSaveIssuedInvoiceException();
            var entity = _mapper.Map<Invoice>(command);
            //entity.Validate();
            Invoice invoice = _invoiceRepository.GetByNumber(entity.Number);
            if (invoice != null)
                throw new InvoiceExistentNumberException();
            entity = _invoiceRepository.Add(entity);
            return entity.Id;
        }



        public InvoiceViewModel GetById(long id)
        {
            return _mapper.Map<InvoiceViewModel>(_invoiceRepository.GetById(id));
        }

        public IQueryable<Invoice> GetAll()
        {
            return _invoiceRepository.GetAll();
        }

        public IQueryable<Invoice> GetAll(InvoiceGetAllQuery query)
        {
            return _invoiceRepository.GetAll(query.Size);
        }

        public bool Remove(InvoiceDeleteCommand entity)
        {
            var isRemovedAll = true;
            foreach (var invoiceId in entity.InvoiceIds)
            {
                var invoiceToDelete = _invoiceRepository.GetById(invoiceId) ?? throw new NotFoundException();
                if (invoiceToDelete.Status.Equals(InvoiceStatus.ISSUED))
                    throw new InvoiceDeleteIssuedInvoiceException();
                var isRemoved = _invoiceRepository.Remove(invoiceId);
                isRemovedAll = isRemoved ? isRemovedAll : false;
            }
            return isRemovedAll;
        }

        public bool Update(InvoiceUpdateCommand command)
        {
            if (command.Status.Equals(InvoiceStatus.ISSUED))
                throw new InvoiceUpdateIssuedInvoiceException();

            var entity = _mapper.Map<Invoice>(command);

            if (entity.InvoiceItems.Count <= 0)
                throw new InvoiceEmptyInvoiceItemsException();

            Invoice invoice = _invoiceRepository.GetByNumber(entity.Number);
            if (invoice != null && !invoice.Id.Equals(entity.Id))
                throw new InvoiceExistentNumberException();

            var invoiceDb = _invoiceRepository.GetById(entity.Id) ?? throw new NotFoundException();

            invoiceDb.EntryDate = entity.EntryDate;
            invoiceDb.NatureOperation = entity.NatureOperation;
            invoiceDb.Number = entity.Number;
            invoiceDb.Status = entity.Status;
            invoiceDb.Carrier = _carrierRepository.GetById(command.CarrierId) ?? throw new NotFoundException();
            invoiceDb.Receiver = _receiverRepository.GetById(command.ReceiverId) ?? throw new NotFoundException();
            invoiceDb.Sender = _senderRepository.GetById(command.SenderId) ?? throw new NotFoundException();

            foreach (var item in entity.InvoiceItems)
            {
                if (item.Id <= 0)
                {
                    item.Product = _productRepository.GetById(item.ProductId) ?? throw new NotFoundException();
                    item.Invoice = invoiceDb;
                    invoiceDb.InvoiceItems.Add(item);
                }
                else
                {
                    invoiceDb.InvoiceItems.ToList().Find(x => x.Id == item.Id).Amount = item.Amount;
                    invoiceDb.InvoiceItems.ToList().Find(x => x.Id == item.Id).Invoice = invoiceDb;
                    invoiceDb.InvoiceItems.ToList().Find(x => x.Id == item.Id).ProductId = item.ProductId;
                    invoiceDb.InvoiceItems.ToList().Find(x => x.Id == item.Id).Product = _productRepository.GetById(item.ProductId) ?? throw new NotFoundException();
                }
            }
            var attachedItems = invoiceDb.InvoiceItems.ToList().FindAll(y => y.Id > 0);
            foreach (var item in attachedItems)
            {
                if (command.attachedItems.ToList().Find(z => z.Id == item.Id) == null)
                {
                    _invoiceItemRepository.Remove(item.Id);
                    invoiceDb.InvoiceItems.Remove(item);
                }
            }
            invoiceDb.Validate();

            return _invoiceRepository.Update(invoiceDb);
        }

        public void ExportToPDF(Invoice entity, string path)
        {
            throw new NotImplementedException();
        }

        public void ExportToXML(Invoice entity, string path)
        {
            throw new NotImplementedException();
        }

        public Invoice Issue(Invoice invoice, double freightValue)
        {
            throw new NotImplementedException();
        }
    }
}
