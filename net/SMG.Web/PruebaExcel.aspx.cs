using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.IO;
using System.Data.Common;


public partial class PruebaExcel : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        String ruta = Server.MapPath("Excel") + "\\CartasEnviadas.xlsx";
        string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + ruta + ";Extended Properties=\"Excel 12.0;HDR=YES;\"";
        


        DbProviderFactory factory = DbProviderFactories.GetFactory("System.Data.OleDb");

        using (DbConnection connection = factory.CreateConnection())
        {
            connection.ConnectionString = connectionString;

            using (DbCommand command = connection.CreateCommand())
            {
                // Cities$ comes from the name of the worksheet
                command.CommandText = "SELECT * from [Hoja1$]";
                try
                {
                    connection.Open();

                    using (DbDataReader dr = command.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            //Debug.WriteLine(dr["ID"].ToString());
                            
                            Response.Write("Contrato:" + dr[0].ToString() + " Visita:" + dr[1].ToString() + " Fecha:" + dr[2].ToString() + "<br />");
                        }
                    }
                }
                catch (Exception ex)
                {
                    string a = "";
                }
                finally
                {
                    connection.Close();
                }

                
            }
        }
    }


    protected void Button1_Click(object sender, EventArgs e)
    {
        if (File.Exists(Server.MapPath("~/Excel") + "\\" + FileUpload1.FileName))
        {
        }
        else
        {
            FileUpload1.SaveAs(Server.MapPath("~/Excel") + "\\" + FileUpload1.FileName);
        }
        
    }

}
