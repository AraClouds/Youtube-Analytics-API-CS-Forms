using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTubeAnalytics.v2;
using Google.Apis.YouTubeAnalytics.v2.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AraClouds_Tutoriel_Youtube_Analytics_API
{
    //
    // https://www.araclouds.com
    //

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        public async void FetchYoutubeAPI()
        {
            UserCredential credential;
            using (var stream = new FileStream("client_secrets.json", FileMode.Open, FileAccess.Read))
            {
                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                GoogleClientSecrets.Load(stream).Secrets,
                new[] { YouTubeService.Scope.YoutubeReadonly },
                "user",
                CancellationToken.None
                );
            }

            var youTubeAnalyticsService = new YouTubeAnalyticsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = Assembly.GetExecutingAssembly().GetName().Name
            });

            var request = youTubeAnalyticsService.Reports.Query();
            request.StartDate = ("2019-01-01");
            request.EndDate = ("2020-09-30");
            request.Ids = ("channel==UCmLQ3sdAd6CypJIne5ZANaA");
            request.Metrics = ("views,comments,likes,dislikes,estimatedMinutesWatched,averageViewDuration");

            QueryResponse requestquery = request.Execute();

            List<int> myChannelDataList = new List<int>();

            int listsize = 0;
            foreach (object obj in requestquery.Rows[0])
            { 
                int value = Convert.ToInt32(obj);
                myChannelDataList.Add(value);
                Debug.WriteLine("Value : " + value);
                Debug.WriteLine("LISTE / " + myChannelDataList[0]);
                Debug.WriteLine("Count : " + myChannelDataList.Count);
                listsize = myChannelDataList.Count;
            }
            List<string> metrics = new List<string> { "Vues", "Commentaires", "Likes", "Dislike", "Minutes", "MoyenneTemps" };
            for (int i = 0; i < listsize; i++)
            {
                Label labels = new Label();
                labels.Top = (i + 4) * 20;
                labels.Left = 100;
                labels.AutoSize = true;
                labels.TextAlign = ContentAlignment.MiddleLeft;
                labels.Text = metrics[i] + " :" + myChannelDataList[i].ToString();
                this.Controls.Add(labels);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                FetchYoutubeAPI();
            }
            catch
            {

            }
        }
    }
}
