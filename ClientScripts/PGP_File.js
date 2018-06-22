function PGPofFile(inputfileID, folderpathID)
{
    var fso, _inputfile, _outputfile, _folderpath, objFile;
    var objShell, commandtoRun, commandParms;
    
    fso = new ActiveXObject("Scripting.FileSystemObject");  
    objShell = new ActiveXObject("WScript.Shell"); 
        
    try
    {     
        _inputfile = document.getElementById(inputfileID).value;
        if(!fso.FileExists( _inputfile))
        {
            alert("Eligibilty File does not exists at " + inputfile);
        }
       
        _folderpath = document.getElementById(folderpathID).value;
        var temp = _folderpath.substring(_folderpath.length - 1);
        if ((temp == "\\") || (temp == "/"))
        {
            _folderpath.remove(_folderpath.length - 1);
        }
        if(!fso.FolderExists(_folderpath))
        {
            alert("PGP Folder does not exists at " + folderpath);
        }
            
        objFile = fso.GetFile(_inputfile);
        
        var today = new Date();
        var dd = today.getDate();
        var mm = today.getMonth()+1;//January is 0!
        var yyyy = today.getFullYear();
        if(dd<10){dd='0'+dd}
        if(mm<10){mm='0'+mm}
        
        _outputfile = fso.GetParentFolderName(objFile) + "\\HRA_" + yyyy+mm+dd + ".txt.pgp";
        if(fso.FileExists(_outputfile))
        {
            fso.GetFile(_outputfile).Delete(); 
        }
        
        if((fso.FileExists( _inputfile)) && (fso.FolderExists(_folderpath)))
        { 
            commandtoRun = _folderpath + "\\gpg.exe";
            
            commandParms = " --homedir \"" + _folderpath +
                                              "\" --always-trust --armor --recipient \"" + "Network_Security@putnaminv.com" +
                                              "\" --output \"" + _outputfile +
                                              "\" --encrypt \"" + _inputfile + "\"";
          
            objShell.Run(commandtoRun + commandParms,0, true);                        
        }        
    }
    catch(e)
    {
        alert("Error -"+ e.description);
    }
    finally
    {
        fso = null;
        objShell = null;
    }
}