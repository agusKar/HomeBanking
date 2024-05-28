using HomeBanking.Models;
using System;
using System.Linq;

namespace HomeBanking.Models
{
    public class DBInitializer
    {
        public static void Initialize(HomeBankingContext context)
        {
            if (!context.Clients.Any())
            {
                var clients = new Client[]
                {
                    new Client { Email = "agustin@gmail.com", FirstName="Agustin", LastName="KAR", Password="123456"},
                    new Client { Email = "maria@gmail.com", FirstName="Maria", LastName="Petrone", Password="345534"},
                    new Client { Email = "jose@gmail.com", FirstName="Jose", LastName="KAR", Password="678"},
                    new Client { Email = "mario@gmail.com", FirstName="Mario", LastName="Lopez", Password="6789"},
                    new Client { Email = "juan@gmail.com", FirstName="Juan", LastName="Gustav", Password="234"}
                };

                context.Clients.AddRange(clients);

                //guardamos
                context.SaveChanges();
            }

        }
    }
}

