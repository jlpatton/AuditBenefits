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

namespace EBA.Desktop.VWA
{
    public class VWAExcelReport
    {
        public VWAExcelReport()
        {
        }

        public static string ExcelXMLRpt(DataSet dsBook, string filename, string[] sheetnames, string[][] titles, String[][] colsFormat, int[][] colsWidth, string footer)
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

            for (int i = 0; i < dsBook.Tables.Count; i++)
            {
                sw.Write(AddWorksheet(sheetnames[i], titles[i], dsBook.Tables[i], colsFormatXml[i], colsFormatXmlBold[i], colsWidth[i], footer));
            }

            sw.WriteLine("</Workbook>");
            sw.Close();

            return sw.ToString();
        }

        public static string ExcelXMLRpt(DataTable dsSheet, string filename, string sheetname, string[] titles, String[][] colsFormat, int[][] colsWidth)
        {
            String[][] colsFormatXml = getcolsFormatXML(colsFormat, 1);
            String[][] colsFormatXmlBold = getcolsFormatXMLBold(colsFormat, 1);
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
            sw.Write(AddSummarySheet(sheetname, titles, dsSheet, colsFormatXml[0], colsFormatXmlBold[0], colsWidth[0]));            
            sw.WriteLine("</Workbook>");
            sw.Close();

            return sw.ToString();
        }
                
        public static string BankDispExcelRpt(DataSet ds, String yrmo)
        {
            String[][] colsFormat, colsFormatXml, colsFormatXmlBold;
            int[] colsWidth;
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

            //Summary
            String[] title = new String[] { "VWA Bank Statement Summary Validation Report for YRMO : " + yrmo };
            colsWidth = new int[] {140, 65 };
            colsFormat = new String[][] { new String[] { "string", "currency" } };
            colsFormatXml = getcolsFormatXML(colsFormat, 1);
            colsFormatXmlBold = getcolsFormatXMLBold(colsFormat, 1);

            sw.Write(AddSummarySheet("Summary", title, ds.Tables["Summary"], colsFormatXml[0], colsFormatXmlBold[0], colsWidth));

            //Deposits
            colsWidth = new int[] { 65, 80, 84, 65};
            colsFormat = new String[][] { new String[] { "stringSum", "currencySum" }, new String[] { "string", "string", "number", "currency" } };
            colsFormatXml = getcolsFormatXML(colsFormat, 2);
            colsFormatXmlBold = getcolsFormatXMLBold(colsFormat, 2);
            sw.Write(AddWorksheet("Deposits", "VWA Bank Statement Deposits Validation Report for YRMO : " + yrmo, ds.Tables["DepSum"], ds.Tables["DepDet"], colsFormatXml, colsFormatXmlBold, colsWidth));

            //Withdrawals
            colsWidth = new int[] { 65, 80, 80, 84, 65 };
            colsFormat = new String[][] { new String[] { "stringSum", "currencySum" }, new String[] { "string", "string", "string", "number", "currency" } };
            colsFormatXml = getcolsFormatXML(colsFormat, 2);
            colsFormatXmlBold = getcolsFormatXMLBold(colsFormat, 2);
            sw.Write(AddWorksheet("Withdrawals", "VWA Bank Statement Withdrawals Validation Report for YRMO : " + yrmo, ds.Tables["WDSum"], ds.Tables["WDDet"], colsFormatXml, colsFormatXmlBold, colsWidth));

            sw.WriteLine("</Workbook>");
            sw.Close();

            return sw.ToString();
        }

        public static string CaseExcelRpt(DataSet ds, string _cnum)
        {
            String[][] colsFormat, colsFormatXml, colsFormatXmlBold;
            int[] colsWidth;
            String[] title;
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

            //Contract tab
            title = new String[] { "Latest Demographic Information of Contract# " + _cnum };
            colsWidth = new int[] { 130, 160 };
            colsFormat = new String[][] { new String[] { "stringSumBold", "string" } };
            colsFormatXml = getcolsFormatXML(colsFormat, 1);
            colsFormatXmlBold = getcolsFormatXMLBold(colsFormat, 1);

            sw.Write(AddSummarySheet("Contract", title, ds.Tables["Contract"], colsFormatXml[0], colsFormatXmlBold[0], colsWidth));

            //Financial tab
            colsWidth = new int[] { 70, 70, 70, 70, 70, 70, 70, 70 };
            colsFormat = new String[][] { new String[] { "currency", "currency", "currency", "currency" }, new String[] { "number", "currency", "currency", "currency", "currency", "stringRight", "stringRight" } };
            colsFormatXml = getcolsFormatXML(colsFormat, 2);
            colsFormatXmlBold = getcolsFormatXMLBold(colsFormat, 2);
            title = new String[] {"Latest Financial Information of Contract# " + _cnum, "Changes to Financial Information of Contract# " + _cnum};
            DataSet dsFin = new DataSet();
            dsFin.Tables.Add(ds.Tables["Financial1"].Copy());
            dsFin.Tables.Add(ds.Tables["Financial2"].Copy());

            sw.Write(AddWorksheet("Financial", title, dsFin, colsFormatXml, colsFormatXmlBold, colsWidth));

            //History tab
            colsWidth = new int[] { 65, 160, 160, 65 };
            colsFormat = new String[][] { new String[] { "string", "string", "string", "stringRight" } };
            colsFormatXml = getcolsFormatXML(colsFormat, 1);
            colsFormatXmlBold = getcolsFormatXMLBold(colsFormat, 1);
            title = new String[] {"Historical Changes of Contract# " + _cnum };
            sw.Write(AddWorksheet("History", title, ds.Tables["History"], colsFormatXml[0], colsFormatXmlBold[0], colsWidth, string.Empty));

            sw.WriteLine("</Workbook>");
            sw.Close();

            return sw.ToString();
        }
        
        static StringWriter xlDocProp()
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

        static StringWriter xlStyles()
        {
            StringWriter sw = new StringWriter();

            sw.WriteLine("<Styles>");
            sw.WriteLine("<Style ss:ID=\"Default\" ss:Name=\"Normal\">");
            sw.WriteLine("<Alignment ss:Vertical=\"Bottom\"/>");
            sw.WriteLine("<Borders/>");
            sw.WriteLine("<Font/>");
            sw.WriteLine("<Interior/>");
            sw.WriteLine("<Protection/>");
            sw.WriteLine("</Style>");
            sw.WriteLine("<Style ss:ID=\"s21\">");
            sw.WriteLine("<Alignment ss:Horizontal=\"Left\" ss:Vertical=\"Bottom\" ss:WrapText=\"0\"/>");
            sw.WriteLine("</Style>");
            sw.WriteLine("<Style ss:ID=\"s22\">");
            sw.WriteLine("<Font x:Family=\"Swiss\" ss:Bold=\"1\" ss:Color=\"#FFFFFF\" />");
            sw.WriteLine("<Alignment ss:Horizontal=\"Center\" ss:Vertical=\"Center\" ss:WrapText=\"1\"/>");
            sw.WriteLine("<Interior ss:Color=\"#969696\" ss:Pattern=\"Solid\"/>");
            sw.WriteLine("</Style>");
            sw.WriteLine("<Style ss:ID=\"s23\">");
            sw.WriteLine("<ss:Font x:Family=\"Swiss\" ss:Size=\"12\" ss:Bold=\"1\" ss:Color=\"#5D478B\"/>");
            sw.WriteLine("</Style>");
            sw.WriteLine("<Style ss:ID=\"s24\">");
            sw.WriteLine("<ss:Font x:Family=\"Swiss\" ss:Bold=\"1\" ss:Color=\"#5D478B\"/>");
            sw.WriteLine("<Alignment ss:Horizontal=\"Left\"/>");
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
            sw.WriteLine("<Style ss:ID=\"s34\">");
            sw.WriteLine("<ss:Font x:Family=\"Swiss\" ss:Bold=\"1\" ss:Color=\"#ff0000\"/>");
            sw.WriteLine("</Style>");
            sw.WriteLine("<Style ss:ID=\"s35\">");
            sw.WriteLine("<NumberFormat ss:Format=\"0.00%;\"/>");
            sw.WriteLine("</Style>");
            sw.WriteLine("<Style ss:ID=\"s36\">");
            sw.WriteLine("<ss:Font ss:Bold=\"1\"/>");
            sw.WriteLine("<NumberFormat ss:Format=\"0.00%;\"/>");
            sw.WriteLine("</Style>");
            sw.WriteLine("<Style ss:ID=\"s37\">");
            sw.WriteLine("<Alignment ss:Horizontal=\"Left\" ss:Vertical=\"Bottom\"/>");
            sw.WriteLine("</Style>");
            sw.WriteLine("<Style ss:ID=\"s38\">");
            sw.WriteLine("<ss:Font ss:Bold=\"1\"/>");
            sw.WriteLine("<Alignment ss:Horizontal=\"Left\" ss:Vertical=\"Bottom\"/>");
            sw.WriteLine("</Style>");
            sw.WriteLine("<Style ss:ID=\"s39\">");
            sw.WriteLine("<Alignment ss:Horizontal=\"Right\" ss:Vertical=\"Bottom\" ss:WrapText=\"0\"/>");
            sw.WriteLine("</Style>");
            sw.WriteLine("<Style ss:ID=\"s40\">");
            sw.WriteLine("<ss:Font ss:Bold=\"1\" />");
            sw.WriteLine("<Alignment ss:Horizontal=\"Right\" ss:Vertical=\"Bottom\" ss:WrapText=\"0\"/>");
            sw.WriteLine("</Style>");
            sw.WriteLine("</Styles>");

            return sw;
        }

        static StringWriter AddWorksheet(String sheetname, String[] title, DataTable dsSheet, String[] colsFormatXml, String[] colsFormatXmlBold, int[] colsWidth, string footernotes)
        {
            int index = 0;
            bool total = false;
            bool _alert = false;
            StringWriter sw = new StringWriter();
            sw.WriteLine(" <Worksheet ss:Name= \"" + sheetname + "\">");

            int rows = dsSheet.Rows.Count + 4;
            int cols = dsSheet.Columns.Count;
            sw.WriteLine(string.Format("<Table ss:ExpandedColumnCount=\"{0}\" x:FullColumns=\"1\"", cols.ToString()));
            sw.WriteLine("   x:FullRows=\"1\">");

            //adjust column widths    
            foreach (int colwidth in colsWidth)
            {
                sw.WriteLine("<Column ss:AutoFitWidth=\"0\" ss:Width=\"" + colwidth + "\"/>");
            }

            //generate title
            index = 1;
            if (!title[0].Equals(""))
            {
                index = 2;
                foreach (string t1 in title)
                {
                    sw.WriteLine("<Row ss:Index=\"" + index + "\">");
                    sw.WriteLine("<Cell ss:StyleID=\"s23\">");
                    sw.WriteLine("<Data ss:Type=\"String\">" + t1 + "</Data>");
                    sw.WriteLine("</Cell>");
                    sw.WriteLine("</Row>");
                    index++;
                }
                index++;
            }

            //generate column headings
            int i = 0;
            sw.WriteLine("<Row ss:Index=\"" + index + "\" ss:Height=\"25\">");
            foreach (DataColumn eachColumn in dsSheet.Columns)
            {
                if (eachColumn.ColumnName.ToString().Equals("Flag")) { sw.WriteLine("<Cell><Data ss:Type=\"String\"></Data></Cell>"); i++; continue; } 
                
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
                                if (eachRow[currentRow].ToString().Equals("!") && dsSheet.Columns[currentRow].ToString().Equals("Flag"))
                                {
                                    sw.Write("<Cell ss:StyleID=\"s34\"><Data ss:Type=\"String\">");
                                    _alert = true;
                                }
                                else
                                {
                                    sw.Write(colsFormatXml[currentRow]);
                                }
                            }
                        }
                    }
                    sw.Write(eachRow[currentRow].ToString());
                    sw.WriteLine("</Data></Cell>");
                }
                sw.WriteLine("</Row>");
                index++;
            }

            //generate footer notes
            index = index + 4;
            if ((!footernotes.Equals("")) && (_alert == true))
            {
                sw.WriteLine("<Row ss:Index=\"" + index + "\">");                
                sw.WriteLine("<Cell ss:StyleID=\"s34\" ss:Index=\"1\">");
                sw.WriteLine("<Data ss:Type=\"String\">" + footernotes + "</Data>");
                sw.WriteLine("</Cell>");
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

        static StringWriter AddWorksheet(String sheetname, String title, DataTable dsSum, DataTable dsDet, String[][] colsFormatXml, String[][] colsFormatXmlBold, int[] colsWidth)
        {
            int index = 0;
            bool total = false;
            StringWriter sw = new StringWriter();
            sw.WriteLine(" <Worksheet ss:Name= \"" + sheetname + "\">");

            int cols = dsDet.Columns.Count;
            sw.WriteLine(string.Format("<Table ss:ExpandedColumnCount=\"{0}\" x:FullColumns=\"1\"", cols.ToString()));
            sw.WriteLine("   x:FullRows=\"1\">");

            //adjust column widths    
            foreach (int colwidth in colsWidth)
            {
                sw.WriteLine("<Column ss:AutoFitWidth=\"0\" ss:Width=\"" + colwidth + "\"/>");
            }

            //generate title
            index = 1;
            if (!title.Equals(""))
            {
                index = 2;
                sw.WriteLine("<Row ss:Index=\"" + index + "\">");
                sw.WriteLine("<Cell ss:StyleID=\"s23\">");
                sw.WriteLine("<Data ss:Type=\"String\">" + title + "</Data>");
                sw.WriteLine("</Cell>");
                sw.WriteLine("</Row>"); 
                index++;
            }

            //generate Summary
            index++;
            foreach (DataRow _row in dsSum.Rows)
            {
                total = false;
                sw.WriteLine("<Row ss:Index=\"" + index + "\">");
                for (int _col = 0; _col < dsSum.Columns.Count; _col++)
                {
                    if (_row[_col].ToString().ToUpper().Contains("TOTAL:") || _row[_col].ToString().ToUpper().Contains("DIFFERENCE:"))
                    {
                        sw.Write("<Cell ss:StyleID=\"s24\"><Data ss:Type=\"String\">");
                        index++;
                        total = true;
                    }
                    else
                    {
                        if (total)
                        {
                            sw.Write(colsFormatXmlBold[0][_col]);
                        }
                        else
                        {
                            sw.Write(colsFormatXml[0][_col]);
                        }
                    }
                    sw.Write(_row[_col].ToString());
                    sw.WriteLine("</Data></Cell>");
                }
                sw.WriteLine("</Row>");
                index++;
            }

            //generate column headings
            int i = 0;
            index++;
            sw.WriteLine("<Row ss:Index=\"" + index + "\" ss:Height=\"25\">");
            foreach (DataColumn eachColumn in dsDet.Columns)
            {
                sw.Write("<Cell ss:StyleID=\"s22\"><Data ss:Type=\"String\">");
                sw.Write(eachColumn.ColumnName.ToString());
                sw.WriteLine("</Data></Cell>");
                i++;
            }
            sw.WriteLine("</Row>");

            //generate data
            index = index + 1;
            foreach (DataRow eachRow in dsDet.Rows)
            {
                total = false;
                sw.WriteLine("<Row ss:Index=\"" + index + "\">");
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
                            index++;
                            total = true;
                        }
                        else
                        {
                            if (total)
                            {
                                sw.Write(colsFormatXmlBold[1][currentRow]);
                            }
                            else
                            {
                                sw.Write(colsFormatXml[1][currentRow]);
                            }
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

        static StringWriter AddSummarySheet(String sheetname, String[] title, DataTable dsSheet, String[] colsFormatXml,String[] colsFormatXmlBold, int[] colsWidth)
        {
            int index = 0;
            bool total = false;
            bool _alert = false;
            StringWriter sw = new StringWriter();
            sw.WriteLine(" <Worksheet ss:Name= \"" + sheetname + "\">");

            int rows = dsSheet.Rows.Count + 4;
            int cols = dsSheet.Columns.Count;
            sw.WriteLine(string.Format("<Table ss:ExpandedColumnCount=\"{0}\" x:FullColumns=\"1\"", cols.ToString()));
            sw.WriteLine("   x:FullRows=\"1\">");

            //adjust column widths    
            foreach (int colwidth in colsWidth)
            {
                sw.WriteLine("<Column ss:AutoFitWidth=\"0\" ss:Width=\"" + colwidth + "\"/>");
            }

            //generate title
            index = 1;
            if (!title[0].Equals(""))
            {
                index = 2;
                foreach (string t1 in title)
                {
                    sw.WriteLine("<Row ss:Index=\"" + index + "\">");
                    sw.WriteLine("<Cell ss:StyleID=\"s23\">");
                    sw.WriteLine("<Data ss:Type=\"String\">" + t1 + "</Data>");
                    sw.WriteLine("</Cell>");
                    sw.WriteLine("</Row>");
                    index++;
                }
                index++;
            }

            //generate data
            foreach (DataRow eachRow in dsSheet.Rows)
            {
                total = false;
                sw.WriteLine("<Row ss:Index=\"" + index + "\">");
                for (int currentRow = 0; currentRow != cols; currentRow++)
                {
                    if (eachRow[currentRow].ToString() == null || eachRow[currentRow].ToString() == String.Empty)
                    {
                        sw.Write("<Cell ss:StyleID=\"s21\"><Data ss:Type=\"String\">");
                    }
                    else
                    {
                        if ((eachRow[currentRow].ToString().ToUpper().Contains("TOTAL:")) || (eachRow[currentRow].ToString().ToUpper().Equals("DIFFERENCE:")))
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
                                if (eachRow[currentRow].ToString().Equals("!") && dsSheet.Columns[currentRow].ToString().Equals("Flag"))
                                {
                                    sw.Write("<Cell ss:StyleID=\"s34\"><Data ss:Type=\"String\">");
                                    _alert = true;
                                }
                                else
                                {
                                    sw.Write(colsFormatXml[currentRow]);
                                }
                            }
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

        static StringWriter AddWorksheet(String sheetname, String[] subtitles, DataSet dsSheets, string[][] colsFormatXml, string[][] colsFormatXmlBold, int[] colsWidth)
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

            int rowindex = 2;
            int j = 0;
            foreach (DataTable dsSheet in dsSheets.Tables)
            {
                //generate sub title
                sw.WriteLine("<Row ss:Index=\"" + rowindex + "\">");
                sw.WriteLine("<Cell ss:StyleID=\"s23\">");
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
                            if (eachRow[currentRow].ToString().Contains("TOTAL:"))
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
        
        static String[][] getcolsFormatXML(String[][] colsFormat, int _length)
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
                        case "percent":
                            colsFormatXML[i][j] = "<Cell ss:StyleID=\"s35\"><Data ss:Type=\"Number\">";
                            break;
                        case "stringSum":
                            colsFormatXML[i][j] = "<Cell ss:StyleID=\"s37\"><Data ss:Type=\"String\">";
                            break;
                        case "stringSumBold":
                            colsFormatXML[i][j] = "<Cell ss:StyleID=\"s38\"><Data ss:Type=\"String\">";
                            break;
                        case "currencySum":
                            colsFormatXML[i][j] = "<Cell ss:Index=\"3\" ss:StyleID=\"s32\"><Data ss:Type=\"Number\">";
                            break;
                        case "stringRight":
                            colsFormatXML[i][j] = "<Cell ss:StyleID=\"s39\"><Data ss:Type=\"String\">";
                            break;
                    }
                }
            }

            return colsFormatXML;
        }

        static String[][] getcolsFormatXMLBold(String[][] colsFormat, int _length)
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
                        case "percent":
                            colsFormatXMLBold[i][j] = "<Cell ss:StyleID=\"s36\"><Data ss:Type=\"Number\">";
                            break;
                        case "stringSum":
                            colsFormatXMLBold[i][j] = "<Cell ss:StyleID=\"s38\"><Data ss:Type=\"String\">";
                            break;
                        case "stringSumBold":
                            colsFormatXMLBold[i][j] = "<Cell ss:StyleID=\"s38\"><Data ss:Type=\"String\">";
                            break;
                        case "currencySum":
                            colsFormatXMLBold[i][j] = "<Cell ss:Index=\"3\" ss:StyleID=\"s33\"><Data ss:Type=\"Number\">";
                            break;
                        case "stringRight":
                            colsFormatXMLBold[i][j] = "<Cell ss:StyleID=\"s40\"><Data ss:Type=\"String\">";
                            break;
                    }
                }
            }
            return colsFormatXMLBold;
        }
    }
}