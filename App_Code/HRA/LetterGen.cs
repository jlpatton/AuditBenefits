using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace EBA.Desktop.HRA
{
    /// <summary>
    /// Summary description for LetterGen
    /// </summary>
    public class LetterGen
    {
        static public XmlDocument oXMLWordDoc = new XmlDocument();
        static public XmlNamespaceManager oNSMgr = new XmlNamespaceManager(oXMLWordDoc.NameTable);
        DataTable dt = new DataTable(); 
        DataTable dtFinal = new DataTable();

        public LetterGen(string _namespace)
        {
            //add the schema's namespace to a name space manager
            LoadNS("ns1", _namespace);
            LoadNS("w", "http://schemas.microsoft.com/office/word/2003/wordml");
            dt.Clear();
            dtFinal.Clear();
        }

        public void LoadFile(string sFilePath)
        {
            oXMLWordDoc.Load(sFilePath);
        }

        private void LoadNS(object sPrefix, object sURI)
        {
            oNSMgr.AddNamespace(sPrefix.ToString(), sURI.ToString());
        }

        public void save(string sFileName)
        {
            oXMLWordDoc.Save(sFileName);
        }

        public string[] generateLetter(string _empNums, string _lType, int _version)
        {
            string _temReaderstr = LettersGenDAL.getTemplate(_lType, _version);
            _temReaderstr = replaceNamespace(_temReaderstr);
            StringReader _temReader = new StringReader(_temReaderstr);
            List<string> _elList0 = new List<string>();
            string _exceptionEmpl = "";
            XmlDataDocument xmlDataDoc = new XmlDataDocument();
            int _genertedId = 0 ;
            try
            {
                if (!_empNums.Equals(""))
                {
                    _elList0 = createEmpList(_empNums);
                    foreach (string _empl in _elList0)
                    {
                        PilotData pData = new PilotData();
                        bool _found = pData.checkExistRecord(Convert.ToInt32(_empl));
                        if (!_found)
                        {
                            _exceptionEmpl += _empl + ", ";
                        }                        
                        pData = null;
                    }
                    if (!_exceptionEmpl.Equals(""))
                    {
                        _exceptionEmpl = _exceptionEmpl.Remove(_exceptionEmpl.Length - 1);
                        throw (new Exception("Employee number(s) " + _exceptionEmpl + " not valid! "));
                    }
                }                
                dt = LettersGenDAL.getEmpwithallDepData(_empNums);
                getFinalData(_temReader);
                string[] _letters = new string[dtFinal.Rows.Count];

                //get letters with all bookmarks replaced with correct values
                _letters = buildLetter(_temReaderstr);

                //get Identity for generated letter
                DataSet dsGen = new DataSet();
                dsGen.Clear();
                dsGen.Tables.Add(dt);
                _genertedId = LettersGenDAL.StoreGeneratedLetter(_lType, _version,dsGen.GetXml());

                //store list of employees with letter ids for history purpose
                LettersGenDAL.StoreLetterHistory(_genertedId, _elList0);
                
                return _letters;
            }
            catch (Exception ex)
            {
                if(_genertedId != 0)
                {
                    LettersGenDAL.rollbackStoreLetter(_genertedId);
                }
                throw ex;
            }

        }

        public string[] generateLetter(string _lType, int _version, bool _preview)
        {
            string _temReaderstr = LettersGenDAL.getTemplate(_lType, _version);
            _temReaderstr = replaceNamespace(_temReaderstr);
            StringReader _temReader = new StringReader(_temReaderstr);
            List<string> _elList0 = new List<string>();
            string _exceptionEmpl = "";
            XmlDataDocument xmlDataDoc = new XmlDataDocument();
            int _genertedId = 0;
            try
            {
                dt = LettersGenDAL.getEmpwithallDepData();
                getFinalData(_temReader);
                string[] _letters = new string[dtFinal.Rows.Count];

                //get letters with all bookmarks replaced with correct values
                _letters = buildLetter(_temReaderstr);

                //get Identity for generated letter
                DataSet dsGen = new DataSet();
                dsGen.Clear();
                dsGen.Tables.Add(dt);
                if (!_preview)
                {
                    _genertedId = LettersGenDAL.StoreGeneratedLetter(_lType, _version, dsGen.GetXml());
                    _elList0 = LettersGenDAL.getEmpNumbers();
                    //store list of employees with letter ids for history purpose
                    LettersGenDAL.StoreLetterHistory(_genertedId, _elList0);
                }

                return _letters;
            }
            catch (Exception ex)
            {
                if (_genertedId != 0)
                {
                    LettersGenDAL.rollbackStoreLetter(_genertedId);
                }
                throw ex;
            }

        }

        public string[] generateLetter(string _empNums, string _lType, int _version, int _penid)
        {
            string _temReaderstr = LettersGenDAL.getTemplate(_lType, _version);
            _temReaderstr = replaceNamespace(_temReaderstr);
            StringReader _temReader = new StringReader(_temReaderstr);
            List<string> _elList0 = new List<string>();
            string _exceptionEmpl = "";
            XmlDataDocument xmlDataDoc = new XmlDataDocument();
            int _genertedId = 0;
            try
            {
                if (!_empNums.Equals(""))
                {
                    _elList0 = createEmpList(_empNums);
                    foreach (string _empl in _elList0)
                    {
                        PilotData pData = new PilotData();
                        bool _found = pData.checkExistRecord(Convert.ToInt32(_empl));
                        if (!_found)
                        {
                            _exceptionEmpl += _empl + ", ";
                        }
                        pData = null;
                    }
                    if (!_exceptionEmpl.Equals(""))
                    {
                        _exceptionEmpl = _exceptionEmpl.Remove(_exceptionEmpl.Length - 1);
                        throw (new Exception("Employee number(s) " + _exceptionEmpl + " not valid! "));
                    }
                }
                dt = LettersGenDAL.getEmpwithallDepData(_empNums);
                getFinalData(_temReader);
                string[] _letters = new string[dtFinal.Rows.Count];

                //get letters with all bookmarks replaced with correct values
                _letters = buildLetter(_temReaderstr);

                //get Identity for generated letter
                DataSet dsGen = new DataSet();
                dsGen.Clear();
                dsGen.Tables.Add(dt);
                _genertedId = LettersGenDAL.StoreGeneratedLetter(_lType, _version, dsGen.GetXml());

                //store list of employees with letter ids for history purpose
                LettersGenDAL.StoreLetterHistory(_genertedId, _elList0);

                //Update pending status
                LettersGenDAL.updatePending(_genertedId, _penid);

                return _letters;
            }
            catch (Exception ex)
            {
                if (_genertedId != 0)
                {
                    LettersGenDAL.rollbackStoreLetter(_genertedId);
                }
                throw ex;
            }

        }

        public string[] generateBenValidationLetter(int _empNum, string _dssn, string _lType, int _version, int _penid)
        {
            string _temReaderstr = LettersGenDAL.getTemplate(_lType, _version);
            _temReaderstr = replaceNamespace(_temReaderstr);
            StringReader _temReader = new StringReader(_temReaderstr); 
            XmlDataDocument xmlDataDoc = new XmlDataDocument();
            int _genertedId = 0;

            try
            {
                if (!_empNum.Equals(""))
                {                
                    PilotData pData = new PilotData();
                    bool _found = pData.checkExistRecord(_empNum);
                    if (!_found)
                    {
                        throw (new Exception("Employee number(s) " + _empNum + " not valid! "));
                    }
                    pData = null; 
                }

                dt = LettersGenDAL.getEmpDepData(_empNum, _dssn);
                getFinalData(_temReader);
                string[] _letters = new string[dtFinal.Rows.Count];

                //get letters with all bookmarks replaced with correct values
                _letters = buildLetter(_temReaderstr);

                //get Identity for generated letter
                DataSet dsGen = new DataSet();
                dsGen.Clear();
                dsGen.Tables.Add(dt);
                _genertedId = LettersGenDAL.StoreGeneratedLetter(_lType, _version, dsGen.GetXml());

                //store list of employees with letter ids for history purpose
                LettersGenDAL.StoreLetterHistory(_genertedId, _empNum);

                //Update pending status
                LettersGenDAL.updatePending(_genertedId, _penid);

                return _letters;
            }
            catch (Exception ex)
            {
                if (_genertedId != 0)
                {
                    LettersGenDAL.rollbackStoreLetter(_genertedId);
                }
                throw ex;                
            }
        }

        public string[] reprintLetters(int _templateId)
        {
            string _temReaderstr = LettersGenDAL.getTemplateGenerated(_templateId);
            _temReaderstr = replaceNamespace(_temReaderstr);
            StringReader _temReader = new StringReader(_temReaderstr);
            XmlDataDocument xmlDataDoc = new XmlDataDocument();
            DataSet dsreprint = new DataSet();
            dsreprint.Clear();
            dsreprint = LettersGenDAL.getGenDataset(_templateId);
            string _tblName = "";
            foreach (DataTable tble in dsreprint.Tables)
            {
                _tblName = tble.TableName;
            }
            dt = dsreprint.Tables[_tblName];
            getFinalData(_temReader);
            string[] _letters = new string[dtFinal.Rows.Count];

            //get letters with all bookmarks replaced with correct values
            _letters = buildLetter(_temReaderstr);

            return _letters;
        }

        public string[] reprintLetters(int _templateId, string _empnums)
        {
            string _temReaderstr = LettersGenDAL.getTemplateGenerated(_templateId);
            _temReaderstr = replaceNamespace(_temReaderstr);
            StringReader _temReader = new StringReader(_temReaderstr);
            XmlDataDocument xmlDataDoc = new XmlDataDocument();
            List<string> _elList0 = new List<string>();
            string _exceptionEmpl = "";

            try
            {
                if (!_empnums.Equals(""))
                {
                    _elList0 = createEmpList(_empnums);
                }

                DataSet dsreprint = new DataSet();
                dsreprint.Clear();
                dsreprint = LettersGenDAL.getGenDataset(_templateId);
                string _tblName = "";
                foreach (DataTable tble in dsreprint.Tables)
                {
                    _tblName = tble.TableName;
                }
                dt.Clear();

                dt = dsreprint.Tables[_tblName].Clone();

                foreach (string _emp in _elList0)
                {
                    foreach (DataRow dr in dsreprint.Tables[_tblName].Rows)
                    {
                        if(dr["EmployeeNo"].ToString().Equals(_emp))
                        {
                            DataRow r = dt.NewRow();
                            r.ItemArray = dr.ItemArray;
                            dt.Rows.Add(r);
                            break;
                        }
                    }
                }
                
                getFinalData(_temReader);
                string[] _letters = new string[dtFinal.Rows.Count];

                //get letters with all bookmarks replaced with correct values
                _letters = buildLetter(_temReaderstr);

                return _letters;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string[] buildLetter(String _tReader)
        {
           
            //Loading XML Data
            //XmlReader templateReader = XmlReader.Create(_tReader);

            string[] _lettersTemp = new string[dtFinal.Rows.Count];
            int i = 0;

            //string _letterReader = templateReader.ReadInnerXml();
            foreach (DataRow dr in dtFinal.Rows)
            {
                oXMLWordDoc.LoadXml(_tReader);
                foreach (DataColumn dc in dtFinal.Columns)
                {
                    ProcessNodes(dc.ColumnName, dr[dc].ToString());
                }
                StringWriter sw = new StringWriter();
                XmlTextWriter xw = new XmlTextWriter(sw);
                oXMLWordDoc.WriteTo(xw);
                _lettersTemp[i] = replaceIllegalXMLCharacters(sw.ToString());
                i++;
            }

            //templateReader.Close();
            return _lettersTemp;
        }

        public void ProcessNodes(string sNodeName, string sNodeValue)
        {
            //replace node(s) in document with value
            XmlNodeList oNodeList;

            
            //gets nodes that have embedded paragraph marks
            oNodeList = oXMLWordDoc.SelectNodes("//ns1:" + sNodeName + "//w:p", oNSMgr);

            if ((oNodeList != null))
            {
                FillNodes(oNodeList, sNodeValue);
            }
            //gets nodes that do NOT have
            //embedded paragraph marks
            oNodeList = oXMLWordDoc.SelectNodes("//ns1:" + sNodeName, oNSMgr);

            if ((oNodeList != null))
            {
                FillNodes(oNodeList, sNodeValue);
            }
        }

        private void FillNodes(XmlNodeList oNodeList, string sNodeValue)
        {
            int i;
            XmlNode oXMLNode;
            XmlNode oInnerNode;

            for (i = 0; i <= oNodeList.Count - 1; i++)
            {
                oXMLNode = oNodeList[i];
                oInnerNode = oXMLNode.SelectSingleNode("w:r/w:t", oNSMgr);
                if ((oInnerNode != null))
                {
                    oInnerNode.InnerText = sNodeValue;
                }
            }
        }

        public List<string> ParseBookMarkNodes(StringReader strUrl)
        {
            List<string> _try = new List<string>();
            try
            {
                XmlReader reader = XmlReader.Create(strUrl);
                while (reader.Read())
                {
                    if (reader.Name.StartsWith("ns1:") && reader.IsStartElement())
                        _try.Add(reader.Name.Substring(reader.Name.IndexOf(':') + 1));
                }
                reader.Close();
                return _try;
            }
            catch (XmlException ex)
            {
                throw ex;
            }
        }

        public List<string> createEmpList(string _list)
        {
            List<string> _elList1;
            if (_list.Contains(","))
            {
                _elList1 = new List<string>(_list.Split(','));
            }
            else
            {
                _elList1 = new List<string>();
                _elList1.Add(_list);
            }
            return _elList1;
        }

        public string createEmplCSV(List<string> _elList)
        {
            string _empList = ""; 
            foreach (string emp in _elList)
            {
                _empList += emp + ",";
            }
            _empList = _empList.Remove(_empList.Length - 1);  
            return _empList;
        }

        private void getFinalData(StringReader _cReader)
        {
            List<string> _columnNodes = new List<string>();
            _columnNodes = ParseBookMarkNodes(_cReader);            
            foreach (string _cN in _columnNodes)
            {
                if (!dtFinal.Columns.Contains(_cN))
                {
                    DataColumn dc = new DataColumn(_cN);
                    dtFinal.Columns.Add(dc);
                }
            }
            DataRow r;

            int _seq = 0;
            foreach (DataRow dr in dt.Rows)
            {
                _seq++;
                r = dtFinal.NewRow();

                foreach (string _cN in _columnNodes)
                {
                    if (_cN.Equals("father_mother") || _cN.Equals("fathers_mothers") ||
                                    _cN.Equals("his_her") || _cN.Equals("he_she"))
                    {
                        r[_cN] = getVariables(_cN, dr["Gender"].ToString());                        
                    }
                    else if (_cN.Equals("Print_Date"))
                    {
                        r[_cN] = DateTime.Now.ToShortDateString();
                    }
                    else if (_cN.Equals("Seq_Num"))
                    {
                        r[_cN] = _seq;
                    }
                    else
                    {
                        if (dt.Columns.Contains(_cN))
                        {
                            r[_cN] = dr[_cN];
                        }
                    }
                }
                if (_columnNodes.Count > 0)
                {
                    dtFinal.Rows.Add(r);
                }
            }            
        }

        private string getVariables(string _node, string _gender)
        {
            string _value = "";

            switch (_node)
            {
                case "father_mother":
                    if (_gender.Equals("M"))
                    {
                        _value = "father";
                    }
                    else if (_gender.Equals("F"))
                    {
                        _value = "mother";
                    }
                    break;
                case "fathers_mothers":
                    if (_gender.Equals("M"))
                    {
                        _value = "father's";
                    }
                    else if (_gender.Equals("F"))
                    {
                        _value = "mother's";
                    }
                    break;
                case "he_she":
                    if (_gender.Equals("M"))
                    {
                        _value = "he";
                    }
                    else if (_gender.Equals("F"))
                    {
                        _value = "she";
                    }
                    break;
                case "his_her":
                    if (_gender.Equals("M"))
                    {
                        _value = "his";
                    }
                    else if (_gender.Equals("F"))
                    {
                        _value = "her";
                    }
                    break;
            }

            return _value;
        }

        public static string Encode(string str)
        {
            byte[] encbuff = System.Text.Encoding.UTF8.GetBytes(str);
            return Convert.ToBase64String(encbuff);
        }
        public static string Decode(string str)
        {
            byte[] decbuff = Convert.FromBase64String(str);
            return System.Text.Encoding.UTF8.GetString(decbuff);
        }

        public static string replaceIllegalXMLCharacters(string _cnt)
        {
            _cnt = _cnt.Replace("“", "&#8220;");
            _cnt = _cnt.Replace("”", "&#8221;");
            _cnt = _cnt.Replace("‘", "&#8216;");
            _cnt = _cnt.Replace("’", "&#8217;");
            _cnt = _cnt.Replace("'", "&apos;");
            Regex rgx = new Regex(@", ss",RegexOptions.IgnoreCase);
            DisplayMatches(_cnt, @", ss");
            rgx.Replace(_cnt,"",1,1);
           // _cnt = _cnt.Replace("ss", "");
           _cnt = _cnt.Replace("Zip", "");
            return _cnt;
        }

        private static void DisplayMatches(string text, string regularExpressionString)
        {
           
            {
               // string txt = ", ss";

                string re1 = ".*?";	// 
                string re2 = "(ss)";	// Word 1

                Regex r = new Regex(re1 + re2, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                Match m = r.Match(text);
                if (m.Success)
                {
                    String word1 = m.Groups[1].ToString();
                    Console.Write("(" + word1.ToString() + ")" + "\n");
                }
                Console.ReadLine();
            }

        }


        public string replaceNamespace(string _cont)
        {
            string _pattern1 = @"xmlns\:ns\d{1,4}=";
            string _reppattern1 = @"xmlns:ns1=";

            string _pattern2 = @"ns\d{1,4}:";
            string _reppattern2 = "ns1:";

            Regex r1 = new Regex(_pattern1);
            string _finalContent = r1.Replace(_cont, _reppattern1);

            Regex r2 = new Regex(_pattern2);
            _finalContent = r2.Replace(_finalContent, _reppattern2);

            return _finalContent;
        }    
       
    }
}
