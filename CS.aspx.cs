using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Services;


public partial class CS : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            DataTable dummy = new DataTable();
            dummy.Columns.Add("CustomerID");
            dummy.Columns.Add("ContactName");
            dummy.Columns.Add("City");
            dummy.Columns.Add("Country");
            dummy.Rows.Add();
            gvCustomers.DataSource = dummy;
            gvCustomers.DataBind();

            //Required for jQuery DataTables to work.
            gvCustomers.UseAccessibleHeader = true;
            gvCustomers.HeaderRow.TableSection = TableRowSection.TableHeader;
        }
    }

    [WebMethod]
    public static List<Customer> GetCustomers()
    {
        List<Customer> customers = new List<Customer>();
        string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand("SELECT CustomerID, ContactName, City, Country FROM Customers", con))
            {
                con.Open();
                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        customers.Add(new Customer
                        {
                            CustomerID = sdr["CustomerID"].ToString(),
                            ContactName = sdr["ContactName"].ToString(),
                            City = sdr["City"].ToString(),
                            Country = sdr["Country"].ToString()
                        });
                    }
                }
                con.Close();
            }
        }

        return customers;
    }

    public class Customer
    {
        public string CustomerID { get; set; }
        public string ContactName { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }
}