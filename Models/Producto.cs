using SQLite;

namespace RegistroProductos.Models;

public class Producto
{ 
    [PrimaryKey, AutoIncrement]
    public int IdProducto { get; set; }

    [MaxLength(100), NotNull]
    public string NombreProducto { get; set; } = string.Empty;

    [MaxLength(500), NotNull]
    public string Descripcion { get; set; } = string.Empty;

    public decimal Precio { get; set; }

    public int Cantidad { get; set; }

    public DateTime FechaRegistro { get; set; }
}
