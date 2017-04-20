using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibraries.Models.dbModels
{
    public class CimVcsCommunicationLog
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public string TxCommand { get; set; }
        public string RxCommand { get; set; }
        public string Result { get; set; }

    }
}
