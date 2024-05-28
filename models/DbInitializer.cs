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
            }

            //escribir logica de ingreso de datos de accounts
            if (!context.Accounts.Any()) {

                var accountGet = context.Clients.FirstOrDefault(c => c.Email == "agustin@gmail.com");

                if (accountGet != null)
                {
                    var accounts = new Account[]
                    {
                        new Account { Number = "VN001", Balance = 250000, ClientId = accountGet.Id, CreationDate = DateTime.Now },
                        new Account { Number = "VN002", Balance = 450000, ClientId = accountGet.Id, CreationDate = DateTime.Now }
                    };
                    context.Accounts.AddRange(accounts);
                }
            }
            //guardamos
            context.SaveChanges();

        }
    }
}

