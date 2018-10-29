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
                if (txtBody.Text.Length > 140)
                {
                    throw new Exception("Body character count exceeded. Limit is 140.");
                }
                sms asset = new sms();
                return asset;
            }
            else if (header.Contains("E"))
            {
                if (txtBody.Text.Length > 1028)
                {
                    throw new Exception("Body character count exceeded. Limit is 1028.");
                }
                email asset = new email();
                return asset;
            }
            else if (header.Contains("T"))
            {
                if (txtBody.Text.Length > 140)
                {
                    throw new Exception("Body character count exceeded. Limit is 140.");
                }
                tweet asset = new tweet();
                return asset;
            }
            return null;
        }

        public void assignAttributes(message asset, string body)
        {
            string[] splitString;
            if (asset is email)
            {
                //THIS NEEDS FIXED. AWAITING EMAIL FROM MODULE LEADER ON SPLITTING INT. PHONE NUMBER FROM BODY
                splitString = body.Split(null, 2);
            }
            else
            {
                splitString = body.Split(null, 2);
            }
            asset.Sender = splitString[0];
            asset.Body = splitString[1];
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //read in message from input
                string header = txtHeader.Text;
                string body = txtBody.Text;

                if (txtHeader.Text != "" || txtBody.Text != "")
                {
                    message asset = sortMessageType(header, body);
                    if (asset == null)
                    {
                        MessageBox.Show("Message type could not be determined. Check header.");
                    }

                    assignAttributes(asset, body);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
