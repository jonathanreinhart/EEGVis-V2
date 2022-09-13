using EEGVis_V2.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EEGVis_V2.Viewmodels
{
    public class NavigationViewModel : ViewModelBase
    {
        public ICommand NextPage { get; set; }

        private object currentPage;
		public object CurrentPage
		{
			get
			{
				return currentPage;
			}
			set
			{
				currentPage = value;
				OnPropertyChanged(nameof(CurrentPage));
			}
		}

        public NavigationViewModel()
		{
			NextPage = new NextPageCommand(this);
		}
	}
}
