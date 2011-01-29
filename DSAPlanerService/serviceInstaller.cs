using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;

namespace DSAPlanerService
{
    [RunInstaller(false)]
    public partial class serviceInstaller : System.ServiceProcess.ServiceInstaller
    {
        public serviceInstaller()
        {
            InitializeComponent();
        }
    }
}