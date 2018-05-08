using Core_AMS.Utilities;
using System;
using System.Linq;
using System.Windows;

namespace Circulation.Windows
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class PrintSaveDialog : Window
    {
        public event Action<Enums.DialogResponses> Answer;

        public PrintSaveDialog()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (Answer != null)
                Answer(Enums.DialogResponses.Save);

            this.Close();
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            if (Answer != null)
                Answer(Enums.DialogResponses.Print);

            this.Close();
        }
    }
}
