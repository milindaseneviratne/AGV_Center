using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CommonLibraries.Models
{
    public class agvTaskDequer
    {
        private SQLCommunicator sqlComm = new SQLCommunicator();
        private VCSCommunicator _vcsComm;
        public agvTaskDequer(VCSCommunicator vcsComm)
        {
            _vcsComm = vcsComm;
        }
        public void DequeueTasks()
        {
            while (true)
            {
                var enqueuedTasks = sqlComm.GetTasksInQueue();

                foreach (var taskItem in enqueuedTasks)
                {
                    //check if destination is idle.
                    var destination = sqlComm.GetStation(taskItem.Dest_Station);

                    if (destination.Status.Equals("Idle", StringComparison.OrdinalIgnoreCase))
                    {
                        //send VCS command "TableFromIdle" to take the table to the destination.
                        _vcsComm.SendDequeueTaskCommand(taskItem, destination);
                        Thread.Sleep(500);
                    }
                }
                Thread.Sleep(5000);
            }
        }
    }
}
