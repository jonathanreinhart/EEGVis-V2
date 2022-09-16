using EEGVis_V2.Viewmodels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EEGVis_V2.Commands
{
    internal class NextPageCommand : CommandBase
    {
        private readonly SerialGraphViewModel serialGraphViewModel;

        public NextPageCommand(SerialGraphViewModel _serialGraphViewModel)
        {
            serialGraphViewModel = _serialGraphViewModel;
        }

        public override void Execute(object? parameter)
        {
            
        }
    }
}
