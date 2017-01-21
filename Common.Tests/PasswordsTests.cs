using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Common.Tests
{
    [TestClass]
    public class PasswordsTests
    {
        [TestMethod]
        public void HashPassword()
        {
            const string password = "hello1234";
            var hash = Passwords.HashPassword(password);

            Assert.IsTrue(hash.Length > 0);
        }

        [TestMethod]
        public void VerifyPassword_PasswordsMatch_Success()
        {
            const string password = "hello1234";
            var hash = Passwords.HashPassword(password);

            Assert.IsTrue(Passwords.VerifyPassword(password, hash));
        }

        [TestMethod]
        public void VerifyPassword_PasswordsUnmatched_Fail()
        {
            const string password = "hello1234";
            var hash = Passwords.HashPassword(password);

            Assert.IsFalse(Passwords.VerifyPassword(password + " ", hash));
        }
    }
}