using Entidades;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using Logica;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentacion_GUI.Formularios
{
    public partial class DetalleVenta : Form
    {
        public DetalleVenta()
        {
            InitializeComponent();
        }

        private void DetalleVenta_Load(object sender, EventArgs e)
        {
            txtBusqueda.Select();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            Venta oVenta = new CL_Venta().ObtenerVenta(txtBusqueda.Text);

            if (oVenta.IdVenta != 0)
            {
                txtNumeroDocumento.Text = oVenta.NumeroDocumento;
                txtFecha.Text = oVenta.FechaRegistro;
                txtTipoDocumento.Text = oVenta.TipoDocumento;
                txtUsuario.Text = oVenta.oUsuario.NombreCompleto;
                txtDocCliente.Text = oVenta.DocumentoCLiente;
                txtNombreCliente.Text = oVenta.NombreCliente;
                dgvData.Rows.Clear();

                foreach (Detalle_Venta dv in oVenta.oDetalle_Venta)
                {
                    dgvData.Rows.Add(new object[]
                    {
                        dv.oProducto.Nombre,
                        dv.PrecioVenta,
                        dv.Cantidad,
                        dv.SubTotal
                    });
                }

                txtMontoTotal.Text = oVenta.MontoTotal.ToString("C", CultureInfo.CurrentCulture);
                txtMontoPago.Text = oVenta.MontoPago.ToString("C", CultureInfo.CurrentCulture);
                txtMontoCambio.Text = oVenta.MontoCambio.ToString("C", CultureInfo.CurrentCulture);

            }
        }

        private void btnLimpiarBuscador_Click(object sender, EventArgs e)
        {
            txtFecha.Text = "";
            txtTipoDocumento.Text = "";
            txtUsuario.Text = "";
            txtDocCliente.Text = "";
            txtNombreCliente.Text = "";
            dgvData.Rows.Clear();
            txtMontoTotal.Text = "0.00";
            txtMontoPago.Text = "0.00";
            txtMontoCambio.Text = "0.00";
        }

        private void txtBusqueda_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= 32 && e.KeyChar <= 47) || (e.KeyChar >= 58 && e.KeyChar <= 255))
            {
                e.Handled = true;
            }

            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                Venta oVenta = new CL_Venta().ObtenerVenta(txtBusqueda.Text);

                if (oVenta.IdVenta != 0)
                {
                    txtNumeroDocumento.Text = oVenta.NumeroDocumento;
                    txtFecha.Text = oVenta.FechaRegistro;
                    txtTipoDocumento.Text = oVenta.TipoDocumento;
                    txtUsuario.Text = oVenta.oUsuario.NombreCompleto;
                    txtDocCliente.Text = oVenta.DocumentoCLiente;
                    txtNombreCliente.Text = oVenta.NombreCliente;
                    dgvData.Rows.Clear();

                    foreach (Detalle_Venta dv in oVenta.oDetalle_Venta)
                    {
                        dgvData.Rows.Add(new object[]
                        {
                        dv.oProducto.Nombre,
                        dv.PrecioVenta,
                        dv.Cantidad,
                        dv.SubTotal
                        });
                    }

                    txtMontoTotal.Text = oVenta.MontoTotal.ToString("C", CultureInfo.CurrentCulture);
                    txtMontoPago.Text = oVenta.MontoPago.ToString("C", CultureInfo.CurrentCulture);
                    txtMontoCambio.Text = oVenta.MontoCambio.ToString("C", CultureInfo.CurrentCulture);

                }
            }
        }

        private void btnDescargar_Click(object sender, EventArgs e)
        {
            if (txtTipoDocumento.Text == "")
            {
                MessageBox.Show("No se encontraron resultados", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            String Texto_Html = Properties.Resources.PlantillaVenta.ToString();
            Negocio oDatos = new CL_Negocio().ObtenerDatos();

            Texto_Html = Texto_Html.Replace("@nombrenegocio", oDatos.Nombre.ToUpper());
            Texto_Html = Texto_Html.Replace("@docnegocio", oDatos.RUC);
            Texto_Html = Texto_Html.Replace("@direcnegocio", oDatos.Direccion);

            Texto_Html = Texto_Html.Replace("@tipodocumento", txtTipoDocumento.Text.ToUpper());
            Texto_Html = Texto_Html.Replace("@numerodocumento", txtNumeroDocumento.Text);

            Texto_Html = Texto_Html.Replace("@doccliente", txtDocCliente.Text);
            Texto_Html = Texto_Html.Replace("@nombrecliente", txtNombreCliente.Text);
            Texto_Html = Texto_Html.Replace("@fecharegistro", txtFecha.Text);
            Texto_Html = Texto_Html.Replace("@usuariovendedor", txtUsuario.Text);

            String filas = String.Empty;
            foreach (DataGridViewRow row in dgvData.Rows)
            {
                filas += "<tr>";
                filas += "<td>" + row.Cells["Producto"].Value.ToString() + "</td>";
                filas += "<td>" + row.Cells["Precio"].Value.ToString() + "</td>";
                filas += "<td>" + row.Cells["Cantidad"].Value.ToString() + "</td>";
                filas += "<td>" + row.Cells["SubTotal"].Value.ToString() + "</td>";
                filas += "</tr>";
            }
            Texto_Html = Texto_Html.Replace("@filas", filas);
            Texto_Html = Texto_Html.Replace("@montototal", txtMontoTotal.Text);
            Texto_Html = Texto_Html.Replace("@pagocon", txtMontoPago.Text);
            Texto_Html = Texto_Html.Replace("@cambio", txtMontoCambio.Text);

            SaveFileDialog savefile = new SaveFileDialog();
            savefile.FileName = String.Format("Venta_{0}.pdf", txtNumeroDocumento.Text);
            savefile.Filter = "Pdf Files|*.pdf";

            if (savefile.ShowDialog() == DialogResult.OK)
            {
                using (FileStream stream = new FileStream(savefile.FileName, FileMode.Create))
                {
                    Document pdfDoc = new Document(PageSize.A4, 25, 25, 25, 25);
                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();

                    bool obtenido = true;
                    byte[] byteImage = new CL_Negocio().ObtenerLogo(out obtenido);

                    if (obtenido)
                    {
                        iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(byteImage);
                        img.ScaleToFit(110, 110);
                        img.Alignment = iTextSharp.text.Image.UNDERLYING;
                        img.SetAbsolutePosition(pdfDoc.Left, pdfDoc.GetTop(71));
                        pdfDoc.Add(img);
                    }

                    using (StringReader sr = new StringReader(Texto_Html))
                    {
                        XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                    }

                    pdfDoc.Close();
                    stream.Close();
                    MessageBox.Show("Documento Generado", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }
}
