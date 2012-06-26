using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interfaces;
using CCStepsCoords;
using System.Reflection;
using Logic;
using System.Windows;
using System.Windows.Forms;

namespace MasterElement
{
    class Hashsim
    {
        public static int Main()
        {
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Wind());



            return 0;
        }
    }

}
