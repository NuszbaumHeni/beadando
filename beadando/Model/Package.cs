using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;

namespace beadando.Model
{
    public enum Status
    {
        InTransit,
        Delivered,
        Pending,
        Canceled
    }
    public class Package
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime SentDate { get; set; }
        public string FromCity { get; set; }
        public string ToCity { get; set; }
        public Status Status { get; set; }
        public int Price { get; set; }
        public int DaysRemaining { get; set; }
        public RelayCommand<Package> EditPackageCommand { get; set; }
    }
}
