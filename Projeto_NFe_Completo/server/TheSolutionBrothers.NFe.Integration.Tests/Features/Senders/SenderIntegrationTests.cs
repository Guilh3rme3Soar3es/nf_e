//using FluentAssertions;
//using NUnit.Framework;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using TheSolutionBrothers.NFe.Application.Features.Senders;
//using TheSolutionBrothers.NFe.Common.Tests.Base;
//using TheSolutionBrothers.NFe.Common.Tests.Features.ObjectMothers;
//using TheSolutionBrothers.NFe.Domain.Features.Addresses;
//using TheSolutionBrothers.NFe.Domain.Features.Senders;
//using TheSolutionBrothers.NFe.Infra.Data.Features.Addresses;
//using TheSolutionBrothers.NFe.Infra.Data.Features.Senders;

//namespace NDD.NFe.Integration.Tests.Features.Senders
//{
//    [TestFixture]
//    public class SenderIntegrationTests 
//    {
//        private ISenderRepository _senderRepository;
//        private IAddressRepository _addressRepository;
//        private ISenderService _service;

//        private Address _address;

//        [SetUp]
//        public void Initialize()
//        {
//            BaseSqlTest.SeedDatabase();
//            _senderRepository = new SenderRepository();
//            _addressRepository = new AddressRepository();
//            _service = new SenderService(_senderRepository, _addressRepository);

//            _address = ObjectMother.GetNewValidAddress();
//        }

//        [Test]
//        public void Test_SenderIntegration_Save_ShouldBeOk()
//        {
//            Sender sender = ObjectMother.GetNewValidSender(_address);
//            sender = _service.Add(sender);
//            sender.Id.Should().BeGreaterThan(0);
//        }

//        [Test]
//        public void Test_SenderIntegration_SaveWithUninformedFancyName_ShouldThrowException()
//        {
//            Sender senderToSave = ObjectMother.GetInvalidSenderWithUninformedFancyName(_address);
//            Action action = () => _service.Add(senderToSave);
//            action.Should().Throw<SenderUninformedFancyNameException>();
//        }

//        [Test]
//        public void Test_SenderIntegration_SaveWithUninformedCompanyName_ShouldThrowException()
//        {
//            Sender senderToSave = ObjectMother.GetInvalidSenderWithUninformedCompanyName(_address);
//            Action action = () => _service.Add(senderToSave);
//            action.Should().Throw<SenderUninformedCompanyNameException>();
//        }

//        [Test]
//        public void Test_SenderIntegration_SaveWithUninformedStateRegistration_ShouldThrowException()
//        {
//            Sender senderToSave = ObjectMother.GetInvalidSenderWithUninformedStateRegistration(_address);
//            Action action = () => _service.Add(senderToSave);
//            action.Should().Throw<SenderUninformedStateRegistrationException>();
//        }

//        [Test]
//        public void Test_SenderIntegration_SaveWithUninformedMunicipalRegistration_ShouldThrowException()
//        {
//            Sender senderToSave = ObjectMother.GetInvalidSenderWithUninformedMunicipalRegistration(_address);
//            Action action = () => _service.Add(senderToSave);
//            action.Should().Throw<SenderUninformedMunicipalRegistrationException>();
//        }

//        [Test]
//        public void Test_SenderIntegration_SaveWithUninformedCnpj_ShouldThrowException()
//        {
//            Sender senderToSave = ObjectMother.GetInvalidSenderWithUninformedCnpj(_address);
//            Action action = () => _service.Add(senderToSave);
//            action.Should().Throw<SenderUninformedCnpjException>();
//        }

//        [Test]
//        public void Test_SenderIntegration_SaveWithAddressNull_ShouldThrowException()
//        {
//            Sender senderToSave = ObjectMother.GetInvalidSenderWithAddressNull();
//            Action action = () => _service.Add(senderToSave);
//            action.Should().Throw<SenderNullAddressException>();
//        }

//        [Test]
//        public void Test_SenderIntegration_SaveWithAddressNegativeNumber_ShouldThrowException()
//        {
//            _address = ObjectMother.GetInvalidAddressWithNegativeNumber();
//            Sender senderToSave = ObjectMother.GetNewValidSender(_address);
//            Action action = () => _service.Add(senderToSave);
//            action.Should().Throw<AddressNegativeNumberException>();
//        }

//        [Test]
//        public void Test_SenderIntegration_SaveWithAddressUninformedStreetName_ShouldThrowException()
//        {
//            _address = ObjectMother.GetInvalidAddressWithUninformedStreetName();
//            Sender senderToSave = ObjectMother.GetNewValidSender(_address);
//            Action action = () => _service.Add(senderToSave);
//            action.Should().Throw<AddressUninformedStreetNameException>();
//        }

//        [Test]
//        public void Test_SenderIntegration_SaveWithAddressUninformedNeighborhood_ShouldThrowException()
//        {
//            _address = ObjectMother.GetInvalidAddressWithUninformedNeighborhood();
//            Sender senderToSave = ObjectMother.GetNewValidSender(_address);
//            Action action = () => _service.Add(senderToSave);
//            action.Should().Throw<AddressUninformedNeighborhoodException>();
//        }

//        [Test]
//        public void Test_SenderIntegration_SaveWithAddressUninformedCity_ShouldThrowException()
//        {
//            _address = ObjectMother.GetInvalidAddressWithUninformedCity();
//            Sender senderToSave = ObjectMother.GetNewValidSender(_address);
//            Action action = () => _service.Add(senderToSave);
//            action.Should().Throw<AddressUninformedCityException>();
//        }

//        [Test]
//        public void Test_SenderIntegration_SaveWithAddressUninformedState_ShouldThrowException()
//        {
//            _address = ObjectMother.GetInvalidAddressWithUninformedState();
//            Sender senderToSave = ObjectMother.GetNewValidSender(_address);
//            Action action = () => _service.Add(senderToSave);
//            action.Should().Throw<AddressUninformedStateException>();
//        }

//        [Test]
//        public void Test_SenderIntegration_SaveWithAddressUninformedCountry_ShouldThrowException()
//        {
//            _address = ObjectMother.GetInvalidAddressWithUninformedCountry();
//            Sender senderToSave = ObjectMother.GetNewValidSender(_address);
//            Action action = () => _service.Add(senderToSave);
//            action.Should().Throw<AddressUninformedCountryException>();
//        }

//        [Test]
//        public void Test_SenderIntegration_SaveWithFancyNameLengthOverflow_ShouldThrowException()
//        {
//            Sender senderToSave = ObjectMother.GetInvalidSenderWithFancyNameLenghtOverflow(_address);
//            Action action = () => _service.Add(senderToSave);
//            action.Should().Throw<SenderFancyNameLenghtOverflowException>();
//        }

//        [Test]
//        public void Test_SenderIntegration_SaveWithCompanyNameLengthOverflow_ShouldThrowException()
//        {
//            Sender senderToSave = ObjectMother.GetInvalidSenderWithCompanyNameLenghtOverflow(_address);
//            Action action = () => _service.Add(senderToSave);
//            action.Should().Throw<SenderCompanyNameLenghtOverflowException>();
//        }

//        [Test]
//        public void Test_SenderIntegration_SaveWithStateRegistrationLengthOverflow_ShouldThrowException()
//        {
//            Sender senderToSave = ObjectMother.GetInvalidSenderWithStateRegistrationLenghtOverflow(_address);
//            Action action = () => _service.Add(senderToSave);
//            action.Should().Throw<SenderStateRegistrationLenghtOverflowException>();
//        }

//        [Test]
//        public void Test_SenderIntegration_SaveWithMunicipalRegistrationLengthOverflow_ShouldThrowException()
//        {
//            Sender senderToSave = ObjectMother.GetInvalidSenderWithMunicipalRegistrationLenghtOverflow(_address);
//            Action action = () => _service.Add(senderToSave);
//            action.Should().Throw<SenderMunicipalRegistrationLenghtOverflowException>();
//        }

//        [Test]
//        public void Test_SenderIntegration_Update_ShouldBeOk()
//        {
//            _address = ObjectMother.GetExistentValidAddress();
//            Sender senderToUpdate = ObjectMother.GetExistentValidSender(_address);

//            senderToUpdate = _service.Update(senderToUpdate);

//            Sender result = _service.Get(senderToUpdate.Id);
//            result.Should().NotBeNull();
//            result.CompanyName.Should().Be(senderToUpdate.CompanyName);
//        }

//        [Test]
//        public void Test_SenderIntegration_UpdateWithUninformedFancyName_ShouldThrowException()
//        {
//            long existentId = 1;
//            _address = ObjectMother.GetExistentValidAddress();
//            Sender senderToUpdate = ObjectMother.GetInvalidSenderWithUninformedFancyName(_address);
//            senderToUpdate.Id = existentId;

//            Action action = () => _service.Update(senderToUpdate);

//            action.Should().Throw<SenderUninformedFancyNameException>();
//            Sender result = _service.Get(senderToUpdate.Id);
//            result.CompanyName.Should().NotBe(senderToUpdate.CompanyName);
//        }

//        [Test]
//        public void Test_SenderIntegration_UpdateWithUninformedCompanyName_ShouldThrowException()
//        {
//            long existentId = 1;
//            _address = ObjectMother.GetExistentValidAddress();
//            Sender senderToUpdate = ObjectMother.GetInvalidSenderWithUninformedCompanyName(_address);
//            senderToUpdate.Id = existentId;

//            Action action = () => _service.Update(senderToUpdate);

//            action.Should().Throw<SenderUninformedCompanyNameException>();
//            Sender result = _service.Get(senderToUpdate.Id);
//            result.CompanyName.Should().NotBe(senderToUpdate.CompanyName);
//        }

//        [Test]
//        public void Test_SenderIntegration_UpdateWithUninformedStateRegistration_ShouldThrowException()
//        {
//            long existentId = 1;
//            _address = ObjectMother.GetExistentValidAddress();
//            Sender senderToUpdate = ObjectMother.GetInvalidSenderWithUninformedStateRegistration(_address);
//            senderToUpdate.Id = existentId;

//            Action action = () => _service.Update(senderToUpdate);

//            action.Should().Throw<SenderUninformedStateRegistrationException>();
//            Sender result = _service.Get(senderToUpdate.Id);
//            result.CompanyName.Should().NotBe(senderToUpdate.CompanyName);
//        }

//        [Test]
//        public void Test_SenderIntegration_UpdateWithUninformedMunicipalRegistration_ShouldThrowException()
//        {
//            long existentId = 1;
//            _address = ObjectMother.GetExistentValidAddress();
//            Sender senderToUpdate = ObjectMother.GetInvalidSenderWithUninformedMunicipalRegistration(_address);
//            senderToUpdate.Id = existentId;

//            Action action = () => _service.Update(senderToUpdate);

//            action.Should().Throw<SenderUninformedMunicipalRegistrationException>();
//            Sender result = _service.Get(senderToUpdate.Id);
//            result.CompanyName.Should().NotBe(senderToUpdate.CompanyName);
//        }

//        [Test]
//        public void Test_SenderIntegration_UpdateWithUninformedCnpj_ShouldThrowException()
//        {
//            long existentId = 1;
//            _address = ObjectMother.GetExistentValidAddress();
//            Sender senderToUpdate = ObjectMother.GetInvalidSenderWithUninformedCnpj(_address);
//            senderToUpdate.Id = existentId;

//            Action action = () => _service.Update(senderToUpdate);

//            action.Should().Throw<SenderUninformedCnpjException>();
//            Sender result = _service.Get(senderToUpdate.Id);
//            result.CompanyName.Should().NotBe(senderToUpdate.CompanyName);
//        }

//        [Test]
//        public void Test_SenderIntegration_UpdateWithFancyNameOverflow_ShouldThrowException()
//        {
//            long existentId = 1;
//            _address = ObjectMother.GetExistentValidAddress();
//            Sender senderToUpdate = ObjectMother.GetInvalidSenderWithFancyNameLenghtOverflow(_address);
//            senderToUpdate.Id = existentId;

//            Action action = () => _service.Update(senderToUpdate);

//            action.Should().Throw<SenderFancyNameLenghtOverflowException>();
//            Sender result = _service.Get(senderToUpdate.Id);
//            result.CompanyName.Should().NotBe(senderToUpdate.CompanyName);
//        }

//        [Test]
//        public void Test_SenderIntegration_UpdateWithCompanyNameOverflow_ShouldThrowException()
//        {
//            long existentId = 1;
//            _address = ObjectMother.GetExistentValidAddress();
//            Sender senderToUpdate = ObjectMother.GetInvalidSenderWithCompanyNameLenghtOverflow(_address);
//            senderToUpdate.Id = existentId;

//            Action action = () => _service.Update(senderToUpdate);

//            action.Should().Throw<SenderCompanyNameLenghtOverflowException>();
//            Sender result = _service.Get(senderToUpdate.Id);
//            result.CompanyName.Should().NotBe(senderToUpdate.CompanyName);
//        }

//        [Test]
//        public void Test_SenderIntegration_UpdateWithStateRegistrationOverflow_ShouldThrowException()
//        {
//            long existentId = 1;
//            _address = ObjectMother.GetExistentValidAddress();
//            Sender senderToUpdate = ObjectMother.GetInvalidSenderWithStateRegistrationLenghtOverflow(_address);
//            senderToUpdate.Id = existentId;

//            Action action = () => _service.Update(senderToUpdate);

//            action.Should().Throw<SenderStateRegistrationLenghtOverflowException>();
//            Sender result = _service.Get(senderToUpdate.Id);
//            result.CompanyName.Should().NotBe(senderToUpdate.CompanyName);
//        }

//        [Test]
//        public void Test_SenderIntegration_UpdateWithMunicipalRegistrationOverflow_ShouldThrowException()
//        {
//            long existentId = 1;
//            _address = ObjectMother.GetExistentValidAddress();
//            Sender senderToUpdate = ObjectMother.GetInvalidSenderWithMunicipalRegistrationLenghtOverflow(_address);
//            senderToUpdate.Id = existentId;

//            Action action = () => _service.Update(senderToUpdate);

//            action.Should().Throw<SenderMunicipalRegistrationLenghtOverflowException>();
//            Sender result = _service.Get(senderToUpdate.Id);
//            result.CompanyName.Should().NotBe(senderToUpdate.CompanyName);
//        }

//        [Test]
//        public void Test_SenderIntegration_UpdateWithAddressNull_ShouldThrowException()
//        {
//            long existentId = 1;
//            _address = ObjectMother.GetExistentValidAddress();
//            Sender senderToUpdate = ObjectMother.GetInvalidSenderWithAddressNull();
//            senderToUpdate.Id = existentId;

//            Action action = () => _service.Update(senderToUpdate);

//            action.Should().Throw<SenderNullAddressException>();
//            Sender result = _service.Get(senderToUpdate.Id);
//            result.CompanyName.Should().NotBe(senderToUpdate.CompanyName);
//        }

//        [Test]
//        public void Test_SenderIntegration_UpdateWithAddressNegativeNumber_ShouldThrowException()
//        {
//            long existentId = 1;
//            _address = ObjectMother.GetInvalidAddressWithNegativeNumber();
//            _address.Id = existentId;
//            Sender senderToUpdate = ObjectMother.GetExistentValidSender(_address);

//            Action action = () => _service.Update(senderToUpdate);

//            action.Should().Throw<AddressNegativeNumberException>();
//            Sender result = _service.Get(senderToUpdate.Id);
//            result.CompanyName.Should().NotBe(senderToUpdate.CompanyName);
//        }

//        [Test]
//        public void Test_SenderIntegration_UpdateWithAddressUninformedStreetName_ShouldThrowException()
//        {
//            long existentId = 1;
//            _address = ObjectMother.GetInvalidAddressWithUninformedStreetName();
//            _address.Id = existentId;
//            Sender senderToUpdate = ObjectMother.GetExistentValidSender(_address);

//            Action action = () => _service.Update(senderToUpdate);

//            action.Should().Throw<AddressUninformedStreetNameException>();
//            Sender result = _service.Get(senderToUpdate.Id);
//            result.CompanyName.Should().NotBe(senderToUpdate.CompanyName);
//        }

//        [Test]
//        public void Test_SenderIntegration_UpdateWithAddressUninformedNeighborhood_ShouldThrowException()
//        {
//            long existentId = 1;
//            _address = ObjectMother.GetInvalidAddressWithUninformedNeighborhood();
//            _address.Id = existentId;
//            Sender senderToUpdate = ObjectMother.GetExistentValidSender(_address);

//            Action action = () => _service.Update(senderToUpdate);

//            action.Should().Throw<AddressUninformedNeighborhoodException>();
//            Sender result = _service.Get(senderToUpdate.Id);
//            result.CompanyName.Should().NotBe(senderToUpdate.CompanyName);
//        }

//        [Test]
//        public void Test_SenderIntegration_UpdateWithAddressUninformedCity_ShouldThrowException()
//        {
//            long existentId = 1;
//            _address = ObjectMother.GetInvalidAddressWithUninformedCity();
//            _address.Id = existentId;
//            Sender senderToUpdate = ObjectMother.GetExistentValidSender(_address);

//            Action action = () => _service.Update(senderToUpdate);

//            action.Should().Throw<AddressUninformedCityException>();
//            Sender result = _service.Get(senderToUpdate.Id);
//            result.CompanyName.Should().NotBe(senderToUpdate.CompanyName);
//        }

//        [Test]
//        public void Test_SenderIntegration_UpdateWithAddressUninformedState_ShouldThrowException()
//        {
//            long existentId = 1;
//            _address = ObjectMother.GetInvalidAddressWithUninformedState();
//            _address.Id = existentId;
//            Sender senderToUpdate = ObjectMother.GetExistentValidSender(_address);

//            Action action = () => _service.Update(senderToUpdate);

//            action.Should().Throw<AddressUninformedStateException>();
//            Sender result = _service.Get(senderToUpdate.Id);
//            result.CompanyName.Should().NotBe(senderToUpdate.CompanyName);
//        }

//        [Test]
//        public void Test_SenderIntegration_UpdateWithAddressUninformedCountry_ShouldThrowException()
//        {
//            long existentId = 1;
//            _address = ObjectMother.GetInvalidAddressWithUninformedCountry();
//            _address.Id = existentId;
//            Sender senderToUpdate = ObjectMother.GetExistentValidSender(_address);

//            Action action = () => _service.Update(senderToUpdate);

//            action.Should().Throw<AddressUninformedCountryException>();
//            Sender result = _service.Get(senderToUpdate.Id);
//            result.CompanyName.Should().NotBe(senderToUpdate.CompanyName);
//        }

//        [Test]
//        public void Test_SenderIntegration_UpdateWithAddressStreetOverflow_ShouldThrowException()
//        {
//            long existentId = 1;
//            _address = ObjectMother.GetInvalidAddressWithStreetNameLengthOverflow();
//            Sender senderToUpdate = ObjectMother.GetExistentValidSender(_address);
//            senderToUpdate.Id = existentId;

//            Action action = () => _service.Update(senderToUpdate);

//            action.Should().Throw<AddressStreetNameLengthOverflowException>();
//            Sender result = _service.Get(senderToUpdate.Id);
//            result.CompanyName.Should().NotBe(senderToUpdate.CompanyName);
//        }

//        [Test]
//        public void Test_SenderIntegration_UpdateWithAddressNeighborhoodNameOverflow_ShouldThrowException()
//        {
//            long existentId = 1;
//            _address = ObjectMother.GetInvalidAddressWithNeighborhoodNameLengthOverflow();
//            Sender senderToUpdate = ObjectMother.GetExistentValidSender(_address);
//            senderToUpdate.Id = existentId;

//            Action action = () => _service.Update(senderToUpdate);

//            action.Should().Throw<AddressNeighborhoodLengthOverflowException>();
//            Sender result = _service.Get(senderToUpdate.Id);
//            result.CompanyName.Should().NotBe(senderToUpdate.CompanyName);
//        }

//        [Test]
//        public void Test_SenderIntegration_UpdateWithAddressCityNameOverflow_ShouldThrowException()
//        {
//            long existentId = 1;
//            _address = ObjectMother.GetInvalidAddressWithCityNameLengthOverflow();
//            Sender senderToUpdate = ObjectMother.GetExistentValidSender(_address);
//            senderToUpdate.Id = existentId;

//            Action action = () => _service.Update(senderToUpdate);

//            action.Should().Throw<AddressCityLengthOverflowException>();
//            Sender result = _service.Get(senderToUpdate.Id);
//            result.CompanyName.Should().NotBe(senderToUpdate.CompanyName);
//        }

//        [Test]
//        public void Test_SenderIntegration_UpdateWithAddressStateNameOverflow_ShouldThrowException()
//        {
//            long existentId = 1;
//            _address = ObjectMother.GetInvalidAddressWithStateNameLengthOverflow();
//            Sender senderToUpdate = ObjectMother.GetExistentValidSender(_address);
//            senderToUpdate.Id = existentId;

//            Action action = () => _service.Update(senderToUpdate);

//            action.Should().Throw<AddressStateLengthOverflowException>();
//            Sender result = _service.Get(senderToUpdate.Id);
//            result.CompanyName.Should().NotBe(senderToUpdate.CompanyName);
//        }

//        [Test]
//        public void Test_SenderIntegration_UpdateWithAddressCountryNameOverflow_ShouldThrowException()
//        {
//            long existentId = 1;
//            _address = ObjectMother.GetInvalidAddressWithCountryNameLengthOverflow();
//            Sender senderToUpdate = ObjectMother.GetExistentValidSender(_address);
//            senderToUpdate.Id = existentId;

//            Action action = () => _service.Update(senderToUpdate);

//            action.Should().Throw<AddressCountryLengthOverflowException>();
//            Sender result = _service.Get(senderToUpdate.Id);
//            result.CompanyName.Should().NotBe(senderToUpdate.CompanyName);
//        }

//        [Test]
//        public void Test_SenderIntegration_UpdateInvalidId_ShouldThrowException()
//        {
//            _address = ObjectMother.GetExistentValidAddress();
//            Sender senderToUpdate = ObjectMother.GetNewValidSender(_address);
//            Action action = () => _service.Update(senderToUpdate);
//            action.Should().Throw<IdentifierUndefinedException>();
//        }

//        [Test]
//        public void Test_SenderIntegration_Delete_ShouldBeOk()
//        {
//            _address = ObjectMother.GetExistentValidAddress();
//            Sender senderToDele = ObjectMother.GetExistentValidSender(_address);
//            _service.Delete(senderToDele);
//            _service.Get(senderToDele.Id).Should().BeNull();
//        }

//        [Test]
//        public void Test_SenderIntegration_DeleteInvalidId_ShouldThrowException()
//        {
//            Sender senderToDelete = ObjectMother.GetNewValidSender(_address);

//            Action action = () => _service.Delete(senderToDelete);
//            action.Should().Throw<IdentifierUndefinedException>();
//        }



//    }
//}
