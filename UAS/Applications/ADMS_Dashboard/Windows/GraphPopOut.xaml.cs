using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Telerik.Charting;
using Telerik.Windows.Controls.ChartView;

namespace UAD_Explorer.Windows
{
    /// <summary>
    /// Interaction logic for GraphPopOut.xaml
    /// </summary>
    public partial class GraphPopOut : Window
    {
        bool closing = false;
        public static bool IamOpen { get; set; }
        public static string client { get; set; }
        public ObservableCollection<ClientOverview> lstClientRecords = new ObservableCollection<ClientOverview>();
        #region ClassDeclarations
        public class ClientInfo
        {
            public string ClientName { get; set; }
            public double RecordCount { get; set; }

            public ClientInfo(string name, double records)
            {
                this.ClientName = name;
                this.RecordCount = records;
            }
        }

        public class ClientOverview
        {
            public string ClientName { get; set; }
            public ObservableCollection<ClientInfo> Data { get; set; }

            public ClientOverview(string name, ObservableCollection<ClientInfo> data)
            {
                this.ClientName = name;
                this.Data = data;
            }
        }

        public class MainViewModel
        {
            public ObservableCollection<ClientOverview> Data { get; set; }

            public MainViewModel()
            {
                this.Data = GetSampleData();
            }

            private ObservableCollection<ClientOverview> GetSampleData()
            {                
                FrameworkUAS.BusinessLogic.Reports reportData = new FrameworkUAS.BusinessLogic.Reports();
                FrameworkUAS.BusinessLogic.SourceFile sfData = new FrameworkUAS.BusinessLogic.SourceFile();
                Random r = new Random();
                var result = new ObservableCollection<ClientOverview>();
                KMPlatform.BusinessLogic.Client clData = new KMPlatform.BusinessLogic.Client();
                foreach (KMPlatform.Entity.Client cl in clData.Select())
                {
                    int count = 0;
                    DataTable dt = reportData.GetTransformationCount(cl.ClientID, cl.ClientName);
                    foreach(DataRow dr in dt.Rows)
                    {
                        count = (int)dr["Transformation Count"];
                    }

                    int fileCount = sfData.Select(cl.ClientID, false).ToList().Count;

                    result.Add(new ClientOverview(cl.ClientName, new ObservableCollection<ClientInfo> { new ClientInfo(cl.ClientName, fileCount) }));
                    result.Add(new ClientOverview(cl.ClientName, new ObservableCollection<ClientInfo> { new ClientInfo(cl.ClientName, count) }));
                }

                return result;
            }
        }
        #endregion

        public GraphPopOut(String graphName, PieSeries pie)
        {
            InitializeComponent();
            makeCharts(graphName, pie);
        }

        public GraphPopOut()
        {
            InitializeComponent();
            makeTransformationsChart();
        }

        #region EventHandlers
        private void Window_MouseLeave(object sender, MouseEventArgs e)
        {
            if (closing == false)
            {
                client = "";
                IamOpen = false;
                this.Close();
            }
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            if (closing == false)
            {
                client = "";
                IamOpen = false;
                this.Close();
            }
        }
        #endregion

        List<KMPlatform.Entity.Client> allClients = new List<KMPlatform.Entity.Client>();
        public KMPlatform.BusinessLogic.Client clientData = new KMPlatform.BusinessLogic.Client();
        public FrameworkUAS.BusinessLogic.FileLog logData = new FrameworkUAS.BusinessLogic.FileLog();
        public List<FrameworkUAS.Entity.FileLog> fileLogs = new List<FrameworkUAS.Entity.FileLog>();
        public List<int> sourceFileIDS = new List<int>();

        public void makeCharts(string graphName, PieSeries pie)
        {
            pieChart.Visibility = System.Windows.Visibility.Visible;
            IamOpen = true;
            pieChart.HoverMode = PieChartHoverMode.FadeOtherItems;
            this.Closing += (s, _) => { closing = true; };
            this.Closed += (s, _) => { closing = true; };

            PieSeries series = new PieSeries();
            foreach(PieDataPoint p in pie.DataPoints)
            {
                series.DataPoints.Add(new PieDataPoint {Value = p.Value, Label = p.Label});
            }
            series.LabelConnectorsSettings = pie.LabelConnectorsSettings;
            series.ShowLabels = true;
            series.MouseEnter += (s, _) => { this.Cursor = Cursors.Hand; };
            series.MouseLeave += (s, _) => { this.Cursor = Cursors.Arrow; };

            pieChart.Series.Add(series);
        }

        private void ChartSelectionBehavior_SelectionChanged(object sender, ChartSelectionChangedEventArgs e)
        {
            PieDataPoint selection = e.AddedPoints[0] as PieDataPoint;
            string[] labelSplit = selection.Label.ToString().Split(new string[] { "\r\n" }, StringSplitOptions.None);
            client = labelSplit[0];
            IamOpen = false;
            this.Close();
        }

        public void makeTransformationsChart()
        {
            IamOpen = true;
            this.Closing += (s, _) => { closing = true; };
            this.Closed += (s, _) => { closing = true; };
            currentRecordsChart.Visibility = System.Windows.Visibility.Visible;
            spCounts.Visibility = System.Windows.Visibility.Visible;
            MainViewModel mvm = new MainViewModel();
            lstClientRecords = mvm.Data;
            currentRecordsChart.SeriesProvider.Source = lstClientRecords;
        }
    }
}
