using EEGVis_V2.Commands;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EEGVis_V2.Viewmodels
{
    public class NavigationViewModel : ViewModelBase
    {
		private string _currentView;
		public string CurrentView
		{
			get
			{
				return _currentView;
			}
			set
			{
				_currentView = value;
				OnPropertyChanged(nameof(CurrentView));
			}
		}

		private object _currentViewObject;
		public object CurrentViewObject
		{
			get
			{
				return _currentViewObject;
			}
			set
			{
				_currentViewObject = value;
				OnPropertyChanged(nameof(CurrentViewObject));
			}
		}

		public NavigationViewModel()
		{
			PropertyChanged += NavigationViewModel_PropertyChanged;
		}

		private void NavigationViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == nameof(CurrentView))
			{
				if (CurrentView == "Graph")
				{
					
				}
			}
		}
	}
}
