using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

/// <summary>
/// Summary description for ListObjectComparer
/// </summary>
/// 
namespace EBA.Desktop
{
    public class ListObjectComparer : IComparer<CodesTable>, IComparer<CodesData>
    {
        public ListObjectComparer(string p_PropertyName)
        {
            this.PropertyName = p_PropertyName;
        }

        private string _propertyName;

        public string PropertyName
        {
            get { return _propertyName; }
            set { _propertyName = value; }
        }
        // IComparer<CodesTable>,

        /// <summary>
        /// This comparer is used to sort the generic comparer
        /// The constructor sets the PropertyName that is used
        /// by reflection to access that property in the object to 
        /// object compare.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int Compare(CodesTable x, CodesTable y)
        {
            Type t = x.GetType();
            PropertyInfo val = t.GetProperty(this.PropertyName);
            if (val != null)
            {
                return Comparer.DefaultInvariant.Compare(val.GetValue(x, null), val.GetValue(y, null));
            }
            else
            {
                throw new Exception(this.PropertyName + " is not a valid property to sort on.  It doesn't exist in the Class.");
            }
        }

        public int Compare(CodesData x, CodesData y)
        {
            Type t = x.GetType();
            PropertyInfo val = t.GetProperty(this.PropertyName);
            if (val != null)
            {
                return Comparer.DefaultInvariant.Compare(val.GetValue(x, null), val.GetValue(y, null));
            }
            else
            {
                throw new Exception(this.PropertyName + " is not a valid property to sort on.  It doesn't exist in the Class.");
            }
        }
    }
}
