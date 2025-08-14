using System;
using System.Collections.Generic;
using System.IO;
using System.Linq; // Added for OrderByDescending

namespace DCIT318_Assignment3.Question4
{
    // Question 4a: Create a Student Class
    public class Student
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public int Score { get; set; }

        public Student(int id, string fullName, int score)
        {
            Id = id;
            FullName = fullName;
            Score = score;
        }

        public string GetGrade()
        {
            if (Score >= 80 && Score <= 100)
                return "A";
            else if (Score >= 70 && Score <= 79)
                return "B";
            else if (Score >= 60 && Score <= 69)
                return "C";
            else if (Score >= 50 && Score <= 59)
                return "D";
            else
                return "F";
        }
    }

    // Question 4b: Define Two Custom Exception Classes
    public class InvalidScoreFormatException : Exception
    {
        public InvalidScoreFormatException(string message) : base(message) { }
    }

    public class MissingFieldException : Exception
    {
        public MissingFieldException(string message) : base(message) { }
    }

    // Question 4d: Create a StudentResultProcessor Class
    public class StudentResultProcessor
    {
        public List<Student> ReadStudentsFromFile(string inputFilePath)
        {
            var students = new List<Student>();
            
            using (var reader = new StreamReader(inputFilePath))
            {
                string? line;
                int lineNumber = 0;
                
                while ((line = reader.ReadLine()) != null)
                {
                    lineNumber++;
                    
                    try
                    {
                        // Split each line by comma and validate the number of fields
                        var fields = line.Split(',');
                        
                        if (fields.Length != 3)
                        {
                            throw new MissingFieldException($"Line {lineNumber}: Expected 3 fields, found {fields.Length}");
                        }

                        // Try converting the score to an integer
                        if (!int.TryParse(fields[2].Trim(), out int score))
                        {
                            throw new InvalidScoreFormatException($"Line {lineNumber}: Invalid score format '{fields[2]}'");
                        }

                        // Validate score range
                        if (score < 0 || score > 100)
                        {
                            throw new InvalidScoreFormatException($"Line {lineNumber}: Score {score} is out of range (0-100)");
                        }

                        var student = new Student(
                            int.Parse(fields[0].Trim()),
                            fields[1].Trim(),
                            score
                        );
                        
                        students.Add(student);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error processing line {lineNumber}: {ex.Message}");
                        // Continue processing other lines
                    }
                }
            }
            
            return students;
        }

        public void WriteReportToFile(List<Student> students, string outputFilePath)
        {
            using (var writer = new StreamWriter(outputFilePath))
            {
                writer.WriteLine("=== STUDENT GRADE REPORT ===");
                writer.WriteLine($"Generated on: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                writer.WriteLine($"Total Students: {students.Count}");
                writer.WriteLine();
                
                foreach (var student in students)
                {
                    var grade = student.GetGrade();
                    writer.WriteLine($"{student.FullName} (ID: {student.Id}): Score = {student.Score}, Grade = {grade}");
                }
                
                writer.WriteLine();
                writer.WriteLine("=== GRADE SUMMARY ===");
                var gradeCounts = new Dictionary<string, int>();
                foreach (var student in students)
                {
                    var grade = student.GetGrade();
                    if (gradeCounts.ContainsKey(grade))
                        gradeCounts[grade]++;
                    else
                        gradeCounts[grade] = 1;
                }
                
                foreach (var grade in gradeCounts.OrderByDescending(x => x.Key))
                {
                    writer.WriteLine($"Grade {grade.Key}: {grade.Value} students");
                }
            }
        }
    }
} 