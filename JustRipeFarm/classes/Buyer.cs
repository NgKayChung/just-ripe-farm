using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustRipeFarm
{
    class Buyer
    {
        private string firstName;
        private string lastName;
        private string emailAddress;
        private string phoneNumber;
        private int visitedTimes;
        private string avgVisitedTime;
        private decimal totalSpent;
        private string companyName;

        public Buyer()
        {

        }

        public Buyer(string first_name = "", string last_name = "", string email_address = "", string phone_number = "", int visited_times = 0, string avg_visit_time = "", decimal total_spent = 0, string company_name = "")
        {
            firstName = first_name;
            lastName = last_name;
            emailAddress = email_address;
            phoneNumber = phone_number;
            visitedTimes = visited_times;
            avgVisitedTime = avg_visit_time;
            totalSpent = total_spent;
            companyName = company_name;
        }

        public string FirstName { get => firstName; set => firstName = value; }
        public string LastName { get => lastName; set => lastName = value; }
        public string Fullname
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }
        public string EmailAddress { get => emailAddress; set => emailAddress = value; }
        public string PhoneNumber { get => phoneNumber; set => phoneNumber = value; }
        public int VisitedTimes { get => visitedTimes; set => visitedTimes = value; }
        public string AvgVisitedTime { get => avgVisitedTime; set => avgVisitedTime = value; }
        public decimal TotalSpent { get => totalSpent; set => totalSpent = value; }
        public string CompanyName { get => companyName; set => companyName = value; }
    }
}