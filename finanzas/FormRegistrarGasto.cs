using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FinanzasPersonales;

namespace finanzas
{
    public partial class FormRegistrarGasto : Form
    {
        public FormRegistrarGasto()
        {
            InitializeComponent();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                // Obtener datos del formulario
                string categoria = cmbCategoria.SelectedItem.ToString();
                decimal monto = Convert.ToDecimal(txtMonto.Text);
                DateTime fecha = dtpFecha.Value;

                // Crear un nuevo gasto
                Gasto nuevoGasto = new Gasto(categoria, monto, fecha);

                // Enviar el gasto al formulario principal
                FormMain main = (FormMain)Owner;
                main.AgregarGasto(nuevoGasto);

                MessageBox.Show("Gasto registrado correctamente.");
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar el gasto: " + ex.Message);
            }
        }

        private void FormRegistrarGasto_Load(object sender, EventArgs e)
        {

        }
    }
}
