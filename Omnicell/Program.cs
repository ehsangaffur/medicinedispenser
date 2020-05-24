using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http.Headers;
using System.Reflection.Metadata.Ecma335;

namespace Omnicell
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Creating cabinet.");
            Cabinet cabinet = BuildCabinetBins();

            Console.WriteLine("Creating medicine for small bins.");
            List<Medicine> medicinesForSmallBin = CreateMedicinesForSmallBins();

            Console.WriteLine("Adding medicine for small bins into cabinet.");
            List<Medicine> listOfLeftoverMedicine = cabinet.AddMedicine(medicinesForSmallBin, BinTypes.Small);
            DisplayListOfMedicineInBin(cabinet.SmallBinCollection, "List of medicines in small bins.");
            DisplayListOfLeftoverMedicine(listOfLeftoverMedicine);

            Console.WriteLine("Creating medicine for medium bins.");
            List<Medicine> medicinesForMediumBin = CreateMedicinesForMediumBins();

            Console.WriteLine("Adding medicine for medium bins into cabinet.");
            listOfLeftoverMedicine = cabinet.AddMedicine(medicinesForMediumBin, BinTypes.Medium);
            DisplayListOfMedicineInBin(cabinet.MediumBinCollection, "List of medicines in medium bins.");
            DisplayListOfLeftoverMedicine(listOfLeftoverMedicine);

            Console.WriteLine("Removing 2 units of \"Medicine 4\" from the cabinet.");
            cabinet.RemoveMedicine("Medicine 4", 2);
            DisplayListOfMedicineInBin(cabinet.SmallBinCollection, "List of medicines in small bins.");

            Console.WriteLine("Removing 5 units of \"Medicine 36\" from the cabinet.");
            cabinet.RemoveMedicine("Medicine 36", 2);
            DisplayListOfMedicineInBin(cabinet.MediumBinCollection, "List of medicines in medium bins.");
        }

        private static void DisplayListOfMedicineInBin(List<MedicineBin> listOfBins, string title)
        {
            Console.WriteLine(title);

            for (int i = 0; i < listOfBins.Count; i++)
            {
                foreach (Medicine medicine in listOfBins[i].Inventory)
                {
                    Console.WriteLine(string.Format("Bin {0}: {1} {2}", i + 1, medicine.Id, medicine.Name));
                }
                
            }

            Console.WriteLine();
        }

        private static void DisplayListOfLeftoverMedicine(List<Medicine> listOfLeftoverMedicine)
        {
            if (listOfLeftoverMedicine.Count > 0)
            {
                Console.WriteLine("List of medicines leftover.");

                foreach (Medicine medicine in listOfLeftoverMedicine)
                {
                    Console.WriteLine(string.Format("{0} {1}", medicine.Id, medicine.Name));
                }

                Console.WriteLine();
            }
        }

        private static List<Medicine> CreateMedicinesForSmallBins()
        {
            List<Medicine> listOfMedicines = new List<Medicine>();
            string id, name;

            for (int i = 1; i <= 20; i++)
            {
                id = string.Format("S-{0}", i.ToString().PadLeft(2,'0'));
                name = string.Format("Medicine {0}", i % 5);
                listOfMedicines.Add(new Medicine(id, name));
            }

            return listOfMedicines;
        }

        private static List<Medicine> CreateMedicinesForMediumBins()
        {
            List<Medicine> listOfMedicines = new List<Medicine>();
            string id, name;

            for (int i = 30; i <= 70; i++)
            {
                id = string.Format("M-{0}", i.ToString().PadLeft(2, ' '));
                name = string.Format("Medicine {0}", (i % 7)*9);
                listOfMedicines.Add(new Medicine(id, name));
            }

            return listOfMedicines;
        }

        private static Cabinet BuildCabinetBins()
        {
            // A cabinet with five small bins, one medium and one large bin
            return new Cabinet(5, 1, 1);
        }
    }
}
