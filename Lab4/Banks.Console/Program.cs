using System;
using Banks.Tools;
using Spectre.Console;

namespace Banks;

public static class Program
{
    private static void Main()
    {
        var rule = new Rule("[lightslateblue]Bank's Console[/]");
        AnsiConsole.Write(rule);
        CentralBank centralBank = CentralBank.GetInstance();
        List<Client> registeredClients = new List<Client>();
        string name;
        string surname;

        while (true)
        {
            var framework = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Choose account mode")
                    .AddChoices(new[]
                    {
                        "Client mode", "Bank manager mode", "Exit"
                    }));
            switch (framework)
            {
                case "Client mode":
                {
                    var clientsOption = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                            .Title("Pick one of the operations")
                            .AddChoices(new[]
                            {
                                "Registration", "Create account", "Accounts info", "Fill up account",
                                "Withdraw money", "Transfer money", "Subscribe to bank's updates",
                            }));
                    switch (clientsOption)
                    {
                        case "Registration":
                            IClientBuilder builder = new ClientBuilder();
                            name = AnsiConsole.Ask<string>("Enter your [green]name[/]");
                            surname = AnsiConsole.Ask<string>("Enter your [green]surname[/]");
                            FullName clientName = new FullName(name, surname);
                            builder.SetName(clientName);
                            if (!AnsiConsole.Confirm("Would you like to add your address?"))
                            {
                                builder.SetAddress(null);
                            }
                            else
                            {
                                var city = AnsiConsole.Ask<string>("Enter your [green]city[/]");
                                var street = AnsiConsole.Ask<string>("Enter your [green]street[/]");
                                var houseNumber = AnsiConsole.Ask<string>("Enter your [green]house number[/]");
                                var mailCode = AnsiConsole.Ask<int>("Enter your [green]mail code[/]");
                                builder.SetAddress(new Address(city, street, houseNumber, mailCode));
                            }

                            if (!AnsiConsole.Confirm("Would you like to add your passport data?"))
                            {
                                builder.SetPassport(null);
                            }
                            else
                            {
                                var series = AnsiConsole.Ask<int>("Enter your passport [green]series[/]");
                                var number = AnsiConsole.Ask<int>("Enter your passport [green]number[/]");
                                builder.SetPassport(new PassportData(series, number));
                            }

                            registeredClients.Add(builder.BuildClient());
                            break;
                        case "Create account":
                            name = AnsiConsole.Ask<string>("Enter your [green]name[/]");
                            surname = AnsiConsole.Ask<string>("Enter your [green]surname[/]");
                            FullName fullName = new FullName(name, surname);
                            foreach (var regClient in registeredClients)
                            {
                                if (fullName == regClient.FullName)
                                {
                                    foreach (var bank in centralBank.Banks)
                                        AnsiConsole.WriteLine($"{bank.Name}");
                                    string bankName =
                                        AnsiConsole.Ask<string>(
                                            "Choose bank you want to create an account in (input name");
                                    foreach (var bank in centralBank.Banks)
                                    {
                                        if (bankName == bank.Name)
                                        {
                                            var accountOption = AnsiConsole.Prompt(
                                                new SelectionPrompt<string>()
                                                    .Title("Choose the type of account")
                                                    .AddChoices(new[]
                                                    {
                                                        "Deposit", "Debit", "Credit",
                                                    }));
                                            if (accountOption == "Deposit")
                                            {
                                                decimal sum = AnsiConsole.Ask<decimal>("Type sum to put");
                                                int term = AnsiConsole.Ask<int>("Type deposit period (in months)");
                                                bank.CreateDepositAccount(regClient, sum, DateTime.Now, term);
                                            }
                                            else if (accountOption == "Debit")
                                            {
                                                decimal sum = AnsiConsole.Ask<decimal>("Type sum to put");
                                                bank.CreateDebitAccount(regClient, sum);
                                            }
                                            else if (accountOption == "Credit")
                                            {
                                                bank.CreateCreditAccount(regClient);
                                            }
                                        }
                                        else
                                        {
                                            AnsiConsole.WriteLine("Invalid bank name input");
                                        }
                                    }
                                }
                                else
                                {
                                    AnsiConsole.WriteLine("You need to register first to create an account in bank");
                                }
                            }

                            break;
                        case "Accounts info":
                            name = AnsiConsole.Ask<string>("Enter your [green]name[/]");
                            surname = AnsiConsole.Ask<string>("Enter your [green]surname[/]");
                            FullName fName = new FullName(name, surname);
                            foreach (var client in registeredClients)
                            {
                                if (fName == client.FullName)
                                {
                                    foreach (var bank in centralBank.Banks)
                                    {
                                        foreach (var account in bank.Accounts)
                                        {
                                            if (account.Owner == client)
                                            {
                                                AnsiConsole.WriteLine($"{account.Id} {account.Balance}");
                                            }
                                        }
                                    }
                                }
                            }

                            break;
                        case "Fill up account":
                            var id = Guid.Parse(AnsiConsole.Ask<string>("Enter your [green]account id[/]"));
                            var acc = centralBank.FindAccountById(id);
                            var moneyToPut = AnsiConsole.Ask<decimal>("Enter the amount of money you want to put");
                            acc.FillUp(moneyToPut);

                            break;
                        case "Withdraw money":
                            var id_ = Guid.Parse(AnsiConsole.Ask<string>("Enter your [green]account id[/]"));
                            var accountById = centralBank.FindAccountById(id_);
                            var moneyToWithdraw = AnsiConsole.Ask<decimal>("Enter the amount of money you want to put");
                            accountById.FillUp(moneyToWithdraw);

                            break;
                        case "Transfer money":
                            var anotherId = Guid.Parse(AnsiConsole.Ask<string>("Enter your [green]account id[/]"));
                            var notMyId =
                                Guid.Parse(AnsiConsole.Ask<string>(
                                    "Enter [green]account id[/] you want to transfer the money to"));
                            var recipientAccount = centralBank.FindAccountById(notMyId);
                            var senderAccount = centralBank.FindAccountById(anotherId);
                            var moneyToTransfer =
                                AnsiConsole.Ask<decimal>("Enter the amount of money you want to transfer");
                            senderAccount.Transfer(moneyToTransfer, recipientAccount);

                            break;
                        case "Subscribe to bank's updates":
                            name = AnsiConsole.Ask<string>("Enter your [green]name[/]");
                            surname = AnsiConsole.Ask<string>("Enter your [green]surname[/]");
                            FullName ffulName = new FullName(name, surname);
                            foreach (var client in registeredClients.Where(client => ffulName == client.FullName))
                            {
                                client.ConfirmSubscription();
                            }

                            break;
                    }

                    break;
                }

                case "Bank manager mode":
                    var bankManagerOption = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                            .Title("Pick one of the operations")
                            .AddChoices(new[]
                            {
                                "Create bank", "Rewind time",
                            }));
                    if (bankManagerOption == "Create bank")
                    {
                        var percentSelection = AnsiConsole.Prompt(
                            new SelectionPrompt<string>()
                                .Title("Set all the percents")
                                .AddChoices(new[]
                                {
                                    "Debit percentage", "Minimal deposit percentage",
                                    "Middle deposit percentage", "Maximum deposit percentage",
                                    "Credit commission",
                                }));
                        double debitPercent = 0;
                        double minDepositPercent = 0;
                        double middleDepositPercent = 0;
                        double maxDepositPercent = 0;
                        double commissionCredit = 0;
                        switch (percentSelection)
                        {
                            case "Debit percentage":
                                var debitPercentage = AnsiConsole.Prompt(
                                    new TextPrompt<double>("Enter desired [green]percentage for debit account[/]")
                                        .PromptStyle("green")
                                        .ValidationErrorMessage("[red]Not in set range[/]")
                                        .Validate(percent =>
                                        {
                                            return percent switch
                                            {
                                                < Constans.MinDebitPercentage => ValidationResult.Error(
                                                    "[red]The percentage is too small[/]"),
                                                _ => ValidationResult.Success(),
                                            };
                                        }));
                                debitPercent = debitPercentage;
                                break;
                            case "Minimal deposit percentage":
                                var minDepositPercentage = AnsiConsole.Prompt(
                                    new TextPrompt<double>(
                                            "Enter desired [green]minimal percentage for deposit account[/]")
                                        .PromptStyle("green")
                                        .ValidationErrorMessage("[red]Not in set range[/]")
                                        .Validate(percent =>
                                        {
                                            return percent switch
                                            {
                                                < Constans.MinLowDepositPercentage => ValidationResult.Error(
                                                    "[red]The percentage is too small[/]"),
                                                _ => ValidationResult.Success(),
                                            };
                                        }));
                                minDepositPercent = minDepositPercentage;
                                break;
                            case "Middle deposit percentage":
                                var middleDepositPercentage = AnsiConsole.Prompt(
                                    new TextPrompt<double>(
                                            "Enter desired [green]middle percentage for deposit account[/]")
                                        .PromptStyle("green")
                                        .ValidationErrorMessage("[red]Not in set range[/]")
                                        .Validate(percent =>
                                        {
                                            return percent switch
                                            {
                                                < Constans.MinAverageDepositPercentage => ValidationResult.Error(
                                                    "[red]The percentage is too small[/]"),
                                                _ => ValidationResult.Success(),
                                            };
                                        }));
                                middleDepositPercent = middleDepositPercentage;
                                break;
                            case "Maximum deposit percentage":
                                var maxDepositPercentage = AnsiConsole.Prompt(
                                    new TextPrompt<double>(
                                            "Enter desired [green]maximum percentage for deposit account[/]")
                                        .PromptStyle("green")
                                        .ValidationErrorMessage("[red]Not in set range[/]")
                                        .Validate(percent =>
                                        {
                                            return percent switch
                                            {
                                                < Constans.MinHighDepositPercentage => ValidationResult.Error(
                                                    "[red]The percentage is too small[/]"),
                                                > Constans.MaxHighDepositPercentage => ValidationResult.Error(
                                                    "[red]The percentage is too large[/]"),
                                                _ => ValidationResult.Success(),
                                            };
                                        }));
                                maxDepositPercent = maxDepositPercentage;
                                break;
                            case "Credit commission":
                                var creditCommission = AnsiConsole.Prompt(
                                    new TextPrompt<double>(
                                            "Enter desired [green]percentage for credit commission[/]")
                                        .PromptStyle("green")
                                        .ValidationErrorMessage("[red]Not in set range[/]")
                                        .Validate(percent =>
                                        {
                                            return percent switch
                                            {
                                                < Constans.MinCreditCommission => ValidationResult.Error(
                                                    "[red]The percentage is too small[/]"),
                                                _ => ValidationResult.Success(),
                                            };
                                        }));
                                commissionCredit = creditCommission;
                                break;
                        }

                        decimal limitForCredit = 0;
                        decimal minDepositSum = 0;
                        decimal middleDepositSum = 0;
                        decimal starterCapital = 0;
                        var limitSelection = AnsiConsole.Prompt(
                            new SelectionPrompt<string>()
                                .Title("Set all the limits")
                                .AddChoices(new[]
                                {
                                    "Credit limit", "Little deposit sum",
                                    "Average deposit sum", "Authorized capital",
                                }));
                        switch (limitSelection)
                        {
                            case "Credit limit":
                                var creditLimit = AnsiConsole.Prompt(
                                    new TextPrompt<decimal>(
                                            "Enter desired [green]credit limit[/]")
                                        .PromptStyle("green")
                                        .ValidationErrorMessage("[red]Not in set range[/]")
                                        .Validate(sum =>
                                        {
                                            return sum switch
                                            {
                                                < Constans.MinCreditLimit => ValidationResult.Error(
                                                    "[red]The amount of money is too small[/]"),
                                                _ => ValidationResult.Success(),
                                            };
                                        }));
                                limitForCredit = creditLimit;
                                break;
                            case "Little deposit sum":
                                var littleDepositSum = AnsiConsole.Prompt(
                                    new TextPrompt<decimal>(
                                            "Enter desired [green]first border[/] for deposit account")
                                        .PromptStyle("green")
                                        .ValidationErrorMessage("[red]Not in set range[/]")
                                        .Validate(sum =>
                                        {
                                            return sum switch
                                            {
                                                < Constans.MinLowDepositSum => ValidationResult.Error(
                                                    "[red]The amount of money is too small[/]"),
                                                _ => ValidationResult.Success(),
                                            };
                                        }));
                                minDepositSum = littleDepositSum;
                                break;
                            case "Average deposit sum":
                                var averageDepositSum = AnsiConsole.Prompt(
                                    new TextPrompt<decimal>(
                                            "Enter desired [green]second border[/] for deposit account")
                                        .PromptStyle("green")
                                        .ValidationErrorMessage("[red]Not in set range[/]")
                                        .Validate(sum =>
                                        {
                                            return sum switch
                                            {
                                                < Constans.MinLowDepositSum => ValidationResult.Error(
                                                    "[red]The amount of money is too small[/]"),
                                                > Constans.MinAverageDepositSum => ValidationResult.Error(
                                                    "[red]The amount of money is too small[/]"),
                                                _ => ValidationResult.Success(),
                                            };
                                        }));
                                middleDepositSum = averageDepositSum;
                                break;
                            case "Authorized capital":
                                var capital = AnsiConsole.Prompt(
                                    new TextPrompt<decimal>(
                                            "Enter desired [green]starter capital[/] for bank")
                                        .PromptStyle("green")
                                        .ValidationErrorMessage("[red]Not in set range[/]")
                                        .Validate(sum =>
                                        {
                                            return sum switch
                                            {
                                                < Constans.MinMoneyToCreateBank => ValidationResult.Error(
                                                    "[red]The amount of money is too small[/]"),
                                                _ => ValidationResult.Success(),
                                            };
                                        }));
                                starterCapital = capital;
                                break;
                        }

                        BankSettings settings = new BankSettings(
                            debitPercent,
                            minDepositPercent,
                            minDepositSum,
                            middleDepositPercent,
                            middleDepositSum,
                            maxDepositPercent,
                            limitForCredit,
                            commissionCredit,
                            starterCapital);
                        var bankName = AnsiConsole.Ask<string>("Enter [green]bank name[/]");
                        centralBank.RegisterBank(bankName, settings);
                    }
                    else if (bankManagerOption == "Rewind time")
                    {
                        var period = AnsiConsole.Ask<int>("Enter [green]the amount of days you want to rewind[/]");
                        centralBank.SkipTime(period);
                    }

                    break;

                case "Exit":
                    return;
            }
        }
    }
}
