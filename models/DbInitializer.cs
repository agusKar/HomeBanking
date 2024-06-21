namespace HomeBanking.Models
{
    public class DBInitializer
    {
        public static void Initialize(HomeBankingContext context)
        {
            //if (!context.Clients.Any())
            //{
            //    var clients = new Client[]
            //    {
            //        new Client { Email = "agustin@gmail.com", FirstName="Agustin", LastName="KAR", Password="123456"},
            //        new Client { Email = "maria@gmail.com", FirstName="Maria", LastName="Petrone", Password="345534"},
            //        new Client { Email = "jose@gmail.com", FirstName="Jose", LastName="KAR", Password="678"},
            //        new Client { Email = "mario@gmail.com", FirstName="Mario", LastName="Lopez", Password="6789"},
            //        new Client { Email = "juan@gmail.com", FirstName="Juan", LastName="Gustav", Password="234"}
            //    };

            //    context.Clients.AddRange(clients);
            //    //guardamos
            //    context.SaveChanges();
            //}

            ////escribir logica de ingreso de datos de accounts
            //if (!context.Accounts.Any()) {

            //    var clientGet = context.Clients.FirstOrDefault(c => c.Email == "agustin@gmail.com");

            //    if (clientGet != null)
            //    {
            //        var accounts = new Account[]
            //        {
            //            new Account { Number = "VN001", Balance = 250000, ClientId = clientGet.Id, CreationDate = DateTime.Now },
            //            new Account { Number = "VN002", Balance = 450000, ClientId = clientGet.Id, CreationDate = DateTime.Now }
            //        };
            //        context.Accounts.AddRange(accounts);
            //        //guardamos
            //        context.SaveChanges();
            //    }
            //}
            //if (!context.Transactions.Any())
            //{
            //    var accountGet = context.Accounts.FirstOrDefault(a => a.Number == "VN001");
            //    if(accountGet != null)
            //    {
            //        var listTransactions = new Transaction[]
            //        {
            //            new Transaction{AccountId = accountGet.Id, Amount = -10000, Description = "Transaccion debitada desde Mercado pago", Type = TransactionType.DEBIT.ToString(), Date = DateTime.Now.AddMonths(1)},
            //            new Transaction{AccountId = accountGet.Id, Amount = 7000, Description = "Compra en cuotas desde TIENDAMIA", Type = TransactionType.CREDIT.ToString(), Date = DateTime.Now.AddDays(2)},
            //            new Transaction{AccountId = accountGet.Id, Amount = -40000, Description = "Compra en tienda ML", Type = TransactionType.DEBIT.ToString(), Date = DateTime.Now}

            //        };
            //        context.Transactions.AddRange(listTransactions);
            //        context.SaveChanges();
            //    }
            //}
            
            if (!context.Loans.Any())
            {
                var loans = new Loan[]
                {
                    new Loan { Name = "Hipotecario", MaxAmount = 500000, Payments = "12,24,36,48,60" },
                    new Loan { Name = "Personal", MaxAmount = 100000, Payments = "6,12,24" },
                    new Loan { Name = "Automotriz", MaxAmount = 300000, Payments = "6,12,24,36" },
                };
                context.Loans.AddRange(loans);
                context.SaveChanges();
            }

            ////agarrar el client
            //var clientToLoan = context.Clients.FirstOrDefault(c => c.Email == "agustin@gmail.com");
            //if (!context.ClientLoans.Any()) {
            //    var loan1 = context.Loans.FirstOrDefault(l => l.Name == "Hipotecario");
            //    if (loan1 != null)
            //    {
            //        var clientLoan1 = new ClientLoan
            //        {
            //            Amount = 400000,
            //            ClientId = clientToLoan.Id,
            //            LoanId = loan1.Id,
            //            Payments = "60"
            //        };
            //        context.ClientLoans.Add(clientLoan1);
            //    }

            //    var loan2 = context.Loans.FirstOrDefault(l => l.Name == "Personal");
            //    if (loan2 != null)
            //    {
            //        var clientLoan2 = new ClientLoan
            //        {
            //            Amount = 50000,
            //            ClientId = clientToLoan.Id,
            //            LoanId = loan2.Id,
            //            Payments = "12"
            //        };
            //        context.ClientLoans.Add(clientLoan2);
            //    }

            //    var loan3 = context.Loans.FirstOrDefault(l => l.Name == "Automotriz");
            //    if (loan3 != null)
            //    {
            //        var clientLoan3 = new ClientLoan
            //        {
            //            Amount = 100000,
            //            ClientId = clientToLoan.Id,
            //            LoanId = loan3.Id,
            //            Payments = "24"
            //        };
            //        context.ClientLoans.Add(clientLoan3);
            //    }

            //    //guardamos todos los prestamos
            //    context.SaveChanges();
            //}
            
            //if (!context.Cards.Any())
            //{
            //    // Traer el clientId para asignarselo a cada tarjeta nueva creada
            //    var clientGet = context.Clients.FirstOrDefault(c => c.Email == "agustin@gmail.com");
            //    if (clientGet != null) {
            //        // crear las tarjeta y asignarle el ID a cada una y dsps guardarlas en la entidad
            //        var cardsCollection = new Card[]
            //        {
            //            new Card {
            //                ClientId = clientGet.Id,
            //                CardHolder = clientGet.FirstName + " " + clientGet.LastName,
            //                Type = CardType.DEBIT.ToString(),
            //                Color = CardColor.TITANIUM.ToString(),
            //                Number = "3325-6745-7876-4445",
            //                Cvv = 823,
            //                FromDate= DateTime.Now,
            //                ThruDate= DateTime.Now.AddYears(4)
            //            },
            //            new Card {
            //                ClientId = clientGet.Id,
            //                CardHolder = clientGet.FirstName + " " + clientGet.LastName,
            //                Type = CardType.DEBIT.ToString(),
            //                Color = CardColor.GOLD.ToString(),
            //                Number = "4563-2345-7893-3567",
            //                Cvv = 645,
            //                FromDate= DateTime.Now,
            //                ThruDate= DateTime.Now.AddYears(4)
            //            },
            //            new Card {
            //                ClientId = clientGet.Id,
            //                CardHolder = clientGet.FirstName + " " + clientGet.LastName,
            //                Type = CardType.CREDIT.ToString(),
            //                Color = CardColor.SILVER.ToString(),
            //                Number = "2234-6745-552-7888",
            //                Cvv = 990,
            //                FromDate= DateTime.Now,
            //                ThruDate= DateTime.Now.AddYears(4)
            //            },
            //        };
            //        context.Cards.AddRange(cardsCollection);
            //        context.SaveChanges();
            //    }
            //}

        }
    }
}

