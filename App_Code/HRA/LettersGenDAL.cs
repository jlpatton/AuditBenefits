using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Xml;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
//using iTextSharp.text;
//using iTextSharp.text.pdf;

namespace EBA.Desktop.HRA
{
    /// <summary>
    /// Summary description for LettersGenDAL
    /// </summary>
    public class LettersGenDAL
    {
        static string connStr = ConfigurationManager.ConnectionStrings["EBADB"].ConnectionString;
        static SqlConnection conn = new SqlConnection(connStr);
        static SqlCommand command = null;

        public LettersGenDAL()
        {

        }

        #region LetterTemplate-Insert/Update

        public static int insertNewLetterType(string _letterType, string _lettercode)
        {
            int _id;
            try
            {
                if (conn != null && conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                command = new SqlCommand("sp_HRAInsertLetterType", conn);
                command.Parameters.AddWithValue("@type", _letterType);
                command.Parameters.AddWithValue("@code", _lettercode);
                command.CommandType = CommandType.StoredProcedure;

                _id = Convert.ToInt32(command.ExecuteScalar());
                return _id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }

        }

        public static String getTemplate(string _lType, int _version)
        {
            string _cmdstr = "SELECT tpContent FROM hra_Ltrs_template,hra_Ltrs_Type "
                                + " WHERE ltrTypeCode = @tName AND tpVersion = @Version AND ltrId = tpTypeId";

            XmlDocument xDoc1 = new XmlDocument();
            StringReader reader;
            string x1;

            try
            {
                if (conn != null && conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                command = new SqlCommand(_cmdstr, conn);
                command.Parameters.AddWithValue("@tName", _lType);
                command.Parameters.AddWithValue("@Version", _version);

                x1 = command.ExecuteScalar().ToString();
                return x1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }

        public static String getTemplate(int _id)
        {
            string _cmdstr = "SELECT tpContent FROM hra_Ltrs_template,hra_Ltrs_Type "
                                + " WHERE tpId = @id AND ltrId = tpTypeId";

            XmlDocument xDoc1 = new XmlDocument();
            StringReader reader;
            string x1;

            try
            {
                if (conn != null && conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                command = new SqlCommand(_cmdstr, conn);
                command.Parameters.AddWithValue("@id", _id);

                x1 = command.ExecuteScalar().ToString();
                return x1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }

        public static String getTemplateGenerated(int _id)
        {
            string _cmdstr = "SELECT tpContent FROM hra_Ltrs_template,hra_Ltrs_Generated "
                                + " WHERE lgId = @id AND tpId = lgLetterId";

            XmlDocument xDoc1 = new XmlDocument();
            StringReader reader;
            string x1;

            try
            {
                if (conn != null && conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                command = new SqlCommand(_cmdstr, conn);
                command.Parameters.AddWithValue("@id", _id);

                x1 = command.ExecuteScalar().ToString();
                return x1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }

        public static int getTemplateVersion(int _templateId)
        {
            //string _cmdstr = "SELECT MAX(tpVersion) FROM hra_Ltrs_template WHERE tpTypeId = @type";
            //modified the query to get the latest tpversion based the last template date uploaded 9-14-2009
            string _cmdstr = "SELECT (tpVersion) FROM hra_Ltrs_template WHERE tpTypeId = @type and tpdate in (SELECT MAX(tpdate) FROM hra_Ltrs_template WHERE tpTypeId = @type)";
            int _version = 0;

            try
            {
                if (conn != null && conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                command = new SqlCommand(_cmdstr, conn);
                command.Parameters.AddWithValue("@type", _templateId);
                
                if (!command.ExecuteScalar().Equals(DBNull.Value))
                {
                    _version = Convert.ToInt32(command.ExecuteScalar());
                }

                return _version;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }

        public static int getTemplateVersion(string _letterType)
        {
            //string _cmdstr = "SELECT MAX(tpVersion) FROM hra_Ltrs_template,hra_Ltrs_Type "  
                               // + "WHERE tpTypeId = ltrId AND ltrTypeCode = @code";

            string _cmdstr = "SELECT MAX(tpVersion) FROM hra_Ltrs_template,hra_Ltrs_Type WHERE tpTypeId = ltrId AND ltrTypeCode = @code "
                             + " and tpdate in (SELECT MAX(tpdate) FROM hra_Ltrs_template,hra_Ltrs_Type WHERE tpTypeId = ltrId AND ltrTypeCode = @code)";
            int _version = 0;

            try
            {
                if (conn != null && conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                command = new SqlCommand(_cmdstr, conn);
                command.Parameters.AddWithValue("@code", _letterType);

                if (!command.ExecuteScalar().Equals(DBNull.Value))
                {
                    _version = Convert.ToInt32(command.ExecuteScalar());
                }

                return _version;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }

        public static string getLetterTypeCode(int _templateId)
        {
            string _cmdstr = "SELECT ltrTypeCode FROM hra_Ltrs_Type,hra_Ltrs_template "
                                + " WHERE tpId = @type AND ltrId = tpTypeId";
            string _lcode = "";

            try
            {
                if (conn != null && conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                command = new SqlCommand(_cmdstr, conn);
                command.Parameters.AddWithValue("@type", _templateId);

                if (!command.ExecuteScalar().Equals(DBNull.Value))
                {
                    _lcode = Convert.ToString(command.ExecuteScalar());
                }

                return _lcode;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }
        public static String getStageTemplate(int _id)
        {
            string _cmdstr = "SELECT tpStageContent FROM hra_Ltrs_template_staging,hra_Ltrs_Type "
                                + " WHERE tpStageId = @id AND ltrId = tpStageTypeId";

            XmlDocument xDoc1 = new XmlDocument();
            StringReader reader;
            string x1;

            try
            {
                if (conn != null && conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                command = new SqlCommand(_cmdstr, conn);
                command.Parameters.AddWithValue("@id", _id);

                x1 = command.ExecuteScalar().ToString();
                return x1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }

        public static string getLetterTypeCodeStage(int _templateId)
        {
            string _cmdstr = "SELECT ltrTypeCode FROM hra_Ltrs_Type,hra_Ltrs_template_staging "
                                + " WHERE tpStageId = @type AND ltrId = tpStageTypeId";
            string _lcode = "";

            try
            {
                if (conn != null && conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                command = new SqlCommand(_cmdstr, conn);
                command.Parameters.AddWithValue("@type", _templateId);

                if (!command.ExecuteScalar().Equals(DBNull.Value))
                {
                    _lcode = Convert.ToString(command.ExecuteScalar());
                }

                return _lcode;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }

        public static bool checkStageExists(int _templateId, int _version)
        {
            string _cmdstr = "SELECT COUNT(*) FROM hra_Ltrs_template_staging "
                                + " WHERE tpStageTypeId = @type AND tpStageVersion = @version ";

            bool _exists = false;

            try
            {
                if (conn != null && conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                command = new SqlCommand(_cmdstr, conn);
                command.Parameters.AddWithValue("@type", _templateId);                
                command.Parameters.AddWithValue("@version", _version);
                if (Convert.ToInt32(command.ExecuteScalar()) > 0)
                {
                    _exists = true;
                }

                return _exists;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }

        public static void StoreStageTemplate(int _templateId,int _version, string _xmlFile)
        {

            try
            {
                if (conn != null && conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                command = new SqlCommand("sp_HRALetterTemplateStage", conn);
                command.Parameters.AddWithValue("@tId", _templateId);               
                command.Parameters.AddWithValue("@Version", _version);
                command.Parameters.AddWithValue("@file", _xmlFile);
                command.Parameters.AddWithValue("@time", DateTime.Now);                
                command.CommandType = CommandType.StoredProcedure;
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }

        public static void UpdateStageTemplate(int _templateId, int _version, string _xmlFile)
        {

            try
            {
                if (conn != null && conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                command = new SqlCommand("sp_HRALetterUpdateTemplateStage", conn);
                command.Parameters.AddWithValue("@tId", _templateId);
                command.Parameters.AddWithValue("@Version", _version);
                command.Parameters.AddWithValue("@file", _xmlFile);
                command.Parameters.AddWithValue("@time", DateTime.Now);
                command.CommandType = CommandType.StoredProcedure;
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }

        public static void StoreTemplate(int _letterid)
        {
            int _idgen;
            try
            {
                if (conn != null && conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                command = new SqlCommand("sp_HRATemplate_Approved", conn);
                command.Parameters.AddWithValue("@letterid", _letterid);
                command.CommandType = CommandType.StoredProcedure;
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }

        #endregion

        #region EmploeeData

        public static DataTable getEmpData()
        {
            try
            {
                if (conn != null && conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                command = new SqlCommand("sp_HRAgetEmployees", conn);
                command.Parameters.AddWithValue("@empList", "");
                command.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter da = new SqlDataAdapter();
                DataTable dt = new DataTable();
                da.SelectCommand = command;
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }

        public static DataTable getEmpData(string _eList)
        {
            try
            {
                if (conn != null && conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                command = new SqlCommand("sp_HRAgetEmployees", conn);
                command.Parameters.AddWithValue("@empList", _eList);
                command.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter da = new SqlDataAdapter();
                DataTable dt = new DataTable();
                da.SelectCommand = command;
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }

        public static DataTable getEmpwithallDepData()
        {           
            try
            {
                if (conn != null && conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                command = new SqlCommand("sp_HRAgetEmployeeDepsAll", conn);                
                command.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter da = new SqlDataAdapter();
                DataTable dt = new DataTable();
                da.SelectCommand = command;
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }

        public static DataTable getEmpwithallDepData(string _eList)
        {
            try
            {
                if (conn != null && conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                command = new SqlCommand("sp_HRAgetEmployeeDeps_Test", conn);
                //replace this with the follwing sp for production db 8-21-2009
                //command = new SqlCommand("sp_HRAgetEmployeeDeps_Dep", conn);
                command.Parameters.AddWithValue("@empList", _eList);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter();
                DataTable dt = new DataTable();
                da.SelectCommand = command;
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }        

        public static DataTable getEmpDepData(int _enum, string _dssn)
        {
            try
            {
                if (conn != null && conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                command = new SqlCommand("sp_HRAgetEmployeeDepRecord", conn);
                command.Parameters.AddWithValue("@empNum", _enum);
                command.Parameters.AddWithValue("@dssn", _dssn);
                command.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter da = new SqlDataAdapter();
                DataTable dt = new DataTable();
                da.SelectCommand = command;
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }

        public static List<string> getEmpNumbers()
        {
            string _cmdstr = "SELECT DISTINCT empl_empno FROM Employee WHERE empl_hra_part = 1 ORDER BY empl_empno ";
            List<string> emp = new List<string>();
            SqlDataReader _reader = null;

            try
            {
                if (conn != null && conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                command = new SqlCommand(_cmdstr, conn);
                
                _reader = command.ExecuteReader();
                while (_reader.Read())
                {
                    emp.Add(_reader[0].ToString());
                }
                _reader.Close();
                return emp;
            }
            catch (Exception ex)
            {
                _reader.Close();
                throw ex;
            }
            finally
            {                
                conn.Close();
            }
        }

        #endregion


        public static DataSet getGenDataset(int _genId)
        {
            string _cmdstr = "SELECT lgDataset FROM hra_Ltrs_Generated "
                                + " WHERE lgId = @id ";
            string _dsStr = "";
            DataSet _ds = new DataSet(); _ds.Clear();            
            try
            {
                if (conn != null && conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                command = new SqlCommand(_cmdstr, conn);
                command.Parameters.AddWithValue("@id", _genId);
                object _temp = command.ExecuteScalar();
                if (!_temp.Equals(DBNull.Value))
                {
                    _dsStr = _temp.ToString();
                    StringReader s = new StringReader(_dsStr);
                    _ds.ReadXml(s);                       
                }
                return _ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }

        public static int StoreGeneratedLetter(string _letType, int _version, string _dataset)
        {
            //Guid uniqueID = Guid.NewGuid();
            //byte[] bArr = uniqueID.ToByteArray();
            //int autonum = BitConverter.ToInt32(bArr, 0);

            int _idgen;
            try
            {
                if (conn != null && conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                command = new SqlCommand("sp_HRAStoreGeneratedLetter", conn);
                command.Parameters.AddWithValue("@ltype", _letType);
                command.Parameters.AddWithValue("@lprintnum", "p" + DateTime.Now.ToString("s",DateTimeFormatInfo.InvariantInfo));
                command.Parameters.AddWithValue("@version", _version);
                command.Parameters.AddWithValue("@dataset", _dataset);
                command.Parameters.AddWithValue("@user", HttpContext.Current.User.Identity.Name.ToString());
                command.CommandType = CommandType.StoredProcedure;
                _idgen = Convert.ToInt32(command.ExecuteScalar());
                return _idgen;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }

        public static void StoreLetterHistory(int _genId, List<string> _employees)
        {
            try
            {
                if (conn != null && conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                foreach (string _empnum in _employees)
                {

                    command = new SqlCommand("sp_HRALetterHistory", conn);
                    command.Parameters.AddWithValue("@gid", _genId);
                    command.Parameters.AddWithValue("@emp", _empnum);
                    command.CommandType = CommandType.StoredProcedure;
                    command.ExecuteNonQuery();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }

        public static void StoreLetterHistory(int _genId, int _employee)
        {
            try
            {
                if (conn != null && conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                command = new SqlCommand("sp_HRALetterHistory", conn);
                command.Parameters.AddWithValue("@gid", _genId);
                command.Parameters.AddWithValue("@emp", _employee);
                command.CommandType = CommandType.StoredProcedure;
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }

        public static void rollbackStoreLetter(int _gid)
        {
            string _cmdstr = "DELETE FROM hra_Ltrs_Generated "
                                + " WHERE lgId = @id";
            try
            {
                if (conn != null && conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                command = new SqlCommand(_cmdstr, conn);
                command.Parameters.AddWithValue("@id", _gid); 
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }

        public static void updatePending(int _genId,int _penId)
        {
            string _cmdstr = "UPDATE hra_Ltrs_Pending SET pnStatus = '0',pnGenId = @id WHERE pnId = @pid";
            try
            {
                if (conn != null && conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                command = new SqlCommand(_cmdstr, conn);
                command.Parameters.AddWithValue("@id", _genId);
                command.Parameters.AddWithValue("@pid", _penId);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }  
      
        //public static void openPDF(string filepath)

        //{
        //    PdfReader pdfreader =  new PdfReader(new RandomAccessFileOrArray(HttpContext.Current.Request.MapPath(filepath)), null);
        //    PdfStamper ps = null;
        //    ps = new PdfStamper(pdfreader, HttpContext.Current.Response.OutputStream);
        //    PdfAction jAction = PdfAction.JavaScript("this.print(true);\r", ps.Writer);
        //    ps.Writer.AddJavaScript(jAction);
        //    ps.FormFlattening = true;
        //    ps.Close();
        //    HttpContext.Current.Response.ContentType = "application/pdf";
        //    HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + "HRA_SPD_Language.pdf");
        //    HttpContext.Current.Response.Flush();
        //    HttpContext.Current.Response.End();

        //}

    }
}
