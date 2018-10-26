﻿using System;
using System.Reflection;

using AlarmClock.Properties;

using FluentAssertions;

using TestStack.White;
using TestStack.White.UIItems.WindowItems;

using Xunit;

using Button = TestStack.White.UIItems.Button;
using Label = TestStack.White.UIItems.Label;
using TextBox = TestStack.White.UIItems.TextBox;

namespace AlarmClock.tests
{
    public class SignUpTest : IDisposable
    {
        private static Window Window { get; set; }

        public SignUpTest()
        {
            var path = Assembly.GetExecutingAssembly().CodeBase.Replace("#", "%23");

            Window = Application.Launch(new Uri(path).LocalPath).GetWindow("Alarm clock");
            ToSignUp();
        }

        public void Dispose() => Window.Close();

        #region Test cases
        [Fact]
        public void ButtonsStartState()
        {
            SignUp.Enabled.Should().BeFalse();
            Window.Get<Button>("ToSignIn").Enabled.Should().BeTrue();
        }

        [Fact]
        public void SignInEnableAfterInput()
        {
            EnterCredentials();

            SignUp.Enabled.Should().BeTrue();
        }

        [Fact]
        public void InvalidEmail()
        {
            EnterCredentials();

            const string invalidEmail = "Invalid";
            Window.Get<TextBox>("Email").Enter(invalidEmail);

            SignUp.Click();

            var errorBox = Window.MessageBox("");

            errorBox.Should().NotBe(null);

            errorBox.Get<Label>().Text
                    .Should().Be(string.Format(Resources.InvalidEmailError, invalidEmail));

            CloseMessageBox();
        }

        [Fact]
        public void UserAlreadyExists()
        {
            EnterCredentials();
            SignUp.Click();

            SignOut.Should().NotBe(null);
            SignOut.Click();

            ToSignUp();

            EnterCredentials();
            SignUp.Click();

            var errorBox = Window.MessageBox("");

            errorBox.Should().NotBe(null);

            errorBox.Get<Label>().Text
                    .Should().Be(string.Format(Resources.UserAlreadyExistsError, Email, Login));

            CloseMessageBox();
        }

        [Fact]
        public void SuccessSignUp()
        {
            EnterCredentials();

            SignUp.Click();

            SignOut.Should().NotBe(null);
        }
        #endregion

        #region Helpers
        private static void EnterCredentials()
        {
            Window.Get<TextBox>("Name").Enter("Name");
            Window.Get<TextBox>("Surname").Enter("Surname");
            Window.Get<TextBox>("Email").Enter(Email);
            Window.Get<TextBox>("Login").Enter(Login);
            Window.Get<TextBox>("Password").Enter("Password");
        }

        private static void CloseMessageBox() => Window.MessageBox("")?.Close();

        private const string Login = "Login";
        private const string Email = "E@mai.l";

        private static Button SignUp => Window.Get<Button>("SignUp");
        private static Button SignOut => Window.Get<Button>("SignOut");

        private static void ToSignUp() => Window.Get<Button>("ToSignUp").Click();
        #endregion
    }
}
