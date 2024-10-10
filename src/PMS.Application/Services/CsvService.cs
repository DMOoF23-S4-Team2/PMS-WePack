using System;
using PMS.Application.Interfaces;
using PMS.Infrastructure.FileHandlers;

namespace PMS.Application.Services
{
    public class CsvService : ICsvService
    {
        private readonly CsvHandler _csvHandler;

        public CsvService()
        {
            // Intialize CsvHandler from Infrastructure layer
            _csvHandler = new CsvHandler();            
        }

        public void ReadCsv(string filepath)
        {
            // The data is getting read and returned by the infrastructure layer
            var csvData = _csvHandler.GetCsv(filepath);
            System.Console.WriteLine("Reading csv");
            // DEV
            foreach (var line in csvData)
            {
                Console.WriteLine(line); 
            }
        }
    }
}
