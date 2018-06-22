using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.IO;
using System.Xml;
using System.Text.RegularExpressions;
using System.Text;
using System.Collections;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Collections.Generic;

namespace EBA.Desktop.YEB
{
    /// <summary>
    /// Routines for Importing Data from Various Source files
    /// </summary>
    /// 4/23/2009 R.A. 
    /// Import various data files for YEB routine
    public class ImportYEBData
    {
        static string connStr = ConfigurationManager.ConnectionStrings["EBADB"].ConnectionString;
        static SqlConnection connect = new SqlConnection(connStr);
        static SqlCommand command = null;
        public ImportYEBData()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public int importCobraNP_YEB(string _yrmo, string _fileName, string _pind)
        {
            string _Reportyrmo = "";
            DataTable dt= new DataTable();
            dt.Rows.Clear();
           int _recCount = 0;
            dt=importCobraTextFile(_yrmo,_fileName,_pind);
            if (dt.Rows.Count > 0)
            {
                //Now insert the rows into the YEB Detail Table
               _recCount = insertYEBDetails(dt);
            }
            return _recCount;
        }

        static string calculateYRMO(string _month, string _year)
        {
            string _yrmo = null;
            switch (_month)
            {
                case "JANUARY": _yrmo = _year + "01";
                    break;
                case "FEBRUARY": _yrmo = _year + "02";
                    break;
                case "MARCH": _yrmo = _year + "03";
                    break;
                case "APRIL": _yrmo = _year + "04";
                    break;
                case "MAY": _yrmo = _year + "05";
                    break;
                case "JUNE": _yrmo = _year + "06";
                    break;
                case "JULY": _yrmo = _year + "07";
                    break;
                case "AUGUST": _yrmo = _year + "08";
                    break;
                case "SEPTEMBER": _yrmo = _year + "09";
                    break;
                case "OCTOBER": _yrmo = _year + "10";
                    break;
                case "NOVEMBER": _yrmo = _year + "11";
                    break;
                case "DECEMBER": _yrmo = _year + "12";
                    break;
            }
            return _yrmo;
        }

        public static string getPrevYRMO(int months)
        {
            DateTime prevmondt = DateTime.Today.AddMonths(-months);
            string prevyear = prevmondt.Year.ToString();
            string prevmonth = prevmondt.Month.ToString();
            if (prevmonth.Length == 1)
                prevmonth = "0" + prevmonth;
            string prevYRMO = prevyear + prevmonth;

            return prevYRMO;
        }

        public static string getPrevYRMO(string yrmo)
        {
            yrmo = yrmo.Insert(4, "/");
            DateTime prevYRMO = Convert.ToDateTime(yrmo).AddMonths(-1);
            string prev_year = prevYRMO.Year.ToString();
            string prev_month = prevYRMO.Month.ToString();
            if (prev_month.Length == 1)
                prev_month = "0" + prev_month;
            string prev_yrmo = prev_year + prev_month;

            return prev_yrmo;
        }

        public static DateTime GetLastDayofYRMO(string yrmo)
        {
            yrmo = yrmo.Substring(0, 4) + "/" + yrmo.Substring(4);
            int _month = Convert.ToDateTime(yrmo).Month;
            int _year = Convert.ToDateTime(yrmo).Year;
            int numberOfDays = DateTime.DaysInMonth(_year, _month);

            DateTime lastDay = new DateTime(_year, _month, numberOfDays);

            return lastDay;
        }       

        private DataTable CreateCobraFdxNPTable()
            //Create datatable structure for storing the Cobra Non Pilot Information
            //from Text File
        {
            // Create a new DataTable titled 'CobraFedexNP'
            DataTable CobraNPTable = new DataTable("CbrFdxNP");
            // Add column objects to the table to mirror the YEBdetail table structure.
            DataColumn yrmoColumn = new DataColumn();
            yrmoColumn.DataType = System.Type.GetType("System.String");
            yrmoColumn.ColumnName = "YRMO";
            yrmoColumn.AutoIncrement = false;
            CobraNPTable.Columns.Add(yrmoColumn);

            DataColumn PIColumn = new DataColumn();
            PIColumn.DataType = System.Type.GetType("System.String");
            PIColumn.ColumnName = "PilotInd";
            PIColumn.AutoIncrement = false;
            CobraNPTable.Columns.Add(PIColumn);

            DataColumn SourceCDColumn = new DataColumn();
            SourceCDColumn.DataType = System.Type.GetType("System.String");
            SourceCDColumn.ColumnName = "SourceCD";
            SourceCDColumn.AutoIncrement = false;
            CobraNPTable.Columns.Add(SourceCDColumn);

            DataColumn TypeCDColumn = new DataColumn();
            TypeCDColumn.DataType = System.Type.GetType("System.String");
            TypeCDColumn.ColumnName = "TypeCD";
            TypeCDColumn.DefaultValue = "EBACOB";
            CobraNPTable.Columns.Add(TypeCDColumn);
           
            DataColumn nameColumn = new DataColumn();
            nameColumn.DataType = System.Type.GetType("System.String");
            nameColumn.ColumnName = "Name";
            nameColumn.AutoIncrement = false;
            CobraNPTable.Columns.Add(nameColumn);

            DataColumn lastnameColumn = new DataColumn();
            lastnameColumn.DataType = System.Type.GetType("System.String");
            lastnameColumn.ColumnName = "lastName";
            lastnameColumn.AutoIncrement = false;
            CobraNPTable.Columns.Add(lastnameColumn);

            DataColumn ssnColumn = new DataColumn();
            ssnColumn.DataType = System.Type.GetType("System.String");
            ssnColumn.ColumnName = "SSNO";
            ssnColumn.DefaultValue = "000000000";
            CobraNPTable.Columns.Add(ssnColumn);

            DataColumn addr1Column = new DataColumn();
            addr1Column.DataType = System.Type.GetType("System.String");
            addr1Column.ColumnName = "Addr1";
            CobraNPTable.Columns.Add(addr1Column);

            DataColumn addr2Column = new DataColumn();
            addr2Column.DataType = System.Type.GetType("System.String");
            addr2Column.ColumnName = "Addr2";
            CobraNPTable.Columns.Add(addr2Column);

            DataColumn cityColumn = new DataColumn();
            cityColumn.DataType = System.Type.GetType("System.String");
            cityColumn.ColumnName = "City";
            CobraNPTable.Columns.Add(cityColumn);

            DataColumn stateColumn = new DataColumn();
            stateColumn.DataType = System.Type.GetType("System.String");
            stateColumn.ColumnName = "State";
            CobraNPTable.Columns.Add(stateColumn);

            DataColumn zipColumn = new DataColumn();
            zipColumn.DataType = System.Type.GetType("System.String");
            zipColumn.ColumnName = "ZipCode";
            CobraNPTable.Columns.Add(zipColumn);

            DataColumn yebOLColumn = new DataColumn();
            yebOLColumn.DataType = System.Type.GetType("System.Boolean");
            yebOLColumn.ColumnName = "YEBonline";
            yebOLColumn.DefaultValue = false;
            CobraNPTable.Columns.Add(yebOLColumn);

            DataColumn EENOColumn = new DataColumn();
            EENOColumn.DataType = System.Type.GetType("System.Int32");
            EENOColumn.ColumnName = "EENO";
            EENOColumn.DefaultValue = 0;
            CobraNPTable.Columns.Add(EENOColumn);

            DataColumn expatColumn = new DataColumn();
            expatColumn.DataType = System.Type.GetType("System.Boolean");
            expatColumn.ColumnName = "expatFlag";
            expatColumn.DefaultValue = false;
            CobraNPTable.Columns.Add(expatColumn);

            // Create an array for DataColumn objects.
            DataColumn[] keys = new DataColumn[1];
            keys[0] = ssnColumn;
            CobraNPTable.PrimaryKey = keys;
            // Return the new DataTable.
            return CobraNPTable;
        }

        private DataTable importCobraTextFile(string yrmo,string filepath, string pind)
        {

            StreamReader fh = new StreamReader(filepath);
            string s = "";
            DataTable dt = new DataTable();
            //Declare a row for the datatable
            DataRow myRow;
            DataRow excRow;
            //primary key value for the current row added to Datatable
            string pkey ="";
            dt = CreateCobraFdxNPTable();
            Int32 rowCounter = 0;

            while ((s = fh.ReadLine()) != null)
            {

                try
                {
                    if (rowCounter == 0)
                    {
                        string[] f = s.Split('\t');
                        string col1 = f[0];
                        string col2 = f[1];
                        string col3 = f[2];
                        string col4 = f[3];
                        string col5 = f[4];
                        string col6 = f[5];
                        string col7 = f[6];
                        string col8 = f[7];
                        rowCounter++;
                    }
                    else
                    {

                        //Changes the order of fields 6/15/2009
                        //Andrea will be using a new file with fields
                        //lastname and firstname

                        string[] f = s.Split('\t');
                        string ssn = f[0].Trim().ToUpper();
                        ssn = ssn.Replace("-", "").Substring(0, 9);
                        string name = f[1].Trim().ToUpper().ToString() + " " + f[2].Trim().ToUpper().ToString();
                        string lname = "";
                        //Retrieve the lastname for this emp from the employee table
                        //to append the last name to the YEB distribution list
                        //lname = getEmployeeLastname(name);
                        //6-15-2009 Now lastname is a filed in the file
                        lname = Convert.ToString(f[1]).Trim().ToUpper();
                        string addr1 = Convert.ToString(f[3]).Trim().ToUpper();
                        string addr2 = Convert.ToString(f[4].Trim().ToUpper());
                        string city = Convert.ToString(f[5].Trim().ToUpper());
                        string state = Convert.ToString(f[6].Trim().ToUpper());
                        string zip = Convert.ToString(f[7]).Trim();
                        myRow = dt.NewRow();
                        // Then add the new row to the collection.
                        myRow["yrmo"] = yrmo;
                        myRow["pilotind"] = pind;
                        myRow["sourceCD"] = "COB" + pind;
                        myRow["Name"] = name;
                        myRow["SSNO"] = ssn;
                        pkey=ssn;
                        myRow["lastname"] = lname;
                        myRow["Addr1"] = addr1;
                        myRow["Addr2"] = addr2;
                        myRow["City"] = city;
                        myRow["State"] = state;
                        myRow["ZipCode"] = zip;
                        dt.Rows.Add(myRow);
                        //increment the rowcounter
                        rowCounter++;
                    }
                    
                }
                catch (System.Data.ConstraintException ex)
                {
                   
                    //    /* special case for primary key violation */
                    //    Duplicate rows found in the source file
                    //Assign the source as Cobra + Pilot indicator Var and Yrmo
                    //Find the original row and asssign it to the exception row var
                    excRow = dt.Rows.Find(pkey);
                    // insert the duplicate row found to the YEB_DuplicateDetail table
                    insertYEB_DupDetails(excRow);
                    continue;
                 }

            }
            fh.Close();
            //dt.WriteXml(@"C:\Documents and Settings\739635\My Documents\FEProjects\YEB\CobraNP.xml");
            return dt;

        }
        public static int updateYEBDetailData(string _yrmo, string _source)
        {
            int _counter = 0; 
            try
            {
                if (connect != null && connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand("[dbo].[sp_YEB_YEBDetails_Upd]", connect);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@yrmo", _yrmo);
                command.Parameters.AddWithValue("@source", _source);
                _counter=command.ExecuteNonQuery();

                
            }
            catch (Exception ex)
            {
                throw (new Exception("Error updating YEB Detail data"));
            }
            finally
            {
                connect.Close();
            }
            return _counter;
        }
        public static DataSet getYEBData(string _yrmo,string _source)
        {
            DataSet dsbresult = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                if (connect != null && connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand("[dbo].[sp_YEB_getResults]", connect);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@yrmo", _yrmo);
                command.Parameters.AddWithValue("@source", "Summary");
                da.SelectCommand = command;
                da.Fill(dsbresult);
            }
            catch (Exception ex)
            {
                throw (new Exception("Error getting YEB Detail data"));
            }
            finally
            {
                connect.Close();
            }
            return dsbresult;
        }
        public static DataSet getYEBDuplicateRecs(string _pilotind, string _yrmo, string _col)
            //6-16-2009this function will replace the 2 functions below
        //getYEBDuplicateRecsbySSN and getYEBDuplicateRecsbyADDR
        {
            DataSet dsbresult = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                if (connect != null && connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand("[dbo].[sp_YEB_GetYEBDuplicates]", connect);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@pilotind", _pilotind);
                command.Parameters.AddWithValue("@yrmo", _yrmo);
                command.Parameters.AddWithValue("@col", _col);
                da.SelectCommand = command;
                da.Fill(dsbresult);
            }
            catch (Exception ex)
            {
                throw (new Exception("Error getting Duplicate YEB Records Detail"));
            }
            finally
            {
                connect.Close();
            }
            return dsbresult;
        }
        public static DataSet getYEBDuplicateRecsbySSN(string _pilotind)
        {
            //6-16-2009 this function is replaced by getYEBDuplicateRecs
            DataSet dsbresult = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                if (connect != null && connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand("[dbo].[sp_YEB_GetEmployeeDuplicatesbySSN]", connect);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@pilotind", _pilotind);
               da.SelectCommand = command;
                da.Fill(dsbresult);
            }
            catch (Exception ex)
            {
                throw (new Exception("Error getting Duplicate YEB Records Detail by SSN"));
            }
            finally
            {
                connect.Close();
            }
            return dsbresult;
        }
        public static DataSet getYEBDuplicateRecsbyADDR(string _pilotind)
        {
            //6-16-2009 this function is replaced by getYEBDuplicateRecs
            DataSet dsbresult = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                if (connect != null && connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand("[dbo].[sp_YEB_GetEmployeeDuplicatesbyADDR]", connect);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@pilotind", _pilotind);
                da.SelectCommand = command;
                da.Fill(dsbresult);
            }
            catch (Exception ex)
            {
                throw (new Exception("Error getting Duplicate YEB Records Detail by ADDR"));
            }
            finally
            {
                connect.Close();
            }
            return dsbresult;
        }
        public static DataSet getYEBEmployeesbyStatus(string _pilotind, string _status)
        {
            DataSet dsbresult = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {
                if (connect != null && connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand("[dbo].[sp_YEB_GetEmployeesbyStatus]", connect);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@status", _status);
                command.Parameters.AddWithValue("@pilotind", _pilotind);
                da.SelectCommand = command;
                da.Fill(dsbresult);
            }
            catch (Exception ex)
            {
                throw (new Exception("Error getting YEB Detail data"));
            }
            finally
            {
                connect.Close();
            }
            return dsbresult;
        }
        public void insertWWRetData(DataRow[] dr, string yrmo, string pind, string src, string typecd)
        {
            //returns the number of rows inserted into the yebdetail table
             insertYEBDetails(dr,yrmo,pind,src,typecd);
        }
        public int insertYEB_ACT_LOA_Data(DataTable dt, string yrmo, string pind, string src, string typecd)
        {
            //returns the number of rows inserted into the yebdetail table
            return insertYEBDetails(dt,yrmo,pind,src,typecd);
        }
        public int insertYEBOLData(DataTable dt)
        {
            //returns the number of rows inserted into the yebdetail table
            return insertYEB_OL_Imports(dt);
        }

        public int insertYEBExpatData(DataTable dt)
        {
            //returns the number of rows inserted into the yebexpat group table
            return insertYEB_Expat_Imports(dt);
        }
        public int insertYEBDupData(DataTable dt, string dupcolname)
        {
            //returns the number of rows inserted into the yebexpat group table
            return insertYEB_DupDetails(dt, dupcolname);
        }

        protected static int insertYEBDetails(DataTable dt)
        {
            SqlCommand command = null;
            string connStr = ConfigurationManager.ConnectionStrings["EBADB"].ConnectionString;
            SqlConnection connect = new SqlConnection(connStr);
            int _counter = 0;            
            command = new SqlCommand("sp_YEB_InsertYEBDetail", connect);
            command.CommandType = CommandType.StoredProcedure;
            if (connect != null && connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            try
                
            {
                //loop thru the datatable rows for inserting to YEBdetail table
                foreach (DataRow dr in dt.Rows)
                {
                command.Parameters.AddWithValue("@yrmo", dr["yrmo"].ToString());
                command.Parameters.AddWithValue("@pilotind", dr["pilotind"].ToString());
                command.Parameters.AddWithValue("@SourceCd", dr["sourceCd"].ToString());
                //If the Yebdetaildata is being imported from source files then 
                //@rectype param is set to IMP
                command.Parameters.AddWithValue("@Rectype", "IMP");
                command.Parameters.AddWithValue("@TypeCd", dr["TypeCd"].ToString());
                command.Parameters.AddWithValue("@name", dr["name"].ToString().ToUpper());
                if (dr["lastname"] == DBNull.Value)
                {
                    command.Parameters.AddWithValue("@lastname", DBNull.Value);
                }
                else
                {
                    command.Parameters.AddWithValue("@lastname", dr["lastname"].ToString().ToUpper());
                }

                command.Parameters.AddWithValue("@ssno", dr["ssno"].ToString());
                command.Parameters.AddWithValue("@addr1", dr["addr1"].ToString());
                command.Parameters.AddWithValue("@addr2", dr["addr2"].ToString());
                command.Parameters.AddWithValue("@city", dr["city"].ToString());
                command.Parameters.AddWithValue("@state", dr["state"].ToString());
                command.Parameters.AddWithValue("@zipcode", dr["zipcode"].ToString());
                if (dr["eeno"] != null && dr["eeno"].ToString().Length > 1)
                {
                    command.Parameters.AddWithValue("@eeno", Convert.ToInt32(dr["eeno"].ToString()));
                }
                else
                {
                    command.Parameters.AddWithValue("@eeno", 0);
                }
                if (dr["yebonline"] != null && Convert.ToBoolean(dr["yebonline"].ToString()) == false)
                {
                    command.Parameters.AddWithValue("@yebonline", false);
                }
                else if (dr["yebonline"] != null && Convert.ToBoolean(dr["yebonline"].ToString()) == true)
                {
                    command.Parameters.AddWithValue("@yebonline", true);
                }
                else
                {
                    command.Parameters.AddWithValue("@yebonline", false);
                }
                
                if (dr["expatflag"] != null && Convert.ToBoolean(dr["expatflag"].ToString()) == false)
                {
                    command.Parameters.AddWithValue("@expatflag", false);
                }
                else if (dr["expatflag"] != null && Convert.ToBoolean(dr["expatflag"].ToString()) == true)
                {
                    command.Parameters.AddWithValue("@expatflag", true);
                }
                else
                {
                    command.Parameters.AddWithValue("@expatflag", false);
                }
                //execute the query
                command.ExecuteNonQuery();
                    //increment the rowcounter
                 _counter++;
                 command.Parameters.Clear();
                }
                return _counter;
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connect.Close();
            }

        }
        protected static int insertYEBDetails(DataTable dt, string yrmo, string pind, string src, string typecd)
            //function to insert the LOA and Actives rocords into YEb Detail table
        {
            SqlCommand command = null;
            string connStr = ConfigurationManager.ConnectionStrings["EBADB"].ConnectionString;
            SqlConnection connect = new SqlConnection(connStr);
            int _counter = 0;
            command = new SqlCommand("sp_YEB_InsertYEBDetail", connect);
            command.CommandType = CommandType.StoredProcedure;
            if (connect != null && connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
              try
                    {
                        //loop thru the datarow columns for inserting to YEBdetail table
                        foreach (DataRow dr in dt.Rows)
                        {
                            command.Parameters.AddWithValue("@yrmo", yrmo);
                            command.Parameters.AddWithValue("@pilotind", pind);
                            //If the Yebdetaildata is being imported from source files then 
                            //@rectype param is set to IMP
                            command.Parameters.AddWithValue("@Rectype", "IMP");
                            command.Parameters.AddWithValue("@SourceCd", src);
                            command.Parameters.AddWithValue("@TypeCd", typecd);
                            if (typecd == "EBAACT")
                            {
                                //change request by legal dept - Andrea D 5/15/2009
                                //prefix the name with "household of"
                                command.Parameters.AddWithValue("@name", (dr["fname"].ToString().Trim().ToUpper() + ' ' + dr["lname"].ToString().Trim().ToUpper()));
                                //command.Parameters.AddWithValue("@name", "HOUSEHOLD OF " + (dr["lname"].ToString().Trim().ToUpper()));
                            }
                            else
                            {
                                command.Parameters.AddWithValue("@name", dr["fname"].ToString().Trim().ToUpper() + ' ' + (dr["lname"].ToString().Trim().ToUpper()));
                            }
                            if (dr["lname"] == DBNull.Value)
                            {
                                command.Parameters.AddWithValue("@lastname", DBNull.Value);
                            }
                            else
                            {
                                command.Parameters.AddWithValue("@lastname", dr["lname"].ToString().ToUpper());
                            }
                            command.Parameters.AddWithValue("@ssno", dr["ssn"].ToString().Trim());
                            command.Parameters.AddWithValue("@addr1", dr["addr1"].ToString().Trim().ToUpper());
                            command.Parameters.AddWithValue("@addr2", dr["addr2"].ToString().Trim().ToUpper());
                            command.Parameters.AddWithValue("@city", dr["city"].ToString().Trim().ToUpper());
                            command.Parameters.AddWithValue("@state", dr["state"].ToString().Trim().ToUpper());
                            command.Parameters.AddWithValue("@zipcode", dr["zip"].ToString().Trim());
                            if (dr["eeid"] != null && dr["eeid"].ToString().Length > 1)
                            {
                                command.Parameters.AddWithValue("@eeno", Convert.ToInt32(dr["eeid"].ToString()));
                            }
                            else
                            {
                                command.Parameters.AddWithValue("@eeno", 0);
                            }
                                           
                                command.Parameters.AddWithValue("@yebonline", false);
                                               
                                command.Parameters.AddWithValue("@expatflag", false);
                           
                            //execute the query
                            command.ExecuteNonQuery();
                            //increment the rowcounter
                            _counter++;
                            command.Parameters.Clear();
                        }
                        return _counter;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        connect.Close();
                    }
            }

        protected static void insertYEBDetails(DataRow[] drfoundrows, string yrmo, string pind, string src, string typecd)
        {
            SqlCommand command = null;
            string connStr = ConfigurationManager.ConnectionStrings["EBADB"].ConnectionString;
            SqlConnection connect = new SqlConnection(connStr);
            int _counter = 0;
            command = new SqlCommand("sp_YEB_InsertYEBDetail", connect);
            command.CommandType = CommandType.StoredProcedure;
            if (connect != null && connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            switch (src)
            {
                case "EBAS_WW":
                    try
                    {
                        //loop thru the datarow columns for inserting to YEBdetail table
                        foreach (DataRow dr in drfoundrows)
                        {
                            command.Parameters.AddWithValue("@yrmo", yrmo);
                            command.Parameters.AddWithValue("@pilotind", pind);
                            //If the Yebdetaildata is being imported from source files then 
                            //@rectype param is set to IMP
                            command.Parameters.AddWithValue("@Rectype", "IMP");
                            command.Parameters.AddWithValue("@SourceCd", src);
                            command.Parameters.AddWithValue("@TypeCd", typecd);
                            command.Parameters.AddWithValue("@name", dr["fname"].ToString().Trim().ToUpper() + ' ' + (dr["lname"].ToString().Trim().ToUpper()));
                            if (dr["lname"] == DBNull.Value)
                            {
                                command.Parameters.AddWithValue("@lastname", DBNull.Value);
                            }
                            else
                            {
                                command.Parameters.AddWithValue("@lastname", dr["lname"].ToString().ToUpper());
                            }
                            command.Parameters.AddWithValue("@ssno", dr["ssn"].ToString().Trim());
                            command.Parameters.AddWithValue("@addr1", dr["addr1"].ToString().Trim().ToUpper());
                            command.Parameters.AddWithValue("@addr2", dr["addr2"].ToString().Trim().ToUpper());
                            command.Parameters.AddWithValue("@city", dr["city"].ToString().Trim().ToUpper());
                            command.Parameters.AddWithValue("@state", dr["state"].ToString().Trim().ToUpper());
                            command.Parameters.AddWithValue("@zipcode", dr["zip"].ToString().Trim());
                            if (dr["eeid"] != null && dr["eeid"].ToString().Length > 1)
                            {
                                command.Parameters.AddWithValue("@eeno", Convert.ToInt32(dr["eeid"].ToString()));
                            }
                            else
                            {
                                command.Parameters.AddWithValue("@eeno", 0);
                            }
                                           
                                command.Parameters.AddWithValue("@yebonline", false);
                                               
                                command.Parameters.AddWithValue("@expatflag", false);
                           
                            //execute the query
                            command.ExecuteNonQuery();
                            //increment the rowcounter
                            _counter++;
                            command.Parameters.Clear();
                        }
                        break;

                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        connect.Close();
                    }
                    //return _counter;
                    //break;
                case "CobraNP":
                    try
                    {
                        //loop thru the datarow columns for inserting to YEBdetail table
                        foreach (DataRow dr in drfoundrows)
                        {
                            command.Parameters.AddWithValue("@yrmo", yrmo);
                            command.Parameters.AddWithValue("@pilotind", pind);
                            //If the Yebdetaildata is being imported from source files then 
                            //@rectype param is set to IMP
                            command.Parameters.AddWithValue("@Rectype", "IMP");
                            command.Parameters.AddWithValue("@SourceCd", src);
                            command.Parameters.AddWithValue("@TypeCd", typecd);
                            command.Parameters.AddWithValue("@name", dr["name"].ToString());
                            command.Parameters.AddWithValue("@ssno", dr["ssno"].ToString());
                            command.Parameters.AddWithValue("@addr1", dr["addr1"].ToString());
                            command.Parameters.AddWithValue("@addr2", dr["addr2"].ToString());
                            command.Parameters.AddWithValue("@city", dr["city"].ToString());
                            command.Parameters.AddWithValue("@state", dr["state"].ToString());
                            command.Parameters.AddWithValue("@zipcode", dr["zipcode"].ToString());
                            if (dr["eeno"] != null && dr["eeno"].ToString().Length > 1)
                            {
                                command.Parameters.AddWithValue("@eeno", Convert.ToInt32(dr["eeno"].ToString()));
                            }
                            else
                            {
                                command.Parameters.AddWithValue("@eeno", 0);
                            }
                            if (dr["yebonline"] != null && Convert.ToBoolean(dr["yebonline"].ToString()) == false)
                            {
                                command.Parameters.AddWithValue("@yebonline", false);
                            }
                            else if (dr["yebonline"] != null && Convert.ToBoolean(dr["yebonline"].ToString()) == true)
                            {
                                command.Parameters.AddWithValue("@yebonline", true);
                            }
                            else
                            {
                                command.Parameters.AddWithValue("@yebonline", false);
                            }

                            if (dr["expatflag"] != null && Convert.ToBoolean(dr["expatflag"].ToString()) == false)
                            {
                                command.Parameters.AddWithValue("@expatflag", false);
                            }
                            else if (dr["expatflag"] != null && Convert.ToBoolean(dr["expatflag"].ToString()) == true)
                            {
                                command.Parameters.AddWithValue("@expatflag", true);
                            }
                            else
                            {
                                command.Parameters.AddWithValue("@expatflag", false);
                            }
                            //execute the query
                            command.ExecuteNonQuery();
                            //increment the rowcounter
                            _counter++;
                            command.Parameters.Clear();
                        }
                        //return _counter;
                        break;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        connect.Close();
                    }
                    //return _counter;
                    //break;
                default:
                    break;

            }

        }

        protected static void insertYEB_DupDetails(DataRow dr)
        {
            SqlCommand command = null;
            string connStr = ConfigurationManager.ConnectionStrings["EBADB"].ConnectionString;
            SqlConnection connect = new SqlConnection(connStr);
            //var to store the type of record
            // Duplicate (Dup) or (Dis) Distribution
            string RecordType = "";  
            //int _counter = 0;
            command = new SqlCommand("sp_YEB_InsertYEB_DupDetail", connect);
            command.CommandType = CommandType.StoredProcedure;
            if (connect != null && connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            try
            {
                //loop thru the datatable rows for inserting to YEB_Dupdetail table
                //foreach (DataRow dr in dt.Rows)
                {
                    command.Parameters.AddWithValue("@yrmo", dr["yrmo"].ToString());
                    command.Parameters.AddWithValue("@pilotind", dr["pilotind"].ToString());
                    command.Parameters.AddWithValue("@RecType", "COB");
                    command.Parameters.AddWithValue("@SourceCd", dr["sourceCd"].ToString());
                    command.Parameters.AddWithValue("@TypeCd", dr["TypeCd"].ToString());
                    command.Parameters.AddWithValue("@name", dr["name"].ToString());
                    command.Parameters.AddWithValue("@ssno", dr["ssno"].ToString());
                    command.Parameters.AddWithValue("@addr1", dr["addr1"].ToString());
                    command.Parameters.AddWithValue("@addr2", dr["addr2"].ToString());
                    command.Parameters.AddWithValue("@city", dr["city"].ToString());
                    command.Parameters.AddWithValue("@state", dr["state"].ToString());
                    command.Parameters.AddWithValue("@zipcode", dr["zipcode"].ToString());
                    if (dr["eeno"] != null && dr["eeno"].ToString().Length>1)
                    {
                        command.Parameters.AddWithValue("@eeno", Convert.ToInt32(dr["eeno"].ToString()));
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@eeno", 0);
                    }
                    if (dr["yebonline"] != null && Convert.ToBoolean(dr["yebonline"].ToString())==false)
                    {
                        command.Parameters.AddWithValue("@yebonline", 0);
                    }
                    else if (dr["yebonline"] != null && Convert.ToBoolean(dr["yebonline"].ToString()) == true)
                    {
                        command.Parameters.AddWithValue("@yebonline", 1);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@yebonline", 0);
                    }
                    if (dr["expatflag"] != null && Convert.ToBoolean(dr["expatflag"].ToString())== false)
                    {
                        command.Parameters.AddWithValue("@expatflag", 0);
                    }
                    else if (dr["expatflag"] != null && Convert.ToBoolean(dr["expatflag"].ToString()) == true)
                    {
                        command.Parameters.AddWithValue("@expatflag", 1);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@expatflag", false);
                    }
                    
                    //execute the query
                    command.ExecuteNonQuery();
                    //increment the rowcounter
                   // _counter++;
                }
                //return _counter;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connect.Close();
            }

        }
        protected static int insertYEB_OL_Imports(DataTable dt)
            //insert the YEB Online imported data 
        {
            SqlCommand command = null;
            string connStr = ConfigurationManager.ConnectionStrings["EBADB"].ConnectionString;
            SqlConnection connect = new SqlConnection(connStr);
            int _counter = 0;
            command = new SqlCommand("sp_YEB_InsertYEBOnline", connect);
            command.CommandType = CommandType.StoredProcedure;
            if (connect != null && connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            try
            {
                //loop thru the datatable rows for inserting to YEBOnline table
                foreach (DataRow dr in dt.Rows)
                {
                    command.Parameters.AddWithValue("@type", dr["type"].ToString());
                    command.Parameters.AddWithValue("@yebelectronicdate", Convert.ToDateTime(dr["yebelectronicdate"].ToString()));
                    command.Parameters.AddWithValue("@emplstatus", dr["emplstatus"].ToString());
                    command.Parameters.AddWithValue("@ssn", dr["ssn"].ToString());
                    if (dr["eeid"] != null && dr["eeid"].ToString().Length > 1)
                    {
                        command.Parameters.AddWithValue("@eeid", Convert.ToInt32(dr["eeid"].ToString()));
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@eeid", 0);
                    }
                    if (dr["yebelectronic"] != null && dr["yebelectronic"].ToString()=="Y")
                    {
                        command.Parameters.AddWithValue("@yebelectronic", true);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@yebelectronic", false);
                    }

                    //execute the query
                    command.ExecuteNonQuery();
                    //increment the rowcounter
                    _counter++;
                    command.Parameters.Clear();
                }
                return _counter;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connect.Close();
            }

        }

        protected static int insertYEB_Expat_Imports(DataTable dt)
        //insert the YEB Expat Group imported data 
        {
            SqlCommand command = null;
            string connStr = ConfigurationManager.ConnectionStrings["EBADB"].ConnectionString;
            SqlConnection connect = new SqlConnection(connStr);
            int _counter = 0;
            command = new SqlCommand("sp_YEB_InsertWWExpat", connect);
            command.CommandType = CommandType.StoredProcedure;
            if (connect != null && connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            try
            {
                //loop thru the datatable rows for inserting to YEBOnline table
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["ssn"].ToString().Trim() == "")
                    {
                        continue;
                    }
                    command.Parameters.AddWithValue("@ssn", dr["ssn"].ToString());
                    if (dr["eeid"] != null && dr["eeid"].ToString().Length > 1)
                    {
                        command.Parameters.AddWithValue("@eeid", Convert.ToInt32(dr["eeid"].ToString()));
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@eeid", 0);
                    }
                    if (dr["expatflag"] != null && dr["expatflag"].ToString()=="True")
                    {
                        command.Parameters.AddWithValue("@expatflag", true);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@expatflag", false);
                    }

                    //execute the query
                    command.ExecuteNonQuery();
                    //increment the rowcounter
                    _counter++;
                    command.Parameters.Clear();
                }
                return _counter;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connect.Close();
            }

        }

        protected static int insertYEB_DupDetails(DataTable dt, string keycol)
        {
            SqlCommand command = null;
            string cur_row_keycolvalue = "";
            string prev_row_keycolvalue = "";
            Int64 cur_row_id = 0;
            //var to store the type of record
            // Duplicate (Dup) or (Dis) Distribution
            string RecordType = "";  
            //SqlTransaction trs;
            //Name to stored in the Distribution
            //Record if it is a home mailing record
            string Dis_Name = "";
            DataRow[] foundrows;
            string connStr = ConfigurationManager.ConnectionStrings["EBADB"].ConnectionString;
            SqlConnection connect = new SqlConnection(connStr);
            int _counter = 0;
            command = new SqlCommand("sp_YEB_InsertYEB_DupDetail", connect);
            command.CommandType = CommandType.StoredProcedure;
            if (connect != null && connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            //trs = connect.BeginTransaction();
            try
            {
                //loop thru the datatable rows for inserting to SSN duplicates to YEB_Dupdetail table
                foreach (DataRow dr in dt.Rows)
                {
                    //cur_row_keycolvalue = ImportYEBData.ParseSingleQuoteString(dr[0].ToString());
                    cur_row_keycolvalue = escapeSingleQuoteChar(dr[0].ToString());
                    cur_row_id = Int64.Parse(dr["rid"].ToString());
                    //now insert each of the found rows for this primary key value into the YEB-DUp detail table
                    if (keycol == "ADDR1" && cur_row_keycolvalue != prev_row_keycolvalue)
                    {
                        string cur_city = "";
                        //cur_city = ImportYEBData.ParseSingleQuoteString(dr["city"].ToString());
                        cur_city = escapeSingleQuoteChar(dr["city"].ToString());
                        foundrows=dt.Select("addr1=" + "'" + cur_row_keycolvalue + "'" + " and city = " + "'" + cur_city + "'");
                        if (foundrows.Length == 1)
                        {
                            prev_row_keycolvalue = cur_row_keycolvalue;
                            continue;
                        }
                    }

                    if (keycol == "SSNO" && cur_row_keycolvalue != prev_row_keycolvalue)
                    {
                        foundrows = dt.Select("ssno=" + "'" + cur_row_keycolvalue + "'");
                        if (foundrows.Length == 1)
                        {
                            prev_row_keycolvalue = cur_row_keycolvalue;
                            continue;
                        }
                    }

                  
                    {
                        if (cur_row_keycolvalue != prev_row_keycolvalue)
                        {
                            // if cur_row_keycolvalue != prev_row_keycolvalue then the priority index is the max and 
                            // this record will be kept for  YEB disrtribution
                           
                            RecordType = "DIS" + "_" + keycol;
                            //Check to see if the typecd != "EBAACT"
                            // for a home mailing record
                            if (dr["typecd"].ToString() != "EBAACT")
                            {
                                Dis_Name = dr["name"].ToString();

                                string t19 = Dis_Name;
                                string p19 = @"^\S+\s+(\S)\S*\s+\S";
                                Match m19 = Regex.Match(t19, p19);
                                                    

                                string[] split = Dis_Name.Split(new Char[] { ' '});
                                int len = split.Length;
                                if (len > 2 && m19.Success )
                                {
                                    Dis_Name = "";
                                    for (len = 2; len < split.Length; len++)
                                    {
                                        if (len < split.Length - 1)
                                        {
                                            Dis_Name = Dis_Name + split[len] + " ";
                                        }
                                        else if (len == split.Length - 1)
                                        {
                                             Dis_Name = Dis_Name + split[len];
                                        }
                                     

                                    }
                                    Dis_Name = "HOUSEHOLD OF " + Dis_Name;
                                }
                                if (len == 2)
                                {

                                    Dis_Name = "HOUSEHOLD OF " + split[1];
                                }
                                if (len > 2 && !m19.Success)
                                {
                                    Dis_Name = "";
                                    for (len = 1; len < split.Length; len++)
                                    {
                                        Dis_Name = Dis_Name + split[len];
                                    }
                                    Dis_Name = "HOUSEHOLD OF " + Dis_Name;
                                }
                            }
                            else
                            {
                                Dis_Name = dr["name"].ToString();
                            }
                            //Now Update the Recordtype for this YEBdetail record 
                            //and marck it as DIS for Distribution

                            YEBDetail_UpdateRecordType(dr["yrmo"].ToString(), RecordType, Int64.Parse(dr["rid"].ToString()));
                            //increment the rowcounter
                            //Only when a record for YEB Distribution is created
                            _counter++;
                            //now assign the rows primary key value to the prev row key value 
                            //for comparing with the next row in the loop
                            prev_row_keycolvalue = cur_row_keycolvalue; 
                            //Now continue to the next row 
                            continue;
                        }
                        else if (cur_row_keycolvalue == prev_row_keycolvalue)
                        // if _cur_row_keycolvalue == prev_row_keycolvalue then the priority index is not the max and 
                        // this record will NOT be kept for  YEB disrtribution and marked as DUP
                        {
                            RecordType = "DUP" + "_" + keycol;
                            Dis_Name = dr["name"].ToString();
                            YEBDetail_DeleteDups(dr["yrmo"].ToString(), Int64.Parse(dr["rid"].ToString()));

                        }
                    
                        command.Parameters.AddWithValue("@yrmo", dr["yrmo"].ToString());
                        command.Parameters.AddWithValue("@pilotind", dr["pilotind"].ToString());
                        command.Parameters.AddWithValue("@RecType", RecordType);
                        command.Parameters.AddWithValue("@SourceCd", dr["sourceCd"].ToString());
                        command.Parameters.AddWithValue("@TypeCd", dr["TypeCd"].ToString());
                        command.Parameters.AddWithValue("@name", Dis_Name);
                        command.Parameters.AddWithValue("@ssno", dr["ssno"].ToString());
                        command.Parameters.AddWithValue("@addr1", dr["addr1"].ToString());
                        command.Parameters.AddWithValue("@addr2", dr["addr2"].ToString());
                        command.Parameters.AddWithValue("@city", dr["city"].ToString());
                        command.Parameters.AddWithValue("@state", dr["state"].ToString());
                        command.Parameters.AddWithValue("@zipcode", dr["zipcode"].ToString());
                        if (dr["eeno"] != null && dr["eeno"].ToString().Length > 1)
                        {
                            command.Parameters.AddWithValue("@eeno", Convert.ToInt32(dr["eeno"].ToString()));
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@eeno", 0);
                        }
                        if (dr["yebonline"] != null && Convert.ToBoolean(dr["yebonline"].ToString()) == false)
                        {
                            command.Parameters.AddWithValue("@yebonline", 0);
                        }
                        else if (dr["yebonline"] != null && Convert.ToBoolean(dr["yebonline"].ToString()) == true)
                        {
                            command.Parameters.AddWithValue("@yebonline", 1);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@yebonline", 0);
                        }
                        if (dr["expatflag"] != null && Convert.ToBoolean(dr["expatflag"].ToString()) == false)
                        {
                            command.Parameters.AddWithValue("@expatflag", 0);
                        }
                        else if (dr["expatflag"] != null && Convert.ToBoolean(dr["expatflag"].ToString()) == true)
                        {
                            command.Parameters.AddWithValue("@expatflag", 1);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@expatflag", false);
                        }

                        //execute the query
                        command.ExecuteNonQuery();
                        command.Parameters.Clear();
                       
                         //now assign the rows primary key value to the prev row key value 
                        //for comparing with the next row in the loop

                        prev_row_keycolvalue = cur_row_keycolvalue; // dr[0].ToString();
                        
                        
                    }                    
                  }
                //trs.Commit();
                return _counter;

            }
            catch (Exception ex)
            {
                throw ex;
                //trs.Rollback();
            }
            finally
            {
                connect.Close();
            }

        }


        public static void Rollback(string source, string period)
        {
            string cmdStr1 = "";
            string cmdStr2 = "DELETE FROM ImportRecon_status WHERE period=@period AND source=@source AND type='Import' AND module='YEB'";

            try
            {
                
                switch (source)
                {
                    case "COBNP":
                        cmdStr1 = "DELETE FROM YebDetail WHERE yrmo = @period and sourcecd = @source";
                        break;
                    case "COBPI":
                        cmdStr1 = "DELETE FROM YebDetail WHERE yrmo = @period and sourcecd = @source";
                        break;
                    case "COB":
                        cmdStr1 = "DELETE FROM YEB_Dup_Detail WHERE yrmo = @period and rectype = 'COB'";
                        break;
                    case "EBAS_WWNP":
                        cmdStr1 = "DELETE FROM YebDetail WHERE yrmo = @period and sourcecd= left(@source,7) and pilotind= 'NP'";
                        break;
                    case "EBAS_WWPI":
                        cmdStr1 = "DELETE FROM YebDetail WHERE yrmo = @period and sourcecd= left(@source,7) and pilotind= 'PI'";
                        break;
                    case "LOANP":
                        cmdStr1 = "DELETE FROM YebDetail WHERE yrmo = @period and typecd= 'EBALOA' and pilotind = 'NP'";
                        break;
                    case "LOAPI":
                        cmdStr1 = "DELETE FROM YebDetail WHERE yrmo = @period and typecd= 'EBALOA' and pilotind = 'PI'";
                        break;
                    case "ACTNP":
                        cmdStr1 = "DELETE FROM YebDetail WHERE yrmo = @period and typecd= 'EBAACT' and pilotind = 'NP'";
                        break;
                    case "ACTPI":
                        cmdStr1 = "DELETE FROM YebDetail WHERE yrmo = @period and typecd= 'EBAACT' and pilotind = 'PI'";
                        break;
                    case "YEB_EXPAT":
                        cmdStr1 = "DELETE FROM wwexpat";
                        break;
                    case "YEB_OL":
                        cmdStr1 = "DELETE FROM Yeb_online";
                        break;
                    case "DUPSSNNP":
                        cmdStr1 = "DELETE FROM YEB_Dup_Detail WHERE yrmo = @period and right(RecType,4) = 'SSNO' and Pilotind = right(@source,2)";
                        break;
                    case "DUPSSNPI":
                        cmdStr1 = "DELETE FROM YEB_Dup_Detail WHERE yrmo = @period and right(RecType,4) = 'SSNO' and Pilotind = right(@source,2)";
                        break;
                    case "DUPADDRNP":
                        cmdStr1 = "DELETE FROM YEB_Dup_Detail WHERE yrmo = @period and right(RecType,5) = 'ADDR1' and Pilotind = right(@source,2)";
                        break;
                    case "DUPADDRPI":
                        cmdStr1 = "DELETE FROM YEB_Dup_Detail WHERE yrmo = @period and right(RecType,5) = 'ADDR1' and Pilotind = right(@source,2)";
                        break;
                }

                if (connect != null && connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdStr1, connect);
                command.Parameters.AddWithValue("@period", period);
                command.Parameters.AddWithValue("@source", source);
                command.ExecuteNonQuery();
                command.Dispose();
                command = new SqlCommand(cmdStr2, connect);
                command.Parameters.AddWithValue("@period", period);
                command.Parameters.AddWithValue("@source", source);
                command.ExecuteNonQuery();
                command.Dispose();
            }
            finally
            {
                connect.Close();
            }
        }
        public static void YEBDetail_DeleteDups(string yrmo, Int64 rowid)
        {
            string cmdStr1 = "DELETE FROM YEBDETAIL WHERE yrmo = " + "'" + yrmo + "'" + " AND rid = " + rowid ;
            //delete this row from the yebdetailtable based on the unique rowid passed
            
            try
            {
                if (connect != null && connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdStr1, connect);
                command.ExecuteNonQuery();
                command.Dispose();
                
            }
            finally
            {
                connect.Close();
            }
        }
        public static void YEBDetail_UpdateRecordType(string yrmo, string RecordType, Int64 rowid)
        {
            //Update the yebdetail RecType Field to marck it as DIS (for Distribution)
            // when the highest pririty record is found
            string cmdStr1 = "UPDATE YEBDETAIL " +
                             "Set Rectype = " + "'" + RecordType + "'" + 
                             " WHERE yrmo = " + "'" + yrmo + "'" + " AND rid = " + rowid;
            try
            {
                if (connect != null && connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdStr1, connect);
                command.ExecuteNonQuery();
                command.Dispose();

            }
            finally
            {
                connect.Close();
            }
        }

        public static string getEmployeeLastname(string Dis_Name)
        {
            string t19 = Dis_Name;
            string p19 = @"^\S+\s+(\S)\S*\s+\S";
            Match m19 = Regex.Match(t19, p19);


            string[] split = Dis_Name.Split(new Char[] { ' ' });
            int len = split.Length;
            if (len > 2 && m19.Success)
            {
                Dis_Name = "";
                for (len = 2; len < split.Length; len++)
                {
                    if (len < split.Length - 1)
                    {
                        Dis_Name = Dis_Name + split[len] + " ";
                    }
                    else if (len == split.Length - 1)
                    {
                        Dis_Name = Dis_Name + split[len];
                    }

                }
                Dis_Name = Dis_Name;
            }
            if (len == 2)
            {

                Dis_Name =  split[1];
            }
            if (len > 2 && !m19.Success)
            {
                Dis_Name = "";
                for (len = 1; len < split.Length; len++)
                {
                    Dis_Name = Dis_Name + split[len];
                }
                Dis_Name = Dis_Name;
            }
            return Dis_Name;
            
        }

        public static void insertImportStatus(string yrmo, string source)
        {
            string cmdStr = "INSERT INTO ImportRecon_status(period,source,type,module) VALUES(@yrmo, @source, 'Import', 'YEB')";

            if (connect == null || connect.State == ConnectionState.Closed)
            {
                connect.Open();
            }
            try
            {
                command = new SqlCommand(cmdStr, connect);
                command.Parameters.AddWithValue("@yrmo", yrmo);
                command.Parameters.AddWithValue("@source", source);
                command.ExecuteNonQuery();
            }
            finally
            {
                connect.Close();
            }
        }
        public static Boolean PastImport(string source, string period)
        {
            string cmdStr = "SELECT COUNT(*) FROM ImportRecon_status WHERE period=@period AND source=@source AND type='Import' AND module='YEB'";
            int count = 0;

            try
            {
                if (connect != null && connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdStr, connect);
                command.Parameters.AddWithValue("@period", period);
                command.Parameters.AddWithValue("@source", source);
                count = Convert.ToInt32(command.ExecuteScalar());
                if (count == 0)
                    return false;
                else
                    return true;
            }
            finally
            {
                connect.Close();
            }
        }

        

        List<int> getColsIndices(DataSet ds, String _tableName, String[] colsH)
        {
            List<int> colsIndices = new List<int>();
            int lastCol = ds.Tables[_tableName].Columns.Count;
            DataRow dr = null;
            string colname;

            dr = ds.Tables[_tableName].Rows[0];
            foreach (String colH in colsH)
            {
                for (int j = 0; j < lastCol; j++)
                {
                    colname = ds.Tables[_tableName].Columns[j].ColumnName.ToString().Trim();

                    if (String.Compare(colname, colH, true) == 0)
                    {
                        colsIndices.Add(j);
                        break;
                    }
                }
            }

            if (colsIndices.Count < colsH.Length)
            {
                throw new Exception("Cannot find required columns in the report.<br >Check format of the report");
            }

            return colsIndices;
        }
        private static string ParseSingleQuoteString(string input)
        {
            //escape single quote with double quotes
            //for string input
            string ret = "";
            if (input.Contains("'"))
            {
                string[] inputstrs = input.Split('\'');
                foreach (string inputstr in inputstrs)
                {
                    if (ret != "")
                        ret += ",\"'\",";
                    ret += "\"" + inputstr + "\"";
                }
                ret = "concat(" + ret + ")";
            }
            else
            {
                //ret = "'" + input + "'";
                ret = input;
            }
            return ret;
        }
        private static string escapeSingleQuoteChar(string strToEsc)
        {
            if (strToEsc.IndexOf("'") > -1)
            {
                strToEsc = strToEsc.Replace("'","''"); // notice the addition of the @ symbol
                //Replace single quote with 2 single quotes ("'","''") 
            }
            return strToEsc;
        }

        

        
        public static string GetProperCase(string strName)
        {
            string strproper = "";

            if (strName != null && strName.Length > 1)
            {
                strproper = strName.Substring(0, 1).ToUpper() + strName.Substring(1).ToLower();
            }

            return strproper;
        }

        public static string GetFixedLengthString(string input, int length)
        {
            string result = string.Empty;

            if (string.IsNullOrEmpty(input))
            {
                result = new string(' ', length);
            }
            else if (input.Length > length)
            {
                result = input.Substring(0, length);
            }
            else
            {
                result = input.PadRight(length);
            }

            return result;
        }

        public static DataSet ProcessReportData(string source, string period, string pilotind)
        {
            string cmdStr1 = "";
            DataSet dsbresult = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            try
            {

                switch (source)
                {
                    case "HOME_YEB_RET":
                        cmdStr1 = "SELECT YRMO, PilotInd, SourceCd, TypeCd, 'HOUSEHOLD OF ' + lastName as Name, SSNO, Addr1, Addr2, City, State, ZipCode, EENO, YEBOnline, ExpatFlag FROM dbo.YEBDetail WHERE yrmo = @period and TypeCd IN ('EBARET') and YEBOnline =0 and pilotind = " + "'" + pilotind + "'";
                        break;
                    case "HOME_YEB_NONRET":
                        cmdStr1 = "SELECT YRMO, PilotInd, SourceCd, TypeCd, 'HOUSEHOLD OF ' + lastName as Name, SSNO, Addr1, Addr2, City, State, ZipCode, EENO, YEBOnline, ExpatFlag FROM dbo.YEBDetail WHERE yrmo = @period and TypeCd IN ('EBACOB','EBALOA') and pilotind = " + "'" + pilotind + "'";
                        break;
                    case "HOME_SAR_ONLY":
                        cmdStr1 = "SELECT YRMO, PilotInd, SourceCd, TypeCd, 'HOUSEHOLD OF ' + lastName as Name, SSNO, Addr1, Addr2, City, State, ZipCode, EENO, YEBOnline, ExpatFlag FROM dbo.YEBDetail WHERE yrmo = @period and TypeCd IN ('EBARET') and YEBOnline=1 and pilotind = " + "'" + pilotind + "'";
                        break;
                    case "Active_YEB_Group1":
                        cmdStr1 = "SELECT YRMO, PilotInd, SourceCd, TypeCd, Name, SSNO, Addr1, Addr2, City, State, ZipCode, EENO, YEBOnline, ExpatFlag FROM dbo.YEBDetail WHERE yrmo = @period and TypeCd IN ('EBAACT') and YEBOnline =0 and Expatflag=0 and eeno<=458323 and State <> 'PR' and pilotind = " + "'" + pilotind + "'";
                        //cmdStr1 = "SELECT YRMO, PilotInd, SourceCd, TypeCd, 'HOUSEHOLD OF ' + lastName as Name, SSNO, Addr1, Addr2, City, State, ZipCode, EENO, YEBOnline, ExpatFlag FROM dbo.YEBDetail WHERE yrmo = @period and TypeCd IN ('EBAACT') and YEBOnline =0 and Expatflag=0 and State <> 'PR' and eeno>=458321 and eeno <=458322";
                        break;
                    case "Active_YEB_Group2": 
                        //cmdStr1 = "SELECT YRMO, PilotInd, SourceCd, TypeCd, 'HOUSEHOLD OF ' + lastName as Name, SSNO, ISNULL(Addr1,''), isnull(Addr2,''), isnull(City,''), isnull(State,''), isnull(ZipCode,''), isnull(EENO,0), YEBOnline, ExpatFlag FROM dbo.YEBDetail WHERE yrmo = @period and TypeCd IN ('EBAACT') and YEBOnline =0 and Expatflag=0 and State <> 'PR' and eeno>=444001";
                        cmdStr1 = "SELECT YRMO, PilotInd, SourceCd, TypeCd, Name, SSNO, Addr1, Addr2, City, State, ZipCode, EENO, YEBOnline, ExpatFlag FROM dbo.YEBDetail WHERE yrmo = @period and TypeCd IN ('EBAACT') and YEBOnline =0 and Expatflag=0 and eeno>=458325 and State <> 'PR' and pilotind = " + "'" + pilotind + "'";
                        break;
                    case "Active_SAROnly":
                        cmdStr1 = "SELECT YRMO, PilotInd, SourceCd, TypeCd, Name, SSNO, Addr1, Addr2, City, State, ZipCode, EENO, YEBOnline, ExpatFlag FROM dbo.YEBDetail WHERE yrmo = @period and TypeCd IN ('EBAACT') and YEBOnline =1 and Expatflag=0 and pilotind = " + "'" + pilotind + "'";
                        //cmdStr1 = "SELECT YRMO, PilotInd, SourceCd, TypeCd, 'HOUSEHOLD OF ' + lastName as Name, SSNO, Addr1, Addr2, City, State, ZipCode, EENO, YEBOnline, ExpatFlag FROM dbo.YEBDetail WHERE yrmo = @period and TypeCd IN ('EBAACT') and YEBOnline =0 and Expatflag=0 and State <> 'PR' and eeno>=458323 and eeno <=458323";
                        break;
                    case "Active_PROnly":
                        cmdStr1 = "SELECT YRMO, PilotInd, SourceCd, TypeCd, Name, SSNO, Addr1, Addr2, City, State, ZipCode, EENO, YEBOnline, ExpatFlag FROM dbo.YEBDetail WHERE yrmo = @period and TypeCd IN ('EBAACT') and State ='PR' and YEBONLINE=0 and pilotind = " + "'" + pilotind + "'";
                        break;
                    case "DUP_ADDR1":
                        cmdStr1 = "SELECT Rectype, YRMO, PilotInd, SourceCd, TypeCd, Name, SSNO, Addr1, Addr2, City, State, ZipCode, EENO FROM Yeb_DUP_Detail WHERE yrmo = @period and rectype= 'DUP_ADDR1' and pilotind = " + "'" + pilotind + "'" + " UNION SELECT Rectype, YRMO, PilotInd, SourceCd, TypeCd, Name, SSNO, Addr1, Addr2, City, State, ZipCode, EENO FROM YebDetail WHERE yrmo = @period and rectype= 'DIS_ADDR1' and pilotind = " + "'" + pilotind + "'";
                        break;
                    case "DUP_SSN":
                        cmdStr1 = "SELECT Rectype, YRMO, PilotInd, SourceCd, TypeCd, Name, SSNO, Addr1, Addr2, City, State, ZipCode, EENO FROM Yeb_DUP_Detail WHERE yrmo = @period and rectype= 'DUP_SSNO' and pilotind = " + "'" + pilotind + "'" + " UNION SELECT Rectype, YRMO, PilotInd, SourceCd, TypeCd, Name, SSNO, Addr1, Addr2, City, State, ZipCode, EENO FROM YebDetail WHERE yrmo = @period and rectype= 'DIS_SSNO' and pilotind = " + "'" + pilotind + "'";
                        break;
                   
                }

                if (connect != null && connect.State == ConnectionState.Closed)
                {
                    connect.Open();
                }
                command = new SqlCommand(cmdStr1, connect);
                command.Parameters.AddWithValue("@period", period);
                command.Parameters.AddWithValue("@source", source);
                //command.ExecuteNonQuery();
                da.SelectCommand = command;
                da.Fill(dsbresult);
             }
            finally
            {
                connect.Close();
            }
            return dsbresult;
        }
        public static void PrintOLProgressBar()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<div id='updiv' style='Font-weight:bold;font-size:11pt;Left:320px;COLOR:black;font-family:verdana;Position:absolute;Top:140px;Text-Align:center;'>");
            sb.Append("&nbsp;<script> var up_div=document.getElementById('updiv');up_div.innerText='';</script>");
            sb.Append("<script language=javascript>");
            sb.Append("var dts=0; var dtmax=10;");
            sb.Append("function ShowWait(){var output;output='YEB Online Source File records are being imported to the DB!';dts++;if(dts>=dtmax)dts=1;");
            //sb.AppendFormat("var rcount = {0};\n", count);
            //sb.AppendFormat("function ShowWait(){var output;output= {0};", message);
            //sb.AppendFormat("dts++;if(dts>=dtmax)dts=1");
            sb.Append("for(var x=0;x < dts; x++){output+='';}up_div.innerText=output;up_div.style.color='green';}");
            sb.Append("function StartShowWait(){up_div.style.visibility='visible';ShowWait();window.setInterval('ShowWait()',100);}");
            sb.Append("StartShowWait();</script>");
            HttpContext.Current.Response.Write(sb.ToString());
            HttpContext.Current.Response.Flush();
        }
        //Javascript function to clear progressbar
        public static void ClearProgressBar(int count)
        {
            StringBuilder sbc = new StringBuilder();
            sbc.Append("<script language='javascript'>");
            sbc.AppendFormat("var rcount = {0};\n", count);
            sbc.Append("alert('Import process completed successfully! Imported Recordcount = ' + rcount);");
            sbc.Append("up_div.style.visibility='hidden';");
            sbc.Append("history.go(-1)");
            sbc.Append("</script>");
            HttpContext.Current.Response.Write(sbc.ToString());
            //HttpContext.Current.Response.Flush();
        }

        public static void PrintExpatProgressBar()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<div id='updiv' style='Font-weight:bold;font-size:11pt;Left:320px;COLOR:black;font-family:verdana;Position:absolute;Top:140px;Text-Align:center;'>");
            sb.Append("&nbsp;<script> var up_div=document.getElementById('updiv');up_div.innerText='';</script>");
            sb.Append("<script language=javascript>");
            sb.Append("var dts=0; var dtmax=10;");
            sb.Append("function ShowWait(){var output;output='YEB Expat Group Source File records are being imported to the DB!';dts++;if(dts>=dtmax)dts=1;");
            //sb.AppendFormat("var rcount = {0};\n", count);
            //sb.AppendFormat("function ShowWait(){var output;output= {0};", message);
            //sb.AppendFormat("dts++;if(dts>=dtmax)dts=1");
            sb.Append("for(var x=0;x < dts; x++){output+='';}up_div.innerText=output;up_div.style.color='green';}");
            sb.Append("function StartShowWait(){up_div.style.visibility='visible';ShowWait();window.setInterval('ShowWait()',100);}");
            sb.Append("StartShowWait();</script>");
            HttpContext.Current.Response.Write(sb.ToString());
            HttpContext.Current.Response.Flush();
        }

        public static void PrintLOAProgressBar()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<div id='updiv' style='Font-weight:bold;font-size:11pt;Left:320px;COLOR:black;font-family:verdana;Position:absolute;Top:140px;Text-Align:center;'>");
            sb.Append("&nbsp;<script> var up_div=document.getElementById('updiv');up_div.innerText='';</script>");
            sb.Append("<script language=javascript>");
            sb.Append("var dts=0; var dtmax=10;");
            sb.Append("function ShowWait(){var output;output='YEB LOA emp records are being imported to the DB!';dts++;if(dts>=dtmax)dts=1;");
            //sb.AppendFormat("var rcount = {0};\n", count);
            //sb.AppendFormat("function ShowWait(){var output;output= {0};", message);
            //sb.AppendFormat("dts++;if(dts>=dtmax)dts=1");
            sb.Append("for(var x=0;x < dts; x++){output+='';}up_div.innerText=output;up_div.style.color='green';}");
            sb.Append("function StartShowWait(){up_div.style.visibility='visible';ShowWait();window.setInterval('ShowWait()',100);}");
            sb.Append("StartShowWait();</script>");
            HttpContext.Current.Response.Write(sb.ToString());
            HttpContext.Current.Response.Flush();
        }
        public static void PrintACTProgressBar()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<div id='updiv' style='Font-weight:bold;font-size:11pt;Left:320px;COLOR:black;font-family:verdana;Position:absolute;Top:140px;Text-Align:center;'>");
            sb.Append("&nbsp;<script> var up_div=document.getElementById('updiv');up_div.innerText='';</script>");
            sb.Append("<script language=javascript>");
            sb.Append("var dts=0; var dtmax=10;");
            sb.Append("function ShowWait(){var output;output='YEB ACT emp records are being imported to the DB!';dts++;if(dts>=dtmax)dts=1;");
            //sb.AppendFormat("var rcount = {0};\n", count);
            //sb.AppendFormat("function ShowWait(){var output;output= {0};", message);
            //sb.AppendFormat("dts++;if(dts>=dtmax)dts=1");
            sb.Append("for(var x=0;x < dts; x++){output+='';}up_div.innerText=output;up_div.style.color='green';}");
            sb.Append("function StartShowWait(){up_div.style.visibility='visible';ShowWait();window.setInterval('ShowWait()',100);}");
            sb.Append("StartShowWait();</script>");
            HttpContext.Current.Response.Write(sb.ToString());
            HttpContext.Current.Response.Flush();
        }
        public static void DisplayProgressBar()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<div id='updiv' style='Font-weight:bold;font-size:11pt;Left:320px;COLOR:black;font-family:verdana;Position:absolute;Top:140px;Text-Align:center;'>");
            sb.Append("&nbsp;<script> var up_div=document.getElementById('updiv');up_div.innerText='';</script>");
            sb.Append("<script language=javascript>");
            sb.Append("var dts=0; var dtmax=10;");
            sb.Append("function ShowWait(){var output;output='Processing Your Request!';dts++;if(dts>=dtmax)dts=1;");
            sb.Append("for(var x=0;x < dts; x++){output+='';}up_div.innerText=output;up_div.style.color='green';}");
            sb.Append("function StartShowWait(){up_div.style.visibility='visible';ShowWait();window.setInterval('ShowWait()',100);}");
            sb.Append("StartShowWait();</script>");
            HttpContext.Current.Response.Write(sb.ToString());
            HttpContext.Current.Response.Flush();
        }

        public static void HideProgressBar(int count)
        {
            StringBuilder sbc = new StringBuilder();
            sbc.Append("<script language='javascript'>");
            sbc.AppendFormat("var rcount = {0};\n", count);
            sbc.Append("alert('Duplicates Identified = ' + rcount);");
            sbc.Append("up_div.style.visibility='hidden';");
            sbc.Append("history.go(-1)");
            sbc.Append("</script>");
            HttpContext.Current.Response.Write(sbc.ToString());
            //HttpContext.Current.Response.Flush();
        }
    }





}