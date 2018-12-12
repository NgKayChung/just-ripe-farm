using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustRipeFarm
{
    class StockStorage
    {
        private string storageID;
        private int totalCapacity;
        private int usedCapacity;
        private string status;
        private List<Stock> stocks;

        public StockStorage()
        {

        }
        public StockStorage(string storage_id, int total_capacity, int used_capacity, string status)
        {
            storageID = storage_id;
            totalCapacity = total_capacity;
            usedCapacity = used_capacity;
            this.status = status;
        }

        public string StorageID { get => storageID; set => storageID = value; }
        public int TotalCapacity { get => totalCapacity; set => totalCapacity = value; }
        public int UsedCapacity { get => usedCapacity; set => usedCapacity = value; }
        public string Status { get => status; set => status = value; }
        public List<Stock> Stocks { get => stocks; set => stocks = value; }

    }
}