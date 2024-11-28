using System;

namespace FinanzasPersonales
{

    [Serializable]
    public class Gasto
    {
        public string Categoria { get; set; } // Categoría del gasto, como "Alimentos", "Transporte", etc.
        public decimal Monto { get; set; }   // Monto del gasto
        public DateTime Fecha { get; set; } // Fecha en que se realizó el gasto

        // Constructor de la clase
        public Gasto(string categoria, decimal monto, DateTime fecha)
        {
            Categoria = categoria;
            Monto = monto;
            Fecha = fecha;
        }
    }
}