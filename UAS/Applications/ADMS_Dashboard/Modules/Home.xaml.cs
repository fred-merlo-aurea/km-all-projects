using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Win32;
using Telerik.Windows.Controls;
using Telerik.Windows.Documents.Layout;
using Telerik.Windows.Documents.Model;
using Telerik.Windows.Documents.FormatProviders.Pdf;
using Telerik.Windows.Controls.ChartView;
using Telerik.Charting;
using UAD_Explorer.Windows;
using System.Collections.ObjectModel;
using System.Windows.Controls.Primitives;
using Telerik.Windows.Controls.Map;
using System.Windows.Threading;
using Telerik.Windows.Documents.TextSearch;
using Telerik.Windows.Documents.FormatProviders.Txt;
using Telerik.Windows.Documents.UI;
using Telerik.Windows.Documents.Flow.FormatProviders.Pdf;
using Telerik.Windows.Documents.FormatProviders;

namespace ADMS_Dashboard.Modules
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : UserControl
    {
        #region BunchaDeclarations
        public DoubleAnimation animateButtonWidth { get; set; }
        public DoubleAnimation animateOpacity { get; set; }
        public DoubleAnimation resetRecWidth { get; set; }
        public DoubleAnimation animateRec { get; set; }
        public DoubleAnimation recWidthToZero { get; set; }
        public DoubleAnimation resetOpacity { get; set; }
        public FrameworkUAS.BusinessLogic.FileLog logData = new FrameworkUAS.BusinessLogic.FileLog();
        public List<FrameworkUAS.Entity.FileLog> fileLogs = new List<FrameworkUAS.Entity.FileLog>();
        public FrameworkUAS.BusinessLogic.SourceFile sfData = new FrameworkUAS.BusinessLogic.SourceFile();
        public KMPlatform.BusinessLogic.Client clientData = new KMPlatform.BusinessLogic.Client();
        public List<int> sourceFiles = new List<int>();
        //public FrameworkUAS.BusinessLogic.FileStatus fsData = new FrameworkUAS.BusinessLogic.FileStatus();
        public List<FrameworkUAD_Lookup.Entity.Code> allFSTypes = new List<FrameworkUAD_Lookup.Entity.Code>();//FileStatusType
        public FrameworkUAD_Lookup.BusinessLogic.Code fsTypeData = new FrameworkUAD_Lookup.BusinessLogic.Code();//FileStatusType
        List<KMPlatform.Entity.Client> allClients = new List<KMPlatform.Entity.Client>();
        private List<KMPlatform.Entity.Client> fileAndLogClientLst = new List<KMPlatform.Entity.Client>();
        public ObservableCollection<ClientOverview> lstClientRecords = new ObservableCollection<ClientOverview>();
        public BackgroundWorker w = new BackgroundWorker();
        System.Windows.Threading.DispatcherTimer liveTimer = new System.Windows.Threading.DispatcherTimer();
        public bool onReports = false;
        public bool onHome = false;
        public bool onGraphs = false;
        public bool onLogs = false;
        public bool onStatus = false;
        public bool logsFirstRun = true;
        public string currentPage = "Home";
        public double cpuUsage = 0;
        public int total = 0;

        FrameworkUAS.Object.AppData myAppData;
        #endregion

        #region Class Declarations
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
                Random r = new Random();
                var result = new ObservableCollection<ClientOverview>();
                KMPlatform.BusinessLogic.Client clData = new KMPlatform.BusinessLogic.Client();
                foreach(KMPlatform.Entity.Client cl in clData.Select())
                {
                    result.Add(new ClientOverview(cl.ClientName, new ObservableCollection<ClientInfo> {new ClientInfo(cl.ClientName, r.Next(0,100))}));
                }

                return result;
            }
        }

        public class RunningFiles : INotifyPropertyChanged
        {
            public RunningFiles(int fileCount, DateTime currentTime)
            {
                this.FileCount = fileCount;
                this.CurrentTime = currentTime;
            }
            private int count;
            public int FileCount
            {
                get { return count; }
                set { SetField(ref count, value, "FileCount"); }
            }
            private DateTime time;
            public DateTime CurrentTime
            {
                get { return time; }
                set { SetField(ref time, value, "CurrentTime"); }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            private void OnPropertyChanged(string prop)
            {
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }

            protected bool SetField<T>(ref T field, T value, string propertyName)
            {
                if (EqualityComparer<T>.Default.Equals(field, value)) return false;
                field = value;
                OnPropertyChanged(propertyName);
                return true;
            }
        }
        #endregion

        public Home(FrameworkUAS.Object.AppData appData)
        {
            InitializeComponent();
            myAppData = appData;
            
            UAD_Explorer.Windows.BusyIndicator s = new UAD_Explorer.Windows.BusyIndicator();
            s.RadBusyIndicator.IsBusy = true;
            s.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            s.Dispatcher.BeginInvoke(new Action(() =>
            {
                w = new BackgroundWorker();
                w.WorkerSupportsCancellation = true;
                w.DoWork += delegate
                {
                    LoadData();
                };
                w.RunWorkerCompleted += delegate
                {
                    s.Close();
                };
                w.RunWorkerAsync();
            }));
            s.ShowDialog();
            LiveData();
            
        }

        private void LoadData()
        {
            LoadAnimations();

            allClients = myAppData.AuthorizedUser.User.CurrentClientGroup.Clients;// clientData.Select();
            allFSTypes = fsTypeData.Select();
            Style style = this.FindResource("TelerikFlyOutButton") as Style;
            this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    rbReport.Style = style;
                    rbGraph.Style = style;
                    rbLogs.Style = style;
                    rbHome.Style = style;
                    //string desktop = ConfigurationManager.AppSettings["DashBoardViewer"].ToString();
                    //if (!desktop.Equals("WEB"))
                    //{
                    //    Window b = Core.Utilities.WPF.GetMainWindow();
                    //    ScrollViewer sv = b.FindName("mainScrollBar") as ScrollViewer;
                    //    sv.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
                    //}
                    svRecordsLegend.MouseLeave += delegate { rExpanderLegend.IsExpanded = false; };
                    rExpanderLegend.MouseEnter += delegate { rExpanderLegend.IsExpanded = true; };
                    rExpanderLegend.MouseLeave += delegate { rExpanderLegend.IsExpanded = false; };
                    pieChart.Visibility = System.Windows.Visibility.Visible;
                    updateGUIContent();
                }));
            LoadGraphs();
            //bool isRunning = (Process.GetProcessesByName("ADMS_Console.vshost").Count() > 0);
            //Line line = new Line();
            //myCanvas.Children.Add(line);
            //line.Stroke = Brushes.Red;
            //line.StrokeThickness = 2;
            //line.X1 = 0;
            //line.Y1 = 0;
            //Storyboard sb = new Storyboard();
            ////DoubleAnimation da = new DoubleAnimation(line.Y2, 100, new Duration(new TimeSpan(0, 0, 1)));
            //DoubleAnimation da1 = new DoubleAnimation(line.X2, 1000, new Duration(new TimeSpan(0, 0, 1)));
            ////Storyboard.SetTargetProperty(da, new PropertyPath("(Line.Y2)"));
            //Storyboard.SetTargetProperty(da1, new PropertyPath("(Line.X2)"));
            ////sb.Children.Add(da);
            //sb.Children.Add(da1);
            //line.BeginStoryboard(sb);
        }

        #region MenuButtons
        private void RadButton_MouseEnter(object sender, MouseEventArgs e)
        {
            RadButton meButton = (RadButton)sender;
            meButton.Content = meButton.Tag.ToString();
        }

        private void RadButton_MouseLeave(object sender, MouseEventArgs e)
        {
            RadButton meButton = (RadButton)sender;
            if (!meButton.Tag.Equals("Home"))
            {
                Image img = menuButtons.FindName(meButton.Tag.ToString().Replace(" ", "")) as Image;
                meButton.Content = img;
            }
            else
            {
                Image img = menuButtons.FindName("homePage") as Image;
                meButton.Content = img;
            }
        }
        #endregion

        #region Home

        public void clearHome()
        {
            onHome = false;
            HomePage.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void rbHome_Click(object sender, RoutedEventArgs e)
        {
            if (onHome == false)
            {
                disableMenuButtons(true);
                LoadAnimations();
                onHome = true;
                resetRecWidth.Completed += (s, _) => disableMenuButtons(false);
                resetOpacity.Completed += (s, _) => swipeRec.BeginAnimation(Rectangle.WidthProperty, resetRecWidth);
                recWidthToZero.Completed += (s, _) => swipeRec.BeginAnimation(Rectangle.OpacityProperty, resetOpacity);
                animateOpacity.Completed += (s, _) => swipeRec.BeginAnimation(Rectangle.WidthProperty, recWidthToZero);
                animateRec.Completed += (l, b) =>
                {
                    loadHome();
                    swipeRec.BeginAnimation(Rectangle.OpacityProperty, animateOpacity);
                };
                swipeRec.BeginAnimation(Rectangle.WidthProperty, animateRec);
            }
        }

        private void loadHome()
        {
            callClear();
            currentPage = "Home";
            HomePage.Visibility = System.Windows.Visibility.Visible;
        }

        private void LoadGraphs()
        {
            FrameworkUAD_Lookup.Entity.Code fsCompletedType = fsTypeData.SelectCodeName(FrameworkUAD_Lookup.Enums.CodeType.File_Status, FrameworkUAD_Lookup.Enums.FileStatusType.Completed.ToString());//FileStatusType
            FrameworkUAD_Lookup.Entity.Code fsInvalidType = fsTypeData.SelectCodeName(FrameworkUAD_Lookup.Enums.CodeType.File_Status, FrameworkUAD_Lookup.Enums.FileStatusType.Invalid.ToString());//FileStatusType
            List<int> totalFilesBarSource = new List<int>();
            foreach (KMPlatform.Entity.Client cl in allClients)
            {
                fileLogs = logData.SelectClient(cl.ClientID);
                sourceFiles = fileLogs.Select(x => x.SourceFileID).Distinct().ToList();
                if (sourceFiles != null && sourceFiles.Count > 0)
                {
                    PieDataPoint point2 = new PieDataPoint();
                    point2.Value = sourceFiles.Count;
                    point2.Label = cl.ClientName + "\r\n" + sourceFiles.Count;
                    totalFiles.DataPoints.Add(point2);
                    total += sourceFiles.Count;
                }
                fileLogs = logData.SelectClient(cl.ClientID);
                sourceFiles = fileLogs.Where(x => x.LogDate.ToShortDateString() == DateTime.Today.ToShortDateString()).Select(x => x.SourceFileID).Distinct().ToList();
                if (sourceFiles != null && sourceFiles.Count > 0)
                {
                    PieDataPoint point2 = new PieDataPoint();
                    point2.Value = sourceFiles.Count;
                    point2.Label = cl.ClientName + "\r\n" + sourceFiles.Count;
                    dailyFiles.DataPoints.Add(point2);
                }
            }
            MainViewModel mvm = new MainViewModel();
            lstClientRecords = mvm.Data;
            this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    currentRecordsChart.SeriesProvider.Source = lstClientRecords;
                    //totalFilesBarSource.Add(fsData.Select().Count);
                    //totalFilesCompleteSummary.Value = fsData.SelectStatus(fsCompletedType.CodeId).Count;
                    //totalFilesFailSummary.Value = fsData.SelectStatus(fsInvalidType.CodeId).Count;
                    totalFilesBarSource.Add(total);
                    totalFilesSummary.BarBrushes.Clear();
                    totalFilesSummary.BarBrushes.Add((SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF47E1F")));
                    totalFilesSummary.BarBrushes.Add((SolidColorBrush)(new BrushConverter().ConvertFrom("#FF045DA4")));
                    totalFilesSummary.ItemsSource = totalFilesBarSource;
                    totalRecordsSummary.BarBrushes.Clear();
                    totalRecordsSummary.BarBrushes.Add((SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF47E1F")));
                    totalRecordsSummary.BarBrushes.Add((SolidColorBrush)(new BrushConverter().ConvertFrom("#FF045DA4")));
                    totalRecordsSummary.ItemsSource = totalFilesBarSource;
                }));

        }

        private void pieChart_MouseEnter(object sender, MouseEventArgs e)
        {
            if (GraphPopOut.IamOpen == false)
            {
                PieSeries pie = sender as PieSeries;
                DoubleAnimation fadeIn = new DoubleAnimation
                {
                    To = 1,
                    Duration = TimeSpan.FromSeconds(0.5)
                };

                GraphPopOut a = new GraphPopOut(pie.Tag.ToString(), pie);
                a.Closed += delegate
                {
                    if (GraphPopOut.client != null && GraphPopOut.client != "")
                    {
                        rbGraph.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                    }
                };
                Point p = pie.PointToScreen(new Point(0d, 0d));
                a.Left = p.X - 400;
                a.Top = p.Y - 50;
                a.Opacity = 0;
                a.Show();
                a.BeginAnimation(Window.OpacityProperty, fadeIn);
            }
        }

        private void updateGUIContent()
        {
            var counters = new List<PerformanceCounter>();
            System.Windows.Threading.DispatcherTimer guiTimer = new System.Windows.Threading.DispatcherTimer();
            System.Windows.Threading.DispatcherTimer sleepTimer = new System.Windows.Threading.DispatcherTimer();
            sleepTimer.Tick += new EventHandler(delegate(object s, EventArgs ev)
            {
                int i = 0;
                foreach (var counter in counters)
                {
                    cpuUsage = (Math.Round(counter.NextValue(), 1));
                    ++i;
                }
                cpuIndicator.Value = cpuUsage;

                guiTimer.Start();
            });
            sleepTimer.Interval = new TimeSpan(0, 0, 3);
            guiTimer.Tick += new EventHandler(delegate(object s, EventArgs ev)
            {
                Process[] runningNow = Process.GetProcesses();
                counters = new List<PerformanceCounter>();
                foreach (Process process in runningNow.Where(x => x.ProcessName == "firefox"))
                {
                    BackgroundWorker bw = new BackgroundWorker();
                    bw.DoWork += delegate
                    {
                        var counter = new PerformanceCounter("Process", "% Processor Time", process.ProcessName);
                        counter.NextValue();
                        counters.Add(counter);
                    };
                    bw.RunWorkerAsync();
                }
                sleepTimer.Start();
            });
            guiTimer.Interval = new TimeSpan(0, 0, 5);
            guiTimer.Start();
        }

        private void LiveData()
        {
            ObservableCollection<RunningFiles> files = new ObservableCollection<RunningFiles>();
            Telerik.Windows.Controls.ChartView.AreaSeries liveFiles = new Telerik.Windows.Controls.ChartView.AreaSeries();
            liveFiles.CategoryBinding = new PropertyNameDataPointBinding() { PropertyName = "CurrentTime" };
            liveFiles.ValueBinding = new GenericDataPointBinding<RunningFiles, int>() { ValueSelector = product => product.FileCount };
            liveFiles.ShowLabels = true;
            liveFiles.LabelDefinitions.Add(new ChartSeriesLabelDefinition { Binding = new GenericDataPointBinding<RunningFiles, int>() { ValueSelector = product => product.FileCount }, Margin = new Thickness(0, 0, 6, 0) });
            liveFiles.ItemsSource = files;
            liveFiles.Fill = ((SolidColorBrush)(new BrushConverter().ConvertFrom("#FF045DA4")));
            int i = 1;
            files.Add(new RunningFiles(10, DateTime.Now));
            liveChart.Series.Add(liveFiles);
            //DateTime currentMin = DateTime.Now.AddSeconds(1);
            
            liveTimer.Tick += delegate
            {
                BackgroundWorker bwLive = new BackgroundWorker();
                bwLive.DoWork += delegate
                {
                    liveTimer.IsEnabled = false;
                    //int runningFiles = 0;
                    //fileStatuses = fsData.Select();
                    //FrameworkUAD_Lookup.Entity.Code fsCompletedType = fsTypeData.SelectCodeName(FrameworkUAD_Lookup.Enums.CodeTypes.File_Status, FrameworkUAD_Lookup.Enums.FileStatusTypes.Processed.ToString());
                    //FrameworkUAD_Lookup.Entity.Code fsInvalidType = fsTypeData.SelectCodeName(FrameworkUAD_Lookup.Enums.CodeTypes.File_Status, FrameworkUAD_Lookup.Enums.FileStatusTypes.Invalid.ToString());
                    //runningFiles = fileStatuses.Where(x => x.FileStatusTypeID != fsCompletedType.CodeId && x.FileStatusTypeID != fsInvalidType.CodeId).Count();
                    //var q = persons.IndexOf(persons.Where(x => x.CurrentTime == DateTime.Now.ToShortTimeString()).FirstOrDefault());
                    Random random = new Random();
                    int randomNumber = random.Next(0, 50);
                    //if (q != -1)
                    //{
                    //    persons[q].FileCount = randomNumber;
                    //}
                    if (files.Count > 5)
                    {
                        i = 0;
                        this.Dispatcher.BeginInvoke(new Action(() =>
                            {
                                files.RemoveAt(0);
                            }));
                    }
                    if (randomNumber != files.Last().FileCount)
                        this.Dispatcher.BeginInvoke(new Action(() =>
                            {
                                files.Add(new RunningFiles(randomNumber, DateTime.Now));
                                i++;
                            }));
                    //liveTimer.Start();
                };
                bwLive.RunWorkerCompleted += delegate
                {
                    liveTimer.IsEnabled = true;
                };
                bwLive.RunWorkerAsync();
                };
                
            liveTimer.Interval = new TimeSpan(0, 0, 3);
            liveTimer.Start();
        }

        private void rwpRecordsLegend_MouseEnter(object sender, MouseEventArgs e)
        {
            RadWrapPanel rwp = sender as RadWrapPanel;
            rwp.MouseLeave += delegate { currentRecordsChart.HoverMode = ChartHoverMode.None; };

            currentRecordsChart.HoverMode = ChartHoverMode.FadeOtherSeries;
            foreach (Telerik.Windows.Controls.Legend.LegendItem l in currentRecordsChart.LegendItems)
            {
                if (l.IsHovered == true && l.VisualState.Equals("Highlighted"))
                {
                    CategoricalDataPoint point = l.Presenter as CategoricalDataPoint;
                    point.IsSelected = true;
                }
            }
        }

        private void rbTransformCounts_Click(object sender, RoutedEventArgs e)
        {
            if (GraphPopOut.IamOpen == false)
            {
                //PieSeries pie = sender as PieSeries;
                DoubleAnimation fadeIn = new DoubleAnimation
                {
                    To = 1,
                    Duration = TimeSpan.FromSeconds(0.5)
                };

                GraphPopOut g = new GraphPopOut();
                g.BorderThickness = new Thickness(2);
                g.Width = 1000;
                g.Height = 500;
                Point p = tbCurrentRecords.PointToScreen(new Point(0d, 0d));
                g.Left = p.X - 50;
                g.Top = p.Y;
                g.Opacity = 0;
                g.Show();
                g.BeginAnimation(Window.OpacityProperty, fadeIn);
            }
        }

        private void imgPausePlayLiveChart_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (imgPausePlayLiveChart.Tag.Equals("On"))
            {
                imgPausePlayLiveChart.Tag = "Off";
                liveTimer.Stop();
            }
            else
            {
                imgPausePlayLiveChart.Tag = "On";
                liveTimer.Start();
            }
        }

        #endregion

        #region Reports

        List<string> fileExtensionsLst = new List<string>();
        List<string> fileStatusesLst = new List<string>();
        FrameworkUAD.BusinessLogic.SubscriberOriginal subOriginalData = new FrameworkUAD.BusinessLogic.SubscriberOriginal();
        private void rbReport_Click(object sender, RoutedEventArgs e)
        {
            if (onReports == false)
            {
                LoadAnimations();
                disableMenuButtons(true);
                onReports = true;
                resetRecWidth.Completed += (s, _) => disableMenuButtons(false);
                resetOpacity.Completed += (s, _) => swipeRec.BeginAnimation(Rectangle.WidthProperty, resetRecWidth);
                recWidthToZero.Completed += (s, _) => swipeRec.BeginAnimation(Rectangle.OpacityProperty, resetOpacity);
                animateOpacity.Completed += (s, _) => swipeRec.BeginAnimation(Rectangle.WidthProperty, recWidthToZero);
                animateRec.Completed += (l, b) =>
                {
                    w = new BackgroundWorker();
                    w.WorkerSupportsCancellation = true;
                    w.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompletedReports);
                    w.DoWork += new DoWorkEventHandler(worker_DoWorkReports);
                    w.RunWorkerAsync();
                    swipeRec.BeginAnimation(Rectangle.OpacityProperty, animateOpacity);
                };
                swipeRec.BeginAnimation(Rectangle.WidthProperty, animateRec);
            }
        }

        #region Background Work

        public void worker_RunWorkerCompletedReports(object sender, RunWorkerCompletedEventArgs e)
        {
            callClear();
            currentPage = "Reports";
            loadReports();
        }

        public void clearReports()
        {
            onReports = false;
            fileNameReports.ItemsSource = null;
            DateFromReports.SelectedValue = null;
            clientListReports.SelectedValue = null;
            filePubCodeReports.ItemsSource = null;
            fileStatusReports.SelectedValue = null;
            fileExtensionReports.SelectedValue = null;
            saveReport.Visibility = System.Windows.Visibility.Collapsed;
            ReportsPage.Visibility = System.Windows.Visibility.Collapsed;
            filePubCodeReports.IsEnabled = false;
            RadDocument blank = new RadDocument();
            richTxtBoxReports.Document = blank;
        }

        List<FrameworkUAS.Entity.SourceFile> allSF = new List<FrameworkUAS.Entity.SourceFile>();
        List<FrameworkUAD_Lookup.Entity.Code> allFST = new List<FrameworkUAD_Lookup.Entity.Code>();//FileStatusType
        public void worker_DoWorkReports(object sender, DoWorkEventArgs e)
        {
            //allSF = sfData.Select(false);
            foreach(KMPlatform.Entity.Client c in myAppData.AuthorizedUser.User.CurrentClientGroup.Clients)
            {
                //allSF.AddRange(c.SourceFilesList);
            }
            allFST = fsTypeData.Select();
        }

        private void loadReports()
        {
            ReportsPage.Visibility = System.Windows.Visibility.Visible;
            Thread t = new Thread(new ThreadStart(DoWork));
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.IsBackground = true;
            t.Start();
            List<string> extensions = new List<string>() { ".dbf", ".xlsx", ".csv", ".txt", ".xls" };
            List<string> fileTypes = new List<string>() { "Standard", "Special" };
            clientListReports.ItemsSource = allClients.Select(x => x.ClientName);
            fileStatusReports.ItemsSource = allFST;
            fileNameReports.ItemsSource = allSF.Select(x => x.FileName);
            fileExtensionReports.ItemsSource = extensions;
            fileTypeReports.ItemsSource = fileTypes;
            rbtnGenerateReports.Click -= generateReportsClick;
            t.Abort();
            w.CancelAsync();
            w.Dispose();
            w = null;
            GC.Collect();
        }

        #endregion

        #region SelectionsAndCheckBoxes
        private void radComboChecked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            var p = cb.Parent;
            TextBlock c = null;
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(p); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(p, i);
                if (child is TextBlock)
                    c = child as TextBlock;
            }
            switch(c.Tag.ToString())
            {
                case "FileStatus":
                    fileStatusesLst.Add(c.Text);
                    break;
                case "FileExtension":
                    fileExtensionsLst.Add(c.Text);
                    break;
                case "Client":
                    fileAndLogClientLst.Add(myAppData.AuthorizedUser.User.CurrentClientGroup.Clients.Single(x => x.ClientName.Equals(c.Text)));
                    break;
            }

        }

        private void radComboUnChecked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            var p = cb.Parent;
            TextBlock c = null;
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(p); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(p, i);
                if (child is TextBlock)
                    c = child as TextBlock;
            }
            //var p2 = myAppData.AuthorizedUser.User.CurrentClientGroup.Clients.Single(x => x.ClientName.Equals(c.Text));
            switch (c.Tag.ToString())
            {
                case "FileStatus":
                    fileStatusesLst.Remove(c.Text);
                    break;
                case "FileExtension":
                    fileExtensionsLst.Remove(c.Text);
                    break;
                case "Client":
                    //fileAndLogClientLst.Remove(clientData.Select(c.Text));
                    fileAndLogClientLst.RemoveAll(x => x.ClientName.Equals(c.Text));
                    break;
            }
        }

        private void clientListReports_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (onReports)
            {
                RadComboBox rcb = sender as RadComboBox;
                KMPlatform.Entity.Client client = myAppData.AuthorizedUser.User.CurrentClientGroup.Clients.Single(x => x.ClientName.Equals(rcb.SelectedItem.ToString()));
                //fileNameReports.ItemsSource = client.SourceFilesList.Select(x => x.FileName);
                filePubCodeReports.IsEnabled = true;
                FrameworkUAD.BusinessLogic.PubCode pcData = new FrameworkUAD.BusinessLogic.PubCode();
                List<string> pubCodes = new List<string>();
                if (client.IsActive && !client.ClientName.Equals("SpecialityFoods") && !client.ClientName.Equals("UAStest") && !client.ClientName.Equals("Vance"))
                    pubCodes.AddRange(pcData.Select(client.ClientName + "MasterDB").Select(x => x.Pubcode));
                else if (client.ClientName.Equals("SpecialityFoods"))
                    pubCodes.AddRange(pcData.Select("NASFTMasterDB").Select(x => x.Pubcode));
                filePubCodeReports.ItemsSource = pubCodes;
                rbtnGenerateReports.Click += generateReportsClick;
            }
        }

        private void fileNameReports_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //fileTypeReports.IsEnabled = false;
            //suppressNo.IsEnabled = false;
            //suppressYes.IsEnabled = false;
            //fileStatus.IsEnabled = false;
            //filePubCodeReports.IsEnabled = false;
            //fileExtensionReports.IsEnabled = false;
            //DateFromReports.IsEnabled = false;
            //DateToReports.IsEnabled = false;
        }

        private void suppressYes_Checked(object sender, RoutedEventArgs e)
        {
            suppressNo.IsChecked = false;
        }

        private void suppressNo_Checked(object sender, RoutedEventArgs e)
        {
            suppressYes.IsChecked = false;
        }

        private void btn_ResetReports(object sender, RoutedEventArgs e)
        {
            List<string> extensions = new List<string>() { ".dbf", ".xlsx", ".csv", ".txt", ".xls" };
            List<string> fileTypes = new List<string>() { "Standard", "Special" };

            clientListReports.SelectionChanged -= clientListReports_SelectionChanged;
            clientListReports.SelectedItem = null;
            clientListReports.SelectionChanged += clientListReports_SelectionChanged;
            fileNameReports.SelectedItem = null;
            fileTypeReports.SelectedItem = null;
            fileTypeReports.ItemsSource = fileTypes;
            suppressNo.IsChecked = false;
            suppressYes.IsChecked = false;
            fileStatusesLst.Clear();
            fileStatusReports.ItemsSource = null;
            fileStatusReports.ItemsSource = allFST;
            filePubCodeReports.SelectedItem = null;
            fileExtensionsLst.Clear();
            fileExtensionReports.ItemsSource = extensions;
            DateFromReports.SelectedDate = null;
            DateToReports.SelectedDate = null;
            rbtnGenerateReports.Click -= generateReportsClick;
            saveReport.Visibility = System.Windows.Visibility.Collapsed;
        }
        #endregion

        #region Creating Document
        string clientName = "";
        List<FrameworkUAS.Entity.SourceFile> sFiles = new List<FrameworkUAS.Entity.SourceFile>();
        //List<FrameworkUAS.Entity.FileStatus> fileStatuses = new List<FrameworkUAS.Entity.FileStatus>();
        List<FrameworkUAS.Entity.FileLog> logs = new List<FrameworkUAS.Entity.FileLog>();
        //int filesRun = 0;
        int completed = 0;
        int totalRows = 0;
        string currentStatus = "";
        string fileType = "";
        private void generateReportsClick(object sender, RoutedEventArgs e)
        {
            if (rbDocument.IsChecked.Value)
            {
                ParseParseParse();
                RadDocument document = new RadDocument();
                richTxtBoxReports.IsSpellCheckingEnabled = false;
                richTxtBoxReports.LayoutMode = Telerik.Windows.Documents.Model.DocumentLayoutMode.Flow;
                Section section = new Section();
                Paragraph paragraph1 = new Paragraph();
                Stream stream = Application.GetResourceStream(new Uri(@"pack://application:,,,/ImageLibrary;component/Images/Brands/KM_Logo.png", UriKind.RelativeOrAbsolute)).Stream;
                ImageInline imageInline = new ImageInline(stream, new Size(200, 81), "png");
                paragraph1.Inlines.Add(imageInline);

                TableRow row1 = new TableRow();
                //TableRow row2 = new TableRow();
                TableCell cell1 = new TableCell();
                TableCell cell2 = new TableCell();
                Paragraph p1 = new Paragraph();
                Paragraph p2 = new Paragraph();
                //Span s1 = new Span();
                //Span s2 = new Span();

                #region HeaderArea
                Span span1 = new Span("UAD Report");
                span1.FontWeight = System.Windows.FontWeights.Bold;
                p2.Inlines.Add(span1);
                if (DateFromReports.SelectedDate != null && DateToReports.SelectedDate != null)
                {
                    p2.Inlines.Add(new Span(FormattingSymbolLayoutBox.LINE_BREAK));
                    p2.Inlines.Add(new Span { Text = "For  ", FontStyle = System.Windows.FontStyles.Italic });
                    p2.Inlines.Add(new Span(DateFromReports.SelectedDate.Value.ToShortDateString().ToString()));
                    p2.Inlines.Add(new Span { Text = "  To  ", FontStyle = System.Windows.FontStyles.Italic });
                    p2.Inlines.Add(new Span(DateToReports.SelectedDate.Value.ToShortDateString().ToString()));
                }
                if (clientListReports.SelectedItem != null)
                {
                    p2.Inlines.Add(new Span(FormattingSymbolLayoutBox.LINE_BREAK));
                    p2.Inlines.Add(new Span { Text = "Client: ", FontStyle = System.Windows.FontStyles.Italic });
                    p2.Inlines.Add(new Span(clientListReports.SelectedItem.ToString()));
                }
                Table headerTable = new Table();
                headerTable.Borders.SetBottom(new Telerik.Windows.Documents.Model.Border(1, BorderStyle.Single, Colors.Black));
                headerTable.PreferredWidth = new TableWidthUnit(500);
                row1 = new TableRow();
                cell1 = new TableCell();
                cell2 = new TableCell();
                cell1.Blocks.Add(paragraph1);
                cell2.Blocks.Add(p2);
                row1.Cells.Add(cell1);
                row1.Cells.Add(cell2);
                headerTable.CellSpacing = 0;
                headerTable.CellPadding = new Telerik.Windows.Documents.Layout.Padding(0);
                headerTable.Rows.Add(row1);

                section.Blocks.Add(headerTable);
                //document.Sections.Add(section);
                #endregion

                #region BodyArea
                p1 = new Paragraph();
                //section = new Section();
                //sFiles = sFiles.Where(x => fileStatuses.Select(y => y.SourceFileID).ToList().Contains(x.SourceFileID)).ToList();
                p1.Inlines.Add(new Span(FormattingSymbolLayoutBox.LINE_BREAK));
                p1.Inlines.Add(new Span { Text = "Client Summary" + FormattingSymbolLayoutBox.LINE_BREAK, UnderlineDecoration = Telerik.Windows.Documents.UI.TextDecorations.DecorationProviders.UnderlineTypes.Line });
                //p1.Inlines.Add(new Span("Total Files Run: " + fileStatuses.Count.ToString() + FormattingSymbolLayoutBox.LINE_BREAK));
                p1.Inlines.Add(new Span("Files Completed: " + completed + FormattingSymbolLayoutBox.LINE_BREAK + FormattingSymbolLayoutBox.LINE_BREAK));
                foreach (FrameworkUAS.Entity.SourceFile sf in sFiles)
                {
                    ParseLogMessage(sf);
                    p1.Inlines.Add(new Span { Text = "File Name: ", FontWeight = System.Windows.FontWeights.Bold });
                    p1.Inlines.Add(new Span(sf.FileName + FormattingSymbolLayoutBox.LINE_BREAK));
                    p1.Inlines.Add(new Span(FormattingSymbolLayoutBox.TAB + "Original Row Count: " + totalRows + FormattingSymbolLayoutBox.LINE_BREAK));
                    p1.Inlines.Add(new Span(FormattingSymbolLayoutBox.TAB + "Current Status: " + currentStatus + FormattingSymbolLayoutBox.LINE_BREAK));
                    p1.Inlines.Add(new Span(FormattingSymbolLayoutBox.TAB + "File Type: " + fileType + FormattingSymbolLayoutBox.LINE_BREAK));
                }
                section.Blocks.Add(p1);
                document.Sections.Add(section);
                richTxtBoxReports.Document = document;
                saveReport.Visibility = System.Windows.Visibility.Visible;
                #endregion
            }
        }

        private void saveReport_Click(object sender, RoutedEventArgs e)
        {
            //ExportContent(".pdf", "PDF File (*.pdf)|*.pdf", new PdfFormatProvider());


            // exports to HTML file format
            //ExportContent(".html", "HTML File (*.html)|*.html", new HtmlFormatProvider());

            //// exports to PDF file format
            //ExportContent(".pdf", "PDF File (*.pdf)|*.pdf", new PdfFormatProvider());

            //// exports to RTF file format
            //ExportContent(".rtf", "RTF File (*.rtf)|*.rtf", new RtfFormatProvider());

            //// exports to DOCX file format
            //ExportContent(".docx", "DOCX File (*.docx)|*.docx", new DocxFormatProvider());

            //// exports to XAML file format
            //ExportContent(".xaml", "XAML File (*.xaml)|*.xaml", new XamlFormatProvider());

            //// exports to TXT file format
            //ExportContent(".txt", "Text File (*.txt)|*.txt", new TxtFormatProvider());


            //PdfFormatProvider provider = new PdfFormatProvider();
            //SaveFileDialog saveDialog = new SaveFileDialog();
            //saveDialog.DefaultExt = ".pdf";
            //saveDialog.Filter = "Documents|*.pdf";
            //saveDialog.FileName = DateTime.Now.ToShortDateString() + "_UADReport";
            //saveDialog.FileName = saveDialog.FileName.Replace("/", "_");
            //bool? dialogResult = saveDialog.ShowDialog();
            //if (dialogResult == true)
            //{
            //    using (Stream output = saveDialog.OpenFile())
            //    {
            //        provider.Export(richTxtBoxReports.Document, output);
            //        MessageBox.Show("Saved Successfuly!");
            //    }
            //}
        }
        public void ExportContent(string defaultExtension, string filter, IDocumentFormatProvider formatProvider)
        {
            var saveFileDialog = new SaveFileDialog
            {
                DefaultExt = defaultExtension,
                Filter = filter
            };

            var dialogResult = saveFileDialog.ShowDialog();
            if (dialogResult == true)
            {
                using (var outputStream = saveFileDialog.OpenFile())
                {
                    formatProvider.Export(richTxtBoxReports.Document, outputStream);
                }
            }
        }
        private void ParseParseParse()
        {
            clientName = clientListReports.SelectedItem.ToString();
            KMPlatform.Entity.Client client = myAppData.AuthorizedUser.User.CurrentClientGroup.Clients.Single(x => x.ClientName.Equals(clientName));
            //List<FrameworkUAS.Entity.SourceFile> originalSFList = client.SourceFilesList;
            //sFiles = originalSFList.ToList();
            //List<FrameworkUAS.Entity.FileStatus> originalFSList = fsData.SelectClient(client.ClientID);
            //fileStatuses = originalFSList.ToList();
            //if(fileStatusesLst.Count > 0)
            //{
            //    fileStatuses.Clear();
            //    foreach(string s in fileStatusesLst)
            //    {
            //        int typeID = allFSTypes.Where(x=> x.CodeName == s).Select(x => x.CodeId).FirstOrDefault();
            //        fileStatuses.AddRange(originalFSList.Where(x => x.FileStatusTypeID == typeID));
            //    }
            //}
            //if (fileExtensionsLst.Count > 0)
            //{
            //    sFiles.Clear();
            //    foreach (string s in fileExtensionsLst)
            //    {
            //        //sFiles.AddRange(originalSFList.Where(x => x.Extension.IndexOf(s, StringComparison.CurrentCultureIgnoreCase) != -1));
            //    }
            //    fileStatuses = fileStatuses.Where(x => sFiles.Select(y => y.SourceFileID).ToList().Contains(x.SourceFileID)).ToList();
            //}
            //if (DateFromReports.SelectedDate != null && DateToReports.SelectedDate != null)
            //{
            //    sFiles = sFiles.Where(x => x.DateCreated <= DateToReports.SelectedDate && x.DateCreated >= DateFromReports.SelectedDate).ToList();
            //    fileStatuses = fileStatuses.Where(x => x.DateCreated <= DateToReports.SelectedDate && x.DateCreated >= DateFromReports.SelectedDate).ToList();
            //}
            //if(fileTypeReports.SelectedItem != null)
            //{
            //    bool isSpecial = false;
            //    if(fileTypeReports.SelectedItem.Equals("Special"))
            //        isSpecial = true;
            //    sFiles = sFiles.Where(x => x.IsSpecialFile == isSpecial).ToList();
            //    sFiles = sFiles.Where(x => fileStatuses.Select(y => y.SourceFileID).ToList().Contains(x.SourceFileID)).ToList();
            //    fileStatuses = fileStatuses.Where(x => sFiles.Select(y => y.SourceFileID).ToList().Contains(x.SourceFileID)).ToList();
            //}
            //if(fileNameReports.SelectedItem != null)
            //{
            //    int sfID = sFiles.Where(x => x.FileName.Equals(fileNameReports.SelectedItem.ToString())).Select(x => x.SourceFileID).FirstOrDefault();
            //    fileStatuses = fileStatuses.Where(x => x.SourceFileID == sfID).ToList();
            //}
            //FrameworkUAD_Lookup.Entity.Code fsType = fsTypeData.SelectCodeName(FrameworkUAD_Lookup.Enums.CodeType.File_Status, FrameworkUAD_Lookup.Enums.FileStatusTypes.Processed.ToString());
            //completed = fileStatuses.Where(x=> x.FileStatusTypeID == fsType.CodeId).Count();
            logs = logData.SelectClient(client.ClientID);
        }

        private void ParseLogMessage(FrameworkUAS.Entity.SourceFile sf)
        {
            totalRows = 0;
            FrameworkUAS.Entity.FileLog fileLog = logs.Where(x => x.SourceFileID == sf.SourceFileID && x.Message.Contains("total rows")).OrderByDescending(x => x.LogDate).ThenByDescending(x => x.LogTime).FirstOrDefault();
            if (fileLog != null)
            {
                string[] msgs = fileLog.Message.Split(":");
                totalRows = Int32.Parse(msgs[3]);
            }
            //string clientName = clientListReports.SelectedItem.ToString();
            //totalRows = subOriginalData.Select(sf.SourceFileID, clientData.Select(clientName)).Count;
            int typeID = 0;// fileStatuses.Where(x => x.SourceFileID == sf.SourceFileID).Select(x => x.FileStatusTypeID).FirstOrDefault();
            if (typeID == 0)
                currentStatus = "N/A";
            else
                currentStatus = fsTypeData.SelectCodeId(typeID).CodeName;//fsTypeData.Select(typeID).FileStatusName;
            if (!sf.IsSpecialFile)
                fileType = "Standard";
            else
                fileType = "Special";
        }
        #endregion

        #region Creating Grid
        
        #endregion

        #endregion

        #region Graphs
        private void rbGraph_Click(object sender, RoutedEventArgs e)
        {
            if (onGraphs == false)
            {
                w = new BackgroundWorker();
                LoadAnimations();
                disableMenuButtons(true);
                onGraphs = true;
                resetRecWidth.Completed += (s, _) => disableMenuButtons(false);
                resetOpacity.Completed += (s, _) => swipeRec.BeginAnimation(Rectangle.WidthProperty, resetRecWidth);
                recWidthToZero.Completed += (s, _) => swipeRec.BeginAnimation(Rectangle.OpacityProperty, resetOpacity);
                animateOpacity.Completed += (s, _) => swipeRec.BeginAnimation(Rectangle.WidthProperty, recWidthToZero);
                animateRec.Completed += (l, b) =>
                {
                    UAD_Explorer.Windows.BusyIndicator s = new UAD_Explorer.Windows.BusyIndicator();
                    s.RadBusyIndicator.IsBusy = true;
                    s.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    s.Dispatcher.BeginInvoke(new Action(() =>
                {
                    w.WorkerSupportsCancellation = true;
                    w.DoWork += new DoWorkEventHandler(worker_DoWorkGraphs);
                    w.RunWorkerCompleted += delegate
                    {
                        callClear();
                        currentPage = "Graphs";
                        s.Close();
                        loadGraphsPage();
                        swipeRec.BeginAnimation(Rectangle.OpacityProperty, animateOpacity);
                    };
                    w.RunWorkerAsync();
                }));
                    s.ShowDialog();
                };
                swipeRec.BeginAnimation(Rectangle.WidthProperty, animateRec);
            }
        }

        #region Background Work
        ObservableCollection<TestData> whoopwhoop = new ObservableCollection<TestData>();
        Telerik.Windows.Controls.ChartView.AreaSeries fileHistory = new Telerik.Windows.Controls.ChartView.AreaSeries();
        public class TestData
        {
            public TestData(int fileCount, DateTime? currentTime)
            {
                this.FileCount = fileCount;
                this.CurrentDay = currentTime;
            }

            private int count;
            public int FileCount
            {
                get { return count; }
                set { count = value; }
            }
            private DateTime? time;
            public DateTime? CurrentDay
            {
                get { return time; }
                set { time = value; }
            }
        }

        public void worker_DoWorkGraphs(object sender, DoWorkEventArgs e)
        {
            Random r = new Random();
            whoopwhoop = new ObservableCollection<TestData>();
            for (int i = 0; i < 300; i++)
            {
                int l = r.Next(1, 50);
                //CategoricalDataPoint p = new CategoricalDataPoint();
                whoopwhoop.Add(new TestData(l, DateTime.Now.AddDays(i)));
            }
        }

        private void loadGraphsPage()
        {
            //var t = new Uri("/MapLibrary;component/Maps/world_continents.shp", UriKind.RelativeOrAbsolute);
            mapShapeDataReader.Source = new Uri("/MapLibrary;component/Maps/world_continents.shp", UriKind.RelativeOrAbsolute);
            mapShapeDataReader.DataSource = new Uri(@"/MapLibrary;component/Maps/world_continents.dbf", UriKind.RelativeOrAbsolute);
            GraphsPage.Visibility = System.Windows.Visibility.Visible;
            sparkline.ItemsSource = whoopwhoop;
            timeBar.Content = sparkline;
            liveChartGraphs.Series.Add(fileHistory);
            rcbClientListGraphs.ItemsSource = myAppData.AuthorizedUser.User.CurrentClientGroup.Clients.Select(x => x.ClientName).ToList();
            txtBlockClientGraphs.Text = GraphPopOut.client;
            percentageCompleteRecords.Text = ((rdbUADRecordsActual.Value / rdbUADRecordsTarget.Value) * 100).ToString() + "%";
            w.CancelAsync();
            w.Dispose();
            w = null;
            GC.Collect();
        }
        #endregion

        #region UIChanges

        private void timeBar_SelectionChanged(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (onGraphs)
            {
                RadTimeBar me = e.Source as RadTimeBar;
                List<TestData> tempList = new List<TestData>();
                liveChartGraphs.Series.Clear();
                if (fileHistory.DataPoints.Count > 0)
                    fileHistory.DataPoints.Clear();
                tempList = whoopwhoop.Where(x => (x.CurrentDay).Value.DayOfYear >= me.SelectionStart.DayOfYear && (x.CurrentDay).Value.DayOfYear <= me.SelectionEnd.DayOfYear).ToList();
                foreach (TestData t in tempList)
                {
                    CategoricalDataPoint p = new CategoricalDataPoint { Category = t.CurrentDay.Value.ToShortDateString(), Value = t.FileCount, Label = t.FileCount };
                    fileHistory.DataPoints.Add(p);
                }
                fileHistory.ShowLabels = false;
                fileHistory.Fill = ((SolidColorBrush)(new BrushConverter().ConvertFrom("#FF045DA4")));
                fileHistory.PointTemplate = this.FindResource("EllipseTemplate") as DataTemplate;
                liveChartGraphs.Series.Add(fileHistory);
            }
        }

        private void rcbClientListGraphs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (onGraphs)
            {
                RadComboBox rcb = sender as RadComboBox;
                txtBlockClientGraphs.Text = rcb.SelectedItem.ToString();
            }
        }

        private void VisualizationLayer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            VisualizationLayer mapLayer = sender as VisualizationLayer;
            if(mapLayer.SelectedItems.Count > 1)
                mapLayer.SelectedItems.RemoveAt(0);
            if (e.AddedItems.Count > 0 && (e.AddedItems[0].GetType().Name.ToString().Equals("PathData") || e.AddedItems[0].GetType().Name.ToString().Equals("PolygonData")))
            {
                Object p = new object();
                string s = e.AddedItems[0].GetType().Name.ToString();

                if (s.Equals("PolygonData"))
                    p = e.AddedItems[0] as PolygonData;
                else
                    p = e.AddedItems[0] as PathData;

                PropertyInfo t = p.GetType().GetProperty("ExtendedData");
                ExtendedData data = t.GetValue(p, null) as ExtendedData;
                string region = "";

                switch (RadMap1.Tag.ToString())
                {
                    case "World":

                        region = data.GetValue("CONTINENT").ToString();

                        switch (region)
                        {
                            case "North America":
                                mapShapeDataReader.Source = new Uri("C:\\Projects\\Unified Audience System\\Platform Services\\Branches\\Dev\\2014_Q3\\MapLibrary\\Maps\\north_america.shp");
                                mapShapeDataReader.DataSource = new Uri("C:\\Projects\\Unified Audience System\\Platform Services\\Branches\\Dev\\2014_Q3\\MapLibrary\\Maps\\north_america.dbf");
                                //mapShapeDataReader = new AsyncShapeFileReader();
                                //mapShapeDataReader.DataSource = new Uri(@"pack://application:,,,/MapLibrary;component/Maps/sm_cntry.dbf", UriKind.RelativeOrAbsolute);
                                //mapShapeDataReader.Source = new Uri(@"pack://application:,,,/MapLibrary;component/Maps/sm_cntry.shp", UriKind.RelativeOrAbsolute);
                                RadMap1.Tag = "Country";
                                mapShapeDataReader.ToolTipFormat = "CNTRY_NAME";
                                RegionName.Text = region;
                                break;

                            case "South America":
                                //mapShapeDataReader.Source = new Uri("C:\\Users\\Nick.Nelson\\Downloads\\Demos\\Examples\\Map\\Resources\\south_america.shp");
                                //mapShapeDataReader.DataSource = new Uri("C:\\Users\\Nick.Nelson\\Downloads\\Demos\\Examples\\Map\\Resources\\south_america.dbf");
                                //RadMap1.Tag = "Country";
                                //mapShapeDataReader.ToolTipFormat = "CNTRY_NAME";
                                RegionName.Text = region;
                                break;

                            case "Africa":
                                //mapShapeDataReader.Source = new Uri("C:\\Users\\Nick.Nelson\\Downloads\\Demos\\Examples\\Map\\Resources\\africa.shp");
                                //mapShapeDataReader.DataSource = new Uri("C:\\Users\\Nick.Nelson\\Downloads\\Demos\\Examples\\Map\\Resources\\africa.dbf");
                                //RadMap1.Tag = "Country";
                                //mapShapeDataReader.ToolTipFormat = "CNTRY_NAME";
                                RegionName.Text = region;
                                break;

                            case "Australia":
                                //mapShapeDataReader.Source = new Uri("C:\\Users\\Nick.Nelson\\Downloads\\Demos\\Examples\\Map\\Resources\\australia.shp");
                                //mapShapeDataReader.DataSource = new Uri("C:\\Users\\Nick.Nelson\\Downloads\\Demos\\Examples\\Map\\Resources\\australia.dbf");
                                //RadMap1.Tag = "Country";
                                //mapShapeDataReader.ToolTipFormat = "CNTRY_NAME";
                                RegionName.Text = region;
                                break;

                            case "Oceania":
                                //mapShapeDataReader.Source = new Uri("C:\\Users\\Nick.Nelson\\Downloads\\Demos\\Examples\\Map\\Resources\\oceania.shp");
                                //mapShapeDataReader.DataSource = new Uri("C:\\Users\\Nick.Nelson\\Downloads\\Demos\\Examples\\Map\\Resources\\oceania.dbf");
                                //RadMap1.Tag = "Country";
                                //mapShapeDataReader.ToolTipFormat = "CNTRY_NAME";
                                RegionName.Text = region;
                                break;

                            case "Asia":
                                //mapShapeDataReader.Source = new Uri("C:\\Users\\Nick.Nelson\\Downloads\\Demos\\Examples\\Map\\Resources\\asia.shp");
                                //mapShapeDataReader.DataSource = new Uri("C:\\Users\\Nick.Nelson\\Downloads\\Demos\\Examples\\Map\\Resources\\asia.dbf");
                                //RadMap1.Tag = "Country";
                                //mapShapeDataReader.ToolTipFormat = "CNTRY_NAME";
                                RegionName.Text = region;
                                break;

                            case "Europe":
                                //mapShapeDataReader.Source = new Uri("C:\\Users\\Nick.Nelson\\Downloads\\Demos\\Examples\\Map\\Resources\\eruope.shp");
                                //mapShapeDataReader.DataSource = new Uri("C:\\Users\\Nick.Nelson\\Downloads\\Demos\\Examples\\Map\\Resources\\eruope.dbf");
                                //RadMap1.Tag = "Country";
                                //mapShapeDataReader.ToolTipFormat = "CNTRY_NAME";
                                RegionName.Text = region;
                                break;
                        }
                        break;

                    case "Country":

                        region = data.GetValue("CNTRY_NAME").ToString();
                        switch (region)
                        {
                            case "United States":
                                mapShapeDataReader.Source = new Uri("C:\\Projects\\Unified Audience System\\Platform Services\\Branches\\Dev\\2014_Q3\\MapLibrary\\Maps\\usa_states.shp", UriKind.RelativeOrAbsolute);
                                mapShapeDataReader.DataSource = new Uri("C:\\Projects\\Unified Audience System\\Platform Services\\Branches\\Dev\\2014_Q3\\MapLibrary\\Maps\\usa_states.dbf", UriKind.RelativeOrAbsolute);
                                RegionName.Text = region;
                                RadMap1.Tag = "State";
                                RadMap1.Center = new Location(39, -98);
                                RadMap1.ZoomLevel = 3;
                                mapShapeDataReader.ToolTipFormat = "STATE_NAME";
                                break;

                            case "Canada":
                                RegionName.Text = region;
                                break;
                        }
                        break;

                    case "State":
                        region = data.GetValue("STATE_NAME").ToString();
                        string stateName = region;
                        RegionName.Text = stateName;
                        break;
                }
            }
        }

        #endregion

        private void mapBack_MouseUp(object sender, MouseButtonEventArgs e)
        {
            switch (RadMap1.Tag.ToString())
            {
                case "Country":
                    RadMap1.Tag = "World";
                    RadMap1.ZoomLevel = 1;
                    RadMap1.Center = new Location(65, -20);
                    mapShapeDataReader.Source = new Uri("C:\\Projects\\Unified Audience System\\Platform Services\\Branches\\Dev\\2014_Q3\\MapLibrary\\Maps\\world_continents.shp");
                    mapShapeDataReader.DataSource = new Uri("C:\\Projects\\Unified Audience System\\Platform Services\\Branches\\Dev\\2014_Q3\\MapLibrary\\Maps\\world_continents.dbf");
                    
                    //mapShapeDataReader.Source = new Uri("/MapLibrary;Component/Maps/world_continents.shp");
                    //mapShapeDataReader.Source = new Uri(@"/ADMS_Dashboard;component/Maps/world_continents.shp", UriKind.RelativeOrAbsolute);
                    //mapShapeDataReader.DataSource = new Uri(@"/ADMS_Dashboard;component/Maps/world_continents.dbf", UriKind.RelativeOrAbsolute);
                    mapShapeDataReader.ToolTipFormat = "CONTINENT";
                    break;
                case "State":
                    RadMap1.Tag = "Country";
                    RadMap1.ZoomLevel = 2;
                    mapShapeDataReader.Source = new Uri("C:\\Projects\\Unified Audience System\\Platform Services\\Branches\\Dev\\2014_Q3\\MapLibrary\\Maps\\north_america.shp");
                    mapShapeDataReader.DataSource = new Uri("C:\\Projects\\Unified Audience System\\Platform Services\\Branches\\Dev\\2014_Q3\\MapLibrary\\Maps\\north_america.dbf");
                    mapShapeDataReader.ToolTipFormat = "CNTRY_NAME";
                    break;
            }
        }

        public void clearGraphs()
        {
            onGraphs = false;
            whoopwhoop.Clear();
            liveChartGraphs.Series.Clear();
            visualizationLayer.SelectedItems.Clear();
            RegionName.Text = "";
            GraphsPage.Visibility = Visibility.Collapsed;
            rcbClientListGraphs.SelectedItem = null;
            txtBlockClientGraphs.Text = "";
        }

        #endregion

        #region FileLogs
        private void rbLogs_Click(object sender, RoutedEventArgs e)
        {
            if (onLogs == false)
            {
                w = new BackgroundWorker();
                LoadAnimations();
                disableMenuButtons(true);
                onLogs = true;
                resetRecWidth.Completed += (s, _) => disableMenuButtons(false);
                resetOpacity.Completed += (s, _) => swipeRec.BeginAnimation(Rectangle.WidthProperty, resetRecWidth);
                recWidthToZero.Completed += (s, _) => swipeRec.BeginAnimation(Rectangle.OpacityProperty, resetOpacity);
                animateOpacity.Completed += (s, _) => swipeRec.BeginAnimation(Rectangle.WidthProperty, recWidthToZero);
                animateRec.Completed += (l, b) =>
                {
                    UAD_Explorer.Windows.BusyIndicator s = new UAD_Explorer.Windows.BusyIndicator();
                    s.RadBusyIndicator.IsBusy = true;
                    s.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    s.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        w.WorkerSupportsCancellation = true;
                        w.DoWork += new DoWorkEventHandler(worker_DoWorkLogs);
                        w.RunWorkerCompleted += delegate
                        {
                            callClear();
                            currentPage = "Logs";
                            s.Close();
                            loadLogsPage();
                            swipeRec.BeginAnimation(Rectangle.OpacityProperty, animateOpacity);
                        };
                        w.RunWorkerAsync();
                    }));
                    s.ShowDialog();
                };
                swipeRec.BeginAnimation(Rectangle.WidthProperty, animateRec);
            }
        }

        #region BackGround Work
        List<RadTreeViewItem> newLogItems = new List<RadTreeViewItem>();
        RadTreeViewItem newHeader = new RadTreeViewItem();
        RadTreeViewItem childItem = new RadTreeViewItem();
        public void worker_DoWorkLogs(object sender, DoWorkEventArgs e)
        {
            if (logsFirstRun == true)
            {
                foreach (KMPlatform.Entity.Client client in myAppData.AuthorizedUser.User.CurrentClientGroup.Clients)
                {
                    List<int> SFileIDs = logData.SelectClient(client.ClientID).GroupBy(p => p.SourceFileID).Select(g => g.First().SourceFileID).ToList();
                    if (SFileIDs.Count > 0)
                    {
                        this.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            newHeader = new RadTreeViewItem { Header = client.ClientName, Tag = client.ClientID };
                        }));
                        foreach (int id in SFileIDs)
                        {
                            //string fileName = client.SourceFilesList.Single(x => x.SourceFileID == id).FileName;
                            this.Dispatcher.BeginInvoke(new Action(() =>
                                {
                                    //childItem = new RadTreeViewItem { Header = fileName, Tag = id };
                                    newHeader.Items.Add(childItem);
                                }));
                        }
                        this.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            newHeader.Items.SortDescriptions.Add(new SortDescription("Header", ListSortDirection.Ascending));
                            newLogItems.Add(newHeader);
                        }));
                    }
                }
            }
        }

        private void loadLogsPage()
        {
            LogsPage.Visibility = System.Windows.Visibility.Visible;
            foreach (RadTreeViewItem rtv in newLogItems)
                rtvLogsList.Items.Add(rtv);
            rcmbBoxLogClients.ItemsSource = allClients.Select(x => x.ClientName);
            logsFirstRun = false;
        }
        #endregion

        private void rtvLogsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (onLogs && e.AddedItems.Count > 0)
            {
                RadTreeViewItem rtvItem = e.AddedItems[0] as RadTreeViewItem;
                if (rtvItem.Parent != null && !rtvItem.Parent.GetType().Name.ToString().Equals("RadTreeView"))
                {
                    RadDocument logDocument = new RadDocument();
                    richTxtBoxReports = new RadRichTextBox();
                    richTxtBoxReports.IsSpellCheckingEnabled = false;
                    richTxtBoxReports.LayoutMode = Telerik.Windows.Documents.Model.DocumentLayoutMode.Flow;
                    Section section = new Section();
                    Paragraph p1 = new Paragraph();

                    KMPlatform.Entity.Client client = myAppData.AuthorizedUser.User.CurrentClientGroup.Clients.Single(x => x.ClientName.Equals(rtvItem.Parent.ToString()));//clientData.Select(rtvItem.Parent.ToString());
                    //FrameworkUAS.Entity.SourceFile sFile = client.SourceFilesList.Single(x => x.FileName.Equals(rtvItem.Header.ToString()));//sfData.Select(rtvItem.Parent.ToString(), rtvItem.Header.ToString(), false);
                    //List<FrameworkUAS.Entity.FileLog> fileLogs = logData.SelectClient(client.ClientID).Where(x => x.SourceFileID == sFile.SourceFileID).ToList();

                    UAD_Explorer.Windows.BusyIndicator s = new UAD_Explorer.Windows.BusyIndicator();
                    s.RadBusyIndicator.IsBusy = true;
                    s.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    s.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        w = new BackgroundWorker();
                        w.WorkerSupportsCancellation = true;
                        w.DoWork += delegate
                        {
                            foreach (FrameworkUAS.Entity.FileLog log in fileLogs)
                            {
                                p1.Inlines.Add(new Span { Text = log.Message + FormattingSymbolLayoutBox.LINE_BREAK + FormattingSymbolLayoutBox.LINE_BREAK, FontSize = 10 });
                            }

                            section.Blocks.Add(p1);
                            this.Dispatcher.BeginInvoke(new Action(() =>
                                {
                                    logDocument.Sections.Add(section);
                                    richtxtBoxLogs.Document = logDocument;
                                    spFileLogViewer.Visibility = System.Windows.Visibility.Visible;
                                    rtvLogsList.IsEnabled = false;
                                }
                            ));
                        };
                        w.RunWorkerCompleted += delegate
                        {
                            s.BringIntoView();
                            this.Dispatcher.BeginInvoke(
                              DispatcherPriority.Loaded,
                              new Action(() =>
                              {
                                  s.Close();
                                  DoubleAnimation background = new DoubleAnimation
                                  {
                                      To = 0.2,
                                      Duration = TimeSpan.FromSeconds(.5)
                                  };
                                  gridBackGround.Visibility = System.Windows.Visibility.Visible;
                                  gridBackGround.BeginAnimation(Window.OpacityProperty, background);
                              }));
                            //System.Windows.Forms.Application.DoEvents();
                        };
                        w.RunWorkerAsync();
                    }));
                    s.ShowDialog();
                    s.BringIntoView();
                }
            }
        }

        #region ButtonsCheckBoxes

        private void FileLogMenuClick(object sender, MouseButtonEventArgs e)
        {
            Image img = sender as Image;
            switch (img.Tag.ToString())
            {
                case "Search":
                    FileLogMenuOptions options = new FileLogMenuOptions(img.Tag.ToString());
                    string searchValue = "";
                    options.Check += value => searchValue = value;
                    options.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    options.ShowDialog();

                    if (searchValue != "")
                    {
                        this.richtxtBoxLogs.Document.Selection.Clear(); // this clears the selection before processing
                        DocumentTextSearch search = new DocumentTextSearch(this.richtxtBoxLogs.Document);
                        foreach (var textRange in search.FindAll(searchValue))
                        {
                            this.richtxtBoxLogs.Document.Selection.AddSelectionStart(textRange.StartPosition);
                            this.richtxtBoxLogs.Document.Selection.AddSelectionEnd(textRange.EndPosition);
                        }
                    }
                    break;

                case "Save":
                    TxtFormatProvider provider = new TxtFormatProvider();
                    SaveFileDialog saveDialog = new SaveFileDialog();
                    saveDialog.DefaultExt = ".txt";
                    saveDialog.Filter = "Documents|*.txt";
                    saveDialog.FileName = rtvLogsList.SelectedItem.ToString() + "_" + DateTime.Now.ToShortDateString() + "_FileLog";
                    saveDialog.FileName = saveDialog.FileName.Replace("/", "_");
                    bool? dialogResult = saveDialog.ShowDialog();
                    if (dialogResult == true)
                    {
                        using (Stream output = saveDialog.OpenFile())
                        {
                            provider.Export(richtxtBoxLogs.Document, output);
                            MessageBox.Show("Saved Successfuly!");
                        }
                    }
                    break;
                case "Close":
                    //spFileLogViewer.Visibility = System.Windows.Visibility.Collapsed;
                    DoubleAnimation backgroundFadeIn = new DoubleAnimation
                    {
                        To = 0,
                        Duration = TimeSpan.FromSeconds(.5)
                    };

                    gridBackGround.BeginAnimation(Rectangle.OpacityProperty, backgroundFadeIn);
                    gridBackGround.Visibility = System.Windows.Visibility.Collapsed;
                    spFileLogViewer.Visibility = System.Windows.Visibility.Collapsed;
                    rtvLogsList.IsEnabled = true;
                    break;
                case "Print":
                    PrintSettings settings = new PrintSettings()
                    {
                        DocumentName = rtvLogsList.SelectedItem.ToString() + "_" + DateTime.Now.ToShortDateString() + "_FileLog",
                        PrintMode = PrintMode.Native,
                        PrintScaling = PrintScaling.None,
                        //UseDefaultPrinter = true,
                        //PageRange = new PageRange(2, 4)
                    };
                    this.richtxtBoxLogs.Print(settings);
                    break;
            }

        }

        private void FileLogsCheckUnCheckBox(object sender, RoutedEventArgs e)
        {
            CheckBox cBox = sender as CheckBox;

            switch (cBox.Tag.ToString())
            {
                case "Errors":
                    cBoxNoErrors.IsChecked = false;
                    break;
                case "NoErrors":
                    cBoxErrors.IsChecked = false;
                    break;
            }
        }

        private void rBtnResetLogs_Click(object sender, RoutedEventArgs e)
        {
            rtvLogsList.ItemsSource = null;
            rtvLogsList.Items.Clear();
            foreach (RadTreeViewItem rtv in newLogItems)
                rtvLogsList.Items.Add(rtv);
            rcmbBoxLogClients.ItemsSource = allClients.Select(x => x.ClientName);
            fileAndLogClientLst.Clear();
            rcmbBoxLogClients.SelectedItem = null;
            cBoxErrors.IsChecked = false;
            cBoxNoErrors.IsChecked = false;
        }

        List<RadTreeViewItem> filteredItems = new List<RadTreeViewItem>();
        bool? errorsChecked = false;
        bool? noErrorsChecked = false;
        private void rBtnFilterLogs_Click(object sender, RoutedEventArgs e)
        {
            filteredItems.Clear();
            w = new BackgroundWorker();
            errorsChecked = cBoxErrors.IsChecked;
            noErrorsChecked = cBoxNoErrors.IsChecked;
            UAD_Explorer.Windows.BusyIndicator s = new UAD_Explorer.Windows.BusyIndicator();
            s.RadBusyIndicator.IsBusy = true;
            s.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            s.Dispatcher.BeginInvoke(new Action(() =>
            {
                w.WorkerSupportsCancellation = true;
                w.DoWork += delegate
                {
                        if (fileAndLogClientLst.Count == 0)
                            fileAndLogClientLst.AddRange(myAppData.AuthorizedUser.User.CurrentClientGroup.Clients);

                        foreach (KMPlatform.Entity.Client client in fileAndLogClientLst)
                        {
                            List<int> SFileIDs = logData.SelectClient(client.ClientID).GroupBy(p => p.SourceFileID).Select(g => g.First().SourceFileID).ToList();
                            if (SFileIDs.Count > 0)
                            {
                                this.Dispatcher.BeginInvoke(new Action(() =>
                                {
                                    newHeader = new RadTreeViewItem { Header = client.ClientName, Tag = client.ClientID };
                                }));
                                foreach (int id in SFileIDs)
                                {
                                    List<FrameworkUAS.Entity.FileLog> logs = logData.SelectClient(client.ClientID).Where(x => x.SourceFileID == id).ToList();
                                    if (errorsChecked == true || noErrorsChecked == true)
                                    {
                                        bool containsErrors = false;
                                        foreach (FrameworkUAS.Entity.FileLog log in logs)
                                        {
                                            if (log.Message.Contains("Target Site"))
                                                containsErrors = true;
                                        }
                                        if (errorsChecked == true)
                                        {
                                            if (containsErrors)
                                            {
                                                //string fileName = client.SourceFilesList.Single(x => x.SourceFileID == id).FileName;//sfData.SelectSourceFileID(id).FileName;
                                                this.Dispatcher.BeginInvoke(new Action(() =>
                                                {
                                                    //childItem = new RadTreeViewItem { Header = fileName, Tag = id };
                                                    newHeader.Items.Add(childItem);
                                                }));
                                            }
                                        }
                                        else if(noErrorsChecked == true)
                                        {
                                            if (!containsErrors)
                                            {
                                                //string fileName = client.SourceFilesList.Single(x => x.SourceFileID == id).FileName;
                                                this.Dispatcher.BeginInvoke(new Action(() =>
                                                {
                                                    //childItem = new RadTreeViewItem { Header = fileName, Tag = id };
                                                    newHeader.Items.Add(childItem);
                                                }));
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //string fileName = client.SourceFilesList.Single(x => x.SourceFileID == id).FileName;
                                        this.Dispatcher.BeginInvoke(new Action(() =>
                                        {
                                            //childItem = new RadTreeViewItem { Header = fileName, Tag = id };
                                            newHeader.Items.Add(childItem);
                                        }));
                                    }
                                }
                                this.Dispatcher.BeginInvoke(new Action(() =>
                                {
                                    newHeader.Items.SortDescriptions.Add(new SortDescription("Header", ListSortDirection.Ascending));
                                    filteredItems.Add(newHeader);
                                }));
                            }
                        }
                };
                w.RunWorkerCompleted += delegate
                {
                    fileAndLogClientLst.Clear();
                    rcmbBoxLogClients.ItemsSource = allClients.Select(x => x.ClientName);
                    this.rtvLogsList.Items.Clear();
                    this.rtvLogsList.ItemsSource = null;
                    foreach (RadTreeViewItem rtv in filteredItems)
                        this.rtvLogsList.Items.Add(rtv);
                    s.Close();
                };
            w.RunWorkerAsync();
            }));
            s.ShowDialog();
        }

        #endregion

        public void clearLogs()
        {
            onLogs = false;
            LogsPage.Visibility = Visibility.Collapsed;
            rtvLogsList.Items.Clear();
            cBoxErrors.IsChecked = false;
            cBoxNoErrors.IsChecked = false;
            gridBackGround.Visibility = System.Windows.Visibility.Collapsed;
            spFileLogViewer.Visibility = System.Windows.Visibility.Collapsed;
            rtvLogsList.IsEnabled = true;
        }

        #endregion

        #region SharedMethods
        private void LoadAnimations()
        {
            animateButtonWidth = new DoubleAnimation
            {
                To = 40,
                Duration = TimeSpan.FromSeconds(.4)
            };
            animateOpacity = new DoubleAnimation
            {
                To = 0,
                Duration = TimeSpan.FromSeconds(1)
            };
            resetOpacity = new DoubleAnimation
            {
                To = 1,
                Duration = TimeSpan.FromSeconds(.1)
            };
            resetRecWidth = new DoubleAnimation
            {
                To = 40,
                Duration = TimeSpan.FromSeconds(.4)
            };
            animateRec = new DoubleAnimation
            {
                To = 1035,
                Duration = TimeSpan.FromSeconds(1)
            };
            recWidthToZero = new DoubleAnimation
            {
                To = 0,
                Duration = TimeSpan.FromSeconds(.1)
            };
        }

        public void DoWork()
        {
            Thread.Sleep(100);
            UAD_Explorer.Windows.BusyIndicator s = new UAD_Explorer.Windows.BusyIndicator();
            s.RadBusyIndicator.IsBusy = true;
            s.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            s.ShowDialog();
        }

        public void callClear()
        {
            Type thisType = this.GetType();
            MethodInfo theMethod = thisType.GetMethod("clear" + currentPage);
            theMethod.Invoke(this, null);
        }

        public void disableMenuButtons(bool disable)
        {
            if(disable == true)
            {
                rbHome.Click -= rbHome_Click;
                rbReport.Click -= rbReport_Click;
                rbGraph.Click -= rbGraph_Click;
                rbLogs.Click -= rbLogs_Click;
            }
            else
            {
                rbHome.Click += rbHome_Click;
                rbReport.Click += rbReport_Click;
                rbGraph.Click += rbGraph_Click;
                rbLogs.Click += rbLogs_Click;
            }
        }
        #endregion

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            //string desktop = "";
            //desktop = ConfigurationManager.AppSettings["DashBoardViewer"].ToString();
            //if (!desktop.Equals("WEB"))
            //{
            //    Window b = Core.Utilities.WPF.GetMainWindow();
            //    ScrollViewer sv = b.FindName("mainScrollBar") as ScrollViewer;
            //    sv.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            //}
        }
    }

    //
    
//Now, this is a story all about how
//My life got flipped-turned upside down
//And I'd like to take a minute
//Just sit right there
//I'll tell you how I became the prince of a town called Bel Air

//In west Philadelphia born and raised
//On the playground was where I spent most of my days
//Chillin' out maxin' relaxin' all cool
//And all shootin some b-ball outside of the school
//When a couple of guys who were up to no good
//Started making trouble in my neighborhood
//I got in one little fight and my mom got scared
//She said 'You're movin' with your auntie and uncle in Bel Air'

//I begged and pleaded with her day after day
//But she packed my suit case and sent me on my way
//She gave me a kiss and then she gave me my ticket.
//I put my Walkman on and said, 'I might as well kick it'.

//First class, yo this is bad
//Drinking orange juice out of a champagne glass.
//Is this what the people of Bel-Air living like?
//Hmmmmm this might be alright.

//But wait I hear they're prissy, bourgeois, all that
//Is this the type of place that they just send this cool cat?
//I don't think so
//I'll see when I get there
//I hope they're prepared for the prince of Bel-Air

//Well, the plane landed and when I came out
//There was a dude who looked like a cop standing there with my name out
//I ain't trying to get arrested yet
//I just got here
//I sprang with the quickness like lightning, disappeared

//I whistled for a cab and when it came near
//The license plate said fresh and it had dice in the mirror
//If anything I could say that this cab was rare
//But I thought 'Nah, forget it' - 'Yo, homes to Bel Air'

//I pulled up to the house about 7 or 8
//And I yelled to the cabbie 'Yo homes smell ya later'
//I looked at my kingdom
//I was finally there
//To sit on my throne as the Prince of Bel Air

}
