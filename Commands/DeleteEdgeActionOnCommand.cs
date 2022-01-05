using Graphs2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphs2.Commands
{
    class DeleteEdgeActionOnCommand : CommandBase
    {
        private Action<EdgeViewModel> _onClickActrion;
        private EdgeViewModel _edge;
        public DeleteEdgeActionOnCommand(Action<EdgeViewModel> onClickActrion, EdgeViewModel edge)
        {
            _onClickActrion = onClickActrion;
            _edge = edge;
        }

        public override void Execute(object parameter)
        {
            _onClickActrion.Invoke(_edge);
        }
    }
}
