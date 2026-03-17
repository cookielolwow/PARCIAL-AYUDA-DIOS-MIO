using NUnit.Framework;
using System.Collections.Generic;

namespace parcial
{
    public static class TestDataRPG
    {
        public static readonly Item espada = new Item("Espada", 10m, CategoriaItem.Weapon);
        public static readonly Item armadura = new Item("Armadura", 20m, CategoriaItem.Armor);
        public static readonly Item anillo = new Item("Anillo", 5m, CategoriaItem.Accessory);
        public static readonly Item pocion = new Item("Pocion", 3m, CategoriaItem.Supply);

        public static IEnumerable<TestCaseData> CrearItemData()
        {
            yield return new TestCaseData(espada.Nombre, espada.Precio, espada.Categoria, true);
            yield return new TestCaseData(armadura.Nombre, armadura.Precio, armadura.Categoria, true);
            yield return new TestCaseData(anillo.Nombre, anillo.Precio, anillo.Categoria, true);
            yield return new TestCaseData(pocion.Nombre, pocion.Precio, pocion.Categoria, true);
            yield return new TestCaseData("", 10m, CategoriaItem.Weapon, false);
            yield return new TestCaseData("ItemMalo", -1m, CategoriaItem.Weapon, false);
        }

        public static IEnumerable<TestCaseData> JugadorPuedePagarData()
        {
            yield return new TestCaseData(100m, 50m, true);
            yield return new TestCaseData(10m, 20m, false);
        }

        public static IEnumerable<TestCaseData> ItemInventarioData()
        {
            yield return new TestCaseData(espada, true, false);
            yield return new TestCaseData(pocion, false, true);
        }

        public static IEnumerable<TestCaseData> TiendaTieneItemsData()
        {
            yield return new TestCaseData(0, false);
            yield return new TestCaseData(1, true);
        }

        public static IEnumerable<TestCaseData> ComprarItemsData()
        {
            yield return new TestCaseData(100m, 10m, 5, true);
            yield return new TestCaseData(10m, 10m, 2, false);
            yield return new TestCaseData(100m, 10m, 20, false);
        }
    }

    [TestFixture]
    public class PruebasCreacionItem
    {
        [TestCaseSource(typeof(TestDataRPG), nameof(TestDataRPG.CrearItemData))]
        public void CrearItem(string nombre, decimal precio, CategoriaItem categoria, bool esperado)
        {
            Item item = new Item(nombre, precio, categoria);
            Assert.That(item.EsValido(), Is.EqualTo(esperado));
        }
    }

    [TestFixture]
    public class PruebasCreacionTienda
    {
        [TestCaseSource(typeof(TestDataRPG), nameof(TestDataRPG.TiendaTieneItemsData))]
        public void TiendaTieneItems(int cantidadItems, bool esperado)
        {
            List<ItemTienda> lista = new List<ItemTienda>();
            for (int i = 0; i < cantidadItems; i++)
                lista.Add(new ItemTienda(TestDataRPG.espada, 5));

            Tienda tienda = new Tienda(lista);
            Assert.That(tienda.TieneItems(), Is.EqualTo(esperado));
        }
    }

    [TestFixture]
    public class PruebasJugador
    {
        [TestCaseSource(typeof(TestDataRPG), nameof(TestDataRPG.JugadorPuedePagarData))]
        public void JugadorPuedePagar(decimal oroInicial, decimal costo, bool esperado)
        {
            Jugador jugador = new Jugador(oroInicial);
            Assert.That(jugador.PuedePagar(costo), Is.EqualTo(esperado));
        }
    }

    [TestFixture]
    public class PruebasCompra
    {
        [TestCaseSource(typeof(TestDataRPG), nameof(TestDataRPG.ComprarItemsData))]
        public void ComprarItems(decimal oro, decimal precio, int cantidad, bool esperado)
        {
            Item item = new Item("Pocion", precio, CategoriaItem.Supply);
            Tienda tienda = new Tienda(new List<ItemTienda> { new ItemTienda(item, 10) });
            Jugador jugador = new Jugador(oro);
            List<ItemTienda> compra = new List<ItemTienda> { new ItemTienda(item, cantidad) };

            bool resultado = ServicioCompra.Comprar(jugador, tienda, compra);
            Assert.That(resultado, Is.EqualTo(esperado));

            if (resultado)
            {
                Assert.That(jugador.Consumibles.Contains(item), Is.True);
                Assert.That(jugador.Oro, Is.EqualTo(oro - precio * cantidad));
                Assert.That(tienda.TieneStock(item, 10 - cantidad), Is.True);
                Assert.That(tienda.TieneStock(item, 11 - cantidad), Is.False);
            }
        }
    }

    [TestFixture]
    public class PruebasInventarioJugador
    {
        [TestCaseSource(typeof(TestDataRPG), nameof(TestDataRPG.ItemInventarioData))]
        public void ItemSeAgregaAlInventarioCorrecto(Item item, bool enEquipamiento, bool enConsumibles)
        {
            Jugador jugador = new Jugador(100m);
            jugador.AgregarItem(item);

            Assert.That(jugador.Equipamiento.Contains(item), Is.EqualTo(enEquipamiento));
            Assert.That(jugador.Consumibles.Contains(item), Is.EqualTo(enConsumibles));
        }
    }
}