using OwlReadingRoom.DTOs;
using OwlReadingRoom.Models;
using OwlReadingRoom.Services.Repository;

namespace OwlReadingRoom.Services;

public class CustomerService : ICustomerService
{
    private readonly IRepository<Customer> _customerRepository;
    private readonly IRepository<Document> _documentRepository;
    private readonly IRepository<PersonalDetail> _personalDetailRepository;

    public CustomerService(IRepository<Customer> customerRepository, IRepository<PersonalDetail> personalDetailRepository, IRepository<Document> documentRepository)
    {
        _customerRepository = customerRepository;
        _personalDetailRepository = personalDetailRepository;
        _documentRepository = documentRepository;
    }

    public int CreateNewCustomerWithMinimumDetails(MinimumCustomerDetail minimumCustomerDetail)
    {
        int personalDetailId = SavePersonalDetail(minimumCustomerDetail.FullName);
        int documentId = SaveDocument(minimumCustomerDetail.DocumentPath);

        Customer customer = CreateCustomer(minimumCustomerDetail.ContactNumber, personalDetailId, documentId);

        _customerRepository.SaveItem(customer);
        return customer.Id;
    }

    private int SavePersonalDetail(string fullName)
    {
        PersonalDetail personalDetail = new PersonalDetail
        {
            FullName = fullName
        };
        _personalDetailRepository.SaveItem(personalDetail);
        return personalDetail.Id;
    }

    private int SaveDocument(string documentPath)
    {
        Document document = new Document
        {
            Location = documentPath
        };
        _documentRepository.SaveItem(document);
        return document.Id;
    }

    private Customer CreateCustomer(string contactNumber, int personalDetailId, int documentId)
    {
        return new Customer
        {
            MobileNumber = contactNumber,
            PersonalDetailId = personalDetailId,
            DocumentId = documentId
        };
    }
}
