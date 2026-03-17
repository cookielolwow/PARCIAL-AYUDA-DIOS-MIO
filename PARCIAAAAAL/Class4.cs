using parcial;
using System.Collections.Generic;

public class ServicioCompra
{
    public static bool Comprar(Jugador jugador, Tienda tienda, List<ItemTienda> itemsAComprar)
    {
        if (jugador == null || tienda == null || itemsAComprar == null)
            return false;

        decimal costoTotal = 0;

   
        for (int i = 0; i < itemsAComprar.Count; i++)
        {
            var it = itemsAComprar[i];

            if (it == null || it.Item == null || !it.Item.EsValido() || it.Cantidad <= 0)
                return false;

            costoTotal += it.Item.Precio * it.Cantidad;

            if (!tienda.TieneStock(it.Item, it.Cantidad))
                return false;
        }

        if (!jugador.PuedePagar(costoTotal))
            return false;

        
        jugador.GastarOro(costoTotal);

        for (int i = 0; i < itemsAComprar.Count; i++)
        {
            var it = itemsAComprar[i];

            tienda.ReducirStock(it.Item, it.Cantidad);

            for (int j = 0; j < it.Cantidad; j++)
            {
                jugador.AgregarItem(it.Item);
            }
        }

        return true;
    }
}