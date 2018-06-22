function EditableDropDown(txtboxID, ddlID, regx)
{
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    txtbox = document.getElementById(txtboxID)
    ddl = document.getElementById(ddlID)
    var regex = regx    
    
    if (regex.test(txtbox.value))   
    {     
        prm._doPostBack(txtboxID, '');
    }
}

