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

namespace EBA.Desktop.HRA
{
    public class HRAExcelReport
    {
        public HRAExcelReport()
        {

        }

        public string ExcelXMLRpt(DataSet dsBook, string filename, string[] sheetnames, string[] titles, string[][] colsName, string[][] colsFormat, int[][] colsWidth)
        {
            String[][] colsFormatXml = getcolsFormatXML(colsFormat, dsBook.Tables.Count);
            String[][] colsFormatXmlBold = getcolsFormatXMLBold(colsFormat, dsBook.Tables.Count);
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
            if (filename.Contains("HRA_Putnam_Balance"))
            {
                sw.Write(AddSOFOSummary(sheetnames[0], titles[0], dsBook.Tables[0]));
                sw.Write(AddWorksheet(sheetnames[1], titles[1], dsBook.Tables[1], colsName[1], colsFormatXml[1], colsFormatXmlBold[1], colsWidth[1]));
            }
            else
            {
                for (int i = 0; i < dsBook.Tables.Count; i++)
                {
                    sw.Write(AddWorksheet(sheetnames[i], titles[i], dsBook.Tables[i], colsName[i], colsFormatXml[i], colsFormatXmlBold[i], colsWidth[i]));
                }
            }
            sw.WriteLine("</Workbook>");
            sw.Close();

            return sw.ToString();
        }

        public string ExcelXMLRpt(DataSet dsBook1, DataSet dsBook2, string filename, string[] sheetnames, string[] titles, string[][] subtitles, string[][] colsFormat, int[][] colsWidth)
        {
            String[][] colsFormatXml = getcolsFormatXML(colsFormat, 2);
            String[][] colsFormatXmlBold = getcolsFormatXMLBold(colsFormat, 2);
            StringWriter sw = new StringWriter();
            DataSet ds;
           
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
            for (int i = 0; i < sheetnames.Length; i++)
            {
                ds = new DataSet();
                ds.Tables.Add(dsBook1.Tables[i].Copy());
                ds.Tables.Add(dsBook2.Tables[i].Copy());
                sw.Write(AddWorksheet(sheetnames[i], titles[i], subtitles[i], ds, colsFormatXml, colsFormatXmlBold, colsWidth[i]));
                ds.Clear();
            }
            
            sw.WriteLine("</Workbook>");
            sw.Close();

            return sw.ToString();
        }

        StringWriter xlDocProp()
        {
            StringWriter sw = new StringWriter();

            sw.WriteLine(" <DocumentProperties xmlns=\"urn:schemas-microsoft-com:office:office\">;");
            sw.WriteLine("  <Author>EBS_HRA</Author>");
            sw.WriteLine(string.Format("  <Created>{0}T</Created>", DateTime.Now.ToString("yyyy-mm-dd")));
            sw.WriteLine("  <Company>Acme</Company>");
            sw.WriteLine("  <Version>1.0</Version>");
            sw.WriteLine(" </DocumentProperties>");

            return sw;
        }

        StringWriter xlStyles()
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
            sw.WriteLine("   <Alignment ss:Horizontal=\"Left\" ss:Vertical=\"Bottom\" ss:WrapText=\"0\"/>");
            sw.WriteLine("  </Style>");
            sw.WriteLine("<Style ss:ID=\"s22\">");
            sw.WriteLine("<Font x:Family=\"Swiss\" ss:Bold=\"1\" ss:Color=\"#FFFFFF\" />");
            sw.WriteLine("   <Alignment ss:Horizontal=\"Center\" ss:Vertical=\"Center\" ss:WrapText=\"1\"/>");
            sw.WriteLine("<Interior ss:Color=\"#969696\" ss:Pattern=\"Solid\"/>");
            sw.WriteLine("</Style>");
            sw.WriteLine("<Style ss:ID=\"s23\">");
            sw.WriteLine("<ss:Font x:Family=\"Swiss\" ss:Size=\"12\" ss:Bold=\"1\" ss:Color=\"#5D478B\"/>");
            sw.WriteLine("</Style>");
            sw.WriteLine("<Style ss:ID=\"s24\">");
            sw.WriteLine("<ss:Font x:Family=\"Swiss\" ss:Bold=\"1\" ss:Color=\"#5D478B\"/>");
            sw.WriteLine("</Style>");
            sw.WriteLine("<Style ss:ID=\"s25\">");
            sw.WriteLine("<NumberFormat ss:Format=\"#,##;-#,##;#,#0;\"/>");
            sw.WriteLine("</Style>");
            sw.WriteLine("<Style ss:ID=\"s26\">");
            sw.WriteLine("<NumberFormat ss:Format=\"#,##0.00;\"/>");
            sw.WriteLine("</Style>");
            sw.WriteLine("<Style ss:ID=\"s27\">");
            sw.WriteLine("<ss:Font ss:Bold=\"1\"/>");
            sw.WriteLine("<NumberFormat ss:Format=\"#,##;-#,##;#,#0;\"/>");
            sw.WriteLine("</Style>");
            sw.WriteLine("<Style ss:ID=\"s28\">");
            sw.WriteLine("<ss:Font ss:Bold=\"1\"/>");
            sw.WriteLine("<NumberFormat ss:Format=\"#,##0.00;\"/>");
            sw.WriteLine("</Style>");
            sw.WriteLine("<Style ss:ID=\"s29\">");
            sw.WriteLine("<ss:Font ss:Bold=\"1\" />");
            sw.WriteLine("<Alignment ss:Horizontal=\"Left\" ss:Vertical=\"Bottom\" ss:WrapText=\"0\"/>");
            sw.WriteLine("</Style>");
            sw.WriteLine("<Style ss:ID=\"s30\">");
            sw.WriteLine("<ss:Font ss:Bold=\"1\"/>");
            sw.WriteLine("<NumberFormat ss:Format=\"###;-###;##0;\"/>");
            sw.WriteLine("</Style>");
            sw.WriteLine("<Style ss:ID=\"s31\">");
            sw.WriteLine("<NumberFormat ss:Format=\"###;-###;##0;\"/>");
            sw.WriteLine("</Style>");
            sw.WriteLine("<Style ss:ID=\"s32\">");
            sw.WriteLine("<NumberFormat ss:Format=\"$#,#0.00;($#,#0.00);$0.00;\"/>");
            sw.WriteLine("</Style>");
            sw.WriteLine("<Style ss:ID=\"s33\">");
            sw.WriteLine("<ss:Font ss:Bold=\"1\"/>");
            sw.WriteLine("<NumberFormat ss:Format=\"$#,#0.00;($#,#0.00);$0.00;\"/>");
            sw.WriteLine("</Style>");
            sw.WriteLine(" </Styles>");

            return sw;
        }

        StringWriter AddWorksheet(String sheetname, String title, DataTable dsSheet, String[] colsName, String[] colsFormatXml, String[] colsFormatXmlBold, int[] colsWidth)
        {
            bool total = false;
            StringWriter sw = new StringWriter();
            sw.WriteLine(" <Worksheet ss:Name= \"" + sheetname + "\">");

            int cols = dsSheet.Columns.Count;
            sw.WriteLine(string.Format("  <Table x:FullColumns=\"1\""));
            sw.WriteLine("   x:FullRows=\"1\">");

            //adjust column widths    
            foreach (int colwidth in colsWidth)
            {
                sw.WriteLine("<Column ss:AutoFitWidth=\"0\" ss:Width=\"" + colwidth + "\"/>");
            }

            //generate title
            sw.WriteLine("<Row ss:Index=\"2\">");
            sw.WriteLine("<Cell ss:StyleID=\"s23\">");
            sw.WriteLine("<Data ss:Type=\"String\">" + title + "</Data>");
            sw.WriteLine("</Cell>");
            sw.WriteLine("</Row>");

            int dataindex = 4;
            //Add rows count to HRA_AUDITR
            if (sheetname.Equals("HRAAUDITR"))
            {
                sw.WriteLine("<Row ss:Index=\"" + dataindex + "\">");
                sw.Write("<Cell ss:Index=\"" + colsName.Length + "\" ss:StyleID=\"s24\"><Data ss:Type=\"String\">");
                sw.Write("Participants="+dsSheet.Rows.Count);
                sw.WriteLine("</Data></Cell>");
                sw.WriteLine("</Row>");
                dataindex++;
            }

            //generate column headings
            int i = 0;
            sw.WriteLine("<Row ss:Index=\"" + dataindex + "\" ss:Height=\"25\">");
            foreach (string colName in colsName)
            {                
                sw.Write("<Cell ss:StyleID=\"s22\"><Data ss:Type=\"String\">");
                sw.Write(colName);
                sw.WriteLine("</Data></Cell>");
                i++;
            }
            sw.WriteLine("</Row>");
            dataindex++;

            //generate data
            foreach (DataRow eachRow in dsSheet.Rows)
            {
                total = false;
                sw.WriteLine("<Row ss:Index=\"" + dataindex + "\">");
                for (int currentRow = 0; currentRow != cols; currentRow++)
                {
                    if (eachRow[currentRow].ToString() == null || eachRow[currentRow].ToString() == String.Empty)
                    {
                        sw.Write("<Cell ss:StyleID=\"s21\"><Data ss:Type=\"String\">");
                    }
                    else
                    {
                        if (eachRow[currentRow].ToString().ToUpper().Contains("TOTAL:"))
                        {
                            sw.Write("<Cell ss:StyleID=\"s24\"><Data ss:Type=\"String\">");
                            dataindex++;
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
                    }
                    
                    sw.Write(eachRow[currentRow].ToString());
                    sw.WriteLine("</Data></Cell>");
                }
                sw.WriteLine("</Row>");
                dataindex++;
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

        StringWriter AddWorksheet(String sheetname, String title, String[] subtitles, DataSet dsSheets, string[][] colsFormatXml, string[][] colsFormatXmlBold, int[] colsWidth)
        {
            StringWriter sw = new StringWriter();
            bool total = false;
            int cols = 0;
            if (dsSheets.Tables[0].Columns.Count > dsSheets.Tables[1].Columns.Count)
                cols = dsSheets.Tables[0].Columns.Count;
            else
                cols = dsSheets.Tables[1].Columns.Count;

            sw.WriteLine(" <Worksheet ss:Name= \"" + sheetname + "\">");

            sw.WriteLine(string.Format("  <Table x:FullColumns=\"1\""));
            sw.WriteLine("   x:FullRows=\"1\">");            

            //adjust column widths
            foreach (int colwidth in colsWidth)
            {
                sw.WriteLine("<Column ss:AutoFitWidth=\"0\" ss:Width=\"" + colwidth + "\"/>");
            }           

            //generate title
            sw.WriteLine("<Row ss:Index=\"2\">");
            sw.WriteLine("<Cell ss:StyleID=\"s23\">");
            sw.WriteLine("<Data ss:Type=\"String\">" + title + "</Data>");
            sw.WriteLine("</Cell>");
            sw.WriteLine("</Row>");            
            
            int rowindex = 4;
            int j = 0;
            foreach(DataTable dsSheet in dsSheets.Tables)
            {
                //generate sub title
                sw.WriteLine("<Row ss:Index=\"" + rowindex + "\">");
                sw.WriteLine("<Cell ss:StyleID=\"s24\">");
                sw.WriteLine("<Data ss:Type=\"String\">" + subtitles[j] + "</Data>");
                sw.WriteLine("</Cell>");
                sw.WriteLine("</Row>");
                rowindex += 2;

                //generate column heading
                sw.WriteLine("<Row ss:Index=\"" + rowindex + "\">");
                foreach (DataColumn eachColumn in dsSheet.Columns)
                {
                    sw.Write("<Cell ss:StyleID=\"s22\"><Data ss:Type=\"String\">");
                    sw.Write(eachColumn.ColumnName.ToString());
                    sw.WriteLine("</Data></Cell>");

                }
                sw.WriteLine("</Row>");

                //generate data
                foreach (DataRow eachRow in dsSheet.Rows)
                {
                    total = false;
                    sw.WriteLine("<Row>");
                    for (int currentRow = 0; currentRow != dsSheet.Columns.Count; currentRow++)
                    {
                        if (eachRow[currentRow].ToString() == null || eachRow[currentRow].ToString() == String.Empty)
                        {
                            sw.Write("<Cell ss:StyleID=\"s21\"><Data ss:Type=\"String\">");
                        }
                        else
                        {
                            if (eachRow[currentRow].ToString().ToUpper().Contains("TOTAL:"))
                                //added .ToUpper() function to compare the correct case 3/17/2009 R.A
                            //if (eachRow[currentRow].ToString().Contains("Total:"))
                            {
                                sw.Write("<Cell ss:StyleID=\"s24\"><Data ss:Type=\"String\">");
                                total = true;
                            }
                            else
                            {
                                if (total)
                                {
                                    sw.Write(colsFormatXmlBold[j][currentRow]);
                                }
                                else
                                {
                                    sw.Write(colsFormatXml[j][currentRow]);
                                }
                            }
                        }
                        sw.Write(eachRow[currentRow].ToString());
                        sw.WriteLine("</Data></Cell>");
                    }
                    sw.WriteLine("</Row>");
                }
                j++;
                rowindex += (dsSheet.Rows.Count + 3);
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

        StringWriter AddSOFOSummary(String sheetname, String title, DataTable dsSheet)
        {
            StringWriter sw = new StringWriter();
            sw.WriteLine(" <Worksheet ss:Name= \"" + sheetname + "\">");

            int cols = dsSheet.Columns.Count;
            sw.WriteLine(string.Format("  <Table  x:FullColumns=\"1\""));
            sw.WriteLine("   x:FullRows=\"1\">");
            
            sw.WriteLine("<Column ss:AutoFitWidth=\"0\" ss:Width=\"190\"/>");
            sw.WriteLine("<Column ss:AutoFitWidth=\"0\" ss:Width=\"100\"/>");            

            //generate title
            sw.WriteLine("<Row ss:Index=\"2\">");
            sw.WriteLine("<Cell ss:StyleID=\"s23\">");
            sw.WriteLine("<Data ss:Type=\"String\">" + title + "</Data>");
            sw.WriteLine("</Cell>");
            sw.WriteLine("</Row>");
            sw.WriteLine("<Row ss:Index=\"3\">");
            sw.WriteLine("<Cell ss:StyleID=\"s23\">");
            sw.WriteLine("<Data ss:Type=\"String\">" + "Summary vs. Detail" + "</Data>");
            sw.WriteLine("</Cell>");
            sw.WriteLine("</Row>");

            //generate data

            int index = 6;
            int introw = 0;
            foreach (DataRow eachRow in dsSheet.Rows)
            {
                sw.WriteLine("<Row ss:Index=\"" + index + "\" ss:Height=\"16\">");
                
                sw.Write("<Cell ss:StyleID=\"s29\"><Data ss:Type=\"String\">");
                sw.Write(eachRow[0].ToString());
                sw.WriteLine("</Data></Cell>");

                if (introw == 0 || introw == 1 || introw == 11)
                {
                    if (introw == 11)
                    {
                        sw.Write("<Cell ss:StyleID=\"s21\"><Data ss:Type=\"String\">");
                        sw.Write(eachRow[1].ToString());
                    }
                    else
                    {
                        sw.Write("<Cell ss:StyleID=\"s31\"><Data ss:Type=\"Number\">");
                        sw.Write(eachRow[1].ToString());
                    }
                }
                else
                {
                    sw.Write("<Cell ss:StyleID=\"s32\"><Data ss:Type=\"Number\">");
                    sw.Write(decimal.Parse(eachRow[1].ToString(), System.Globalization.NumberStyles.Currency));
                }
                sw.WriteLine("</Data></Cell>");
                
                sw.WriteLine("</Row>");
                index++;
                introw++;
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

        String[][] getcolsFormatXML(String[][] colsFormat, int _length)
        {
            String[][] colsFormatXML = new String[_length][];

            for (int i = 0; i < colsFormat.Length; i++)
            {
                colsFormatXML[i] = new string[colsFormat[i].Length];

                for (int j = 0; j < colsFormat[i].Length; j++)
                {
                    switch (colsFormat[i][j])
                    {
                        case "string":
                            colsFormatXML[i][j] = "<Cell ss:StyleID=\"s21\"><Data ss:Type=\"String\">";
                            break;
                        case "number_comma":
                            colsFormatXML[i][j] = "<Cell ss:StyleID=\"s25\"><Data ss:Type=\"Number\">";
                            break;
                        case "number":
                            colsFormatXML[i][j] = "<Cell ss:StyleID=\"s31\"><Data ss:Type=\"Number\">";
                            break;
                        case "decimal":
                            colsFormatXML[i][j] = "<Cell ss:StyleID=\"s26\"><Data ss:Type=\"Number\">";
                            break;
                        case "currency":
                            colsFormatXML[i][j] = "<Cell ss:StyleID=\"s32\"><Data ss:Type=\"Number\">";
                            break;
                    }
                }
            }

            return colsFormatXML;
        }

        String[][] getcolsFormatXMLBold(String[][] colsFormat, int _length)
        {
            String[][] colsFormatXMLBold = new String[_length][];
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
                        case "number_comma":
                            colsFormatXMLBold[i][j] = "<Cell ss:StyleID=\"s27\"><Data ss:Type=\"Number\">";
                            break;
                        case "number":
                            colsFormatXMLBold[i][j] = "<Cell ss:StyleID=\"s30\"><Data ss:Type=\"Number\">";
                            break;
                        case "decimal":
                            colsFormatXMLBold[i][j] = "<Cell ss:StyleID=\"s28\"><Data ss:Type=\"Number\">";
                            break;
                        case "currency":
                            colsFormatXMLBold[i][j] = "<Cell ss:StyleID=\"s33\"><Data ss:Type=\"Number\">";
                            break;
                    }
                }
            }
            return colsFormatXMLBold;
        }
    }
}