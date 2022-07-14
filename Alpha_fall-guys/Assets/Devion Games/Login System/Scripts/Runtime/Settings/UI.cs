using System.Collections;
using System.Collections.Generic;
using DevionGames.UIWidgets;
using UnityEngine;
using UnityEngine.Assertions;

namespace DevionGames.LoginSystem.Configuration
{
    public class UI : Settings
    {
        public override string Name
        {
            get
            {
                return "UI";
            }
        }

        public override int Order { get => 1; }

        [Header("UI References:")]
        [InspectorLabel("Login Window", "Name of the login window.")]
        public string loginWindowName = "Login";
        [InspectorLabel("Registration Window", "Name of the registration window.")]
        public string registrationWindowName = "Registration";
        [InspectorLabel("Recover Password Window", "Name of the recover password window.")]
        public string recoverPasswordWindowName = "Recover Password";
        [InspectorLabel("Dialog Box", "Name of the dialog box widget.")]
        public string dialogBoxName = "Dialog Box";
        [InspectorLabel("Tooltip", "Name of the tooltip widget.")]
        public string tooltipName = "Tooltip";


        private LoginWindow m_LoginWindow;
        public LoginWindow loginWindow
        {
            get
            {
                if (this.m_LoginWindow == null)
                {
                    this.m_LoginWindow = WidgetUtility.Find<LoginWindow>(this.loginWindowName);
                }
                Assert.IsNotNull(this.m_LoginWindow, "Login Window with name " + this.loginWindowName + " is not present in scene.");
                return this.m_LoginWindow;
            }
        }

        private RegistrationWindow m_RegistrationWindow;
        public RegistrationWindow registrationWindow
        {
            get
            {
                if (this.m_RegistrationWindow == null)
                {
                    this.m_RegistrationWindow = WidgetUtility.Find<RegistrationWindow>(this.registrationWindowName);
                }
                Assert.IsNotNull(this.m_RegistrationWindow, "Registration Window with name " + this.registrationWindowName + " is not present in scene.");
                return this.m_RegistrationWindow;
            }
        }

        private RecoverPasswordWindow m_RecoverPasswordWindow;
        public RecoverPasswordWindow recoverPasswordWindow
        {
            get
            {
                if (this.m_RecoverPasswordWindow == null)
                {
                    this.m_RecoverPasswordWindow = WidgetUtility.Find<RecoverPasswordWindow>(this.recoverPasswordWindowName);
                }
                Assert.IsNotNull(this.m_RecoverPasswordWindow, "Recover Password Window with name " + this.recoverPasswordWindowName + " is not present in scene.");
                return this.m_RecoverPasswordWindow;
            }
        }

        private DialogBox m_DialogBox;
        public DialogBox dialogBox
        {
            get
            {
                if (this.m_DialogBox == null)
                {
                    this.m_DialogBox = WidgetUtility.Find<DialogBox>(this.dialogBoxName);
                }
                Assert.IsNotNull(this.m_DialogBox, "DialogBox widget with name " + this.dialogBoxName + " is not present in scene.");
                return this.m_DialogBox;
            }
        }

        private Tooltip m_Tooltip;
        public Tooltip tooltip
        {
            get
            {
                if (this.m_Tooltip == null)
                {
                    this.m_Tooltip = WidgetUtility.Find<Tooltip>(this.tooltipName);
                }
                Assert.IsNotNull(this.m_Tooltip, "Tooltip widget with name " + this.tooltipName + " is not present in scene.");
                return this.m_Tooltip;
            }
        }

       
    }
}