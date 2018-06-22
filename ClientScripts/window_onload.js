// JScript File

function initAll()
{
    var scroller = document.getElementById("scroller");
    var divscroller = document.getElementById("DivScroller");
     
    if(scroller)
    {
        sstchur_SmartScroller_Scroll();
    }  
    if(divscroller)
    {
        DivScroller__ResetScroll();
    }      
    sitemapstyler();
}

window.onload=initAll;
