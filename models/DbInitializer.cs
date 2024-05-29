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

            //escribir logica de ingreso de datos de accounts
            if (!context.Accounts.Any()) {

                var clientGet = context.Clients.FirstOrDefault(c => c.Email == "agustin@gmail.com");

                if (clientGet != null)
                {
                    var accounts = new Account[]
                    {
                        new Account { Number = "VN001", Balance = 250000, ClientId = clientGet.Id, CreationDate = DateTime.Now },
                        new Account { Number = "VN002", Balance = 450000, ClientId = clientGet.Id, CreationDate = DateTime.Now }
                    };
                    context.Accounts.AddRange(accounts);
                    //guardamos
                    context.SaveChanges();
                }
            }
            if (!context.Transactions.Any())
            {
                var accountGet = context.Accounts.FirstOrDefault(a => a.Number == "VN001");
                if(accountGet != null)
                {
                    var listTransactions = new Transaction[]
                    {
                        new Transaction{AccountId = accountGet.Id, Amount = -10000, Description = "Transaccion debitada desde Mercado pago", Type = TransactionType.DEBIT.ToString()},
                        new Transaction{AccountId = accountGet.Id, Amount = 7000, Description = "Compra en cuotas desde TIENDAMIA", Type = TransactionType.CREDIT.ToString()},
                        new Transaction{AccountId = accountGet.Id, Amount = -40000, Description = "Compra en tienda ML", Type = TransactionType.DEBIT.ToString()}

                    };
                    context.Transactions.AddRange(listTransactions);
                    context.SaveChanges();
                }
            }

        }
    }
}

