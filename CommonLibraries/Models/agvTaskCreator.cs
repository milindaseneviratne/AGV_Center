using CommonLibraries.Extensions;
using CommonLibraries.Models.dbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibraries.Models
{
    public class agvTaskCreator
    {
        public agvTask Task { get; set; }

        SQLCommunicator sqlComm = new SQLCommunicator();
        public bool CreateTask(Barcode barcode)
        {
            if (barcode != null && barcode.HasData())
            {
                var model = sqlComm.GetModel();
                var currentStation = sqlComm.GetStation(barcode.Station);
                var destinationStation = sqlComm.GetStation(barcode.Destination);
                var result = GetResult(destinationStation);

                if (model != null && currentStation != null && destinationStation != null && !string.IsNullOrEmpty(result))
                {
                    return sqlComm.CreateTask(model, currentStation, destinationStation, result);
                }
            }
            return false;
        }

        private string GetResult(agvStation_Info destination)
        {
            var isLocked = destination.Status.Equals("Locked", StringComparison.OrdinalIgnoreCase);
            var isBusy = destination.Status.Equals("Busy", StringComparison.OrdinalIgnoreCase);

            if (!isLocked && !isBusy)
            {
                return "OK";
            }

            return "In Queue";
        }
    }
}
