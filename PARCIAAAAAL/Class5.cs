using parcial;
using System.Collections.Generic;

public class Tienda
{
    private List<ItemTienda> inventario = new List<ItemTienda>();

    public Tienda(List<ItemTienda> itemsIniciales)
    {
        if (itemsIniciales == null || itemsIniciales.Count == 0)
            throw new ArgumentException("La tienda debe tener al menos un item");

        for (int i = 0; i < itemsIniciales.Count; i++)
        {
            var it = itemsIniciales[i];
            if (it != null && it.Item != null && it.Item.EsValido())
            {
                inventario.Add(it);
            }
        }

        if (inventario.Count == 0)
            throw new ArgumentException("La tienda debe tener al menos un item válido");
    }
    public bool TieneItems()
    {
        for (int i = 0; i < inventario.Count; i++)
        {
            if (inventario[i].Cantidad > 0)
                return true;
        }
        return false;
    }

    public bool AgregarItem(Item item, int cantidad)
    {
        if (item == null || !item.EsValido() || cantidad <= 0)
            return false;

        ItemTienda existente = null;

        for (int i = 0; i < inventario.Count; i++)
        {
            var it = inventario[i];

            if (it.Item.Nombre == item.Nombre &&
                it.Item.Categoria == item.Categoria)
            {
                existente = it;
                break;
            }
        }

        if (existente != null)
        {
            if (existente.Item.Precio != item.Precio)
                return false;

            return existente.AgregarCantidad(cantidad);
        }

        inventario.Add(new ItemTienda(item, cantidad));
        return true;
    }

    public bool TieneStock(Item item, int cantidad)
    {
        if (item == null || !item.EsValido() || cantidad <= 0)
            return false;

        for (int i = 0; i < inventario.Count; i++)
        {
            var it = inventario[i];

            if (it.Item.Nombre == item.Nombre &&
                it.Item.Categoria == item.Categoria)
            {
                return it.Cantidad >= cantidad;
            }
        }

        return false;
    }

    public bool ReducirStock(Item item, int cantidad)
    {
        if (item == null || !item.EsValido() || cantidad <= 0)
            return false;

        for (int i = 0; i < inventario.Count; i++)
        {
            var it = inventario[i];

            if (it.Item.Nombre == item.Nombre &&
                it.Item.Categoria == item.Categoria)
            {
                return it.ReducirCantidad(cantidad);
            }
        }

        return false;
    }
}