using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace PD3100sifra43
{
    public class gPunjac
    {
        public gPunjac()
        {
        }
        public void PuniDataSet(SqlDataReader reader, DataTable tabela)
        {
            try
            {
                object[] tmp = new object[tabela.Columns.Count];
                tabela.Rows.Clear();
                if (reader != null && reader.HasRows)
                    while (reader.Read())
                    {
                        reader.GetValues(tmp);
                        tabela.LoadDataRow(tmp, false);
                    }
                if (reader != null) { reader.Close(); }
                tabela.DataSet.AcceptChanges();
            }
            catch (Exception e)
            {
                MessageBox.Show("Greska!" + e.Message);
            }
        }
       
    }
}