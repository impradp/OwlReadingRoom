using OwlReadingRoom.Models;

namespace OwlReadingRoom.Utils
{
    /// <summary>
    /// Class that contains the list of functions to validate the details across the system.
    /// </summary>
    public class Validator
    {
        private Validator() { }

        /// <summary>
        /// Checks the validity of the document.
        /// </summary>
        /// <param name="file">The file object container the metadata of file.</param>
        /// <returns>Boolean value depending upon the file validation result.</returns>
        public static Boolean IsValidDocument(FileResult? file)
        {
            if (file is null || (!file.FileName.EndsWith("jpg", StringComparison.OrdinalIgnoreCase) &&
                    !file.FileName.EndsWith("png", StringComparison.OrdinalIgnoreCase) &&
                    !file.FileName.EndsWith("pdf", StringComparison.OrdinalIgnoreCase)))
            {
                return ShowError(ValidationMessages.InvalidFile);
            }
            return true;

        }

        /// <summary>
        /// Validates the additional customer details required during customer update or document update.
        /// </summary>
        /// <param name="packageTypeIndex">The index of selected package.</param>
        /// <param name="paymentTypeIndex">The index of selected payment type.</param>
        /// <param name="filePath"></param>
        /// <returns>Boolean value indicating the validation fo the customer details.</returns>
        private static Boolean ValidateAdditionalCustomerDetails(int? packageTypeIndex, int? paymentTypeIndex, String filePath)
        {
            if (packageTypeIndex == -1)
            {
                return ShowError(ValidationMessages.NoPackageSelected);
            }

            if (paymentTypeIndex == -1)
            {
                return ShowError(ValidationMessages.NoPaymentTypeSelected);
            }

            if (string.IsNullOrEmpty(filePath))
            {
                return ShowError(ValidationMessages.InvalidFile);
            }
            return true;
        }

        /// <summary>
        /// Handles the validation of new registration of customers.
        /// </summary>
        /// <param name="fullName">The full name of the customer.</param>
        /// <param name="contactNumber">The mobile number of the customer.</param>
        /// <param name="validateAdditionalFields">The flag to indicate whether to validate additional fields or not.</param>
        /// <param name="packageTypeIndex">The index of the selected package type.</param>
        /// <param name="paymentTypeIndex">The index of the selected payment type.</param>
        /// <param name="filePath">The path of the file to be copied from.</param>
        /// <returns>Boolean flag indicating the validation of new customer.</returns>
        public static Boolean IsValidNewCustomer(String fullName, String contactNumber, Boolean validateAdditionalFields, int? packageTypeIndex, int? paymentTypeIndex, String filePath)
        {
            if (string.IsNullOrWhiteSpace(fullName))
            {
                return ShowError(ValidationMessages.EmptyFullName);
            }

            if (string.IsNullOrWhiteSpace(contactNumber) || contactNumber.Trim().Equals("+977-"))
            {
                return ShowError(ValidationMessages.InvalidContactNumber);
            }

            if (validateAdditionalFields)
            {
                return ValidateAdditionalCustomerDetails(packageTypeIndex, paymentTypeIndex, filePath);
            }

            return true;
        }

        /// <summary>
        /// Handles the validation of room entry.
        /// </summary>
        /// <param name="numberOfRooms">The number of rooms to be created.</param>
        /// <param name="roomTypeIndex">The type of room to be created.</param>
        /// <returns>Boolean flag indicating the validity of the room.</returns>
        public static Boolean IsValidRoom(String numberOfRooms, int roomTypeIndex)
        {
            if (roomTypeIndex == -1)

            {
                return ShowError(ValidationMessages.NoRoomTypeSelected);
            }
            if (string.IsNullOrEmpty(numberOfRooms))
            {
                return ShowError(ValidationMessages.EmptyNumberOfRooms);
            }

            if (!int.TryParse(numberOfRooms, out int number))
            {
                return ShowError(ValidationMessages.InvalidNumberOfRooms);
            }

            return true;
        }

        /// <summary>
        /// Validates the room name.
        /// </summary>
        /// <param name="roomName">The name of the room to be updated.</param>
        /// <returns></returns>
        public static Boolean isValidRoomName(String roomName)
        {
            if (string.IsNullOrEmpty(roomName))
            {
                return ShowError(ValidationMessages.EmptyRoomName);
            }
            return true;
        }

        /// <summary>
        /// Checks the validity of the package.
        /// </summary>
        /// <param name="packageName">The name of package to be created.</param>
        /// <param name="packageDays">The number of days valid for a package.</param>
        /// <param name="packageAmount">The amount assigned for the package.</param>
        /// <param name="roomTypeIndex">The type of room selected for the package.</param>
        /// <returns></returns>
        public static Boolean isValidPackage(String packageName, string packageDays, string packageAmount, int roomTypeIndex)
        {
            if (roomTypeIndex == -1)
            {
                return ShowError(ValidationMessages.NoRoomTypeSelected);
            }
            if (!double.TryParse(packageAmount, out double amount) && amount <= 0)
            {
                return ShowError(ValidationMessages.InvalidAmount);
            }

            if (!int.TryParse(packageDays, out int days) && days <= 0)
            {
                return ShowError(ValidationMessages.InvalidDays);
            }
            return true;
        }

        /// <summary>
        /// Shows the error message with detailed error message.
        /// </summary>
        /// <param name="message">The message passed down to display upon error.</param>
        /// <returns>The boolean value indicating that the error has been encountered.</returns>
        private static bool ShowError(string message)
        {
            CustomAlert.ShowAlert("Error", message, "OK");
            return false;
        }
    }

    public static class ValidationMessages
    {
        public const string InvalidFile = "Please select a valid file (jpg, png, or pdf)";
        public const string EmptyFullName = "Please enter a full name.";
        public const string InvalidContactNumber = "Please enter a contact number.";
        public const string NoPackageSelected = "Please select a package type.";
        public const string NoPaymentTypeSelected = "Please select a payment type.";
        public const string NoRoomTypeSelected = "Please select a room type.";
        public const string EmptyNumberOfRooms = "Please enter the number of rooms.";
        public const string InvalidNumberOfRooms = "Please enter a valid whole number.";
        public const string EmptyRoomName = "Please enter a valid room name.";
        public const string InvalidAmount = "Please enter valid amount.";
        public const string InvalidDays = "Please enter a valid number of days.";
    }
}
