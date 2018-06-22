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

namespace EBA.Desktop.Anthem
{
    public class CAExcelRpt
    {
        public CAExcelRpt()
        {           
        }

        public void ExcelXMLRpt(DataSet dsBook, string filename, string[] sheetnames, string[] titles, String[][] colsFormat)
        {
            String[][] colsFormatXml = getcolsFormatXML(colsFormat);
            String[][] colsFormatXmlBold = getcolsFormatXMLBold(colsFormat);
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
                sw.Write(AddWorksheet(sheetnames[i], titles[i], dsBook.Tables[i], colsFormatXml[i], colsFormatXmlBold[i]));
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
            sw.WriteLine("<Style ss:ID=\"s24\">");
            sw.WriteLine("<ss:Font x:Family=\"Swiss\" ss:Bold=\"1\" ss:Color=\"#5D478B\"/>");
            sw.WriteLine("<Alignment ss:Horizontal=\"Left\"/>");
            sw.WriteLine("</Style>");
            sw.WriteLine("<Style ss:ID=\"s25\">");
            sw.WriteLine("<NumberFormat ss:Format=\"#,##\"/>");
            sw.WriteLine("</Style>");
            sw.WriteLine("<Style ss:ID=\"s26\">");
            sw.WriteLine("<NumberFormat ss:Format=\"#,##0.00\"/>");
            sw.WriteLine("</Style>");
            sw.WriteLine("<Style ss:ID=\"s27\">");
            sw.WriteLine("<ss:Font ss:Bold=\"1\"/>");
            sw.WriteLine("<NumberFormat ss:Format=\"#,##\"/>");
            sw.WriteLine("</Style>");
            sw.WriteLine("<Style ss:ID=\"s28\">");
            sw.WriteLine("<ss:Font ss:Bold=\"1\"/>");
            sw.WriteLine("<NumberFormat ss:Format=\"#,##0.00\"/>");
            sw.WriteLine("</Style>");
            sw.WriteLine("  <Style ss:ID=\"s29\">");
            sw.WriteLine("<ss:Font ss:Bold=\"1\" />");
            sw.WriteLine("   <Alignment ss:Horizontal=\"Center\" ss:Vertical=\"Bottom\" ss:WrapText=\"0\"/>");
            sw.WriteLine("  </Style>");
            sw.WriteLine("<Style ss:ID=\"s30\">");
            sw.WriteLine("<ss:Font ss:Bold=\"1\"/>");
            sw.WriteLine("<NumberFormat ss:Format=\"###\"/>");
            sw.WriteLine("</Style>");
            sw.WriteLine("<Style ss:ID=\"s31\">");
            sw.WriteLine("<NumberFormat ss:Format=\"###\"/>");
            sw.WriteLine("</Style>");
            sw.WriteLine(" </Styles>");

            return sw;
        }

        StringWriter AddWorksheet(String sheetname, String title, DataTable dsSheet, String[] colsFormatXml, String[] colsFormatXmlBold)
        {
            int index=0;
            bool total = false;
            StringWriter sw = new StringWriter();
            sw.WriteLine(" <Worksheet ss:Name= \"" + sheetname + "\">");

            int rows = dsSheet.Rows.Count + 4;
            int cols = dsSheet.Columns.Count;
            sw.WriteLine(string.Format("  <Table ss:ExpandedColumnCount=\"{0}\" x:FullColumns=\"1\"", cols.ToString()));
            sw.WriteLine("   x:FullRows=\"1\">");

            //adjust column widths    
            foreach (DataColumn eachColumn in dsSheet.Columns)
            {
                sw.WriteLine("<Column ss:AutoFitWidth=\"0\" ss:Width=\"90\"/>");
            }

            //generate title
            sw.WriteLine("<Row ss:Index=\"2\">");
            sw.WriteLine("<Cell ss:StyleID=\"s23\">");
            sw.WriteLine("<Data ss:Type=\"String\">" + title + "</Data>");
            sw.WriteLine("</Cell>");
            sw.WriteLine("</Row>");
            sw.WriteLine("<Row ss:Index=\"4\">");

            //generate column headings
            int i = 0;
            foreach (DataColumn eachColumn in dsSheet.Columns)
            {
                sw.Write("<Cell ss:StyleID=\"s22\"><Data ss:Type=\"String\">");
                sw.Write(eachColumn.ColumnName.ToString());
                sw.WriteLine("</Data></Cell>");
                i++;
            }
            sw.WriteLine("</Row>");

            //generate data
            index = 5;
            foreach (DataRow eachRow in dsSheet.Rows)
            {
                total = false;
                sw.WriteLine("<Row ss:Index=\"" + index + "\">");
                for (int currentRow = 0; currentRow != cols; currentRow++)
                {
                    if (eachRow[currentRow].ToString().Contains("TOTAL:"))
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

        String[][] getcolsFormatXML(String[][] colsFormat)
        {
            String[][] colsFormatXML = new String[5][];

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
                        case "number":
                            colsFormatXML[i][j] = "<Cell ss:StyleID=\"s25\"><Data ss:Type=\"Number\">";
                            break;
                        case "checknum":
                            colsFormatXML[i][j] = "<Cell ss:StyleID=\"s31\"><Data ss:Type=\"Number\">";
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
                        case "checknum":
                            colsFormatXMLBold[i][j] = "<Cell ss:StyleID=\"s30\"><Data ss:Type=\"Number\">";
                            break;
                        case "decimal":
                            colsFormatXMLBold[i][j] = "<Cell ss:StyleID=\"s28\"><Data ss:Type=\"Number\">";
                            break;
                    }
                }
            }
            return colsFormatXMLBold;
        }

    }
}