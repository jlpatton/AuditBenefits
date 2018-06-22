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

namespace EBA.Desktop
{
    /// <summary>
    /// Summary description for ObjectSource
    /// </summary>
    public class ObjectSource
    {
        static ObjectSource()
        {
            
        }
         public static List<string> getPlanGrpHTHHMO()
        {
            List<string> _pgrp = new List<string>();
            _pgrp.Add("LMO");
            _pgrp.Add("INT");

            return _pgrp;
        }
    }

   
}
