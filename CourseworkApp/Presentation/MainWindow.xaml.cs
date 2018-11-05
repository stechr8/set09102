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
using System.Text.RegularExpressions;

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

        public void assignSubject(message asset, List<string> incidents, sirList SIRList)
        {
            if (asset.Body.Length >= 40)
            {
                string SIRstring = asset.Body.Substring(0, 12);
                if (Regex.IsMatch(SIRstring, "SIR [0-9][0-9]/[0-9][0-9]/[0-9][0-9]"))
                {
                    string sortCode;
                    string[] splitString = asset.Body.Split(null);
                    if (splitString[2] == "Sort" && splitString[3] == "Code:" && splitString[5] == "Nature" && splitString[6] == "of" && splitString[7] == "Incident:")
                    {
                        asset.IsSIR = true;

                        if (Regex.IsMatch(splitString[4], "[0-9][0-9]-[0-9][0-9]-[0-9][0-9]"))
                        {
                            sortCode = splitString[4];
                        }
                        else
                        {
                            throw new Exception("Sort code invalid");
                        }

                        asset.Subject = "SIR " + splitString[4];
                        string[] sirInfo = new string[2];
                        sirInfo[0] = sortCode;

                        string sirIncident = splitString[8];

                        bool found = false;
                        for (int i = 0; i < incidents.Count(); i++)
                        {
                            if (incidents[i].Equals(sirIncident))
                            {
                                found = true;
                                sirInfo[1] = sirIncident;
                                break;
                            }
                        }
                        if (!found)
                        {
                            sirIncident = splitString[8] + " " + splitString[9];
                            for (int i = 0; i < incidents.Count(); i++)
                            {
                                if (incidents[i].Equals(sirIncident))
                                {
                                    found = true;
                                    sirInfo[1] = sirIncident;
                                    break;
                                }
                            }
                        }
                        if (!found)
                        {
                            throw new Exception("The incident listed is not a registered incident");
                        }
                        string sirInfoJoined = string.Join(" ", sirInfo[0], sirInfo[1]);
                        SIRList.add(sirInfoJoined);
                        return;
                    }
                }

                else if (SIRstring.Contains("SIR"))
                {
                    throw new Exception("This message has been detected as a Significant Incident Report, however the format of the of the subject is incorrect.");
                }
            }
            if (!asset.IsSIR && asset.Body.Length >= 20)
            {
                asset.Subject = asset.Body.Substring(0, 20);
                string newBodyText = asset.Body.Remove(0, 20);
                asset.Body = newBodyText;
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

        public void emailDisplay(sirList sir, urlQuarantinedList urlList)
        {
            if (lstSIR.HasItems)
            {
                lstSIR.Items.Clear();
            }
            for(int i = 0; i < sir.count(); i++)
            {
                lstSIR.Items.Add(sir.returnValue(i));
            }

            if (lstURLs.HasItems)
            {
                lstURLs.Items.Clear();
            }
            for (int i = 0; i < urlList.count(); i++)
            {
                lstURLs.Items.Add(urlList.returnValue(i));
            }

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

                    if (asset.MessageType == "email")
                    {
                        List<string> incidents = new List<string>();
                        sirList SIRList = new sirList();
                        incidents = createIncidentList(incidents);
                        assignSubject(asset, incidents, SIRList);
                        removeUrls(asset, quarantinedList);
                        lstSIR.Items.Add("Sort Code");
                        emailDisplay(SIRList, quarantinedList);
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
