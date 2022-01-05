using Graphs2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Graphs2.Commands
{
    public class TestCommand : CommandBase
    {
        public TestCommand ()
        { }
        public override void Execute(object parameter)
        {
            MessageBox.Show("Test");
        }
    }
}
