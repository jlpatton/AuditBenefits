// JScript File
function ShowSSNHistory(txtboxID)
{
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    var regx= /^\d{3}-\d{2}-\d{4}$/
    
    element = document.getElementById(txtboxID)
    if(element.value.length == 11)
    {
        if (regx.test(element.value))            
		    prm._doPostBack(txtboxID, '');
	}
}

function MaxMonthsIndicator(effdtID, yrmoID, plancdID, trancdID)
{
     var msg = "The effective date which you have entered exceeds the maximum number of retroactive payment periods allowed. \n " +
               "Please note this entry and refer to the output reports if you wish to extend the retro amount offline.";
     var regxdt= /^((0[1-9])|(1[0-2]))\/((0[1-9])|([12][0-9])|([3][01]))\/(\d{4})$/
     var regxyrmo = /^20\d\d(0[1-9]|1[012])$/
     var effdt = document.getElementById(effdtID).value;
     var yrmo = document.getElementById(yrmoID).value;
     plancd = document.getElementById(plancdID)     
     var plancode = plancd[plancd.selectedIndex].value;
     trancd = document.getElementById(trancdID)
     var trancode = trancd[trancd.selectedIndex].value;
     
     var errmsg="";
     
     if(!regxyrmo.test(yrmo))    
        errmsg = "Enter valid YRMO\n";
    
     if(plancode == "--Select--")
        errmsg += "Select HTH/Local HMO Plan Code\n";
     
     if(trancode == "--Select--")     
        errmsg += "Select Tran Code\n";
    
     if(errmsg != "")
        alert(errmsg);
        
     if(regxdt.test(effdt) && regxyrmo.test(yrmo)&& plancode != "--Select--" && trancode != "--Select--")
     {        
        var yrmodt = new Date(yrmo.substring(4)+ "/01/" + yrmo.substring(0,4)); 
        var effdt1 = new Date(effdt);
        var effdtDay = effdt1.getDate();
        
        var monthsApart = 12 * (yrmodt.getFullYear() - effdt1.getFullYear()) + yrmodt.getMonth() - effdt1.getMonth();      
        var maxmonths;
       
        if(plancode == "P5")
            maxmonths = 4;
        else
            maxmonths = 3;
         
        if(monthsApart == maxmonths)
        {               
            if(effdtDay < 15 && trancode.substring(0,1) != "D")
               alert (msg); 
            if(trancode.substring(0,1) == "D")
               alert (msg); 
        } 
        if(monthsApart > maxmonths)
        { 
            alert (msg); 
        }   
     }
}

function ScrollInto(objectID)
{
    window.onload= function()
    {
        sitemapstyler();
        var divscroller = document.getElementById("DivScroller");
        if(divscroller)
        {
            DivScroller__ResetScroll();
        }    
        document.getElementById(objectID).scrollIntoView(true);
    }
}

function ScrollOut(objectID)
{
    window.onload= function()
    {
        sitemapstyler();
        var divscroller = document.getElementById("DivScroller");
        if(divscroller)
        {
            DivScroller__ResetScroll();
        } 
    }
}

