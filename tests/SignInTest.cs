﻿using AlarmClock.Properties;

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
            SignInBtn.Enabled.Should().BeFalse();
            ToSignUpBtn.Enabled.Should().BeTrue();
        }

        [Fact]
        public void SignInEnableAfterInput()
        {
            EnterCredentials();

            SignInBtn.Enabled.Should().BeTrue();
        }

        [Fact]
        public void ToSignUp()
        {
            ToSignUpBtn.Click();

            // At the SignUp window
            Window.Get<Button>("SignUp").Should().NotBe(null);
        }

        [Fact]
        public void UserDoesntExist()
        {
            EnterCredentials();

            SignInBtn.Click();

            var errorBox = Window.MessageBox("");

            errorBox.Should().NotBe(null);

            errorBox.Get<Label>().Text
                    .Should().Be(string.Format(Resources.UserDoesntExistError, Login));

            CloseMessageBox();
        }

        [Fact]
        public void SuccessSignIn()
        {
            ToSignUpBtn.Click();

            Window.Get<TextBox>("Name").Enter("Name");
            Window.Get<TextBox>("Surname").Enter("Surname");
            Window.Get<TextBox>("Email").Enter("E@mai.l");
            Window.Get<TextBox>("Login").Enter(Login);
            Window.Get<TextBox>("Password").Enter("Password");

            Window.Get<Button>("SignUp").Click();
            Window.Get<Button>("SignOut").Click();

            EnterCredentials();

            SignInBtn.Click();

            // At the Main window
            Window.Get<Button>("SignOut").Should().NotBe(null);
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

        private static Button SignInBtn => Window.Get<Button>("SignIn");
        private static Button ToSignUpBtn => Window.Get<Button>("ToSignUp");
        #endregion
    }
}
