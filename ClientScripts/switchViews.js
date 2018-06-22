function switchViews(obj,row)
{
    var div = document.getElementById(obj);
    var img = document.getElementById('img' + obj);
    
    if (div.style.display == "none")
    {
        div.style.display = "block";
        if (row == 'alt')
        {
            img.src = "../../styles/images/expanded1.gif";
        }
        else
        {
            img.src = "../../styles/images/expanded1.gif";
        }
        img.alt = "Close";
    }
    else
    {
        div.style.display = "none";
        if (row == 'alt')
        {
            img.src = "../../styles/images/collapsed1.gif";
        }
        else
        {
            img.src = "../../styles/images/collapsed1.gif";
        }
        img.alt = "Expand";
    }    
}
   


