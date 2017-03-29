using System.Linq;

namespace Registrar.Models
{
    public static class EmailContentWriter
    {
        public static readonly string ConfirmEmailSubject = "Confirm your account";
        public static readonly string ResetPasswordSubject = "Reset your password";

        public static string ConfirmEmail(string email, string code)
        {
            var uri = GetUri("confirmEmail", email, code);
            return $"Email confirmation code: {code}<br /><br />Alternatively, click <a href=\"{uri}\">here</a>";
        }

        public static string ResetPassword(string email, string code)
        {
            var uri = GetUri("resetpassword", email, code);
            return
                $"Password reset authorisation code: {code}<br /><br />Alternatively, click <a href=\"{uri}\">here</a>";
        }

        private static string GetUri(string action, params string[] parameters)
        {
            var uri = $"evoto://{action}";
            if (parameters.Any())
                uri += "/" + string.Join("/", parameters);
            return uri;
        }
    }
}