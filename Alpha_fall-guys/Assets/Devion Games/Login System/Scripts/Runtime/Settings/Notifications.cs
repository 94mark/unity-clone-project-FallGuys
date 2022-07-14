using System.Collections;
using System.Collections.Generic;
using DevionGames.LoginSystem;
using DevionGames.UIWidgets;
using UnityEngine;
using UnityEngine.Events;

namespace DevionGames.LoginSystem
{
    public static class NotificationExtension
    {
        public static void Show(this NotificationOptions options, UnityAction<int> result, params string[] buttons)
        {
            if (LoginManager.UI.dialogBox != null)
            {
                LoginManager.UI.dialogBox.Show(options, result,buttons);
            }
        }
    }
}

namespace DevionGames.LoginSystem.Configuration
{
    [System.Serializable]
    public class Notifications : Settings
    {

        public override string Name
        {
            get
            {
                return "Notification";
            }
        }

        public override int Order { get => 2; }

        [Header("Notification Messages:")]
        public NotificationOptions loginFailed = new NotificationOptions()
        {
            title = "Login Failed",
            text = "Username or password wrong! You may need to activate your account!"
        };

        public NotificationOptions emptyField = new NotificationOptions()
        {
            title ="Error",
            text = "You need to complete all fields!"
        };

        public NotificationOptions passwordMatch = new NotificationOptions()
        {
            title="Error",
            text = "Password does not match confirm password!"
        };

        public NotificationOptions invalidEmail = new NotificationOptions()
        {
            title="Error",
            text = "Please enter a valid email!"
        };

        public NotificationOptions termsOfUse = new NotificationOptions()
        {
            title = "Error",
            text = "You need to accept the terms of use!"
        };

        public NotificationOptions userExists = new NotificationOptions()
        {
            title = "Error",
            text = "Username already exists!"
        };

        public NotificationOptions accountCreated = new NotificationOptions()
        {
            title = "Account Created",
            text = "Your account was created and an activation link was sent to your email!"
        };

        public NotificationOptions accountNotFound = new NotificationOptions()
        {
            title = "Error",
            text = "No corresponding account found!"
        };

        public NotificationOptions passwordRecovered = new NotificationOptions()
        {
            title = "Password Recovered",
            text = "Your password has beed send to your email address. You may login now."
        };
    }
}