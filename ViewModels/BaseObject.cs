using Graphs2.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace Graphs2.ViewModels
{
    public abstract class BaseObject : BaseViewModel
    {
        protected SolidColorBrush defaultColor;
        protected SolidColorBrush whenSelectedColor;

        protected string whenSelectedText;
        protected string whenNotSelectedText;


        public Action OnUnselection;

        protected bool _isSelected;

        private SolidColorBrush _objectColor;

        public SolidColorBrush ObjectColor
        {
            get => _objectColor;
            set
            {
                _objectColor = value;
                OnPropertyChanged(nameof(ObjectColor));
            }
        }

        private string _contextMenuText;
        public string ContextMenuText
        {
            get => _contextMenuText;
            set
            {
                _contextMenuText = value;
                OnPropertyChanged(nameof(ContextMenuText));
            }
        }

        public ICommand SelectionChange { get; set; }
        public ICommand DeleteFromGraph { get; set; }

        public BaseObject() 
        {
            ObjectColor = new SolidColorBrush(Colors.Black);
            
            _isSelected = false;

            OnUnselection = null;

            defaultColor = new SolidColorBrush(Colors.Black);
            whenSelectedColor = new SolidColorBrush(Colors.Red);

            SelectionChange = new ActionOnCommand(ChangeSelection);

            whenSelectedText = "Снять выделение";
            whenNotSelectedText = "Выделить";

            ContextMenuText = whenNotSelectedText;
            
        }

        public abstract void EnableSelection();

        public void DisableSelection()
        {
            if (_isSelected == true)
                _isSelected = false;
            if (!(OnUnselection is null))
                OnUnselection.Invoke();

            setDefaultState();
        }

        public void setDefaultState()
        {
            ContextMenuText = whenNotSelectedText;
            ObjectColor = defaultColor;
        }

        public void ChangeSelection()
        {
            _isSelected = !_isSelected;
            if (_isSelected)
                EnableSelection();
            else
                DisableSelection();
        }

        public abstract void ChangePosition(double x, double y);
    }
}
