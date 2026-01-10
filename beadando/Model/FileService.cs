using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beadando.Model
{
    public class FileService
    {
        public List<Package> LoadPackages(string path)
        {
            using (StreamReader reader = new StreamReader(path))
            {
                try
                {
                    List<Package> packages = new List<Package>();
                    string line = reader.ReadLine();
                    int numPackages = int.Parse(line);
                    for (int i = 0; i < numPackages; i++)
                    {
                        line = reader.ReadLine();
                        string[] parts = line.Split(';');
                        Package p = new Package
                        {
                            Id = int.Parse(parts[0]),
                            Name = parts[1],
                            SentDate = DateTime.Parse(parts[2]),
                            FromCity = parts[3],
                            ToCity = parts[4],
                            Status = (Status)Enum.Parse(typeof(Status), parts[5]),
                            Price = int.Parse(parts[6]),
                            DaysRemaining = int.Parse(parts[7])
                        };
                        packages.Add(p);
                    }
                    return packages;
                }
                catch (Exception ex)
                {

                }
            }
            return new List<Package>();
        }
        public async Task SavePackages(string path, List<Package> packages)
        {
            using (StreamWriter writer = new StreamWriter("packages.txt"))
            {
                try
                {
                    string num = packages.Count.ToString();
                    await writer.WriteLineAsync(num);
                    for (int i = 0; i < packages.Count; i++)
                    {
                        Package p = packages[i];
                        string line = $"Id: {p.Id};" +
                            $"Name: {p.Name};" +
                            $"SentDate: {p.SentDate};" +
                            $"FromCity: {p.FromCity};" +
                            $"ToCity: {p.ToCity};" +
                            $"Status: {p.Status};" +
                            $"Price: {p.Price};" +
                            $"DaysRemaining: {p.DaysRemaining}";
                        await writer.WriteLineAsync(line);
                    }
                }
                catch (Exception ex) 
                {

                }
            }
        }
    }
}
