using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Omnicell
{
    /// <summary>
    /// Predefined types of medicine bins with numerical value associated with each to indicate
    /// their medicine capacity
    /// </summary>
    public enum BinTypes
    {
        Small = 5,
        Medium = 10,
        Large = 15
    }

    public class MedicineBin : IMedicineBin
    {
        private int _capacity;
        private decimal _lowInventoryPercentage;
        private List<Medicine> _listOfMedicines;


        public int BinCapacity { get => _capacity; }
        public bool HasLowInventory { get => (_listOfMedicines == null) ? true : (_listOfMedicines.Count / _capacity) < _lowInventoryPercentage; }
        public int NumberOfEmptySlots { get => (_listOfMedicines == null) ? _capacity : (_capacity - _listOfMedicines.Count); }

        public List<Medicine> Inventory { get => _listOfMedicines; }

        #region Class Constructors
        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="capacity">Number of medicines that will fit into this bin</param>
        /// <param name="lowInventoryThreshold">Decimal value indicating low inventory percentage</param>
        public MedicineBin(int capacity, decimal lowInventoryThreshold)
        {
            _capacity = capacity;
            _lowInventoryPercentage = lowInventoryThreshold;

            if (capacity > 0)
            {
                _listOfMedicines = new List<Medicine>(capacity);
            }
        }

        public MedicineBin(int capacity, decimal lowInventoryThreshold, List<Medicine> listOfMedicines)
        {
            _capacity = capacity;
            _lowInventoryPercentage = lowInventoryThreshold;
            _listOfMedicines = listOfMedicines;
        }
        #endregion Class Constructors

        /// <summary>
        /// Clears out the medicine bin
        /// </summary>
        public void Reset()
        {
            _listOfMedicines.Clear();
        }

        /// <summary>
        /// Add the list of medicines into the bin
        /// </summary>
        /// <param name="medicines">List of medicine objects to add</param>
        /// <returns>Array of medicines from the input list that were NOT added based on available space</returns>
        public List<Medicine> Add(List<Medicine> medicines)
        {
            List<Medicine> listOfMedicinesNotAdded = new List<Medicine>();

            if (NumberOfEmptySlots > 0)
            {
                int remainder = medicines.Count - NumberOfEmptySlots > 0 ? medicines.Count - NumberOfEmptySlots : 0;
                int originalNumberOfSlots = NumberOfEmptySlots;

                for (int i = 0; i < originalNumberOfSlots; i++)
                {
                    _listOfMedicines.Add(medicines[i]);
                }

                if (remainder > 0)
                {
                    for (int i = medicines.Count - remainder; i < medicines.Count; i++)
                    {
                        listOfMedicinesNotAdded.Add(medicines[i]);
                    }
                }
            }
            else
            {
                listOfMedicinesNotAdded.AddRange(medicines);
            }

            return listOfMedicinesNotAdded;
        }

        /// <summary>
        /// Remove the specified number of units of medicine from the bin based on the matching name
        /// </summary>
        /// <param name="medicineName">Name of medicine to remove</param>
        /// <param name="units">Number of units to remove</param>
        /// <returns>Array of medicine objects that are removed from the bin</returns>
        public List<Medicine> Remove(string medicineName, int units)
        {
            List<Medicine> listOfMedicinesRemoved = new List<Medicine>();

            if (_listOfMedicines.Exists(medicine => medicine.Name.Equals(medicineName, StringComparison.OrdinalIgnoreCase)))
            {
                List<Medicine> foundMedications = _listOfMedicines.FindAll(medicine => medicine.Name.Equals(medicineName, StringComparison.OrdinalIgnoreCase));
                listOfMedicinesRemoved = foundMedications;

                if (foundMedications.Count <= units)
                {
                    // The number of requested units to remove exceeds the number available in the bin, so remove everything
                    _listOfMedicines.RemoveAll(medicine => medicine.Name.Equals(medicineName, StringComparison.OrdinalIgnoreCase));
                }
                else
                {
                    // The number of requested units to remove is smaller than the total available in the bin
                    int foundIndex;
                    listOfMedicinesRemoved.RemoveRange(0, foundMedications.Count - units - 1);

                    // Remove medicine from the master list too
                    for (int i = 0; i < units; i++)
                    {
                        foundIndex = _listOfMedicines.FindLastIndex(medicine => medicine.Name.Equals(medicineName, StringComparison.OrdinalIgnoreCase));

                        if (foundIndex > -1)
                        {
                            _listOfMedicines.RemoveAt(foundIndex);
                        }
                    }
                }
            }

            return listOfMedicinesRemoved;
        }
    }
}
