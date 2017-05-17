using CommonLibraries.Extensions;
using CommonLibraries.Models.dbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CommonLibraries.Models
{
    public class agvTaskCreator
    {
        private VCSCommunicator vcsComm = new VCSCommunicator();
        private SQLCommunicator sqlComm = new SQLCommunicator();
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
                return "Ready";
            }

            return "In Queue";
        }

        public void DequeueTasks()
        {
            while(true)
            {
                var enqueuedTasks = sqlComm.GetTasksInQueue();

                foreach (var taskItem in enqueuedTasks)
                {
                    //check if destination is idle.
                    var destination = sqlComm.GetStation(taskItem.Dest_Station);

                    if (destination.Status.Equals("Idle", StringComparison.OrdinalIgnoreCase))
                    {
                        //send VCS command "TableFromIdle" to take the table to the destination.
                        vcsComm.SendDequeueTaskCommand(taskItem, destination);
                        Thread.Sleep(500);
                    }
                }
                Thread.Sleep(5000);
            }
        }
    }
}
