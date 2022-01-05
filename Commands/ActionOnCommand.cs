using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Graphs2.ViewModels;

namespace Graphs2.Commands
{
    public class ActionOnCommand : CommandBase
    {
        private Action _onClickActrion;
        public ActionOnCommand( Action onClickActrion )
        {
            _onClickActrion = onClickActrion;
        }

        public override void Execute(object parameter)
        {
            _onClickActrion.Invoke();
        }
    }
}
