using System;
using System.IO;
using DCIT318_Assignment3.Question1;
using DCIT318_Assignment3.Question2;
using DCIT318_Assignment3.Question3;
using DCIT318_Assignment3.Question4;
using DCIT318_Assignment3.Question5;

namespace DCIT318_Assignment3
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== DCIT 318 - PROGRAMMING II - ASSIGNMENT 3 ===\n");
            
            while (true)
            {
                Console.WriteLine("Please select a question to run:");
                Console.WriteLine("1. Finance Management System");
                Console.WriteLine("2. Healthcare System");
                Console.WriteLine("3. Warehouse Inventory Management System");
                Console.WriteLine("4. School Grading System");
                Console.WriteLine("5. Inventory Records System");
                Console.WriteLine("6. Run All Questions");
                Console.WriteLine("0. Exit");
                Console.Write("\nEnter your choice (0-6): ");
                
                var choice = Console.ReadLine();
                Console.WriteLine();
                
                switch (choice)
                {
                    case "1":
                        RunQuestion1();
                        break;
                    case "2":
                        RunQuestion2();
                        break;
                    case "3":
                        RunQuestion3();
                        break;
                    case "4":
                        RunQuestion4();
                        break;
                    case "5":
                        RunQuestion5();
                        break;
                    case "6":
                        RunAllQuestions();
                        break;
                    case "0":
                        Console.WriteLine("Goodbye!");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.\n");
                        break;
                }
                
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                Console.Clear();
            }
        }

        static void RunQuestion1()
        {
            Console.WriteLine("=== QUESTION 1: FINANCE MANAGEMENT SYSTEM ===\n");
            try
            {
                var financeApp = new FinanceApp();
                financeApp.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error running Question 1: {ex.Message}");
            }
        }

        static void RunQuestion2()
        {
            Console.WriteLine("=== QUESTION 2: HEALTHCARE SYSTEM ===\n");
            try
            {
                var healthApp = new HealthSystemApp();
                healthApp.SeedData();
                healthApp.BuildPrescriptionMap();
                healthApp.PrintAllPatients();
                
                // Display prescriptions for a specific patient
                Console.Write("Enter a Patient ID to view prescriptions (1-3): ");
                if (int.TryParse(Console.ReadLine(), out int patientId))
                {
                    healthApp.PrintPrescriptionsForPatient(patientId);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error running Question 2: {ex.Message}");
            }
        }

        static void RunQuestion3()
        {
            Console.WriteLine("=== QUESTION 3: WAREHOUSE INVENTORY MANAGEMENT SYSTEM ===\n");
            try
            {
                var warehouseManager = new WareHouseManager();
                warehouseManager.SeedData();
                
                // Print all items
                warehouseManager.PrintAllItems(warehouseManager.GetType().GetField("_groceries", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(warehouseManager) as InventoryRepository<GroceryItem>);
                warehouseManager.PrintAllItems(warehouseManager.GetType().GetField("_electronics", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(warehouseManager) as InventoryRepository<ElectronicItem>);
                
                // Test exception handling
                Console.WriteLine("=== Testing Exception Handling ===");
                
                // Try to add a duplicate item
                try
                {
                    var duplicateItem = new ElectronicItem(1, "Duplicate Laptop", 5, "Dell", 24);
                    warehouseManager.GetType().GetField("_electronics", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(warehouseManager) as InventoryRepository<ElectronicItem>?.AddItem(duplicateItem);
                }
                catch (DuplicateItemException ex)
                {
                    Console.WriteLine($"Caught DuplicateItemException: {ex.Message}");
                }
                
                // Try to remove a non-existent item
                try
                {
                    warehouseManager.GetType().GetField("_electronics", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(warehouseManager) as InventoryRepository<ElectronicItem>?.RemoveItem(999);
                }
                catch (ItemNotFoundException ex)
                {
                    Console.WriteLine($"Caught ItemNotFoundException: {ex.Message}");
                }
                
                // Try to update with invalid quantity
                try
                {
                    warehouseManager.GetType().GetField("_electronics", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(warehouseManager) as InventoryRepository<ElectronicItem>?.UpdateQuantity(1, -5);
                }
                catch (InvalidQuantityException ex)
                {
                    Console.WriteLine($"Caught InvalidQuantityException: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error running Question 3: {ex.Message}");
            }
        }

        static void RunQuestion4()
        {
            Console.WriteLine("=== QUESTION 4: SCHOOL GRADING SYSTEM ===\n");
            try
            {
                // Create sample input file
                var inputFilePath = "students_input.txt";
                var outputFilePath = "students_report.txt";
                
                // Create sample data
                var sampleData = new[]
                {
                    "101,Alice Smith,84",
                    "102,Bob Johnson,72",
                    "103,Carol Davis,95",
                    "104,David Wilson,58",
                    "105,Eva Brown,67",
                    "106,Frank Miller,91",
                    "107,Grace Lee,45",
                    "108,Henry Taylor,78"
                };
                
                File.WriteAllLines(inputFilePath, sampleData);
                Console.WriteLine($"Created sample input file: {inputFilePath}");
                
                var processor = new StudentResultProcessor();
                var students = processor.ReadStudentsFromFile(inputFilePath);
                
                if (students.Count > 0)
                {
                    processor.WriteReportToFile(students, outputFilePath);
                    Console.WriteLine($"Successfully processed {students.Count} students");
                    Console.WriteLine($"Report written to: {outputFilePath}");
                    
                    // Display first few students
                    Console.WriteLine("\nFirst 3 students processed:");
                    for (int i = 0; i < Math.Min(3, students.Count); i++)
                    {
                        var student = students[i];
                        Console.WriteLine($"{student.FullName} (ID: {student.Id}): Score = {student.Score}, Grade = {student.GetGrade()}");
                    }
                }
                else
                {
                    Console.WriteLine("No students were processed successfully.");
                }
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"FileNotFoundException: {ex.Message}");
            }
            catch (InvalidScoreFormatException ex)
            {
                Console.WriteLine($"InvalidScoreFormatException: {ex.Message}");
            }
            catch (MissingFieldException ex)
            {
                Console.WriteLine($"MissingFieldException: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
        }

        static void RunQuestion5()
        {
            Console.WriteLine("=== QUESTION 5: INVENTORY RECORDS SYSTEM ===\n");
            try
            {
                var inventoryApp = new InventoryApp();
                
                // Seed sample data
                inventoryApp.SeedSampleData();
                
                // Save data to file
                inventoryApp.SaveData();
                
                // Clear memory and simulate a new session
                inventoryApp.ClearMemory();
                Console.WriteLine("Memory cleared. Starting new session...\n");
                
                // Load data from file
                inventoryApp.LoadData();
                
                // Print all items to confirm data was recovered
                inventoryApp.PrintAllItems();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error running Question 5: {ex.Message}");
            }
        }

        static void RunAllQuestions()
        {
            Console.WriteLine("=== RUNNING ALL QUESTIONS ===\n");
            
            RunQuestion1();
            Console.WriteLine("\n" + new string('=', 50) + "\n");
            
            RunQuestion2();
            Console.WriteLine("\n" + new string('=', 50) + "\n");
            
            RunQuestion3();
            Console.WriteLine("\n" + new string('=', 50) + "\n");
            
            RunQuestion4();
            Console.WriteLine("\n" + new string('=', 50) + "\n");
            
            RunQuestion5();
            
            Console.WriteLine("\n=== ALL QUESTIONS COMPLETED ===");
        }
    }
} 