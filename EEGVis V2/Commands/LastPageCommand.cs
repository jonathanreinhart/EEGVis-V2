using EEGVis_V2.Viewmodels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEGVis_V2.Commands
{
    internal class LastPageCommand : CommandBase
    {
        private readonly SerialGraphViewModel serialGraphViewModel;

        public LastPageCommand(SerialGraphViewModel _serialGraphViewModel)
        {
            serialGraphViewModel = _serialGraphViewModel;
        }

        public override void Execute(object? parameter)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                int newPage = (serialGraphViewModel.SelectedPage - 1) % serialGraphViewModel.NumPages;
                // go to the last page if we're on the first page
                if (newPage < 0)
                    newPage = serialGraphViewModel.NumPages - 1;
                
                serialGraphViewModel.getPageNumChannels(newPage);
                serialGraphViewModel.SelectedPage = newPage;
            });
        }
    }
}
