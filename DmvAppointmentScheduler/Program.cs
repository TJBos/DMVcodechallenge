using System;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace DmvAppointmentScheduler
{
    class Program
    {
        public static Random random = new Random();
        public static List<Appointment> appointmentList = new List<Appointment>();
        static void Main(string[] args)
        {
            CustomerList customers = ReadCustomerData();
            TellerList tellers = ReadTellerData();
            Calculation(customers, tellers);
            OutputTotalLengthToConsole();

        }
        private static CustomerList ReadCustomerData()
        {
            string fileName = "CustomerData.json";
            string path = Path.Combine(Environment.CurrentDirectory, @"InputData\", fileName);
            string jsonString = File.ReadAllText(path);
            CustomerList customerData = JsonConvert.DeserializeObject<CustomerList>(jsonString);
            return customerData;

        }
        private static TellerList ReadTellerData()
        {
            string fileName = "TellerData.json";
            string path = Path.Combine(Environment.CurrentDirectory, @"InputData\", fileName);
            string jsonString = File.ReadAllText(path);
            TellerList tellerData = JsonConvert.DeserializeObject<TellerList>(jsonString);
            return tellerData;

        }
        static void Calculation(CustomerList customers, TellerList tellers)
        {
            // Your code goes here .....
            // Re-write this method to be more efficient instead of a assigning all customers to the same teller

            //order tellers by multiplier (asc) and customers by duration (desc)
            List<Teller> tellerList = tellers.Teller.OrderBy(x => x.multiplier).ToList();
            List<Customer> customerList = customers.Customer.OrderByDescending(x => x.duration).ToList();

            //iterate over customers, if type matches assign customer to first teller from list of all matches
            //if no type matches, just assign first teller.
            foreach(Customer customer in customerList) {
                var teller = tellerList.Where(x => x.specialtyType == customer.type).Count() !=0 ?
                                tellerList.Where(x => x.specialtyType == customer.type).First() : tellerList.First();

                var appointment = new Appointment(customer, teller);
                appointmentList.Add(appointment);

                // remove teller from list after assigned to customer, if all tellers used, reset list of tellers and startover with next customer
                tellerList = tellerList.Where(x => x.id != teller.id).ToList();
                if (tellerList.Count() == 0) {  
                    tellerList = tellers.Teller.OrderBy(x => x.multiplier).ToList();
                }
            }
          
        }
        static void OutputTotalLengthToConsole()
        {
            var tellerAppointments =
                from appointment in appointmentList
                group appointment by appointment.teller into tellerGroup
                select new
                {
                    teller = tellerGroup.Key,
                    totalDuration = tellerGroup.Sum(x => x.duration),
                };
            var max = tellerAppointments.OrderBy(i => i.totalDuration).LastOrDefault();
            Console.WriteLine("Teller " + max.teller.id + " will work for " + max.totalDuration + " minutes!");
        }

    }
}
