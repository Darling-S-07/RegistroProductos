using System.Globalization;
using RegistroProductos.Data;
using RegistroProductos.Models;

namespace RegistroProductos.Views;

public partial class RegistroProductosPage : ContentPage
{
    private readonly SQLiteDatabase _database;

    public RegistroProductosPage()
    {
        InitializeComponent();

        string dbPath = Path.Combine(FileSystem.AppDataDirectory, "productos.db3");
        _database = new SQLiteDatabase(dbPath);

        LimpiarFormulario();
    }

    private async void GuardarButton_Clicked(object? sender, EventArgs e)
    {
        // 1. Validar los datos ingresados
        Producto? producto = ValidarFormulario(out List<string> errores, out VisualElement? campoInvalido);

        if (producto is null)
        {
            await DisplayAlertAsync(
                "Datos incompletos o inválidos",
                string.Join(Environment.NewLine, errores),
                "OK");

            campoInvalido?.Focus();
            return;
        }

        // 2. Guardar en SQLite (deshabilitamos el botón para evitar duplicados por doble toque)
        GuardarButton.IsEnabled = false;
        try
        {
            await _database.GuardarProductoAsync(producto);

            await DisplayAlertAsync(
                "Éxito",
                $"El producto se registró correctamente (ID: {producto.IdProducto}).",
                "OK");

            LimpiarFormulario();
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync(
                "Error",
                $"Ocurrió un error al guardar el producto: {ex.Message}",
                "OK");
        }
        finally
        {
            GuardarButton.IsEnabled = true;
        }
    }

    /// Valida todos los campos. Devuelve el producto si todo es correcto;
    /// de lo contrario devuelve null, la lista de errores y el primer campo con error.
    private Producto? ValidarFormulario(out List<string> errores, out VisualElement? primerCampoInvalido)
    {
        var listaErrores = new List<string>();
        VisualElement? primerInvalido = null;

        void Falla(string mensaje, VisualElement campo)
        {
            listaErrores.Add($"• {mensaje}");
            primerInvalido ??= campo;
        }

        // Nombre
        string nombre = NombreProductoEntry.Text?.Trim() ?? string.Empty;
        if (nombre.Length == 0)
            Falla("El nombre del producto es obligatorio.", NombreProductoEntry);

        // Descripción
        string descripcion = DescripcionEntry.Text?.Trim() ?? string.Empty;
        if (descripcion.Length == 0)
            Falla("La descripción es obligatoria.", DescripcionEntry);

        // Precio
        string precioTexto = PrecioEntry.Text?.Trim() ?? string.Empty;
        decimal precio = 0;
        if (precioTexto.Length == 0)
            Falla("El precio es obligatorio.", PrecioEntry);
        else if (!TryParsePrecio(precioTexto, out precio))
            Falla("El precio debe ser un número válido (ej. 25.50).", PrecioEntry);
        else if (precio <= 0)
            Falla("El precio debe ser mayor que 0.", PrecioEntry);

        // Cantidad
        string cantidadTexto = CantidadEntry.Text?.Trim() ?? string.Empty;
        int cantidad = 0;
        if (cantidadTexto.Length == 0)
            Falla("La cantidad es obligatoria.", CantidadEntry);
        else if (!int.TryParse(cantidadTexto, NumberStyles.AllowLeadingSign, CultureInfo.CurrentCulture, out cantidad))
            Falla("La cantidad debe ser un número entero válido.", CantidadEntry);
        else if (cantidad <= 0)
            Falla("La cantidad debe ser mayor que 0.", CantidadEntry);

        // Fecha de registro
        DateTime fecha = DateTime.Today;
        if (FechaRegistroPicker.Date is not DateTime fechaSeleccionada)
            Falla("Selecciona la fecha de registro.", FechaRegistroPicker);
        else if (fechaSeleccionada.Date > DateTime.Today)
            Falla("La fecha de registro no puede ser futura.", FechaRegistroPicker);
        else
            fecha = fechaSeleccionada.Date;

        errores = listaErrores;
        primerCampoInvalido = primerInvalido;

        if (listaErrores.Count > 0)
            return null;

        return new Producto
        {
            NombreProducto = nombre,
            Descripcion = descripcion,
            Precio = precio,
            Cantidad = cantidad,
            FechaRegistro = fecha
        };
    }

    // Acepta el separador decimal de la cultura del dispositivo o el punto.
    private static bool TryParsePrecio(string texto, out decimal precio)
    {
        const NumberStyles estilo = NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign;

        return decimal.TryParse(texto, estilo, CultureInfo.CurrentCulture, out precio)
            || decimal.TryParse(texto, estilo, CultureInfo.InvariantCulture, out precio);
    }

    private void LimpiarFormulario()
    {
        IdEntry.Text = string.Empty;
        NombreProductoEntry.Text = string.Empty;
        DescripcionEntry.Text = string.Empty;
        PrecioEntry.Text = string.Empty;
        CantidadEntry.Text = string.Empty;
        FechaRegistroPicker.Date = DateTime.Today;
    }
}
