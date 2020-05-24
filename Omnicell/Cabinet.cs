using System;
using System.Collections.Generic;
using System.Text;

namespace Omnicell
{
    public class Cabinet
    {
        private List<MedicineBin> _smallBinCollection;  // 1 slot
        private List<MedicineBin> _mediumBinCollection; // 2 slots
        private List<MedicineBin> _largeBinCollection;  // 3 slots
        private delegate List<Medicine> AddMedicineToBinDelegate(List<Medicine> listOfMedicines);

        public Cabinet(int numberOfSmallBins, int numberOfMediumBins, int numberOfLargeBins)
        {
            _smallBinCollection = new List<MedicineBin>(numberOfSmallBins);
            for (int i = 0; i < numberOfSmallBins; i++)
            {
                _smallBinCollection.Add(new MedicineBin((int)BinTypes.Small, 0.2m));
            }

            _mediumBinCollection = new List<MedicineBin>(numberOfMediumBins);
            for (int i = 0; i < numberOfMediumBins; i++)
            {
                _mediumBinCollection.Add(new MedicineBin((int)BinTypes.Medium, 0.2m));
            }

            _largeBinCollection = new List<MedicineBin>(numberOfLargeBins);
            for (int i = 0; i < numberOfLargeBins; i++)
            {
                _largeBinCollection.Add(new MedicineBin((int)BinTypes.Large, 0.2m));
            }
        }

        public List<MedicineBin> SmallBinCollection { get => _smallBinCollection; }
        public List<MedicineBin> MediumBinCollection { get => _mediumBinCollection; }
        public List<MedicineBin> LargeBinCollection { get => _largeBinCollection; }

        /// <summary>
        /// Add list of medicines to the specified bin(s) of the cabinet
        /// </summary>
        /// <param name="medicines">List of medicine objects to add</param>
        /// <param name="binType">Enumerated bin-type (Small, Medium, Large)</param>
        /// <returns>List of medicine objects that could not be added because of lack of space</returns>
        public List<Medicine> AddMedicine(List<Medicine> medicines, BinTypes binType)
        {
            AddMedicineToBinDelegate addMedicineOperation;
            List<Medicine> overflowMedication = new List<Medicine>();

            switch (binType)
            {
                case BinTypes.Small:
                    addMedicineOperation = new AddMedicineToBinDelegate(AddMedicineToSmallBins);
                    break;
                case BinTypes.Medium:
                    addMedicineOperation = new AddMedicineToBinDelegate(AddMedicineToMediumBins);
                    break;
                case BinTypes.Large:
                default:
                    addMedicineOperation = new AddMedicineToBinDelegate(AddMedicineToLargeBins);
                    break;
            }

            return addMedicineOperation(medicines);
        }

        /// <summary>
        /// Remove number of units of medicine with specified name from bin(s)
        /// </summary>
        /// <param name="name">Name of medicine to remove from bin(s)</param>
        /// <param name="units">Number of units of medicine to remove</param>
        /// <returns>List of medicine objects whose name matches specified name</returns>
        public List<Medicine> RemoveMedicine(string name, int units)
        {
            List<Medicine> listOfRemovedMedicine = new List<Medicine>();

            // Try to get the medicine with the matching name from the set of small bins first
            for (int i = 0; i < _smallBinCollection.Count && listOfRemovedMedicine.Count < units; i++)
            {
                listOfRemovedMedicine.AddRange(_smallBinCollection[i].Remove(name, units));
            }

            // If not all the units removed then try the medium bins next
            for (int i = 0; i < _mediumBinCollection.Count && listOfRemovedMedicine.Count < units; i++)
            {
                listOfRemovedMedicine.AddRange(_mediumBinCollection[i].Remove(name, units - listOfRemovedMedicine.Count));
            }

            // If still not all the units removed then try the large bins last
            for (int i = 0; i < _largeBinCollection.Count && listOfRemovedMedicine.Count < units; i++)
            {
                listOfRemovedMedicine.AddRange(_largeBinCollection[i].Remove(name, units - listOfRemovedMedicine.Count));
            }

            return listOfRemovedMedicine;
        }

        public void Reset(BinTypes binType)
        {
            switch (binType)
            {
                case BinTypes.Small:
                    foreach (MedicineBin smallBin in _smallBinCollection)
                    {
                        smallBin.Reset();
                    }
                    break;
                case BinTypes.Medium:
                    foreach (MedicineBin mediumBin in _mediumBinCollection)
                    {
                        mediumBin.Reset();
                    }
                    break;
                case BinTypes.Large:
                default:
                    foreach (MedicineBin largeBin in _largeBinCollection)
                    {
                        largeBin.Reset();
                    }
                    break;
            }
        }

        #region Private Methods
        private List<Medicine> AddMedicineToSmallBins(List<Medicine> medicine)
        {
            List<Medicine> remainingMedication = new List<Medicine>(medicine);

            // For each small bin within the cabinet...
            for (int i = 0; i < _smallBinCollection.Capacity && remainingMedication.Count > 0; i++)
            {
                if (remainingMedication.Count > 0 && _smallBinCollection[i].NumberOfEmptySlots > 0)
                {
                    remainingMedication = _smallBinCollection[i].Add(remainingMedication);
                }
            }

            return remainingMedication;
        }

        private List<Medicine> AddMedicineToMediumBins(List<Medicine> medicine)
        {
            List<Medicine> remainingMedication = new List<Medicine>(medicine);

            // For each small bin within the cabinet...
            for (int i = 0; i < _mediumBinCollection.Capacity && remainingMedication.Count > 0; i++)
            {
                if (remainingMedication.Count > 0 && _mediumBinCollection[i].NumberOfEmptySlots > 0)
                {
                    remainingMedication = _mediumBinCollection[i].Add(remainingMedication);
                }
            }

            return remainingMedication;
        }

        private List<Medicine> AddMedicineToLargeBins(List<Medicine> medicine)
        {
            List<Medicine> remainingMedication = new List<Medicine>(medicine);

            // For each small bin within the cabinet...
            for (int i = 0; i < _largeBinCollection.Capacity && remainingMedication.Count > 0; i++)
            {
                if (remainingMedication.Count > 0 && _largeBinCollection[i].NumberOfEmptySlots > 0)
                {
                    remainingMedication = _largeBinCollection[i].Add(remainingMedication);
                }
            }

            return remainingMedication;
        }
        #endregion Private Methods
    }
}
