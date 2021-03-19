using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _19615
{
    /// <summary>
    /// Логика взаимодействия для EditAddForm.xaml
    /// </summary>
    public partial class EditAddForm : Window
    {
        public Service servic { get; set; }
        public string mod { get; set; }
        public string path { get; set; }
        public EditAddForm(Service service, string mode)
        {
            InitializeComponent();

            mod = mode;
            
            if (mode == "Edit")
            {
                servic = service;
                if (service.MainImagePath != null)
                {
                    path = service.MainImagePath;
                    BitmapImage bimage = new BitmapImage();
                    bimage.BeginInit();
                    bimage.UriSource = new Uri(service.MainImagePath, UriKind.Relative);
                    bimage.EndInit();
                    
                    imgImage.Source = bimage;
                }
                textID.Content += service.ID.ToString();
                textTitle.Text = service.Title;
                textCost.Text = Math.Round(service.Cost / Convert.ToDecimal((100 - service.Discount) / 100), 0).ToString();
                textDuration.Text = service.DurationInSeconds.ToString();
                textDiscount.Text = (service.Discount).ToString();
            }
            else if (mode == "Add")
            {
                this.Title = "Добавление услуги";
                textHead.Text = "Добавление услуги";
                textID.Visibility = Visibility.Hidden;
                Edit.Content = "Добавить";
            }
        }
        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (mod == "Edit")
            {
                var serv = Core.db.Service.Where(s => s.ID == servic.ID).ToList();
                serv.First().Title = textTitle.Text;
                serv.First().MainImagePath = path;
                serv.First().Cost = Convert.ToInt32(textCost.Text);
                serv.First().DurationInSeconds = Convert.ToInt32(textDuration.Text) * 60;
                serv.First().Discount = Convert.ToDouble(textDiscount.Text) / 100;
                Core.db.SaveChanges();
            }
            else if (mod == "Add")
            {
                Service serv = new Service();
                serv.Title = textTitle.Text;
                serv.MainImagePath = path;
                serv.Cost = Convert.ToInt32(textCost.Text);
                serv.DurationInSeconds = Convert.ToInt32(textDuration.Text) * 60;
                serv.Discount = Convert.ToDouble(textDiscount.Text) / 100;
                if (Core.db.Service.Where(s => s.Title == serv.Title).ToList().Count > 0)
                {
                    MessageBox.Show("Услуга с данным наименованием уже существует в базе!", "Ошибка!");
                    return;
                }
                Core.db.Service.Add(serv);
                Core.db.SaveChanges();
            }
           
            CountWindow.windows = false;
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            CountWindow.windows = false;
            this.Close();
        }

        private void btnImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Выберите изображение";
            op.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";
            if (op.ShowDialog() == true)
            {
                imgImage.Source = new BitmapImage(new Uri(op.FileName));
                path = op.FileName;
            }
        }
    }
}
