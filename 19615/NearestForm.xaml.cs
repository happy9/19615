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
    /// Логика взаимодействия для NearestForm.xaml
    /// </summary>
    public partial class NearestForm : Window
    {
        public struct clientsServices
        {
            public string Title;
            public string FIO;
            public string Email;
            public string Phone;
            public DateTime Date;
            public TimeSpan LeftTime;
        }
        public NearestForm()
        {
            InitializeComponent();

            var clientService = Core.db.ClientService.OrderBy(cs => cs.StartTime).ToList();

            foreach(ClientService clientservice in clientService)
            {
                var service = Core.db.Service.Where(s => s.ID == clientservice.ServiceID).ToList().First();
                var client = Core.db.Client.Where(c => c.ID == clientservice.ClientID).ToList().First();

                clientsServices cs = new clientsServices();
                cs.Title = service.Title;
                cs.FIO = client.LastName + " " + client.FirstName + " " + client.Patronymic;
                cs.Email = client.Email;
                cs.Phone = client.Phone;
                cs.Date = clientservice.StartTime;
                cs.LeftTime = clientservice.StartTime - DateTime.Now;

                gridRecord.Items.Add(new { Title = cs.Title, FIO = cs.FIO, Email = cs.Email, Phone = cs.Phone, Date = cs.Date, LeftTime = cs.LeftTime });
            }

        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
