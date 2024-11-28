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

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using finanzas;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace FinanzasPersonales
{
    public partial class FormMain : Form
    {
        // Lista para almacenar los gastos
        private List<Gasto> gastos = new List<Gasto>();

        public FormMain()
        {
            InitializeComponent();
        }

        private void GuardarGastosEnArchivo()
        {
            try
            {
                // Define la ruta donde se guardará el archivo
                string rutaArchivo = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "gastos.bin");

                // Crea un flujo para escribir el archivo
                using (FileStream stream = new FileStream("gastos.bin", FileMode.Create))
                {
                    // Serializador binario
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(stream, gastos); // Serializa la lista de gastos
                }
                MessageBox.Show("Los gastos se han guardado correctamente.", "Éxito");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar los gastos: " + ex.Message, "Error");
            }
        }
        private void CargarGastosDesdeArchivo()
        {
            try
            {
                // Define la ruta desde donde se cargará el archivo
                string rutaArchivo = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "gastos.bin");

                if (File.Exists("gastos.bin"))
                {
                    using (FileStream stream = new FileStream("gastos.bin", FileMode.Open))
                    {
                        BinaryFormatter formatter = new BinaryFormatter();
                        gastos = (List<Gasto>)formatter.Deserialize(stream);
                    }
                    ActualizarResumen();
                }
            }
            catch
            {
                MessageBox.Show("El archivo de datos está corrupto o no es válido. Se iniciará una lista vacía.", "Advertencia");
                gastos = new List<Gasto>(); // Reinicia la lista si hay problemas
            }
        }

        // Método que se ejecuta al cargar el formulario principal
        private void FormMain_Load(object sender, EventArgs e)
        {

           CargarGastosDesdeArchivo();

            // Inicializa el resumen de gastos al abrir el formulario
            ActualizarResumen();
        }

        // Método para agregar un gasto a la lista
        public void AgregarGasto(Gasto gasto)
        {
            gastos.Add(gasto); // Agrega el gasto a la lista
            ActualizarResumen(); // Actualiza el resumen total de gastos
        }

        // Método para actualizar el resumen de gastos totales
        private void ActualizarResumen()
        {
            decimal total = 0; // Inicializa el total en 0

            // Suma todos los montos de los gastos
            foreach (var gasto in gastos)
            {
                total += gasto.Monto;
            }

            // Actualiza el texto del Label con el total formateado
            lblGastosTotales.Text = $"Gastos Totales: {total:C}";
        }

        private void FormMain_Load_1(object sender, EventArgs e)
        {

        }

        private void FormMain_Load_2(object sender, EventArgs e)
        {

        }

        private void btnRegistrarGasto_Click(object sender, EventArgs e)
        {
            FormRegistrarGasto formRegistrar = new FormRegistrarGasto();
            formRegistrar.Owner = this; // Establece el formulario principal como dueño
            formRegistrar.ShowDialog(); // Abre el formulario de registro como modal
        }
        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            GuardarGastosEnArchivo(); // Llama al método para guardar los gastos
            var resultado = MessageBox.Show("¿Deseas guardar los cambios antes de salir?", "Confirmar cierre", MessageBoxButtons.YesNoCancel);

            if (resultado == DialogResult.Yes)
            {
                GuardarGastosEnArchivo(); // Llama al método para guardar los datos
            }
            else if (resultado == DialogResult.Cancel)
            {
                e.Cancel = true; // Cancela el cierre si el usuario selecciona "Cancelar"
            }
        }

        private void btnVerGastos_Click(object sender, EventArgs e)
        {
            string mensaje = "Lista de Gastos:\n\n";

            foreach (var gasto in gastos)
            {
                mensaje += $"- {gasto.Categoria}: {gasto.Monto:C} el {gasto.Fecha.ToShortDateString()}\n";
            }

            MessageBox.Show(mensaje, "Gastos Registrados");
        }

        private void exportarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // Crear un cuadro de diálogo para guardar archivos
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "Archivo de Texto (*.txt)|*.txt";
                    saveFileDialog.Title = "Guardar Gastos";
                    saveFileDialog.FileName = "Gastos.txt";

                    // Mostrar el cuadro de diálogo y verificar si el usuario seleccionó un archivo
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        // Construir el contenido del archivo
                        StringBuilder contenido = new StringBuilder();
                        contenido.AppendLine("Lista de Gastos:");
                        contenido.AppendLine("----------------");

                        foreach (var gasto in gastos)
                        {
                            contenido.AppendLine(
                                $"{gasto.Fecha.ToShortDateString()} - {gasto.Categoria}: {gasto.Monto:C}"
                            );
                        }

                        // Escribir el contenido en el archivo seleccionado
                        System.IO.File.WriteAllText(saveFileDialog.FileName, contenido.ToString());

                        MessageBox.Show("Gastos exportados correctamente.", "Éxito");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error al exportar los gastos: " + ex.Message, "Error");
            }
        }

        private void resumenPorCategoríaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (gastos.Count == 0)
            {
                MessageBox.Show("No hay gastos registrados.", "Información");
                return;
            }

            // Calcula los totales por categoría
            var gastosPorCategoria = new Dictionary<string, decimal>();
            foreach (var gasto in gastos)
            {
                if (!gastosPorCategoria.ContainsKey(gasto.Categoria))
                {
                    gastosPorCategoria[gasto.Categoria] = 0;
                }
                gastosPorCategoria[gasto.Categoria] += gasto.Monto;
            }

            // Construye el mensaje para mostrar los resultados
            StringBuilder resumen = new StringBuilder();
            resumen.AppendLine("Gastos por Categoría:");
            resumen.AppendLine("----------------------");

            foreach (var categoria in gastosPorCategoria)
            {
                resumen.AppendLine($"{categoria.Key}: {categoria.Value:C}");
            }

            // Muestra el resumen en un cuadro de diálogo
            MessageBox.Show(resumen.ToString(), "Resumen de Gastos por Categoría");
        }

        private void FormMain_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            GuardarGastosEnArchivo(); // Llama al método para guardar los gastos
            var resultado = MessageBox.Show("¿Deseas guardar los cambios antes de salir?", "Confirmar cierre", MessageBoxButtons.YesNoCancel);

            if (resultado == DialogResult.Yes)
            {
                GuardarGastosEnArchivo(); // Llama al método para guardar los datos
            }
            else if (resultado == DialogResult.Cancel)
            {
                e.Cancel = true; // Cancela el cierre si el usuario selecciona "Cancelar"
            }
        }
    }
}

