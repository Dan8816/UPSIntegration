﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        dbquery(System.DateTime.Now.AddDays(-45), System.DateTime.Now);
    }
    public void dbquery(DateTime fromDate, DateTime toDate)
    {
        string connectionSTR = "Server=AAG-Omni;Database=OAI_ShippingRequest;Integrated Security=SSPI";
        //
        //using (ShippingRequestDataContext db = new ShippingRequestDataContext(new SqlConnection(connectionSTR)))
        //{
        //}
        //
        
        SqlConnection allrequests = new SqlConnection(connectionSTR);
        ShippingRequestDataContext db = new ShippingRequestDataContext(allrequests);

        var query = from x in db.Processed_Requests
                    where x.Ship_Type == "P"
                    where x.Ship_InitDate.Value > fromDate
                    where x.Ship_InitDate.Value <= toDate
                    select new
                    {
                        PK = x.ID,
                        id = x.Original_ShipID,
                        Sender = x.From_Name,
                        //Purpose = x.Ship_Type,//Presumably these are all personal requests for payroll purposes, no reason to display it
                        Cost = x.Total_Cost,
                        Status = x.Payroll_Status,//Presumable these are all 'Waiting' to be processed, not reason to display it
                        InitDate = x.Ship_InitDate.Value
                    };

        Literal1.Text = "<table id='main-table' class='display table table-hover text-center'>";
            Literal1.Text += "<thead>";
                Literal1.Text += "<tr>";
                    Literal1.Text += "<th>Request #: </th>";
                    Literal1.Text += "<th>Sender: </th>";
                    Literal1.Text += "<th>Request Date</th>";
                    Literal1.Text += "<th>Cost: </th>";
                    Literal1.Text += "<th>Status: </th>";
                Literal1.Text += "</tr>";
            Literal1.Text += "</thead>";
            Literal1.Text += "<tbody>";

        foreach (var rec in query)
        {
            Literal1.Text += "<tr>";
                Literal1.Text += "<td><a href='UpdateInfo.aspx?field1=" + rec.id + "'>" + rec.id + "</a></td>";
                Literal1.Text += "<td>" + rec.Sender + "</td>";
                Literal1.Text += "<td>" + rec.InitDate.ToShortDateString() + "</td>";
                Literal1.Text += "<td>" + rec.Cost + "</td>";
            Literal1.Text += "<td>" + rec.Status + "</td>";
            Literal1.Text += "</tr>";
        }
            Literal1.Text += "</tbody>";
        Literal1.Text += "</table>";

        foreach (var q in query)
        {
            System.Diagnostics.Debug.WriteLine(">>>>> " + q.Sender + " <<<<<");
        }
        //GridView1.DataSource = querry;
        //GridView1.DataBind();

        db.Dispose();
        
    }
}