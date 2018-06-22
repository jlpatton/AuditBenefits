function CheckAll(checkIDs)
{   
    var chkbox;
    
    for(var i=0; i<checkIDs.length;i++)
    {       
        chkbox = document.getElementById(checkIDs[i])
        chkbox.checked = true;
    }
}

function UnCheckAll(checkIDs)
{  
    var chkbox;
    
    for(var i=0; i<checkIDs.length;i++)
    {       
        chkbox = document.getElementById(checkIDs[i])
        chkbox.checked = false;
    }
}
//The Functions below are used to call a server side function when the text box loses focus Ram -3/6/2009
function CallMe(src,dest)
{ 
var ctrl = document.getElementById(src);
// call server side method
PageMethods.GetDob(ctrl.value, CallSuccess, CallFailed, dest);
}

// set the destination textbox value with the ContactName
function CallSuccess(res, destCtrl)
{ 
var dest = document.getElementById(destCtrl);
dest.value = res;
}

// alert message on some failure
function CallFailed(res, destCtrl)
{
alert(res.get_message());
}
/// End of the functions used for the text box lose focus event