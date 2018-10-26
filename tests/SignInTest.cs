using AlarmClock.Properties;

using System;
using System.Reflection;

using FluentAssertions;

using TestStack.White;
using TestStack.White.UIItems.WindowItems;

using Xunit;

using Button  = TestStack.White.UIItems.Button;
using Label   = TestStack.White.UIItems.Label;
using TextBox = TestStack.White.UIItems.TextBox;

namespace AlarmClock.tests
{
    public class SignInTest : IDisposable
    {
        private static Window Window { get; set; }

        public SignInTest()
        {
            var path = Assembly.GetExecutingAssembly().CodeBase.Replace("#", "%23");

            Window = Application.Launch(new Uri(path).LocalPath).GetWindow("Alarm clock");
        }

        public void Dispose() => Window.Close();

        #region Test cases
        [Fact]
        public void ButtonsStartState()
        {
            SignIn.Enabled.Should().BeFalse();
            Window.Get<Button>("ToSignUp").Enabled.Should().BeTrue();
        }

        [Fact]
        public void SignInEnableAfterInput()
        {
            EnterCredentials();

            SignIn.Enabled.Should().BeTrue();
        }

        [Fact]
        public void UserDoesntExist()
        {
            EnterCredentials();

            SignIn.Click();

            var errorBox = Window.MessageBox("");

            errorBox.Should().NotBe(null);

            errorBox.Get<Label>().Text
                    .Should().Be(string.Format(Resources.UserDoesntExistError, Login));

            CloseMessageBox();
        }

        //[Fact]
        public void SuccessSignIn()
        {
            // TODO
        }
        #endregion

        #region Helpers
        private static void EnterCredentials()
        {
            Window.Get<TextBox>("EmailOrLogin").Enter(Login);
            Window.Get<TextBox>("Password").Enter("Password");
        }

        private static void CloseMessageBox() => Window.MessageBox("")?.Close();

        private const string Login = "Login";

        private static Button SignIn => Window.Get<Button>("SignIn");
        #endregion
    }
}
