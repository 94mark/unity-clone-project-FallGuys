using DevionGames.UIWidgets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DevionGames.LoginSystem
{
    public class RecoverPasswordWindow : UIWidget
    {

        public override string[] Callbacks
        {
            get
            {
                List<string> callbacks = new List<string>(base.Callbacks);
                callbacks.Add("OnPasswordRecovered");
                callbacks.Add("OnFailedToRecoverPassword");
                return callbacks.ToArray();
            }
        }

        [Header("Reference")]
        [SerializeField]
        protected InputField email;
        [SerializeField]
        protected Button recoverButton;
        [SerializeField]
        protected GameObject loadingIndicator;

        protected override void OnStart()
        {
            base.OnStart();
            if (loadingIndicator != null)
            {
                loadingIndicator.SetActive(false);
            }
            EventHandler.Register("OnPasswordRecovered", OnPasswordRecovered);
            EventHandler.Register("OnFailedToRecoverPassword", OnFailedToRecoverPassword);
            recoverButton.onClick.AddListener(RecoverPasswordUsingFields);
        }

        private void RecoverPasswordUsingFields() {
            if (!LoginManager.ValidateEmail(email.text))
            {
               LoginManager.Notifications.invalidEmail.Show( delegate (int result) { Show(); }, "OK");
                Close();
                return;
            }
            recoverButton.interactable = false;
            if (loadingIndicator != null)
            {
                loadingIndicator.SetActive(true);
            }
            LoginManager.RecoverPassword(email.text);
        }

        private void OnPasswordRecovered() {
            Execute("OnPasswordRecovered",new CallbackEventData());
            LoginManager.Notifications.passwordRecovered.Show( delegate (int result) { LoginManager.UI.loginWindow.Show(); }, "OK");
            recoverButton.interactable = true;
            if (loadingIndicator != null)
            {
                loadingIndicator.SetActive(false);
            }
            Close();
        }

        private void OnFailedToRecoverPassword() {
            Execute("OnFailedToRecoverPassword",new CallbackEventData());
            LoginManager.Notifications.accountNotFound.Show( delegate (int result) { Show(); }, "OK");
            recoverButton.interactable = true;
            if (loadingIndicator != null)
            {
                loadingIndicator.SetActive(false);
            }
            Close();
        }
    }
}