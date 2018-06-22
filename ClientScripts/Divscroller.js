 
function SetDivPosition()
{  
    var intY = document.getElementById("grddivscroll").scrollTop;  
    document.title = intY;  
    document.cookie = "yPos=!~" + intY + "~!";  
}

