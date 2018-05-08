using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using ControlCenter.Modules;
using System.Windows.Forms;

namespace ControlCenter.Windows
{
    /// <summary>
    /// Interaction logic for QuestionBranching.xaml
    /// </summary>
    public partial class QuestionBranching : Window
    {
        CodeSheetViewer thisParent { get; set; }
        public QuestionBranching(CodeSheetViewer parent)
        {
            InitializeComponent();
            thisParent = parent;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData(thisParent);
        }  

        private DoubleAnimation animationOpacity;
        private System.Windows.Forms.ListBox lb2 = new System.Windows.Forms.ListBox();
        //private System.Windows.Forms.TreeView tv = new TreeView();

        private void LoadData(CodeSheetViewer parent)
        {
            this.lb.MouseDown += new MouseEventHandler(listBox1_MouseDown);
            this.lb.DragOver += new System.Windows.Forms.DragEventHandler(listBox1_DragOver);

            this.tv.DragEnter += new System.Windows.Forms.DragEventHandler(treeView1_DragEnter);
            this.tv.DragDrop += new System.Windows.Forms.DragEventHandler(treeView1_DragDrop);

            TextBlock txt = new TextBlock();
            txt.Text = "Test";
            TextBlock txt2 = new TextBlock();
            txt2.Text = "Test";

            lb.Items.Add("Test");
            lb.Items.Add("Test2");//Go ??
            //NicksWinforms.Children.Add()

            Grid.SetRow(NicksWinforms, 1);
            Grid.SetColumn(NicksWinforms, 0);
            lb2.Items.Add("Test");

            //lb.Items.Add(txt);
            //lb.Items.Add(txt2);
            

            tv.BeginUpdate();
            TreeNode t = new TreeNode();
            t.Name = "t";
            t.Tag = "t";
            t.Text = "t";

            tv.Nodes.Add("Parent Node");
            tv.Nodes.Add("t"); //FUUUUUUUUUUUUUUUUUUUUUUUUUUUU
            tv.EndUpdate(); 

            animationOpacity = new DoubleAnimation
            {
                To = 0,
                Duration = TimeSpan.FromSeconds(.4)
            };
        }

        private void treeView1_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
        {

            TreeNode nodeToDropIn = this.tv.GetNodeAt(this.tv.PointToClient(new System.Drawing.Point(e.X, e.Y)));
            if (nodeToDropIn == null) { return; }
            if (nodeToDropIn.Level > 0)
            {
                nodeToDropIn = nodeToDropIn.Parent;
            }

            object data = e.Data.GetData(typeof(DateTime));
            if (data == null) { return; }
            nodeToDropIn.Nodes.Add(data.ToString());
            this.lb.Items.Remove(data);
        }

        private void listBox1_DragOver(object sender, System.Windows.Forms.DragEventArgs e)
        {
            e.Effect = System.Windows.Forms.DragDropEffects.Move;
        }

        private void treeView1_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
        {
            e.Effect = System.Windows.Forms.DragDropEffects.Move;
        }

        private void listBox1_MouseDown(object sender, MouseEventArgs e)
        {
            this.lb.DoDragDrop(this.lb.SelectedItem, System.Windows.Forms.DragDropEffects.Move);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Closing -= Window_Closing;
            e.Cancel = true;
            var anim = animationOpacity;
            anim.Completed += (s, _) => this.Close();
            this.BeginAnimation(Rectangle.OpacityProperty, animationOpacity);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }      
    }
}
