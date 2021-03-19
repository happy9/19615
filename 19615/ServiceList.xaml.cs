using LinqToDB;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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
    /// Логика взаимодействия для ServiceList.xaml
    /// </summary>
    public partial class ServiceList : Page
    {
        private List<Service> serviceList;
        List<Service> services = new List<Service> { };
        
        public ServiceList()
        {
            InitializeComponent();
            serviceList = Core.db.Service.ToList();

            foreach (Service service in serviceList)
            {
                decimal Cost= service.Cost;
                double Discount=0;
                string Description = "";
                if (service.Discount != 0)
                {
                    Cost = Math.Round(service.Cost * Convert.ToDecimal(1-service.Discount), 0);
                    Discount = Convert.ToDouble(service.Discount * 100);
                    Description = "* скидка " + Discount + "%";
                }

                services.Add(new Service
                {
                    MainImagePath = service.MainImagePath,
                    Title = service.Title,
                    Cost = Cost,
                    
                    Discount = Discount,
                    DurationInSeconds = service.DurationInSeconds / 60,
                    Description = Description
                });
            }
            ServiceGrid.ItemsSource = services;

            Count.Content = ServiceGrid.Items.Count.ToString();
            Count.Content += " из "+ serviceList.Count().ToString();
        }
        
        private void ServiceList_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            ServiceGrid.UnselectAllCells();
        }

        private void btnAdmin_Click(object sender, RoutedEventArgs e)
        {
            string result = Microsoft.VisualBasic.Interaction.InputBox("Для продолжения введите пароль:");
            if (result == "0000")
            {
                NavigationService.Navigate(new ServiceListAdmin());
            }
            else
                MessageBox.Show("Вы ввели неверный пароль!", "Ошибка!");
                
        }

        private void SortCost_Click(object sender, RoutedEventArgs e)
        {

            if (SortCost.Content.ToString() == "Стоимость по убыванию")
            {
                serviceList = Core.db.Service.OrderByDescending(s => s.Cost).ToList();
                SortCost.Content = "Стоимость по возрастанию";
            }
            else
            {
                serviceList = Core.db.Service.OrderBy(s => s.Cost).ToList();
                SortCost.Content = "Стоимость по убыванию";
            }
            services.Clear();


            foreach (Service service in serviceList)
            {
                decimal Cost = service.Cost;
                double Discount = 0;
                string Description = "";
                if (service.Discount != 0)
                {
                    Cost = Math.Round(service.Cost * Convert.ToDecimal(1 - service.Discount), 0);
                    Discount = Convert.ToDouble(service.Discount * 100);
                    Description = "* скидка " + Discount + "%";
                }

                services.Add(new Service
                {
                    ID = service.ID,
                    MainImagePath = service.MainImagePath,
                    Title = service.Title,
                    Cost = Cost,

                    Discount = Discount,
                    DurationInSeconds = service.DurationInSeconds / 60,
                    Description = Description
                });
            }
            ServiceGrid.ItemsSource = services;

            Count.Content = ServiceGrid.Items.Count.ToString();
            Count.Content += " из " + serviceList.Count().ToString();
        }
    }
}
