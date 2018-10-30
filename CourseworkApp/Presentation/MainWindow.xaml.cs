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

        public message sortMessageType(string header, string body, message asset)
        {
            
            if (header.Contains("S"))
            {
                if (txtBody.Text.Length > 140)
                {
                    throw new Exception("Body character count exceeded. Limit is 140.");
                }
                asset.MessageType = "sms";                
            }
            else if (header.Contains("E"))
            {
                if (txtBody.Text.Length > 1028)
                {
                    throw new Exception("Body character count exceeded. Limit is 1028.");
                }
                asset.MessageType = "email";
            }
            else if (header.Contains("T"))
            {
                if (txtBody.Text.Length > 140)
                {
                    throw new Exception("Body character count exceeded. Limit is 140.");
                }
                asset.MessageType = "tweet";
            }
            
            return asset;
        }

        public void assignAttributes(message asset, string body)
        {
            string[] splitString;
            splitString = body.Split(null, 2);
            asset.Sender = splitString[0];
            asset.Body = splitString[1];
        }

        public void assignSubject(message asset)
        {  
            asset.Subject = asset.Body.Substring(0, 20);
            string newBodyText = asset.Body.Remove(0, 20);
            asset.Body = newBodyText;
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
                    if (!txtBody.Text.Contains(" "))
                    {
                        throw new Exception("Please ensure there is a space between the sender and the main body text.");
                    }
                    message asset = new message();
                    asset = sortMessageType(header, body, asset);
            
                    if (asset.MessageType == null)
                    {
                        MessageBox.Show("Message type could not be determined. Check header.");
                    }

                    assignAttributes(asset, body);

                    if(asset is email)
                    {
                        assignSubject(asset);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
