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
using Shell32;

public partial class BrowseClientFolder : System.Web.UI.UserControl
{
    public string Text
    {
        get { return text_path.Text; }
        set { text_path.Text = value; }
    }

    public string Width
    {
        set { text_path.Width= Convert.ToInt32(value); }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        
    }

    protected void browse_folder_Click(object sender, EventArgs e)
    {
        Shell32.ShellClass shl = new Shell32.ShellClass();
        Shell32.Folder2 fld = (Shell32.Folder2)shl.BrowseForFolder(0, "Select a directory",
        0, System.Reflection.Missing.Value);        
        if(fld != null)       
        text_path.Text = fld.Self.Path;
    }
}
