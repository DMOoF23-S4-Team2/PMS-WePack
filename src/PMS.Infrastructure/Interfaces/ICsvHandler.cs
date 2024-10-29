using System;

namespace PMS.Infrastructure.Interfaces
{
    public interface ICsvHandler
    {
        List<string> GetCsv(string filepath);
    }
}