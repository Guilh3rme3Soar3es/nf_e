using System;
using TheSolutionBrothers.NFe.Application.Features.Receivers;
using TheSolutionBrothers.NFe.Common.Tests.Base;
using TheSolutionBrothers.NFe.Common.Tests.Features.ObjectMothers;
using TheSolutionBrothers.NFe.Domain.Features.Addresses;
using TheSolutionBrothers.NFe.Domain.Features.Receivers;
using TheSolutionBrothers.NFe.Infra.Data.Features.Addresses;
using TheSolutionBrothers.NFe.Infra.Data.Features.Receivers;
using NUnit.Framework;
using FluentAssertions;
using TheSolutionBrothers.Nfe.Infra.Features.CPFs;
using TheSolutionBrothers.Nfe.Infra.Features.CNPJs;
using TheSolutionBrothers.NFe.Domain.Features.Invoices;
using TheSolutionBrothers.NFe.Infra.Data.Features.Invoices;
using TheSolutionBrothers.Nfe.API.Controllers.Receivers;
using TheSolutionBrothers.NFe.Infra.Data.Contexts;
using System.Configuration;
using System.Data.Entity;
using AutoMapper;
using TheSolutionBrothers.NFe.Application.Mappers;
using TheSolutionBrothers.NFe.Application.Features.Receivers.Commands;
using TheSolutionBrothers.NFe.Application.Features.Addresses.commands;
using System.Web.Http.Results;
using System.Collections.Generic;
using System.Net;
using FluentValidation.Results;

namespace TheSolutionBrothers.NFe.Integration.Tests.Features.Receivers
{
	[TestFixture]
	public partial class ReceiverIntegrationTests
	{
        private ContextNfe _context;
        private ReceiversController _controller;
		private IReceiverRepository _receiverRepository;
		private IAddressRepository _addressRepository;
        private IInvoiceRepository _invoiceRepository;
        private IReceiverService _service;
        private IMapper _mapper;

		private AddressCommand _addressCommand;
		private Address _addressWithId;

		private CPF _cpf;
		private CNPJ _cnpj;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            AutoMapperConfig.Reset();
            AutoMapperConfig.RegisterMappings();
        }

        [SetUp]
        public void Initialize()
        {
            Database.SetInitializer(new DbCreator<ContextNfe>());
            _context = new ContextNfe(ConfigurationManager.AppSettings.Get("ConnectionStringName"));


            _mapper = Mapper.Instance;
            _receiverRepository = new ReceiverRepository(_context);
            _addressRepository = new AddressRepository(_context);
            _invoiceRepository = new InvoiceRepository(_context);
            _service = new ReceiverService(_receiverRepository, _addressRepository, _invoiceRepository, _mapper);

            _controller = new ReceiversController(_service);

            _addressCommand = ObjectMother.GetValidAddresCommand();

            _cpf = ObjectMother.GetValidCPF();
            _cnpj = ObjectMother.GetValidCNPJ();
        }

        [Test]
        public void Test_ReceiverIntegration_AddPhysical_ShouldBeOk()
        {
            long expectedId = 3;
            ReceiverRegisterCommand receiverCommand = ObjectMother.GetValidPhysicalReceiverRegisterCommand(_addressCommand, _cpf);

            var result = _controller.Post(receiverCommand);

            var httpResponse = result.Should().BeOfType<OkNegotiatedContentResult<long>>().Subject;
            httpResponse.Content.Should().Be(expectedId);
        }

        [Test]
        public void Test_ReceiverIntegration_AddLegal_ShouldBeOk()
        {
            long expectedId = 3;
            ReceiverRegisterCommand receiverCommand = ObjectMother.GetValidLegalReceiverRegisterCommand(_addressCommand, _cnpj);

            var result = _controller.Post(receiverCommand);

            var httpResponse = result.Should().BeOfType<OkNegotiatedContentResult<long>>().Subject;
            httpResponse.Content.Should().Be(expectedId);
        }

        [Test]
        public void Test_ReceiverIntegration_Add_UninformedName_ShouldThrowException()
        {
            ReceiverRegisterCommand receiverCommand = ObjectMother.GetInvalidLegalReceiverRegisterCommandWithUninformedName(_addressCommand, _cnpj);

            var result = _controller.Post(receiverCommand);

            var httpResponse = result.Should().BeOfType<NegotiatedContentResult<IList<ValidationFailure>>>().Subject;
            httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            httpResponse.Content.Should().HaveCount(3);
        }

        //[Test]
        //public void Test_ReceiverIntegration_Add_NullName_ShouldThrowException()
        //{
        //    Receiver receiver = ObjectMother.GetInvalidPhysicalReceiverNullName(_addressCommand, _cpf);
        //    Action action = () => _service.Add(receiver);
        //    action.Should().Throw<ReceiverUninformedNameException>();
        //}

        //[Test]
        //public void Test_ReceiverIntegration_Add_NameLengthOverflow_ShouldThrowException()
        //{
        //    Receiver receiver = ObjectMother.GetInvalidPhysicalReceiverNameLength(_addressCommand, _cpf);
        //    Action action = () => _service.Add(receiver);
        //    action.Should().Throw<ReceiverNameLengthOverflowException>();
        //}

        //[Test]
        //public void Test_ReceiverIntegration_AddPhysical_CpfWithUninformedValue_ShouldThrowException()
        //{
        //    CPF cpfEmpty = ObjectMother.GetInvalidCPFWithUninformedValue();
        //    Receiver receiver = ObjectMother.GetInvalidPhysicalReceiverEmptyCpf(_addressCommand, cpfEmpty);
        //    Action action = () => _service.Add(receiver);
        //    action.Should().Throw<CPFUninformedValueException>();
        //}

        //[Test]
        //public void Test_ReceiverIntegration_AddPhysical_NullCpf_ShouldThrowException()
        //{
        //    Receiver receiver = ObjectMother.GetInvalidPhysicalReceiverNullCpf(_addressCommand);
        //    Action action = () => _service.Add(receiver);
        //    action.Should().Throw<ReceiverNullCpfException>();
        //}
        //[Test]
        //public void Test_ReceiverIntegration_AddLegal_CnpjWithUninformedValue_ShouldThrowException()
        //{
        //    CNPJ cnpjEmpty = ObjectMother.GetInvalidCNPJWithUninformedValue();
        //    Receiver receiver = ObjectMother.GetInvalidLegalReceiverEmptyCnpj(_addressCommand, cnpjEmpty);

        //    Action action = () => _service.Add(receiver);
        //    action.Should().Throw<CNPJUninformedValueException>();
        //}

        //[Test]
        //public void Test_ReceiverIntegration_AddLegal_NullCnpj_ShouldThrowException()
        //{
        //    Receiver receiver = ObjectMother.GetInvalidLegalReceiverNullCnpj(_addressCommand);
        //    Action action = () => _service.Add(receiver);
        //    action.Should().Throw<ReceiverNullCnpjException>();
        //}

        //[Test]
        //public void Test_ReceiverIntegration_AddLegal_UninformedCompanyName_ShouldThrowException()
        //{
        //    Receiver receiver = ObjectMother.GetInvalidLegalReceiverEmptyCompanyName(_addressCommand, _cnpj);
        //    Action action = () => _service.Add(receiver);
        //    action.Should().Throw<ReceiverUninformedCompanyNameException>();
        //}

        //[Test]
        //public void Test_ReceiverIntegration_AddLegal_NullCompanyName_ShouldThrowException()
        //{
        //    Receiver receiver = ObjectMother.GetInvalidLegalReceiverNullCompanyName(_addressCommand, _cnpj);
        //    Action action = () => _service.Add(receiver);
        //    action.Should().Throw<ReceiverUninformedCompanyNameException>();
        //}

        //[Test]
        //public void Test_ReceiverIntegration_AddLegal_CompanyNameLengthOverflow_ShouldThrowException()
        //{
        //    Receiver receiver = ObjectMother.GetInvalidLegalReceiverCompanyNameLengthOverflow(_addressCommand, _cnpj);
        //    Action action = () => _service.Add(receiver);
        //    action.Should().Throw<ReceiverCompanyNameLengthOverflowException>();
        //}

        //[Test]
        //public void Test_ReceiverIntegration_AddLegal_UninformedStateRegistration_ShouldThrowException()
        //{
        //    Receiver receiver = ObjectMother.GetInvalidLegalReceiverEmptyStateRegistration(_addressCommand, _cnpj);
        //    Action action = () => _service.Add(receiver);
        //    action.Should().Throw<ReceiverUninformedStateRegistrationException>();
        //}

        //[Test]
        //public void Test_ReceiverIntegration_AddLegal_NullStateRegistration_ShouldThrowException()
        //{
        //    Receiver receiver = ObjectMother.GetInvalidLegalReceiverNullStateRegistration(_addressCommand, _cnpj);
        //    Action action = () => _service.Add(receiver);
        //    action.Should().Throw<ReceiverUninformedStateRegistrationException>();
        //}

        //[Test]
        //public void Test_ReceiverIntegration_AddLegal_StateRegistrationLengthOverflow_ShouldThrowException()
        //{
        //    Receiver receiver = ObjectMother.GetInvalidLegalReceiverStateRegistrationLengthOverflow(_addressCommand, _cnpj);
        //    Action action = () => _service.Add(receiver);
        //    action.Should().Throw<ReceiverStateRegistrationLengthOverflowException>();
        //}

        //[Test]
        //public void Test_ReceiverIntegration_AddPhysical_NullAddress_ShouldThrowException()
        //{
        //    Receiver receiver = ObjectMother.GetInvalidPhysicalReceiverNullAddress(_cpf);
        //    Action action = () => _service.Add(receiver);
        //    action.Should().Throw<ReceiverUninformedAddressException>();
        //}

        //[Test]
        //public void Test_ReceiverIntegration_AddPhysical_InvalidAddress_ShouldThrowException()
        //{
        //    Address addressInvalid = ObjectMother.GetInvalidAddressWithNegativeNumber();
        //    Receiver receiver = ObjectMother.GetNewValidPhysicalReceiver(addressInvalid, _cpf);

        //    Action action = () => _service.Add(receiver);

        //    action.Should().Throw<AddressNegativeNumberException>();
        //}

        //[Test]
        //public void Test_ReceiverIntegration_AddLegal_NullAddress_ShouldThrowException()
        //{
        //    Receiver receiver = ObjectMother.GetInvalidLegalReceiverNullAddress(_cnpj);
        //    Action action = () => _service.Add(receiver);
        //    action.Should().Throw<ReceiverUninformedAddressException>();
        //}

        //[Test]
        //public void Test_ReceiverIntegration_AddLegal_InvalidAddress_ShouldThrowException()
        //{
        //    Address addressInvalid = ObjectMother.GetInvalidAddressWithNegativeNumber();
        //    Receiver receiver = ObjectMother.GetNewValidLegalReceiver(addressInvalid, _cnpj);

        //    Action action = () => _service.Add(receiver);

        //    action.Should().Throw<AddressNegativeNumberException>();
        //}
        //[Test]
        //public void Test_ReceiverIntegration_Add_AddressWithNegativeNumber_ShouldThrowException()
        //{
        //    Address address = ObjectMother.GetInvalidAddressWithNegativeNumber();
        //    Receiver receiverToSave = ObjectMother.GetExistentValidLegalReceiver(address, _cnpj);

        //    Action action = () => _service.Add(receiverToSave);

        //    action.Should().Throw<AddressNegativeNumberException>();
        //}

        //[Test]
        //public void Test_ReceiverIntegration_Add_AddressWithUninformedStreetName_ShouldThrowException()
        //{
        //    Address address = ObjectMother.GetInvalidAddressWithUninformedStreetName();
        //    Receiver receiverToSave = ObjectMother.GetExistentValidLegalReceiver(address, _cnpj);

        //    Action action = () => _service.Add(receiverToSave);

        //    action.Should().Throw<AddressUninformedStreetNameException>();
        //}

        //[Test]
        //public void Test_ReceiverIntegration_Add_AddressWithUninformedNeighborhood_ShouldThrowException()
        //{
        //    Address address = ObjectMother.GetInvalidAddressWithUninformedNeighborhood();
        //    Receiver receiverToSave = ObjectMother.GetExistentValidLegalReceiver(address, _cnpj);

        //    Action action = () => _service.Add(receiverToSave);

        //    action.Should().Throw<AddressUninformedNeighborhoodException>();
        //}

        //[Test]
        //public void Test_ReceiverIntegration_Add_AddressWithUninformedCity_ShouldThrowException()
        //{
        //    Address address = ObjectMother.GetInvalidAddressWithUninformedCity();
        //    Receiver receiverToSave = ObjectMother.GetExistentValidLegalReceiver(address, _cnpj);

        //    Action action = () => _service.Add(receiverToSave);

        //    action.Should().Throw<AddressUninformedCityException>();
        //}

        //[Test]
        //public void Test_ReceiverIntegration_Add_AddressWithUninformedState_ShouldThrowException()
        //{
        //    Address address = ObjectMother.GetInvalidAddressWithUninformedState();
        //    Receiver receiverToSave = ObjectMother.GetExistentValidLegalReceiver(address, _cnpj);

        //    Action action = () => _service.Add(receiverToSave);

        //    action.Should().Throw<AddressUninformedStateException>();
        //}

        //[Test]
        //public void Test_ReceiverIntegration_Add_AddressWithUninformedCountry_ShouldThrowException()
        //{
        //    Address address = ObjectMother.GetInvalidAddressWithUninformedCountry();
        //    Receiver receiverToSave = ObjectMother.GetExistentValidLegalReceiver(address, _cnpj);

        //    Action action = () => _service.Add(receiverToSave);

        //    action.Should().Throw<AddressUninformedCountryException>();
        //}

        //[Test]
        //public void Test_ReceiverIntegration_Add_AddressWithStreetNameLengthOverflow_ShouldThrowException()
        //{
        //    Address address = ObjectMother.GetInvalidAddressWithStreetNameLengthOverflow();
        //    Receiver receiverToSave = ObjectMother.GetExistentValidLegalReceiver(address, _cnpj);

        //    Action action = () => _service.Add(receiverToSave);

        //    action.Should().Throw<AddressStreetNameLengthOverflowException>();
        //}

        //[Test]
        //public void Test_ReceiverIntegration_Add_AddressWithNeighborhoodNameLengthOverflow_ShouldThrowException()
        //{
        //    Address address = ObjectMother.GetInvalidAddressWithNeighborhoodNameLengthOverflow();
        //    Receiver receiverToSave = ObjectMother.GetExistentValidLegalReceiver(address, _cnpj);

        //    Action action = () => _service.Add(receiverToSave);

        //    action.Should().Throw<AddressNeighborhoodLengthOverflowException>();
        //}

        //[Test]
        //public void Test_ReceiverIntegration_Add_AddressWithCityNameLengthOverflow_ShouldThrowException()
        //{
        //    Address address = ObjectMother.GetInvalidAddressWithCityNameLengthOverflow();
        //    Receiver receiverToSave = ObjectMother.GetExistentValidLegalReceiver(address, _cnpj);

        //    Action action = () => _service.Add(receiverToSave);

        //    action.Should().Throw<AddressCityLengthOverflowException>();
        //}

        //[Test]
        //public void Test_ReceiverIntegration_Add_AddressWithStateNameLengthOverflow_ShouldThrowException()
        //{
        //    Address address = ObjectMother.GetInvalidAddressWithStateNameLengthOverflow();
        //    Receiver receiverToSave = ObjectMother.GetExistentValidLegalReceiver(address, _cnpj);

        //    Action action = () => _service.Add(receiverToSave);

        //    action.Should().Throw<AddressStateLengthOverflowException>();
        //}

        //[Test]
        //public void Test_ReceiverIntegration_Add_AddressWithCountryNameLengthOverflow_ShouldThrowException()
        //{
        //    Address address = ObjectMother.GetInvalidAddressWithCountryNameLengthOverflow();
        //    Receiver receiverToSave = ObjectMother.GetExistentValidLegalReceiver(address, _cnpj);

        //    Action action = () => _service.Add(receiverToSave);

        //    action.Should().Throw<AddressCountryLengthOverflowException>();
        //}
    }
}
