using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace quanlykhachsan.DatabaseConect
{
    internal class ConnectDb
    {
        private MySqlConnection connection;

        public static MySqlConnection GetConnection()
        {
            string sql = "datasource=localhost;port=3306;username=root;password=123;database=hotelmanage";

            MySqlConnection con = new MySqlConnection(sql);
            try
            {
                con.Open();
            }catch(MySqlException ex)
            {
                MessageBox.Show("MySql connection! \n " + ex.Message,"error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            return con;
        }
        public static void CloseConnection(MySqlConnection con){
            if (con != null && con.State == System.Data.ConnectionState.Open)
            {
                try
                {
                    con.Close();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Error closing connection: \n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }

}
