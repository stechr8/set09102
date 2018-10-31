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

        public void sortMessageType(string header, string body, message asset)
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
            if(txtBody.Text.Length >= 20)
            {
                string potentialSIR = asset.Body.Substring(0, 59);
                string[] splitString = potentialSIR.Split(null);
                if (splitString[0] == "Sort" && splitString[1] == "Code:" && splitString[4] == "Nature" && splitString[5] == "of" && splitString[6] == "Incident:")
                {
                    asset.IsSIR = true;
                    //implement SIR Incident list and saving
                    
                }
                else
                {
                    asset.Subject = asset.Body.Substring(0, 20);
                    string newBodyText = asset.Body.Remove(0, 20);
                    asset.Body = newBodyText;
                }
                
            }
            else
            {
                throw new Exception("The subject of the message is less than 20 characters");
            }
         }

        public void removeUrls(message asset, urlQuarantinedList quarantinedList)
        {
            string[] bodyText = asset.Body.Split(null);
            string urlReplacement = "<URL Quarantined>";
            for (int i = 0; i < bodyText.Length; i++)
            {
                if (bodyText[i].Contains("http:\\"))
                {
                    quarantinedList.add(bodyText[i]);
                    bodyText[i] = urlReplacement;
                }
            }

            asset.Body = string.Join(" ", bodyText);
            MessageBox.Show(asset.Body);
        }

        public List<string> createIncidentList(List<string> incidents)
        {
            incidents.Add("Theft");
            incidents.Add("Staff Attack");
            incidents.Add("ATM Theft");
            incidents.Add("Raid");
            incidents.Add("Customer Attack");
            incidents.Add("Staff Abuse");
            incidents.Add("Bomb Threat");
            incidents.Add("Terrorism");
            incidents.Add("Suspicious Incident");
            incidents.Add("Intelligence");
            incidents.Add("Cash Loss");
            return incidents;
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

                    urlQuarantinedList quarantinedList = new urlQuarantinedList();

                    message asset = new message();
                    sortMessageType(header, body, asset);
            
                    if (asset.MessageType == null)
                    {
                        MessageBox.Show("Message type could not be determined. Check header.");
                    }

                    assignAttributes(asset, body);

                    if(asset.MessageType == "email")
                    {
                        assignSubject(asset);
                        List<string> incidents = new List<string>();
                        incidents = createIncidentList(incidents);
                        removeUrls(asset, quarantinedList);
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
