using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;

namespace APP.Common.Utilities
{
    public enum CharacterTypes : byte
    {
        Alpha_Lower = 1,
        Alpha_Upper = 2,
        Alpha_Upper_and_Lower = 3,
        Digit = 4,
        AlphaLowerNumeric = Digit + Alpha_Lower,        //  5 (4+1)
        AlphaUpperNumeric = Digit + Alpha_Upper,        //  6 (4+2)
        AlphaNumeric = Alpha_Upper_and_Lower + Digit,   //  7 (4+3)
        Special = 8,
        // You could add more character types here such as Alpha_Lower  + Special, but why?
        AlphaNumericSpecial = AlphaNumeric + Special    // 15 (8+7)
    }

    public class RandomPasswordGenerator
    {
        // Define default password length.
        private static int DEFAULT_PASSWORD_LENGTH = 16;

        private static PasswordOption AlphaLC = new PasswordOption() { Characters = "abcdefghijklmnopqrstuvwxyz", Count = 0 };
        private static PasswordOption AlphaUC = new PasswordOption() { Characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ", Count = 0 };
        private static PasswordOption Digits = new PasswordOption() { Characters = "0123456789", Count = 0 };
        private static PasswordOption Specials = new PasswordOption() { Characters = "!@#$%^&*()~<>?", Count = 0 };

        #region Overloads

        /// <summary>
        /// Generates a random password with the default length.
        /// </summary>
        /// <returns>Randomly generated password.</returns>
        public static SecureString Generate()
        {
            return Generate(DEFAULT_PASSWORD_LENGTH,
                            CharacterTypes.AlphaNumericSpecial);
        }

        /// <summary>
        /// Generates a random password with the default length.
        /// </summary>
        /// <returns>Randomly generated password.</returns>
        public static SecureString Generate(CharacterTypes option)
        {
            return Generate(DEFAULT_PASSWORD_LENGTH, option);
        }

        /// <summary>
        /// Generates a random password with the specified length.
        /// </summary>
        /// <returns>Randomly generated password.</returns>
        public static SecureString Generate(int passwordLength)
        {
            return Generate(passwordLength,
                            CharacterTypes.AlphaNumericSpecial);
        }

        /// <summary>
        /// Generates a random password.
        /// </summary>
        /// <returns>Randomly generated password.</returns>
        public static SecureString Generate(int passwordLength,
                                      CharacterTypes option)
        {
            return GeneratePassword(passwordLength, option);
        }

        #endregion

        /// <summary>
        /// Generates the password.
        /// </summary>
        /// <returns></returns>
        private static SecureString GeneratePassword(int passwordLength, CharacterTypes option)
        {
            // Password length must at lest be 1 character long
            if (passwordLength < 1)
                throw new InvalidPasswordLengthException();

            // Character type must be a valid CharacterType
            if (option < CharacterTypes.Alpha_Lower || option > CharacterTypes.AlphaNumericSpecial)
                throw new InvalidPasswordCharacterTypeException();

            PasswordOptions passwordOptions = GetCharacters(option);

            // Make sure the password is long enough.
            // For example CharacterTypes.AlphaNumericSpecial
            // requires at least 4 characters: 1 upper, 1 lower, 1 digit, 1 special
            if (passwordLength < passwordOptions.Count)
                throw new InvalidPasswordLengthException();

            SecureString securePassword = new SecureString();
            string passwordChars = String.Empty;

            foreach (PasswordOption po in passwordOptions)
            {
                passwordChars += po.Characters;
            }

            if (string.IsNullOrEmpty(passwordChars))
                return null;

            var random = RandomSeedGenerator.GetRandom();

            for (int i = 0; i < passwordLength; i++)
            {
                int index;
                char passwordChar;
                if (!passwordOptions.AllOptionsAreUsed)
                {
                    PasswordOption po = passwordOptions.GetUnusedOption();
                    index = random.Next(po.Characters.Length);
                    passwordChar = po.Characters[index];
                }
                else
                {
                    index = random.Next(passwordChars.Length);
                    passwordChar = passwordChars[index];
                }

                securePassword.AppendChar(passwordChar);
            }

            return securePassword;
        }

        private int GetOptionsUsed()
        {
            int ret = 0;
            foreach (CharacterTypes option in Enum.GetValues(typeof(CharacterTypes)))
            {

            }
            return ret;
        }

        /// <summary>
        /// Gets the characters selected by the option
        /// </summary>
        /// <returns></returns>
        private static PasswordOptions GetCharacters(CharacterTypes option)
        {
            PasswordOptions list = new PasswordOptions();
            switch (option)
            {
                case CharacterTypes.Alpha_Lower:
                    list.Add(AlphaLC);
                    break;
                case CharacterTypes.Alpha_Upper:
                    list.Add(AlphaUC);
                    break;
                case CharacterTypes.Alpha_Upper_and_Lower:
                    list.Add(AlphaLC);
                    list.Add(AlphaUC);
                    break;
                case CharacterTypes.Digit:
                    list.Add(Digits);
                    break;
                case CharacterTypes.AlphaNumeric:
                    list.Add(AlphaLC);
                    list.Add(AlphaUC);
                    list.Add(Digits);
                    break;
                case CharacterTypes.Special:
                    list.Add(Specials);
                    break;
                case CharacterTypes.AlphaNumericSpecial:
                    list.Add(AlphaLC);
                    list.Add(AlphaUC);
                    list.Add(Digits);
                    list.Add(Specials);
                    break;
                default:
                    break;
            }
            return list;
        }
    }

    public static class RandomSeedGenerator
    {
        /// <summary>
        /// Gets a random object with a real random seed
        /// </summary>
        /// <returns></returns>
        public static Random GetRandom()
        {
            // Use a 4-byte array to fill it with random bytes and convert it then
            // to an integer value.
            byte[] randomBytes = new byte[4];

            // Generate 4 random bytes.
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(randomBytes);

            // Convert 4 bytes into a 32-bit integer value.
            int seed = (randomBytes[0] & 0x7f) << 24 |
                        randomBytes[1] << 16 |
                        randomBytes[2] << 8 |
                        randomBytes[3];

            // Now, this is real randomization.
            return new Random(seed);
        }
    }

    public class PasswordOption
    {
        public int Count { get; set; }
        public String Characters { get; set; }
    }

    public class PasswordOptions : List<PasswordOption>
    {
        public bool AllOptionsAreUsed
        {
            get
            {
                foreach (PasswordOption po in this)
                {
                    if (po.Count < 1)
                        return false;
                }
                return true;
            }
        }

        public PasswordOption GetUnusedOption()
        {
            PasswordOptions options = new PasswordOptions();
            foreach (PasswordOption po in this)
            {
                if (po.Count < 1)
                    options.Add(po);
            }
            if (options.Count < 1)
                return null;

            var random = RandomSeedGenerator.GetRandom();
            int optionIndex = random.Next(options.Count);
            return options[optionIndex];
        }
    }

    public class InvalidPasswordLengthException : ArgumentException { }
    public class InvalidPasswordCharacterTypeException : ArgumentException { }

    public static class SecureStringExtender
    {
        public static string ConvertToPlainTextString(this SecureString securePassword)
        {
            if (securePassword == null)
                throw new ArgumentNullException("securePassword");

            IntPtr unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(securePassword);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }

        public static SecureString Scramble(this SecureString securePassword)
        {
            SecureString retSS = securePassword;
            Random random = RandomSeedGenerator.GetRandom();
            int moves = random.Next(securePassword.Length, securePassword.Length * 2);
            for (int i = 0; i < moves; i++)
            {
                int origIndex = random.Next(securePassword.Length);
                int newIndex = random.Next(securePassword.Length);
                char c = retSS.GetAt(origIndex);
                retSS.InsertAt(newIndex, c);
            }
            return retSS;
        }

        public static Char GetAt(this SecureString securePassword, int index)
        {
            if (securePassword.Length < index)
                throw new ArgumentException("The index parameter must not be greater than the string's length.");
            if (index < 0)
                throw new ArgumentException("The index must be 0 or greater.");
            return securePassword.ConvertToPlainTextString().Substring(index, 1).ToCharArray()[0];
        }
    }
}
