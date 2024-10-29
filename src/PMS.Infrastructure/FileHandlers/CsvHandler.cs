// The logic of parsing a csv file
using System;
using System.Collections.Generic;
using System.IO;
using PMS.Infrastructure.Interfaces;

namespace PMS.Infrastructure.FileHandlers
{    
    public class CsvHandler : ICsvHandler
    {
        public List<string> GetCsv(string filepath){                        
            var lines = new List<string>();            
            try
            {
                // Reading file
                using (var reader = new StreamReader(filepath))
                {
                    while (!reader.EndOfStream)
                    {         
                        // Adds each line a list of lines              
                        var line = reader.ReadLine();
                        lines.Add(line);  
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading file: {ex.Message}");
            }
            return lines;            
        }
    }
}