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
using Data;

namespace Presentation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public message sortMessageType(string header, string body)
        {
            if (header.Contains("S"))
            {
                sms asset = new sms();
                return asset;
            }
            else if (header.Contains("E"))
            {
                email asset = new email();
                return asset;
            }
            else if (header.Contains("T"))
            {
                tweet asset = new tweet();
                return asset;
            }
            return null;
        }

        public void assignSender(message asset, string header)
        {
            
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            //read in message from input
            string header = txtHeader.Text;
            string body = txtBody.Text;

            if(txtHeader.Text != "" || txtBody.Text != "")
            {
                message asset = sortMessageType(header, body);
                if(asset == null)
                {
                    MessageBox.Show("Message type could not be determined. Check header.");
                }
            }

        }
    }
}
