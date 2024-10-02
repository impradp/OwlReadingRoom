using MimeKit;
using MimeKit.Utils;
using OwlReadingRoom.DTOs;
using OwlReadingRoom.Models;
using OwlReadingRoom.Proxy;
using OwlReadingRoom.Services.Constants;
using OwlReadingRoom.Services.Email;
using OwlReadingRoom.Services.Resources;
using OwlReadingRoom.Services.Transactions;
using OwlReadingRoom.ViewModels;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace OwlReadingRoom.Services.Booking;

/// <summary>
/// The `NewPackageBookingStrategy` class is responsible for processing booking requests for new packages.
/// This strategy is used when a new package is assigned to a customer
/// </summary>
public class NewPackageBookingStrategy : BaseBookingStrategy
{
    private readonly IEmailService _emailService;
    private readonly ICustomerDetailsService _customerDetailsService;
    public NewPackageBookingStrategy(IBookingService bookingService, ITransactionService transactionService, IDeskService deskService, IEmailService emailService, ICustomerDetailsService customerDetailsService) : base(bookingService, transactionService, deskService)
    {
        _emailService = emailService;
        _customerDetailsService = customerDetailsService;
    }

    /// <summary>
    /// Processes the booking based on the provided view model and booking information.
    /// This method is responsible for updating the booking information, calculating the total amount and due amount, creating a new transaction, and saving the transaction log if needed.
    /// This method also sends an email regarding the new pacakge assignment.
    /// </summary>
    /// <param name="packagePaymentDetail">The package and payment edit view model, which contains the details of the new package and payment information.</param>
    /// <param name="bookingInfo">The booking information, which contains the details of the existing booking.</param>
    [Transactional]
    public override void Process(PackageAndPaymentEditViewModel packagePaymentDetail, BookingInfo bookingInfo)
    {
        // Update Booking Information
        UpdateBookingInformation(bookingInfo, packagePaymentDetail);

        // Calculate total amount and due amount
        decimal totalAmount = CalculateTotalAmount(packagePaymentDetail);
        decimal dueAmount = CalculateDueAmount(totalAmount, (decimal)packagePaymentDetail.PaidAmount);

        // Create and save the transaction
        Transaction transaction = CreateTransaction(bookingInfo.Id, totalAmount, dueAmount, packagePaymentDetail.PaidAmount, packagePaymentDetail.LockerAmount, packagePaymentDetail.ParkingAmount);
        _transactionService.SaveTransaction(transaction);

        // Create and save transaction log if needed
        if (packagePaymentDetail.PaidAmount > 0)
        {
            SaveTransactionLog(transaction.Id, packagePaymentDetail.PaidAmount, packagePaymentDetail.LastPaymentDate, packagePaymentDetail.PaymentMethod);
        }

        // Retrieve customer's name for email body
        MinimumCustomerDetail customerDetail = _customerDetailsService.GetCustomerMinimumDetail(bookingInfo.CustomerId);

        // Fire-and-forget email sending (use with caution)
        _ = Task.Run(async () =>
        {
            try
            {
                string subject = $"New Subscription: {packagePaymentDetail.Package.Name} for {customerDetail.FullName}";
                var bodyBuilder = await CreateBodyForPackageExpiryNotification(customerDetail, packagePaymentDetail);
                await _emailService.SendEmailAsync(subject, bodyBuilder);
            }
            catch (Exception ex)
            {
                // Log the exception for retry or monitoring purposes
                Console.WriteLine($"Failed to send email: {ex.Message}");
            }
        });
    }

    /// <summary>
    /// Creates the HTML body for the new package assignment notification email.
    /// It reads the email template, inserts customer information, and adds an embedded image.
    /// </summary>
    /// <param name="customerDetail">Minimum detail of the customer who subscribed the pacakge.</param>
    /// <param name="packagePaymentDetail">The package and payment edit view model, which contains the details of the new package and payment information.</param>
    /// <returns>A BodyBuilder object containing the email body and embedded resources.</returns>
    private async Task<BodyBuilder> CreateBodyForPackageExpiryNotification(MinimumCustomerDetail customerDetail, PackageAndPaymentEditViewModel packagePaymentDetail)
    {
        var bodyBuilder = new BodyBuilder();
        string emailTemplate = await ReadEmailTemplateAsync("customer_package_renew.html");

        var htmlBody = new StringBuilder(emailTemplate);
        var tableRows = new StringBuilder();

        // Set expiration threshold date and replace placeholders in the template
        var packageStartDate = packagePaymentDetail.PackageStartDate?.Date.ToString("dddd, dd MMMM yyyy", CultureInfo.CreateSpecificCulture("en-US"));
        htmlBody.Replace("{FullName}", customerDetail.FullName)
                .Replace("{PackageName}", packagePaymentDetail.Package.Name)
                .Replace("{PackageStartDate}", packageStartDate);

        await AddEmbeddedImageAsync(bodyBuilder, htmlBody, "OwlReadingRoom.Resources.Images.owl_logo.png");

        bodyBuilder.HtmlBody = htmlBody.ToString();
        return bodyBuilder;
    }

    /// <summary>
    /// Adds an embedded image to the email body and replaces the image placeholder with its Content-ID.
    /// </summary>
    /// <param name="bodyBuilder">The BodyBuilder object used to build the email.</param>
    /// <param name="htmlBody">The StringBuilder containing the HTML body of the email.</param>
    /// <param name="resourcePath">The resource path of the image.</param>
    /// 
    //TODO: make this logic common and move it to a utility class
    private async Task AddEmbeddedImageAsync(BodyBuilder bodyBuilder, StringBuilder htmlBody, string resourcePath)
    {
        var assembly = Assembly.GetExecutingAssembly();
        using (var imageStream = assembly.GetManifestResourceStream(resourcePath))
        {
            if (imageStream == null) throw new FileNotFoundException($"Image resource not found in assembly: {resourcePath}");

            using (var memoryStream = new MemoryStream())
            {
                await imageStream.CopyToAsync(memoryStream);
                var imageData = memoryStream.ToArray();

                var image = new MimePart("image", "png")
                {
                    Content = new MimeContent(new MemoryStream(imageData)),
                    ContentDisposition = new ContentDisposition(ContentDisposition.Inline),
                    ContentTransferEncoding = ContentEncoding.Base64,
                    ContentId = MimeUtils.GenerateMessageId()
                };

                bodyBuilder.LinkedResources.Add(image);

                // Replace the image placeholder {0} with the generated Content-ID
                htmlBody.Replace("{0}", image.ContentId);
            }
        }
    }

    /// <summary>
    /// Updates the booking information based on the provided view model.
    /// </summary>
    /// <param name="bookingInfo">The booking information to be updated.</param>
    /// <param name="packagePaymentDetail">The package and payment edit view model, which contains the updated package details.</param>
    private void UpdateBookingInformation(BookingInfo bookingInfo, PackageAndPaymentEditViewModel packagePaymentDetail)
    {
        bookingInfo.PackageId = packagePaymentDetail.Package.Id;
        // Set ReservationStartDate to 12:00 AM (start of the day)
        bookingInfo.ReservationStartDate = packagePaymentDetail.PackageStartDate?.Date;
        // Set ReservationEndDate to 11:59 PM (end of the day)
        bookingInfo.ReservationEndDate = packagePaymentDetail.PackageEndDate?.Date.AddDays(1).AddSeconds(-1) ?? DateTime.MinValue;

        Desk desk = _deskService.TableQuery.FirstOrDefault(d => d.RoomId == packagePaymentDetail.Room.Id && d.Name.Equals(packagePaymentDetail.DeskName));

        bookingInfo.DeskId = desk.Id;
        _bookingService.SaveItem(bookingInfo);
    }

    /// <summary>
    /// Calculates the total amount based on the provided view model.
    /// The total amount includes the package amount, locker fee, and parking fee.
    /// </summary>
    /// <param name="packagePaymentDetail">The package and payment edit view model, which contains the package details.</param>
    /// <returns>The calculated total amount.</returns>
    private decimal CalculateTotalAmount(PackageAndPaymentEditViewModel packagePaymentDetail)
    {
        decimal packageAmount = (decimal)packagePaymentDetail.Package.Price;

        decimal lockerAmount = (decimal)packagePaymentDetail.LockerAmount;
        decimal parkingAmount = (decimal)packagePaymentDetail.ParkingAmount;

        // Calculate and round total amount
        decimal totalAmount = packageAmount + lockerAmount + parkingAmount;
        return Math.Round(totalAmount, 2, MidpointRounding.ToEven);
    }

    /// <summary>
    /// Calculates the due amount based on the total amount and the paid amount.
    /// </summary>
    /// <param name="totalAmount">The total amount.</param>
    /// <param name="paidAmount">The paid amount.</param>
    /// <returns>The calculated due amount.</returns>
    private decimal CalculateDueAmount(decimal totalAmount, decimal paidAmount)
    {
        decimal dueAmount = totalAmount - (decimal)paidAmount;
        return Math.Round(dueAmount, 2, MidpointRounding.ToEven);
    }

    /// <summary>
    /// Creates a new transaction based on the provided information.
    /// </summary>
    /// <param name="bookingInfoId">The booking information ID.</param>
    /// <param name="totalAmount">The total amount.</param>
    /// <param name="dueAmount">The due amount.</param>
    /// <param name="paidAmount">The paid amount.</param>
    /// <returns>The new transaction.</returns>
    private Transaction CreateTransaction(int bookingInfoId, decimal totalAmount, decimal dueAmount, double paidAmount, double lockerAmount, double parkingAmount)
    {
        return new Transaction
        {
            BookingInformationId = bookingInfoId,
            PaymentStatus = GetPaymentStatus(dueAmount, totalAmount),
            DueAmount = decimal.ToDouble(dueAmount),
            TotalAmount = decimal.ToDouble(totalAmount),
            PaidAmount = paidAmount,
            LockerAmount = lockerAmount,
            ParkingAmount = parkingAmount,
        };
    }

    /// <summary>
    /// Reads the email template from an embedded resource.
    /// </summary>
    /// <param name="fileName">The name of the template file.</param>
    /// <returns>The content of the template as a string.</returns>
    private static async Task<string> ReadEmailTemplateAsync(string fileName)
    {
        try
        {
            var assembly = Assembly.GetExecutingAssembly();
            using (var stream = assembly.GetManifestResourceStream($"OwlReadingRoom.Resources.EmailTemplate.{fileName}"))
            using (var reader = new StreamReader(stream ?? throw new FileNotFoundException($"Email template file not found: {fileName}")))
            {
                return await reader.ReadToEndAsync();
            }
        }
        catch (FileNotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error reading email template: {ex.Message}", ex);
        }
    }

    public override string SupportedType()
    {
        return BookingStrategyNames.NewPackage;
    }
}
