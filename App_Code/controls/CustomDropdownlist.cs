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
    /// Display hierarchical SiteMapNodes in a 
    /// DropDownList
    /// </summary>
    public class CustomDropdownlist : WebControl, IPostBackDataHandler
    { 
        private string _listItems;

        /// <summary>
        /// The List of values to bind to Dropdownlist, seperated by comma.
        /// </summary>       

        public string ListItems
        {
            get
            {
                return _listItems;
            }
            set
            {
                _listItems = value;
            }
        }

        /// <summary>
        /// Gets or sets the selected list item
        /// </summary>
        public string SelectedValue
        {
            get
            {
                if (ViewState["SelectedValue"] == null)
                    return String.Empty;
                else
                    return (string)ViewState["SelectedValue"];
            }
            set { ViewState["SelectedValue"] = value; }
        }


        /// <summary>
        /// Recursively render child SiteMapNodes
        /// </summary>
        protected override void RenderContents(HtmlTextWriter writer)
        {
            string[] _itemArray = _listItems.Split(',');           
            foreach (string _item in _itemArray)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Value, _item);
                writer.RenderBeginTag(HtmlTextWriterTag.Option);
                writer.Write(_item);
                writer.RenderEndTag();
            } 
        }

        

        /// <summary>
        /// Add Name attribute in order to support
        /// retrieving postback data
        /// </summary>
        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Name, this.UniqueID);
            base.AddAttributesToRender(writer);
        }

        /// <summary>
        /// Render this control as a DropDownList
        /// </summary>
        protected override HtmlTextWriterTag TagKey
        {
            get
            {
                return HtmlTextWriterTag.Select;
            }
        }

        /// <summary>
        /// Recover posted value from posted form
        /// </summary>
        public bool LoadPostData(string postDataKey, System.Collections.Specialized.NameValueCollection postCollection)
        {
            string newValue = postCollection[postDataKey];
            if (newValue != SelectedValue)
            {
                SelectedValue = newValue;
                return true;
            }
            return false;
        }

        public void RaisePostDataChangedEvent()
        {
        }
    }
}
