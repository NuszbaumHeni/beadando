using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Intrinsics.Wasm;
using beadando.Model;
using CommunityToolkit.Mvvm.Input;

namespace beadando.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    public MainModel Model;
    public Package Package;
    public FileService FileService;
    public ObservableCollection<Package> Packages { get; set; }
    private Package editPackage;

    private Package? _selectedPackage;
    public Package? SelectedPackage
    {
        get => _selectedPackage;
        set
        {
            _selectedPackage = value;
            OnPropertyChanged(nameof(SelectedPackage));
        }
    }

    public List<Status> Statuses { get; set; }

    public string NewId { get; set; }
    public string NewName { get; set; }
    public DateTime NewSentDate { get; set; }
    public string NewFromCity { get; set; }
    public string NewToCity { get; set; }

    private Status newstatus;
    public Status NewStatus { get { return newstatus; } 
        set { newstatus = value; } }

    public string NewPrice { get; set; }
    public string NewDaysRemaining { get; set; }
    public string ErrorMessage { get; set; }

    public RelayCommand SaveToFileCommand { get; set; }
    public RelayCommand LoadFromFileCommand { get; set; }
    public RelayCommand AddPackageCommand { get; set; }
    public RelayCommand SaveEditCommand { get; set; }
    public RelayCommand<Package> EditPackageCommand { get; set; }

    public MainViewModel(MainModel model)
    {
        Packages = new ObservableCollection<Package>();
        //LoadFromFileCommand = new RelayCommand(LoadFromFile);
        //SaveToFileCommand = new RelayCommand(SaveToFile);
        AddPackageCommand = new RelayCommand(AddPackage);
        SaveEditCommand = new RelayCommand(SaveEdit);
        EditPackageCommand = new RelayCommand<Package>(EditPackage);
        Model = model;
        Statuses = Enum.GetValues<Status>().ToList();
    }
    private void LoadFromFile(string path)
    {
        var loadedPackages = FileService.LoadPackages(path);
        Packages.Clear();
        foreach (var package in loadedPackages)
        {
            Packages.Add(package);
        }
    }
    private void SaveToFile(string path)
    {
        FileService.SavePackages(path, Packages.ToList());
    }
    private void AddPackage()
    {
        if (string.IsNullOrWhiteSpace(NewName) ||
            string.IsNullOrWhiteSpace(NewFromCity) ||
            string.IsNullOrWhiteSpace(NewToCity))
        {
            ShowError("Minden mezőt ki kell tölteni!");
            return;
        }
        if (int.Parse(NewPrice) < 0)
        {
            ShowError("Az ár nem lehet negatív.");
            return;
        }
        if (int.Parse(NewDaysRemaining) < 0)
        {
            ShowError("A hátralévő napok nem lehetnek negatívak.");
            return;
        }
        if (NewStatus == Status.Delivered || NewStatus == Status.Canceled)
        {
            NewDaysRemaining = "0";
            OnPropertyChanged(nameof(NewDaysRemaining));
        }

        var package = new Package
        {
            Name = NewName,
            SentDate = NewSentDate.Date,
            FromCity = NewFromCity,
            ToCity = NewToCity,
            Status = NewStatus,
            Price = int.Parse(NewPrice),
            DaysRemaining = int.Parse(NewDaysRemaining),
            EditPackageCommand = new RelayCommand<Package>(EditPackage)
        };
        Packages.Add(package);
        Model.Addpackage(package);
        NewName = string.Empty;
        NewFromCity = string.Empty;
        NewToCity = string.Empty;
        NewPrice = string.Empty;
        NewDaysRemaining = string.Empty;
        NewStatus = Status.InTransit;
        OnPropertyChanged(nameof(NewName));
        OnPropertyChanged(nameof(NewFromCity));
        OnPropertyChanged(nameof(NewToCity));
        OnPropertyChanged(nameof(NewPrice));
        OnPropertyChanged(nameof(NewDaysRemaining));
        OnPropertyChanged(nameof(NewStatus));
    }

    private void SaveEdit()
    {
        if (SelectedPackage != null)
        {
            if (SelectedPackage.Price < 0)
            {
                ShowError("Az ár nem lehet negatív.");
                return;
            }
            if (SelectedPackage.DaysRemaining < 0)
            {
                ShowError("A hátralévő napok nem lehetnek negatívak.");
                return;
            }
            if (SelectedPackage.Status == Status.Delivered || SelectedPackage.Status == Status.Canceled)
            {
                SelectedPackage.DaysRemaining = 0;
                OnPropertyChanged(nameof(SelectedPackage.DaysRemaining));
            }
            foreach (Package p in Packages)
            {
                if(p.Id == SelectedPackage.Id)
                {
                    p.Name = SelectedPackage.Name;
                    p.SentDate = SelectedPackage.SentDate.Date;
                    p.FromCity = SelectedPackage.FromCity;
                    p.ToCity = SelectedPackage.ToCity;
                    p.Status = SelectedPackage.Status;
                    p.Price = SelectedPackage.Price;
                    p.DaysRemaining = SelectedPackage.DaysRemaining;
                    Model.SaveEdit(SelectedPackage.Id, p);
                }
            }
        }
    }

    private void ShowError(string message)
    {
        ErrorMessage = message;
    }

    public void EditPackage(Package package)
    {
        SelectedPackage = package;
        editPackage = new Package
        {
            Name = package.Name,
            SentDate = package.SentDate.Date,
            FromCity = package.FromCity,
            ToCity = package.ToCity,
            Status = package.Status,
            Price = package.Price,
            DaysRemaining = package.DaysRemaining
        };
    }
}
