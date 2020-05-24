using System;
using System.Collections.Generic;
using System.Text;

namespace Omnicell
{
    interface IMedicineBin
    {
        int BinCapacity { get; }
        bool HasLowInventory { get; }
        int NumberOfEmptySlots { get; }

        List<Medicine> Add(List<Medicine> medicines);

        List<Medicine> Remove(string medicineName, int units);

        void Reset();
    }
}
