using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI2.Demo.Models
{
    public static class PatientDB
    {
        public static IMongoCollection<Patient> Open() {

            var client = new MongoClient("mongodb://localhost");
            var database = client.GetDatabase("PatientDB");
            return database.GetCollection<Patient>("Patients");
        }
    }
}