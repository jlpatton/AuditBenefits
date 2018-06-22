using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Xml;
using System.Text;
using System.Collections.Generic;
using EBA.Desktop;
using EBA.Desktop.HRA;
namespace EBA.Desktop.YEB
{
    /// <summary>
    /// Summary description for YEB_Reports
    /// </summary>
    public class YEB_ReportsDef
    {
        public YEB_ReportsDef()
        {
            // R.A. 5/16/2009
            // TODO: Add constructor logic here
            //
        }
        public static DataSet GetReportContentDS(string _source, string _pilotind, string _filename, string yrmo)
        {
            HRAExcelReport xlObj = new HRAExcelReport();
            DataSet ds = new DataSet(); ds.Clear();
            DataSet dsTemp = new DataSet(); dsTemp.Clear();
            string[] sheetnames, titles, YRMOs;
            string[][] colsFormat, colsName;
            int[][] colsWidth;
            string _content = "";
            DataSet results = new DataSet();
            XmlDocument doc = new XmlDocument();
            switch (_source)
            {
                case "HOME_YEB":
                    //Home Mailing List Non Pilots
                    sheetnames = new string[] { "Home_YEB_" + _pilotind + "_Detail" };
                    titles = new string[] { yrmo + "_Home_YEB_" + _pilotind + "  Date Report Run : " + DateTime.Now.ToString() };
                    colsName = new string[][] { new string[] { "YRMO", "PilotInd", "SourceCd", "TypeCd", "Name", "SSNO", "Addr1", "Addr2", "City", "State", "Zipcode", "EENO", "YebOnline", "ExpatFlag" } };
                    colsFormat = new string[][] { new string[] { "string", "string", "string", "string", "string", "string", "string", "string", "string", "string", "string", "number", "string", "string" } };
                    colsWidth = new int[][] { new int[] { 55, 140, 70, 60, 190, 65, 170, 60, 110, 40, 60, 60, 40, 40 } };

                    dsTemp = ImportYEBData.ProcessReportData("HOME_YEB", yrmo,_pilotind); dsTemp.Tables[0].TableName = "DetailTable";
                    if (dsTemp.Tables[0].Rows.Count != 0)
                    {
                        DataRow row;
                        row = dsTemp.Tables[0].NewRow();
                        row["Pilotind"] = "** Total Number of Records:";
                        row["Sourcecd"] = dsTemp.Tables[0].Compute("count([yrmo])", "yrmo = '" + yrmo + "'");
                        dsTemp.Tables[0].Rows.Add(row);
                    }

                    ds.Tables.Add(dsTemp.Tables[0].Copy()); ds.Tables[0].TableName = "DetailTableF"; dsTemp.Clear();
                    ////_content = xlObj.ExcelXMLRpt(ds, _filename, sheetnames, titles, colsName, colsFormat, colsWidth);
                    ////_content = LetterGen.replaceIllegalXMLCharacters(_content);
                    break;

                case "HOME_SAR_ONLY":
                    sheetnames = new string[] { "HOME_SAROnly_" + _pilotind + "Detail" };
                    titles = new string[] { yrmo + "_HOME_SAROnly_" + _pilotind + " Date Report Run : " + DateTime.Now.ToString() };
                    colsName = new string[][] { new string[] { "YRMO", "PilotInd", "SourceCd", "TypeCd", "Name", "SSNO", "Addr1", "Addr2", "City", "State", "Zipcode", "EENO", "YebOnline", "ExpatFlag" } };
                    colsFormat = new string[][] { new string[] { "string", "string", "string", "string", "string", "string", "string", "string", "string", "string", "string", "number", "string", "string" } };
                    colsWidth = new int[][] { new int[] { 55, 140, 70, 60, 190, 65, 170, 60, 110, 40, 60, 60, 40, 40 } };

                    dsTemp = ImportYEBData.ProcessReportData("HOME_SAR_ONLY", yrmo,_pilotind); dsTemp.Tables[0].TableName = "DetailTable";
                    if (dsTemp.Tables[0].Rows.Count != 0)
                    {
                        DataRow row;
                        row = dsTemp.Tables[0].NewRow();
                        row["Pilotind"] = "** Total Number of Records:";
                        row["Sourcecd"] = dsTemp.Tables[0].Compute("count([yrmo])", "yrmo = '" + yrmo + "'");
                        dsTemp.Tables[0].Rows.Add(row);
                    }

                    ds.Tables.Add(dsTemp.Tables[0].Copy()); ds.Tables[0].TableName = "DetailTableF"; dsTemp.Clear();
                    //_content = xlObj.ExcelXMLRpt(ds, _filename, sheetnames, titles, colsName, colsFormat, colsWidth);
                    //_content = LetterGen.replaceIllegalXMLCharacters(_content);
                    break;

                case "Active_YEB_Group1":
                    sheetnames = new string[] { "Active_YEB_Group1_" + _pilotind + "Detail" };
                    titles = new string[] { yrmo + "_Active_YEB_Group1_" + _pilotind + " Date Report Run : " + DateTime.Now.ToString() };
                    colsName = new string[][] { new string[] { "YRMO", "PilotInd", "SourceCd", "TypeCd", "Name", "SSNO", "Addr1", "Addr2", "City", "State", "Zipcode", "EENO", "YebOnline", "ExpatFlag" } };
                    colsFormat = new string[][] { new string[] { "string", "string", "string", "string", "string", "string", "string", "string", "string", "string", "string", "number", "string", "string" } };
                    colsWidth = new int[][] { new int[] { 55, 140, 70, 60, 190, 65, 170, 60, 110, 40, 60, 60, 40, 40 } };

                    dsTemp = ImportYEBData.ProcessReportData("Active_YEB_Group1", yrmo,_pilotind); dsTemp.Tables[0].TableName = "DetailTable";
                    if (dsTemp.Tables[0].Rows.Count != 0)
                    {
                        DataRow row;
                        row = dsTemp.Tables[0].NewRow();
                        row["Pilotind"] = "** Total Number of Records:";
                        row["Sourcecd"] = dsTemp.Tables[0].Compute("count([yrmo])", "yrmo = '" + yrmo + "'");
                        dsTemp.Tables[0].Rows.Add(row);
                    }
                    ds.Tables.Add(dsTemp.Tables[0].Copy()); ds.Tables[0].TableName = "DetailTableF"; dsTemp.Clear(); dsTemp.Dispose();
                    //_content = xlObj.ExcelXMLRpt(ds, _filename, sheetnames, titles, colsName, colsFormat, colsWidth);
                    ////_content = LetterGen.replaceIllegalXMLCharacters(_content);
                    
                    //doc.LoadXml(_content);
                    //Stream str;
                    //doc.Save(HttpContext.Current.Server.MapPath("~/uploads/") + "data.xml");
                    //results.ReadXml(HttpContext.Current.Server.MapPath("~/uploads/") + "data.xml");
                    break;
                case "Active_YEB_Group2":
                    sheetnames = new string[] { "Active_YEB_Group2_" + _pilotind + "Detail" };
                    titles = new string[] { yrmo + "_Active_YEB_Group2_" + _pilotind + " Date Report Run : " + DateTime.Now.ToString() };
                    colsName = new string[][] { new string[] { "YRMO", "PilotInd", "SourceCd", "TypeCd", "Name", "SSNO", "Addr1", "Addr2", "City", "State", "Zipcode", "EENO", "YebOnline", "ExpatFlag" } };
                    colsFormat = new string[][] { new string[] { "string", "string", "string", "string", "string", "string", "string", "string", "string", "string", "string", "number", "string", "string" } };
                    colsWidth = new int[][] { new int[] { 55, 140, 70, 60, 190, 65, 170, 60, 110, 40, 60, 60, 40, 40 } };

                    dsTemp = ImportYEBData.ProcessReportData("Active_YEB_Group2", yrmo,_pilotind); dsTemp.Tables[0].TableName = "DetailTable";
                    if (dsTemp.Tables[0].Rows.Count != 0)
                    {
                        DataRow row;
                        row = dsTemp.Tables[0].NewRow();
                        row["Pilotind"] = "** Total Number of Records:";
                        row["SourceCd"] = dsTemp.Tables[0].Compute("count([yrmo])", "yrmo = '" + yrmo + "'");
                        dsTemp.Tables[0].Rows.Add(row);
                    }
                    ds.Tables.Add(dsTemp.Tables[0].Copy()); ds.Tables[0].TableName = "DetailTableF"; dsTemp.Clear(); dsTemp.Dispose();
                    //_content = xlObj.ExcelXMLRpt(ds, _filename, sheetnames, titles, colsName, colsFormat, colsWidth);
                    //_content = LetterGen.replaceIllegalXMLCharacters(_content);
                
                    //XmlSanitizedString xmlsanit = new XmlSanitizedString(_content);
                    //doc.LoadXml(xmlsanit.ToString());
                    //doc.Save(HttpContext.Current.Server.MapPath("~/uploads/") + "data.xml");
                    //results.ReadXml(HttpContext.Current.Server.MapPath("~/uploads/") + "data.xml");
                    break;

            }
            return ds;
        }
        public static string GetReportContent(string _source,string _pilotind, string _filename, string yrmo)
        {
            HRAExcelReport xlObj = new HRAExcelReport();
            DataSet ds = new DataSet(); ds.Clear();
            DataSet dsTemp = new DataSet(); dsTemp.Clear();
            string[] sheetnames, titles, YRMOs;
            string[][] colsFormat, colsName;
            int[][] colsWidth;
            string _content = "";

            switch (_source)
            {
                case "HOME_YEB_RET":
                    //Home Mailing List Non Pilots
                    sheetnames = new string[] { "Home_YEB_RET" + _pilotind + "_Detail" };
                    titles = new string[] { yrmo + "_Home_YEB_RET" + _pilotind + "  Date Report Run : " + DateTime.Now.ToString() };
                    colsName = new string[][] { new string[] { "YRMO", "PilotInd", "SourceCd", "TypeCd", "Name", "SSNO", "Addr1", "Addr2", "City", "State", "Zipcode", "EENO", "YebOnline", "ExpatFlag" } };
                    colsFormat = new string[][] { new string[] { "string", "string", "string", "string", "string", "string", "string", "string", "string", "string", "string", "number", "string", "string" } };
                    colsWidth = new int[][] { new int[] { 55, 140, 70, 60, 190, 65, 170, 60, 110, 40, 60, 60, 40, 40 } };

                    dsTemp = ImportYEBData.ProcessReportData("HOME_YEB_RET", yrmo,_pilotind); dsTemp.Tables[0].TableName = "DetailTable";
                    if (dsTemp.Tables[0].Rows.Count != 0)
                    {
                        DataRow row;
                        row = dsTemp.Tables[0].NewRow();
                        row["Pilotind"] = "** Total Number of Records:";
                        row["Sourcecd"] = dsTemp.Tables[0].Compute("count([yrmo])", "yrmo = '" + yrmo + "'");
                        dsTemp.Tables[0].Rows.Add(row);
                    }

                    ds.Tables.Add(dsTemp.Tables[0].Copy()); ds.Tables[0].TableName = "DetailTableF"; dsTemp.Clear();
                    _content = xlObj.ExcelXMLRpt(ds, _filename, sheetnames, titles, colsName, colsFormat, colsWidth);
                    _content = LetterGen.replaceIllegalXMLCharacters(_content);
                    break;

                case "HOME_YEB_NONRET":
                    //Home Mailing List Non Pilots
                    sheetnames = new string[] { "Home_YEB_NONRET" + _pilotind + "_Detail" };
                    titles = new string[] { yrmo + "_Home_YEB_NONRET" + _pilotind + "  Date Report Run : " + DateTime.Now.ToString() };
                    colsName = new string[][] { new string[] { "YRMO", "PilotInd", "SourceCd", "TypeCd", "Name", "SSNO", "Addr1", "Addr2", "City", "State", "Zipcode", "EENO", "YebOnline", "ExpatFlag" } };
                    colsFormat = new string[][] { new string[] { "string", "string", "string", "string", "string", "string", "string", "string", "string", "string", "string", "number", "string", "string" } };
                    colsWidth = new int[][] { new int[] { 55, 140, 70, 60, 190, 65, 170, 60, 110, 40, 60, 60, 40, 40 } };

                    dsTemp = ImportYEBData.ProcessReportData("HOME_YEB_NONRET", yrmo, _pilotind); dsTemp.Tables[0].TableName = "DetailTable";
                    if (dsTemp.Tables[0].Rows.Count != 0)
                    {
                        DataRow row;
                        row = dsTemp.Tables[0].NewRow();
                        row["Pilotind"] = "** Total Number of Records:";
                        row["Sourcecd"] = dsTemp.Tables[0].Compute("count([yrmo])", "yrmo = '" + yrmo + "'");
                        dsTemp.Tables[0].Rows.Add(row);
                    }

                    ds.Tables.Add(dsTemp.Tables[0].Copy()); ds.Tables[0].TableName = "DetailTableF"; dsTemp.Clear();
                    _content = xlObj.ExcelXMLRpt(ds, _filename, sheetnames, titles, colsName, colsFormat, colsWidth);
                    _content = LetterGen.replaceIllegalXMLCharacters(_content);
                    break;

                case "HOME_SAR_ONLY":
                    sheetnames = new string[] { "HOME_SAROnly_" + _pilotind + "Detail" };
                    titles = new string[] { yrmo +"_HOME_SAROnly_" + _pilotind + " Date Report Run : " + DateTime.Now.ToString() };
                    colsName = new string[][] { new string[] { "YRMO", "PilotInd", "SourceCd", "TypeCd", "Name", "SSNO", "Addr1", "Addr2", "City", "State", "Zipcode", "EENO", "YebOnline", "ExpatFlag" } };
                    colsFormat = new string[][] { new string[] { "string", "string", "string", "string", "string", "string", "string", "string", "string", "string", "string", "number", "string", "string" } };
                    colsWidth = new int[][] { new int[] { 55, 140, 70, 60, 190, 65, 170, 60, 110, 40, 60, 60, 40, 40 } };

                    dsTemp = ImportYEBData.ProcessReportData("HOME_SAR_ONLY", yrmo,_pilotind); dsTemp.Tables[0].TableName = "DetailTable";
                    if (dsTemp.Tables[0].Rows.Count != 0)
                    {
                        DataRow row;
                        row = dsTemp.Tables[0].NewRow();
                        row["Pilotind"] = "** Total Number of Records:";
                        row["Sourcecd"] = dsTemp.Tables[0].Compute("count([yrmo])", "yrmo = '" + yrmo + "'");
                        dsTemp.Tables[0].Rows.Add(row);
                    }

                    ds.Tables.Add(dsTemp.Tables[0].Copy()); ds.Tables[0].TableName = "DetailTableF"; dsTemp.Clear();
                   _content = xlObj.ExcelXMLRpt(ds, _filename, sheetnames, titles, colsName, colsFormat, colsWidth);
                    _content = LetterGen.replaceIllegalXMLCharacters(_content);
                    break;

                case "Active_YEB_Group1":
                    sheetnames = new string[] { "Active_YEB_Group1_" + _pilotind + "Detail" };
                    titles = new string[] { yrmo + "_Active_YEB_Group1_" + _pilotind + " Date Report Run : " + DateTime.Now.ToString() };
                    colsName = new string[][] { new string[] { "YRMO", "PilotInd", "SourceCd", "TypeCd", "Name", "SSNO", "Addr1", "Addr2", "City", "State", "Zipcode", "EENO", "YebOnline", "ExpatFlag" } };
                    colsFormat = new string[][] { new string[] { "string", "string", "string", "string", "string", "string", "string", "string", "string", "string", "string", "number", "string", "string" } };
                    colsWidth = new int[][] { new int[] { 55, 140, 70, 60, 190, 65, 170, 60, 110, 40, 60, 60, 40, 40 } };

                    dsTemp = ImportYEBData.ProcessReportData("Active_YEB_Group1", yrmo,_pilotind); dsTemp.Tables[0].TableName = "DetailTable";
                    if (dsTemp.Tables[0].Rows.Count != 0)
                    {
                        DataRow row;
                        row = dsTemp.Tables[0].NewRow();
                        row["Pilotind"] = "** Total Number of Records:";
                        row["Sourcecd"] = dsTemp.Tables[0].Compute("count([yrmo])", "yrmo = '" + yrmo + "'");
                        dsTemp.Tables[0].Rows.Add(row);
                    }
                    ds.Tables.Add(dsTemp.Tables[0].Copy()); ds.Tables[0].TableName = "DetailTableF"; dsTemp.Clear(); dsTemp.Dispose();
                    _content = xlObj.ExcelXMLRpt(ds, _filename, sheetnames, titles, colsName, colsFormat, colsWidth);
                    _content = LetterGen.replaceIllegalXMLCharacters(_content);
                    break;

                case "Active_YEB_Group2":
                    sheetnames = new string[] { "Active_YEB_Group2_" + _pilotind + "Detail" };
                    titles = new string[] { yrmo + "_Active_YEB_Group2_" + _pilotind + " Date Report Run : " + DateTime.Now.ToString() };
                    colsName = new string[][] { new string[] { "YRMO", "PilotInd", "SourceCd", "TypeCd", "Name", "SSNO", "Addr1", "Addr2", "City", "State", "Zipcode", "EENO", "YebOnline", "ExpatFlag" } };
                    colsFormat = new string[][] { new string[] { "string", "string", "string", "string", "string", "string", "string", "string", "string", "string", "string", "number", "string", "string" } };
                    colsWidth = new int[][] { new int[] { 55, 140, 70, 60, 190, 65, 170, 60, 110, 40, 60, 60, 40, 40 } };

                    dsTemp = ImportYEBData.ProcessReportData("Active_YEB_Group2", yrmo,_pilotind); dsTemp.Tables[0].TableName = "DetailTable";
                    if (dsTemp.Tables[0].Rows.Count != 0)
                    {
                        DataRow row;
                        row = dsTemp.Tables[0].NewRow();
                        row["Pilotind"] = "** Total Number of Records:";
                        row["SourceCd"] = dsTemp.Tables[0].Compute("count([yrmo])", "yrmo = '" + yrmo + "'");
                        dsTemp.Tables[0].Rows.Add(row);
                    }
                    ds.Tables.Add(dsTemp.Tables[0].Copy()); ds.Tables[0].TableName = "DetailTableF"; dsTemp.Clear(); dsTemp.Dispose();
                    _content = xlObj.ExcelXMLRpt(ds, _filename, sheetnames, titles, colsName, colsFormat, colsWidth);
                    _content = LetterGen.replaceIllegalXMLCharacters(_content);
                    break;


                case "Active_SAROnly":
                    sheetnames = new string[] { "Active_SAROnly_" + _pilotind + "Detail" };
                    titles = new string[] { yrmo + "_Active_SARonly_" + _pilotind + " Date Report Run : " + DateTime.Now.ToString() };
                    colsName = new string[][] { new string[] { "YRMO", "PilotInd", "SourceCd", "TypeCd", "Name", "SSNO", "Addr1", "Addr2", "City", "State", "Zipcode", "EENO", "YebOnline", "ExpatFlag" } };
                    colsFormat = new string[][] { new string[] { "string", "string", "string", "string", "string", "string", "string", "string", "string", "string", "string", "number", "string", "string" } };
                    colsWidth = new int[][] { new int[] { 55, 140, 70, 60, 190, 65, 170, 60, 110, 40, 60, 60, 40, 40 } };

                    dsTemp = ImportYEBData.ProcessReportData("Active_SAROnly", yrmo,_pilotind); dsTemp.Tables[0].TableName = "DetailTable";
                    if (dsTemp.Tables[0].Rows.Count != 0)
                    {
                        DataRow row;
                        row = dsTemp.Tables[0].NewRow();
                        row["Pilotind"] = "** Total Number of Records:";
                        row["Sourcecd"] = dsTemp.Tables[0].Compute("count([yrmo])", "yrmo = '" + yrmo + "'");
                        dsTemp.Tables[0].Rows.Add(row);
                    }
                    ds.Tables.Add(dsTemp.Tables[0].Copy()); ds.Tables[0].TableName = "DetailTableF"; dsTemp.Clear();
                   _content = xlObj.ExcelXMLRpt(ds, _filename, sheetnames, titles, colsName, colsFormat, colsWidth);
                    _content = LetterGen.replaceIllegalXMLCharacters(_content);
                    break;
                case "Active_PROnly":
                    sheetnames = new string[] { "Active_YEB_" + _pilotind + "PRonly"  + "Detail" };
                    titles = new string[] { yrmo + "_Active_YEB_" + _pilotind + "PRonly"  + " Date Report Run : " + DateTime.Now.ToString() };
                    colsName = new string[][] { new string[] { "YRMO", "PilotInd", "SourceCd", "TypeCd", "Name", "SSNO", "Addr1", "Addr2", "City", "State", "Zipcode", "EENO", "YebOnline", "ExpatFlag" } };
                    colsFormat = new string[][] { new string[] { "string", "string", "string", "string", "string", "string", "string", "string", "string", "string", "string", "number", "string", "string" } };
                    colsWidth = new int[][] { new int[] { 55, 140, 70, 60, 190, 65, 170, 60, 110, 40, 60, 60, 40, 40 } };

                    dsTemp = ImportYEBData.ProcessReportData("Active_PROnly", yrmo,_pilotind); dsTemp.Tables[0].TableName = "DetailTable";
                    if (dsTemp.Tables[0].Rows.Count != 0)
                    {
                        DataRow row;
                        row = dsTemp.Tables[0].NewRow();
                        row["Pilotind"] = "** Total Number of Records:";
                        row["Sourcecd"] = dsTemp.Tables[0].Compute("count([yrmo])", "yrmo = '" + yrmo + "'");
                        dsTemp.Tables[0].Rows.Add(row);
                    }
                    ds.Tables.Add(dsTemp.Tables[0].Copy()); ds.Tables[0].TableName = "DetailTableF"; dsTemp.Clear();
                    _content = xlObj.ExcelXMLRpt(ds, _filename, sheetnames, titles, colsName, colsFormat, colsWidth);
                    _content = LetterGen.replaceIllegalXMLCharacters(_content);
                    break;
                case "DUP_SSN":
                    sheetnames = new string[] { "DUPLICATES_SSN_ONLY_" + _pilotind + "Detail" };
                    titles = new string[] { yrmo + "_SSNDuplicates_YEB_" + _pilotind  + " Date Report Run : " + DateTime.Now.ToString() };
                    colsName = new string[][] { new string[] { "Rectype","YRMO", "PilotInd", "SourceCd", "TypeCd", "Name", "SSNO", "Addr1", "Addr2", "City", "State", "Zipcode", "EENO"} };
                    colsFormat = new string[][] { new string[] { "string","string", "string", "string", "string", "string", "string", "string", "string", "string", "string", "string", "number"} };
                    colsWidth = new int[][] { new int[] { 65,55, 140, 70, 60, 190, 65, 170, 60, 110, 40, 60, 60 } };

                    dsTemp = ImportYEBData.ProcessReportData("DUP_SSN", yrmo,_pilotind); dsTemp.Tables[0].TableName = "DetailTable";
                   if (dsTemp.Tables[0].Rows.Count != 0)
                    {
                        DataRow row;
                        row = dsTemp.Tables[0].NewRow();
                        row["Pilotind"] = "** Total Number of Records:";
                        row["Sourcecd"] = dsTemp.Tables[0].Compute("count([yrmo])", "yrmo = '" + yrmo + "'");
                        dsTemp.Tables[0].Rows.Add(row);
                    }
                    ds.Tables.Add(dsTemp.Tables[0].Copy()); ds.Tables[0].TableName = "DetailTableF"; dsTemp.Clear();
                    _content = xlObj.ExcelXMLRpt(ds, _filename, sheetnames, titles, colsName, colsFormat, colsWidth);
                    _content = LetterGen.replaceIllegalXMLCharacters(_content);
                    break;
                case "DUP_ADDR1":
                    sheetnames = new string[] { "DUPLICATES_ADDR1_ONLY_" + _pilotind + "Detail" };
                    titles = new string[] { yrmo + "_ADDR1Duplicates_YEB_" + _pilotind + " Date Report Run : " + DateTime.Now.ToString() };
                    colsName = new string[][] { new string[] { "Rectype", "YRMO", "PilotInd", "SourceCd", "TypeCd", "Name", "SSNO", "Addr1", "Addr2", "City", "State", "Zipcode", "EENO" } };
                    colsFormat = new string[][] { new string[] { "string", "string", "string", "string", "string", "string", "string", "string", "string", "string", "string", "string", "number" } };
                    colsWidth = new int[][] { new int[] { 65, 55, 140, 70, 60, 190, 65, 170, 60, 110, 40, 60, 60 } };

                    dsTemp = ImportYEBData.ProcessReportData("DUP_ADDR1", yrmo,_pilotind); dsTemp.Tables[0].TableName = "DetailTable";
                    if (dsTemp.Tables[0].Rows.Count != 0)
                    {
                        DataRow row;
                        row = dsTemp.Tables[0].NewRow();
                        row["Pilotind"] = "** Total Number of Records:";
                        row["Sourcecd"] = dsTemp.Tables[0].Compute("count([yrmo])", "yrmo = '" + yrmo + "'");
                        dsTemp.Tables[0].Rows.Add(row);
                    }
                    ds.Tables.Add(dsTemp.Tables[0].Copy()); ds.Tables[0].TableName = "DetailTableF"; dsTemp.Clear();
                    _content = xlObj.ExcelXMLRpt(ds, _filename, sheetnames, titles, colsName, colsFormat, colsWidth);
                    _content = LetterGen.replaceIllegalXMLCharacters(_content);
                    break;
            }

            return _content;
        }

        public static string GetFileName(string source,string pilotind, string yrmo)
        {
            string _filename = "";

            switch (source)
            {
                case "HOME_YEB_RET":
                    _filename = yrmo + "_HOME_YEB_RET" + pilotind + ".xls";
                    break;
                case "HOME_YEB_NONRET":
                    _filename = yrmo + "_HOME_YEB_NONRET" + pilotind + ".xls";
                    break;
                case "HOME_SAR_ONLY":
                    _filename = yrmo + "_HOME_SAROnly_" + pilotind + ".xls";
                    break;
                case "Active_YEB_Group1":
                    _filename = yrmo+ "_Active_YEB_" +  pilotind + "_Group1"  + ".xls";
                    break;
                case "Active_YEB_Group2":
                    _filename = yrmo + "_Active_YEB_" + pilotind + "_Group2" + ".xls";
                    break;
                case "Active_SAROnly":
                    _filename = yrmo + "_Active_SAROnly_" + pilotind + ".xls";
                    break;
                case "Active_PROnly":
                    _filename = yrmo + "_Active_" + pilotind + "_PROnly_" + ".xls";
                    break;
                case "DUP_SSN":
                    _filename = yrmo + "_DUPLICATE_SSN_" + pilotind + ".xls";
                    break;
                case "DUP_ADDR1":
                    _filename = yrmo +"_DUPLICATE_ADDR1_" + pilotind + ".xls";
                    break; 
            }

            return _filename;
        }

        public static string[] GetReportCodes(string source)
        {
            string[] _reports;

            switch (source)
            {
                case "Reconciliation":
                    _reports = new string[] { "Recon", "CF", "CFnotCleared", "Transaction" };
                    break;
                case "SOFO":
                    _reports = new string[] { "SOFO" };
                    break;
                case "Admin Bill Validation":
                    _reports = new string[] { "AdminRecon", "AdminDispAP", "AdminDispPA", "AdminDispWP", "AdminDispPW", "AdminWPnoBal" };
                    break;
                default:
                    _reports = new string[] { "Elig", "EligTermAudit", "EligAudit", "EligAddrChg", "EligStatChg", "EligNoBen", "EligBen24", "EligNoBenOwner", "EligChildBenLtr", "EligDeathsXdays" };
                    break;
            }

            return _reports;
        }

        public static void GenerateReport(string _source,string _pilotind, string _yrmo)
        {
            string _filename, file_extn, _content, _contenttype;
            DataSet dsresult;
            _filename = GetFileName(_source,_pilotind, _yrmo);
            file_extn = _filename.Substring(_filename.Length - 4);
            _content = GetReportContent(_source, _pilotind, _filename, _yrmo);
            if (file_extn.Equals(".xls")) _contenttype = "application/vnd.ms-excel";
            else _contenttype = "text/plain";

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Charset = "";
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=" + _filename);
            HttpContext.Current.Response.ContentType = _contenttype;
            HttpContext.Current.Response.Write(_content);
            HttpContext.Current.Response.End();
            /// Use this to reduce the sze of the file if the dataset returned is large
         //   if ((_filename== _yrmo+ "_Active_YEB_" +  _pilotind + "_Group1"  + ".xls") || (_filename== _yrmo+ "_Active_YEB_" +  _pilotind + "_Group2"  + ".xls"))
         ////Display reports using the html write method to overcome the Outof Memory exception
         //   {
         //   dsresult = GetReportContentDS(_source, _pilotind, _filename, _yrmo);
         //   HttpContext.Current.Response.Clear();
         //   HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + _filename + ".xls");
         //   HttpContext.Current.Response.Charset = "";
         //   //HttpContext.Current.EnableViewState = false;
         //   HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
         //   HttpContext.Current.Response.ContentType = "application/vnd.xls";

         //   System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(HttpContext.Current.Response.Output);

         //   DataGrid myDataGrid = new DataGrid();
         //   myDataGrid.ShowHeader = true;
         //   myDataGrid.DataSource = dsresult.Tables[0];
         //   myDataGrid.DataBind();
         //   myDataGrid.RenderControl(htmlWrite);
         //   HttpContext.Current.Response.End();
         //   }
         //       //Show reports as regular 6-25-2009
         //   else
         //   {
         //   _content = GetReportContent(_source, _pilotind, _filename, _yrmo);
         //   if (file_extn.Equals(".xls")) _contenttype = "application/vnd.ms-excel";
         //   else _contenttype = "text/plain";

         //   HttpContext.Current.Response.Clear();
         //   HttpContext.Current.Response.Charset = "";
         //   HttpContext.Current.Response.ClearContent();
         //   HttpContext.Current.Response.ClearHeaders();
         //   HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
         //   HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=" + _filename);
         //   HttpContext.Current.Response.ContentType = _contenttype;
         //   HttpContext.Current.Response.Write(_content);
         //   HttpContext.Current.Response.End();
            
         //   }
        }

        public static void SavePrintFiles(string _category, string yrmo)
        {
            string[] reports = GetReportCodes(_category);
            Page page = HttpContext.Current.Handler as Page;
            string _content, _file, _filepath, _filename;
            Boolean _checkedPrint, _checkedSave;
            int _seq = 0;
            StreamWriter sw;

            foreach (string report in reports)
            {
                _checkedPrint = HRA_ReconDAL.checkedPrint(_category, report);
                _checkedSave = HRA_ReconDAL.checkedSave(_category, report);

                if (!_checkedSave && !_checkedPrint)
                    continue;

                _filename = GetFileName(report,"", yrmo);
                _content = GetReportContent(report,"", _filename, yrmo);
                _filepath = HRA_ReconDAL.GetFilePath(_category, report);

                _file = _filepath + _filename;

                if (_checkedSave || _checkedPrint)
                {
                    if (report.Equals("Elig"))
                    {
                        _content = _content.Replace("'", "\\'");
                        if (!page.ClientScript.IsClientScriptBlockRegistered(_seq + "SaveTextFile"))
                            page.ClientScript.RegisterClientScriptBlock(typeof(Page), _seq + "SaveTextFile", "SaveTextFile('" + _content.Replace(Environment.NewLine, "\\n") + "', '" + _file.Replace("\\", "\\\\") + "');", true);
                    }
                    else
                    {
                        if (!page.ClientScript.IsClientScriptBlockRegistered(_seq + "SaveFile"))
                            page.ClientScript.RegisterClientScriptBlock(typeof(Page), _seq + "SaveFile", "SaveFile('" + _content.Replace(Environment.NewLine, "") + "', '" + _file.Replace("\\", "\\\\") + "');", true);
                    }
                }

                if (_checkedPrint)
                {
                    if (report.Equals("Elig"))
                    {
                        if (!page.ClientScript.IsClientScriptBlockRegistered(_seq + "PrintWord"))
                            page.ClientScript.RegisterClientScriptBlock(typeof(Page), _seq + "PrintWord", "PrintWord('" + _file.Replace("\\", "\\\\") + "');", true);
                    }
                    else
                    {
                        if (!page.ClientScript.IsClientScriptBlockRegistered(_seq + "PrintExcel"))
                            page.ClientScript.RegisterClientScriptBlock(typeof(Page), _seq + "PrintExcel", "PrintExcel('" + _file.Replace("\\", "\\\\") + "');", true);
                    }
                    if (!_checkedSave)
                    {
                        if (!page.ClientScript.IsClientScriptBlockRegistered(_seq + "DeleteFile"))
                            page.ClientScript.RegisterClientScriptBlock(typeof(Page), _seq + "DeleteFile", "DeleteFile('" + _file.Replace("\\", "\\\\") + "');", true);
                    }
                }

                _seq++;
            }
        }

    }

    public class XmlSanitizedString
    {
        private readonly string value;

        public XmlSanitizedString(string s)
        {
            this.value = XmlSanitizedString.SanitizeXmlString(s);
        }

        /// <summary>
        /// Get the XML-santizied string.
        /// </summary>
        public override string ToString()
        {
            return this.value;
        }

        /// <summary>
        /// Remove illegal XML characters from a string.
        /// </summary>
        private static string SanitizeXmlString(string xml)
        {
            if (string.IsNullOrEmpty(xml))
            {
                return xml;
            }

            StringBuilder buffer = new StringBuilder(xml.Length);

            foreach (char c in xml)
            {
                if (XmlSanitizedString.IsLegalXmlChar(c))
                {
                    buffer.Append(c);
                }
            }

            return buffer.ToString();
        }

        /// <summary>
        /// Whether a given character is allowed by XML 1.0.
        /// </summary>
        private static bool IsLegalXmlChar(int character)
        {
            return
            (
                 character == 0x9 /* == '\t' == 9   */        ||
                 character == 0xA /* == '\n' == 10  */        ||
                 character == 0xD /* == '\r' == 13  */        ||
                (character >= 0x20 && character <= 0xD7FF) ||
                (character >= 0xE000 && character <= 0xFFFD) ||
                (character >= 0x10000 && character <= 0x10FFFF)
            );
        }
    }
}
    
