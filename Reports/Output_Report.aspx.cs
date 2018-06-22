using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using EBA.Desktop;
using EBA.Desktop.HRA;

public partial class HRA_Reports_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void lnk_genRpt_OnClick(Object sender, EventArgs e)
    {
        Label1.Visible = false;
        errorDiv1.Visible = false;
        try
        {          
            if ((DateTimePicker1.Value == null) || (DateTimePicker2.Value == null))
            {
                if ((DateTimePicker1.Value == null))
                {
                    Label1.Visible = true;
                }
                if ((DateTimePicker2.Value == null))
                {
                    Label2.Visible = true;
                }
            }            
            else
            {
                DateTime _d1 = Convert.ToDateTime(DateTimePicker1.Value);
                DateTime _d2 = Convert.ToDateTime(DateTimePicker2.Value);

                string _date1 = "";
                string _date2 = "";

                if (_d1.Day < 10)
                {
                    _date1 = _d1.Month.ToString() + "/0" + _d1.Day.ToString() + "/" + _d1.Year.ToString();
                }
                else
                {
                    _date1 = _d1.Month.ToString() + "/" + _d1.Day.ToString() + "/" + _d1.Year.ToString();
                }

                if (_d2.Day < 10)
                {
                    _date2 = _d2.Month.ToString() + "/0" + _d2.Day.ToString() + "/" + _d2.Year.ToString();
                }
                else
                {
                    _date2 = _d2.Month.ToString() + "/" + _d2.Day.ToString() + "/" + _d2.Year.ToString();
                }
                
                string filename;
                string[] sheetnames;
                string[][] titles ;
                string[][] colsFormat;
                int[][] colsWidth;

                CreateExcelRpt xlobj = new CreateExcelRpt();

                DataSet dsImFinal = new DataSet();
                DataSet dsUpFinal = new DataSet();
                DataSet dsExFinal = new DataSet();
                dsImFinal = HRAdata.getEmployeeAuditInsert(_date1,_date2);
                dsUpFinal = HRAdata.getEmployeeAuditUpdate(_date1, _date2);
                dsExFinal = HRAdata.getEmployeeAuditException(_date1, _date2);
                              
                DataSet dsTemp = new DataSet();
                DataTable obNewDt1 = new DataTable();
                DataTable obNewDt2 = new DataTable();
                DataTable obNewDt3 = new DataTable();               
                

                if (dsImFinal.Tables.Count > 0)
                {
                    DataView dv1 = new DataView();
                    dv1 = dsImFinal.Tables[0].DefaultView;
                    dv1.Sort = "[Task Date] ASC,[Trigger Event] DESC,[EE#] ASC";

                    obNewDt1 = dv1.Table.Clone();
                    int idx = 0;
                    string[] strColNames = new string[obNewDt1.Columns.Count];

                    foreach (DataColumn col in obNewDt1.Columns)
                    {
                        strColNames[idx++] = col.ColumnName;
                    }

                    IEnumerator viewEnumerator = dv1.GetEnumerator();

                    while (viewEnumerator.MoveNext())
                    {
                        DataRowView drv = (DataRowView)viewEnumerator.Current;
                        DataRow dr = obNewDt1.NewRow();
                        try
                        {
                            foreach (string strName in strColNames)
                            {
                                dr[strName] = drv[strName];
                            }
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                        obNewDt1.Rows.Add(dr);
                    }
                }
                
                obNewDt1.TableName = "tbl1";
                dsTemp.Tables.Add(obNewDt1.Copy());
                dsTemp.Tables[0].TableName = "tbl1";
                

                if (dsUpFinal.Tables.Count > 0)
                {
                    DataView dv2 = new DataView();
                    dv2 = dsUpFinal.Tables[0].DefaultView;
                    dv2.Sort = "[Task Date] ASC,[Trigger Event] DESC,[EE#] ASC";

                    obNewDt2 = dv2.Table.Clone();
                    int idx = 0;
                    string[] strColNames = new string[obNewDt2.Columns.Count];

                    foreach (DataColumn col in obNewDt2.Columns)
                    {
                        strColNames[idx++] = col.ColumnName;
                    }

                    IEnumerator viewEnumerator = dv2.GetEnumerator();

                    while (viewEnumerator.MoveNext())
                    {
                        DataRowView drv = (DataRowView)viewEnumerator.Current;
                        DataRow dr = obNewDt2.NewRow();
                        try
                        {
                            foreach (string strName in strColNames)
                            {
                                dr[strName] = drv[strName];
                            }
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                        obNewDt2.Rows.Add(dr);
                    }
                }

                obNewDt2.TableName = "tbl2";
                dsTemp.Tables.Add(obNewDt2.Copy());
                dsTemp.Tables[1].TableName = "tbl2";
               

                if (dsExFinal.Tables.Count > 0)
                {
                    DataView dv3 = new DataView();
                    dv3 = dsExFinal.Tables[0].DefaultView;
                    dv3.Sort = "[Task Date] ASC,[Trigger Event] DESC,[EE#] ASC";

                    obNewDt3 = dv3.Table.Clone();
                    int idx = 0;
                    string[] strColNames = new string[obNewDt3.Columns.Count];

                    foreach (DataColumn col in obNewDt3.Columns)
                    {
                        strColNames[idx++] = col.ColumnName;
                    }

                    IEnumerator viewEnumerator = dv3.GetEnumerator();

                    while (viewEnumerator.MoveNext())
                    {
                        DataRowView drv = (DataRowView)viewEnumerator.Current;
                        DataRow dr = obNewDt3.NewRow();
                        try
                        {
                            foreach (string strName in strColNames)
                            {
                                dr[strName] = drv[strName];
                            }
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                        obNewDt3.Rows.Add(dr);
                    }
                }

                obNewDt3.TableName = "tbl3";
                dsTemp.Tables.Add(obNewDt3.Copy());
                dsTemp.Tables[2].TableName = "tbl3";

                titles = new string[3][];
                sheetnames = new string[3];
                colsFormat = new string[3][];
                colsWidth = new int[3][];

                if (!_date1.Equals(_date2))
                {
                    filename = "Upload_Report_" + _date1 + _date2;
                    titles[0] = new string[] { "Audit report for inserts - " + _date1 + " to " + _date2 };
                    titles[1] = new string[] { "Audit report for Updates - " + _date1 + " to " + _date2 };
                    titles[2] = new string[] { "Audit report for Exceptions - " + _date1 + " to " + _date2 };
                }
                else
                {
                    filename = "Upload_Report_" + _date1;
                    titles[0] = new string[] { "Audit report for inserts - " + _date1};
                    titles[1] = new string[] { "Audit report for Updates - " + _date1};
                    titles[2] = new string[] { "Audit report for Exceptions - " + _date1};
                }
                
                
                
                sheetnames[0] = "Inserts";
                sheetnames[1] = "Updates";
                sheetnames[2] = "Exceptions";
                colsFormat[0] = new string[] { "number", "string", "string", "string", "string", "string", "string", "string", "string", "string"};
                colsFormat[1] = new string[] { "number", "string", "string", "string", "string", "string", "string", "string", "string", "string" };
                colsFormat[2] = new string[] { "number", "string", "string", "string", "string", "string", "string", "string", "string", "string" };
                colsWidth[0] = new int[] { 50, 65, 140, 140, 75, 65, 50, 50, 100, 50 };
                colsWidth[1] = new int[] { 50, 65, 140, 140, 75, 65, 50, 50, 100, 50 };
                colsWidth[2] = new int[] { 50, 65, 140, 140, 75, 65, 50, 50, 100, 50 };
                xlobj.ExcelXMLRpt(dsTemp, filename, sheetnames, titles, colsFormat, colsWidth, String.Empty);
            }
        }
        catch (Exception ex)
        {
            errorDiv1.Visible = true;
            lbl_error1.Text = "Error generating report - " + ex.Message;
        }
    }
}
