using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using EBA.Desktop.HRA;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Diagnostics;
using System.Net;
using System.Threading;

namespace EBA.Desktop.HRA
{
    public class HRAEligFile
    {
        private Process p;
        private string _errormsg;

        public HRAEligFile()
	    {		
	    }        

        public string GetEligFile()
        {
            DataSet empds = new DataSet();
            DataSet dshrainfo = new DataSet();
            StringBuilder sb = new StringBuilder();
            HRAOperDAL dobj = new HRAOperDAL();
            empds.Clear();
            dshrainfo.Clear();
            int counter = 0;
            string start, end;
            string hraPlanNum = HRAOperDAL.GetHRAPlanNum();
            

            /**************  Header of Elig File   ************************************/

            sb.Append(GetEligFileHeader(hraPlanNum) + Environment.NewLine);


            /**************  Build Records   ******************************************/
            
            empds = dobj.GetEmployeeInfo();

            foreach (DataRow row in empds.Tables[0].Rows)
            {
                //start and end of each line
                start = "813" + "  " + hraPlanNum.PadLeft(6, '0') + " " + row["ssn"].ToString().Trim().PadLeft(9, '0');
                end = "";
                end = end.PadRight(24, ' ') + Environment.NewLine;

                //participant info
                dshrainfo = getHRaPartInfo(row);

                foreach (DataRow row1 in dshrainfo.Tables[0].Rows)
                {
                    if (row1["codeid"].ToString() != "N/A" && row1["codeid"].ToString() != "RET")
                    {
                        sb.Append(start + row1["codeid"].ToString().PadLeft(3,'0') + HRA.GetFixedLengthString(row1["value"].ToString(),32) + end);
                        counter++;
                    }
                }
            }

            /*****************  (Footer) Trailer Record   *****************************/

            sb.Append(GetEligFileFooter(counter, hraPlanNum));


            return sb.ToString();
        }

        public DataSet GetAuditFile()
        {
            DataSet empds = new DataSet();
            DataSet dshrainfo = new DataSet();            
            HRAOperDAL dobj = new HRAOperDAL();
            empds.Clear();
            dshrainfo.Clear();
            int counter;

            DataSet dsAudit = new DataSet();
            dsAudit.Clear();
            DataTable dsTable;
            dsTable = dsAudit.Tables.Add("dsTable");
            DataRow row2;
            DataColumn col;
            col = new DataColumn("SSN"); dsTable.Columns.Add(col);
            col = new DataColumn("Emp#"); dsTable.Columns.Add(col);
            col = new DataColumn("Name"); dsTable.Columns.Add(col);
            col = new DataColumn("SubCtr"); dsTable.Columns.Add(col);
            col = new DataColumn("CodeID"); dsTable.Columns.Add(col);
            col = new DataColumn("Code Desc"); dsTable.Columns.Add(col);
            col = new DataColumn("Value"); dsTable.Columns.Add(col);


            empds = dobj.GetEmployeeInfo();

            foreach (DataRow row in empds.Tables[0].Rows)
            {
                counter = 0;
                dshrainfo = getHRaPartInfo(row);

                foreach (DataRow row1 in dshrainfo.Tables[0].Rows)
                {
                    if (row1["codedsc"].ToString() != "NAME" && row1["codedsc"].ToString() != "EMPNO")
                    {
                        counter++;
                        row2 = dsTable.NewRow();
                        row2["SSN"] = row["ssn"].ToString().Trim();
                        row2["Emp#"] = row["empno"].ToString();
                        row2["Name"] = row["lname"].ToString().Trim() + ", " + row["fname"].ToString().Trim();
                        row2["SubCtr"] = counter;
                        row2["CodeID"] = row1["codeid"].ToString();
                        row2["Code Desc"] = row1["codedsc"].ToString();
                        row2["Value"] = row1["value"].ToString();
                        dsTable.Rows.Add(row2);
                    }                    
                }
            }

            return dsAudit;
        }

        public DataSet GetAddrChgData(DataSet dsCurr)
        {
            string addr1_1, addr1_2, addr2_1, addr2_2, city_1, city_2, state_1, state_2, zip_1, zip_2;
            Boolean _found;
            DataSet dsPrev = new DataSet(); dsPrev.Clear();            
            DataSet dsResult = new DataSet(); dsResult.Clear();
            DataTable dsTable; dsTable = dsResult.Tables.Add("dsAddrChg");
            DataRow row;
            DataColumn col;
            col = new DataColumn("EE#"); dsTable.Columns.Add(col);
            col = new DataColumn("Name"); dsTable.Columns.Add(col);
            col = new DataColumn("Prior Addr1"); dsTable.Columns.Add(col);
            col = new DataColumn("Prior Addr2"); dsTable.Columns.Add(col);
            col = new DataColumn("Prior City"); dsTable.Columns.Add(col);
            col = new DataColumn("Prior State"); dsTable.Columns.Add(col);
            col = new DataColumn("Prior Zip"); dsTable.Columns.Add(col);
            col = new DataColumn("New Addr1"); dsTable.Columns.Add(col);
            col = new DataColumn("New Addr2"); dsTable.Columns.Add(col);
            col = new DataColumn("New City"); dsTable.Columns.Add(col);
            col = new DataColumn("New State"); dsTable.Columns.Add(col);
            col = new DataColumn("New Zip"); dsTable.Columns.Add(col);
            
            dsPrev = HRAOperDAL.GetPrevEligData("EligAddr");
           
            foreach (DataRow row1 in dsCurr.Tables[0].Rows)
            {
                _found = false;
                if (dsPrev.Tables.Count > 0)
                {
                    foreach (DataRow row2 in dsPrev.Tables[0].Rows)
                    {
                        if ((row1["empno"].ToString()).Equals(row2["empno"].ToString()))
                        {
                            _found = true;
                            addr1_1 = row1["addr1"].ToString().Trim().ToUpper();
                            addr1_2 = row2["addr1"].ToString().Trim().ToUpper();
                            addr2_1 = row1["addr2"].ToString().Trim().ToUpper();
                            addr2_2 = row2["addr2"].ToString().Trim().ToUpper();
                            city_1 = row1["city"].ToString().Trim().ToUpper();
                            city_2 = row2["city"].ToString().Trim().ToUpper();
                            state_1 = row1["state"].ToString().Trim().ToUpper();
                            state_2 = row2["state"].ToString().Trim().ToUpper();
                            zip_1 = row1["zip"].ToString().Trim().ToUpper();
                            zip_2 = row2["zip"].ToString().Trim().ToUpper();

                            if ((!addr1_1.Equals(addr1_2)) || (!addr2_1.Equals(addr2_2)) || (!city_1.Equals(city_2)) || (!state_1.Equals(state_2)) || (!zip_1.Equals(zip_2)))
                            {
                                row = dsTable.NewRow();
                                row["EE#"] = row1["empno"];
                                row["Name"] = row1["name"];
                                row["Prior Addr1"] = row2["addr1"];
                                row["Prior Addr2"] = row2["addr2"];
                                row["Prior City"] = row2["city"];
                                row["Prior State"] = row2["state"];
                                row["Prior Zip"] = row2["zip"];
                                row["New Addr1"] = row1["addr1"];
                                row["New Addr2"] = row1["addr2"];
                                row["New City"] = row1["city"];
                                row["New State"] = row1["state"];
                                row["New Zip"] = row1["zip"];
                                dsTable.Rows.Add(row);
                            }
                            break;
                        }
                    }
                }
                if (!_found)
                {
                    row = dsTable.NewRow();
                    row["EE#"] = row1["empno"];
                    row["Name"] = row1["name"];
                    row["Prior Addr1"] = String.Empty;
                    row["Prior Addr2"] = String.Empty;
                    row["Prior City"] = String.Empty;
                    row["Prior State"] = String.Empty;
                    row["Prior Zip"] = String.Empty;
                    row["New Addr1"] = row1["addr1"];
                    row["New Addr2"] = row1["addr2"];
                    row["New City"] = row1["city"];
                    row["New State"] = row1["state"];
                    row["New Zip"] = row1["zip"];
                    dsTable.Rows.Add(row);   
                }
            }

            if ((dsPrev.Tables.Count > 0) && (dsPrev.Tables[0].Rows.Count > dsCurr.Tables[0].Rows.Count))
            {
                foreach (DataRow row2 in dsPrev.Tables[0].Rows)
                {
                    _found = false;
                    foreach (DataRow row1 in dsCurr.Tables[0].Rows)
                    {
                        if ((row1["empno"].ToString()).Equals(row2["empno"].ToString()))
                        {
                            _found = true;
                            break;
                        }                        
                    }
                    if (!_found)
                    {
                        row = dsTable.NewRow();
                        row["EE#"] = row2["empno"];
                        row["Name"] = row2["name"];
                        row["Prior Addr1"] = row2["addr1"];
                        row["Prior Addr2"] = row2["addr2"];
                        row["Prior City"] = row2["city"];
                        row["Prior State"] = row2["state"];
                        row["Prior Zip"] = row2["zip"];
                        row["New Addr1"] = String.Empty;
                        row["New Addr2"] = String.Empty;
                        row["New City"] = String.Empty;
                        row["New State"] = String.Empty;
                        row["New Zip"] = String.Empty;
                        dsTable.Rows.Add(row);
                    }
                }
            }

            return dsResult;
        }

        public DataSet GetStatChgData(DataSet dsCurr)
        {
            string statcd_1, statcd_2;
            Boolean _found;
            DataSet dsPrev = new DataSet(); dsPrev.Clear();            
            DataSet dsResult = new DataSet(); dsResult.Clear();
            DataTable dsTable; dsTable = dsResult.Tables.Add("dsStatChg");
            DataRow row;
            DataColumn col;
            col = new DataColumn("EE#"); dsTable.Columns.Add(col);
            col = new DataColumn("Name"); dsTable.Columns.Add(col);
            col = new DataColumn("Status Code"); dsTable.Columns.Add(col);
            col = new DataColumn("Status Date"); dsTable.Columns.Add(col);

            dsPrev = HRAOperDAL.GetPrevEligData("EligStat");

            foreach (DataRow row1 in dsCurr.Tables[0].Rows)
            {
                _found = false;
                if (dsPrev.Tables.Count > 0)
                {
                    foreach (DataRow row2 in dsPrev.Tables[0].Rows)
                    {
                        if ((row1["empno"].ToString()).Equals(row2["empno"].ToString()))
                        {
                            _found = true;
                            statcd_1 = row1["statcd"].ToString().Trim().ToUpper();
                            statcd_2 = row2["statcd"].ToString().Trim().ToUpper();

                            if (!statcd_1.Equals(statcd_2))
                            {
                                row = dsTable.NewRow();
                                row["EE#"] = row1["empno"];
                                row["Name"] = row1["name"];
                                row["Status Code"] = row1["statcd"];
                                row["Status Date"] = row1["statdt"];
                                dsTable.Rows.Add(row);
                            }
                            break;
                        }
                    }
                }
                if (!_found)
                {
                    row = dsTable.NewRow();
                    row["EE#"] = row1["empno"];
                    row["Name"] = row1["name"];
                    row["Status Code"] = row1["statcd"];
                    row["Status Date"] = row1["statdt"];
                    dsTable.Rows.Add(row);
                }
            }

            return dsResult;
        }

        public void PGPEligFile_Client(string inputFile,string outputFile, string folderPath)
        {
            Page page = HttpContext.Current.Handler as Page;
            if (!page.ClientScript.IsClientScriptBlockRegistered("PGPFile"))
                page.ClientScript.RegisterClientScriptBlock(typeof(Page), "PGPFile", "PGPofFile('" + inputFile + "', '" + outputFile + "', '" + folderPath + "');", true);
        }
       
        public void PGPEligFile()
        {
            const int ProcessTimeOutMilliseconds = 10000;                     
            StreamWriter _sw;            
            string _file, _content;             
            _file = "";

            try
            {
                _file = HttpContext.Current.Server.MapPath("~/uploads/") + HRA_Results.GetFileName("Elig", String.Empty);
                _content = GetEligFile();                
                _sw = File.CreateText(_file);
                _sw.WriteLine(_content);
                _sw.Close();
                ProcessStartInfo startInfo = new ProcessStartInfo();

                //Set up process info

                startInfo.FileName = HttpContext.Current.Server.MapPath("~/GnuPG/") + "gpg.exe";
                startInfo.Arguments = "--homedir \"" + HttpContext.Current.Server.MapPath("~/GnuPG") +
                                      "\" --always-trust --armor --recipient \"" + "Network_Security@putnaminv.com" +
                                      "\" --output \"" + HttpContext.Current.Server.MapPath("~/uploads/") + "HRA.txt.pgp" +
                                      "\" --encrypt \"" + _file + "\"";
                startInfo.UseShellExecute = false;
                startInfo.CreateNoWindow = true;
                startInfo.WorkingDirectory = HttpContext.Current.Server.MapPath("~/GnuPG");
                //Start process and wait for it to execute
                startInfo.ErrorDialog = false;
                startInfo.RedirectStandardError = true;
                p = Process.Start(startInfo);
                ThreadStart errorEntry = new ThreadStart(StandardErrorReader);
                Thread errorThread = new Thread(errorEntry);
                errorThread.Start();

                if (p.WaitForExit(ProcessTimeOutMilliseconds))
                {
                    if (!errorThread.Join(ProcessTimeOutMilliseconds / 2))
                    {
                        errorThread.Abort();
                    }
                }
                else
                {
                    _errormsg = "Timed out after " + ProcessTimeOutMilliseconds.ToString() + " milliseconds";
                    p.Kill();
                    if (errorThread.IsAlive)
                    {
                        errorThread.Abort();
                    }
                }
                if (p.ExitCode != 0)
                {
                    if (_errormsg == "")
                    {
                        _errormsg = "GPGNET: [" + p.ExitCode.ToString() + "]: Unknown error";
                    }
                    throw new Exception(_errormsg);
                }
            }
            finally
            {
                if (File.Exists(_file))
                {
                    File.Delete(_file);
                }
            }
        }

        public void FTPEligFile(string _file)
        {
            string _uri;
            int contentLen, buffLength;
            Impersonation _imp = new Impersonation();
            Page page = HttpContext.Current.Handler as Page;
            FtpWebRequest reqFTP;
            FileInfo fileInf;
            FileStream fs;
            Stream strm;


            if (_imp.impersonateValidUser(HttpContext.Current.Session["uid"].ToString(), "CORP", EncryptDecrypt.Decrypt(HttpContext.Current.Session["pwd"].ToString())))
            {
                if (!File.Exists(_file))
                    throw new Exception("Problem in PGP of file");               

                fileInf = new FileInfo(_file);
                _uri = "ftp://ftp.datamgmt.mercerhrs.com/fa00760.in.pgp";

                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(_uri));
                reqFTP.Credentials = new NetworkCredential("Blackftp", "N0v2oo4");
                reqFTP.Proxy = null;
                reqFTP.KeepAlive = false;
                reqFTP.Method = WebRequestMethods.Ftp.UploadFile;
                reqFTP.UseBinary = true;
                reqFTP.ContentLength = fileInf.Length;

                buffLength = 2048;
                byte[] buff = new byte[buffLength];
                fs = fileInf.OpenRead();
                strm = reqFTP.GetRequestStream();
                contentLen = fs.Read(buff, 0, buffLength);

                while (contentLen != 0)
                {
                    strm.Write(buff, 0, contentLen);
                    contentLen = fs.Read(buff, 0, buffLength);
                }
                strm.Close();
                fs.Close();

                _imp.undoImpersonation();

                if (!page.ClientScript.IsClientScriptBlockRegistered("OpenWindow"))
                    page.ClientScript.RegisterClientScriptBlock(typeof(Page), "OpenWindow", "window.open('ftp://Blackftp:N0v2oo4@ftp.datamgmt.mercerhrs.com/');", true);
            }
            else
            {
                throw new Exception("Error in accessing network location");
            }
        }

        void StandardErrorReader()
        {
            string error = p.StandardError.ReadToEnd();
            lock (this)
            {
                _errormsg = error;
            }
        }

        string GetEligFileHeader(string hraPlanNum)
        {
            StringBuilder sb = new StringBuilder();
            
            sb.Append("PUTSY");
            sb.Append(HRA.GetFixedLengthString(hraPlanNum,6));
            sb.Append(HRA.GetFixedLengthString("CENSUS",9)); 
            sb.Append(' ', 60);

            return sb.ToString();
        }

        DataSet getHRaPartInfo(DataRow row)
        {
            
            Boolean ownerfound = false;
            string ownereffdt = "";
            string origowner = "";
            string origDOB="";
            DataSet dscodes = new DataSet(); dscodes.Clear();
            DataSet depds = new DataSet(); depds.Clear();            
            HRAOperDAL dobj = new HRAOperDAL();
            DataSet dsinfo = new DataSet(); dsinfo.Clear();            
            DataTable dsTable;
            dsTable =  dsinfo.Tables.Add("newTable1");
            DataRow row1;
            DataColumn col;
            col = new DataColumn("codeid"); dsTable.Columns.Add(col);
            col = new DataColumn("codedsc"); dsTable.Columns.Add(col);
            col = new DataColumn("value"); dsTable.Columns.Add(col);

            if ((row["dob"] != DBNull.Value) && (row["dob"].ToString().Trim() != string.Empty))
                origDOB = Convert.ToDateTime(row["dob"]).ToString("MM/dd/yyyy");
            else
                origDOB = "";
            
            // Check if ownership has changed
            if (OwnerChanged(row["statuscd"].ToString().Trim()))
            {
                // check ownership information
                depds = dobj.GetDependantInfo(row["empno"].ToString());
                ownerfound = OwnerFound(depds);
                if (ownerfound)
                {
                    origowner = row["lname"].ToString().Trim() + ", " + row["fname"].ToString().Trim();
                    row = OverlayOwnerInfo(row, depds);  // Overlay ownership information                        
                    ownereffdt = GetOwnerEffDt(depds); // Get owner effective date
                }
            }

            //Code Ids
            NameValueCollection _codeids = HRAOperDAL.GetCodeIds();

            //Previous Owner
            if (ownerfound)
            {
                row1 = dsTable.NewRow();
                row1["codeid"] = _codeids.Get("PREVOWNR");
                row1["codedsc"] = "PREVOWNR";
                row1["value"] = origowner;
                dsTable.Rows.Add(row1);
            }
            
            // Name
            row1 = dsTable.NewRow();
            row1["codeid"] = _codeids.Get("NAME");
            row1["codedsc"] = "NAME";
            row1["value"] = (row["lname"].ToString().Trim() + ", " + row["fname"].ToString().Trim());            
            dsTable.Rows.Add(row1);

            //Employee Number
            row1 = dsTable.NewRow();
            row1["codeid"] = _codeids.Get("EMPNO");
            row1["codedsc"] = "EMPNO";
            row1["value"] = row["empno"].ToString();
            dsTable.Rows.Add(row1);

            // Sex Code
            string sexcd = row["sex"].ToString().Trim().ToUpper();
            
            if (sexcd.Equals("M"))
                sexcd = "1";
            else
                sexcd = "2";

            row1 = dsTable.NewRow();
            row1["codeid"] = _codeids.Get("SEXCD");
            row1["codedsc"] = "SEXCD";
            row1["value"] = sexcd;
            dsTable.Rows.Add(row1);

            // Date of Birth            
            string dob = "";
            if ((row["dob"] != DBNull.Value) && (row["dob"].ToString().Trim() != string.Empty))
                dob = Convert.ToDateTime(row["dob"]).ToString("yyyyMMdd");
            else
                dob = dob.PadRight(8, ' ');

            row1 = dsTable.NewRow();
            row1["codeid"] = _codeids.Get("DOB");
            row1["codedsc"] = "DOB";
            row1["value"] = dob;
            dsTable.Rows.Add(row1);

            // Date of Death
            if (OwnerChanged(row["statuscd"].ToString().Trim()))                
            {
                string statusdt = "";                
                if ((row["statusdt"] != DBNull.Value) && (row["statusdt"].ToString().Trim() != string.Empty))
                {
                    statusdt = Convert.ToDateTime(row["statusdt"]).ToString("yyyyMMdd");
                }
                else
                {
                    statusdt = statusdt.PadRight(8, ' ');
                }
               
                row1 = dsTable.NewRow();
                row1["codeid"] = _codeids.Get("DIED");
                row1["codedsc"] = "DIED";
                row1["value"] = statusdt;
                dsTable.Rows.Add(row1);               
            }

            // Permanent Full Time Date
            string permftdt = "";
            if ((row["permftdt"] != DBNull.Value) && (row["permftdt"].ToString().Trim() != string.Empty))
                permftdt = Convert.ToDateTime(row["permftdt"]).ToString("yyyyMMdd");
            else
                permftdt = permftdt.PadRight(8, ' ');

            row1 = dsTable.NewRow();
            row1["codeid"] = _codeids.Get("PERMDT");
            row1["codedsc"] = "PERMDT";
            row1["value"] = permftdt;
            dsTable.Rows.Add(row1);


            //062 record for status 'DTH'(eligible for claims)

            if (OwnerChanged(row["statuscd"].ToString().Trim()) && ((row["statusdt"] != DBNull.Value) && (row["statusdt"].ToString().Trim() != string.Empty)))
            {
                if (origDOB != String.Empty)
                {
                    row1 = dsTable.NewRow();
                    row1["codeid"] = _codeids.Get("RET EFFDT / DIED");
                    row1["codedsc"] = "DIED";
                    row1["value"] = GetDeathModifyDt(row, Convert.ToDateTime(origDOB));
                    dsTable.Rows.Add(row1);
                }
            } 
            
            // Retirement Date (eligible for claims)
            if ((row["retiredt"] != DBNull.Value) && (row["retiredt"].ToString().Trim() != string.Empty))
            {
                DateTime retiredt = Convert.ToDateTime(row["retiredt"]);
                row1 = dsTable.NewRow();
                row1["codeid"] = _codeids.Get("ORIGRET");
                row1["codedsc"] = "ORIGRET";
                row1["value"] = retiredt.ToString("yyyyMMdd");
                dsTable.Rows.Add(row1);

                if (string.Compare(row["statuscd"].ToString().Trim(), "TRM", true) == 0)
                {
                    if (origDOB != String.Empty)
                    {
                        row1 = dsTable.NewRow();
                        row1["codeid"] = _codeids.Get("RET EFFDT / DIED");
                        row1["codedsc"] = "RET EFFDT";
                        row1["value"] = GetTrmModifyDt(Convert.ToDateTime(origDOB), retiredt);
                        dsTable.Rows.Add(row1);
                    }
                }
            }

            // Account Owner Effective Date
            if (ownerfound)
            {
                row1 = dsTable.NewRow();
                row1["codeid"] = _codeids.Get("OWNER DT");
                row1["codedsc"] = "OWNER DT";
                row1["value"] = ownereffdt;
                dsTable.Rows.Add(row1);
            }

            // Term Reason Code
            string _status = row["statuscd"].ToString().Trim().ToUpper();
            string statuscd = String.Empty;
            if ((row["retiredt"] != DBNull.Value) && (row["retiredt"].ToString().Trim() != string.Empty) && (_status.Equals("TRM")))
            {
                statuscd = "4";
            }
            if ((row["statusdt"] != DBNull.Value) && (row["statusdt"].ToString().Trim() != string.Empty) && (_status.Equals("DTH")))
            {
                statuscd = "6";
            }       

            if(!(statuscd.Equals(String.Empty)))
            {
            row1 = dsTable.NewRow();
            row1["codeid"] = _codeids.Get("TERMREAS");
            row1["codedsc"] = "TERMREAS";
            row1["value"] = statuscd;
            dsTable.Rows.Add(row1);
            }            

            // Address line 1
            row1 = dsTable.NewRow();
            row1["codeid"] = _codeids.Get("ADDR1");
            row1["codedsc"] = "ADDR1";
            row1["value"] = row["addr1"].ToString().Trim();
            dsTable.Rows.Add(row1);            

            // Address line 2
            string addr2 = row["addr2"].ToString().Trim();
            if (addr2 != "")
            {
                row1 = dsTable.NewRow();
                row1["codeid"] = _codeids.Get("ADDR2");
                row1["codedsc"] = "ADDR2";
                row1["value"] = addr2;
                dsTable.Rows.Add(row1);      
            }

            // City
            row1 = dsTable.NewRow();
            row1["codeid"] = _codeids.Get("CITY");
            row1["codedsc"] = "CITY";
            row1["value"] = row["city"].ToString().Trim();
            dsTable.Rows.Add(row1);

            // State
            row1 = dsTable.NewRow();
            row1["codeid"] = _codeids.Get("STATE");
            row1["codedsc"] = "STATE";
            row1["value"] = row["state"].ToString().Trim();
            dsTable.Rows.Add(row1);            

            // Zip Code
            row1 = dsTable.NewRow();
            row1["codeid"] = _codeids.Get("ZIPCD");
            row1["codedsc"] = "ZIPCD";
            row1["value"] = row["zip"].ToString().Trim();
            dsTable.Rows.Add(row1);

            // QDRO Flag
            row1 = dsTable.NewRow();
            row1["codeid"] = _codeids.Get("QDRO");
            row1["codedsc"] = "QDRO";
            row1["value"] = "0.00";
            dsTable.Rows.Add(row1);

            return dsinfo;
        }       
       
        Boolean OwnerChanged(string statuscd)
        {
            if (string.Compare(statuscd, "DTH", true) == 0)
                return true;
            else
                return false;
        }

        Boolean OwnerFound(DataSet depds)
        {
            foreach (DataRow drow in depds.Tables[0].Rows)
            {
                if ((Convert.ToInt32(drow["ownerflg"]) == 1) && (Convert.ToInt32(drow["validflg"]) == 1) && (drow["stopdt"] == DBNull.Value))
                {
                    return true;                 
                }
            }

            return false;
        }

        DataRow OverlayOwnerInfo(DataRow row, DataSet depds)
        {
            foreach (DataRow drow in depds.Tables[0].Rows)
            {
                if ((Convert.ToInt32(drow["ownerflg"]) == 1) && (Convert.ToInt32(drow["validflg"]) == 1) && (drow["stopdt"] == DBNull.Value))
                {  
                    row["lname"] = drow["dep_lname"];
                    row["fname"] = drow["dep_fname"];
                    row["sex"] = drow["dep_sex"];
                    row["dob"] = drow["dep_dob"];
                    if (drow["dep_addr1"].ToString().Trim() != string.Empty)
                    {
                        row["addr1"] = drow["dep_addr1"];
                        row["addr2"] = drow["dep_addr2"];
                        row["city"] = drow["dep_city"];
                        row["state"] = drow["dep_state"];
                        row["zip"] = drow["dep_zip"];
                    }
                }
            }

            return row;
        }

        string GetOwnerEffDt(DataSet depds)
        {
            string ownereffdt = "";

            foreach (DataRow drow in depds.Tables[0].Rows)
            {
                if ((Convert.ToInt32(drow["ownerflg"]) == 1) && (Convert.ToInt32(drow["validflg"]) == 1) && (drow["stopdt"] == DBNull.Value))
                {
                    if ((drow["ownereffdt"] != DBNull.Value) && (drow["ownereffdt"].ToString().Trim() != string.Empty))
                        ownereffdt = Convert.ToDateTime(drow["ownereffdt"]).ToString("yyyyMMdd");
                    else
                        ownereffdt = "";
                }
            }

            return ownereffdt.PadRight(8, ' ');
        }

        string GetDeathModifyDt(DataRow row, DateTime dob)
        {
            string retiredt;
            DateTime chkdt = Convert.ToDateTime("2006/08/25"); // Cut-off Date
            DateTime statdt = Convert.ToDateTime(row["statusdt"]);

            if ((row["retiredt"] != DBNull.Value) && (row["retiredt"].ToString().Trim() != string.Empty))
            {
                DateTime retdt = Convert.ToDateTime(row["retiredt"]);

                if (DateTime.Compare(retdt, chkdt) > 0)// You need to perform age 59 calculation
                {
                    if (GetAge(dob.ToString("MM/dd/yyyy"), retdt) < 59)
                    {
                        if (GetAge(dob.ToString("MM/dd/yyyy"), statdt) < 59)
                        {
                            retiredt = (statdt.AddDays(1)).ToString("yyyyMMdd");
                        }
                        else
                        {
                            retiredt = (dob.AddYears(59)).ToString("yyyyMMdd");
                        }
                    }
                    else
                    {
                        retiredt = (retdt.AddDays(1)).ToString("yyyyMMdd");
                    }
                }
                else
                {
                    retiredt = chkdt.ToString("yyyyMMdd");                    
                }
            }
            else
            {
                retiredt = (statdt.AddDays(1)).ToString("yyyyMMdd");
            }

            return retiredt;            
        }

        string GetTrmModifyDt(DateTime dob, DateTime retiredt)
        {
            string retdt;
            DateTime chkdt = Convert.ToDateTime("2006/08/25"); // Cut-off Date

            if (DateTime.Compare(retiredt, chkdt) > 0)// You need to perform age 59 calculation
            {
                if (GetAge(dob.ToString("MM/dd/yyyy"), retiredt) < 59)
                {
                    retdt = (dob.AddYears(59)).ToString("yyyyMMdd");
                }
                else
                {
                    retdt = (retiredt.AddDays(1)).ToString("yyyyMMdd");
                }
            }
            else
            {
                retdt = chkdt.ToString("yyyyMMdd");
            }

            return retdt;
        }

        string GetEligFileFooter(int counter, string hraPlanNum)
        {
            string strcntr = counter.ToString();
            strcntr = strcntr.PadLeft(11, '0');            
            StringBuilder sb = new StringBuilder();

            sb.Append("PUTE");
            sb.Append(' ', 1);
            sb.Append(HRA.GetFixedLengthString(hraPlanNum,6));
            sb.Append(HRA.GetFixedLengthString("CENSUS",9));
            sb.Append(' ', 29);
            sb.Append(strcntr);
            sb.Append(' ', 20);

            return sb.ToString(); 
        }

        int GetAge(string dob, DateTime today)
        {
            DateTime dobdt = Convert.ToDateTime(dob).Date;                   
            int months, years;
            int age;

            // compute difference in total months
            months = 12 * (today.Year - dobdt.Year) + (today.Month - dobdt.Month);

            // based upon the 'days', adjust months 
            if (today.Day < dobdt.Day)
            {
                months--;
            }

            // compute years (age)
            years = months / 12;
            age = years;

            return age;
        }
   }
}