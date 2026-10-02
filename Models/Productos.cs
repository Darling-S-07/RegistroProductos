
using System;
using System.Collections.Generic;
using System.Text;

namespace RegistroProductos.Models;

using SQLite;

public class Productos
{
    [PrimaryKey, AutoIncrement]
    public int IdProducto { get; set; }   
    [MaxLength(100)]   
    public string NombreProducto { get; set; } = string.Empty;
    [MaxLength(500)]
    public string Descripcion { get; set; } = string.Empty;
    [MaxLength(12)]
    public decimal Precio { get; set; }
    [MaxLength(9)]
    public int Cantidad { get; set; }
    public DateTime FechaRegistro { get; set; }
}