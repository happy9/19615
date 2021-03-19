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
using System.Windows.Shapes;

namespace _19615
{
    /// <summary>
    /// Логика взаимодействия для ClientServiceRegForm.xaml
    /// </summary>
    public partial class ClientServiceRegForm : Window
    {
        public struct clients
        {
            public int ID;
            public string FirstName;
            public string LastName;
        }
        public ClientServiceRegForm(Service service)
        {
            InitializeComponent();

            textTitle.Text += service.Title;
            textDuration.Text += service.DurationInSeconds;

            var clientsList = Core.db.Client.ToList();
            foreach (Client client in clientsList)
            {
                clients c = new clients();
                c.ID = client.ID;
                c.FirstName = client.FirstName;
                c.LastName = client.LastName;

                ListClient.Items.Add(c.FirstName+" "+c.LastName);
            }
        }

        private void Reg_Click(object sender, RoutedEventArgs e)
        {
            ClientService clientService = new ClientService();

            var service = Core.db.Service.Where(s => s.Title == textTitle.Text).ToList().First();
            clientService.ServiceID = service.ID;

            var client = Core.db.Client.ToList()[ListClient.SelectedIndex];

            clientService.ClientID = client.ID;

            clientService.StartTime = (DateTime)datePicker.Value;

            Core.db.ClientService.Add(clientService);
            Core.db.SaveChanges();
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
