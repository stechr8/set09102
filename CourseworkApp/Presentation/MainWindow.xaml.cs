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
using System.IO;
using System.Collections;
using Newtonsoft.Json;

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

        public string sortMessageType(string header, string body)
        {
            
            if (header.StartsWith("S"))
            {
                return "S";
            }
            else if (header.StartsWith("E"))
            {
                return "E";
            }
            else if (header.StartsWith("T"))
            {
                return "T";
            }

            return null;
            
        }

        public void assignAttributes(message asset, string body)
        {
            string[] splitString;
            splitString = body.Split(null, 2);
            if (asset is sms)
            {
                if (!splitString[0].StartsWith("+"))
                {
                    throw new Exception("Sender number must start with a '+' as per an international phone number");
                }
                string[] splitSender = splitString[0].Split('+');
                Regex numberRegex = new Regex("^[0-9]+$");
                if (!numberRegex.IsMatch(splitSender[1]))
                {
                    throw new Exception("Sender must contain only numbers (i.e No characters other than digits)");
                }
                if (splitString[1].Length > 140)
                {
                    throw new Exception("Body character count exceeded. Limit is 140.");
                }
            }
            if (asset is email)
            {
                if (!splitString[0].Contains("@"))
                {
                    throw new Exception("Invalid email address format");
                }
                if (splitString[1].Length > 1028)
                {
                    throw new Exception("Body character count exceeded. Limit is 1028.");
                }
            }
            if (asset is tweet) 
            {
                if(splitString[0].Length > 15)
                {
                    throw new Exception("Sender is more than 15 characters");
                }
                if (!splitString[0].StartsWith("@"))
                {
                    throw new Exception("Invalid sender format");
                }
                if (splitString[1].Length > 140)
                {
                    throw new Exception("Body character count exceeded. Limit is 140.");
                }
            }
            asset.Sender = splitString[0];
            asset.Body = splitString[1];
        }

        public void assignId(message asset)
        {
            try
            {
                string splitString = txtHeader.Text.Substring(1);
                asset.Id = Convert.ToInt32(splitString);
            }
            catch (Exception ex)
            {
                throw new Exception("Header not in correct format (i.e 'S1234567...')");
            }
        }

        public void assignEmailSubject(email asset, List<string> incidents, sirList SIRList)
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
                        //check if incident given is a recognised incident in the list
                        for (int i = 0; i < incidents.Count(); i++)
                        {
                            if (incidents[i].Equals(sirIncident))
                            {
                                found = true;
                                sirInfo[1] = sirIncident;
                                break;
                            }
                        }
                        /*if the incident is not found yet the message has been identified as an SIR
                         *then check if the incident consists of two words, such as "ATM Theft" 
                         */
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
            //if message is standard email
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
            //check every word in body for url
            for (int i = 0; i < bodyText.Length; i++)
            {
                if (bodyText[i].StartsWith("http:\\"))
                {
                    //add url to list and replace it with the replacement string
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
            //if listbox needs refreshing, do so
            if (lstSIR.HasItems)
            {
                lstSIR.Items.Clear();
            }
            //then add new list to it
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

        public void removeTextspeak(message asset)
        {
            //abbreviations and extensions will be kept separate for readablity and ease of understanding
            List<string> abbreviation = new List<string>();
            List<string> expanded = new List<string>();
            //read in textspeak abbreviations and extensions from csv file
            using (var reader = new StreamReader(@"C:\Users\stech\source\repos\set09102\CourseworkApp\textwords.csv"))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] buffer = line.Split(',');
                    abbreviation.Add(buffer[0]);
                    expanded.Add(buffer[1]);
                }
            }

            ArrayList bodyText = new ArrayList();
            string[] textSplit = asset.Body.Split(null);
            //fill ArrayList with individual words from the body
            for(int i = 0; i < textSplit.Length; i++)
            {
                bodyText.Insert(i, textSplit[i]);
            }

            //for every word in the body
            for (int i = 0; i < bodyText.Count; i++)
            {
                //for every abbreviation in the dictionary
                for(int j = 0; j < abbreviation.Count(); j++)
                {
                    //if current word matches current abbreviation
                    if (bodyText[i].Equals(abbreviation.ElementAt(j)))
                    {
                        //build extension string
                        string expandedAddition = "<" + expanded.ElementAt(j) + ">";
                        //add extension string into the ArrayList after current word
                        bodyText.Insert(i+1, expandedAddition);
                    }
                }
                    
            }
            //update body
            asset.Body = string.Join(" ", bodyText.ToArray());
            MessageBox.Show(asset.Body);
        }

        public void detectHashtags(message asset, trendingList trending)
        {
            string[] bodyText = asset.Body.Split(null);
            for (int i = 0; i < bodyText.Length; i++)
            {
                if (bodyText[i].StartsWith("#"))
                {
                    trending.add(bodyText[i]);
                }
            }
        }

        public void detectMentions(message asset, mentionsList mentions)
        {
            string[] bodyText = asset.Body.Split(null);
            for (int i = 0; i < bodyText.Length; i++)
            {
                if (bodyText[i].StartsWith("@"))
                {
                    mentions.add(bodyText[i]);
                }
            }
        }

        public void tweetDisplay(message asset, trendingList trending, mentionsList mentions)
        {
            //if listbox needs refreshing, do so
            if (lstTrending.HasItems)
            {
                lstTrending.Items.Clear();
            }
            //then add new list to it
            for (int i = 0; i < trending.count(); i++)
            {
                lstSIR.Items.Add(trending.returnValue(i));
            }

            if (lstMentions.HasItems)
            {
                lstMentions.Items.Clear();
            }
            for (int i = 0; i < mentions.count(); i++)
            {
                lstMentions.Items.Add(mentions.returnValue(i));
            }
        }

        public void serialiser(message asset, List<message> msgList)
        {
            JsonConvert.SerializeObject(asset);
            string filepath = @"C:\Users\stech\source\repos\set09102\CourseworkApp\saveFile.json";

            if (!File.Exists(filepath))
            {
                using (StreamWriter file = File.CreateText(filepath))
                {
                    //add message to list ad serialise to save into new file
                    JsonSerializer serializer = new JsonSerializer();
                    msgList.Add(asset);
                    serializer.Serialize(file, msgList);
                }
            }
            else
            {
                //if file exists already, read in data from file and store in list
                string jsonData = File.ReadAllText(filepath);
                msgList = JsonConvert.DeserializeObject<List<message>>(jsonData);
                //add message to list
                msgList.Add(asset);
                //re-serialise list
                jsonData = JsonConvert.SerializeObject(msgList);
                //re-write updated list
                File.WriteAllText(filepath, jsonData);
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

                    
                    //determine message type
                    string type = sortMessageType(header, body);

                    if (type == null)
                    {
                        MessageBox.Show("Message type could not be determined. Check header.");
                    }

                    

                    List<message> msgList = new List<message>();


                    switch (type)
                    {
                        case "E":
                            email emailAsset = new email();
                            //assign sender and body text to message
                            assignAttributes(emailAsset, body);
                            assignId(emailAsset);

                            List<string> incidents = new List<string>();
                            sirList SIRList = new sirList();

                            //fill list of registered incidents
                            incidents = createIncidentList(incidents);
                            assignEmailSubject(emailAsset, incidents, SIRList);
                            urlQuarantinedList quarantinedList = new urlQuarantinedList();
                            removeUrls(emailAsset, quarantinedList);
                            //display details on UI
                            emailDisplay(SIRList, quarantinedList);
                            serialiser(emailAsset, msgList);
                            break;

                        case "T":
                            tweet tweetAsset = new tweet();
                            //assign sender and body text to message
                            assignAttributes(tweetAsset, body);
                            assignId(tweetAsset);
                            removeTextspeak(tweetAsset);
                            trendingList trending = new trendingList();
                            detectHashtags(tweetAsset, trending);
                            mentionsList mentions = new mentionsList();
                            detectMentions(tweetAsset, mentions);
                            tweetDisplay(tweetAsset, trending, mentions);
                            serialiser(tweetAsset, msgList);
                            break;

                        case "S":
                            sms smsAsset = new sms();
                            //assign sender and body text to message
                            assignAttributes(smsAsset, body);
                            assignId(smsAsset);
                            //identify textspeak and add extension of it
                            removeTextspeak(smsAsset);
                            serialiser(smsAsset, msgList);
                            break;
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
