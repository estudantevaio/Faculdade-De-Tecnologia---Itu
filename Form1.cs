using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Prova
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CarregarLancamento();
            cmbTipo.Items.AddRange(new string[] { "Entrada", "Saída" });
            cmbFiltroTipo.Items.AddRange(new string[] { "Todos", "Entrada", "Saída" });
            cmbFiltroTipo.SelectedIndex = 0;
            AtualizarSaldo();
        }

        private void CarregarLancamento()
        {
            string filtroTipo = cmbFiltroTipo.SelectedItem?.ToString() ?? "";
            string filtroDescricao = txbFiltroDescricao.Text;

            DataTable dt = Lancamento.listarLancamento(filtroTipo, filtroDescricao);
            dataGridView1.DataSource = dt;
        }

        private void AtualizarSaldo()
        {
            decimal saldo = Lancamento.calcularSaldo();
            lblSaldo.Text = $"Saldo Atual: R$ {saldo.ToString("N2")}";

            if(saldo >= 0)
            {
                lblSaldo.ForeColor = System.Drawing.Color.Green;
            }
            else 
            {
                lblSaldo.ForeColor = System.Drawing.Color.Red;
            }
         }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txbDescricao.Text) || string.IsNullOrEmpty(txbValor.Text) || cmbTipo.SelectedItem == null)
            {
                MessageBox.Show("Preencha todos os campos, por favor.");
                return;
            }

            Lancamento lancamento = new Lancamento();
            lancamento.descricao = txbDescricao.Text;
            lancamento.valor = decimal.Parse(txbValor.Text);
            lancamento.dataLancamento = dateTimePicker1.Value;
            lancamento.tipo = cmbTipo.SelectedItem.ToString();

            if(lancamento.gravarLancamento())
            {
                MessageBox.Show("Lançamento salvo com sucesso.");
                CarregarLancamento();
                AtualizarSaldo();
                LimparCampos();
            }
            else 
            {
                MessageBox.Show("Erro ao salvar o lanlamento.");
            }
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            if(dataGridView1.SelectedRows.Count > 0)
            {
                int id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["Id"].Value);

                DialogResult resultado = MessageBox.Show("Tem certeza que deseja excluir este lançamento?", "Confirmação", MessageBoxButtons.YesNo);

                if(resultado == DialogResult.Yes)
                 {
                     Lancamento lancamento = new Lancamento();
                     lancamento.id = id;

                    if(lancamento.excluirLancamento())
                    {
                        MessageBox.Show("Lançamento excluído com sucesso.");
                        CarregarLancamento();
                        AtualizarSaldo();
                    }
                    else
                    {
                        MessageBox.Show("Erro ao excluir o lançamento.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Selecione um lançamento para exclur.");
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if(dataGridView1.SelectedRows.Count > 0)
            {
                int id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["Id"].Value);

                Lancamento lancamento = new Lancamento();
                lancamento = lancamento.consultarLancamento(id);

                if (lancamento != null)
                {
                    txbDescricao.Text = lancamento.descricao;
                    txbValor.Text = lancamento.valor.ToString();
                    dateTimePicker1.Value = lancamento.dataLancamento;
                    cmbTipo.SelectedItem = lancamento.tipo;

                    btnSalvar.Tag = lancamento.id;
                    btnSalvar.Text = "Atualizar";
                }
            }
            else
            {
                MessageBox.Show("Selecioe um lançamento para editar, por favor.");
            }
        }

        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            CarregarLancamento();
            AtualizarSaldo();
        }

        private void LimparCampos()
        {
            txbDescricao.Clear();
            txbValor.Clear();
            dateTimePicker1.Value = DateTime.Now;
            cmbTipo.SelectedIndex = -1;
            btnSalvar.Tag = null;
            btnSalvar.Text = "Salvar";
        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            LimparCampos();
        }
    }
}
