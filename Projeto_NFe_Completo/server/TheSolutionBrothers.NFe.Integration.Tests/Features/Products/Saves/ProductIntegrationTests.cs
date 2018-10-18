using System;
using TheSolutionBrothers.NFe.Application.Features.Products;
using TheSolutionBrothers.NFe.Common.Tests.Base;
using TheSolutionBrothers.NFe.Common.Tests.Features.ObjectMothers;
using TheSolutionBrothers.NFe.Domain.Features.Addresses;
using TheSolutionBrothers.NFe.Domain.Features.Products;
using TheSolutionBrothers.NFe.Infra.Data.Features.Addresses;
using TheSolutionBrothers.NFe.Infra.Data.Features.Products;
using NUnit.Framework;
using FluentAssertions;
using TheSolutionBrothers.Nfe.Infra.Features.CPFs;
using TheSolutionBrothers.Nfe.Infra.Features.CNPJs;
using TheSolutionBrothers.NFe.Domain.Features.Invoices;
using TheSolutionBrothers.NFe.Infra.Data.Features.Invoices;
using TheSolutionBrothers.Nfe.API.Controllers.Products;
using TheSolutionBrothers.NFe.Infra.Data.Contexts;
using System.Configuration;
using System.Data.Entity;
using AutoMapper;
using TheSolutionBrothers.NFe.Application.Mappers;
using TheSolutionBrothers.NFe.Application.Features.Products.Commands;
using TheSolutionBrothers.NFe.Application.Features.Addresses.commands;
using System.Web.Http.Results;
using System.Collections.Generic;
using System.Net;
using FluentValidation.Results;

namespace TheSolutionBrothers.NFe.Integration.Tests.Features.Products
{
	[TestFixture]
	public partial class ProductIntegrationTests
	{
        private ContextNfe _context;
        private ProductsController _controller;
		private IProductRepository _ProductRepository;
        //private IInvoiceRepository _invoiceRepository;
		private IProductService _service;
        private IMapper _mapper;


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
            _ProductRepository = new ProductRepository(_context);

            //_invoiceRepository = new InvoiceRepository();
            _service = new ProductService(_ProductRepository, _mapper);

            _controller = new ProductsController(_service);
        }

        [Test]
        public void Test_ProductIntegration_Add_ShouldBeOk()
        {
            long expectedId = 2;
            ProductRegisterCommand ProductCommand = ObjectMother.GetValidProductRegisterCommand();

            var result = _controller.Post(ProductCommand);

            var httpResponse = result.Should().BeOfType<OkNegotiatedContentResult<long>>().Subject;
            httpResponse.Content.Should().Be(expectedId);
        }

        [Test]
        public void Test_ProductIntegration_Add_InvalidProduct_ShouldThrowException()
        {
            ProductRegisterCommand ProductCommand = ObjectMother.GetInvalidProductWithCodeLengthOverflowRegisterCommand();

            var result = _controller.Post(ProductCommand);

            var httpResponse = result.Should().BeOfType<NegotiatedContentResult<IList<ValidationFailure>>>().Subject;
            httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            httpResponse.Content.Should().HaveCount(1);
        }

        //[Test]
        //public void Test_ProductIntegration_Add_NullName_ShouldThrowException()
        //{
        //    Product Product = ObjectMother.GetInvalidPhysicalProductNullName(_addressCommand, _cpf);
        //    Action action = () => _service.Add(Product);
        //    action.Should().Throw<ProductUninformedNameException>();
        //}

        //[Test]
        //public void Test_ProductIntegration_Add_NameLengthOverflow_ShouldThrowException()
        //{
        //    Product Product = ObjectMother.GetInvalidPhysicalProductNameLength(_addressCommand, _cpf);
        //    Action action = () => _service.Add(Product);
        //    action.Should().Throw<ProductNameLengthOverflowException>();
        //}

        //[Test]
        //public void Test_ProductIntegration_AddPhysical_CpfWithUninformedValue_ShouldThrowException()
        //{
        //    CPF cpfEmpty = ObjectMother.GetInvalidCPFWithUninformedValue();
        //    Product Product = ObjectMother.GetInvalidPhysicalProductEmptyCpf(_addressCommand, cpfEmpty);
        //    Action action = () => _service.Add(Product);
        //    action.Should().Throw<CPFUninformedValueException>();
        //}

        //[Test]
        //public void Test_ProductIntegration_AddPhysical_NullCpf_ShouldThrowException()
        //{
        //    Product Product = ObjectMother.GetInvalidPhysicalProductNullCpf(_addressCommand);
        //    Action action = () => _service.Add(Product);
        //    action.Should().Throw<ProductNullCpfException>();
        //}
        //[Test]
        //public void Test_ProductIntegration_AddLegal_CnpjWithUninformedValue_ShouldThrowException()
        //{
        //    CNPJ cnpjEmpty = ObjectMother.GetInvalidCNPJWithUninformedValue();
        //    Product Product = ObjectMother.GetInvalidLegalProductEmptyCnpj(_addressCommand, cnpjEmpty);

        //    Action action = () => _service.Add(Product);
        //    action.Should().Throw<CNPJUninformedValueException>();
        //}

        //[Test]
        //public void Test_ProductIntegration_AddLegal_NullCnpj_ShouldThrowException()
        //{
        //    Product Product = ObjectMother.GetInvalidLegalProductNullCnpj(_addressCommand);
        //    Action action = () => _service.Add(Product);
        //    action.Should().Throw<ProductNullCnpjException>();
        //}

        //[Test]
        //public void Test_ProductIntegration_AddLegal_UninformedCompanyName_ShouldThrowException()
        //{
        //    Product Product = ObjectMother.GetInvalidLegalProductEmptyCompanyName(_addressCommand, _cnpj);
        //    Action action = () => _service.Add(Product);
        //    action.Should().Throw<ProductUninformedCompanyNameException>();
        //}

        //[Test]
        //public void Test_ProductIntegration_AddLegal_NullCompanyName_ShouldThrowException()
        //{
        //    Product Product = ObjectMother.GetInvalidLegalProductNullCompanyName(_addressCommand, _cnpj);
        //    Action action = () => _service.Add(Product);
        //    action.Should().Throw<ProductUninformedCompanyNameException>();
        //}

        //[Test]
        //public void Test_ProductIntegration_AddLegal_CompanyNameLengthOverflow_ShouldThrowException()
        //{
        //    Product Product = ObjectMother.GetInvalidLegalProductCompanyNameLengthOverflow(_addressCommand, _cnpj);
        //    Action action = () => _service.Add(Product);
        //    action.Should().Throw<ProductCompanyNameLengthOverflowException>();
        //}

        //[Test]
        //public void Test_ProductIntegration_AddLegal_UninformedStateRegistration_ShouldThrowException()
        //{
        //    Product Product = ObjectMother.GetInvalidLegalProductEmptyStateRegistration(_addressCommand, _cnpj);
        //    Action action = () => _service.Add(Product);
        //    action.Should().Throw<ProductUninformedStateRegistrationException>();
        //}

        //[Test]
        //public void Test_ProductIntegration_AddLegal_NullStateRegistration_ShouldThrowException()
        //{
        //    Product Product = ObjectMother.GetInvalidLegalProductNullStateRegistration(_addressCommand, _cnpj);
        //    Action action = () => _service.Add(Product);
        //    action.Should().Throw<ProductUninformedStateRegistrationException>();
        //}

        //[Test]
        //public void Test_ProductIntegration_AddLegal_StateRegistrationLengthOverflow_ShouldThrowException()
        //{
        //    Product Product = ObjectMother.GetInvalidLegalProductStateRegistrationLengthOverflow(_addressCommand, _cnpj);
        //    Action action = () => _service.Add(Product);
        //    action.Should().Throw<ProductStateRegistrationLengthOverflowException>();
        //}

        //[Test]
        //public void Test_ProductIntegration_AddPhysical_NullAddress_ShouldThrowException()
        //{
        //    Product Product = ObjectMother.GetInvalidPhysicalProductNullAddress(_cpf);
        //    Action action = () => _service.Add(Product);
        //    action.Should().Throw<ProductUninformedAddressException>();
        //}

        //[Test]
        //public void Test_ProductIntegration_AddPhysical_InvalidAddress_ShouldThrowException()
        //{
        //    Address addressInvalid = ObjectMother.GetInvalidAddressWithNegativeNumber();
        //    Product Product = ObjectMother.GetNewValidPhysicalProduct(addressInvalid, _cpf);

        //    Action action = () => _service.Add(Product);

        //    action.Should().Throw<AddressNegativeNumberException>();
        //}

        //[Test]
        //public void Test_ProductIntegration_AddLegal_NullAddress_ShouldThrowException()
        //{
        //    Product Product = ObjectMother.GetInvalidLegalProductNullAddress(_cnpj);
        //    Action action = () => _service.Add(Product);
        //    action.Should().Throw<ProductUninformedAddressException>();
        //}

        //[Test]
        //public void Test_ProductIntegration_AddLegal_InvalidAddress_ShouldThrowException()
        //{
        //    Address addressInvalid = ObjectMother.GetInvalidAddressWithNegativeNumber();
        //    Product Product = ObjectMother.GetNewValidLegalProduct(addressInvalid, _cnpj);

        //    Action action = () => _service.Add(Product);

        //    action.Should().Throw<AddressNegativeNumberException>();
        //}
        //[Test]
        //public void Test_ProductIntegration_Add_AddressWithNegativeNumber_ShouldThrowException()
        //{
        //    Address address = ObjectMother.GetInvalidAddressWithNegativeNumber();
        //    Product ProductToSave = ObjectMother.GetExistentValidLegalProduct(address, _cnpj);

        //    Action action = () => _service.Add(ProductToSave);

        //    action.Should().Throw<AddressNegativeNumberException>();
        //}

        //[Test]
        //public void Test_ProductIntegration_Add_AddressWithUninformedStreetName_ShouldThrowException()
        //{
        //    Address address = ObjectMother.GetInvalidAddressWithUninformedStreetName();
        //    Product ProductToSave = ObjectMother.GetExistentValidLegalProduct(address, _cnpj);

        //    Action action = () => _service.Add(ProductToSave);

        //    action.Should().Throw<AddressUninformedStreetNameException>();
        //}

        //[Test]
        //public void Test_ProductIntegration_Add_AddressWithUninformedNeighborhood_ShouldThrowException()
        //{
        //    Address address = ObjectMother.GetInvalidAddressWithUninformedNeighborhood();
        //    Product ProductToSave = ObjectMother.GetExistentValidLegalProduct(address, _cnpj);

        //    Action action = () => _service.Add(ProductToSave);

        //    action.Should().Throw<AddressUninformedNeighborhoodException>();
        //}

        //[Test]
        //public void Test_ProductIntegration_Add_AddressWithUninformedCity_ShouldThrowException()
        //{
        //    Address address = ObjectMother.GetInvalidAddressWithUninformedCity();
        //    Product ProductToSave = ObjectMother.GetExistentValidLegalProduct(address, _cnpj);

        //    Action action = () => _service.Add(ProductToSave);

        //    action.Should().Throw<AddressUninformedCityException>();
        //}

        //[Test]
        //public void Test_ProductIntegration_Add_AddressWithUninformedState_ShouldThrowException()
        //{
        //    Address address = ObjectMother.GetInvalidAddressWithUninformedState();
        //    Product ProductToSave = ObjectMother.GetExistentValidLegalProduct(address, _cnpj);

        //    Action action = () => _service.Add(ProductToSave);

        //    action.Should().Throw<AddressUninformedStateException>();
        //}

        //[Test]
        //public void Test_ProductIntegration_Add_AddressWithUninformedCountry_ShouldThrowException()
        //{
        //    Address address = ObjectMother.GetInvalidAddressWithUninformedCountry();
        //    Product ProductToSave = ObjectMother.GetExistentValidLegalProduct(address, _cnpj);

        //    Action action = () => _service.Add(ProductToSave);

        //    action.Should().Throw<AddressUninformedCountryException>();
        //}

        //[Test]
        //public void Test_ProductIntegration_Add_AddressWithStreetNameLengthOverflow_ShouldThrowException()
        //{
        //    Address address = ObjectMother.GetInvalidAddressWithStreetNameLengthOverflow();
        //    Product ProductToSave = ObjectMother.GetExistentValidLegalProduct(address, _cnpj);

        //    Action action = () => _service.Add(ProductToSave);

        //    action.Should().Throw<AddressStreetNameLengthOverflowException>();
        //}

        //[Test]
        //public void Test_ProductIntegration_Add_AddressWithNeighborhoodNameLengthOverflow_ShouldThrowException()
        //{
        //    Address address = ObjectMother.GetInvalidAddressWithNeighborhoodNameLengthOverflow();
        //    Product ProductToSave = ObjectMother.GetExistentValidLegalProduct(address, _cnpj);

        //    Action action = () => _service.Add(ProductToSave);

        //    action.Should().Throw<AddressNeighborhoodLengthOverflowException>();
        //}

        //[Test]
        //public void Test_ProductIntegration_Add_AddressWithCityNameLengthOverflow_ShouldThrowException()
        //{
        //    Address address = ObjectMother.GetInvalidAddressWithCityNameLengthOverflow();
        //    Product ProductToSave = ObjectMother.GetExistentValidLegalProduct(address, _cnpj);

        //    Action action = () => _service.Add(ProductToSave);

        //    action.Should().Throw<AddressCityLengthOverflowException>();
        //}

        //[Test]
        //public void Test_ProductIntegration_Add_AddressWithStateNameLengthOverflow_ShouldThrowException()
        //{
        //    Address address = ObjectMother.GetInvalidAddressWithStateNameLengthOverflow();
        //    Product ProductToSave = ObjectMother.GetExistentValidLegalProduct(address, _cnpj);

        //    Action action = () => _service.Add(ProductToSave);

        //    action.Should().Throw<AddressStateLengthOverflowException>();
        //}

        //[Test]
        //public void Test_ProductIntegration_Add_AddressWithCountryNameLengthOverflow_ShouldThrowException()
        //{
        //    Address address = ObjectMother.GetInvalidAddressWithCountryNameLengthOverflow();
        //    Product ProductToSave = ObjectMother.GetExistentValidLegalProduct(address, _cnpj);

        //    Action action = () => _service.Add(ProductToSave);

        //    action.Should().Throw<AddressCountryLengthOverflowException>();
        //}
    }
}
