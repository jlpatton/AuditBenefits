
function ImportFileAutomatically(clientfile, serverpath)
{
    var objFSO = new ActiveXObject("Scripting.FileSystemObject");    
    
    try
    {
        var _file = clientfile;
        var _path = serverpath;  
        objFSO.CopyFile(_file, _path);
    }    
    finally
    {
        objFSO = null;
    }
}

function OpenExcel(filepath) 
{
    var objExcel = new ActiveXObject ("Excel.Application");
    
    try 
    {        
        var Book = objExcel.Workbooks.Open(filepath);
        objExcel.visible = true;
    }
    catch(err)
    { 
       objExcel.Quit();
    }
}

function SaveFile(content, filepath)
{
    var fso, s;
    fso = new ActiveXObject("Scripting.FileSystemObject");
    var appFolder = "C:\\EBATemp";    
    try 
    {        
        if(!fso.FolderExists(appFolder))
        {
            fso.CreateFolder(appFolder) 
        }
        s = fso.CreateTextFile(filepath, true);
        s.write(content);
        s.Close();
    }    
    finally
    {
        fso = null;
    }
}

function SaveTextFile(content, filepath)
{
    var fso, s, _txtcontent;
    fso = new ActiveXObject("Scripting.FileSystemObject");
    var appFolder = "C:\\EBATemp";    
    try 
    {
        if(!fso.FolderExists(appFolder))
        {
            fso.CreateFolder(appFolder) 
        }
        s = fso.CreateTextFile(filepath, true);
        _txtcontent = content.split("\n");
        for(var i=0; i < _txtcontent.length; i++)
        {
            s.writeline(_txtcontent[i]);
        }        
        s.Close();
    }    
    finally
    {        
        fso = null;
    }
}

function PrintExcel(filepath)
{
    var objExcel = new ActiveXObject ("Excel.Application");
    
    try 
    {
        var Book = objExcel.Workbooks.Open(filepath);   
        objExcel.DisplayAlerts = false;  
        var _count = Book.Worksheets.count;
        for(var i=1; i <= _count; i++)
        {
            Book.Worksheets(i).PageSetup.Orientation = 2;    
        }   
        Book.PrintOut();
        Book.Close();
    }   
    finally
    {
        objExcel.Quit();
    }
}

function PrintWord(filepath)
{
    var objWord = new ActiveXObject("Word.Application"); 
       
    try 
    {           
        objWord.visible = false;
        objWord.DisplayAlerts = false;
        objWord.Documents.Open(filepath);
        objWord.ActiveDocument.PrintOut();
        objWord.ActiveDocument.Close();             
    }
    finally
    {
        objWord.Quit();
    }
}


function DeleteFile(filepath)
{
    var fso, _file;
    fso = new ActiveXObject("Scripting.FileSystemObject");
        
    try 
    {
        _file = fso.GetFile(filepath);
        _file.Delete();
    }
    finally
    {
        fso = null;
    }
}

function InstallDocSchema(templateFile,schemaNamespace)
{
    var objWord = new ActiveXObject("Word.Application");     
    try 
    {         
        var schemaNamespaceUriObj = schemaNamespace;
        var schemaAliasObj = "EBAHRA";        
        objWord.Documents.Add();
        var document = objWord.ActiveDocument;  
        objWord.XMLNamespaces.Add(templateFile, 
                                        schemaNamespaceUriObj, schemaAliasObj, true);     
        objWord.XMLNamespaces(schemaNamespaceUriObj).AttachToDocument(document);         
        objWord.TaskPanes(5).visible = true;
        objWord.visible = true;
    }
    catch(err)
    { 
       objWord.Quit();
       alert(err.description);
    }
}

function DownloadTemplate(filepath,templateFile,schemaAlias,schemaUri)
{
    var objWord = new ActiveXObject("Word.Application");     
    
    try 
    {           
        var schemaNamespaceUriObj = schemaUri;
        var schemaAliasObj = schemaAlias;        
        objWord.DisplayAlerts = false;                
        objWord.Documents.Open(filepath);         
        var document = objWord.ActiveDocument; 
        objWord.XMLNamespaces.Add(templateFile, 
                                        schemaNamespaceUriObj, schemaAliasObj, true);
        objWord.XMLNamespaces(schemaNamespaceUriObj).AttachToDocument(document); 
        objWord.TaskPanes(5).visible = true;
        objWord.visible = true;
    }
    catch(err)
    { 
       alert(err.description);
       objWord.Quit();
    }
}

function OpenWord(filepath)
{
    var objWord = new ActiveXObject("Word.Application"); 
    
    try 
    {
        objWord.Documents.Open(filepath);
        objWord.visible = true;
    }
    catch(err)
    { 
       objWord.Quit();
    }
}


