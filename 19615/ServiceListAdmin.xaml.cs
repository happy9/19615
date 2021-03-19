using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Логика взаимодействия для ServiceListAdmin.xaml
    /// </summary>
    public partial class ServiceListAdmin : Page
    {
        private List<Service> serviceList;
        List<Service> services = new List<Service> { };
        public ServiceListAdmin()
        {
            InitializeComponent();
            SortCost.Content = "Стоимость по убыванию";
            serviceList = Core.db.Service.ToList();

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

        private void btnClient_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ServiceList());
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (CountWindow.windows)
                return;
            Service item = null;
            var rowIndex = ServiceGrid.SelectedIndex;
            var row = (DataGridRow)ServiceGrid.ItemContainerGenerator.ContainerFromIndex(rowIndex);
            if (row.Item is Service)
                item = row.Item as Service;
            EditAddForm form = new EditAddForm(item, "Edit");
            form.Show();

        }

        private void AddService_Click(object sender, RoutedEventArgs e)
        {
            if (CountWindow.windows)
                return;
            CountWindow.windows = true;
            Service item = null;
            EditAddForm form = new EditAddForm(item, "Add");
            form.Show();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            Service item = null;
            var rowIndex = ServiceGrid.SelectedIndex;
            var row = (DataGridRow)ServiceGrid.ItemContainerGenerator.ContainerFromIndex(rowIndex);
            if (row.Item is Service)
                item = row.Item as Service;
            Core.db.Service.Remove(Core.db.Service.Where(s => s.ID == item.ID).First());
            Core.db.SaveChanges();
            NavigationService.Navigate(new ServiceListAdmin());
        }

        private void Registration_Click(object sender, RoutedEventArgs e)
        {
            Service item = null;
            var rowIndex = ServiceGrid.SelectedIndex;
            var row = (DataGridRow)ServiceGrid.ItemContainerGenerator.ContainerFromIndex(rowIndex);
            if (row.Item is Service)
                item = row.Item as Service;
            ClientServiceRegForm form = new ClientServiceRegForm(item);
            form.Show();
        }

        private void btnNearest_Click(object sender, RoutedEventArgs e)
        {
            NearestForm form = new NearestForm();
            form.Show();
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
