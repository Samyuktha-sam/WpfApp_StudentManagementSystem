using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using SMS.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using WpfApp_SMS.Models;
using WpfApp_SMS.Views;

namespace WpfApp_SMS.VM
{
    public partial class AddWinVM : ObservableObject
    {
        [ObservableProperty]
        public string firstName;
        [ObservableProperty]
        public string lastName;
        [ObservableProperty]
        public DateOnly dOB;
        [ObservableProperty]
        public float gPA;
        [ObservableProperty]
        private BitmapImage imagePreview;

        private string _imageFilePath;
        

        [RelayCommand]
        private void UploadImage()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg";
            if (dialog.ShowDialog() == true)
            {
                _imageFilePath = dialog.FileName;
                BitmapImage image = new BitmapImage(new Uri(_imageFilePath));
                ImagePreview = image;
            }
        }

        [RelayCommand]
        public void Save()
        {
            if (string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(LastName))
            {
                MessageBox.Show("Please fill in all the fields", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (GPA < 0 || GPA > 4)
            {
                MessageBox.Show("GPA value must be between 0 and 4.", "Error");
                return;
            }
            Student student = new Student(FirstName, LastName, DOB, GPA, ConvertBitmapImageToByteArray(ImagePreview));
            using (var _context = new SMSDbContext())
            {
                _context.Students.Add(student);
                _context.SaveChanges();
            }
            MessageBox.Show($"Student added successfully Student ID : {student.StudentID}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            var a = new MainWindowVM();
            Application.Current.Windows.OfType<AddWin>().FirstOrDefault()?.Close();

        }

        public byte[] ConvertBitmapImageToByteArray(BitmapImage bitmapImage)
        {
            byte[] data;
            BitmapEncoder encoder = new PngBitmapEncoder(); // or JpegBitmapEncoder or any other encoder you prefer
            encoder.Frames.Add(BitmapFrame.Create(bitmapImage));

            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                data = ms.ToArray();
            }

            return data;
        }

        [RelayCommand]
        public void Cancel()
        {
            Application.Current.Windows.OfType<AddWin>().FirstOrDefault()?.Close();
        }
    }
}
