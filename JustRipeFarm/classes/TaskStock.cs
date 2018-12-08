using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustRipeFarm
{
    class TaskStock
    {
        private int taskID;
        private Stock stock;
        private int quantityUse;

        public TaskStock()
        {

        }

        public TaskStock(int task_id, Stock stock, int quantity)
        {
            taskID = task_id;
            this.stock = stock;
            quantityUse = quantity;
        }

        public TaskStock(Stock stock, int quantity)
        {
            this.stock = stock;
            QuantityUse = quantity;
        }

        public int TaskID { get => taskID; set => taskID = value; }
        public Stock Stock { get => stock; set => stock = value; }
        public int QuantityUse { get => quantityUse; set => quantityUse = value; }
    }
}