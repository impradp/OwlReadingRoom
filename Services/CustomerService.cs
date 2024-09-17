using OwlReadingRoom.DTOs;
using OwlReadingRoom.Models;
using OwlReadingRoom.Proxy;
using OwlReadingRoom.Services.Repository;
using OwlReadingRoom.ViewModels;
using SQLite;
using System.Diagnostics;

namespace OwlReadingRoom.Services;

public class CustomerService : ICustomerService
{
    private readonly IRepository<Customer> _customerRepository;
    private readonly IRepository<DocumentInformation> _documentRepository;
    private readonly IRepository<DocumentImage> _documentImageRepository;
    private readonly IRepository<PersonalDetail> _personalDetailRepository;
    private readonly IRepository<Address> _addressRepository;

    public TableQuery<PersonalDetail> PersonalDetailTableQuery => _personalDetailRepository.Table;

    public TableQuery<DocumentInformation> DocumentTableQuery => _documentRepository.Table;

    public TableQuery<Address> AddressTableQuery => _addressRepository.Table;

    public TableQuery<Customer> CustomerTableQuery => _customerRepository.Table;

    public TableQuery<DocumentImage> DocumentImageTableQuery => _documentImageRepository.Table;

    public CustomerService(IRepository<Customer> customerRepository, IRepository<PersonalDetail> personalDetailRepository, IRepository<DocumentInformation> documentRepository, IRepository<Address> addressRepository, IRepository<DocumentImage> documentImageRepository)
    {
        _customerRepository = customerRepository;
        _personalDetailRepository = personalDetailRepository;
        _documentRepository = documentRepository;
        _addressRepository = addressRepository;
        _documentImageRepository = documentImageRepository;
    }

    public Customer GetCustomerByMobileNumber(string mobileNumber)
    {
        return _customerRepository.Table.Where(customer => customer.MobileNumber.Equals(mobileNumber)).FirstOrDefault();
    }
    public Customer CreateNewCustomerWithMinimumDetails(MinimumCustomerDetail minimumCustomerDetail)
    {
        Customer customer = CreateCustomer(minimumCustomerDetail.ContactNumber);
        _customerRepository.SaveItem(customer);
        customer.PersonalDetail = SavePersonalDetail(minimumCustomerDetail.FullName, customer.Id);
        customer.Address = SaveTemporaryAddress(minimumCustomerDetail.CurrentAddress, customer.Id);
        return customer;
    }

    public PersonalDetail GetPersonalDetails(int customerId)
    {
        return _personalDetailRepository.Table.Where(p => p.CustomerId == customerId).FirstOrDefault();
    }
    public DocumentViewModel GetCustomerDocumentDetails(int customerId)
    {
        var query = from customer in _customerRepository.Table
                    join document in _documentRepository.Table on customer.Id equals document.CustomerId
                    join documentImage in _documentImageRepository.Table on document.Id equals documentImage.DocumentInformationId
                    where customer.Id == customerId
                    group new DocumentImageViewModel
                    {
                        Id = documentImage.Id,
                        ImagePath = documentImage.ImagePath,
                    } by document into documentGroup
                    let documentInfo = documentGroup.Key
                    select new DocumentViewModel
                    {
                        Id = documentInfo.Id,
                        DocumentNumber = documentInfo.DocumentNumber,
                        DocumentType = documentInfo.DocumentType,
                        IssueDate = documentInfo.IssueDate.ToShortDateString(),
                        PlaceOfIssue = documentInfo.PlaceOfIssue,
                        Locations = documentGroup.ToList()
                    };

        return query.FirstOrDefault();

    }

    [Transactional]
    public void RegisterWithMinimumDetails(MinimumCustomerDetail minimumCutomerDetail)
    {
        Customer customer = GetCustomerByMobileNumber(minimumCutomerDetail.ContactNumber);
        if (customer is not null)
        {
            Debug.WriteLine($"A customer with the contact number :{minimumCutomerDetail.ContactNumber} already exists.");
            throw new InvalidDataException("Duplicate user entry.");
        }
        this.CreateNewCustomerWithMinimumDetails(minimumCutomerDetail);
    }

    private Address SaveTemporaryAddress(string currentAddress, int customerId)
    {
        Address address = new Address()
        {
            TemporaryAddress = currentAddress,
            CustomerId = customerId
        };
        _addressRepository.SaveItem(address);
        return address;
    }

    private Address SaveAddress(string permanentAddress, string temporaryAddress, int customerId)
    {
        Address address = new Address()
        {
            PermanentAddress = permanentAddress,
            TemporaryAddress = temporaryAddress,
            CustomerId = customerId
        };
        _addressRepository.SaveItem(address);
        return address;
    }
    private PersonalDetail SavePersonalDetail(string fullName, int customerId)
    {
        PersonalDetail personalDetail = new PersonalDetail
        {
            FullName = fullName,
            CustomerId = customerId,
            DOB = null
        };
        _personalDetailRepository.SaveItem(personalDetail);
        return personalDetail;
    }

    private Customer CreateCustomer(string contactNumber)
    {
        return new Customer
        {
            MobileNumber = contactNumber
        };
    }

    public Address GetAddressDetails(int customerId)
    {
        return _addressRepository.Table.Where(add
            => add.CustomerId == customerId)
            .FirstOrDefault();
    }
}
