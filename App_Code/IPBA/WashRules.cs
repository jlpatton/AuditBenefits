using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;


namespace EBA.Desktop.IPBA
{
    public class WashRules
    {
        public WashRules()
        {

        }

        public int washRuleADD(DateTime _effDt, string _yrmo, string plancode)
        {
            int _maxmonths, _monthDifference;
            bool _greaterThan15;

            //HTH - 90 days;  HMO - 60 days
            if(plancode.Contains("P5"))
                _maxmonths = 4;
            else
                _maxmonths = 3;

            _monthDifference = MonthDifference(_effDt, _yrmo);
            _greaterThan15 = checkPeriod(_effDt);

            //4 months debit for HTH and 3 months debit for HMO is maximum in all cases
            if (_monthDifference >= _maxmonths)
                return (_maxmonths);

            if (!_greaterThan15)
            {
                _monthDifference++;
            }

            return _monthDifference;
        }

        public int washRuleTERM(DateTime _effDt, string _yrmo, string plancode)
        {
            int _maxmonths, _monthDifference;
            bool _greaterThan15;

            //HTH - 90 days;  HMO - 60 days
            if (plancode.Contains("P5"))
                _maxmonths = 4;
            else
                _maxmonths = 3;

            _monthDifference = MonthDifference(_effDt, _yrmo);
            _greaterThan15 = checkPeriod(_effDt);

            //3 months credit for HTH and 2 months credit for HMO is maximum in all cases
            if (_monthDifference >= _maxmonths)
                return ((-1) * (_maxmonths - 1));

            _monthDifference = (-1) * _monthDifference;
            if (_greaterThan15)
            {
                _monthDifference++;
            }
            return _monthDifference;
        }

        public string washRuleCHG(DateTime _effDt, string _yrmo, string plancode)
        {
            int _monthDifference;
            bool _greaterThan15;
            string _rate = "";


            _monthDifference = MonthDifference(_effDt, _yrmo);
            _greaterThan15 = checkPeriod(_effDt);
            
            if (_monthDifference == 0)
            {
                if (_greaterThan15)
                {
                    _rate = "-1";
                }
                else
                {
                    _rate = "1";
                }
            }
            else if (_monthDifference == 1)
            {
                if (_greaterThan15)
                {
                    _rate = "1-";
                }
                else
                {
                    _rate = "A+1";
                }
            }
            else if (_monthDifference == 2)
            {
                if (_greaterThan15)
                {
                    _rate = "A+1";
                }
                else
                {
                    _rate = "2A+1";
                }
            }
            else if (_monthDifference == 3)
            {
                if (plancode.Contains("P5"))
                {
                    if (_greaterThan15)
                    {
                        _rate = "2A+1";
                    }
                    else
                    {
                        _rate = "3A+1";
                    }
                }
                else
                _rate = "2A+1"; //max 2 months HMO
            }
            else if (_monthDifference > 3)
            {               
                if (plancode.Contains("P5")) _rate = "3A+1"; //max 3 months for HTH
                    else _rate = "2A+1"; 
            }

            return _rate;
        }

        public int MonthDifference(DateTime _effDt, string _yrmo)
        {
            DateTime _yrmoDt;
            int _monthsApart;
            
            _yrmoDt = Convert.ToDateTime(_yrmo.Insert(4, "/"));
            _monthsApart = 12 * (_yrmoDt.Year - _effDt.Year) + _yrmoDt.Month - _effDt.Month;

            return _monthsApart;
        }

        protected bool checkPeriod(DateTime _eDate)
        {
            int _billingDay = _eDate.Day;
            bool _greater = false;

            if (_billingDay > 15)
            {
                _greater = true;
            }

            return _greater;
        }
    }
}