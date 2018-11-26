using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Data;
using Presentation;
using System.Collections.Generic;

namespace MessageTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void SMS()
        {
            MainWindow mainWindow = new MainWindow();
            string testHeader = "S123456789";
            string testBody = "+123123123 This is my test body with a ROTFL textspeak example";
            mainWindow.sortMessageType(testHeader, testBody);
            sms smsAsset = new sms();
            mainWindow.assignAttributes(smsAsset, testBody);
            mainWindow.removeTextspeak(smsAsset);

            Assert.AreEqual(smsAsset.Sender, "+123123123");
            Assert.AreEqual(smsAsset.Body, "This is my test body with a ROTFL <Rolling on the floor laughing> textspeak example");
        }

        [TestMethod]
        public void Email()
        {
            MainWindow mainWindow = new MainWindow();
            string testHeader = "E123456789";
            string testBody = "tester@test.com this is my subject!! This is my test body with a http:\\example.com URL example";
            mainWindow.sortMessageType(testHeader, testBody);
            email emailAsset = new email();
            mainWindow.assignAttributes(emailAsset, testBody);
            List<string> incidents = new List<string>();
            sirList SIRList = new sirList();
            incidents = mainWindow.createIncidentList(incidents);
            mainWindow.assignEmailSubject(emailAsset, incidents, SIRList);
            urlQuarantinedList quarantinedList = new urlQuarantinedList();
            mainWindow.removeUrls(emailAsset, quarantinedList);

            Assert.AreEqual(emailAsset.Sender, "tester@test.com");
            Assert.AreEqual(emailAsset.IsSIR, false);
            Assert.AreEqual(emailAsset.Subject, "this is my subject!!");
            Assert.AreEqual(emailAsset.Body, "This is my test body with a <URL Quarantined> URL example");
        }

        [TestMethod]
        public void EmailSIR()
        {
            MainWindow mainWindow = new MainWindow();
            string testHeader = "E123456789";
            string testBody = "tester@test.com SIR 04/05/18 Sort Code: 99-99-99 Nature of Incident: Staff Attack My body message is http:\\example.com URL example";
            mainWindow.sortMessageType(testHeader, testBody);
            email emailAsset = new email();
            mainWindow.assignAttributes(emailAsset, testBody);
            List<string> incidents = new List<string>();
            sirList SIRList = new sirList();
            incidents = mainWindow.createIncidentList(incidents);
            mainWindow.assignEmailSubject(emailAsset, incidents, SIRList);
            urlQuarantinedList quarantinedList = new urlQuarantinedList();
            mainWindow.removeUrls(emailAsset, quarantinedList);

            Assert.AreEqual(emailAsset.Sender, "tester@test.com");
            Assert.AreEqual(emailAsset.IsSIR, true);
            Assert.AreEqual(emailAsset.Subject, "SIR 04/05/18");
            Assert.AreEqual(emailAsset.Body, "Sort Code: 99-99-99 Nature of Incident: Staff Attack My body message is <URL Quarantined> URL example");
        }

        [TestMethod]
        public void Tweet()
        {
            MainWindow mainWindow = new MainWindow();
            string testHeader = "T123456789";
            string testBody = "@tester This is my test body with a ROTFL textspeak example and a #testingHashtag hashtag example and a mention @steve";
            mainWindow.sortMessageType(testHeader, testBody);
            tweet tweetAsset = new tweet();
            mainWindow.assignAttributes(tweetAsset, testBody);
            mainWindow.removeTextspeak(tweetAsset);
            trendingList trending = new trendingList();
            mainWindow.detectHashtags(tweetAsset, trending);
            mentionsList mentions = new mentionsList();
            mainWindow.detectMentions(tweetAsset, mentions);

            Assert.AreEqual(tweetAsset.Sender, "@tester");
            Assert.AreEqual(tweetAsset.Body, "This is my test body with a ROTFL <Rolling on the floor laughing> textspeak example and a #testingHashtag hashtag example and a mention @steve");
            Assert.AreEqual(trending.returnValue(0), "#testingHashtag");
            Assert.AreEqual(mentions.returnValue(0), "@steve");
        }
    }
}
