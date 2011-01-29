using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;

namespace DSAPlanerService
{
    [RunInstaller(true)]
    public partial class globalInstaller : Installer
    {
        public globalInstaller()
        {
            InitializeComponent();

            serviceInstaller siInstance = new serviceInstaller();
            serviceProcessInstaller sipInstance = new serviceProcessInstaller();

            Installers.Add(siInstance);
            Installers.Add(sipInstance);
        }
    }
}