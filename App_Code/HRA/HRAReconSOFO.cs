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
    public class HRAReconSOFO
    {
        public HRAReconSOFO()
        {            
        }

        public void ReconcileSOFO(string yrmo, Decimal begBal, Decimal endBal, Decimal totContr, Decimal totDiv, Decimal termPay, Decimal withdraw, Decimal manAdjAmt, String manAdjNotes)
        {
            Decimal diffamt, totPutnamAmt, totPutnamAdjAmt;
            HRASofoDAL sobj = new HRASofoDAL();

            totPutnamAmt = sobj.getTotPutnamAmt(yrmo);
            totPutnamAdjAmt = sobj.getTotPutnamAdjAmt(yrmo);
            diffamt = (((begBal + totDiv + totContr) - (termPay + withdraw)) - endBal) + manAdjAmt;

            Pass1(yrmo, begBal); 
            Pass2(yrmo, diffamt); 
            Pass3(yrmo, termPay, totPutnamAmt); 
            Pass4(yrmo, withdraw, totPutnamAdjAmt);

            sobj.insertSOFOtbl(yrmo, begBal, endBal, totContr, totDiv, termPay, withdraw, totPutnamAmt, totPutnamAdjAmt, diffamt, manAdjAmt, manAdjNotes);            
        }

        public void Pass1(String yrmo, Decimal begBal)
        {
            HRASofoDAL sobj = new HRASofoDAL();
            HRA gobj = new HRA();
            String priyrmo = gobj.getPrevYRMO(yrmo);
            Decimal diffamt, prev_endBal;            
            String errMsg = "";

            
            prev_endBal = sobj.getPrevEndBal(priyrmo);
            diffamt = begBal - prev_endBal;
            
            if (diffamt != 0)
            {
                throw new Exception("Previous month ending balance " + prev_endBal.ToString("C") + " does not equal current month beginning balance " + begBal.ToString("C") + " !");
            }
        }

        public void Pass2(String yrmo, Decimal diffamt)
        {
            String errMsg = "";
            
            if (diffamt != 0)
            {
                throw new Exception("Statement is out of balance by " + diffamt.ToString("C") + " !");
            }
        }

        public void Pass3(String yrmo, Decimal termPay, Decimal totPutnamAmt)
        {
           Decimal diffamt;
           String errMsg = "";

            diffamt = termPay - totPutnamAmt;
            
            if (diffamt != 0)
            {
                throw new Exception("Termination Payments does NOT equal Total Putnam Amount by " + diffamt.ToString("C") + " !"); 
            }
        }

        public void Pass4(String yrmo, Decimal withdraw, Decimal totPutnamAdjAmt)
        {
            Decimal diffamt;
            String errMsg = "";

            diffamt = withdraw - totPutnamAdjAmt;
            
            if (diffamt != 0)
            {
                throw new Exception("Withdrawals Amount does NOT equal Total Putnam Adjustments Amount by " + diffamt.ToString("C") + " !");  
            }
        }        
    }
}
