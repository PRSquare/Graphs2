using Graphs2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphs2.Commands
{
    class DeleteVertexActionOnCommand : CommandBase
    {
        private Action<VertexViewModel> _onClickActrion;
        private VertexViewModel _vertex;
        public DeleteVertexActionOnCommand(Action<VertexViewModel> onClickActrion, VertexViewModel vertex)
        {
            _onClickActrion = onClickActrion;
            _vertex = vertex;
        }

        public override void Execute(object parameter)
        {
            _onClickActrion.Invoke(_vertex);
        }
    }
}
