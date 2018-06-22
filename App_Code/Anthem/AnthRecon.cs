using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;

/// <summary>
/// Summary description for AnthRecon
/// </summary>
/// 
namespace EBA.Desktop.Anthem
{
    public class AnthRecon
    {
        public AnthRecon()
        {            
        }

        //RFDF Recon
        //public static void matchCF(string _yrmo)
        //{
        //    List<int> rid = new List<int>();
        //    rid = ReconDAL.matchCF(_yrmo);
        //    if (rid.Count != 0)
        //    {
        //        ReconDAL.updatematchCF(rid);
        //    }
        //}

        //public static void amountMatch(string _yrmo)
        //{
        //    DataSet ds = new DataSet();
        //    ds = ReconDAL.amountMatch(_yrmo);
        //    insertReconData(ds, _yrmo);
        //}
        //public static void CF(string _yrmo)
        //{
        //    DataSet ds = new DataSet();
        //    ds = ReconDAL.updateCFFinal(_yrmo);
        //    insertReconCF(ds, _yrmo);
        //}
        //private static void insertReconData(DataSet ds, string _yrmo)
        //{
        //    string _claimid;
        //    decimal _clmamt, _rfdfamt, _var;

        //    foreach (DataRow row in ds.Tables["reconTemp"].Rows)
        //    {
        //        _claimid = row[0].ToString();
        //        _clmamt = decimal.Parse(row[1].ToString(), System.Globalization.NumberStyles.Currency);
        //        _rfdfamt = decimal.Parse(row[2].ToString(), System.Globalization.NumberStyles.Currency);
        //        _var = _clmamt - _rfdfamt;
        //        ReconDAL.insertRecon(_yrmo, _claimid, _clmamt, _rfdfamt, _var);
        //    }

        //    ReconDAL.updateTablesCF(_yrmo);
        //}
        //private static void insertReconCF(DataSet ds, string _yrmo)
        //{
        //    string _claimid;
        //    decimal _fclmamt, _frfdfamt, _var;

        //    foreach (DataRow row in ds.Tables["CFTemp"].Rows)
        //    {
        //        _claimid = row[0].ToString();
        //        _fclmamt = decimal.Parse(row[1].ToString(), System.Globalization.NumberStyles.Currency);
        //        _frfdfamt = decimal.Parse(row[2].ToString(), System.Globalization.NumberStyles.Currency);
        //        _var = _fclmamt - _frfdfamt;
        //        if (_var == 0)
        //        {
        //            ReconDAL.updateReconCF(_yrmo, _claimid, _fclmamt, _frfdfamt, _var);
        //        }
        //    }
        //}

        //public static void matchDFCl(string _yrmo)
        //{
        //    List<int> rid = new List<int>();
        //    rid = ReconDAL.matchCF(_yrmo);
        //    if (rid.Count != 0)
        //    {
        //        ReconDAL.updatematchCF(rid);
        //    }
        //    ReconDAL.calcTotalAmt(_yrmo, rid);
        //}

        //RX Recon

        public static void anthemRXrecon(string _yrmo)
        {
            decimal _frdamt = 0;
            decimal _anbtamt = 0;
            _frdamt = ReconDAL.getFRDData(_yrmo);
            if (_frdamt.Equals(-1))
            {
                throw (new Exception("No data from FRD. FRD may not have processed the Anthem bank Reconciliation.<br/>Contact FRD."));
            }
            _anbtamt = ReconDAL.getAnthemRX(_yrmo);
            ReconDAL.insertRXRecon(_yrmo, _frdamt, _anbtamt);
        }
        
        public static string prevYRMO(string yrmo)
        {
            yrmo = yrmo.Insert(4, "/");
            DateTime prevYRMO = Convert.ToDateTime(yrmo).AddMonths(-1);
            string prev_year = prevYRMO.Year.ToString();
            string prev_month = prevYRMO.Month.ToString();
            if (prev_month.Length == 1)
                prev_month = "0" + prev_month;
            string prev_yrmo = prev_year + prev_month;

            return prev_yrmo;
        }

        // CA Claims Recon

        public static void CAClaimsRecon(string yrmo)
        {
            CAClaimsDAL cobj = new CAClaimsDAL();

            cobj.SetAnthDup(yrmo);
            cobj.SetBOADup(yrmo);
            cobj.SetCurrentCF(yrmo);
            cobj.UpdatePrevCF(yrmo);
            cobj.SetReconciled(yrmo);
        }

        public static string CheckReconOrder(string yrmo, string source)
        {
            string latestYRMO = ReconDAL.GetLatestYRMO(source);
            string _yrmo = yrmo.Insert(4, "/");
            string _latestYRMO = latestYRMO.Insert(4, "/");
            string _monthList = "";
            string _prevyrmo = null;
            int _mDiff = Convert.ToDateTime(_yrmo).Month - Convert.ToDateTime(_latestYRMO).Month;
            int _YearDiff = Convert.ToDateTime(_yrmo).Year - Convert.ToDateTime(_latestYRMO).Year;
            int _months = Math.Abs(_mDiff + (12 * _YearDiff));
            if (_months > 1)
            {
                string _pramYrmo = yrmo;
                for (int i = 0; i < (_months - 1); i++)
                {                    
                    _prevyrmo = prevYRMO(_pramYrmo);
                    _monthList = _monthList + _prevyrmo + ", ";
                    _pramYrmo = _prevyrmo;
                }
                _monthList = _monthList.Remove(_monthList.LastIndexOf(','), 2);
            }          

            return _monthList;
        }
    }
}
