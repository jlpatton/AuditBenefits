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

public partial class UserControls_FileUploadControl2 : System.Web.UI.UserControl
{
    //Fileupload control width
    private string fileUplStyle;
    //File Title Label control width
    private string lblStyle;
    //File title; Default is 'File:'
    private string fileTitle = "File :";
    //Has the user browsed for a file?
    private bool pGotFile;
    //The file extension of the Posted File
    private string pFileExt;
    //The File itself
    private HttpPostedFile pFilePost;
    //Is the user required to upload an image?
    private bool pRequired=false;
    //The validation group that the Custom Validator belongs to on the page
    private string pVgroup;
    //Extensions you allow to be uploaded
    private string[] pFileTypeRange;
    //Boolean indicator to check if file extension is allowed
    private bool Ind = false;

    public string FileUplStyle
    {
        set { fileUplStyle = value; }
    }
    public string LblStyle
    {
        set { lblStyle = value; }
    }
    public string FileTitle
    {
        set { fileTitle = value; }
    }
    public bool GotFile
    {
        get { return pGotFile; }
    }
    public string FileExt
    {
        get { return pFileExt; }
    }
    public HttpPostedFile FilePost
    {
        get { return pFilePost; }
    }
    public bool Required
    {
        set { pRequired = value; }
    }
    public string Vgroup
    {
        set { pVgroup = value; }
    }
    public string FileTypeRange
    {
        set { pFileTypeRange = value.ToString().Split(','); }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        //here we assign the validation group to the Custom Validator
        ErrorMsg.ValidationGroup = pVgroup;
        //setting visible propery of required field property
        RequiredFieldValidator1.Visible = pRequired;
        //setting style of File Upload control
        if ((fileUplStyle != null) || (fileUplStyle != String.Empty)) { FilUpl.Attributes.Add("style", fileUplStyle); }       
        //setting style of file title Label control
        if ((lblStyle != null) || (lblStyle != String.Empty)) { lblFile.Attributes.Add("style", lblStyle); }        
        //setting File title
        lblFile.Text = fileTitle;
    }

    protected void ErrorMsg_ServerValidate(object source, ServerValidateEventArgs args)
    {
        if (FilUpl.HasFile)
        {
            pGotFile = true;
            pFileExt = GetExtension(FilUpl.PostedFile.FileName);
            pFilePost = FilUpl.PostedFile;

            foreach (string str in pFileTypeRange)
            {
                if (String.Compare(str, pFileExt, true) == 0)
                {
                    Ind = true;
                }
            }

            if (!Ind)
            {
                args.IsValid = false;
                //ErrorMsg.Text = "The file format you specified is not supported";
                ErrorMsg.Text = "File is required in " + pFileTypeRange.GetValue(0) + " format!";
                //Stop the function
                return;
            }
        }
        else
        {
            //So if we get here the user has not browsed for a file
            pGotFile = false;
            //If you have stated that the user has to browse for a file.
            //then we must stop now and inform the user of such.
            if (pRequired)
            {
                args.IsValid = false;
                ErrorMsg.Text = "Please upload file";
            }
        }
    }
    /// <summary>
    /// This returns the file extension. 
    /// </summary>
    /// <param name="FileName">string</param>
    /// <returns>string: the file extension e.g. jpg</returns>
    private string GetExtension(string FileName)
    {
        string[] split = FileName.Split('.');
        string Extension = split[split.Length - 1];
        return Extension;
    }
}
