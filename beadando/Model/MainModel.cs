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

        public void SaveEdit(Package selectedPackage, Package editedPackage)
        {
            var index = Packages.IndexOf(selectedPackage);
            if (index != -1)
            {
                Packages[index] = editedPackage;
            }
        }
    }

}
