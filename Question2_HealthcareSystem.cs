using System;
using System.Collections.Generic;
using System.Linq;

namespace DCIT318_Assignment3.Question2
{
    // Question 2a: Create a generic class Repository<T> to handle entity storage and retrieval
    public class Repository<T>
    {
        private List<T> items = new List<T>();

        public void Add(T item)
        {
            items.Add(item);
        }

        public List<T> GetAll()
        {
            return items.ToList();
        }

        public T? GetById(Func<T, bool> predicate)
        {
            return items.FirstOrDefault(predicate);
        }

        public bool Remove(Func<T, bool> predicate)
        {
            var item = items.FirstOrDefault(predicate);
            if (item != null)
            {
                return items.Remove(item);
            }
            return false;
        }
    }

    // Question 2b: Patient class
    public class Patient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }

        public Patient(int id, string name, int age, string gender)
        {
            Id = id;
            Name = name;
            Age = age;
            Gender = gender;
        }
    }

    // Question 2c: Prescription class
    public class Prescription
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public string MedicationName { get; set; }
        public DateTime DateIssued { get; set; }

        public Prescription(int id, int patientId, string medicationName, DateTime dateIssued)
        {
            Id = id;
            PatientId = patientId;
            MedicationName = medicationName;
            DateIssued = dateIssued;
        }
    }

    // Question 2g: Create a HealthSystemApp class
    public class HealthSystemApp
    {
        private Repository<Patient> _patientRepo;
        private Repository<Prescription> _prescriptionRepo;
        private Dictionary<int, List<Prescription>> _prescriptionMap;

        public HealthSystemApp()
        {
            _patientRepo = new Repository<Patient>();
            _prescriptionRepo = new Repository<Prescription>();
            _prescriptionMap = new Dictionary<int, List<Prescription>>();
        }

        // Question 2g: SeedData method
        public void SeedData()
        {
            // Add 2-3 Patient objects to the patient repository
            _patientRepo.Add(new Patient(1, "John Doe", 35, "Male"));
            _patientRepo.Add(new Patient(2, "Jane Smith", 28, "Female"));
            _patientRepo.Add(new Patient(3, "Bob Johnson", 45, "Male"));

            // Add 4-5 Prescription objects to the prescription repository
            _prescriptionRepo.Add(new Prescription(1, 1, "Aspirin", DateTime.Now.AddDays(-5)));
            _prescriptionRepo.Add(new Prescription(2, 1, "Ibuprofen", DateTime.Now.AddDays(-3)));
            _prescriptionRepo.Add(new Prescription(3, 2, "Vitamin D", DateTime.Now.AddDays(-7)));
            _prescriptionRepo.Add(new Prescription(4, 2, "Calcium", DateTime.Now.AddDays(-2)));
            _prescriptionRepo.Add(new Prescription(5, 3, "Metformin", DateTime.Now.AddDays(-1)));
        }

        // Question 2g: BuildPrescriptionMap method
        public void BuildPrescriptionMap()
        {
            var allPrescriptions = _prescriptionRepo.GetAll();
            
            foreach (var prescription in allPrescriptions)
            {
                if (!_prescriptionMap.ContainsKey(prescription.PatientId))
                {
                    _prescriptionMap[prescription.PatientId] = new List<Prescription>();
                }
                _prescriptionMap[prescription.PatientId].Add(prescription);
            }
        }

        // Question 2g: PrintAllPatients method
        public void PrintAllPatients()
        {
            Console.WriteLine("=== All Patients ===");
            var patients = _patientRepo.GetAll();
            foreach (var patient in patients)
            {
                Console.WriteLine($"ID: {patient.Id}, Name: {patient.Name}, Age: {patient.Age}, Gender: {patient.Gender}");
            }
            Console.WriteLine();
        }

        // Question 2g: PrintPrescriptionsForPatient method
        public void PrintPrescriptionsForPatient(int id)
        {
            Console.WriteLine($"=== Prescriptions for Patient ID: {id} ===");
            
            if (_prescriptionMap.ContainsKey(id))
            {
                var prescriptions = _prescriptionMap[id];
                foreach (var prescription in prescriptions)
                {
                    Console.WriteLine($"Medication: {prescription.MedicationName}, Date Issued: {prescription.DateIssued:yyyy-MM-dd}");
                }
            }
            else
            {
                Console.WriteLine("No prescriptions found for this patient.");
            }
            Console.WriteLine();
        }

        // Question 2f: GetPrescriptionsByPatientId method
        public List<Prescription> GetPrescriptionsByPatientId(int patientId)
        {
            if (_prescriptionMap.ContainsKey(patientId))
            {
                return _prescriptionMap[patientId];
            }
            return new List<Prescription>();
        }
    }
} 