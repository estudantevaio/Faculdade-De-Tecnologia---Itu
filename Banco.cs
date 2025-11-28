using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prova
{
    internal class Banco
    {
        private string stringConexao = "Data Source=localhost; Initial Catalog=ControleFinanceiro; User ID=financas; password=senha123; language=Portuguese";

        private SqlConnection cn;
        private void conexao()
        {
            cn = new SqlConnection(stringConexao);
        }

        public SqlConnection abrirConexao()
        {
            try
            {
                conexao();
                cn.Open();      

                return cn;  
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public void fecharConexao()
        {
            try
            {
                cn.Close();
            }
            catch (Exception)
            {

            }
        }

        public DataTable executarConsulta(string sql)
        {
            try
            {
                abrirConexao();

                SqlCommand cmd = new SqlCommand(sql, cn);
                cmd.ExecuteNonQuery();

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();
                adapter.Fill(dt);

                return dt;  
            }
            catch (Exception ex)
            {
                return null;
            }
            finally 
            {
                fecharConexao();
            }
        }
    }
}
