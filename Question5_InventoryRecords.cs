using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace DCIT318_Assignment3.Question5
{
    // Question 5a: Define an immutable inventory record using C# record
    public record InventoryItem(int Id, string Name, int Quantity, DateTime DateAdded);

    // Question 5b: Create a marker interface for logging
    public interface IInventoryEntity
    {
        int Id { get; }
    }

    // Question 5c: Implement a generic inventory logger
    public class InventoryLogger<T> where T : IInventoryEntity
    {
        private List<T> _log;
        private string _filePath;

        public InventoryLogger(string filePath)
        {
            _log = new List<T>();
            _filePath = filePath;
        }

        public void Add(T item)
        {
            _log.Add(item);
        }

        public List<T> GetAll()
        {
            return _log.ToList();
        }

        public void SaveToFile()
        {
            try
            {
                using (var writer = new StreamWriter(_filePath))
                {
                    var json = JsonConvert.SerializeObject(_log, Formatting.Indented);
                    writer.Write(json);
                }
                Console.WriteLine($"Successfully saved {_log.Count} items to {_filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving to file: {ex.Message}");
            }
        }

        public void LoadFromFile()
        {
            try
            {
                if (File.Exists(_filePath))
                {
                    using (var reader = new StreamReader(_filePath))
                    {
                        var json = reader.ReadToEnd();
                        _log = JsonConvert.DeserializeObject<List<T>>(json) ?? new List<T>();
                    }
                    Console.WriteLine($"Successfully loaded {_log.Count} items from {_filePath}");
                }
                else
                {
                    Console.WriteLine($"File {_filePath} not found. Starting with empty inventory.");
                    _log = new List<T>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading from file: {ex.Message}");
                _log = new List<T>();
            }
        }

        public void Clear()
        {
            _log.Clear();
        }
    }

    // Question 5f: Create a class InventoryApp
    public class InventoryApp
    {
        private InventoryLogger<InventoryItem> _logger;

        public InventoryApp()
        {
            _logger = new InventoryLogger<InventoryItem>("inventory.json");
        }

        public void SeedSampleData()
        {
            // Add at least 3-5 items to _logger using the Add() method
            _logger.Add(new InventoryItem(1, "Laptop", 15, DateTime.Now.AddDays(-30)));
            _logger.Add(new InventoryItem(2, "Mouse", 50, DateTime.Now.AddDays(-25)));
            _logger.Add(new InventoryItem(3, "Keyboard", 30, DateTime.Now.AddDays(-20)));
            _logger.Add(new InventoryItem(4, "Monitor", 20, DateTime.Now.AddDays(-15)));
            _logger.Add(new InventoryItem(5, "Headphones", 40, DateTime.Now.AddDays(-10)));
            
            Console.WriteLine("Sample data seeded successfully.");
        }

        public void SaveData()
        {
            _logger.SaveToFile();
        }

        public void LoadData()
        {
            _logger.LoadFromFile();
        }

        public void PrintAllItems()
        {
            var items = _logger.GetAll();
            Console.WriteLine("=== INVENTORY ITEMS ===");
            
            if (items.Count == 0)
            {
                Console.WriteLine("No items found in inventory.");
            }
            else
            {
                foreach (var item in items)
                {
                    Console.WriteLine($"ID: {item.Id}, Name: {item.Name}, Quantity: {item.Quantity}, Date Added: {item.DateAdded:yyyy-MM-dd}");
                }
            }
            Console.WriteLine();
        }

        public void ClearMemory()
        {
            _logger.Clear();
            Console.WriteLine("Memory cleared. Inventory is now empty.");
        }
    }
} 