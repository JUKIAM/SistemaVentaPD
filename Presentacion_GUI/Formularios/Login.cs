using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using Logica;
using Entidades;
using MessageBox = System.Windows.Forms.MessageBox;
using Size = System.Drawing.Size;

namespace Presentacion_GUI.Formularios
{
    public partial class Login : Form
    {

        public Login()
        {
            InitializeComponent();
            txtDocumento.Text = "1235340177";
            txtPass.Text = "2312";
            txtPass.Select();
        }

        private void LimpiarCampos()
        {
            txtPass.Text = "";
            txtDocumento.Text = "";
            txtDocumento.Select();
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            Usuario ousuario = new ServicioUsuarios().Listar().Where(u => u.Documento == txtDocumento.Text && u.Clave == txtPass.Text).FirstOrDefault();

            if (ousuario != null)
            {
                Inicio form = new Inicio(ousuario);
                form.Show();
                this.Hide();
                form.FormClosing += frm_closing;
            }
            else
            {
                MessageBox.Show("Credenciales incorrectas", "Mensaje", (MessageBoxButtons)MessageBoxButton.OK, MessageBoxIcon.Exclamation);
                LimpiarCampos();
            }

        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frm_closing(object sender, FormClosingEventArgs e)
        {
            LimpiarCampos();
            this.Show();
        }

        private void txtPass_KeyPress(object sender, KeyPressEventArgs e)
        {
            Usuario ousuario = new ServicioUsuarios().Listar().Where(u => u.Documento == txtDocumento.Text && u.Clave == txtPass.Text).FirstOrDefault();

            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                if (ousuario != null)
                {
                    Inicio form = new Inicio(ousuario);
                    form.Show();
                    this.Hide();
                    form.FormClosing += frm_closing;
                }
                else
                {
                    MessageBox.Show("Credenciales incorrectas", "Mensaje", (MessageBoxButtons)MessageBoxButton.OK, MessageBoxIcon.Exclamation);
                    LimpiarCampos();
                }
            }
        }

        private void txtDocumento_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= 32 && e.KeyChar <= 47) || (e.KeyChar >= 58 && e.KeyChar <= 255))
            {
                e.Handled = true;
            }
        }
    }
}
