using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeekLab07
{
    class Program
    {
        public static DataClasses1DataContext context = new DataClasses1DataContext();

        static void Main(string[] args)
        {
            Console.WriteLine("LISTA DE CLIENTES");
            /*DataSource();
            Console.WriteLine("------");
            IntroToLINQ();
            Console.WriteLine("------");
            Filtering();
            Console.WriteLine("------");
            Ordering();
            Console.WriteLine("------");
            Grouping();
            Console.WriteLine("------");
            Grouping2();
            Console.WriteLine("------");
            Joining();
            Console.WriteLine("------");
            Console.Read();*/

            var clientes = context.clientes.ToList();
            var pedidos = context.Pedidos.ToList();
            var detalles = context.detallesdepedidos.ToList();
            var productos = context.productos.ToList();
            //GetClientWith2OrdersAndMore(clientes);
            //getOrdersLess200(detalles, pedidos);
            //ProvidersMoreThan2Products(productos);
            IntroToLINQ();
            Console.Read();
        }

        static void IntroToLINQ()
        {
            int[] numbers = new int[7] { 0, 1, 2, 3, 4, 5, 6 };

            var numQuery = from num in numbers
                           where (num % 2) == 0
                           select num;
            var numQ = numbers.Where(x => x % 2 == 0);

            foreach(int num in numQ)
            {
                Console.WriteLine(num);
            }
        }

        static void DataSource()
        {
            var queryAllCustomers = from cust in context.clientes
                                    select cust;

            foreach(var item in queryAllCustomers)
            {
                Console.WriteLine(item.NombreCompañia);
            }
        }

        static void Filtering()
        {
            var queryLondonCustomers = from cust in context.clientes
                                       where cust.Ciudad == "Londres"
                                       select cust;

            var londonQ = context.clientes.Where(x => x.Ciudad == "Londres");
            foreach (var item in londonQ)
            {
                Console.WriteLine(item.Ciudad);
            }
        }

        static void Ordering()
        {
            var queryLondonCustomer3 = from cust in context.clientes
                                       where cust.Ciudad == "London"
                                       orderby cust.NombreCompañia ascending
                                       select cust;

            var londonQ2 = context.clientes.Where(x => x.Ciudad == "London").OrderBy(x => x.NombreCompañia);
            foreach (var item in londonQ2)
            {
                Console.WriteLine(item.NombreCompañia);
            }
        }
        
        static void Grouping()
        {
            var queryCustomerByCity = from cust in context.clientes
                                      group cust by cust.Ciudad;

            var queryQ = context.clientes.GroupBy(x => x.Ciudad);

            foreach (var customerGroup in queryQ)
            {
                Console.WriteLine(customerGroup.Key);
                foreach(cliente customer in customerGroup)
                {
                    Console.WriteLine("      {0}", customer.NombreCompañia);
                }
            }
        }

        static void Grouping2()
        {
            var custQuery = from cust in context.clientes
                            group cust by cust.Ciudad into custGroup
                            where custGroup.Count() > 2
                            orderby custGroup.Key
                            select custGroup;
            var custQ = context.clientes.GroupBy(x => x.Ciudad).Where(x => new { Id = x.Key, Key = x.Count() > 2})
            foreach(var item in custQuery)
            {
                Console.WriteLine(item.Key);
            }
        }
        
        static void Joining()
        {
            var innerJoinQuery = from cust in context.clientes
                                 join dist in context.Pedidos on cust.idCliente equals dist.IdCliente
                                 select new { CustomerName = cust.NombreCompañia, DistributorName = dist.PaisDestinatario };
            foreach(var item in innerJoinQuery)
            {
                Console.WriteLine(item.CustomerName);
            }
        }

        static void getOrdersFrom5Years(List<Pedido> pedidos)
        {
            var orders = pedidos
                .Where(x => x.FechaPedido >= new DateTime(2014, 01, 01) 
                        && x.FechaPedido <= new DateTime(2019, 01, 01)); 
            
            /*=>from orders in context.Pedidos
                              //where orders.FechaPedido >= new DateTime(2014, 01, 01) && orders.FechaPedido >= new DateTime(2019, 01, 01)
                              select orders;*/
            foreach(var item in orders)
            {
                Console.WriteLine(item.FechaPedido);
            }
        }

        static void GetClientWith2OrdersAndMore(List<cliente> clientes)
        {
            var clients = clientes.Where(x => x.Pedidos.Count >= 2);

            foreach(var item in clients)
            {
                Console.WriteLine(item.NombreContacto);
            }
        }

        static void getOrdersLess200(List<detallesdepedido> detallePedidos, List<Pedido> pedidos)
        {
            var orders = pedidos
                .Join(detallePedidos, x => x.IdPedido, y => y.idpedido, (x, y) => new { pedidos = x, detallePedidos = y })
                .Where(z => z.detallePedidos.preciounidad * z.detallePedidos.cantidad > 200)
                .Select(a => a.pedidos.IdPedido);

            foreach(var item in orders)
            {
                Console.WriteLine(item);
            }
        }

        static void ProvidersMoreThan2Products(List<producto> productos)
        {
            var providers = productos.GroupBy(x => x.proveedores.nombreCompañia)
                .Where(x => x.Count()>= 2).Select(item => new { Proveedor = item.Key, Count = item.Count() });

            foreach(var item in providers)
            {
                Console.WriteLine(item);
            }
        }
    }
}
