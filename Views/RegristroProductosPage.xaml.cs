namespace RegistroProductos.Views;

using RegistroProductos.Data;
using RegistroProductos.Models;

public partial class RegristroProductosPage : ContentPage
{
    private readonly SQLiteDatabase _database;
    public RegristroProductosPage()
	{
		InitializeComponent();
		string dbPath = Path.Combine(FileSystem.AppDataDirectory, "productos.db3");
		_database = new SQLiteDatabase(dbPath);
	}

    private async void GuardarButtom_Clicked(object sender, EventArgs e)
    {
        try
        {
            Productos productos = new Productos 
            { 
                NombreProducto = NombreProductoEntry.Text,
                Descripcion = DescripcionEntry.Text,
                Precio = decimal.Parse(PrecioEntry.Text),
                Cantidad = int.Parse(CantidadEntry.Text),
                FechaRegistro = FechaRegistroPicker.Date ?? DateTime.Now

            };    
            
            await _database.AddProductosAsync(productos);

            await DisplayAlertAsync("Exito", "Producto guardado correctamente.", "OK");

        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error", $"Ocurrio un error al guardar el producto: {ex.Message}", "OK");
        }
    }
}