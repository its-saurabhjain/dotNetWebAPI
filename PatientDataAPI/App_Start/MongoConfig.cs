using PatientDataAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Driver.Linq;
using MongoDB.Driver;

namespace PatientDataAPI
{
    public static class MongoConfig
    {
        public static void Seed()
        {

            var patients = PatientDB.Open();

            if (!patients.Find<Patient>(i => i.Name == "Scott").Any())
            {
                var data = new List<Patient>()
                {
                    new Patient { Name = "Scott",
                                 Ailments = new List<Ailment>(){ new Ailment() {  Name = "Crazy1"} },
                                 Medications = new List<Medication>() { new Medication() {  Name = "Med1", Doses=3} }
                    },
                    new Patient { Name = "Lez",
                                 Ailments = new List<Ailment>(){ new Ailment() {  Name = "Crazy2"} },
                                 Medications = new List<Medication>() { new Medication() {  Name = "Med2", Doses=3} }
                    },
                    new Patient { Name = "Steve",
                                 Ailments = new List<Ailment>(){ new Ailment() {  Name = "Crazy3"} },
                                 Medications = new List<Medication>() { new Medication() {  Name = "Med3", Doses=3} }
                    }
                };
                patients.InsertMany(data);

            }


        }
    }
}
