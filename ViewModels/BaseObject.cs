using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Graphs2.ViewModels
{
    public class BaseObject : BaseViewModel
    {
        protected SolidColorBrush DefaultColor;
        protected SolidColorBrush WhenSelectedColor;

        public Action<BaseObject> OnSelection;

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

        public BaseObject() 
        {
            ObjectColor = new SolidColorBrush(Colors.Black);
            
            _isSelected = false;

            OnSelection = null;

            DefaultColor = new SolidColorBrush(Colors.Black);
            WhenSelectedColor = new SolidColorBrush(Colors.Red);
        }

        public void EnableSelection()
        {
            if (_isSelected == false)
                _isSelected = true;
            if (!(OnSelection is null))
                OnSelection.Invoke(this);
            ObjectColor = WhenSelectedColor;
        }

        public void DisableSelection()
        {
            if (_isSelected == true)
                _isSelected = false;
            ObjectColor = DefaultColor;
        }

        public void ChangeSelection()
        {
            _isSelected = !_isSelected;
            if (_isSelected)
                EnableSelection();
            else
                DisableSelection();
        }
    }
}
