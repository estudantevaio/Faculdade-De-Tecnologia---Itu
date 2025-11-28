using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prova
{
    internal class Lancamento
    {
        public int id;
        public string descricao;
        public decimal valor;
        public DateTime dataLancamento;
        public string tipo;

        public bool gravarLancamento()
        {
            Banco bd = new Banco();
            SqlConnection cn = bd.abrirConexao();
            SqlTransaction tran = cn.BeginTransaction();
            SqlCommand cmd = new SqlCommand();

            cmd.Connection = cn;
            cmd.Transaction = tran;
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = "INSERT into Lancamentos VALUES (@descricao, @valor, @dataLancamento, @tipo);";
            cmd.Parameters.AddWithValue("@descricao", descricao);
            cmd.Parameters.AddWithValue("@valor", valor);
            cmd.Parameters.AddWithValue("@dataLancamento", dataLancamento);
            cmd.Parameters.AddWithValue("@tipo", tipo);

            try
            {
                cmd.ExecuteNonQuery();
                tran.Commit();
                return true;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                return false;
            }
            finally
            {
                bd.fecharConexao();
            }
        }

        public bool excluirLancamento()
        {
            Banco bd = new Banco();
            SqlConnection cn = bd.abrirConexao();
            SqlTransaction tran = cn.BeginTransaction();
            SqlCommand cmd = new SqlCommand();

            cmd.Connection = cn;
            cmd.Transaction = tran;
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = "DELETE FROM Lancamentos WHERE id = @id";

            cmd.Parameters.Add("@id", System.Data.SqlDbType.Int);
            cmd.Parameters[0].Value = id;

            try
            {
                cmd.ExecuteNonQuery();
                tran.Commit();
                return true;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                return false;
            }
            finally
            {
                bd.fecharConexao();
            }
        }

        public bool atualizarLancamento()
        {
            Banco bd = new Banco();
            SqlConnection cn = bd.abrirConexao();
            SqlTransaction tran = cn.BeginTransaction();
            SqlCommand cmd = new SqlCommand();

            cmd.Connection = cn;
            cmd.Transaction = tran;
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = "UPDATE Lancamentos SET descricao = @descricao, valor = @valor, dataLancamento = @dataLancamento, tipo = @tipo WHERE id = @id";
            cmd.Parameters.Add("@descricao", System.Data.SqlDbType.VarChar);
            cmd.Parameters.Add("@valor", System.Data.SqlDbType.Decimal);
            cmd.Parameters.Add("@dataLancamento", System.Data.SqlDbType.Date);
            cmd.Parameters.Add("@tipo", System.Data.SqlDbType.VarChar);
            cmd.Parameters.Add("@id", System.Data.SqlDbType.Int);
            cmd.Parameters[0].Value = descricao;
            cmd.Parameters[1].Value = valor;
            cmd.Parameters[2].Value = dataLancamento;
            cmd.Parameters[3].Value = tipo;
            cmd.Parameters[4].Value = id;

            try
            {
                cmd.ExecuteNonQuery();
                tran.Commit();
                return true;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                return false;
            }
            finally
            {
                bd.fecharConexao();
            }
        }

        public Lancamento consultarLancamento(int id)
        {
            Banco bd = new Banco();

            try
            {
                SqlConnection cn = bd.abrirConexao();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Lancamentos WHERE id = @id", cn);
                cmd.Parameters.AddWithValue("@id", id);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    this.id = reader.GetInt32(0);
                    this.descricao = reader.GetString(1);
                    this.valor = reader.GetDecimal(2);
                    this.dataLancamento = reader.GetDateTime(3);
                    this.tipo = reader.GetString(4);
                    return this;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                bd.fecharConexao();
            }
        }

        public static DataTable listarLancamento(string filtroTipo = "", string filtroDescricao = "")
        {
            Banco bd = new Banco();

            try
            {
                string sql = "SELECT * FROM Lancamentos WHERE 1 = 1";

                if(!string.IsNullOrEmpty(filtroTipo) && filtroTipo != "Todos")
                {
                    sql += " AND tipo = @tipo";
                }

                if(!string.IsNullOrEmpty(filtroDescricao))
                {
                    sql += " AND descricao LIKE @descricao";
                }

                sql += " ORDER by dataLancamento DESC";

                SqlConnection cn = bd.abrirConexao();
                SqlCommand cmd = new SqlCommand(sql, cn);

                if(!string.IsNullOrEmpty(filtroTipo) && filtroTipo != "Todos")
                {
                    cmd.Parameters.AddWithValue("@tipo", filtroTipo);
                }

                if(!string.IsNullOrEmpty(filtroDescricao))
                {
                    cmd.Parameters.AddWithValue("@descricao", "%" + filtroDescricao + "%");
                }

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
                bd.fecharConexao();
            }
        }

        public static decimal calcularSaldo()
        {
            Banco bd = new Banco();

            try
            {
                SqlConnection cn = bd.abrirConexao();
                SqlCommand cmd = new SqlCommand("SELECT SUM(case WHEN tipo = 'Entrada' THEN valor ELSE -valor END) FROM Lancamentos", cn);

                object resultado = cmd.ExecuteScalar();
                if (resultado != DBNull.Value)
                {
                    return Convert.ToDecimal(resultado);
                }
                return 0;
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                bd.fecharConexao();
            }
        }
    }
}
