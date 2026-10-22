using System.Net.Mail;
using System.Text.RegularExpressions;

namespace Veterinaria.Common.Helpers
{
    public static class Helper
    {
        private static readonly Regex EmailRegex = new(
      @"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?)+$",
      RegexOptions.Compiled | RegexOptions.CultureInvariant
  );

        public static bool IsEmail(string email)
        {
            return !string.IsNullOrWhiteSpace(email) && EmailRegex.IsMatch(email.Trim());
        }
    }
}
