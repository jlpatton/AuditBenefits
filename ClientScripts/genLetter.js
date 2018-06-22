function PrintLetters()
{
    var schemaNamespaceUriObj = "EBAnamespace";
    var schemaAliasObj = "EBAHRA"; 
    var templateUrl = "http://localhost:3124/EBADEV%20Final%20V4.2%20IPBA%20HRA%20NoVSS/HRA/LetterGenerator/Schemas/Employee.xml";     
    alert(templateUrl);
    try 
    {  
        var objWord = new ActiveXObject("Word.Application");     
        objWord.visible = false;      
        for(var i = 0; i < _Letters.length ; i++)
        {              
            objWord.Documents.Add();
            var document = objWord.ActiveDocument;                    
//            objWord.XMLNamespaces.Add(templateUrl, 
//                                    schemaNamespaceUriObj, schemaAliasObj, true);   
//            alert("Attach to doc XMLnamespace");   
//            objWord.XMLNamespaces(schemaNamespaceUriObj).AttachToDocument(document);
            alert("Writing letter");            
            objWord.write(_Letters[i]);            
            alert("Printing");
            objWord.ActiveDocument.PrintOut();
            objWord.ActiveDocument.Close();
            objWord.Quit();             
        }        
    }
    catch(err)
    { 
       alert("Error: " + err.description);
    }
    finally
    {
        objWord.Quit();
    }
}


