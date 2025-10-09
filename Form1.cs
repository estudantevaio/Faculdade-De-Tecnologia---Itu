using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sistema_Para_Registro_Financeiro
{
    public partial class Form1 : Form
    {
        private double saldoAtual;
        public Form1()
        {
            InitializeComponent();
            dtpData.Value = DateTime.Now;

            StreamWriter streamWriter = new StreamWriter("Controle Financeiro.txt");
            streamWriter.WriteLine("INÍCIO DA SESSÃO - " + DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
            streamWriter.Close();
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            try
            {
                if (txbDescricao.Text == "")
                {
                    MessageBox.Show("Digite uma descrição, por favor. ");
                    return; 
                }

                double credito = 0;
                double debito = 0;

                if (txbCredito.Text != "")
                    credito = double.Parse(txbCredito.Text);

                if (txbDebito.Text != "")
                    debito = double.Parse(txbDebito.Text);

                saldoAtual = saldoAtual + credito - debito;

                StreamWriter writer = new StreamWriter("Controle Financeiro.txt", true);
                writer.WriteLine(txbDescricao.Text + "; " + dtpData.Value.ToString("dd/MM/yyyy") + "; Crédito: R$ " + credito.ToString("N2") + "; Débito: R$ " + debito.ToString("N2") + "; Saldo Atual: R$ " + saldoAtual.ToString("N2") + ". ");
                
                writer.Close();

                MessageBox.Show("Registro salvo. Saldo atual: R$ " + saldoAtual.ToString("N2"));

                txbDescricao.Clear();
                txbCredito.Clear();
                txbDebito.Clear();
                txbDescricao.Focus();
            }
            
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao gravar. " + ex.Message);
            }
        }
    }
}
