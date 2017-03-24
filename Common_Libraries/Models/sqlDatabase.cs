using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualBasic;    //For VisualBasic
using System.Data;              //For DatabaseSet
using System.Data.SqlClient;    //For SqlClient
using Common_Lib.Config;
using System.IO;
using System.Data.Odbc;         //For ODBC

namespace Common_Lib.Lib
{
    public class sqlDatabase
    {
        #region "Static Arguments"
        public static string strConnection = string.Empty;
        public static string strCommand = string.Empty;

#if (DEBUG)
        public static string ServerName = ODBC.ODBC_ServerName;
#else
        public static string ServerName = ODBC.ODBC_ServerName;
#endif
        public static string DatabaseName = string.Empty;
        public static string UserID = string.Empty;
        public static string Password = string.Empty;

        //Select Command Arguments
        //string FieldName, string TableName, string WhereCondition, string OrderBy
        public static string FieldName = string.Empty;
        public static string TableName = string.Empty;
        public static string WhereCondition = string.Empty;
        public static string OrderBy = string.Empty;

        //Insert Command Arguments
        //string TableName, string InsertField, string InsertValue
        public static string InsertField = string.Empty;
        public static string InsertValue = string.Empty;

        //Update Command Arguments
        //string TableName, string SetValue, string WhereCondition
        public static string SetValue = string.Empty;

        //Delete Command Arguments
        //string TableName, string WhereCondition

        public static string ConditionValue = string.Empty;

        public static bool flgInsert = true;    //For Database Insert / Update check

        public static int sqlNonQueryReturn = 0;
        public static int FieldLength = 0;
        //public static SqlTransaction _SqlTransaction;
        
        #endregion

        #region "Const Arguments"

        public const string caIntegrated_Security_SSPI = "SSPI";      //SSPI = true: Disable USerID Check
        public const string caIntegrated_Security_false = "false";    //false: Enable USERID Check       
                
        /// <summary>
        /// For Windows Permission Check, 
        /// Value: SSPI(=true): Disable UserID Check, false: Enable UserID Check
        /// </summary>
        public static string caIntegrated_Security = "SSPI";
        /// <summary>
        /// Database == Data Source
        /// </summary>
        public static string caData_Source = @".\SQLEXPRESS";        

        #endregion

        #region "DataTable For Config

        public static DataTable myDataTable;    //For Temp DataTable
        
        #endregion

        #region "sql Test Sample Code"
        //'************** SQL Syntax **********************************************
        //'Select
        //'SELECT "欄位名" FROM "表格名"
        //'
        //'SELECT "欄位名"
        //'From "表格名"
        //'WHERE "欄位名" LIKE {模式}
        //'
        //'SELECT "欄位名"
        //'From "表格名"
        //'[WHERE "條件"]
        //'ORDER BY "欄位名" [ASC, DESC]

        //'SQL ORDER BY: ASC(小到大),DESC(大到小)

        //'Insertion
        //'INSERT INTO "表格名" ("欄位1", "欄位2", ...)
        //'VALUES ("值1", "值2", ...)

        //'Update
        //'Update "表格名"
        //'SET "欄位1" = [新值]
        //'WHERE {條件}

        //'Delete
        //'DELETE FROM "表格名"
        //'WHERE {條件}

        //'String = 'val'
        //'Integer = Val

        //'Get SQL DateTime
        //'SELECT CONVERT(VARCHAR, GETDATE(), 120 )
        //'=> 2007-12-31 10:30:20

        ///************************************************************************

        /// <summary>
        /// sqlSetect_Test
        /// </summary>
        /// <returns></returns>
        public static int sqlSetect_Test()
        {
            //            strConnection = @"data source=(local);
            //                              Database=BrainsCitrine;
            //                              user id=bruser;
            //                              password=BR%ccsz";
#if (DEBUG)
            strConnection = "server=(local);database =BrainsCitrine;integrated security=true";
            //strCommand = "Select * From 機番進捗DB Where 本体号機='A0VD002000629'";

            //'SELECT "欄位名"
            //'From "表格名"
            //'[WHERE "條件"]
            //'ORDER BY "欄位名" [ASC, DESC]

            //'SQL ORDER BY: ASC(小到大),DESC(大到小)

            strCommand = "Select * From UserManagement Where UserID='Jeffery'";
#else

#endif

            return sqlExecuteNonQuery(strConnection, strCommand);
        }

        ///************************************************************************
        /// <summary>
        /// sqlInsert_Test
        /// </summary>
        /// <returns></returns>
        public static int sqlInsert_Test()
        {
            //Delete 'Jeffery'
            sqlDelete_Test();

            //            strConnection = @"data source=(local);
            //                              Database=BrainsCitrine;
            //                              user id=bruser;
            //                              password=BR%ccsz";
#if (DEBUG)
            strConnection = "server=(local);database =BrainsCitrine;integrated security=true";
            //strCommand = "Select * From 機番進捗DB Where 本体号機='A0VD002000629'";

            //'Insertion
            //'INSERT INTO "表格名" ("欄位1", "欄位2", ...)
            //'VALUES ("值1", "值2", ...)
            strCommand = "INSERT INTO UserManagement (UserID, Password, MACAddress) Values('Jeffery','Jeffery','11:22:33:44:55:66')";
#else

#endif

            return sqlExecuteNonQuery(strConnection, strCommand);
        }

        ///************************************************************************
        /// <summary>
        /// sqlDelete_Test
        /// </summary>
        /// <returns></returns>
        public static int sqlDelete_Test()
        {
            //            strConnection = @"data source=(local);
            //                              Database=BrainsCitrine;
            //                              user id=bruser;
            //                              password=BR%ccsz";
#if (DEBUG)
            strConnection = "server=(local);database =BrainsCitrine;integrated security=true";
            //strCommand = "Select * From 機番進捗DB Where 本体号機='A0VD002000629'";

            //'Delete
            //'DELETE FROM "表格名"
            //'WHERE {條件}
            strCommand = "DELETE FROM UserManagement WHERE (UserID='Aby')";
#else

#endif

            return sqlExecuteNonQuery(strConnection, strCommand);
        }

        ///************************************************************************
        /// <summary>
        /// sqlUpdate_Test
        /// </summary>
        /// <returns></returns>
        public static int sqlUpdate_Test()
        {
            int sqlResult = 0;

            if (sqlSetect_Test() == -1)
            {
                //            strConnection = @"data source=(local);
                //                              Database=BrainsCitrine;
                //                              user id=bruser;
                //                              password=BR%ccsz";
#if (DEBUG)
                strConnection = "server=(local);database =BrainsCitrine;integrated security=true";
                //strCommand = "Select * From 機番進捗DB Where 本体号機='A0VD002000629'";

                //'Update
                //'Update "表格名"
                //'SET "欄位1" = [新值]
                //'WHERE {條件}
                strCommand = "UPDATE UserManagement SET UserID='Aby' WHERE (UserID='Jeffery')";
#else

#endif

                sqlResult = sqlExecuteNonQuery(strConnection, strCommand);
            }
            return sqlResult;
        }

        ///************************************************************************
        /// <summary>
        /// sqlDataSet_Test
        /// </summary>
        /// <returns></returns>
        public static DataSet sqlDataSet_Test()
        {
            //            strConnection = @"data source=(local);
            //                              Database=BrainsCitrine;
            //                              user id=bruser;
            //                              password=BR%ccsz";
#if (DEBUG)
            strConnection = "server=(local);database =BrainsCitrine;integrated security=true";
            //strCommand = "Select * From 機番進捗DB Where 本体号機='A0VD002000629'";
            //strCommand = "Select * From 共通製造履歴DB Where 本体号機='A0VD002000629'";

            //'SELECT "欄位名"
            //'From "表格名"
            //'[WHERE "條件"]
            //'ORDER BY "欄位名" [ASC, DESC]
            strCommand = "Select * From UserManagement"; // Where UserID='Jeffery'";
#else

#endif

            return sqlDataSet(strConnection, strCommand, "UserManagement");
        }

        ///************************************************************************
        /// <summary>
        /// sqlDataReader_Test
        /// </summary>
        public static void sqlDataReader_Test()
        {
            //            strConnection = @"data source=(local);
            //                              Database=BrainsCitrine;
            //                              user id=bruser;
            //                              password=BR%ccsz";
#if (DEBUG)
            strConnection = "server=(local);database =BrainsCitrine;integrated security=true";
            //strCommand = "Select * From 機番進捗DB Where 本体号機='A0VD002000629'";
            //strCommand = "Select * From 共通製造履歴DB Where 本体号機='A0VD002000629'";

            strCommand = "Select * From UserManagement Where UserID='test'";
#else

#endif

            sqlExecuteReader(strConnection, strCommand);
        }

        #endregion

        #region "sqlDB Methods"
        /// <summary>
        /// Load_sqlAccount
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="Password"></param>
        public static void sqlLoad_Account(string UserID, string Password)
        {
            sqlDatabase.UserID = UserID;
            sqlDatabase.Password = Password;
        }

        ///************************************************************************
        /// <summary>
        /// sqlConnectionStr for SQL Connection String (Type 1)
        /// </summary>
        /// <param name="Server"></param>
        /// <param name="Database"></param>
        /// <param name="Integrated_Security"></param>
        /// <param name="UserID"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public static string sqlConnectionStr(string Server, string Database, string Integrated_Security)
        {
            if ((Server == string.Empty) || (Server == null))
            {
#if (DEBUG)
                Server = ServerName; 
#else
                Server = ServerName;
#endif
            }
            strConnection = "Server = " + Server + ";";

            if ((Database == string.Empty) || (Database == null))
            {
#if (DEBUG)
                Database = DatabaseName;
#else
                Database = DatabaseName;
#endif

            }
            strConnection += "Database = " + Database + ";";


            ///Integrated Security value: SSPI(=true), true, false            
            if (((Integrated_Security == string.Empty) || (Integrated_Security == null)))
            {
                Integrated_Security = caIntegrated_Security_SSPI;
            }
            strConnection += "Integrated Security =" + Integrated_Security + ";";

            return strConnection;
        }

        ///************************************************************************
        /// <summary>
        /// sqlConnectionStr for SQL Connection String (Type 2)
        /// </summary>
        /// <param name="Server"></param>
        /// <param name="Database"></param>
        /// <param name="Integrated_Security"></param>
        /// <param name="UserID"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        public static string sqlConnectionStr(string Server, string Database, string Integrated_Security, string UserID, string Password)
        {
            if ((Server == string.Empty) || (Server == null))
            {
#if (DEBUG)
                Server = "(local)"; //Local PC
#else
                Server = sqlDatabase.ServerName_Brains_CC1;
#endif

            }
            strConnection = "Server = " + Server + ";";

            if ((Database == string.Empty) || (Database == null))
            {
#if (DEBUG)
                Database = DatabaseName;
#else
                Database = DatabaseName;
#endif

            }
            strConnection += "Database = " + Database + ";";


            ///Integrated Security value: SSPI(=true), true, false            
            if (string.Equals(Integrated_Security.ToUpper(), "true".ToUpper()) || string.Equals(Integrated_Security.ToUpper(), "SSPI"))
            {
                strConnection += "Integrated Security =" + Integrated_Security + ";";
            }

            else if (string.Equals(Database.ToUpper(), "false".ToUpper()))
            {
                //For UserID = BrUser Check 
                if (string.Equals(UserID.ToUpper(), UserID.ToUpper()))
                {
                    strConnection += "UserID =" + UserID + ";";

                    if (string.Equals(Password, Password))
                    {
                        strConnection += "Password =" + Password + ";";
                    }
                }

                //For UserID = BrReader Check 
                else if (string.Equals(UserID.ToUpper(), UserID.ToUpper()))
                {
                    strConnection += "UserID =" + UserID + ";";

                    if (string.Equals(Password, Password))
                    {
                        strConnection += "Password =" + Password + ";";
                    }
                }
            }

            return strConnection;
        }

        ///************************************************************************
        /// <summary>
        /// sqlSelectCommandStr (Type 1)
        /// </summary>
        /// <param name="FieldName"></param>
        /// <param name="TableName"></param>
        /// <returns></returns>
        public static string sqlSelectCommandStr(string FieldName, string TableName)
        {
            //strConnection = "server=(local);database =BrainsCitrine;integrated security=true";
            //strCommand = "Select * From UserManagement Where UserID='Jeffery'";

            if ((FieldName == string.Empty) || (FieldName == null))
            {
                FieldName = "*";
            }
            strCommand = "SELECT " + FieldName;

            if ((TableName == string.Empty) || (TableName == null))
            {
                Error.errorMsg = TestResult.FAIL + "sqlSelect Fail" + TestResult.NewLine +
                                 "Table Name Incorrectly!" + TestResult.NewLine +
                                 "Table Name = " + TableName.ToString();

                Error.ErrorHandle(Error.Error_TitleMsg, Error.errorMsg);

                return Error.errorMsg;
            }
            strCommand += " FROM " + TableName;
            
            return strCommand;
        }
        
        /// <summary>
        /// sqlSelectCommandStr (Type 2)
        /// 'SELECT "欄位名"
        /// 'FROM "表格名"
        /// '[WHERE "條件"]
        /// 'ORDER BY "欄位名" [ASC, DESC]
        /// 'SQL ORDER BY: ASC(小到大),DESC(大到小)
        /// </summary>
        /// <returns></returns>
        public static string sqlSelectCommandStr(string FieldName, string TableName, string WhereCondition)
        {
            //strConnection = "server=(local);database =BrainsCitrine;integrated security=true";
            //strCommand = "Select * From UserManagement Where UserID='Jeffery'";

            if ((FieldName == string.Empty) || (FieldName == null))
            {
                FieldName = "*";
            }
            strCommand = "SELECT " + FieldName;

            if ((TableName == string.Empty) || (TableName == null))
            {
                Error.errorMsg = TestResult.FAIL + "sqlSelect Fail" + TestResult.NewLine +
                                 "Table Name Incorrectly!" + TestResult.NewLine +
                                 "Table Name = " + TableName.ToString();

                Error.ErrorHandle(Error.Error_TitleMsg, Error.errorMsg);

                return Error.errorMsg;
            }
            strCommand += " FROM " + TableName;

            if ((WhereCondition != string.Empty) || (WhereCondition != null))
            {
                strCommand += " WHERE " + WhereCondition;
            }
            return strCommand;
        }
        
        /// <summary>
        /// sqlSelectCommandStr (Type 3)
        /// 'INSERT INTO "表格名" ("欄位1", "欄位2", ...)
        /// 'VALUES ("值1", "值2", ...) 
        /// </summary>
        /// <param name="FieldName"></param>
        /// <param name="TableName"></param>
        /// <param name="WhereCondition"></param>
        /// <param name="OrderBy"></param>
        /// <returns></returns>
        public static string sqlSelectCommandStr(string FieldName, string TableName, string WhereCondition, string OrderBy)
        {
            //strConnection = "server=(local);database =BrainsCitrine;integrated security=true";
            //strCommand = "Select * From UserManagement Where UserID='Jeffery'";

            if ((FieldName == "*") || (FieldName == string.Empty) || (FieldName == null))
            {
                FieldName = "*";
            }
            strCommand = "SELECT " + FieldName;

            if ((TableName == string.Empty) || (TableName == null))
            {
                Error.errorMsg = TestResult.FAIL + "sqlSelect Fail" + TestResult.NewLine +
                                 "Table Name Incorrectly!" + TestResult.NewLine +
                                 "Table Name = " + TableName.ToString();

                Error.ErrorHandle(Error.Error_TitleMsg, Error.errorMsg);

                return Error.errorMsg;
            }
            strCommand += " FROM " + TableName;

            if ((WhereCondition != string.Empty) || (WhereCondition != null))
            {
                strCommand += " WHERE " + WhereCondition;
            }

            if ((OrderBy != string.Empty) || (OrderBy != null))
            {
                strCommand += " ORDER BY " + OrderBy;
            }

            return strCommand;
        }

        ///************************************************************************
        /// <summary>
        /// sqlInsertCommandStr
        /// 'INSERT INTO "表格名" ("欄位1", "欄位2", ...)
        /// 'VALUES ("值1", "值2", ...)
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="InsertField"></param>
        /// <param name="InsertValue"></param>        
        /// <returns></returns>       
        public static string sqlInsertCommandStr(string TableName, string InsertField, string InsertValue)
        {
            //strCommand = "INSERT INTO UserManagement (UserID, Password, MACAddress) Values('Jeffery','Jeffery','11:22:33:44:55:66')";

            if ((TableName == string.Empty) || (TableName == null))
            {
                Error.errorMsg = TestResult.FAIL + "sqlInsertCommand Fail" + TestResult.NewLine +
                                 "Table Name Incorrectly!" + TestResult.NewLine +
                                 "Table Name = " + TableName.ToString();

                Error.ErrorHandle(Error.Error_TitleMsg, Error.errorMsg);

                return Error.errorMsg;
            }
            strCommand = "INSERT INTO " + TableName;

            if ((InsertField == string.Empty) || (InsertField == null))
            {
                Error.errorMsg = TestResult.FAIL + "sqlInsertCommand Fail" + TestResult.NewLine +
                                 "InsertField Incorrectly" + TestResult.NewLine +
                                 "InsertField = " + InsertField.ToString();

                Error.ErrorHandle(Error.Error_TitleMsg, Error.errorMsg);

                return Error.errorMsg;
            }

            strCommand += " (" + InsertField + ")";

            //Values
            if ((InsertValue.ToString() == string.Empty) || (InsertValue.ToString() == null))
            {
                Error.errorMsg = TestResult.FAIL + "sqlInsertCommand Fail" + TestResult.NewLine +
                                 "InsertValue Incorrectly" + TestResult.NewLine +
                                 "InsertValue = " + InsertValue.ToString();

                Error.ErrorHandle(Error.Error_TitleMsg, Error.errorMsg);

                return Error.errorMsg;
            }
            strCommand += " VALUES (" + InsertValue + ")";

            return strCommand;
        }

        ///************************************************************************
        /// <summary>
        /// sqlUpdateCommandStr
        /// 'Update "表格名"
        /// 'SET "欄位1" = [新值]
        /// 'WHERE {條件}
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="SetValue"></param>
        /// <param name="Where"></param>
        /// <returns></returns>
        public static string sqlUpdateCommandStr(string TableName, string SetValue, string WhereCondition)
        {
            //strCommand = "UPDATE UserManagement SET UserID='Aby' WHERE (UserID='Jeffery')";

            if ((TableName == string.Empty) || (TableName == null))
            {
                Error.errorMsg = TestResult.FAIL + "sqlUpdateCommand Fail" + TestResult.NewLine +
                                 "Table Name Incorrectly!" + TestResult.NewLine +
                                 "Table Name = " + TableName;

                Error.ErrorHandle(Error.Error_TitleMsg, Error.errorMsg);

                return Error.errorMsg;
            }
            strCommand = "UPDATE " + TableName;

            if ((SetValue == string.Empty) || (SetValue == null))
            {
                Error.errorMsg = TestResult.FAIL + "sqlUpdateCommand Fail" + TestResult.NewLine +
                                 "SetValue Incorrectly!" + TestResult.NewLine +
                                 "SetValue = " + SetValue;

                Error.ErrorHandle(Error.Error_TitleMsg, Error.errorMsg);

                return Error.errorMsg;
            }
            strCommand += " SET " + SetValue;

            if ((WhereCondition == string.Empty) || (WhereCondition == null))
            {
                Error.errorMsg = TestResult.FAIL + "sqlUpdateCommand Fail" + TestResult.NewLine +
                                 "Where Condition Incorrectly!" + TestResult.NewLine +
                                 "Where Condition = " + WhereCondition;

                Error.ErrorHandle(Error.Error_TitleMsg, Error.errorMsg);

                return Error.errorMsg;
            }
            strCommand += " WHERE " + WhereCondition;

            return strCommand;
        }

        ///************************************************************************
        /// <summary>
        /// sqlDeleteCommandStr
        /// 'DELETE FROM "表格名"
        /// 'WHERE {條件}
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="WhereCondition"></param>
        /// <returns></returns>
        public static string sqlDeleteCommandStr(string TableName, string WhereCondition)
        {
            //strCommand = "DELETE FROM UserManagement WHERE (UserID='Aby')";

            if ((TableName == string.Empty) || (TableName == null))
            {
                Error.errorMsg = TestResult.FAIL + "sqlDeleteCommand Fail \r\n" +
                                 "Table Name Incorrectly! \r\n" +
                                 "Table Name = " + TableName;

                Error.ErrorHandle(Error.Error_TitleMsg, Error.errorMsg);

                return Error.errorMsg;
            }
            strCommand = "DELETE FROM " + TableName;

            if ((WhereCondition == string.Empty) || (WhereCondition == null))
            {
                Error.errorMsg = TestResult.FAIL + "sqlDeleteCommand Fail" + TestResult.NewLine +
                                 "WhereCondition Incorrectly!" + TestResult.NewLine +
                                 "WhereCondition = " + WhereCondition;

                Error.ErrorHandle(Error.Error_TitleMsg, Error.errorMsg);

                return Error.errorMsg;
            }
            strCommand += " WHERE " + WhereCondition;

            return strCommand;
        }
        #endregion

        #region "sqlExecuteNonQuery"
        /// <summary>
        /// sqlExecuteNonQuery
        /// </summary>
        /// <param name="strConnection"></param>
        /// <param name="strCommand"></param>
        /// <returns></returns>

        public static int sqlExecuteNonQuery(string strConnection, string strCommand)
        {
            using (SqlConnection connection = new SqlConnection(strConnection))
            {
                using (SqlCommand cmd = new SqlCommand(strCommand, connection))
                {
                    int raws = 0;
                    try
                    {
                        connection.Open();
                        raws = cmd.ExecuteNonQuery();
                    }
                    catch (Exception eMsg)
                    {
                        Error.ExceptionHandle(Error.Exception_TitleMsg, eMsg.ToString());
                    }
                    finally
                    {
                        cmd.Parameters.Clear();
                        cmd.Connection.Close();
                    }
                    return raws;
                }
            }
        }

        //private string txtFile = "C:\\sqlDataReader.txt";

        ////StreamWriter
        //public void swOpenFile()
        //{
        //    StreamWriter sw = new StreamWriter(txtFile, true);
        //    sw.WriteLine("12345;"); //Change new line
        //    sw.Write("54321;"); //W/O new line
        //    sw.WriteLine("67890;");
        //    sw.Flush(); //Load data from buffer
        //    sw.Close();
        //}      

        ///************************************************************************
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strConnection"></param>
        /// <param name="strCommand"></param>
        public static void sqlExecuteReader(string strConnection, string strCommand)
        {
            using (SqlConnection connection = new SqlConnection(strConnection))
            {
                using (SqlCommand cmd = new SqlCommand(strCommand, connection))
                {
                    connection.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    //Open File
                    string txtFile = "C:\\sqlDataReader.txt";
                    StreamWriter sw = new StreamWriter(txtFile, true);
                    sw.WriteLine("[UserId],[Password],[MACAddress]");

                    while (dr.Read())
                    {

                        sw.WriteLine(dr[0].ToString() + "," + dr[1].ToString() + "," + dr[2].ToString());
                        //frmMain.IniWriteValue("Initial", "App_Name", "Jeffery Sample Code");
                    }

                    sw.Flush(); //Load data from buffer
                    sw.Close();

                    cmd.Parameters.Clear();
                    cmd.Connection.Close();
                }
            }
        }

        ///************************************************************************
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strConnection"></param>
        /// <param name="strCommand"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static DataSet sqlDataSet(string strConnection, string strCommand, string tableName)
        {
            using (SqlConnection connection = new SqlConnection(strConnection))
            {
                DataSet ds = new DataSet();

                try
                {
                    connection.Open();
                    SqlDataAdapter cmd = new SqlDataAdapter(strCommand, connection);

                    cmd.Fill(ds, tableName);
                }
                catch (Exception eMsg)
                {
                    Error.ExceptionHandle(Error.Exception_TitleMsg, eMsg.ToString());
                }
                finally
                {
                    connection.Close();

                }
                return ds;
            }
        }
        #endregion

        #region "DataTable"
        public static void getDataTableValue_Test()
        {
#if (DEBUG)
            strConnection = "server=(local);database =BrainsCitrine;integrated security=SSPI";
            //strCommand = "Select * From 機番進捗DB Where 本体号機='A0VD002000629'";

            //'Update
            //'Update "表格名"
            //'SET "欄位1" = [新值]
            //'WHERE {條件}
            strCommand = "Select * From UserManagement"; // Where UserID='Jeffery'";
#else

#endif
            ConditionValue = "UserID = 'Aby' AND Password = 'Jeffery'";

            myDataTable = getDatatable_Value(strConnection, strCommand);

            if (myDataTable == null)
            {
                MessageBox.Show("DataTable == Null", "Error", MessageBoxButtons.OK);
                return;
            }
            else
            {
                //UserID = getDataTable_FieldValue(myDatatable, ConditionValue, 0, "UserID");
                //Password = getDataTable_FieldValue(myDatatable, ConditionValue, 0, "Password");
                //MacAddress = getDataTable_FieldValue(myDatatable, ConditionValue, 0, "MacAddress");
            }

            myDataTable.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strConnection"></param>
        /// <param name="strCommand"></param>
        /// <returns></returns>
        public static DataTable getDatatable_Value(string strConnection, string strCommand)
        {
            using (SqlConnection myConn = new SqlConnection(strConnection))
            {
                using (SqlCommand myCmd = new SqlCommand())
                {
                    using (DataTable myTable = new DataTable())
                    {
                        try
                        {
                            myCmd.Connection = myConn;
                            myCmd.CommandText = strCommand;
                            SqlDataAdapter myDA = new SqlDataAdapter();
                            myDA.SelectCommand = myCmd;

                            myConn.Open();
                            myDA.Fill(myTable);

                        }
                        catch (Exception ex)
                        {
                            ex.ToString();
                        }
                        finally
                        {
                            myConn.Close();
                        }

                        return myTable;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="Conditionvalue"></param>
        /// <param name="RowIndex"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static string getDataTable_FieldValue(DataTable TableName, string Conditionvalue, int RowIndex, string columnName)
        {
            DataRow[] Table = TableName.Select(Conditionvalue);
            
            if (Table == null || Table.Length == 0)
            {
                return string.Empty;
            }

            return Table[RowIndex][columnName].ToString();
            
        }

        //public static int getDataTable_FieldValue(DataTable TableName, string Conditionvalue, int RowIndex, string columnName)
        //{
        //    DataRow[] Table = TableName.Select(Conditionvalue);

        //    if (Table == null || Table.Length == 0)
        //    {
        //        return string.Empty;
        //    }

        //    return Convert.ToInt32( Table[RowIndex][columnName]);
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="FieldName"></param>
        /// <returns></returns>
        public static int getDataTable_FieldLength(DataTable TableName, string FieldName)
        {
            int rowCount = 0;

            foreach (DataRow row in TableName.Rows)
            {
                if (row[FieldName].ToString() != string.Empty && row[FieldName].ToString() != null)
                {
                    rowCount++;
                }
            }
            return rowCount;
        }

        /// <summary>
        /// getDataTable_ConditionLength
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="Conditionvalue"></param>
        /// <returns></returns>
        public static int getDataTable_ConditionLength(DataTable TableName, string Conditionvalue)
        {
            DataRow[] Table = TableName.Select(Conditionvalue);

            if (Table == null || Table.Length == 0)
            {
                return 0;
            }

            return Table.Length;
        }

        #endregion

        #region "sqlExecuteScalar"
        /// <summary>
        /// sqlExecuteScalar
        /// </summary>
        /// <param name="strConnection"></param>
        /// <param name="strCommand"></param>
        /// <returns></returns>
        public static object sqlExecuteScalar(string strConnection, string strCommand)
        {
            using (SqlConnection connection = new SqlConnection(strConnection))
            {
                using (SqlCommand cmd = new SqlCommand(strCommand, connection))
                {
                    object raws = 0;
                    try
                    {
                        connection.Open();
                        raws = cmd.ExecuteScalar();

                    }
                    catch (Exception eMsg)
                    {
                        Error.ExceptionHandle(Error.Exception_TitleMsg, eMsg.ToString());
                    }
                    finally
                    {
                        cmd.Parameters.Clear();
                        cmd.Connection.Close();
                    }
                    return raws;
                }
            }
        }

        #endregion

        

    }

    public class ODBC
    {

        #region "ODBC Arguments"

#if (DEBUG)
        public static string ODBC_DSN = "NKG_DRRS";
        public static string ODBC_UID = "DRRSUser";
        public static string ODBC_PWD = "DRRS@ccth";
        public static string ODBC_ServerName = "(Local)";
        public static string ODBC_DatabaseName = "NKG_DRRS";
#else
        public static string ODBC_DSN = string.Empty;
        public static string ODBC_UID = string.Empty;
        public static string ODBC_PWD = string.empty;
        public static string ODBC_ServerName = string.Empty;
        public static string ODBC_DatabaseName = string.Empty;
#endif
        #endregion

        #region "Config Arguments"

        public class Config
        {

            /// <summary>
            /// Table Field Name for Lines, Models, Stations, Factory Tables
            /// </summary>
            public const string caField_AttrKey = "AttrKey";
            public const string caField_AttrName = "AttrName";
            public const string caField_AttrValue = "AttrValue";
            public const string caField_Description = "Description";


            /// <summary>
            /// Config Table Name
            /// </summary>
            public const string caTable_Factory = "Factory";
            public const string caTable_Lines = "Lines";
            public const string caTable_Models = "Models";
            public const string caTable_Acount = "Acount";
            public const string caTable_Stations = "Stations";
            public const string caTable_DataBase = "DataBase";
            public const string caTable_Language = "Language";

            /// <summary>
            /// Static Arguments
            /// </summary>
            public static string DSN = "DRRS_Config";
            public static string FilePath = string.Empty;
            public static string TableName = string.Empty;

            public static string[] List_Lines;
            public static string[] List_Stations;
            public static string[] List_Language;

            /// <summary>
            /// Config Datatable
            /// </summary>
            public static DataTable myDataTable;    //For Temp DataTable
            public static DataTable DT_Factory;
            public static DataTable DT_Lines;
            public static DataTable DT_Models;
            public static DataTable DT_Acount;
            public static DataTable DT_Stations;
            public static DataTable DT_DataBase;
            public static DataTable DT_Language;

        }
        
        #endregion

        #region "DRRS_Database Arguments"

        public class DRRS_Database
        {
            public static string DSN = "DRRS_Database";

            /// <summary>
            /// List Arguments
            /// </summary>
            public static string[] DefectList;
            public static string[] DefectList_DefectCode;
            public static string[] DefectList_English;
            public static string[] DefectList_Chinese;
            public static string[] DefectList_ThaiLanguage;
            public static string[] DefectList_Japanese;
            public static string[] DefectList_MyanmarLanguage;

            public static string[] RootcauseList;
            public static string[] RootcauseList_RootcauseCode;
            public static string[] RootcauseList_English;
            public static string[] RootcauseList_Chinese;
            public static string[] RootcauseList_ThaiLanguage;
            public static string[] RootcauseList_Japanese;
            public static string[] RootcauseList_MyanmarLanguage;

            public static string[] ActionList;
            public static string[] ActionList_ActionCode;
            public static string[] ActionList_English;
            public static string[] ActionList_Chinese;
            public static string[] ActionList_ThaiLanguage;
            public static string[] ActionList_Japanese;
            public static string[] ActionList_MyanmarLanguage;
                        
            /// <summary>
            /// Table Name
            /// </summary>
            public const string caTable_DefectList = "DefectList";
            public const string caTable_RootcauseList = "RootcauseList";
            public const string caTable_ActionList = "ActionList";
            public const string caTable_DRRS_Main = "DRRS_Main";

            /// <summary>
            /// Language Type
            /// </summary>
            public const string caLanguage_English = "English";
            public const string caLanguage_Chinese = "Chinese";
            public const string caLanguage_ThaiLanguage = "ThaiLanguage";
            public const string caLanguage_Japanese = "Japanese";
            public const string caLanguage_MyanmarLanguage = "MyanmarLanguage";

            /// <summary>
            /// Code Type
            /// </summary>
            public const string caCode_DefectCode = "DefectCode";
            public const string caCode_RootcauseCode = "RootcauseCode";
            public const string caCode_ActionCode = "ActionCode";

            /// <summary>
            /// DRRS_Database Datatable
            /// </summary>
            public static DataTable DT_DefectList;
            public static DataTable DT_RootcauseList;
            public static DataTable DT_ActionList;
            public static DataTable DT_DRRS_Main;

        }
        
        #endregion

        #region "ODBC Methods"

        /// <summary>
        /// ODBC_ConnectionStr (Type 1)
        /// For ODBC Connection string
        /// </summary>
        /// <param name="DSN"></param>
        /// <returns></returns>
        public static string ODBC_ConnectionStr(string DSN)
        {
            string myConnectionStr = "DSN=" + DSN;

            return myConnectionStr;

        }

        /// <summary>
        /// ODBC_ConnectionStr (Type 2)
        /// For ODBC Connection string
        /// </summary>
        /// <param name="DSN"></param>
        /// <param name="UID"></param>
        /// <param name="PWD"></param>
        /// <returns></returns>
        public static string ODBC_ConnectionStr(string DSN, string UID, string PWD)
        {
            string myConnectionStr = "DSN=" + DSN + ";UID=" + UID + ";PWD=" + PWD;

            return myConnectionStr;

        }

        /// <summary>
        /// ODBC sample code
        /// </summary>
        /// <param name="theDSN"></param>
        /// <param name="theUsername"></param>
        /// <param name="thePassword"></param>
        public static void ODBC_Connection(string theDSN, string theUsername, string thePassword)
        {
            using (OdbcConnection conn = new OdbcConnection(string.Format("DSN={0};Uid={1};Pwd={2}", theDSN, theUsername, thePassword)))
            {
                try
                {
                    conn.Open();

                }
                catch (Exception eMsg)
                {
                    Error.ExceptionHandle(Error.Exception_TitleMsg, eMsg.ToString());
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strConnection"></param>
        /// <param name="strCommand"></param>
        /// <returns></returns>
        public static DataTable ODBC_GetDatatableValue(string strConnection, string strCommand)
        {
            using (OdbcConnection myConn = new OdbcConnection(strConnection))
            {
                using (OdbcCommand myCmd = new OdbcCommand(strCommand, myConn))
                {
                    using (DataTable myTable = new DataTable())
                    {
                        try
                        {
                            OdbcDataAdapter myDA = new OdbcDataAdapter();
                            myDA.SelectCommand = myCmd;

                            myConn.Open();
                            myDA.Fill(myTable);

                        }
                        catch (Exception eMsg)
                        {
                            Error.ExceptionHandle(Error.Exception_TitleMsg, eMsg.ToString());
                        }
                        finally
                        {
                            myConn.Close();
                        }

                        return myTable;
                    }
                }
            }
        }

        /// <summary>
        /// ODBC Sample code
        /// </summary>
        /*
        DataTable dt = new DataTable();
        OdbcConnection myConnection = salesConnection();
        myConnection.Open();

        OdbcCommand selectCMD = new OdbcCommand("SELECT CUSTOMER.CUSTOMER_NBR " +
        "FROM CUSTOMER", myConnection);

        OdbcDataAdapter cmd = new OdbcDataAdapter();
        cmd.SelectCommand = selectCMD;

        cmd.Fill(dt);
        myConnection.Close();

        DataTable dtM = new DataTable();
        myConnection = mConnection();
        myConnection.Open();

        selectCMD = new OdbcCommand("SELECT '**DATA TABLE ABOVE**'.CUSTOMER_NBR "+ 
        "FROM '**HOW TO I REFERENCE THE DATA TABLE ABOVE**'", myConnection);

        cmd = new OdbcDataAdapter();
        cmd.SelectCommand = selectCMD;

        cmd.Fill(dtM);
        myConnection.Close();
        */

        /// <summary>
        /// ODBC_Load_Config_Lines
        /// </summary>
        public static void Load_Config_Lines()
        {
            //string ODBC_Connection = sqlDatabase.ODBC_ConnectionStr(sqlDatabase.ODBC_DSN, sqlDatabase.ODBC_UID, sqlDatabase.ODBC_PWD);
            string ODBC_Connection = ODBC.ODBC_ConnectionStr(ODBC.Config.DSN);
            string ODBC_Command = sqlDatabase.sqlSelectCommandStr(ODBC.Config.caField_AttrValue, ODBC.Config.caTable_Lines,
                                  ODBC.Config.caField_AttrName + "='LineNumber'");

            ODBC.Config.DT_Lines = ODBC.ODBC_GetDatatableValue(ODBC_Connection, ODBC_Command);

            string[] Config_Lines = new string[ODBC.Config.DT_Lines.Rows.Count];
            for (int i = 0; i < ODBC.Config.DT_Lines.Rows.Count; i++)
            {
                Config_Lines[i] = ODBC.Config.DT_Lines.Rows[i]["AttrValue"].ToString();
            }

            ODBC.Config.List_Lines = Config_Lines;

        }

        public static void Load_Config_Stations()
        {
            //string ODBC_Connection = sqlDatabase.ODBC_ConnectionStr(sqlDatabase.ODBC_DSN, sqlDatabase.ODBC_UID, sqlDatabase.ODBC_PWD);
            string ODBC_Connection = ODBC.ODBC_ConnectionStr(ODBC.Config.DSN);
            string ODBC_Command = sqlDatabase.sqlSelectCommandStr(ODBC.Config.caField_AttrValue, ODBC.Config.caTable_Stations,
                                  ODBC.Config.caField_AttrName + "='StationName'");

            ODBC.Config.DT_Stations = ODBC.ODBC_GetDatatableValue(ODBC_Connection, ODBC_Command);

            string[] Config_Stations = new string[ODBC.Config.DT_Stations.Rows.Count];
            for (int i = 0; i < ODBC.Config.DT_Stations.Rows.Count; i++)
            {
                Config_Stations[i] = ODBC.Config.DT_Stations.Rows[i]["AttrValue"].ToString();
            }

            ODBC.Config.List_Stations = Config_Stations;

        }

        public static void Load_Config_Language()
        {
            //string ODBC_Connection = sqlDatabase.ODBC_ConnectionStr(sqlDatabase.ODBC_DSN, sqlDatabase.ODBC_UID, sqlDatabase.ODBC_PWD);
            string ODBC_Connection = ODBC.ODBC_ConnectionStr(ODBC.Config.DSN);
            string ODBC_Command = sqlDatabase.sqlSelectCommandStr(ODBC.Config.caField_AttrValue, ODBC.Config.caTable_Language,
                                  ODBC.Config.caField_AttrName + "='Language'");

            ODBC.Config.DT_Language = ODBC.ODBC_GetDatatableValue(ODBC_Connection, ODBC_Command);

            string[] Config_Language = new string[ODBC.Config.DT_Language.Rows.Count];
            for (int i = 0; i < ODBC.Config.DT_Language.Rows.Count; i++)
            {
                Config_Language[i] = ODBC.Config.DT_Language.Rows[i]["AttrValue"].ToString();
            }

            ODBC.Config.List_Language = Config_Language;

        }

        public static void Load_DRRS_Database_DefectCode()
        {
            string ODBC_Connection = ODBC.ODBC_ConnectionStr(ODBC.DRRS_Database.DSN);
            string ODBC_Command = sqlDatabase.sqlSelectCommandStr("*", ODBC.DRRS_Database.caTable_DefectList);

            ODBC.DRRS_Database.DT_DefectList = ODBC.ODBC_GetDatatableValue(ODBC_Connection, ODBC_Command);

            string[] DefectCode = new string[ODBC.DRRS_Database.DT_DefectList.Rows.Count];
            for (int i = 0; i < ODBC.DRRS_Database.DT_DefectList.Rows.Count; i++)
            {
                DefectCode[i] = ODBC.DRRS_Database.DT_DefectList.Rows[i][ODBC.DRRS_Database.caCode_DefectCode].ToString();
            }

            ODBC.DRRS_Database.DefectList_DefectCode = DefectCode;
        }

        public static void Load_DRRS_Database_DefectList()
        {
            string ODBC_Connection = ODBC.ODBC_ConnectionStr(ODBC.DRRS_Database.DSN);
            string ODBC_Command = sqlDatabase.sqlSelectCommandStr("*", ODBC.DRRS_Database.caTable_DefectList);

            ODBC.DRRS_Database.DT_DefectList = ODBC.ODBC_GetDatatableValue(ODBC_Connection, ODBC_Command);                                

            //foreach (DataRow dr in ODBC.DRRS_Database.DT_DefectList.Rows)
            //{
                for (int i = 0; i < ODBC.DRRS_Database.DT_DefectList.Rows.Count; i++)
                {
                    ODBC.DRRS_Database.DefectList_DefectCode[i] = ODBC.DRRS_Database.DT_DefectList.Rows[i][ODBC.DRRS_Database.caCode_DefectCode].ToString() + " - "+ ODBC.DRRS_Database.DT_DefectList.Rows[i][ODBC.DRRS_Database.caLanguage_English].ToString();
                    //ODBC.DRRS_Database.DefectList_English[i] = ODBC.DRRS_Database.DT_DefectList.Rows[i][ODBC.DRRS_Database.caLanguage_English].ToString();
                }
            //}
                      
        }

        #region "ComboBox Method"

        /// <summary>
        /// Sample code for Combobox with multi values
        /// </summary>
        /// *********************************************************
        /*
          DataTable dt = new DataTable();
          foreach (DataRow dr in dt.Rows)
          {
              comboBox1.Items.Add(dr["Col1"].ToString() + dr["Col2"].ToString());
          }
        
         */

        //***********************************************************
        //Sample code for Datatable to Array
        /*
        SqlConnection cn = new SqlConnection(Constr);
            cn.Close();
            cn.Open();

            DataTable dtResult = new DataTable();

            // Populate Datatable
            string query ="select VALM1,VALM2,VALM3,VALM4,VALM5,VALM6,VALM7,VALM8,VALM9,VALM10,VALM11,VALM12 "
            + "from [TPRINCIPE] where [ID]=" +  DropDownList2.SelectedValue; // I have a ddl from which I pick the ID

            SqlCommand cmd = new SqlCommand(query, cn);

            dtResult.Load(cmd.ExecuteReader());

            // Convert into array
            int[] firstRow = dtResult.AsEnumerable()
                                   .Take(1);
                                   .ToArray();
        */
        //***********************************************************
        #endregion        

        #endregion
    
    }
}
