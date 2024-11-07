﻿using System.Windows;

namespace NASApp
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var loginWindow = new LoginWindow();
            loginWindow.Show();
        }
    }
}
