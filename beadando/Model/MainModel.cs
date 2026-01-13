using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;

namespace beadando.Model
{
    public class MainModel
    {
        public List<Package> Packages { get; set; }

        public MainModel()
        {
             Packages = new List<Package>();
        }
        public void Addpackage(Package packages) 
        {
            Packages.Add(packages);
        }

        public void SaveEdit(int selectedPackageId, Package editedPackage)
        {
            foreach (Package p in Packages)
            {
                if (p.Id == selectedPackageId)
                {
                    p.Name = editedPackage.Name;
                    p.SentDate = editedPackage.SentDate.Date;
                    p.FromCity = editedPackage.FromCity;
                    p.ToCity = editedPackage.ToCity;
                    p.Status = editedPackage.Status;
                    p.Price = editedPackage.Price;
                    p.DaysRemaining = editedPackage.DaysRemaining;
                }
            }
        }

        
    }

}
