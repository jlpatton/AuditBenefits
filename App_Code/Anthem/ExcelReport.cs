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
using System.Collections.Generic;

/// <summary>
/// Summary description for ExcelReport
/// </summary>
/// 

namespace EBA.Desktop
{
    public class ExcelReport
    {       
        public ExcelReport()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public static void ExcelXMLRpt(DataSet dsBook, string filename, string[] sheetname, string[] titles, string[][] colsNames, string[][] colsFormat)
        {
            StringWriter sw = new StringWriter();           
           
            sw.WriteLine("<?xml version=\"1.0\"?>");
            sw.WriteLine("<?mso-application progid=\"Excel.Sheet\"?>");
            sw.WriteLine("<Workbook xmlns=\"urn:schemas-microsoft-com:office:spreadsheet\"");
            sw.WriteLine(" xmlns:o=\"urn:schemas-microsoft-com:office:office\"");
            sw.WriteLine(" xmlns:x=\"urn:schemas-microsoft-com:office:excel\"");
            sw.WriteLine(" xmlns:ss=\"urn:schemas-microsoft-com:office:spreadsheet\"");
            sw.WriteLine(" xmlns:html=\"http://www.w3.org/TR/REC-html40/\">");  
          
            sw.Write(xlDocProp().ToString());

            sw.WriteLine(" <ExcelWorkbook xmlns=\"urn:schemas-microsoft-com:office:excel\">");
            sw.WriteLine("  <WindowHeight>8955</WindowHeight>");
            sw.WriteLine("  <WindowWidth>11355</WindowWidth>");
            sw.WriteLine("  <WindowTopX>480</WindowTopX>");
            sw.WriteLine("  <WindowTopY>15</WindowTopY>");
            sw.WriteLine("  <ProtectStructure>False</ProtectStructure>");
            sw.WriteLine("  <ProtectWindows>False</ProtectWindows>");
            sw.WriteLine(" </ExcelWorkbook>");

            sw.Write(xlStyles().ToString());    
            sw.Write(AddWorksheets(sheetname, titles, colsNames, colsFormat, dsBook));

            sw.WriteLine("</Workbook>");
            sw.Close();
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Charset = "";
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=" + filename + ".xls");
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            HttpContext.Current.Response.Write(sw.ToString());
            HttpContext.Current.Response.End();
        }
        public static StringWriter xlDocProp()
        {
            StringWriter sw = new StringWriter();
            
            sw.WriteLine(" <DocumentProperties xmlns=\"urn:schemas-microsoft-com:office:office\">;");
            sw.WriteLine("  <Author>EBS</Author>");
            sw.WriteLine(string.Format("  <Created>{0}T</Created>", DateTime.Now.ToString("yyyy-mm-dd")));
            sw.WriteLine("  <Company>Acme</Company>");
            sw.WriteLine("  <Version>1.2</Version>");
            sw.WriteLine(" </DocumentProperties>");

            return sw;
        }
        public static StringWriter xlStyles()
        {
            StringWriter sw = new StringWriter();

            sw.WriteLine(" <Styles>");
            sw.WriteLine("  <Style ss:ID=\"Default\" ss:Name=\"Normal\">");
            sw.WriteLine("   <Alignment ss:Vertical=\"Bottom\"/>");
            sw.WriteLine("   <Borders/>");
            sw.WriteLine("   <Font/>");
            sw.WriteLine("   <Interior/>");
            sw.WriteLine("   <Protection/>");
            sw.WriteLine("  </Style>");
            sw.WriteLine("  <Style ss:ID=\"s21\">");
            sw.WriteLine("   <Alignment ss:Horizontal=\"Center\" ss:Vertical=\"Bottom\" ss:WrapText=\"0\"/>");
            sw.WriteLine("  </Style>");
            sw.WriteLine("<Style ss:ID=\"s22\">");
            sw.WriteLine("<Font x:Family=\"Swiss\" ss:Bold=\"1\" ss:Color=\"#FFFFFF\" />");
            sw.WriteLine("   <Alignment ss:Horizontal=\"Center\" ss:Vertical=\"Center\" ss:WrapText=\"1\"/>");
            sw.WriteLine("<Interior ss:Color=\"#969696\" ss:Pattern=\"Solid\"/>");
            sw.WriteLine("</Style>");
            sw.WriteLine("<Style ss:ID=\"s23\">");
            sw.WriteLine("<ss:Font x:Family=\"Swiss\" ss:Size=\"12\" ss:Bold=\"1\" ss:Color=\"#5D478B\"/>");
            sw.WriteLine("</Style>");
            sw.WriteLine("<Style ss:ID=\"s30\">");
            sw.WriteLine("<ss:Font x:Family=\"Swiss\" ss:Size=\"10\" ss:Bold=\"1\" ss:Color=\"#5D478B\"/>");
            sw.WriteLine("</Style>");
            sw.WriteLine("<Style ss:ID=\"s24\">");
            sw.WriteLine("<ss:Font x:Family=\"Swiss\" ss:Bold=\"1\" ss:Color=\"#333399\"/>");
            sw.WriteLine("<Alignment ss:Horizontal=\"Left\"/>");
            sw.WriteLine("</Style>");
            sw.WriteLine("<Style ss:ID=\"s25\">");
            sw.WriteLine("<NumberFormat ss:Format=\"#,##0\"/>");
            sw.WriteLine("</Style>");
            sw.WriteLine("<Style ss:ID=\"s26\">");
            sw.WriteLine("<NumberFormat ss:Format=\"#,##0.00\"/>");
            sw.WriteLine("</Style>");
            sw.WriteLine("<Style ss:ID=\"s27\">");
            sw.WriteLine("<ss:Font ss:Bold=\"1\"/>");
            sw.WriteLine("<NumberFormat ss:Format=\"#,##0\"/>");
            sw.WriteLine("</Style>");
            sw.WriteLine("<Style ss:ID=\"s28\">");
            sw.WriteLine("<ss:Font ss:Bold=\"1\"/>");
            sw.WriteLine("<NumberFormat ss:Format=\"#,##0.00\"/>");
            sw.WriteLine("</Style>");
            sw.WriteLine("  <Style ss:ID=\"s29\">");
            sw.WriteLine("<ss:Font ss:Bold=\"1\" />");
            sw.WriteLine("   <Alignment ss:Horizontal=\"Center\" ss:Vertical=\"Bottom\" ss:WrapText=\"0\"/>");
            sw.WriteLine("  </Style>");
            sw.WriteLine(" </Styles>");

            return sw;
        }
        public static StringWriter AddWorksheets(String[] sheetname, String[] titles, String[][] colsNames, String[][] colsFormat, DataSet dsBook)
        {
            String[][] colsFormatXml1 = getcolsFormatXML(colsFormat);
            String[][] colsFormatXmlBold1 = getcolsFormatXMLBold(colsFormat);

            int index1, index2,index3, index4, index5;           
            bool total = false;
            StringWriter sw = new StringWriter();
            sw.WriteLine(" <Worksheet ss:Name= \"" + sheetname[0] + "\">");

            int rows = dsBook.Tables[0].Rows.Count + 4;
            int cols = dsBook.Tables[0].Columns.Count;
            sw.WriteLine(string.Format("  <Table ss:ExpandedColumnCount=\"{0}\" x:FullColumns=\"1\"", cols.ToString()));
            sw.WriteLine("   x:FullRows=\"1\">");

            //adjust column widths    
            foreach (DataColumn eachColumn in dsBook.Tables[0].Columns)
            {
                sw.WriteLine("<Column ss:AutoFitWidth=\"0\" ss:Width=\"90\"/>");
            }

            //generate title
            sw.WriteLine("<Row ss:Index=\"2\">");
            sw.WriteLine("<Cell ss:StyleID=\"s23\">");
            sw.WriteLine("<Data ss:Type=\"String\">" + titles[0] + "</Data>");
            sw.WriteLine("</Cell>");
            sw.WriteLine("</Row>");
            sw.WriteLine("<Row ss:Index=\"4\">");

            //generate column headings
            int i = 0;
            foreach (DataColumn eachColumn in dsBook.Tables[0].Columns)
            {
                eachColumn.ColumnName = colsNames[0][i];
                sw.Write("<Cell ss:StyleID=\"s22\"><Data ss:Type=\"String\">");
                sw.Write(eachColumn.ColumnName.ToString());
                sw.WriteLine("</Data></Cell>");
                i++;
            }
            sw.WriteLine("</Row>");
            
            //generate data
            index1 = 5;
            
            foreach (DataRow eachRow in dsBook.Tables[0].Rows)
            {
                total = false;
                sw.WriteLine("<Row ss:Index=\"" + index1 + "\">");
                for (int currentRow = 0; currentRow != cols; currentRow++)
                {
                    if (eachRow[currentRow].ToString().Contains("Total:"))
                    {                        
                        sw.Write("<Cell ss:StyleID=\"s24\"><Data ss:Type=\"String\">");
                        index1++;
                        total = true;
                    }
                    else
                    {
                        if (total)
                        {
                            sw.Write(colsFormatXmlBold1[0][currentRow]);
                        }
                        else
                        {
                            sw.Write(colsFormatXml1[0][currentRow]);
                        }
                    }
                    sw.Write(eachRow[currentRow].ToString());
                    sw.WriteLine("</Data></Cell>");
                }
                sw.WriteLine("</Row>");
                index1++;
            }            
            sw.WriteLine("  </Table>");
            sw.WriteLine("  <WorksheetOptions xmlns=\"urn:schemas-microsoft-com:office:excel\">");
            sw.WriteLine("   <Selected/>");
            sw.WriteLine("   <Panes>");
            sw.WriteLine("    <Pane>");
            sw.WriteLine("     <Number>3</Number>");
            sw.WriteLine("     <ActiveRow>1</ActiveRow>");
            sw.WriteLine("    </Pane>");
            sw.WriteLine("   </Panes>");
            sw.WriteLine("   <ProtectObjects>False</ProtectObjects>");
            sw.WriteLine("   <ProtectScenarios>False</ProtectScenarios>");
            sw.WriteLine("  </WorksheetOptions>");
            sw.WriteLine(" </Worksheet>");

            //Add other sheet
            if (dsBook.Tables.Count > 1)
            {
                if (sheetname.Length > 1)
                {
                    sw.WriteLine(" <Worksheet ss:Name=\"" + sheetname[1] + "\">");
                }
                else
                {
                    sw.WriteLine(" <Worksheet ss:Name=\"Sheet2\">");
                }
                int rows1 = dsBook.Tables[1].Rows.Count + 4;
                int cols1 = dsBook.Tables[1].Columns.Count;
                sw.WriteLine(string.Format("  <Table ss:ExpandedColumnCount=\"{0}\" x:FullColumns=\"1\"", cols1.ToString()));
                sw.WriteLine("   x:FullRows=\"1\">");

                foreach (DataColumn eachColumn in dsBook.Tables[1].Columns)
                {
                    sw.WriteLine("<Column ss:AutoFitWidth=\"0\" ss:Width=\"90\"/>");
                }
                sw.WriteLine("<Row ss:Index=\"2\">");
                sw.WriteLine("<Cell ss:StyleID=\"s23\">");
                sw.WriteLine("<Data ss:Type=\"String\">" + titles[1] + "</Data>");
                sw.WriteLine("</Cell>");
                sw.WriteLine("</Row>");
                sw.WriteLine("<Row ss:Index=\"4\">");
                int j = 0;

                foreach (DataColumn eachColumn in dsBook.Tables[1].Columns)
                {
                    if (colsNames.Length > 1)
                    {
                        eachColumn.ColumnName = colsNames[1][j];
                    }
                    else
                    {
                        eachColumn.ColumnName = colsNames[0][j];
                    }
                    sw.Write("<Cell ss:StyleID=\"s22\"><Data ss:Type=\"String\">");
                    sw.Write(eachColumn.ColumnName.ToString());
                    sw.WriteLine("</Data></Cell>");
                    j++;
                }
                sw.WriteLine("</Row>");
                index2 = 5;
                foreach (DataRow eachRow in dsBook.Tables[1].Rows)
                {
                    total = false;
                    sw.WriteLine("<Row ss:Index=\"" + index2 + "\">");
                    for (int currentRow = 0; currentRow < cols1; currentRow++)
                    {
                        if (eachRow[currentRow].ToString().Contains("Total:"))
                        {
                            sw.Write("<Cell ss:StyleID=\"s24\"><Data ss:Type=\"String\">");
                            index2++;
                            total = true;
                        }
                        else
                        {
                            if (colsFormat.Length > 1)
                            {
                                if (total)
                                {
                                    sw.Write(colsFormatXmlBold1[1][currentRow]);
                                }
                                else
                                {
                                    sw.Write(colsFormatXml1[1][currentRow]);
                                }                               
                            }
                            else
                            {
                                if (total)
                                {
                                    sw.Write(colsFormatXmlBold1[0][currentRow]);
                                }
                                else
                                {
                                    sw.Write(colsFormatXml1[0][currentRow]);
                                }
                            }                            
                        }
                        sw.Write(eachRow[currentRow].ToString());
                        sw.WriteLine("</Data></Cell>");
                    }
                    sw.WriteLine("</Row>");
                    index2++;
                }
                sw.WriteLine("  </Table>");
                sw.WriteLine("  <WorksheetOptions xmlns=\"urn:schemas-microsoft-com:office:excel\">");
                sw.WriteLine("   <Selected/>");
                sw.WriteLine("   <Panes>");
                sw.WriteLine("    <Pane>");
                sw.WriteLine("     <Number>3</Number>");
                sw.WriteLine("     <ActiveRow>1</ActiveRow>");
                sw.WriteLine("    </Pane>");
                sw.WriteLine("   </Panes>");
                sw.WriteLine("   <ProtectObjects>False</ProtectObjects>");
                sw.WriteLine("   <ProtectScenarios>False</ProtectScenarios>");
                sw.WriteLine("  </WorksheetOptions>");
                sw.WriteLine(" </Worksheet>");
            }
            //Add other sheet
            if (dsBook.Tables.Count > 2)
            {
                if (sheetname.Length > 2)
                {
                    sw.WriteLine("<Worksheet ss:Name=\"" + sheetname[2] + "\">");
                }
                else
                {
                    sw.WriteLine(" <Worksheet ss:Name=\"Sheet3\">");
                }
                int rows2 = dsBook.Tables[2].Rows.Count + 4;
                int cols2 = dsBook.Tables[2].Columns.Count;
                sw.WriteLine(string.Format("  <Table ss:ExpandedColumnCount=\"{0}\" x:FullColumns=\"1\"", cols2.ToString()));
                sw.WriteLine("   x:FullRows=\"1\">");

                foreach (DataColumn eachColumn in dsBook.Tables[2].Columns)
                {
                    sw.WriteLine("<Column ss:AutoFitWidth=\"0\" ss:Width=\"90\"/>");
                }
                sw.WriteLine("<Row ss:Index=\"2\">");
                sw.WriteLine("<Cell ss:StyleID=\"s23\">");
                sw.WriteLine("<Data ss:Type=\"String\">" + titles[2] + "</Data>");
                sw.WriteLine("</Cell>");
                sw.WriteLine("</Row>");
                sw.WriteLine("<Row ss:Index=\"4\">");

                int k = 0;
                foreach (DataColumn eachColumn in dsBook.Tables[2].Columns)
                {
                    if (colsNames.Length > 2)
                    {
                        eachColumn.ColumnName = colsNames[2][k];
                    }
                    else
                    {
                        eachColumn.ColumnName = colsNames[0][k];
                    }
                    sw.Write("<Cell ss:StyleID=\"s22\"><Data ss:Type=\"String\">");
                    sw.Write(eachColumn.ColumnName.ToString());
                    sw.WriteLine("</Data></Cell>");
                    k++;
                }
                sw.WriteLine("</Row>");
              
                index3 = 5;
                foreach (DataRow eachRow in dsBook.Tables[2].Rows)
                {
                    total = false;
                    sw.WriteLine("<Row ss:Index=\"" + index3 + "\">");
                    for (int currentRow = 0; currentRow != cols2; currentRow++)
                    {
                        if (eachRow[currentRow].ToString().Contains("Total:"))
                        {
                            sw.Write("<Cell ss:StyleID=\"s24\"><Data ss:Type=\"String\">");
                            index3++;
                            total = true;
                        }
                        else
                        {
                            if (colsFormat.Length > 2)
                            {
                                if (total)
                                {
                                    sw.Write(colsFormatXmlBold1[2][currentRow]);
                                }
                                else
                                {
                                    sw.Write(colsFormatXml1[2][currentRow]);
                                }       
                            }
                            else
                            {
                                if (total)
                                {
                                    sw.Write(colsFormatXmlBold1[0][currentRow]);
                                }
                                else
                                {
                                    sw.Write(colsFormatXml1[0][currentRow]);
                                }       
                            }
                        }
                        sw.Write(eachRow[currentRow].ToString());
                        sw.WriteLine("</Data></Cell>");
                    }
                    sw.WriteLine("</Row>");
                    index3++;
                }
                sw.WriteLine("  </Table>");
                sw.WriteLine("  <WorksheetOptions xmlns=\"urn:schemas-microsoft-com:office:excel\">");
                sw.WriteLine("   <Selected/>");
                sw.WriteLine("   <Panes>");
                sw.WriteLine("    <Pane>");
                sw.WriteLine("     <Number>3</Number>");
                sw.WriteLine("     <ActiveRow>1</ActiveRow>");
                sw.WriteLine("    </Pane>");
                sw.WriteLine("   </Panes>");
                sw.WriteLine("   <ProtectObjects>False</ProtectObjects>");
                sw.WriteLine("   <ProtectScenarios>False</ProtectScenarios>");
                sw.WriteLine("  </WorksheetOptions>");
                sw.WriteLine(" </Worksheet>");
            }
                //Add other sheet
                if (dsBook.Tables.Count > 3)
                {
                    if (sheetname.Length > 3)
                    {
                        sw.WriteLine("<Worksheet ss:Name=\"" + sheetname[3] + "\">");
                    }
                    else
                    {
                        sw.WriteLine(" <Worksheet ss:Name=\"Sheet4\">");
                    }
                    int rows3 = dsBook.Tables[3].Rows.Count + 4;
                    int cols3 = dsBook.Tables[3].Columns.Count;
                    sw.WriteLine(string.Format("  <Table ss:ExpandedColumnCount=\"{0}\" x:FullColumns=\"1\"", cols3.ToString()));
                    sw.WriteLine("   x:FullRows=\"1\">");

                    foreach (DataColumn eachColumn in dsBook.Tables[3].Columns)
                    {
                        sw.WriteLine("<Column ss:AutoFitWidth=\"0\" ss:Width=\"90\"/>");
                    }
                    sw.WriteLine("<Row ss:Index=\"2\">");
                    sw.WriteLine("<Cell ss:StyleID=\"s23\">");
                    sw.WriteLine("<Data ss:Type=\"String\">" + titles[3] + "</Data>");
                    sw.WriteLine("</Cell>");
                    sw.WriteLine("</Row>");
                    sw.WriteLine("<Row ss:Index=\"4\">");

                    int k = 0;
                    foreach (DataColumn eachColumn in dsBook.Tables[3].Columns)
                    {
                        if (colsNames.Length > 3)
                        {
                            eachColumn.ColumnName = colsNames[3][k];
                        }
                        else
                        {
                            eachColumn.ColumnName = colsNames[0][k];
                        }
                        sw.Write("<Cell ss:StyleID=\"s22\"><Data ss:Type=\"String\">");
                        sw.Write(eachColumn.ColumnName.ToString());
                        sw.WriteLine("</Data></Cell>");
                        k++;
                    }
                    sw.WriteLine("</Row>");

                    index4 = 5;
                    foreach (DataRow eachRow in dsBook.Tables[3].Rows)
                    {
                        total = false;
                        sw.WriteLine("<Row ss:Index=\"" + index4 + "\">");
                        for (int currentRow = 0; currentRow != cols3; currentRow++)
                        {
                            if (eachRow[currentRow].ToString().Contains("Total:"))
                            {
                                sw.Write("<Cell ss:StyleID=\"s24\"><Data ss:Type=\"String\">");
                                index4++;
                                total = true;
                            }
                            else
                            {
                                if (colsFormat.Length > 3)
                                {
                                    if (total)
                                    {
                                        sw.Write(colsFormatXmlBold1[3][currentRow]);
                                    }
                                    else
                                    {
                                        sw.Write(colsFormatXml1[3][currentRow]);
                                    }
                                }
                                else
                                {
                                    if (total)
                                    {
                                        sw.Write(colsFormatXmlBold1[0][currentRow]);
                                    }
                                    else
                                    {
                                        sw.Write(colsFormatXml1[0][currentRow]);
                                    }
                                }
                            }
                            sw.Write(eachRow[currentRow].ToString());
                            sw.WriteLine("</Data></Cell>");
                        }
                        sw.WriteLine("</Row>");
                        index4++;
                    }
                    sw.WriteLine("  </Table>");
                    sw.WriteLine("  <WorksheetOptions xmlns=\"urn:schemas-microsoft-com:office:excel\">");
                    sw.WriteLine("   <Selected/>");
                    sw.WriteLine("   <Panes>");
                    sw.WriteLine("    <Pane>");
                    sw.WriteLine("     <Number>3</Number>");
                    sw.WriteLine("     <ActiveRow>1</ActiveRow>");
                    sw.WriteLine("    </Pane>");
                    sw.WriteLine("   </Panes>");
                    sw.WriteLine("   <ProtectObjects>False</ProtectObjects>");
                    sw.WriteLine("   <ProtectScenarios>False</ProtectScenarios>");
                    sw.WriteLine("  </WorksheetOptions>");
                    sw.WriteLine(" </Worksheet>");                
            }
            //Add other sheet
            if (dsBook.Tables.Count > 4)
            {
                if (sheetname.Length > 4)
                {
                    sw.WriteLine("<Worksheet ss:Name=\"" + sheetname[4] + "\">");
                }
                else
                {
                    sw.WriteLine(" <Worksheet ss:Name=\"Sheet5\">");
                }
                int rows4 = dsBook.Tables[4].Rows.Count + 4;
                int cols4 = dsBook.Tables[4].Columns.Count;
                sw.WriteLine(string.Format("  <Table ss:ExpandedColumnCount=\"{0}\" x:FullColumns=\"1\"", cols4.ToString()));
                sw.WriteLine("   x:FullRows=\"1\">");

                foreach (DataColumn eachColumn in dsBook.Tables[4].Columns)
                {
                    sw.WriteLine("<Column ss:AutoFitWidth=\"0\" ss:Width=\"90\"/>");
                }
                sw.WriteLine("<Row ss:Index=\"2\">");
                sw.WriteLine("<Cell ss:StyleID=\"s23\">");
                sw.WriteLine("<Data ss:Type=\"String\">" + titles[4] + "</Data>");
                sw.WriteLine("</Cell>");
                sw.WriteLine("</Row>");
                sw.WriteLine("<Row ss:Index=\"4\">");

                int k = 0;
                foreach (DataColumn eachColumn in dsBook.Tables[4].Columns)
                {
                    if (colsNames.Length > 4)
                    {
                        eachColumn.ColumnName = colsNames[4][k];
                    }
                    else
                    {
                        eachColumn.ColumnName = colsNames[0][k];
                    }
                    sw.Write("<Cell ss:StyleID=\"s22\"><Data ss:Type=\"String\">");
                    sw.Write(eachColumn.ColumnName.ToString());
                    sw.WriteLine("</Data></Cell>");
                    k++;
                }
                sw.WriteLine("</Row>");

                index5 = 5;
                foreach (DataRow eachRow in dsBook.Tables[4].Rows)
                {
                    total = false;
                    sw.WriteLine("<Row ss:Index=\"" + index5 + "\">");
                    for (int currentRow = 0; currentRow != cols4; currentRow++)
                    {
                        if (eachRow[currentRow].ToString().Contains("Total:"))
                        {
                            sw.Write("<Cell ss:StyleID=\"s24\"><Data ss:Type=\"String\">");
                            index5++;
                            total = true;
                        }
                        else
                        {
                            if (colsFormat.Length > 4)
                            {
                                if (total)
                                {
                                    sw.Write(colsFormatXmlBold1[4][currentRow]);
                                }
                                else
                                {
                                    sw.Write(colsFormatXml1[4][currentRow]);
                                }
                            }
                            else
                            {
                                if (total)
                                {
                                    sw.Write(colsFormatXmlBold1[0][currentRow]);
                                }
                                else
                                {
                                    sw.Write(colsFormatXml1[0][currentRow]);
                                }
                            }
                        }
                        sw.Write(eachRow[currentRow].ToString());
                        sw.WriteLine("</Data></Cell>");
                    }
                    sw.WriteLine("</Row>");
                    index5++;
                }
                sw.WriteLine("  </Table>");
                sw.WriteLine("  <WorksheetOptions xmlns=\"urn:schemas-microsoft-com:office:excel\">");
                sw.WriteLine("   <Selected/>");
                sw.WriteLine("   <Panes>");
                sw.WriteLine("    <Pane>");
                sw.WriteLine("     <Number>3</Number>");
                sw.WriteLine("     <ActiveRow>1</ActiveRow>");
                sw.WriteLine("    </Pane>");
                sw.WriteLine("   </Panes>");
                sw.WriteLine("   <ProtectObjects>False</ProtectObjects>");
                sw.WriteLine("   <ProtectScenarios>False</ProtectScenarios>");
                sw.WriteLine("  </WorksheetOptions>");
                sw.WriteLine(" </Worksheet>");
            }
            return sw;
        }
        
        static String[][] getcolsFormatXML(String[][] colsFormat)
        {            
            String[][] colsFormatXML = new String[5][];            
           
            for (int i = 0; i < colsFormat.Length; i++)
            {
                colsFormatXML[i] = new string[colsFormat[i].Length];
                //string[] xyz = new string[colsFormat[i].Length];
                for (int j = 0; j < colsFormat[i].Length; j++)
                {                    
                    switch (colsFormat[i][j])
                    {
                        case "string":
                            colsFormatXML[i][j] = "<Cell ss:StyleID=\"s21\"><Data ss:Type=\"String\">";
                            break;
                        case "number":
                            colsFormatXML[i][j] = "<Cell ss:StyleID=\"s25\"><Data ss:Type=\"Number\">";
                            break;
                        case "decimal":
                            colsFormatXML[i][j] = "<Cell ss:StyleID=\"s26\"><Data ss:Type=\"Number\">";
                            break;
                    }
                }
            }
            
            return colsFormatXML;
        }

        static String[][] getcolsFormatXMLBold(String[][] colsFormat)
        {
            String[][] colsFormatXMLBold = new String[5][];           
            for (int i = 0; i < colsFormat.Length; i++)
            {
                colsFormatXMLBold[i] = new string[colsFormat[i].Length];
                for (int j = 0; j < colsFormat[i].Length; j++)
                {                    
                    switch (colsFormat[i][j])
                    {
                        case "string":
                            colsFormatXMLBold[i][j] = "<Cell ss:StyleID=\"s29\"><Data ss:Type=\"String\">";
                            break;
                        case "number":
                            colsFormatXMLBold[i][j] = "<Cell ss:StyleID=\"s27\"><Data ss:Type=\"Number\">";
                            break;
                        case "decimal":
                            colsFormatXMLBold[i][j] = "<Cell ss:StyleID=\"s28\"><Data ss:Type=\"Number\">";
                            break;
                    }
                }
            }        
            return colsFormatXMLBold;
        }

        public static void ExcelXMLRpt(DataSet dsBook, string filename, string[] sheetnames, string[] titles, String[][] colsNames, String[][] colsFormat, DataSet dsSummary, String[] titleSummary, String[][] sumColsFormat)
        {
            String[][] colsFormatXml = getcolsFormatXML(colsFormat);
            String[][] colsFormatXmlBold = getcolsFormatXMLBold(colsFormat);

            String[][] sumColsFormatXml = getcolsFormatXML(sumColsFormat);
            String[][] sumColsFormatXmlBold = getcolsFormatXMLBold(sumColsFormat);

            StringWriter sw = new StringWriter();

            sw.WriteLine("<?xml version=\"1.0\"?>");
            sw.WriteLine("<?mso-application progid=\"Excel.Sheet\"?>");
            sw.WriteLine("<Workbook xmlns=\"urn:schemas-microsoft-com:office:spreadsheet\"");
            sw.WriteLine(" xmlns:o=\"urn:schemas-microsoft-com:office:office\"");
            sw.WriteLine(" xmlns:x=\"urn:schemas-microsoft-com:office:excel\"");
            sw.WriteLine(" xmlns:ss=\"urn:schemas-microsoft-com:office:spreadsheet\"");
            sw.WriteLine(" xmlns:html=\"http://www.w3.org/TR/REC-html40/\">");

            sw.Write(xlDocProp().ToString());

            sw.WriteLine(" <ExcelWorkbook xmlns=\"urn:schemas-microsoft-com:office:excel\">");
            sw.WriteLine("  <WindowHeight>8955</WindowHeight>");
            sw.WriteLine("  <WindowWidth>11355</WindowWidth>");
            sw.WriteLine("  <WindowTopX>480</WindowTopX>");
            sw.WriteLine("  <WindowTopY>15</WindowTopY>");
            sw.WriteLine("  <ProtectStructure>False</ProtectStructure>");
            sw.WriteLine("  <ProtectWindows>False</ProtectWindows>");
            sw.WriteLine(" </ExcelWorkbook>");

            sw.Write(xlStyles().ToString());

            for (int i = 0; i < dsBook.Tables.Count; i++)
            { 
                sw.Write(AddWorksheet(sheetnames[i], titles[i], dsBook.Tables[i], colsNames[i], colsFormatXml[i], colsFormatXmlBold[i], dsSummary.Tables[i], titleSummary, sumColsFormatXml[i], sumColsFormatXmlBold[i]));               
            }

            sw.WriteLine("</Workbook>");
            sw.Close();
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Charset = "";
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=" + filename + ".xls");
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            HttpContext.Current.Response.Write(sw.ToString());
            HttpContext.Current.Response.End();
        }
        
        protected static StringWriter AddWorksheet(String sheetname, String title, DataTable dsSheet, String[] colsNames, String[] colsFormatXml, String[] colsFormatXmlBold, DataTable dtSumm, String[] titleSumm, String[] sumColsFormatXml, String[] sumColsFormatXmlBold)
        {
            int index = 0;
            bool total = false;
            StringWriter sw = new StringWriter();
            sw.WriteLine(" <Worksheet ss:Name= \"" + sheetname + "\">");

            /********************* Summary **********************************************/

            sw.WriteLine(string.Format("  <Table x:FullColumns=\"1\""));
            sw.WriteLine("   x:FullRows=\"1\">");

            //adjust column widths   
            foreach (DataColumn eachColumn in dsSheet.Columns)
            {
                sw.WriteLine("<Column ss:AutoFitWidth=\"0\" ss:Width=\"90\"/>");
            }

            //generate main title
            sw.WriteLine("<Row ss:Index=\"2\">");
            sw.WriteLine("<Cell ss:StyleID=\"s23\">");
            sw.WriteLine("<Data ss:Type=\"String\">" + title + "</Data>");
            sw.WriteLine("</Cell>");
            sw.WriteLine("</Row>");
            

            //generate sub title
            sw.WriteLine("<Row ss:Index=\"4\">");
            sw.WriteLine("<Cell ss:StyleID=\"s30\">");
            sw.WriteLine("<Data ss:Type=\"String\">" + titleSumm[0] + "</Data>");
            sw.WriteLine("</Cell>");
            sw.WriteLine("</Row>");
            sw.WriteLine("<Row ss:Index=\"6\">");


            //generate column headings
            int i = 0;
            foreach (DataColumn eachColumn in dtSumm.Columns)
            {
                sw.Write("<Cell ss:StyleID=\"s22\"><Data ss:Type=\"String\">");
                sw.Write(eachColumn.ColumnName.ToString());
                sw.WriteLine("</Data></Cell>");
                i++;
            }
            sw.WriteLine("</Row>");

            //generate data
            index = 7;
            foreach (DataRow eachRow in dtSumm.Rows)
            {
                total = false;
                sw.WriteLine("<Row ss:Index=\"" + index + "\">");
                for (int currentRow = 0; currentRow != dtSumm.Columns.Count; currentRow++)
                {
                    if (eachRow[currentRow].ToString().Contains("TOTAL:"))
                    {
                        sw.Write("<Cell ss:StyleID=\"s24\"><Data ss:Type=\"String\">");
                        total = true;
                    }
                    else
                    {
                        if (total)
                        {
                            sw.Write(sumColsFormatXmlBold[currentRow]);
                        }
                        else
                        {
                            sw.Write(sumColsFormatXml[currentRow]);
                        }
                    }
                    sw.Write(eachRow[currentRow].ToString());
                    sw.WriteLine("</Data></Cell>");
                }
                sw.WriteLine("</Row>");
                index++;
            }
            //sw.WriteLine("  </Table>");

            /********************* Detail **********************************************/

            index = index + 2;
            //generate sub title
            sw.WriteLine("<Row ss:Index=\"" + index + "\">");
            sw.WriteLine("<Cell ss:StyleID=\"s30\">");
            sw.WriteLine("<Data ss:Type=\"String\">" + titleSumm[1] + "</Data>");
            sw.WriteLine("</Cell>");
            sw.WriteLine("</Row>");
            index = index + 2;
            sw.WriteLine("<Row ss:Index=\"" + index + "\">");


            //generate column headings
            i = 0;
            foreach (DataColumn eachColumn in dsSheet.Columns)
            {
                eachColumn.ColumnName = colsNames[i];
                sw.Write("<Cell ss:StyleID=\"s22\"><Data ss:Type=\"String\">");
                sw.Write(eachColumn.ColumnName.ToString());
                sw.WriteLine("</Data></Cell>");
                i++;
            }
            sw.WriteLine("</Row>");

            //generate data
            index = index + 1;
            foreach (DataRow eachRow in dsSheet.Rows)
            {
                total = false;
                sw.WriteLine("<Row ss:Index=\"" + index + "\">");
                for (int currentRow = 0; currentRow != dsSheet.Columns.Count; currentRow++)
                {
                    if (eachRow[currentRow].ToString().Contains("Total:"))
                    {
                        sw.Write("<Cell ss:StyleID=\"s24\"><Data ss:Type=\"String\">");
                        index++;
                        total = true;
                    }
                    else
                    {
                        if (total)
                        {
                            sw.Write(colsFormatXmlBold[currentRow]);
                        }
                        else
                        {
                            sw.Write(colsFormatXml[currentRow]);
                        }
                    }
                    sw.Write(eachRow[currentRow].ToString());
                    sw.WriteLine("</Data></Cell>");
                }
                sw.WriteLine("</Row>");
                index++;
            }
            sw.WriteLine("  </Table>");
            sw.WriteLine("  <WorksheetOptions xmlns=\"urn:schemas-microsoft-com:office:excel\">");
            sw.WriteLine("   <Selected/>");
            sw.WriteLine("   <Panes>");
            sw.WriteLine("    <Pane>");
            sw.WriteLine("     <Number>3</Number>");
            sw.WriteLine("     <ActiveRow>1</ActiveRow>");
            sw.WriteLine("    </Pane>");
            sw.WriteLine("   </Panes>");
            sw.WriteLine("   <ProtectObjects>False</ProtectObjects>");
            sw.WriteLine("   <ProtectScenarios>False</ProtectScenarios>");
            sw.WriteLine("  </WorksheetOptions>");
            sw.WriteLine(" </Worksheet>");

            return sw;
        }

    }
}
