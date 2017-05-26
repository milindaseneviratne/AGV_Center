using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibraries.Database;
using CommonLibraries.Extensions;
using CommonLibraries.Models.dbModels;

namespace CommonLibraries.Models
{
    public class SQLCommunicator
    {
        private sqlDbContextEF dbContext = new sqlDbContextEF();
        private int SaveChanges()
        {
            try
            {
                return dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                ex.WriteLog().SaveToDataBase().Display();
            }

            return 0;
        }

        public User GetuserInfo(ApplicationUser user)
        {
            var passwordBytes = Encoding.UTF8.GetBytes(user.Password);
            var base64Encoded = Convert.ToBase64String(passwordBytes);

            var query = (from row in dbContext.User
                         where row.Name == user.Name && row.Password == base64Encoded
                         select row).FirstOrDefault();

            return query;
        }

        public agvModel_Info GetModel()
        {
            //In this query we search in agvModel_Info for a row where the value in the
            //Code Column is "3F1SW"
            //FirtstOrDefault just gets the first result if there are many rows that
            //match the condition.
            var query = dbContext.agvModel_Info
                .Where(x => x.Code.Equals("3F1SW", StringComparison.OrdinalIgnoreCase))
                .FirstOrDefault();

            if (query != null)
            {
                return query;
            }

            return null;
        }

        public agvStation_Info GetStation(string destination)
        {
            using (sqlDbContextEF privatedbContext = new sqlDbContextEF())
            {
                var query = privatedbContext.agvStation_Info
               .Where(x => x.Name.Equals(destination, StringComparison.OrdinalIgnoreCase))
               .FirstOrDefault();

                if (query != null)
                {
                    return query;
                }

                return null;
            }
           
        }

        public bool CreateTask(agvModel_Info model, agvStation_Info currentStation, agvStation_Info destinationStation, string result)
        {
            dbContext.agvTask.Add(new agvTask
            {
                ModelCode = model.Code,
                Current_Station = currentStation.Name,
                Dest_Station = destinationStation.Name,
                Result = result,
                StartTime = DateTime.Now
            });


            if (SaveChanges() == 1)
            {
                return true;
            }

            return false;
        }

        public List<agvTask> GetTasksInQueue()
        {
            var query = dbContext.agvTask
                                  .Where(t => t.Result.Equals("In Queue", StringComparison.OrdinalIgnoreCase))
                                  .ToList();

            if (query != null)
            {
                return query;
            }

            return null;
        }

        public bool InsertNewUser(ApplicationUser user)
        {
            //Here we can see a User object being created,
            //then the object properties are loaded.
            //then the object is added to the dbcontext.TableName.Add()
            //Finally changes are saved.
            bool successFlag = false;

            User dbUserObject = new User();

            dbUserObject.Name = user.Name;
            dbUserObject.Password = user.Password;
            dbUserObject.Group = user.Group.ToString();

            dbContext.User.Add(dbUserObject);

            SaveChanges();

            return successFlag;
        }

        public string GetDestination(Barcode barcode)
        {
            var result = dbContext.agvStationTestFlow
                                  .Where(row => row.Current_Station.Name == barcode.Station && row.Status.Status == barcode.Status)
                                  .FirstOrDefault();

            if(result!= null)
            {
                return result.Dest_Station.Name;
            }

            return string.Empty;
        }

        public string GetCommandType(Barcode barcode)
        {
            var result = dbContext.agvStationTestFlow
                                  .Where(row => row.Current_Station.Name == barcode.Station && row.Status.Status == barcode.Status)
                                  .FirstOrDefault();
            if (result != null)
            {
                return result.Command_Type.Name;
            }

            return null;
        }

        public bool InsertExceptionInformation(Exception ex)
        {
            bool successFlag = false;

            ApplicationErrorLog dbAppErrorLog = new ApplicationErrorLog();

            dbAppErrorLog.DumpString = ex.ToString();
            dbAppErrorLog.HashCode = ex.GetHashCode().ToString();
            dbAppErrorLog.HelpLink = ex.HelpLink;
            dbAppErrorLog.Message = ex.Message;
            dbAppErrorLog.Source = ex.Source;
            dbAppErrorLog.StackTrace = ex.StackTrace;

            dbContext.ApplicationErrorLog.Add(dbAppErrorLog);

            SaveChanges();

            return successFlag;
        }

        public void LogUserOUT(ApplicationUser userProperty)
        {
            //Here we fetch a row from the user activity log table
            //And then update the logout time property
            //finally save the changes.
            var query = (from row in dbContext.UserActivityLog
                         where row.UserId == userProperty.Id
                         orderby row.TimeStamp descending
                         select row).FirstOrDefault();

            query.LogoutTime = userProperty.LogOut;

            SaveChanges();
        }

        public void LogUserIN(ApplicationUser userProperty)
        {
            UserActivityLog userLog = new UserActivityLog();

            userLog.UserId = userProperty.Id;
            userLog.UserName = userProperty.Name;
            userLog.UserGroup = userProperty.Group.ToString();
            userLog.LoginTime = userProperty.LogIn;
            userLog.LogoutTime = DateTime.MaxValue;
            dbContext.UserActivityLog.Add(userLog);

            SaveChanges();
        }

        public void LogServerClientCommunications(ServerClientCommunicationLog log)
        {
            dbContext.CommunicationLog.Add(log);

            SaveChanges();
        }

        public List<BarcodeScannerConfig> GetBarcodeScannerConfigs()
        {
            List<BarcodeScannerConfig> barcodeScannerConfigs = new List<BarcodeScannerConfig>();

            BarcodeScannerConfig barcodeScannerConfig = new BarcodeScannerConfig();

            barcodeScannerConfig.QueryString = "SELECT * FROM Win32_PnPEntity";
            barcodeScannerConfig.Value1 = @"USBCDCACM\VID_0C2E&PID_092A\1&2B53A856&0&16364B062D_00";
            barcodeScannerConfig.Key1 = "PNPDeviceID";
            barcodeScannerConfig.Value2 = "Xenon 1902 Wireless Area-Imaging Scanner (COM34)";
            barcodeScannerConfig.Key2 = "Caption";

            List<string> propertyNames = new List<string>
                    {
                        "Caption",
                        "ClassGuid",
                        "CompatibleID",
                        "ConfigManagerErrorCode",
                        "ConfigManagerUserConfig",
                        "CreationClassName",
                        "Description",
                        "DeviceID",
                        "ErrorCleared",
                        "ErrorDescription",
                        "HardwareID",
                        "PNPDeviceID",
                        "InstallDate",
                        "LastErrorCode",
                        "Name",
                        "PowerManagementCapabilities",
                        "Service",
                        "Status",
                        "StatusInfo",
                        "SystemCreationClassName",
                        "SystemName",
                    };
            barcodeScannerConfig.PropertyNames = propertyNames;


            barcodeScannerConfigs.Add(barcodeScannerConfig);

            BarcodeScannerConfig barcodeScannerConfig2 = new BarcodeScannerConfig();

            barcodeScannerConfig2.QueryString = "SELECT * FROM Win32_SerialPort";
            barcodeScannerConfig2.Value1 = @"USB\VID_0483&PID_5740\DEMO_1.000";
            barcodeScannerConfig2.Key1 = "PNPDeviceID";
            barcodeScannerConfig2.Value2 = "MD USB Virtual COM Port (COM35)";
            barcodeScannerConfig2.Key2 = "Caption";

            propertyNames = new List<string>
                    {
                        "Availability",
                        "Binary",
                        "Capabilities",
                        "CapabilityDescriptions",
                        "Caption",
                        "ConfigManagerErrorCode",
                        "ConfigManagerUserConfig",
                        "CreationClassName",
                        "Description",
                        "DeviceID",
                        "ErrorCleared",
                        "ErrorDescription",
                        "InstallDate",
                        "LastErrorCode",
                        "MaxBaudRate",
                        "MaximumInputBufferSize",
                        "MaximumOutputBufferSize",
                        "MaxNumberControlled",
                        "Name",
                        "OSAutoDiscovered",
                        "PNPDeviceID",
                        "PowerManagementCapabilities",
                        "PowerManagementSupported",
                        "ProtocolSupported",
                        "ProviderType",
                        "SettableBaudRate",
                        "SettableDataBits",
                        "SettableFlowControl",
                        "SettableParity",
                        "SettableParityCheck",
                        "SettableRLSD",
                        "SettableStopBits",
                        "Status",
                        "StatusInfo",
                        "Supports16BitMode",
                        "SupportsDTRDSR",
                        "SupportsElapsedTimeouts",
                        "SupportsIntTimeouts",
                        "SupportsParityCheck",
                        "SupportsRLSD",
                        "SupportsRTSCTS",
                        "SupportsSpecialCharacters",
                        "SupportsXOnXOff",
                        "SupportsXOnXOffSet",
                        "SystemCreationClassName",
                        "SystemName",
                        "TimeOfLastReset"
                    };

            barcodeScannerConfig2.PropertyNames = propertyNames;


            barcodeScannerConfigs.Add(barcodeScannerConfig2);

            return barcodeScannerConfigs;
        }
    }
}
