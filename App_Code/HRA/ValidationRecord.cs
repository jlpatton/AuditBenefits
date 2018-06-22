using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;


namespace EBA.Desktop.HRA
{
    /// <summary>
    /// Summary description for ValidationRecord
    /// </summary>
    public class ValidationRecord
    {
        DataSet ds = new DataSet();
        public ValidationRecord()
        {            
        }

        public void createValidationRecordStatusDead(int _empno)
        {
            ds.Clear();
            ds = HRAdata.empDependants(_empno);
            if (ds.Tables[0].Rows.Count > 0)
            {
                verifyRecords(_empno);
            }            
        }       

        public void createValidationRecordStatusOwnershipStopped(int _empno, string _dpndssn)
        {
            ds.Clear();
            ds = HRAdata.empDependants(_empno,_dpndssn);
            if (ds.Tables[0].Rows.Count > 0)
            {
                verifyRecords(_empno);
            }
        }

        private void verifyRecords(int _empno)
        {
            PilotData pobj = new PilotData();
            bool _inserted = false;
            for(int i = 1; i<= pobj.getBeneficiaryOrders(_empno); i++)
            {
                if (_inserted)
                {
                    break;
                }
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (Convert.ToInt32(dr["D_Order"]) == i)
                    {
                        if (dr["D_Rel"].ToString().Trim().Equals("CH"))
                        {
                            if ((!Convert.ToBoolean(dr["D_Elegb"]))
                                && (dr["D_OStopdt"].ToString().Trim().Equals(""))
                                && (Convert.ToInt32(dr["D_Age"]) < 24))
                            {
                                HRAdata.insertValidationRecord(_empno, dr["D_SSN"].ToString().Trim(), dr["D_Rel"].ToString().Trim());
                                _inserted = true;
                                break;
                            }
                        }
                        else
                        {
                            if ((!Convert.ToBoolean(dr["D_Elegb"]))
                                && (dr["D_OStopdt"].ToString().Trim().Equals("")))
                            {
                                HRAdata.insertValidationRecord(_empno, dr["D_SSN"].ToString().Trim(), dr["D_Rel"].ToString().Trim());
                                _inserted = true;
                                break;
                            }
                        }
                        
                    }
                }
            }
        }
        
    }

}
