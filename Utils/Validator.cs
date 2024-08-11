using System;

namespace OwlReadingRoom.Utils
{
    public class Validator
    {
        private Validator() { }
        public static Boolean IsValidDocument(FileResult? file)
        {
            Boolean isValid = true;
            if(file is null || (!file.FileName.EndsWith("jpg", StringComparison.OrdinalIgnoreCase) &&
                    !file.FileName.EndsWith("png", StringComparison.OrdinalIgnoreCase) &&
                    !file.FileName.EndsWith("pdf", StringComparison.OrdinalIgnoreCase)))
            {
                CustomAlert.ShowAlert("Error", "Please select a valid file (jpg, png, or pdf)", "OK");
                isValid = false;
            }
            return isValid;

        }

        public static Boolean IsValidNewCustomer(String fullName, String contactNumber, int packageTypeIndex, int paymentTypeIndex, String filePath)
        {
            Boolean isValid = true;
            if (string.IsNullOrWhiteSpace(fullName))
            {
                CustomAlert.ShowAlert("Error", "Please enter a full name.", "OK");
                isValid=false;
                return isValid;

            }

            if (string.IsNullOrWhiteSpace(contactNumber) || contactNumber.Trim().Equals("+977-"))
            {
                CustomAlert.ShowAlert("Error", "Please enter a contact number.", "OK");
                isValid = false;
                return isValid;
            }

            if (packageTypeIndex == -1)
            {
                CustomAlert.ShowAlert("Error", "Please select a package type.", "OK");
                isValid = false;
                return isValid;
            }

            if (paymentTypeIndex== -1)
            {
                CustomAlert.ShowAlert("Error", "Please select a payment type.", "OK");
                isValid = false;
                return isValid;
            }

            if (string.IsNullOrEmpty(filePath))
            {
                CustomAlert.ShowAlert("Error", "Please select a valid file (jpg, png, or pdf)", "OK");
                isValid = false;
                return isValid;
            }
            
            return isValid;
        }
    }
}
