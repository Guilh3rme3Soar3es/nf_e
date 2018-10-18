using FluentAssertions;
using FluentValidation.Results;
using Microsoft.AspNet.OData;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using TheSolutionBrothers.Nfe.API.Controllers.Invoices;
using TheSolutionBrothers.NFe.Application.Features.Carriers.ViewModels;
using TheSolutionBrothers.NFe.Application.Features.Invoices;
using TheSolutionBrothers.NFe.Application.Features.Invoices.Commands;
using TheSolutionBrothers.NFe.Application.Features.Invoices.Queries;
using TheSolutionBrothers.NFe.Application.Features.Invoices.ViewModels;
using TheSolutionBrothers.NFe.Application.Features.Receivers.ViewModels;
using TheSolutionBrothers.NFe.Application.Features.Senders.ViewModels;
using TheSolutionBrothers.NFe.Common.Tests.Features.ObjectMothers;
using TheSolutionBrothers.NFe.Controller.Tests.Initializer;
using TheSolutionBrothers.NFe.Domain.Features.Carriers;
using TheSolutionBrothers.NFe.Domain.Features.InvoiceItems;
using TheSolutionBrothers.NFe.Domain.Features.Invoices;
using TheSolutionBrothers.NFe.Domain.Features.Receivers;
using TheSolutionBrothers.NFe.Domain.Features.Senders;

namespace TheSolutionBrothers.NFe.Controller.Tests.Features.Invoices
{
    [TestFixture]
    public class InvoiceControllerTests : TestControllerBase
    {
        private InvoicesController _invoiceController;
        private Mock<IInvoiceService> _invoiceServiceMock;
        private Mock<Invoice> _invoice;

        private List<InvoiceItemRegisterCommand> unattachedItems;
        private List<InvoiceItemUpdateCommand> attachedItems;

        [SetUp]
        public void Initialize()
        {
            HttpRequestMessage request = new HttpRequestMessage();
            request.SetConfiguration(new HttpConfiguration());
            _invoiceServiceMock = new Mock<IInvoiceService>();
            _invoiceController = new InvoicesController(_invoiceServiceMock.Object)
            {
                Request = request
            };

            _invoice = new Mock<Invoice>();
            
            attachedItems = new List<InvoiceItemUpdateCommand>()
            {
                ObjectMother.GetValidInvoiceItemUpdateCommand()
            };
            
            unattachedItems = new List<InvoiceItemRegisterCommand>()
            {
                ObjectMother.GetValidInvoiceItemRegisterCommand()
            };
        }


        [Test]
        public void Test_InvoiceController_Get_ShouldBeOk()
        {
            var invoice = ObjectMother.GetExistentValidOpenedInvoice(new Mock<Sender>().Object, 
                new Mock<Receiver>().Object,
                new Mock<Carrier>().Object,
                new List<InvoiceItem>() { new Mock<InvoiceItem>().Object });

            var response = new List<Invoice>() { invoice }.AsQueryable();
            _invoiceServiceMock.Setup(s => s.GetAll()).Returns(response);
            var id = 1;
            var odataOptions = GetOdataQueryOptions<Invoice>(_invoiceController);

            var callback = _invoiceController.Get(odataOptions);

            _invoiceServiceMock.Verify(s => s.GetAll(), Times.Once);
            var httpResponse = callback.Should().BeOfType<OkNegotiatedContentResult<PageResult<InvoiceViewModel>>>().Subject;
            httpResponse.Content.Should().NotBeNullOrEmpty();
            httpResponse.Content.First().Id.Should().Be(id);
        }

        [Test]
        public void Test_InvoiceController_GetWithSize_ShouldBeOk()
        {
            var size = 1;
            long id = 1;
            var odataOptions = GetOdataQueryOptions<Invoice>(_invoiceController);
            _invoiceController.Request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:61552/api/Invoices?size=" + size);

            var invoice = ObjectMother.GetExistentValidOpenedInvoice(new Mock<Sender>().Object,
                new Mock<Receiver>().Object,
                new Mock<Carrier>().Object,
                new List<InvoiceItem>() { new Mock<InvoiceItem>().Object });
            
            var response = new List<Invoice>() { invoice }.AsQueryable();
            _invoiceServiceMock.Setup(s => s.GetAll(It.IsAny<InvoiceGetAllQuery>())).Returns(response);

            var callback = _invoiceController.Get(odataOptions);

            _invoiceServiceMock.Verify(s => s.GetAll(It.IsAny<InvoiceGetAllQuery>()), Times.Once);
            var httpResponse = callback.Should().BeOfType<OkNegotiatedContentResult<PageResult<InvoiceViewModel>>>().Subject;
            httpResponse.Content.Should().NotBeNullOrEmpty();
            httpResponse.Content.First().Id.Should().Be(id);
        }

        [Test]
        public void Test_InvoiceController_GetById_ShouldBeOk()
        {
            long id = 1;
            var invoiceViewModel = ObjectMother.GetInvoiceViewModel(It.IsAny<SenderViewModel>(),
                It.IsAny<ReceiverViewModel>(),
                It.IsAny<CarrierViewModel>(),
                It.IsAny<InvoiceTaxViewModel>(),
                new List<InvoiceItemViewModel>() { It.IsAny<InvoiceItemViewModel>() });
            _invoiceServiceMock.Setup(c => c.GetById(id)).Returns(invoiceViewModel);

            IHttpActionResult callback = _invoiceController.GetById(id);

            var httpResponse = callback.Should().BeOfType<OkNegotiatedContentResult<InvoiceViewModel>>().Subject;
            httpResponse.Content.Should().NotBeNull();
            httpResponse.Content.Id.Should().Be(id);
            _invoiceServiceMock.Verify(s => s.GetById(id));
        }

        [Test]
        public void Test_InvoiceController_Post_ShouldBeOk()
        {
            long expectedId = 1;
            var invoiceCommand = ObjectMother.GetValidInvoiceRegisterCommand(unattachedItems);

            _invoiceServiceMock.Setup(c => c.Add(invoiceCommand)).Returns(expectedId);

            IHttpActionResult callback = _invoiceController.Post(invoiceCommand);

            var httpResponse = callback.Should().BeOfType<OkNegotiatedContentResult<long>>().Subject;
            httpResponse.Content.Should().Be(expectedId);
            _invoiceServiceMock.Verify(s => s.Add(invoiceCommand));
        }

        [Test]
        public void Test_InvoiceController_Post_ShouldThrowException()
        {
            var invalidInvoiceCommand = ObjectMother.GetInvalidInvoiceRegisterCommandWithUninformedNatureOperation(unattachedItems);

            IHttpActionResult callback = _invoiceController.Post(invalidInvoiceCommand);
            var httpResponse = callback.Should().BeOfType<NegotiatedContentResult<IList<ValidationFailure>>>().Subject;
            httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            httpResponse.Content.Should().HaveCount(1);
            _invoiceServiceMock.VerifyNoOtherCalls();
        }

        [Test]
        public void Test_InvoiceController_Put_ShouldBeOk()
        {
            var isUpdated = true;
            var invoiceCommand = ObjectMother.GetValidInvoiceUpdateCommand(unattachedItems, attachedItems);

            _invoiceServiceMock.Setup(c => c.Update(invoiceCommand)).Returns(isUpdated);

            IHttpActionResult callback = _invoiceController.Put(invoiceCommand);

            var httpResponse = callback.Should().BeOfType<OkNegotiatedContentResult<bool>>().Subject;
            httpResponse.Content.Should().BeTrue();
            _invoiceServiceMock.Verify(s => s.Update(invoiceCommand), Times.Once);
        }

        [Test]
        public void Test_InvoiceController_Put_ShouldThrowException()
        {
            var invalidInvoiceCommand = ObjectMother.GetInvalidInvoiceUpdateCommandWithoutId(unattachedItems, attachedItems);

            IHttpActionResult callback = _invoiceController.Put(invalidInvoiceCommand);
            var httpResponse = callback.Should().BeOfType<NegotiatedContentResult<IList<ValidationFailure>>>().Subject;
            httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            httpResponse.Content.Should().HaveCountGreaterThan(0);
            _invoiceServiceMock.VerifyNoOtherCalls();
        }

        [Test]
        public void Test_InvoiceController_Delete_ShouldBeOk()
        {
            var InvoiceCommand = ObjectMother.GetInvoiceDeleteCommand();
            var isUpdated = true;
            _invoiceServiceMock.Setup(c => c.Remove(InvoiceCommand)).Returns(isUpdated);

            IHttpActionResult callback = _invoiceController.Delete(InvoiceCommand);

            var httpResponse = callback.Should().BeOfType<OkNegotiatedContentResult<bool>>().Subject;
            _invoiceServiceMock.Verify(s => s.Remove(InvoiceCommand), Times.Once);
            httpResponse.Content.Should().BeTrue();
        }

        [Test]
        public void Test_AccountController_Delete_ShouldThrowException()
        {
            var invalidInvoiceCommand = ObjectMother.GetInvoiceDeleteCommandWithoutId();

            IHttpActionResult callback = _invoiceController.Delete(invalidInvoiceCommand);
            var httpResponse = callback.Should().BeOfType<NegotiatedContentResult<IList<ValidationFailure>>>().Subject;
            httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            httpResponse.Content.Should().HaveCount(1);
            _invoiceServiceMock.VerifyNoOtherCalls();
        }
    }
}
