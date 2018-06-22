// JScript File

function DisplayAgedDate(txtboxID, labelID)
{
   
    var regx= /^\d+$/    
    
    element = document.getElementById(txtboxID)
    
    if (regx.test(element.value)) 
    {
        var agedDays = new Number(element.value);
        var dt = new Date(); 
        dt.setDate(dt.getDate() - agedDays);
        document.getElementById(labelID).innerHTML = "===> Date: " + (dt.getMonth() + 1) + "/" + dt.getDate() + "/" + dt.getFullYear();
    }
}