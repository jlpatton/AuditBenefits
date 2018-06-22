using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using EBA.Desktop;
using EBA.Desktop.Admin;
using EBA.Desktop.Audit;
using EBA.Desktop.HRA;
using System.Collections.Generic;

public partial class HRA_Maintenance_Pilot : System.Web.UI.Page
{    
    PilotRecord pOldrec = new PilotRecord();
    DepRecord dOldRec = new DepRecord();
    private const string ASCENDING = " ASC";
    private const string DESCENDING = " DESC";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Int32.Parse(Session["mid"].ToString()) == 0 || Session["mid"] == null)
        {
            Response.Redirect("~/Home.aspx");
        }        
        if (!Page.IsPostBack)
        {
            string uname = HttpContext.Current.User.Identity.Name.ToString();
            AdminBLL.InRole(uname, "M200", "M214");
            displayRecord();
            change_mode("r");
            displayTransactions();
            checkBenOrder();
        }
    }
    protected void Menu1_MenuItemClick(object sender, MenuEventArgs e)
    {
        MultiView1.ActiveViewIndex = Int32.Parse(e.Item.Value);
    }

    // Displays Employee record on page Load.
    protected void displayRecord()
    {
        PilotRecord pRec = null;
        string _essn = null;
        int _eempno = 0;

        PilotData pData = new PilotData();
        
        if (Session["ssn"] != null)
        {
            _essn = Session["ssn"].ToString();
        }
        else if (Session["empno"] != null)
        {
            _eempno = Convert.ToInt32(Session["empno"]);
        }
        else
        {
            Response.Redirect("~/HRA/Maintenance/Pilot_Search.aspx");
        }
        if (_essn!=null)
        {
            pRec = pData.getPilotData(_essn);
        }
        else if(_eempno!=0)
        {
            pRec = pData.getPilotData(_eempno);
        }
        bindPilotRecord(pRec);
        pOldrec = pRec;
    }

    /*Employee Methods */

    #region Employee Tab
    //gets and binds pilot records
    protected void bindPilotRecord(PilotRecord pRec1)
    {
        lblEmpHeading.Text = "Employee - " + pRec1.EmpNum.ToString() + "/"
                                + pRec1.LastName + ", " + pRec1.FirstName + " " + pRec1.MiddleInitial;

        txtEmpNo.Text = pRec1.EmpNum.ToString();
        txtFirst.Text = pRec1.FirstName;
        txtMI.Text = pRec1.MiddleInitial;
        txtLast.Text = pRec1.LastName;
        txtSSN.Text = pRec1.SSN.PadLeft(9,'0');
        string s = pRec1.SexCode;
        if (s.Equals("M"))
        {
            rdbSex.SelectedIndex = 0;
        }
        else if (s.Equals("F"))
        {
            rdbSex.SelectedIndex = 1;
        }
        if (!pRec1.DateBirth.Equals(DateTime.MinValue))
        {
            txtBirth.Text = pRec1.DateBirth.ToString("MM/dd/yyyy");
        }
        if (!pRec1.DeathDate.Equals(DateTime.MinValue))
        {
            txtDeath.Text = pRec1.DeathDate.ToString("MM/dd/yyyy");
        }
        if (!pRec1.PermDate.Equals(DateTime.MinValue))
        {
            txtPerm.Text = pRec1.PermDate.ToString("MM/dd/yyyy");
        }
        if (!pRec1.RetDate.Equals(DateTime.MinValue))
        {
            txtRetire.Text = pRec1.RetDate.ToString("MM/dd/yyyy");
        }
        
        string stats = pRec1.Status;
        if (stats.Equals("ACT"))
        {
            ddlStatus.SelectedIndex = 0;
        }
        else if (stats.Equals("TRM"))
        {
            ddlStatus.SelectedIndex = 1;
        }
        else if (stats.Equals("DTH"))
        {
            ddlStatus.SelectedIndex = 2;
        }

        txtAddr1.Text = pRec1.Address1;
        txtAddr2.Text = pRec1.Address2;       
        txtCity.Text = pRec1.City;
        txtState.Text = pRec1.State;
        txtZip.Text = pRec1.Zip;

        txtHRA.Text = pRec1.HRAAmount.ToString("#,#.00#"); 
        txtLump.Text = pRec1.LumpSum.ToString("#,#.00#");
        decimal hraAmt = pRec1.HRAAmount;
        decimal lumpAmt = pRec1.LumpSum;
        decimal total = hraAmt + lumpAmt;
        txtTHRA.Text = total.ToString("#,#.00#");       
    }

    protected void btnEdit_Click(object sender, EventArgs e)
    {
        change_mode("e");
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        clearMessages();
        if (Page.IsValid)
        {
            try
            {
                int _empno = Convert.ToInt32(txtEmpNo.Text);
                UpdateRecords(_empno);
                change_mode("r");                
            }
            catch (Exception ex)
            {
            }
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        clearMessages();
        displayRecord();
        change_mode("r");       
    }
    
    protected void check_Rstatus(object sender, ServerValidateEventArgs e)
    {
        string n_status = ddlStatus.SelectedItem.ToString();
        if (!n_status.Equals("TERMINATED"))
        {
            if (n_status.Equals("DIED"))
            {
                if (txtDeath.Text.Equals(""))
                {
                    e.IsValid = false;
                    ctvRetire.Text = " * You are changing status to DIED from TERMINATED!";                   
                }
                else
                {
                    e.IsValid = true;
                }
            }
            else
            {
                e.IsValid = false;
                ctvRetire.Text = " * You must change status if entering termination date!";
            }
        }
        else
        {
            e.IsValid = true;
        }

    }

    //checks if status is died on entering date of death
    protected void check_Dstatus(object sender, ServerValidateEventArgs e)
    {
        string n_status = ddlStatus.SelectedItem.ToString();
        if (!n_status.Equals("DIED"))
        {
            e.IsValid = false;
            ctvDeath.Text = " * You must change status if entering date of death!";
        }
        else
        {
            e.IsValid = true;
        }
    }

    //on change of status - checks retired date if status is retired, death dt if status id died
    protected void check_dates(object sender, ServerValidateEventArgs e)
    {
        string n_status = ddlStatus.SelectedItem.ToString();        
        if (n_status.Equals("TERMINATED"))
        {
            if (txtRetire.Text.Equals(""))
            {
                e.IsValid = false;
                ctvStatus.Text = " * You must enter retirement date if you are changing status to TERMINATED!";
            }
        }
        else if (n_status.Equals("DIED"))
        {
            if (!txtRetire.Text.Equals(""))
            {
                if (txtDeath.Text.Equals(""))
                {
                    e.IsValid = false;
                    ctvStatus.Text = " * You must enter date of death if you are changing status to DIED!";
                }
            }
            else if (txtDeath.Text.Equals(""))
            {
                e.IsValid = false;
                ctvStatus.Text = " * You must enter date of death if you are changing status to DIED!";
            }
        }
        else
        {
            e.IsValid = true;
        }
    }

    private void change_mode(string type)
    {
        switch (type)
        {
            case "r":
                txtAddr1.ReadOnly = true;
                txtAddr2.ReadOnly = true;
                txtBirth.ReadOnly = true;
                txtCity.ReadOnly = true;
                txtDeath.ReadOnly = true;
                txtEmpNo.ReadOnly = true;
                txtFirst.ReadOnly = true;                
                txtLast.ReadOnly = true;                
                txtMI.ReadOnly = true;
                txtPerm.ReadOnly = true;
                txtRetire.ReadOnly = true;
                txtSSN.ReadOnly = true;
                txtState.ReadOnly = true;                
                txtZip.ReadOnly = true;
                ddlStatus.Enabled = false;
                rdbSex.Enabled = false;
                btnSave.Visible = false;
                btnCancel.Visible = false;
                break;
            case "e":
                txtAddr1.ReadOnly = false;
                txtAddr2.ReadOnly = false;
                txtBirth.ReadOnly = false;
                txtCity.ReadOnly = false;
                txtDeath.ReadOnly = false;
                txtEmpNo.ReadOnly = false;
                txtFirst.ReadOnly = false;               
                txtLast.ReadOnly = false;                
                txtMI.ReadOnly = false;
                txtPerm.ReadOnly = false;
                txtRetire.ReadOnly = false;
                txtSSN.ReadOnly = false;
                txtState.ReadOnly = false;                
                txtZip.ReadOnly = false;
                ddlStatus.Enabled = true;
                rdbSex.Enabled = true;
                btnSave.Visible = true;
                btnCancel.Visible = true;
                break;
        }
    }

    protected void UpdateRecords(int _empno)
    {
        PilotRecord pUrec = new PilotRecord();        
        PilotData uData = new PilotData();
        PilotRecord finalUrec = uData.getPilotData(_empno);

        pUrec.EmpNum = Convert.ToInt32(txtEmpNo.Text);
        pUrec.SSN = Convert.ToInt64(txtSSN.Text).ToString();
        pUrec.FirstName = txtFirst.Text;
        pUrec.LastName = txtLast.Text;
        pUrec.MiddleInitial = txtMI.Text;

        string _sex = rdbSex.SelectedItem.Text;
        if (_sex.Equals("Male"))
        {
            pUrec.SexCode = "M";
        }
        else if (_sex.Equals("Female"))
        {
            pUrec.SexCode = "F";
        }

        string _status = ddlStatus.SelectedItem.Text;
        if (_status.Equals("ACTIVE"))
        {
            pUrec.Status = "ACT";
        }       
        else if (_status.Equals("TERMINATED"))
        {
            pUrec.Status = "TRM";
        }
        else if (_status.Equals("DIED"))
        {
            pUrec.Status = "DTH";
        }

        pUrec.DateBirth = Convert.ToDateTime(txtBirth.Text);

        if (!txtPerm.Text.Equals(""))
        {
            pUrec.PermDate = Convert.ToDateTime(txtPerm.Text);
        }
        if (!txtRetire.Text.Equals(""))
        {
            pUrec.RetDate = Convert.ToDateTime(txtRetire.Text);
        }
        if (!txtDeath.Text.Equals(""))
        {
            pUrec.DeathDate = Convert.ToDateTime(txtDeath.Text);
        }
        
        pUrec.Address1 = txtAddr1.Text;       
        pUrec.Address2 = txtAddr2.Text;        
        pUrec.City = txtCity.Text;
        pUrec.State = txtState.Text;
        pUrec.Zip = txtZip.Text;

        pUrec.LumpSum = Convert.ToDecimal(txtLump.Text);
        pUrec.HRAAmount = Convert.ToDecimal(txtHRA.Text);

        string addrType = "001";
        PilotData uData1 = new PilotData();
        try
        {
            uData1.updateEmployee(addrType, pUrec);
            pOldrec = uData1.getPilotData(pUrec.EmpNum);
            bindPilotRecord(pOldrec);
            auditEmpUpdates(finalUrec, pOldrec, finalUrec.EmpNum);
            string _info = "";

            if ((!txtDeath.Text.Equals("")) && finalUrec.DeathDate.Equals(DateTime.MinValue))
            {
                if (checkPutnamAmount())
                {
                    ValidationRecord vrec = new ValidationRecord();
                    vrec.createValidationRecordStatusDead(_empno);
                    ShowMessageBox("Validation Letter record for next possible Beneficiary owner will be generated. \n "
                                        + "If no Beneficiary is entered no record is generated.");
                    _info = "Validation Letter record for next possible Beneficiary owner will be generated. <br/> "
                                        + "If no Beneficiary is entered no record is generated.";

                }
                else
                {
                    ShowMessageBox("Amount in HRA funds is '0', so no Letter record was generated");
                    _info = "Amount in HRA funds is '0', so no Letter record was generated";
                }
            }

            if (!_info.Equals(""))
            {
                infoDiv1.Visible = true;
                lblInfo.Text = _info;
            }
        }
        catch (Exception ex)
        {
            bindPilotRecord(finalUrec);
        }
        finally
        {
            grdvAudit.DataBind();
            grdvDependants.DataBind();
            grdvTransactions.DataBind();
            grdvLetters1.DataBind();
            grdvltrPending.DataBind();
            grdvConfPen.DataBind();
        }
    }

    protected void auditEmpUpdates(PilotRecord oV, PilotRecord nV,int pKey)
    {
        PilotData aData = new PilotData(); 
        string sessionId= Session.SessionID;
        string moduleId = Session["mid"].ToString();
        string taskId = Session["taskId"].ToString();

        //Employee Audit
        //auditUserTask - Application level auditing
        //insertEmployeeAudit - Audit employee changes for history

        if (aData.createAuditObject(oV.FirstName, nV.FirstName))
        {
            Audit.auditUserTask(sessionId, moduleId, taskId, "HRA", "Pilot maintenance", "NRC", "Update", "Employee", "FirstName", pKey.ToString(),"", oV.FirstName, nV.FirstName);
            HRAdata.insertEmployeeAudit(pKey.ToString(), "FirstName", oV.FirstName, nV.FirstName);
        }
        if (aData.createAuditObject(oV.LastName, nV.LastName))
        {
            Audit.auditUserTask(sessionId, moduleId, taskId, "HRA", "Pilot maintenance", "NRC", "Update", "Employee", "LastName", pKey.ToString(), "", oV.LastName, nV.LastName);
            HRAdata.insertEmployeeAudit(pKey.ToString(), "LastName", oV.LastName, nV.LastName);
        }
        if (aData.createAuditObject(oV.SSN, nV.SSN))
        {
            Audit.auditUserTask(sessionId, moduleId, taskId, "HRA", "Pilot maintenance", "NRC", "Update", "Employee", "SSN", pKey.ToString(), "", oV.SSN, nV.SSN);
            HRAdata.insertEmployeeAudit(pKey.ToString(), "SSN", oV.SSN, nV.SSN);
        }
        if (aData.createAuditObject(oV.DateBirth, nV.DateBirth))
        {
            string oDob =  DBNull.Value.ToString();
            string nDob = DBNull.Value.ToString();
            if (!oV.DateBirth.Equals(DateTime.MinValue) )
            {
                oDob = oV.DateBirth.ToShortDateString(); 
            }
            if (!nV.DateBirth.Equals(DateTime.MinValue))
            {
                nDob = nV.DateBirth.ToShortDateString();
            }
            Audit.auditUserTask(sessionId, moduleId, taskId, "HRA", "Pilot maintenance", "NRC", "Update", "Employee", "DateBirth", pKey.ToString(), "", oDob, nDob);
            HRAdata.insertEmployeeAudit(pKey.ToString(), "DateBirth", oDob, nDob);
        }
        if (aData.createAuditObject(oV.MiddleInitial, nV.MiddleInitial))
        {
            Audit.auditUserTask(sessionId, moduleId, taskId, "HRA", "Pilot maintenance", "NRC", "Update", "Employee", "MiddleInitial", pKey.ToString(), "", oV.MiddleInitial, nV.MiddleInitial);
            HRAdata.insertEmployeeAudit(pKey.ToString(), "MiddleInitial", oV.MiddleInitial, nV.MiddleInitial);
        }
        if (aData.createAuditObject(oV.SexCode, nV.SexCode))
        {
            Audit.auditUserTask(sessionId, moduleId, taskId, "HRA", "Pilot maintenance", "NRC", "Update", "Employee", "SexCode", pKey.ToString(), "", oV.SexCode, nV.SexCode);
            HRAdata.insertEmployeeAudit(pKey.ToString(), "SexCode", oV.SexCode, nV.SexCode);
        }
        if (aData.createAuditObject(oV.Status, nV.Status))
        {
            Audit.auditUserTask(sessionId, moduleId, taskId, "HRA", "Pilot maintenance", "NRC", "Update", "Employee", "Status", pKey.ToString(), "", oV.Status, nV.Status);
            HRAdata.insertEmployeeAudit(pKey.ToString(), "Status", oV.Status, nV.Status);
        }
        if (aData.createAuditObject(oV.PermDate, nV.PermDate))
        {
            string oPerm = DBNull.Value.ToString();
            string nPerm = DBNull.Value.ToString();
            if (!oV.PermDate.Equals(DateTime.MinValue))
            {
                oPerm = oV.PermDate.ToShortDateString();
            }
            if (!nV.PermDate.Equals(DateTime.MinValue))
            {
                nPerm = nV.PermDate.ToShortDateString();
            }
            Audit.auditUserTask(sessionId, moduleId, taskId, "HRA", "Pilot maintenance", "NRC", "Update", "Employee", "PermDate", pKey.ToString(), "", oPerm, nPerm);
            HRAdata.insertEmployeeAudit(pKey.ToString(), "PermDate", oPerm, nPerm);
        }
        if (aData.createAuditObject(oV.RetDate, nV.RetDate))
        {
            string oRet = DBNull.Value.ToString();
            string nRet = DBNull.Value.ToString();
            if (!oV.RetDate.Equals(DateTime.MinValue))
            {
                oRet = oV.RetDate.ToShortDateString();
            }
            if (!nV.RetDate.Equals(DateTime.MinValue))
            {
                nRet = nV.RetDate.ToShortDateString();
            }
            Audit.auditUserTask(sessionId, moduleId, taskId, "HRA", "Pilot maintenance", "NRC", "Update", "Employee", "RetDate", pKey.ToString(), "", oRet, nRet);
            HRAdata.insertEmployeeAudit(pKey.ToString(), "RetDate", oRet, nRet);
        }
        if (aData.createAuditObject(oV.DeathDate, nV.DeathDate))
        {
            string oDeath = DBNull.Value.ToString();
            string nDeath = DBNull.Value.ToString();
            if (!oV.DeathDate.Equals(DateTime.MinValue))
            {
                oDeath = oV.DeathDate.ToShortDateString();
            }
            if (!nV.DeathDate.Equals(DateTime.MinValue))
            {
                nDeath = nV.DeathDate.ToShortDateString();
            }
            Audit.auditUserTask(sessionId, moduleId, taskId, "HRA", "Pilot maintenance", "NRC", "Update", "Employee", "DeathDate", pKey.ToString(), "", oDeath, nDeath);
            HRAdata.insertEmployeeAudit(pKey.ToString(), "DeathDate", oDeath, nDeath);
        }

        //Address Audit
        if (aData.createAuditObject(oV.Address1, nV.Address1))
        {
            Audit.auditUserTask(sessionId, moduleId, taskId, "HRA", "Pilot maintenance", "NRC", "Update", "Address", "Address1", pKey.ToString(), "", oV.Address1, nV.Address1);
            HRAdata.insertEmployeeAudit(pKey.ToString(), "Address1", oV.Address1, nV.Address1);
        }
        if (aData.createAuditObject(oV.Address2, nV.Address2))
        {
            Audit.auditUserTask(sessionId, moduleId, taskId, "HRA", "Pilot maintenance", "NRC", "Update", "Address", "Address2", pKey.ToString(), "", oV.Address2, nV.Address2);
            HRAdata.insertEmployeeAudit(pKey.ToString(), "Address2", oV.Address2, nV.Address2);
        }
        if (aData.createAuditObject(oV.City, nV.City))
        {
            Audit.auditUserTask(sessionId, moduleId, taskId, "HRA", "Pilot maintenance", "NRC", "Update", "Address", "City", pKey.ToString(), "", oV.City, nV.City);
            HRAdata.insertEmployeeAudit(pKey.ToString(), "City", oV.City, nV.City);
        }
        if (aData.createAuditObject(oV.State, nV.State))
        {
            Audit.auditUserTask(sessionId, moduleId, taskId, "HRA", "Pilot maintenance", "NRC", "Update", "Address", "State", pKey.ToString(), "", oV.State, nV.State);
            HRAdata.insertEmployeeAudit(pKey.ToString(), "State", oV.State, nV.State);
        }
        if (aData.createAuditObject(oV.Zip, nV.Zip))
        {
            Audit.auditUserTask(sessionId, moduleId, taskId, "HRA", "Pilot maintenance", "NRC", "Update", "Address", "Zip", pKey.ToString(), "", oV.Zip.ToString(), nV.Zip.ToString());
            HRAdata.insertEmployeeAudit(pKey.ToString(), "Zip", oV.Zip, nV.Zip);
        }

        //HRA Amount - not required Readonly Values
        //if (aData.createAuditObject(oV.LumpSum, nV.LumpSum))
        //{
        //    Audit.auditUserTask(sessionId, moduleId, taskId, "HRA", "Pilot maintenance", "NRC", "Update", "HRA_Amounts", "LumpSum", pKey.ToString(), "", oV.LumpSum.ToString(), nV.LumpSum.ToString());
        //    HRAdata.insertEmployeeAudit(pKey.ToString(), "MiddleInitial", oV.MiddleInitial, nV.MiddleInitial);
        //}
        //if (aData.createAuditObject(oV.HRAAmount, nV.HRAAmount))
        //{
        //    Audit.auditUserTask(sessionId, moduleId, taskId, "HRA", "Pilot maintenance", "NRC", "Update", "HRA_Amounts", "HRAAmount", pKey.ToString(), "", oV.HRAAmount.ToString(), nV.HRAAmount.ToString());
        //    HRAdata.insertEmployeeAudit(pKey.ToString(), "MiddleInitial", oV.MiddleInitial, nV.MiddleInitial);
        //}
    }

    #endregion

    /* Beneficiary Methods */

    #region Dependants Tab

    protected void lnkAddDependant_Click(object sender, EventArgs e)
    {
        divBenAdd.Visible = true;        
        lblEError.Text = "";
        txtBAddr.Text = "";
        txtBAddr2.Text = "";
        txtBBirth.Text = "";
        txtBCity.Text = "";
        txtBFirst.Text = "";
        txtBLast.Text = "";
        txtBMI.Text = "";
        txtBSSN.Text = "";
        txtBState.Text = "";
        txtBzip.Text = "";
        txtEDate.Text = "";
        cbxBAddrDiff.Checked = false;
        rdbBSex.ClearSelection();        
        ddlRelation.SelectedIndex = 0;
        clearMessages();

        //get available orders for new beneficiary
        PilotData pOrder = new PilotData();
        List<int> _maxOrder = pOrder.getBeneficiaryOrderNums(Convert.ToInt32(txtEmpNo.Text));
        int index = 0;
        for (int i = 1; i <= 10; i++)
        {
            if (index == 3)
            {
                break;
            }
            if (_maxOrder.Contains(i))
            {
                continue;
            }
            else
            {
                ddlOrder.Items.Add(i.ToString());
                index++;
            }
        }
    }
   
    protected void lnkEditDependant_Click(object sender, EventArgs e)
    {      
    }

    protected void bindDependant(DepRecord setRec)
    {         
        txtBBirthE.Text = setRec.DDateBirth;        
        txtBFirstE.Text = setRec.DFirstName;
        txtBLastE.Text = setRec.DLastName;
        txtBMIE.Text = setRec.DMiddleInitial;
        txtBSSNE.Text = setRec.DSSN.PadLeft(9,'0');
        Session["dssn"] = setRec.DSSN;
        txtBBirthE.Text = setRec.DDateBirth;       
        string _gender = null;
        _gender = setRec.DSexCode;

        if (_gender.Equals("M"))
        {
            rdbBSexE.SelectedIndex = 0;
        }
        else if (_gender.Equals("F"))
        {
            rdbBSexE.SelectedIndex = 1;
        }
        

        if(setRec.Owner)
        {
            cbxOwnerE.Checked = true;
        }
        else
        {
            cbxOwnerE.Checked = false;
        }

        if (setRec.EligibilityStatus)
        {
            cbxElegbE.Checked = true;
        }
        else
        {
            cbxElegbE.Checked = false;
        }

        if(cbxOwnerE.Checked)
        {
            if(setRec.OwnershipValidated && (!setRec.OwnershipValidDate.Equals("")))
            {                                
                ddlBValidE.SelectedIndex = 1;
                txtVDateE.Text = setRec.OwnershipValidDate;                
            }
        }        
        txtEDate.Text = setRec.OwnershipStartDate;
        if(!setRec.OwnershipEndDate.Equals(""))
        {
            BOwnerStopdtDiv.Visible = true;
            txtBStopDtE.Text = setRec.OwnershipEndDate;
        }
        else
        {
            //5/11/2009 display Bownerstopdatediv even when the 
            //owner chkbox is checked and relationship is "SP"
            if (setRec.Relation != "SP")
            {
                BOwnerStopdtDiv.Visible = false;

            }
            else if (setRec.Relation == "SP")
            {
                BOwnerStopdtDiv.Visible = true;
                txtBStopDtE.Visible = true;
            }
            //5/11/2009 make Bownerstopdatetxtbox visible even when the 
            //owner chkbox is checked and relationship is "SP"
            
            txtBStopDtE.Text = "";
        }

        if (setRec.OwnershipValidated)
        {
            ddlBValidE.SelectedIndex = 1;
            txtVDateE.Text = setRec.OwnershipValidDate;
        }

        ddlOrderE.Items.Clear();
        ddlOrderE.Items.Add(setRec.Order.ToString());
        ddlOrderE.SelectedIndex = 0;

        string _relation = null;
        _relation = setRec.Relation;
        switch (_relation)
        {
            case "SP":
                ddlRelationE.SelectedIndex = 1;
                break;
            case "CH":
                ddlRelationE.SelectedIndex = 2;
                break;            
        }

        if(_relation.Contains("OTH"))
        {
            ddlRelationE.SelectedIndex = 3;
            relOtherDivE.Visible = true;
            if (_relation.Length > 5)
            {
                txtOtherE.Text = _relation.Substring(6);
            }
        }
        else
        {
            relOtherDivE.Visible = false;
            txtOtherE.Text = "";
        }

        if(!setRec.DAddress1.Equals(""))
        {
            BAddrDivE.Visible = true;
            cbxBAddrDiffE.Checked = true;
            txtBAddrE.Text = setRec.DAddress1;
            txtBAddr2E.Text = setRec.DAddress2;
            txtBCityE.Text = setRec.DCity;
            txtBStateE.Text = setRec.DState;
            txtBZipE.Text = setRec.DZip;
            rfvAddrE.Enabled = true;
            rfvCityE.Enabled = true;
            rfvStateE.Enabled = true;
            rfvZipE.Enabled = true;
        }
        else
        {
            BAddrDivE.Visible = false;
            cbxBAddrDiffE.Checked = false;
            txtBAddrE.Text = "";
            txtBAddr2E.Text = "";
            txtBCityE.Text = "";
            txtBStateE.Text = "";
            txtBZipE.Text = "";
            rfvAddrE.Enabled = false;
            rfvCityE.Enabled = false;
            rfvStateE.Enabled = false;
            rfvZipE.Enabled = false;
        }
        int empno = Convert.ToInt32(txtEmpNo.Text);
        PilotData dObj1 = new PilotData();
        bool depAge24 = dObj1.validateBeneficiaryAge(empno, setRec.DSSN);
        if (depAge24 && _relation.Equals("CH"))
        //if (depAge24)
        {
            if (cbxOwnerE.Checked)
            {
                cbxOwnerE.Checked = false;
            }
            cbxElegbE.Checked = true;
            txtElegbEnotes.Text = " Beneficiary age reached 24. ";
            BOwnerStopdtDiv.Visible = true;
            txtBStopDtE.Text = Convert.ToDateTime(setRec.DDateBirth).AddYears(24).ToString("MM/dd/yyyy");
        }
        else
        {
            //3/16/2009 R.A  make sure the Dependant eligibility status check box is checked since the age is less than 24

            cbxElegbE.Checked = false; //false implies the dep is eligible for 

        }
    }
    void CheckDependantAge(int empno, string dpndssn, string dpnddob, DepRecord depRec)
        //R.A 3/12/2009
    {
        PilotData dObj1 = new PilotData();
        bool depAge24 = dObj1.validateBeneficiaryAge(empno, dpndssn);
       // if (depAge24 && _relation.Equals("CH"))  
        //CheckDependantAge = depAge24;
        if (depAge24)
        {
            // Change 5/7/2009 Added the condition depRec.relation != "SP"
            if (cbxOwnerE.Checked && depRec.Relation!="SP")
            {
                cbxOwnerE.Checked = false;
                depRec.Owner = false;
                txtElegbEnotes.Text = "Beneficiary age reached 24.";
                depRec.ElegibilityNotes = "Beneficiary age reached 24.";
            }
            cbxElegbE.Checked = true;
            depRec.EligibilityStatus = false;
            //5/14/2009 Andrea D - the eligibility notes field should not display
            //**Bene age reached 24** if Bene is a SP and also designated owner of the HRA account
            if (cbxOwnerE.Checked == true && depRec.Relation == "SP")
            {
                txtElegbEnotes.Text = "";
                depRec.ElegibilityNotes = "";
            }
            BOwnerStopdtDiv.Visible = true;
            if (depRec.Relation != "SP")
            {
                txtBStopDtE.Text = Convert.ToDateTime(dpnddob).AddYears(24).ToString("MM/dd/yyyy");
                depRec.OwnershipEndDate = Convert.ToDateTime(dpnddob).AddYears(24).ToString("MM/dd/yyyy");
            }
            else   // Change 5/7/2009 Added the condition depRec.relation != "SP"
            {
                txtBStopDtE.Text = "";
                depRec.OwnershipEndDate = "";
            }
        }
        else
        {
            if (cbxOwnerE.Checked)
            {
                cbxOwnerE.Checked = false;
                depRec.Owner = false;
            }
            cbxElegbE.Checked = true;
            depRec.EligibilityStatus = true;
            txtElegbEnotes.Text = " Beneficiary age has not reached 24. ";
            depRec.ElegibilityNotes = " Beneficiary age has not reached 24. ";
            BOwnerStopdtDiv.Visible = true;
            txtBStopDtE.Text = "";// Convert.ToDateTime(dpnddob).AddYears(24).ToString("MM/dd/yyyy");
            depRec.OwnershipEndDate = "";// Convert.ToDateTime(dpnddob).AddYears(24).ToString("MM/dd/yyyy");
        }
    }

    protected void btn1Save_Click(object sender, EventArgs e)
    {
        clearMessages();
        if (Page.IsValid && ddlOrderValidation())
        {
            try
            {
                lblEError.Text = "";

                DepRecord dRecAdd = new DepRecord();

                dRecAdd.DEmpNum = Convert.ToInt32(txtEmpNo.Text);
                dRecAdd.DSSN = Convert.ToInt64(txtBSSN.Text).ToString();
                dRecAdd.DDateBirth = txtBBirth.Text;
                dRecAdd.DFirstName = txtBFirst.Text;
                dRecAdd.DLastName = txtBLast.Text;
                if (!txtMI.Text.Equals(""))
                {
                    dRecAdd.DMiddleInitial = txtBMI.Text;
                }

                string _sexcd = rdbBSex.SelectedItem.Text.ToString();
                if (_sexcd.Equals("Male"))
                {
                    dRecAdd.DSexCode = "M";
                }
                else if (_sexcd.Equals("Female"))
                {
                    dRecAdd.DSexCode = "F";
                }

                dRecAdd.Order = Convert.ToInt32(ddlOrder.SelectedItem.ToString());
                string _rel = ddlRelation.SelectedItem.Text.ToString();

                if (_rel.Equals("OTH"))
                {
                    if (!txtOther.Text.Equals(""))
                    {
                        dRecAdd.Relation = "OTH" + " - " + txtOther.Text;
                    }
                    else
                    {
                        dRecAdd.Relation = "OTH";
                    }
                }
                else
                {
                    dRecAdd.Relation = ddlRelation.SelectedItem.Text.ToString();
                }

                if (cbxBAddrDiff.Checked)
                {
                    dRecAdd.DAddress1 = txtBAddr.Text;
                    dRecAdd.DAddress2 = txtBAddr2.Text;
                    dRecAdd.DCity = txtBCity.Text;
                    dRecAdd.DState = txtBState.Text;
                    dRecAdd.DZip = txtBzip.Text;
                }

                PilotData bObj = new PilotData();
                bObj.insertPilotDepData(dRecAdd);

                //Generate Beneficiary Change confirmation letter
                string _info = "";
                HRAdata.insertConfirmationRecord(dRecAdd.DEmpNum);
                ShowMessageBox("Confirmation Letter record is generated.");
                _info = "Confirmation Letter record is generated.";

                if (_info.Equals(""))
                {
                    infoDiv1.Visible = true;
                    lblInfo.Text = _info;
                }

                string sessionId = Session.SessionID;
                string moduleId = Session["mid"].ToString();
                string taskId = Session["taskId"].ToString();
                Audit.auditUserTask(sessionId, moduleId, taskId, "HRA", "Pilot maintenance", "NRC", "ADD", "Dependant", "SSN", dRecAdd.DEmpNum.ToString(), Convert.ToInt64(txtBSSN.Text).ToString(), "", Convert.ToInt64(txtBSSN.Text).ToString());

                divBenAdd.Visible = false;
                grdvDependants.DataBind();
                grdvDepAudit.DataBind();
                grdvLetters1.DataBind();
                grdvltrPending.DataBind();
            }
            catch (Exception ex)
            {
                errDivBenAdd.Visible = true;
                lblAError.Text = ex.Message;
            }

        }
        else if(!ddlOrderValidation())
        {
            errDivBenAdd.Visible = true;
            lblAError.Text = "Select Correct beneficiary order";
        }
    }

    protected void cbxBAddrDiff_OnCheckedChanged(object sender, EventArgs e)
    {
        txtBAddr.Enabled = false;
        txtBAddr2.Enabled = false;
        txtBCity.Enabled = false;
        txtBzip.Enabled = false;
        txtBState.Enabled = false;
        rfvBAddr.Enabled = false;
        rfvBCity.Enabled = false;
        rfvBState.Enabled = false;
        rfvBZip.Enabled = false;
        txtBAddr.Text = "";
        txtBAddr2.Text = "";
        txtBCity.Text = "";
        txtBState.Text = "";
        txtBzip.Text = "";

        if (cbxBAddrDiff.Checked)
        {
            txtBAddr.Enabled = true;
            txtBAddr2.Enabled = true;
            txtBCity.Enabled = true;
            txtBzip.Enabled = true;
            txtBState.Enabled = true;
            rfvBAddr.Enabled = true;
            rfvBCity.Enabled = true;
            rfvBState.Enabled = true;
            rfvBZip.Enabled = true;
        }
    }

    protected void cbxBAddrDiffE_OnCheckedChanged(object sender, EventArgs e)
    {        
        rfvAddrE.Enabled = false;
        rfvCityE.Enabled = false;
        rfvStateE.Enabled = false;
        rfvZipE.Enabled = false;
        txtBAddrE.Text = "";
        txtBAddr2E.Text = "";
        txtBCityE.Text = "";
        txtBStateE.Text = "";
        txtBZipE.Text = "";
        BAddrDivE.Visible = false;

        if (cbxBAddrDiffE.Checked)
        {
            BAddrDivE.Visible = true;
            rfvAddrE.Enabled = true;
            rfvCityE.Enabled = true;
            rfvStateE.Enabled = true;
            rfvZipE.Enabled = true;
        }
    }

    protected void btn2Save_Click(object sender, EventArgs e)
    {
        clearMessages();
        bool ownerstat = true;        
        try
        {
            if (ddlBValidE.SelectedItem.Text.Equals("Valid"))
            {
                if (!txtVDateE.Text.Equals(""))
                {
                    if (!cbxOwnerE.Checked || txtBStopDtE.Text.Equals(""))
                    {
                        throw (new Exception("Validation date is entered and Ownership box or Effective Date is incomplete."));
                    }
                }
                else
                {
                    throw (new Exception("Validation date is not entered."));
                }
            }
            if (!txtEDate.Text.Equals(""))
            {
                if (!cbxOwnerE.Checked && txtBStopDtE.Text.Equals(""))
                {
                    throw (new Exception("Ownership Effective date is entered but Owner checkbox is not checked."));
                }
            }
            if (cbxOwnerE.Checked)
            {
                if (!ddlBValidE.SelectedItem.Text.Equals("Valid"))
                {
                    throw (new Exception("Owner box is selected and Validation flag is not Valid."));                       
                }
                if (txtVDateE.Text.Equals(""))
                {
                    throw (new Exception("Owner box is selected and Validation date is missing."));                       
                }
                if (!txtEDate.Text.Equals("") && !txtBStopDtE.Text.Equals("") && !txtVDateE.Text.Equals(""))
                {
                    throw (new Exception("Stop date entered. Not eligible for Ownership."));
                }
                if (txtEDate.Text.Equals("") && !txtVDateE.Text.Equals(""))
                {
                    throw (new Exception("Owner box is selected and Effective date is missing."));
                }
                ownerstat = validateOwner();
            } 
            if (Page.IsValid && ownerstat)
            {
                PilotData pdobj = new PilotData();
                DepRecord dRec = new DepRecord();
                DepRecord dORec = new DepRecord();
                dORec = pdobj.getPilotDepData(Convert.ToInt32(txtEmpNo.Text), Session["dssn"].ToString());
                dRec.DEmpNum = Convert.ToInt32(txtEmpNo.Text);
                dRec.DSSN = Convert.ToInt64(txtBSSNE.Text).ToString();
                dRec.DDateBirth = txtBBirthE.Text;
                dRec.DFirstName = txtBFirstE.Text;
                dRec.DLastName = txtBLastE.Text;
                if (!txtBMIE.Text.Equals(""))
                {
                    dRec.DMiddleInitial = txtBMIE.Text;
                }

                string _sexcd = rdbBSexE.SelectedItem.Text.ToString();
                if (_sexcd.Equals("Male"))
                {
                    dRec.DSexCode = "M";
                }
                else if (_sexcd.Equals("Female"))
                {
                    dRec.DSexCode = "F";
                }

                dRec.Order = Convert.ToInt32(ddlOrderE.SelectedItem.ToString());
                string _rel = ddlRelationE.SelectedItem.Text.ToString();

                if (_rel.Equals("OTH"))
                {
                    if (!txtOtherE.Text.Equals(""))
                    {
                        dRec.Relation = "OTH" + " - " + txtOtherE.Text;
                    }
                    else
                    {
                        dRec.Relation = "OTH";
                    }
                }
                else
                {
                    dRec.Relation = ddlRelationE.SelectedItem.Text.ToString();
                }

                string atype = "";
                if (cbxBAddrDiffE.Checked)
                {
                    atype = "004";
                    dRec.DAddress1 = txtBAddrE.Text;
                    dRec.DAddress2 = txtBAddr2E.Text;
                    dRec.DCity = txtBCityE.Text;
                    dRec.DState = txtBStateE.Text;
                    dRec.DZip = txtBZipE.Text;
                }

                if (cbxElegbE.Checked)
                {
                    dRec.EligibilityStatus = true;
                    dRec.ElegibilityNotes = txtElegbEnotes.Text;
                }
                else
                {
                    dRec.EligibilityStatus = false;
                    dRec.ElegibilityNotes = "";
                }
                if (cbxOwnerE.Checked)
                {
                    dRec.Owner = true;
                    dRec.OwnershipStartDate = txtEDate.Text;
                }
                else if (!txtEDate.Text.Equals("") && !txtBStopDtE.Text.Equals(""))
                {
                    dRec.OwnershipStartDate = txtEDate.Text;
                }
                else
                {
                    dRec.Owner = false;
                    dRec.OwnershipStartDate = "";
                }
                if (ddlBValidE.SelectedItem.Text.Equals("Valid"))
                {
                    dRec.OwnershipValidated = true;
                    dRec.OwnershipValidDate = txtVDateE.Text;
                }
                else
                {
                    dRec.OwnershipValidated = false;
                    dRec.OwnershipValidDate = "";
                }
                // changes WO request 5/7/2009 Do not assign a stop date
                // if relationship is a spouse

                if (dRec.Relation != "SP")
                {
                    dRec.OwnershipEndDate = txtBStopDtE.Text.ToString();
                }
                else
                {
                    dRec.OwnershipEndDate = "";
                }

                //End of change 5/7/2009
                PilotData bObj = new PilotData();
                bObj.updateDependant(atype, dRec, Session["dssn"].ToString());
                // getDepData(dRec.DEmpNum,dRec.DSSN);
                // Added thefollowing lines to trigger the change in dependant eligibility if the age changes 24
                CheckDependantAge(dRec.DEmpNum, dRec.DSSN, dRec.DDateBirth,dRec);
                bObj.updateDependant(atype, dRec, Session["dssn"].ToString());
                grdvDependants.DataBind();
                string _info = "";
                if (!txtDeath.Text.Equals(""))
                {
                    if (checkPutnamAmount())
                    {
                        if (!txtBStopDtE.Text.Equals(""))
                        {
                            ValidationRecord vrec = new ValidationRecord();
                            vrec.createValidationRecordStatusOwnershipStopped(Convert.ToInt32(txtEmpNo.Text), dRec.DSSN);
                            ShowMessageBox("Validation Letter record for next possible Beneficiary owner will be generated. \n "
                                            + "If no Beneficiary is entered no record is generated.");
                            _info = "Validation Letter record for next possible Beneficiary owner will be generated.<br/> "
                                            + "If no Beneficiary is entered no record is generated.";
                        }
                    }
                    else
                    {
                        ShowMessageBox("Amount in HRA funds is '0', so no Letter record was generated");
                        _info = "Amount in HRA funds is '0', so no Letter record was generated";
                    }
                }
                if (_info.Equals(""))
                {
                    infoDiv1.Visible = true;
                    lblInfo.Text = _info;
                }
                divBenEdit.Visible = false;
                auditDepUpdates(dORec, dRec, Convert.ToInt32(txtEmpNo.Text));
                grdvDependants.DataBind();
                grdvDepAudit.DataBind();
                grdvLetters1.DataBind();
                grdvltrPending.DataBind();
                grdvConfPen.DataBind();
            }
        }
        catch (Exception ex)
        {
            errorDivBen.Visible = true;
            lblEError.Text = "Error Updating Dependants Record - " + ex.Message;
        }        
            
    }

    protected void cbxOwnerE_OnCheckedChanged(Object sender, EventArgs e)
    {
        //5/11/2009 - HRA Bene stop date Make it visible even when the 
        // dependendt is a spouse and checked as owner Andrea and Todd
        clearMessages();
        bool _valid = true;
        try
        {
            if (!txtBStopDtE.Text.Equals(""))
            {
                cbxOwnerE.Checked = false;
                throw (new Exception("Stop date entered. Not eligible for Ownership."));
            }
            if (!cbxOwnerE.Checked)
            {
                if (!txtEDate.Text.Equals(""))
                {
                    BOwnerStopdtDiv.Visible = true;
                }
            }
            else
            {
                if (ddlRelationE.SelectedItem.Text != "SP")
                //5/11/2009 - HRA Bene stop date div Make it visible even when the
               // owner ship box is checked so they can edit in future - Andrea
                {
                    BOwnerStopdtDiv.Visible = false;
                }
                _valid = validateOwnerOncheck();
            }
            if (!_valid)
            {
                cbxOwnerE.Checked = false;
            }
        }
        catch (Exception ex)
        {
            errorDivBen.Visible = true;
            lblEError.Text = ex.Message;
        }     
    }

    protected void cbxElegbE_OnCheckedChanged(Object sender, EventArgs e)
    {
        bool _valid = true;
        if (!cbxElegbE.Checked)
        {
            txtElegbEnotes.Visible = false;
        }
        else
        {
            txtElegbEnotes.Visible = true;
            rfvElgbNote.Enabled = true;
        }
        if (!_valid)
        {
            cbxOwnerE.Checked = false;
        }
    }

    protected bool validateOwnerOncheck()
    {
        int empno = Convert.ToInt32(txtEmpNo.Text);
        int ordNum = Convert.ToInt32(ddlOrderE.SelectedItem.Text.ToString());
        PilotData dObj = new PilotData();
        bool ownercnt = dObj.validateBeneficiaryOwner(empno);
        bool depAge24 = dObj.validateBeneficiaryAge(empno, Convert.ToInt64(txtBSSNE.Text).ToString());
        bool ownerElegb = dObj.validateBeneficiaryElegb(empno, Convert.ToInt64(txtBSSNE.Text).ToString());
        bool ownerStpdate = true;
        bool ownerEffdate = true;        

        if (ordNum > 1)
        {
            ownerStpdate = dObj.validateBeneficiaryOwnerStopdt(empno, ordNum);
            ownerEffdate = dObj.validateBeneficiaryOwnerEffdt(empno, ordNum);
        }
        bool ownerstat = true;
        string err = "";
        try
        {
            if (ownerElegb)
            {
                if (txtVDateE.Text.Equals(""))
                {
                    ownerstat = false;
                    err = "Owner box is selected and Validation date is missing.";
                }
                else if (ownerStpdate && ownerEffdate)
                {

                    if (depAge24 && ddlRelationE.SelectedItem.Text.Equals("CH"))
                    {
                        ownerstat = false;
                        err = " Beneficiary is already age 24, Not eligible for ownership.";
                        if (cbxOwnerE.Checked)
                        {
                            cbxOwnerE.Checked = false;
                        }
                        cbxElegbE.Checked = true;
                        txtElegbEnotes.Text = " Beneficiary age reached 24. ";
                    }
                    else
                    {
                        string ddate = txtDeath.Text.ToString();
                        if (ddate.Equals(""))
                        {
                            err += " You defined an owner but did not enter a date of death for pilot.";
                            ownerstat = false;
                        } 
                    }
                }
                else if ((!ownerEffdate) && (!ownerStpdate))
                {
                    err = " Not eligible - Please check the Order for ownership.";
                    ownerstat = false;
                }
                else if (ownercnt && ownerEffdate && (!ownerStpdate))
                {
                    ownerstat = false;
                    err = " Owner already defined and Ownership stop date is not entered.";
                }
            }
            else
            {
                err = " Not eligible for ownership!";
                ownerstat = false;
            }
            if (!err.Equals(""))
            {
                ShowMessageBox(err);
                errorDivBen.Visible = true;
                lblEError.Text = err;
            }
            return ownerstat;
        }
        catch (Exception ex)
        {
            ownerstat = false;
            return ownerstat;
        }        
    }
    
    //Verify if there is an Owner already defined or selected benificiary is 24 age 
    //and Date of employee death is enetered

    protected bool validateOwner()
    {
        int empno = Convert.ToInt32(txtEmpNo.Text);
        int ordNum = Convert.ToInt32(ddlOrderE.SelectedItem.Text.ToString());
        PilotData dObj = new PilotData();
        bool ownercnt = dObj.validateBeneficiaryOwner(empno);
        bool depAge24 = dObj.validateBeneficiaryAge(empno, Convert.ToInt64(txtBSSNE.Text).ToString());
        bool ownerElegb = dObj.validateBeneficiaryElegb(empno, Convert.ToInt64(txtBSSNE.Text).ToString());
        bool ownerStpdate = true;
        bool ownerEffdate = true;
        string stopdt = "";

        if (ordNum > 1)
        {
            stopdt = dObj.BeneficiaryOwnerStopdt(empno, ordNum);
            ownerStpdate = dObj.validateBeneficiaryOwnerStopdt(empno, ordNum);
            ownerEffdate = dObj.validateBeneficiaryOwnerEffdt(empno, ordNum);
        }
        bool ownerstat = false;
        string err = "";
        try
        {
            if (ownerElegb)
            {
                if (ownerStpdate && ownerEffdate)
                {

                    if (depAge24 && ddlRelationE.SelectedItem.Text.Equals("CH"))
                    {
                        ownerstat = false;
                        err = " Beneficiary is already age 24, Not eligible for ownership.";
                        if (cbxOwnerE.Checked)
                        {
                            cbxOwnerE.Checked = false;
                        }
                        cbxElegbE.Checked = true;
                        txtElegbEnotes.Text = " Beneficiary age reached 24. ";
                    }
                    else
                    {
                        string effdate = txtEDate.Text.ToString();
                        string ddate = txtDeath.Text.ToString();
                        ownerstat = true;
                        if (effdate.Equals(""))
                        {
                            err = " You must enter owners effective date.";
                            ownerstat = false;
                        }
                        else if (ordNum > 1)
                        {
                            if (!(Convert.ToDateTime(stopdt) < Convert.ToDateTime(effdate)))
                            {
                                err = " Effective date should be the date after the Ownership stop date.";
                                ownerstat = false;
                            }
                        }
                        if (ddate.Equals(""))
                        {
                            err += "<br/> You defined an owner but did not enter a date of death for pilot.";
                            ownerstat = false;
                        }
                        else if (!(Convert.ToDateTime(ddate) < Convert.ToDateTime(effdate)))
                        {
                            err += "<br/> Effective date should be the date after the death date of employee.";
                            ownerstat = false;
                        }

                    }
                }
                else if ((!ownerEffdate) && (!ownerStpdate))
                {
                    err = " Not eligible - Please check the Order for ownership.";
                    ownerstat = false;
                }
                else if (ownercnt && ownerEffdate && (!ownerStpdate))
                {
                    ownerstat = false;
                    err = " Owner already defined and Ownership stop date is not entered.";
                }
            }
            else
            {
                err = " Not eligible for ownership.";
                ownerstat = false;
            }
            if (!err.Equals(""))
            {
                ShowMessageBox(err);
                errorDivBen.Visible = true;
                lblEError.Text = err;
            }
            
            return ownerstat;
        }
        catch (Exception ex)
        {
            ownerstat = false;
            return ownerstat;
        }
    }
    
    protected void checkRelation(object sender, EventArgs e)
    {
        string rtext = ddlRelation.SelectedItem.Text.ToString();
        relOtherDiv.Visible = false;
        if (rtext.Equals("OTH"))
        {
            relOtherDiv.Visible = true;
        }
    }

    protected void validDate(object sender, EventArgs e)
    {
        string vtext = ddlBValidE.SelectedItem.Text.ToString();
        rfvVDateE.Enabled = false;
        if (vtext.Equals("Valid"))
        {
            rfvVDateE.Enabled = true;
        }
    }

    protected void ddlRelationE_SelectedIndexchange(object sender, EventArgs e)
    {
        relOtherDivE.Visible = false;
        if(ddlRelationE.SelectedItem.Text.Equals("OTH"))
        {
            relOtherDivE.Visible = true;
        }
        //5/11/2009 - HRA Bene stop date Populate stop date
        // oif user selects CH or OTH as dep - Andrea
       if (ddlRelationE.SelectedItem.Text != "SP")
        {
            if (Convert.ToDateTime(txtBBirthE.Text) < DateTime.Now.AddYears(-24))
            {
                txtBStopDtE.Text = Convert.ToDateTime(txtBBirthE.Text).AddYears(24).ToString("MM/dd/yyyy");
            }
            
        }
        if (ddlRelationE.SelectedItem.Text == "SP")
        {
            txtBStopDtE.Text = "";
        }
    }

    protected void grdvDependants_rowCommand(Object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Select")
        {
            clearFormE();
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow gr = grdvDependants.Rows[index];
            string dssn = Server.HtmlDecode(gr.Cells[2].Text);
            divBenEdit.Visible = true;
            lblEError.Text = "";
            Session["dssn"] = "";
            int _empNo = Convert.ToInt32(txtEmpNo.Text);
            getDepData(_empNo, dssn);
        }
    }

    protected void grdvDependants_onDeleting(Object sender, GridViewDeleteEventArgs e)
    {
        clearMessages();
        try
        {
            int index = e.RowIndex;
            GridViewRow gr = grdvDependants.Rows[index];
            string dssn = Server.HtmlDecode(gr.Cells[2].Text);
            int _empNo = Convert.ToInt32(txtEmpNo.Text);
            string sessionId = Session.SessionID;
            string moduleId = Session["mid"].ToString();
            string taskId = Session["taskId"].ToString();
            string _info = "";

            HRAdata.insertConfirmationRecord(_empNo);            
            ShowMessageBox("Confirmation Letter record is generated.");
            _info = "Confirmation Letter record is generated.";
                
            if (_info.Equals(""))
            {
                infoDiv1.Visible = true;
                lblInfo.Text = _info;
            }
            Audit.auditUserTask(sessionId, moduleId, taskId, "HRA", "Pilot maintenance", "NRC", "Delete", "Dependant", "SSN", _empNo.ToString(), dssn, dssn, "");
        }
        catch (Exception ex)
        {
            errDivBenAdd.Visible = true;
            lblAError.Text = ex.Message;
        }
        finally
        {
            grdvDependants.DataBind();
            grdvDepAudit.DataBind();
            checkBenOrder();
        }
    }        

    protected void getDepData(int _eNum, string _dSsn)
    {
        PilotData depRecord = new PilotData();
        dOldRec = depRecord.getPilotDepData(_eNum, _dSsn);
        if (!dOldRec.Equals(null))
        {
            bindDependant(dOldRec);
        }
        change_modeDependant("r");
    }

    protected void btn2Edit_Click(object sender, EventArgs e)
    {
        clearMessages();
        change_modeDependant("e");
    }

    protected void btn1Cancel_Click(object sender, EventArgs e)
    {
        clearMessages();
        clearForm();
    }

    protected void btn2Cancel_Click(object sender, EventArgs e)
    {
        clearMessages();
        clearFormE();
    }

    protected void btn2Close_Click(object sender, EventArgs e)
    {
        clearMessages();
        clearFormE();
    }

    protected void clearForm()
    {
        divBenAdd.Visible = false;
        relOtherDiv.Visible = false;        
        lblownerError.Visible = false;        
        txtBAddr.Text = "";
        txtBAddr2.Text = "";
        txtBBirth.Text = "";
        txtBCity.Text = "";
        txtBFirst.Text = "";
        txtBLast.Text = "";
        txtBMI.Text = "";
        txtBSSN.Text = "";
        txtBState.Text = "";
        txtBzip.Text = "";        
        rdbBSex.ClearSelection();
        errDivBenAdd.Visible = false;
        lblAError.Text = "";
        ddlOrder.SelectedIndex = 0;
        ddlRelation.SelectedIndex = 0;        
        ddlOrder.Items.Clear();
        cbxBAddrDiff.Checked = false;
    }

    protected void clearFormE()
    {
        txtBAddrE.Text = "";
        txtBAddr2E.Text = "";
        txtBBirthE.Text = "";
        txtBCityE.Text = "";
        txtBFirstE.Text = "";
        txtBLastE.Text = "";
        txtBMIE.Text = "";
        txtBSSNE.Text = "";
        txtBStateE.Text = "";
        txtBZipE.Text = "";
        rdbBSexE.ClearSelection();
        txtEDate.Text = "";
        cbxOwnerE.Checked = false;
        cbxElegbE.Checked = false;
        txtElegbEnotes.Text = "";
        ddlBValidE.SelectedIndex = 0;
        txtVDateE.Text = "";
        txtBStopDtE.Text = "";
        ddlOrderE.Items.Clear();
        ddlRelationE.SelectedIndex = 0;
        txtOtherE.Text = "";
        cbxBAddrDiffE.Checked = false;
        errorDivBen.Visible = false;
        lblEError.Text = "";
        BOwnerStopdtDiv.Visible = false;
        relOtherDivE.Visible = false;
        BAddrDivE.Visible = false;
        divBenEdit.Visible = false;
        rfvVDateE.Enabled = false;
        rfvElgbNote.Enabled = false;
        change_modeDependant("r");
    }

    private void change_modeDependant(string type)
    {
        switch (type)
        {
            case "e":
                txtBSSNE.ReadOnly = false;
                ddlOrderE.Enabled = true;
                txtBFirstE.ReadOnly = false;
                txtBMIE.ReadOnly = false;
                txtBLastE.ReadOnly = false;
                txtBBirthE.ReadOnly = false;
                ddlRelationE.Enabled = true;
                txtOtherE.ReadOnly = false;
                rdbBSexE.Enabled = true;
                cbxOwnerE.Enabled = true;
                txtEDate.ReadOnly = false;
                txtBStopDtE.ReadOnly = false;
                ddlBValidE.Enabled = true;
                txtVDateE.ReadOnly = false;
                cbxElegbE.Enabled = true;
                cbxBAddrDiffE.Enabled = true;
                txtBAddrE.ReadOnly = false;
                txtBAddr2E.ReadOnly = false;
                txtBCityE.ReadOnly = false;
                txtBStateE.ReadOnly = false;
                txtBZipE.ReadOnly = false;
                btn2Save.Visible = true;
                btn2Cancel.Visible = true;
                btn2Close.Visible = false;
                break;
            case "r":
                txtBSSNE.ReadOnly = true;
                ddlOrderE.Enabled = false;
                txtBFirstE.ReadOnly = true;
                txtBMIE.ReadOnly = true;
                txtBLastE.ReadOnly = true;
                txtBBirthE.ReadOnly = true;
                ddlRelationE.Enabled = false;
                txtOtherE.ReadOnly = true;
                rdbBSexE.Enabled = false;
                cbxOwnerE.Enabled = false;
                txtEDate.ReadOnly = true;
                txtBStopDtE.ReadOnly = true;
                ddlBValidE.Enabled = false;
                txtVDateE.ReadOnly = true;
                cbxElegbE.Enabled = false;
                cbxBAddrDiffE.Enabled = false;
                txtBAddrE.ReadOnly = true;
                txtBAddr2E.ReadOnly = true;
                txtBCityE.ReadOnly = true;
                txtBStateE.ReadOnly = true;
                txtBZipE.ReadOnly = true;
                btn2Save.Visible = false;
                btn2Cancel.Visible = false;
                btn2Close.Visible = true;
                break;
        }
    }

    protected void auditDepUpdates(DepRecord oV, DepRecord nV, int pKey)
    {
        PilotData aData = new PilotData();
        string sessionId = Session.SessionID;
        string moduleId = Session["mid"].ToString();
        string taskId = Session["taskId"].ToString();

        //Dependant Audit - code changed in Anthem for audit       

        if (aData.createAuditObject(oV.DSSN, nV.DSSN))
        {
            Audit.auditUserTask(sessionId, moduleId, taskId, "HRA", "Pilot maintenance", "NRC", "Update", "Dependant", "SSN", pKey.ToString(), nV.DSSN, oV.DSSN, nV.DSSN);
        }        
        if (aData.createAuditObject(oV.DFirstName, nV.DFirstName))
        {
            Audit.auditUserTask(sessionId, moduleId, taskId, "HRA", "Pilot maintenance", "NRC", "Update", "Dependant", "FirstName", pKey.ToString(), nV.DSSN, oV.DFirstName, nV.DFirstName);
        }
        if (aData.createAuditObject(oV.DLastName, nV.DLastName))
        {
            Audit.auditUserTask(sessionId, moduleId, taskId, "HRA", "Pilot maintenance", "NRC", "Update", "Dependant", "LastName", pKey.ToString(), nV.DSSN, oV.DLastName, nV.DLastName);
        }
        if (aData.createAuditObject(oV.DMiddleInitial, nV.DMiddleInitial))
        {
            string oMI = DBNull.Value.ToString();
            string nMI = DBNull.Value.ToString();
            if (!oV.DMiddleInitial.Equals(""))
            {
                oMI = oV.DMiddleInitial;
            }
            if (!nV.DMiddleInitial.Equals(""))
            {
                nMI = nV.DMiddleInitial;
            }
            Audit.auditUserTask(sessionId, moduleId, taskId, "HRA", "Pilot maintenance", "NRC", "Update", "Dependant", "MiddleInitial", pKey.ToString(), nV.DSSN, oMI, nMI);
        }
        if (aData.createAuditObject(oV.DDateBirth, nV.DDateBirth))
        {
            string oDob = DBNull.Value.ToString();
            string nDob = DBNull.Value.ToString();
            if (!oV.DDateBirth.Equals(""))
            {
                oDob = oV.DDateBirth;
            }
            if (!nV.DDateBirth.Equals(""))
            {
                nDob = nV.DDateBirth;
            }
            Audit.auditUserTask(sessionId, moduleId, taskId, "HRA", "Pilot maintenance", "NRC", "Update", "Dependant", "DateBirth", pKey.ToString(), nV.DSSN, oDob, nDob);
        }
        
        if (aData.createAuditObject(oV.DSexCode, nV.DSexCode))
        {
            Audit.auditUserTask(sessionId, moduleId, taskId, "HRA", "Pilot maintenance", "NRC", "Update", "Dependant", "SexCode", pKey.ToString(), nV.DSSN, oV.DSexCode, nV.DSexCode);
        }
        if (aData.createAuditObject(oV.Order, nV.Order))
        {
            Audit.auditUserTask(sessionId, moduleId, taskId, "HRA", "Pilot maintenance", "NRC", "Update", "Dependant", "Order", pKey.ToString(), nV.DSSN, oV.Order.ToString(), nV.Order.ToString());
        }
        if(aData.createAuditObject(oV.Relation, nV.Relation))
        {
            Audit.auditUserTask(sessionId, moduleId, taskId, "HRA", "Pilot maintenance", "NRC", "Update", "Dependant", "Relation", pKey.ToString(), nV.DSSN, oV.Relation, nV.Relation);
        }
        if (aData.createAuditObject(oV.Owner, nV.Owner))
        {
            string oOwner = DBNull.Value.ToString();
            string nOwner = DBNull.Value.ToString();
            string colName = "owner";
            if (oV.Owner)
            {
                oOwner = "True";
            }
            else
            {
                oOwner = "False";
            }
            if (nV.Owner)
            {
                nOwner = "True";                
            }
            else
            {
                nOwner = "False";
            }
            Audit.auditUserTask(sessionId, moduleId, taskId, "HRA", "Pilot maintenance", "NRC", "Update", "Dependant", "Owner", pKey.ToString(), nV.DSSN, oOwner, nOwner);
        }
        if (aData.createAuditObject(oV.EligibilityStatus, nV.EligibilityStatus))
        {
            string oElegb = DBNull.Value.ToString();
            string nElegb = DBNull.Value.ToString();
            string colName = "ownerNotEligible";
            if (oV.EligibilityStatus)
            {
                oElegb = "True";
            }
            else
            {
                oElegb = "False";
            }
            if (nV.EligibilityStatus)
            {
                nElegb = "True";
            }
            else
            {
                nElegb = "False";
            }
            Audit.auditUserTask(sessionId, moduleId, taskId, "HRA", "Pilot maintenance", "NRC", "Update", "Dependant", colName, pKey.ToString(), nV.DSSN, oElegb, nElegb);
        }
        if (aData.createAuditObject(oV.ElegibilityNotes, nV.ElegibilityNotes))
        {           
            string colName = "ownerNotEligibleNotes";
            if (nV.Relation == "SP" && nV.Owner == true)
            {
                nV.ElegibilityNotes = "";
            }
            Audit.auditUserTask(sessionId, moduleId, taskId, "HRA", "Pilot maintenance", "NRC", "Update", "Dependant", colName, pKey.ToString(), nV.DSSN, oV.ElegibilityNotes, nV.ElegibilityNotes);
        }
        if (aData.createAuditObject(oV.OwnershipStartDate, nV.OwnershipStartDate))
        {
            string oOstart = DBNull.Value.ToString();
            string nOstart = DBNull.Value.ToString();
            if (!oV.OwnershipStartDate.Equals(""))
            {
                oOstart = oV.OwnershipStartDate;
            }
            if (!nV.OwnershipStartDate.Equals(""))
            {
                nOstart = nV.OwnershipStartDate;
            }
            Audit.auditUserTask(sessionId, moduleId, taskId, "HRA", "Pilot maintenance", "NRC", "Update", "Dependant", "OwnershipStartDate", pKey.ToString(), nV.DSSN, oOstart, nOstart);
        }
        if (aData.createAuditObject(oV.OwnershipEndDate,nV.OwnershipEndDate))
        {
            string oOEnd = DBNull.Value.ToString();
            string nOEnd = DBNull.Value.ToString();
            if (!oV.OwnershipEndDate.Equals(""))
            {
                oOEnd = oV.OwnershipEndDate;
            }
            if (!nV.OwnershipEndDate.Equals(""))
            {
                nOEnd = nV.OwnershipEndDate;
            }
            Audit.auditUserTask(sessionId, moduleId, taskId, "HRA", "Pilot maintenance", "NRC", "Update", "Dependant", "OwnershipEndDate", pKey.ToString(), nV.DSSN, oOEnd, nOEnd);
        }
        if (aData.createAuditObject(oV.OwnershipValidated, nV.OwnershipValidated))
        {
            string oValid = DBNull.Value.ToString();
            string nValid = DBNull.Value.ToString();
            
            if (oV.OwnershipValidated)
            {
                oValid = "True";
            }
            else
            {
                oValid = "False";
            }
            if (nV.OwnershipValidated)
            {
                nValid = "True";
            }
            else
            {
                nValid = "False";
            }
            Audit.auditUserTask(sessionId, moduleId, taskId, "HRA", "Pilot maintenance", "NRC", "Update", "Dependant", "OwnershipValidated", pKey.ToString(), nV.DSSN, oValid, nValid);
        }
        if (aData.createAuditObject(oV.OwnershipValidDate, nV.OwnershipValidDate))
        {
            string oVdate = DBNull.Value.ToString();
            string nVdate = DBNull.Value.ToString();
            if (!oV.OwnershipValidDate.Equals(""))
            {
                oVdate = oV.OwnershipValidDate;
            }
            if (!nV.OwnershipValidDate.Equals(""))
            {
                nVdate = nV.OwnershipValidDate;
            }
            Audit.auditUserTask(sessionId, moduleId, taskId, "HRA", "Pilot maintenance", "NRC", "Update", "Dependant", "OwnershipValidDate", pKey.ToString(), nV.DSSN, oVdate, nVdate);
        }

        //Address Audit
        if (aData.createAuditObject(oV.DAddress1, nV.DAddress1))
        {
            Audit.auditUserTask(sessionId, moduleId, taskId, "HRA", "Pilot maintenance", "NRC", "Update", "Address", "Dep Address1", pKey.ToString(), nV.DSSN, oV.DAddress1, nV.DAddress1);
        }
        if (aData.createAuditObject(oV.DAddress2, nV.DAddress2))
        {
            Audit.auditUserTask(sessionId, moduleId, taskId, "HRA", "Pilot maintenance", "NRC", "Update", "Address", "Dep Address2", pKey.ToString(), nV.DSSN, oV.DAddress2, nV.DAddress2);
        }
        if (aData.createAuditObject(oV.DCity, nV.DCity))
        {
            Audit.auditUserTask(sessionId, moduleId, taskId, "HRA", "Pilot maintenance", "NRC", "Update", "Address", "Dep City", pKey.ToString(), nV.DSSN, oV.DCity, nV.DCity);
        }
        if (aData.createAuditObject(oV.DState, nV.DState))
        {
            Audit.auditUserTask(sessionId, moduleId, taskId, "HRA", "Pilot maintenance", "NRC", "Update", "Address", "Dep State", pKey.ToString(), nV.DSSN, oV.DState, nV.DState);
        }
        if (aData.createAuditObject(oV.DZip, nV.DZip))
        {
            Audit.auditUserTask(sessionId, moduleId, taskId, "HRA", "Pilot maintenance", "NRC", "Update", "Address", "Dep Zip", pKey.ToString(), nV.DSSN, oV.DZip, nV.DZip);
        }

    }

    protected void ddlOrder_Indexchange(Object sender, EventArgs e)
    {
        clearMessages();
        bool _check = ddlOrderValidation();
        if (!_check)
        {
            errDivBenAdd.Visible = true;
            lblAError.Text = "Select Correct beneficiary order";
        }
    }

    private bool ddlOrderValidation()
    {       
        PilotData pd1 = new PilotData();
        int _maxorder = pd1.getBeneficiaryOrders(Convert.ToInt32(txtEmpNo.Text));
        int _selected = Convert.ToInt32(ddlOrder.SelectedItem.Text);
        if (_selected != _maxorder + 1)
        {
            return false;
        }
        else
        {
            return true;
        }
        pd1 = null;
    }

    #endregion

    /* Employee Transactions */

    #region TransactionTab

    protected void displayTransactions()
    {
        DataSet dsTrans = new DataSet();
        //dsTrans = HRAdata.empTransactions(Convert.ToInt64(txtSSN.Text).ToString());
        //changed on 3/20/2009 to exclude the convert.ToInt64 which is removing the 0 in front of the empssn 
        dsTrans = HRAdata.empTransactions(txtSSN.Text.ToString());
        DataView dv = new DataView();
        if (dsTrans.Tables.Count > 0)
        {
            dv = dsTrans.Tables[0].DefaultView;
            dv.Sort = "YRMO DESC,[Tran Date] DESC";
            grdvTransactions.DataSource = dv;
            grdvTransactions.DataBind();   
        }  
    }

    protected void lnk_genTranRpt_OnClick(object sender, EventArgs e)
    {       
        string filename;
        string[] sheetnames;
        string[][] titles, colsFormat;
        int[][] colsWidth;
        CreateExcelRpt xlobj = new CreateExcelRpt();
        string _empno = txtEmpNo.Text;
        string _emplname = txtFirst.Text + " " + txtLast.Text;
        //string _empssn = Convert.ToInt64(txtSSN.Text).ToString();
        string _empssn = txtSSN.Text.ToString();
        DataSet dsFinal = new DataSet();
        dsFinal = HRAdata.empTransactions(_empssn);
        DataSet dsTemp = new DataSet();
        DataTable obNewDt = new DataTable(); 

        if (dsFinal.Tables.Count > 0)
        {
            //Now add a new row at the end to display the the  Sum of Putnam, Putnam Adj Amounts and Wageworks amount  R.A. 3/23/2009
            DataRow row;
            row = dsFinal.Tables[0].NewRow();
            row["Tran Code"] = "** TOTAL:";
            row["Putnam Amount"] = dsFinal.Tables[0].Compute("SUM([Putnam Amount])", string.Empty);
            row["PutnamAdj Amount"] = dsFinal.Tables[0].Compute("SUM([PutnamAdj Amount])", string.Empty);
            row["Wageworks Amount"] = dsFinal.Tables[0].Compute("SUM([WageWorks Amount])", string.Empty);
            //Now calculate the difference between sum of putnam ammounts and putnam adj amount and Wageworks amount R.A. 3/23/2009
            double sumPutnam = 0.0;
            double sumPutnamAdj = 0.0;
            double sumWageworksamt = 0.0;
            double displaysum = 0.0;
            if ((dsFinal.Tables[0].Compute("SUM([Putnam Amount])", string.Empty)) != DBNull.Value)
                sumPutnam = Convert.ToDouble(dsFinal.Tables[0].Compute("SUM([Putnam Amount])", string.Empty));
            else
                sumPutnam = 0;
            if ((dsFinal.Tables[0].Compute("SUM([PutnamAdj Amount])", string.Empty)) != DBNull.Value)
                sumPutnamAdj = Convert.ToDouble(dsFinal.Tables[0].Compute("SUM([PutnamAdj Amount])", string.Empty));
            else
                sumPutnamAdj = 0;
            if ((dsFinal.Tables[0].Compute("SUM([WageWorks Amount])", string.Empty)) != DBNull.Value)
                sumWageworksamt = Convert.ToDouble(dsFinal.Tables[0].Compute("SUM([WageWorks Amount])", string.Empty));
            else
                sumWageworksamt = 0;

            displaysum = sumPutnam + sumPutnamAdj + sumWageworksamt;
            dsFinal.Tables[0].Rows.Add(row);
            //Now add a new row at the end to display the the difference between Sum of Putnam and Putnam Adj Amounts and Wageworks amount  R.A. 3/23/2009
            row = dsFinal.Tables[0].NewRow();
            row["Tran Code"] = "** (Sum of Putnam and Putnam Adj. amount) + (Sum of Wageworks Amount):";
            //Now add add new row with the sum difference amount
            if (displaysum == 0.0)
            {
                row["Tran Date"] = Convert.ToString("$0.00");
            }
            else
            {
                row["PutnamAdj Amount"] = (displaysum);
            }
            //add a new row
            dsFinal.Tables[0].Rows.Add(row);
            //reset the variables
            sumPutnam = 0.0;
            sumPutnamAdj = 0.0;
            sumWageworksamt = 0.0;
            displaysum = 0.0;
          }
        {
            DataView dv1 = new DataView();
            dv1 = dsFinal.Tables[0].DefaultView;
            dv1.Sort = "YRMO DESC,[Tran Date] DESC";
           obNewDt = dv1.Table.Clone();
            int idx = 0;
            string[] strColNames = new string[obNewDt.Columns.Count];

            foreach (DataColumn col in obNewDt.Columns)
            {
                strColNames[idx++] = col.ColumnName;
            }

            IEnumerator viewEnumerator = dv1.GetEnumerator();

            while (viewEnumerator.MoveNext())
            {
                DataRowView drv = (DataRowView)viewEnumerator.Current;
                DataRow dr = obNewDt.NewRow();
                try
                {
                    foreach (string strName in strColNames)
                    {
                        dr[strName] = drv[strName];
                    }
                }
                catch (Exception ex)
                {                    
                }
                obNewDt.Rows.Add(dr);
            }
        }

        dsTemp.Tables.Add(obNewDt);
        filename = _empno + "_Transactions";
        titles = new string[1][];
        sheetnames = new string[1];
        colsFormat = new string[1][];
        colsWidth = new int[1][];
        titles[0] = new string[] { "All Transactions for Employee - " + _empno + " Employee Name - " + _emplname + " Employee SSN - " + _empssn + " Date Report Run : " + DateTime.Now.ToString()};
        sheetnames[0] = "Transactions";
        colsFormat[0] = new string[] { "string", "string", "string", "string", "currency", "currency", "currency" };
        colsWidth[0] = new int[] { 45, 80, 140, 60, 60, 60, 60 };
        xlobj.ExcelXMLRpt(dsTemp, filename, sheetnames, titles, colsFormat, colsWidth, String.Empty);
        
        
    }

    protected void grdvTransactions_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdvTransactions.PageIndex = e.NewPageIndex;
        displayTransactions();
    }

    protected void grdvTransactions_Sorting(object sender, GridViewSortEventArgs e)
    {
        gridviewSort(e.SortExpression, "trans");
    }

    void gridviewSort(string sortExpression, string source)
    {
        try
        {
            if (GridViewSortDirection == SortDirection.Ascending)
            {
                GridViewSortDirection = SortDirection.Descending;
                SortGridView(sortExpression, DESCENDING, source);
            }
            else
            {
                GridViewSortDirection = SortDirection.Ascending;
                SortGridView(sortExpression, ASCENDING, source);
            }
        }
        catch (Exception ex)
        {

        }
    }

    public SortDirection GridViewSortDirection
    {
        get
        {
            if (ViewState["sortDirection"] == null)
                ViewState["sortDirection"] = SortDirection.Ascending;

            return (SortDirection)ViewState["sortDirection"];
        }
        set { ViewState["sortDirection"] = value; }
    }    

    private void SortGridView(string sortExpression, string direction, string src)
    {
        DataTable dt;
        DataView dv;
        if (src.Equals("trans"))
        {
            string _empno = txtEmpNo.Text;
            dt = HRAdata.empTransactions(_empno).Tables[0];
            dv = new DataView(dt);
            dv.Sort = sortExpression + direction;
            grdvTransactions.DataSource = dv;
            grdvTransactions.DataBind();
        }
    }

    #endregion    

    #region LettersTab

    protected void lnkrePrint_onclick(object sender, EventArgs e)
    {
        clearMessages();

        LinkButton btn = (LinkButton)sender;
        GridViewRow row = (GridViewRow)btn.NamingContainer;
        int _lgid = Convert.ToInt32(grdvLetters1.DataKeys[row.RowIndex].Value);        

        LetterGen gObj = new LetterGen("EBAnamespace");
        Type cstype = this.GetType();

        try
        {

            string[] _Letter = gObj.reprintLetters(_lgid);
            ClientScriptManager cs = Page.ClientScript;

            string _filepath = "C:\\EBATemp\\";
            string _file = _filepath + "Print.xml";


            int _seq = 0;

            foreach (string _lt in _Letter)
            {

                try
                {
                    if (!cs.IsClientScriptBlockRegistered(_seq + "SaveWord"))
                        cs.RegisterClientScriptBlock(typeof(Page), _seq + "SaveWord", "SaveTextFile('" + _lt.Replace(Environment.NewLine, "") + "', '" + _file.Replace("\\", "\\\\") + "');", true);

                    if (!cs.IsClientScriptBlockRegistered(_seq + "PrintWord"))
                        cs.RegisterClientScriptBlock(typeof(Page), _seq + "PrintWord", "PrintWord('" + _file.Replace("\\", "\\\\") + "');", true);
                }
                finally
                {
                    EndProcess();
                    if (!cs.IsClientScriptBlockRegistered(_seq + "DeleteFile"))
                        cs.RegisterClientScriptBlock(typeof(Page), _seq + "DeleteFile", "DeleteFile('" + _file.Replace("\\", "\\\\") + "');", true);
                }
                _seq++;
            }

        }
        catch (Exception ex)
        {
            errorDivl.Visible = true;
            lbl_errorl.Text = "Some error occured while printing letters - " + ex.Message;
        }
        finally
        {
            
        }

    }

    //Pending Validation Letters
    protected void lnkPrint_onclick(Object sender, EventArgs e)
    {
        clearMessages();

        LinkButton btn = (LinkButton)sender;
        GridViewRow row = (GridViewRow)btn.NamingContainer;
        int _lgid = Convert.ToInt32(grdvltrPending.DataKeys[row.RowIndex].Value);

        string _empNum = grdvltrPending.Rows[row.RowIndex].Cells[1].Text;
        string _dpndssn = grdvltrPending.Rows[row.RowIndex].Cells[2].Text;
        string _dpndRel = grdvltrPending.Rows[row.RowIndex].Cells[3].Text;
        string _letterType = "";

        switch (_dpndRel)
        {
            case "SP":
                _letterType = "HRB1";
                break;
            case "CH":
                _letterType = "HRB2";
                break;
        }

        LetterGen gObj = new LetterGen("EBAnamespace");
        Type cstype = this.GetType();

        try
        {
            int _version = LettersGenDAL.getTemplateVersion(_letterType);
            string[] _Letter = gObj.generateBenValidationLetter(Convert.ToInt32(_empNum), _dpndssn, _letterType, _version, _lgid);
            ClientScriptManager cs = Page.ClientScript;

            string _filepath = "C:\\EBATemp\\";
            string _file = _filepath + "Print.xml";


            int _seq = 0;

            foreach (string _lt in _Letter)
            {

                try
                {
                    if (!cs.IsClientScriptBlockRegistered(_seq + "SaveWord"))
                        cs.RegisterClientScriptBlock(typeof(Page), _seq + "SaveWord", "SaveTextFile('" + _lt.Replace(Environment.NewLine, "") + "', '" + _file.Replace("\\", "\\\\") + "');", true);

                    if (!cs.IsClientScriptBlockRegistered(_seq + "PrintWord"))
                        cs.RegisterClientScriptBlock(typeof(Page), _seq + "PrintWord", "PrintWord('" + _file.Replace("\\", "\\\\") + "');", true);
                }
                finally
                {
                    EndProcess();
                    if (!cs.IsClientScriptBlockRegistered(_seq + "DeleteFile"))
                        cs.RegisterClientScriptBlock(typeof(Page), _seq + "DeleteFile", "DeleteFile('" + _file.Replace("\\", "\\\\") + "');", true);
                }
                _seq++;
            }

        }
        catch (Exception ex)
        {
            errorDivl.Visible = true;
            lbl_errorl.Text = "Some error occured while printing letters - " + ex.Message;
        }
        finally
        {
            grdvltrPending.DataBind();            
        }
    }

    //Pending Confirmation Letters
    protected void lnkConfPrint_onclick(Object sender, EventArgs e)
    {
        clearMessages();

        LinkButton btn = (LinkButton)sender;
        GridViewRow row = (GridViewRow)btn.NamingContainer;
        int _lgid = Convert.ToInt32(grdvConfPen.DataKeys[row.RowIndex].Value);

        string _empNum = grdvConfPen.Rows[row.RowIndex].Cells[1].Text;
        string _letterType = "HRC1";

        LetterGen gObj = new LetterGen("EBAnamespace");
        Type cstype = this.GetType();

        try
        {
            int _version = LettersGenDAL.getTemplateVersion(_letterType);
            string[] _Letter = gObj.generateLetter(_empNum, _letterType, _version, _lgid);
            ClientScriptManager cs = Page.ClientScript;

            string _filepath = "C:\\EBATemp\\";
            string _file = _filepath + "Print.xml";


            int _seq = 0;

            foreach (string _lt in _Letter)
            {

                try
                {
                    if (!cs.IsClientScriptBlockRegistered(_seq + "SaveWord"))
                        cs.RegisterClientScriptBlock(typeof(Page), _seq + "SaveWord", "SaveTextFile('" + _lt.Replace(Environment.NewLine, "") + "', '" + _file.Replace("\\", "\\\\") + "');", true);

                    if (!cs.IsClientScriptBlockRegistered(_seq + "PrintWord"))
                        cs.RegisterClientScriptBlock(typeof(Page), _seq + "PrintWord", "PrintWord('" + _file.Replace("\\", "\\\\") + "');", true);
                }
                finally
                {
                    EndProcess();
                    if (!cs.IsClientScriptBlockRegistered(_seq + "DeleteFile"))
                        cs.RegisterClientScriptBlock(typeof(Page), _seq + "DeleteFile", "DeleteFile('" + _file.Replace("\\", "\\\\") + "');", true);
                }
                _seq++;
            }

        }
        catch (Exception ex)
        {
            errorDivl.Visible = true;
            lbl_errorl.Text = "Some error occured while printing letters - " + ex.Message;
        }
        finally
        {
            grdvConfPen.DataBind();            
        }
    }

    #endregion

    private void clearMessages()
    {
        errDivBenAdd.Visible = false;
        errorDivBen.Visible = false;
        errorDivl.Visible = false;
        infoDiv1.Visible = false;
        infoDiv2.Visible = false;
        lblInfo.Text = "";
        lblInfo1.Text = "";
        lbl_errorl.Text = "";
    }

    protected void EndProcess()
    {
        System.Diagnostics.Process[] p = System.Diagnostics.Process.GetProcesses();
        for (int l = 0; l < p.Length; l++)
        {
            if (p[l].ProcessName.ToLower() == "winword")
            {
                p[l].Kill();
            }
        }
    }

    private bool checkPutnamAmount()
    {
        //string _essn = txtSSN.Text;
        int _mnths = DateTime.Now.Month;
        int _years = DateTime.Now.Year;

        HRA iobj = new HRA();
        //string _quater = iobj.GetQuarter(_mnths);
        //string _qy = _quater + " - " + _years.ToString();
        //string _qy = iobj.getPrevQY(1);
        decimal _amt = HRAdata.getPutnamHRAAmount(txtEmpNo.Text);
        if (_amt == 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    protected void ShowMessageBox(string message)
    {
        ClientScriptManager cs = Page.ClientScript;

        string sJavaScript = "<script language=javascript>\n";
        sJavaScript += "alert('" + message + "');\n";
        sJavaScript += "</script>";
        if (!cs.IsStartupScriptRegistered(this.GetType(), "letterAlert"))
        {
            cs.RegisterStartupScript(this.GetType(), "letterAlert", sJavaScript);
        }
    }

    private void checkBenOrder()
    {
        clearMessages();

        int _empno = Convert.ToInt32(txtEmpNo.Text);
        PilotData pd = new PilotData();
        List<int> mis = new List<int>();
        List<int> _orders = pd.getBeneficiaryOrderNums(_empno);
        int _maxorder = pd.getBeneficiaryOrders(_empno);
        int index = 0;
        string _message = "";
        for (int i = 1; i <= _maxorder; i++)
        {
            if (index == 3)
            {
                break;
            }
            if (_orders.Contains(i))
            {
                continue;
            }
            else
            {
                mis.Add(i);
            }
        }

        foreach(int x in mis)
        {
            switch (x)
            {
                case 1:
                    _message = _message + "Primary Beneficiary not defined <br/>";
                    break;
                case 2:
                    _message = _message + "Secondary Beneficiary not defined <br/>";
                    break;
                case 3:
                    _message = _message + "Tertiary Beneficiary not defined <br/>";
                    break;
            }
        }
        if (!_message.Equals(""))
        {
            infoDiv2.Visible = true;
            lblInfo1.Text = _message;
        }
    }


    //protected void txtBBirthE_TextChanged(object sender, EventArgs e)
    //    //Not using this event - postbackstatus set to false 3/13/2009
    //{
    //    if ((txtBBirthE.Text.Trim().Length == 10) && (IsDate(txtBBirthE.Text)))
    //    {
    //        CheckDependantAge(int.Parse(txtEmpNo.Text.ToString()), txtBSSNE.Text,txtBBirthE.Text);
    //    }
    //}
    public static bool IsDate(Object obj)
    {
        string strDate = obj.ToString();
        try
        {
            DateTime dt = DateTime.Parse(strDate);
            if (dt != DateTime.MinValue && dt != DateTime.MaxValue)
                return true;
            return false;
        }
        catch
        {
            return false;
        }
    }


}
