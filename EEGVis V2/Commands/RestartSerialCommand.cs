using EEGVis_V2.models;
using EEGVis_V2.Viewmodels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EEGVis_V2.Commands
{
    internal class RestartSerialCommand : CommandBase
    {
        private readonly SerialGraphViewModel serialGrpahViewModel;

        public RestartSerialCommand(SerialGraphViewModel _serialGrpahViewModel)
        {
            serialGrpahViewModel = _serialGrpahViewModel;
        }

        public override void Execute(object? parameter)
        {
            serialGrpahViewModel.SerialData_.closing = true;
            serialGrpahViewModel.SerialData_.reconnecting = true;
        }
    }
}
