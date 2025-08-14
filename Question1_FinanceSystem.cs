using System;
using System.Collections.Generic;

namespace DCIT318_Assignment3.Question1
{
    // Question 1a: Create a record type to represent financial data
    public record Transaction(int Id, DateTime Date, decimal Amount, string Category);

    // Question 1b: Define an interface ITransactionProcessor
    public interface ITransactionProcessor
    {
        void Process(Transaction transaction);
    }

    // Question 1c: Create three concrete classes implementing ITransactionProcessor
    public class BankTransferProcessor : ITransactionProcessor
    {
        public void Process(Transaction transaction)
        {
            Console.WriteLine($"Bank Transfer: Processing ${transaction.Amount} for {transaction.Category}");
        }
    }

    public class MobileMoneyProcessor : ITransactionProcessor
    {
        public void Process(Transaction transaction)
        {
            Console.WriteLine($"Mobile Money: Processing ${transaction.Amount} for {transaction.Category}");
        }
    }

    public class CryptoWalletProcessor : ITransactionProcessor
    {
        public void Process(Transaction transaction)
        {
            Console.WriteLine($"Crypto Wallet: Processing ${transaction.Amount} for {transaction.Category}");
        }
    }

    // Question 1d: Define a base class Account
    public class Account
    {
        public string AccountNumber { get; set; }
        public decimal Balance { get; protected set; }

        public Account(string accountNumber, decimal initialBalance)
        {
            AccountNumber = accountNumber;
            Balance = initialBalance;
        }

        public virtual void ApplyTransaction(Transaction transaction)
        {
            Balance -= transaction.Amount;
        }
    }

    // Question 1e: Define a sealed class SavingsAccount
    public sealed class SavingsAccount : Account
    {
        public SavingsAccount(string accountNumber, decimal initialBalance) 
            : base(accountNumber, initialBalance)
        {
        }

        public override void ApplyTransaction(Transaction transaction)
        {
            if (transaction.Amount > Balance)
            {
                Console.WriteLine("Insufficient funds");
            }
            else
            {
                base.ApplyTransaction(transaction);
                Console.WriteLine($"Updated balance: ${Balance}");
            }
        }
    }

    // Question 1f: Create a class FinanceApp
    public class FinanceApp
    {
        private List<Transaction> _transactions = new List<Transaction>();

        public void Run()
        {
            // i. Instantiate a SavingsAccount with an account number and initial balance
            var savingsAccount = new SavingsAccount("SA001", 1000);

            // ii. Create three Transaction records with sample values
            var transaction1 = new Transaction(1, DateTime.Now, 50, "Groceries");
            var transaction2 = new Transaction(2, DateTime.Now, 100, "Utilities");
            var transaction3 = new Transaction(3, DateTime.Now, 75, "Entertainment");

            // iii. Use the following processors to process each transaction
            var mobileMoneyProcessor = new MobileMoneyProcessor();
            var bankTransferProcessor = new BankTransferProcessor();
            var cryptoWalletProcessor = new CryptoWalletProcessor();

            mobileMoneyProcessor.Process(transaction1);
            bankTransferProcessor.Process(transaction2);
            cryptoWalletProcessor.Process(transaction3);

            // iv. Apply each transaction to the SavingsAccount using ApplyTransaction
            savingsAccount.ApplyTransaction(transaction1);
            savingsAccount.ApplyTransaction(transaction2);
            savingsAccount.ApplyTransaction(transaction3);

            // v. Add all transactions to _transactions
            _transactions.Add(transaction1);
            _transactions.Add(transaction2);
            _transactions.Add(transaction3);

            Console.WriteLine($"\nFinal account balance: ${savingsAccount.Balance}");
            Console.WriteLine($"Total transactions processed: {_transactions.Count}");
        }
    }
} 