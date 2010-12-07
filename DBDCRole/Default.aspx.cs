using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DatabaseDotCom;

namespace DBDCRole
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // BEGIN Login Example
            DatabaseDotComClient client = new DatabaseDotComClient(DatabaseDotComContext.FromConfigurationSetting("DbDotComConnectionString"));
            // END Login Example

       //     DatabaseDotComClient client = new DatabaseDotComClient();
       //     client.context = client.Login("", "", "");
            Session.Add("context", client.context);

            if (IsPostBack == false) { Query_Test(); }
        }

        protected void EditPhone(object sender, GridViewEditEventArgs e)
        {
            outputbox.Text += "Starting update sample..." + Environment.NewLine;
            
   
            // BEGIN Update Example
            DatabaseDotComContext context = (DatabaseDotComContext)Session["context"];
            DatabaseDotComClient client = new DatabaseDotComClient(context);

            outputbox.Text += "Starting update sample..." + Environment.NewLine;

            DatabaseDotComClient.DBCObject contact = new DatabaseDotComClient.DBCObject("Contact");

            HiddenField idField = (HiddenField)GridView1.Rows[e.NewEditIndex].FindControl("IdField");
            TextBox phoneText = (TextBox)GridView1.Rows[e.NewEditIndex].FindControl("newPhone");
            contact.Id = idField.Value;
            contact.SetStringField("Phone", phoneText.Text);
            outputbox.Text += "New Phone is:"+contact.GetStringField("Phone")+Environment.NewLine;
            DatabaseDotCom.DBDCReference.SaveResult sr;
            contact = client.Update(contact, out sr);
            outputbox.Text += "Update success result is: " + sr.success + Environment.NewLine + Environment.NewLine;
            System.Diagnostics.Debug.WriteLine("Row Updated");


            Query_Test();
        }

        protected void EditRow(object sender, GridViewRowEventArgs e)
        {
            
        }

        protected void Query_Test()
        {
            DatabaseDotComContext context = (DatabaseDotComContext)Session["context"];
            DatabaseDotComClient client = new DatabaseDotComClient(context);

            // BEGIN Query Example
            DatabaseDotCom.DatabaseDotComClient.QueryResult qr = client.Query("Select Id, Name, FirstName, LastName, Phone, Account.Id, Account.Name From Contact");
            System.Diagnostics.Debug.WriteLine("Records: " + qr.size);
            foreach (DatabaseDotComClient.DBCObject obj in qr.records)
            {
                System.Diagnostics.Debug.WriteLine("Id: " + obj.GetStringField("Id"));
                System.Diagnostics.Debug.WriteLine("Name: " + obj.GetStringField("Name"));
                if (obj.getFields().ContainsKey("Contacts"))
                {
                    try
                    {
                        DatabaseDotComClient.QueryResult cons = (DatabaseDotComClient.QueryResult)obj.getFields()["Contacts"];
                        foreach (DatabaseDotComClient.DBCObject con in cons.records)
                        {
                            System.Diagnostics.Debug.WriteLine("Contact name: " + con.GetStringField("Name"));
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
                else if (obj.getFields().ContainsKey("Account"))
                {
                    try
                    {
                        DatabaseDotComClient.DBCObject account = (DatabaseDotComClient.DBCObject)obj.getFields()["Account"];
                        System.Diagnostics.Debug.WriteLine("Account  name: " + account.GetStringField("Name"));
                        System.Diagnostics.Debug.WriteLine("Account  industry: " + account.GetStringField("Industry"));
                        System.Diagnostics.Debug.WriteLine("Account  id: " + account.GetStringField("Id"));
                    }
                    catch (Exception ex)
                    {
                    }
                }

                GridView1.DataSource = qr.GetDataSet();
                GridView1.DataBind();
                // END Query Example

            }
        }

        protected void Query_Click(object sender, EventArgs e)
        {
            Query_Test();
        }

        protected void CreateSingle_Click(object sender, EventArgs e)
        {
            DatabaseDotComContext context = (DatabaseDotComContext)Session["context"];
            DatabaseDotComClient client = new DatabaseDotComClient(context);
            // BEGIN Create Example
            outputbox.Text += "Starting Create Sample..." + Environment.NewLine;
            DatabaseDotComClient.DBCObject newContact = new DatabaseDotComClient.DBCObject("Contact");
            newContact.SetStringField("FirstName", "Joe");
            newContact.SetStringField("LastName", "Test");
            DatabaseDotCom.DBDCReference.SaveResult sr;
            newContact = client.Create(newContact, out sr);
            outputbox.Text += "Create was successful, new id is: " + newContact.Id + Environment.NewLine + Environment.NewLine;


            Query_Test();
        }

        protected void CreateMultiple_Click(object sender, EventArgs e)
        {
            DatabaseDotComContext context = (DatabaseDotComContext)Session["context"];
            DatabaseDotComClient client = new DatabaseDotComClient(context);
            outputbox.Text += "Starting Create with batch..." + Environment.NewLine;
            DatabaseDotComClient.DBCObject[] contacts = new DatabaseDotComClient.DBCObject[1];
            DatabaseDotComClient.DBCObject newContact1 = new DatabaseDotComClient.DBCObject("Contact");
            newContact1.SetStringField("FirstName", "Joe");
            newContact1.SetStringField("LastName", "Test1");

            DatabaseDotComClient.DBCObject newContact2 = new DatabaseDotComClient.DBCObject("Contact");
            newContact2.SetStringField("FirstName", "Joe");
            newContact2.SetStringField("LastName", "Test2");
            
            DatabaseDotCom.DBDCReference.SaveResult[] srs;
            contacts = client.Create(new DatabaseDotComClient.DBCObject[] { newContact1, newContact2 }, out srs);
            outputbox.Text += "Create was successful, new id is: " + contacts[0].Id + Environment.NewLine + Environment.NewLine;
            // END Create Example

            Query_Test();
        }
/*
        protected void Update_Click(object sender, EventArgs e)
        {
            // BEGIN Update Example
            DatabaseDotComContext context = (DatabaseDotComContext)Session["context"];
            DatabaseDotComClient client = new DatabaseDotComClient(context);
            outputbox.Text += "Starting update sample..." + Environment.NewLine;
            DatabaseDotComClient.QueryResult accountQuery = client.Query("Select Id From Account Where Name = 'Azure Test Account'");
            DatabaseDotComClient.DBCObject newAccount = accountQuery.records[0];
            newAccount.SetStringField("Industry", "Agriculture");
            DatabaseDotCom.DBDCReference.SaveResult sr;
            newAccount = client.Update(newAccount, out sr);
            outputbox.Text += "Update success result is: " + sr.success + Environment.NewLine + Environment.NewLine;
            //END Update Example
        }
*/
        protected void DeleteByIds_Click(object sender, EventArgs e)
        {
            // BEGIN Delete Example
            DatabaseDotComContext context = (DatabaseDotComContext)Session["context"];
            DatabaseDotComClient client = new DatabaseDotComClient(context);
            outputbox.Text += "Starting delete using ids sample..." + Environment.NewLine;
            DatabaseDotComClient.QueryResult contactQuery = client.Query("Select Id From Contact Where  FirstName = 'Joe'");

            string[] ids = new string[contactQuery.records.Length];
            for (int i = 0; i < ids.Length; i++)
            {
                ids[i] = contactQuery.records[i].Id;
            }
            DatabaseDotCom.DBDCReference.DeleteResult[] results = client.DeleteByIds(ids);
            outputbox.Text += "Delete success result is: " + results[0].success + Environment.NewLine + Environment.NewLine;


            Query_Test();
        }

        protected void DeleteByQuery_Click(object sender, EventArgs e)
        {
            outputbox.Text += "Starting delete user object array sample ..." + Environment.NewLine;
            DatabaseDotComContext context = (DatabaseDotComContext)Session["context"];
            DatabaseDotComClient client = new DatabaseDotComClient(context);
           
            DatabaseDotComClient.QueryResult contactQuery = client.Query("Select Id From Contact Where FirstName = 'Joe'");
            DatabaseDotCom.DBDCReference.DeleteResult[] results = client.DeleteObjects(contactQuery.records);
            outputbox.Text += "Delete success result is: " + results[0].success + Environment.NewLine + Environment.NewLine;


            Query_Test();
        }

        protected void DeleteByQueryResult_Click(object sender, EventArgs e)
        {
            outputbox.Text += "Starting delete using query result sample..." + Environment.NewLine;
            DatabaseDotComContext context = (DatabaseDotComContext)Session["context"];
            DatabaseDotComClient client = new DatabaseDotComClient(context);

            DatabaseDotComClient.QueryResult contactQuery = client.Query("Select Id From Contact Where  FirstName = 'Joe'");
            DatabaseDotCom.DBDCReference.DeleteResult[] results = client.DeleteByQueryResult(contactQuery);
            outputbox.Text += "Delete success result is: " + results[0].success + Environment.NewLine + Environment.NewLine;


            Query_Test();
        }
    }
}
