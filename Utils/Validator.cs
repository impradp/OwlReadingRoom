using OwlReadingRoom.Models;

namespace OwlReadingRoom.Utils
{
    public class Validator
    {
        private Validator() { }
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

        public static Boolean IsValidNewCustomer(String fullName, String contactNumber, int packageTypeIndex, int paymentTypeIndex, String filePath)
        {
           if (string.IsNullOrWhiteSpace(fullName))
            {
                return ShowError(ValidationMessages.EmptyFullName);
            }

            if (string.IsNullOrWhiteSpace(contactNumber) || contactNumber.Trim().Equals("+977-"))
            {
                return ShowError(ValidationMessages.InvalidContactNumber);
            }

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

        public static Boolean isValidRoomName(String roomName)
        {
            if (string.IsNullOrEmpty(roomName))
            {
                return ShowError(ValidationMessages.EmptyRoomName);
            }
            return true;
        }

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
