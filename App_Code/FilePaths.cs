using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace EBA.Desktop
{
    /// <summary>
    /// Summary description for FilePaths
    /// </summary>
    public class FilePaths
    {
        public FilePaths()
        {            
        }

        public static string getFilePath(string _src)
        {
            string _turl = "";
            bool _servermap = false;
            string host = HttpContext.Current.Request.Url.Host;
            int port = HttpContext.Current.Request.Url.Port;

            switch (_src)
            {
                case "Schema":
                  _turl = "/HRA/LettersGenerator/Schemas/Employee.xml";
                  //  _turl = "Prod/HRA/LettersGenerator/Schemas/Employee.xml";
                    break;
                case "SchemaValid":
               _turl = "/HRA/LettersGenerator/Schemas/ValidationLetter.xml";
             // _turl = "Prod/HRA/LettersGenerator/Schemas/ValidationLetter.xml";
                    break;
                case "Templates":
                    _turl = "/HRA/LettersGenerator/Templates/";
                    _servermap = true;
                    break;
                case "Uploads":
                    _turl = "/uploads/";
                    _servermap = true;
                    break;
            }

/**************************** IMP remove comments while moving to server *******************************/
            if (_servermap)
            {                
                string appPath = HttpContext.Current.Request.ApplicationPath;
                _turl = appPath + _turl;
            }

            UriBuilder uriBuilder = new UriBuilder("http", host, port, _turl);
            string fullyQualifiedUrl = uriBuilder.Uri.AbsoluteUri;

            //return fullyQualifiedUrl;
            string _url = "";

            if (_servermap)
            {
                _url = HttpContext.Current.Server.MapPath(_turl);
                return _url.Replace("\\", "\\\\");
            }
            else
            {
                return fullyQualifiedUrl;
            }
        }
    }


}
