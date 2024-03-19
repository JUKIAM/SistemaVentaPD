using Entidades;
using Logica;
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

namespace Presentacion_GUI.Formularios
{
    public partial class NegocioPD : Form
    {
        public NegocioPD()
        {
            InitializeComponent();
        }

        //Convertir la imagen de byte a imagen
        public Image ByteToImage(byte[] imageBytes)
        {
            MemoryStream ms = new MemoryStream();
            ms.Write(imageBytes, 0, imageBytes.Length);
            Image image = new Bitmap(ms);
            return image;
        }

        private void Empresa_Load(object sender, EventArgs e)
        {
            bool obtenido = true;
            byte[] byteimage = new CL_Negocio().ObtenerLogo(out obtenido);

            if (obtenido)
            {
                picLogo.Image = ByteToImage(byteimage);
            }

            Negocio datos = new CL_Negocio().ObtenerDatos();
            txtNombre.Text = datos.Nombre;
            txtRuc.Text = datos.RUC;
            txtDireccion.Text = datos.Direccion;

        }

        private void btnSubir_Click(object sender, EventArgs e)
        {
            String mensaje = String.Empty;

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.FileName = "LogoEmpresa.png";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                byte[] byteimage = File.ReadAllBytes(openFileDialog.FileName);
                bool respuesta = new CL_Negocio().ActualizarLogo(byteimage, out mensaje);

                if (respuesta)
                {
                    picLogo.Image = ByteToImage(byteimage);
                }
                else
                {
                    MessageBox.Show(mensaje, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        private void btnGuardarCambios_Click(object sender, EventArgs e)
        {
            String mensaje = String.Empty;

            Negocio obj = new Negocio()
            {
                Nombre = txtNombre.Text,
                RUC = txtRuc.Text,
                Direccion = txtDireccion.Text
            };

            bool respuesta = new CL_Negocio().GuardarDatos(obj, out mensaje);

            if (respuesta)
            {
                MessageBox.Show("Los cambios fueron guardados", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("No se pudieron guardar los cambios", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}
