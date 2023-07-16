using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using SMS.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp_SMS.Models;
using WpfApp_SMS.Views;
using WpfApp_SMS.VM;

namespace WpfApp_SMS
{
    public partial class MainWindowVM: ObservableObject
    {
        [ObservableProperty]
        public ObservableCollection<Student> students;
        [ObservableProperty]
        public Student selectedStudent;

        private string searchText;
        public string SearchText
        {
            get { return searchText; }
            set
            {
                searchText = value;
                OnPropertyChanged(nameof(SearchText));
                FilterStudents();
            }
        }

        private void FilterStudents()
        {
            if (string.IsNullOrEmpty(SearchText))
            {
               LoadData();
            }
            else
            {
                string lowerSearchText = SearchText.ToLower();
                Students = new ObservableCollection<Student>(Students
                    .Where(p => p.StudentID.ToString().Contains(lowerSearchText) ||
                                p.FirstName.ToLower().Contains(lowerSearchText) ||
                                p.LastName.ToLower().Contains(lowerSearchText)));
            }
        }


        [RelayCommand]
        public void AddNewStudent()
        {
            var addwin=new AddWin();
            addwin.Show();
            LoadData();
        }

        [RelayCommand]
        public void Edit()
        {
            var s = new EditWinVM(SelectedStudent);
            var editwin = new EditWin(s);
            editwin.ShowDialog();
            LoadData();
        }

        [RelayCommand]
        public void Delete()
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this Student?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                using (var db = new SMSDbContext())
                {
                    db.Students.Remove(SelectedStudent);
                    db.SaveChanges();
                }
            }
            LoadData();
        }

        //load data from db
        public void LoadData()
        {
            using (var db = new SMSDbContext())
            {
                Students = new ObservableCollection<Student>(db.Students.ToList());
            }
        }

        public MainWindowVM()
        {
            LoadData();
        }
    }
}
