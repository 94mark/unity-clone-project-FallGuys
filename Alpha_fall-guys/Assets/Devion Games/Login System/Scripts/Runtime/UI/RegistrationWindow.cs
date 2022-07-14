using DevionGames.UIWidgets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DevionGames.LoginSystem
{
    public class RegistrationWindow : UIWidget
    {
        public override string[] Callbacks
        {
            get
            {
                List<string> callbacks = new List<string>(base.Callbacks);
                callbacks.Add("OnAccountCreated");
                callbacks.Add("OnFailedToCreateAccount");
                return callbacks.ToArray();
            }
        }

        [Header("Reference")]
        /// <summary>
		/// Referenced UI field
		/// </summary>
		[SerializeField]
        protected InputField username;
        /// <summary>
        /// Referenced UI field
        /// </summary>
        [SerializeField]
        protected InputField password;
        /// <summary>
        /// Referenced UI field
        /// </summary>
        [SerializeField]
        protected InputField confirmPassword;
        /// <summary>
        /// Referenced UI field
        /// </summary>
        [SerializeField]
        protected InputField email;
        /// <summary>
        /// Referenced UI field
        /// </summary>
        [SerializeField]
        protected Toggle termsOfUse;
        /// <summary>
        /// Referenced UI field
        /// </summary>
        [SerializeField]
        protected Button registerButton;

        [SerializeField]
        protected GameObject loadingIndicator;

        protected override void OnStart()
        {
            base.OnStart();
            if (loadingIndicator != null)
            {
                loadingIndicator.SetActive(false);
            }

            EventHandler.Register("OnAccountCreated", OnAccountCreated);
            EventHandler.Register("OnFailedToCreateAccount", OnFailedToCreateAccount);

            registerButton.onClick.AddListener(CreateAccountUsingFields);
        }

        /// <summary>
		/// Creates the account using data from referenced fields.
		/// </summary>
		private void CreateAccountUsingFields()
        {
            if (string.IsNullOrEmpty(email.text) ||
                string.IsNullOrEmpty(password.text) ||
                string.IsNullOrEmpty(confirmPassword.text) ||
                string.IsNullOrEmpty(email.text))
            {
                LoginManager.Notifications.emptyField.Show(delegate (int result) { Show(); }, "OK");
                Close();
                return;
            }

            if (password.text != confirmPassword.text)
            {
                password.text = "";
                confirmPassword.text = "";
                LoginManager.Notifications.passwordMatch.Show( delegate (int result) { Show(); }, "OK");
                Close();
                return;
            }

            if (!LoginManager.ValidateEmail(email.text))
            {
                email.text = "";
                LoginManager.Notifications.invalidEmail.Show( delegate (int result) { Show(); }, "OK");
                Close();
                return;
            }

            if (!termsOfUse.isOn)
            {
                LoginManager.Notifications.termsOfUse.Show( delegate (int result) { Show(); }, "OK");
                Close();
                return;
            }
            registerButton.interactable = false;
            if (loadingIndicator != null)
            {
                loadingIndicator.SetActive(true);
            }
            LoginManager.CreateAccount(username.text, password.text, email.text);
        }


        private void OnAccountCreated() {
            Execute("OnAccountCreated", new CallbackEventData());
            LoginManager.Notifications.accountCreated.Show( delegate (int result) { LoginManager.UI.loginWindow.Show(); }, "OK");
            registerButton.interactable = true;
            if (loadingIndicator != null)
            {
                loadingIndicator.SetActive(false);
            }
            Close();
        }

        private void OnFailedToCreateAccount() {
            Execute("OnFailedToCreateAccount", new CallbackEventData());
            username.text = "";
            LoginManager.Notifications.userExists.Show( delegate (int result) { Show(); }, "OK");
            registerButton.interactable = true;
            if (loadingIndicator != null)
            {
                loadingIndicator.SetActive(false);
            }
            Close();
        }
    }
}