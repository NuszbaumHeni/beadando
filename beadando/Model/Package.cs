using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        private static int _idCounter = 1;
        public Package()
        {
            Id = _idCounter++;
        }
        public int Id { get; }
        public string Name { get; set; }
        public DateTime SentDate { get; set; }
        public string FromCity { get; set; }
        public string ToCity { get; set; }
        private Status status;
        
        public Status Status
        {
            get
            {
                return status;
            }
            set
            {
                status = value;
                if (value == Status.Delivered || value == Status.Canceled)
                {
                    DaysRemaining = 0;
                }
            }
        }
        private int price;
        public int Price {
            get { return price; }
            set
            {   
                price = value;
                if (value < 0)
                {
                    throw new ArgumentException("Az ár nem lehet negatív.");
                }

            }
        }
        private int daysRemaining;
        public int DaysRemaining 
        {
            get { return daysRemaining; }
            set 
            {
                daysRemaining = value;
                if(Status == Status.Delivered || Status == Status.Canceled)
                {
                    daysRemaining = 0;
                }
                else if (value < 0)
                {
                    throw new ArgumentException("A hátralévő napok nem lehetnek negatívak.");
                }
            }
        }
        public RelayCommand<Package> EditPackageCommand { get; set; }
    }
}
