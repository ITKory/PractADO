using ClassLibrary;
using System;
using System.Linq;

namespace class1
{
    internal class Program
    {
     
        static void Main(string[] args)
        {
            MySQLConnectionString connection = new MySQLConnectionString("storage");
            string  connectionString = connection.GetConnectionString();
         



                Console.WriteLine("Products:");
                foreach (var produc in new BD(connectionString).GetAllProsucts())
                {
                    Console.WriteLine($"id:{produc.Id}\ncost:{produc.Cost}\nproduct name:{produc.Name}\nType:{produc.TypeId}\nCount{produc.Count}");
                    Console.WriteLine();
                }

                Console.WriteLine("Providers:");
                foreach (var provider in new BD(connectionString).GetAllProviders())
                {
                    Console.WriteLine($"id:{provider.Id}\nprovider name:{provider.Name}\n");
                    Console.WriteLine();
                }

                Console.WriteLine("Types:");
                foreach (var type in new BD(connectionString).GetAllTypes())
                {
                    Console.WriteLine($"id:{type.Id}\nprovider name:{type.Name}\n");
                    Console.WriteLine();
                }

                Console.WriteLine("supplies:");
                foreach (var supply in new BD(connectionString).GetAllSupplys())
                {
                    Console.WriteLine($"id:{supply.Id}\nProviderID:{supply.ProviderId}\nDate:{supply.Date}\nDelivery Count{supply.DCount}\nProductId{supply.ProductId}");
                    Console.WriteLine();
                }
            Console.WriteLine();
            Console.WriteLine($"max:{new BD(connectionString).GrtMaxFrom("count", "product")}\nmin:{new BD(connectionString).GrtMinFrom("count", "product")}");
            Console.WriteLine($"maxCost:{new BD(connectionString).GrtMaxFrom("cost", "product")}\nminCost:{new BD(connectionString).GrtMinFrom("cost", "product")}");
            Console.WriteLine();
            Console.WriteLine("Показать товары, заданной категории");

            foreach (var v in new BD(connectionString).Select("product", "type_id=1", "*"))
            {
                Console.WriteLine(v);

            }
            Console.WriteLine();
            Console.WriteLine("Показать товары, заданного поставщика");
            foreach (var v in new BD(connectionString).Select("supply", "provider_id=1", "*"))
            {
                Console.WriteLine(v);

            }
            Console.WriteLine();


            Console.WriteLine(" Показать среднее количество товаров по каждому типу товара");
            foreach (var sid in new BD(connectionString).Select("type", "1", "id"))
            {
                int sum = 0;
                var list = new BD(connectionString).Select("product", $"type_id={sid}", "count");
                foreach (var v in list)
                    sum += Convert.ToInt32(v);
                Console.WriteLine(sum / list.Count);
            }
            Console.WriteLine();

            Console.WriteLine("Показать самый старый товар на складе;");
            foreach (var v in new BD(connectionString).Select("product", "id = ( SELECT product_id FROM `supply` WHERE date = (SELECT MIN(date) FROM `supply`))=1", "name "))
            {
                Console.WriteLine(v);

            }
        }
    }
}
