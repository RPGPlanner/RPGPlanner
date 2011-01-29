using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;

namespace DSAPlanerService
{
    [RunInstaller(false)]
    public partial class serviceProcessInstaller : System.ServiceProcess.ServiceProcessInstaller
    {
        public serviceProcessInstaller()
        {
            InitializeComponent();
        }
    }
}