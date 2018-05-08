using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfControls.Reporting
{
    /// <summary>
    /// Interaction logic for ReportView.xaml
    /// </summary>
    public partial class ReportView : UserControl
    {
        private object MovingObject;
        //private double FirstXPos;
        //private double FirstYPos;
        //private double FirstArrowXPos;
        //private double FirstArrowYPos;
        private bool isDragging;
        public ReportView()
        {
            InitializeComponent();

            //TelerikReportViewer rpv = new TelerikReportViewer(1);
            //dpView.Children.Add(rpv);
        }

        private void cvMain_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //FirstXPos = e.GetPosition(sender as Control).X;
            //FirstYPos = e.GetPosition(sender as Control).Y;
            //Canvas c = new Canvas();
            //TextBlock co = sender as TextBlock;
            //if(co != null)
            //    c = co.Parent as Canvas;
            //if (co != null && c != null)
            //{
            //    FirstArrowXPos = e.GetPosition(c).X - FirstXPos;
            //    FirstArrowYPos = e.GetPosition(c).Y - FirstYPos;
            //    MovingObject = sender;
            //}
            //e.Handled = true;            
            tb.CaptureMouse();
            isDragging = true;
        }

        private void cvMain_MouseMove(object sender, MouseEventArgs e)
        {
            //if (e.LeftButton == MouseButtonState.Pressed)
            //{
            //    double t = e.GetPosition((MovingObject as FrameworkElement).Parent as FrameworkElement).Y;
            //    (MovingObject as FrameworkElement).SetValue(Canvas.LeftProperty, FirstXPos - e.GetPosition((MovingObject as FrameworkElement).Parent as FrameworkElement).X);
            //    (MovingObject as FrameworkElement).SetValue(Canvas.TopProperty, FirstYPos - e.GetPosition((MovingObject as FrameworkElement).Parent as FrameworkElement).Y);
            //}
            if (isDragging)
            {
                Point canvPosToWindow = cvMain.TransformToAncestor(this).Transform(new Point(0, 0));

                TextBlock r = sender as TextBlock;
                var upperlimit = canvPosToWindow.Y + (r.ActualHeight / 2);
                var lowerlimit = canvPosToWindow.Y + cvMain.ActualHeight - (r.ActualHeight / 2);

                var leftlimit = canvPosToWindow.X + (r.ActualWidth / 2);
                var rightlimit = canvPosToWindow.X + cvMain.ActualWidth - (r.ActualWidth / 2);


                var absmouseXpos = e.GetPosition(this).X;
                var absmouseYpos = e.GetPosition(this).Y;

                if ((absmouseXpos > leftlimit && absmouseXpos < rightlimit)
                    && (absmouseYpos > upperlimit && absmouseYpos < lowerlimit))
                {
                    r.SetValue(Canvas.LeftProperty, e.GetPosition(cvMain).X - (r.ActualWidth / 2));
                    r.SetValue(Canvas.TopProperty, e.GetPosition(cvMain).Y - (r.ActualHeight / 2));
                }
            }
        }

        private void cvMain_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {            
        //    MovingObject = null;
            tb.ReleaseMouseCapture();
            isDragging = false;
        }

        private void test_MouseEnter(object sender, MouseEventArgs e)
        {
            if(MovingObject != null)
            {
                Rectangle r = sender as Rectangle;
                r.Fill = Brushes.Yellow;
                r.Opacity = 0.5;
            }
        }

        private void test_MouseLeave(object sender, MouseEventArgs e)
        {
            Rectangle r = sender as Rectangle;
            r.Fill = Brushes.Transparent;
            r.Opacity = 1;
        }

        private void UserControl_MouseMove(object sender, MouseEventArgs e)
        {
            /// Get the x and y coordinates of the mouse pointer.
            System.Windows.Point position = e.GetPosition(this);
            double pX = position.X;
            double pY = position.Y;

            /// Sets eclipse to the mouse coordinates.
            Canvas.SetLeft(tb, pX);
            Canvas.SetTop(tb, pY);
            Canvas.SetRight(tb, pX);
        }
    }
}
