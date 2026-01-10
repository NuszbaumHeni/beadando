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
    public Status NewStatus { get; set; }
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
        if (string.IsNullOrWhiteSpace(NewId) ||
            string.IsNullOrWhiteSpace(NewName) ||
            string.IsNullOrWhiteSpace(NewFromCity) ||
            string.IsNullOrWhiteSpace(NewToCity))
        {
            ShowError("Minden mezőt ki kell tölteni!");
            return;
        }
        if (int.Parse(NewPrice) <= 0)
        {
            ShowError("Az ár nem lehet 0 vagy negatív.");
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
        }

        var package = new Package
        {
            Id = int.Parse(NewId),
            Name = NewName,
            SentDate = NewSentDate,
            FromCity = NewFromCity,
            ToCity = NewToCity,
            Status = NewStatus,
            Price = int.Parse(NewPrice),
            DaysRemaining = int.Parse(NewDaysRemaining),
            EditPackageCommand = new RelayCommand<Package>(EditPackage)
        };
        Packages.Add(package);
    }

    private void SaveEdit()
    {
        if (SelectedPackage != null)
        {
            SelectedPackage.Id = int.Parse(NewId);
            SelectedPackage.Name = NewName;
            SelectedPackage.SentDate = NewSentDate;
            SelectedPackage.FromCity = NewFromCity;
            SelectedPackage.ToCity = NewToCity;
            SelectedPackage.Status = NewStatus;
            SelectedPackage.Price = int.Parse(NewPrice);
            SelectedPackage.DaysRemaining = int.Parse(NewDaysRemaining);
        }
    }

    private void ShowError(string message)
    {
        ErrorMessage = message;
    }

    public void EditPackage(Package package)
    {
        SelectedPackage = package;
        if (SelectedPackage != null)
        {
            NewId = SelectedPackage.Id.ToString();
            NewName = SelectedPackage.Name;
            NewSentDate = SelectedPackage.SentDate;
            NewFromCity = SelectedPackage.FromCity;
            NewToCity = SelectedPackage.ToCity;
            NewStatus = SelectedPackage.Status;
            NewPrice = SelectedPackage.Price.ToString();
            NewDaysRemaining = SelectedPackage.DaysRemaining.ToString();
        }
    }
}
