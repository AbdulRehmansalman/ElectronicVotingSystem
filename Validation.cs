using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ElectronicVotingSystem
{
    // proxy pattern 
    public static class ValidationProxy
    {
        private static Validation validation = new Validation();

        public static bool ValidateSignUpFields(string name, string username, string password, string contact, string postalCode)
        {
            return Validation.ValidateSignUpFields(name, username, password, contact, postalCode);
        }

        public static bool ValidateLoginFields(string username, string password)
        {
            return Validation.ValidateLoginFields(username, password);
        }
    }
    public class Validation
    {
        public static bool ValidateSignUpFields(string name, string username, string password, string contact, string postalCode)
        {
            // Check if any of the fields are empty
            if (string.IsNullOrWhiteSpace(name) ||
                string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(contact) ||
                string.IsNullOrWhiteSpace(postalCode))
            {
                return false;
            }

            // Additional validation for each field (e.g., length, format, etc.)
            else if (!IsValidUsername(username))
            {
                return false;
            }

            else if (!IsValidPassword(password))
            {
                return false;
            }
            else if (!IsValidContact(contact))
            {
                return false;
            }
            else if (!IsValidateName(name))
            {
                return false;
            }
            return true;
        }
        public static bool ValidateLoginFields(string username, string password)
        {
            // Check if any of the fields are empty
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return false;
            }
            // Validate username and password format
            else if (!IsValidUsername(username))
            {
                return false;
            }

            else if (!IsValidPassword(password))
            {
                return false;
            }
            
            else
            {
                return true;
            }
        }
        // For Create Candidate Class
        public static bool CreateCandidateValidate(string name, string username, string password, string contact, string votingSign, string partyName, string postalCode)
        {
            // Check if any of the fields are empty
            if (string.IsNullOrWhiteSpace(name) ||
                string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(contact) ||
                string.IsNullOrWhiteSpace(votingSign) ||
                string.IsNullOrWhiteSpace(partyName) ||
                string.IsNullOrWhiteSpace(postalCode))
            {
                return false;
            }

            // Validate individual fields as needed
            else if (!IsValidateName(name))
            {
                return false;
            }

            else if (!IsValidUsername(username))
            {
                return false;
            }

            else if (!IsValidPassword(password))
            {
                return false;
            }

            else if (!IsValidContact(contact))
            {
                return false;
            }
            else
            {
                return true;

            }
        }
        public static bool IsValidateName(string name)
        {
            // Check name field contains alphabets and has a length between 2 and 13
            return Regex.IsMatch(name, @"^[a-zA-Z]{2,13}$");
        }
        private static bool IsValidUsername(string username)
        {
            //contain only letter(special) and number between 3 and 16. 
            return Regex.IsMatch(username, @"^[a-zA-Z0-9_]{3,16}$");

        }

        private static bool IsValidPassword(string password)
        {
           //password must be at least 8 characters long and contain at least one uppercase and lowercase letter, and one digit.
            return Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$");
        }
        private static bool IsValidContact(string contact)
        {
            //contains Only Numbers and 10min and 15 Digits max
            return Regex.IsMatch(contact, @"^\d{10,15}$");
        }

    }
}
