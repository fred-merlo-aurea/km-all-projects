using System.Windows.Documents;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Markup;
using System;
using System.Linq;
using Telerik.Windows.Controls.GridView;
using Telerik.Windows.Controls;

namespace Circulation.Helpers
{
    public static class RadGridViewPrinter
    {
        static FixedDocument ToFixedDocument(FrameworkElement element, PrintDialog dialog)
        {
            var capabilities = dialog.PrintQueue.GetPrintCapabilities(dialog.PrintTicket);
            var pageSize = new Size(dialog.PrintableAreaWidth, dialog.PrintableAreaHeight);
            var extentSize = new Size(capabilities.PageImageableArea.ExtentWidth, capabilities.PageImageableArea.ExtentHeight);

            var fixedDocument = new FixedDocument();

            element.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            element.Arrange(new Rect(new Point(0, 0), element.DesiredSize));
            var totalHeight = element.DesiredSize.Height;
            var totalWidth = element.DesiredSize.Width;

            var yOffset = 25d;
            var xOffset = 0d;
            while (yOffset < totalHeight)
            {
                var brush = new VisualBrush(element)
                {
                    Stretch = Stretch.None,
                    AlignmentX = AlignmentX.Left,
                    AlignmentY = AlignmentY.Top,
                    ViewboxUnits = BrushMappingMode.Absolute,
                    TileMode = TileMode.None,
                    Viewbox = new Rect(0, yOffset, extentSize.Width, extentSize.Height)
                };

                var pageContent = new PageContent();
                var page = new FixedPage();                
                ((IAddChild)pageContent).AddChild(page);

                fixedDocument.Pages.Add(pageContent);
                page.Width = pageSize.Width;
                page.Height = pageSize.Height;

                var canvas = new Canvas();
                FixedPage.SetLeft(canvas, capabilities.PageImageableArea.OriginWidth);
                FixedPage.SetTop(canvas, capabilities.PageImageableArea.OriginHeight);
                canvas.Width = extentSize.Width;
                canvas.Height = extentSize.Height;
                canvas.Background = brush;

                page.Children.Add(canvas);

                yOffset += extentSize.Height;
                xOffset += extentSize.Width;

                while (xOffset < totalWidth)
                {
                    var brushX = new VisualBrush(element)
                    {
                        Stretch = Stretch.None,
                        AlignmentX = AlignmentX.Left,
                        AlignmentY = AlignmentY.Top,
                        ViewboxUnits = BrushMappingMode.Absolute,
                        TileMode = TileMode.None,
                        Viewbox = new Rect(xOffset, 0, extentSize.Width, extentSize.Height)
                    };

                    pageContent = new PageContent();
                    page = new FixedPage();                    
                    ((IAddChild)pageContent).AddChild(page);

                    fixedDocument.Pages.Add(pageContent);
                    page.Width = pageSize.Width;
                    page.Height = pageSize.Height;

                    canvas = new Canvas();
                    FixedPage.SetLeft(canvas, capabilities.PageImageableArea.OriginWidth);
                    FixedPage.SetTop(canvas, capabilities.PageImageableArea.OriginHeight);
                    canvas.Width = extentSize.Width;
                    canvas.Height = extentSize.Height;
                    canvas.Background = brushX;

                    page.Children.Add(canvas);

                    xOffset += extentSize.Width;
                }
                xOffset = 0d;
            }
            
            return fixedDocument;
        }

        static GridViewDataControl ToPrintFriendlyGrid(GridViewDataControl source)
        {
            var grid = new RadGridView()
            {
                ItemsSource = source.ItemsSource,
                RowIndicatorVisibility = Visibility.Collapsed,
                ShowGroupPanel = false,
                CanUserFreezeColumns = false,
                IsFilteringAllowed = false,
                AutoExpandGroups = true,
                AutoGenerateColumns = false
            };

            foreach (var column in source.Columns.OfType<GridViewDataColumn>())
            {
                var newColumn = new GridViewDataColumn();
                newColumn.DataMemberBinding = new System.Windows.Data.Binding(column.UniqueName);
                grid.Columns.Add(newColumn);
            }

            StyleManager.SetTheme(grid, StyleManager.GetTheme(grid));

            //grid.SortDescriptors.AddRange(source.SortDescriptors);
            //grid.GroupDescriptors.AddRange(source.GroupDescriptors);
            //grid.FilterDescriptors.AddRange(source.FilterDescriptors);

            return grid;
        }

        public static void PrintPreview(this GridViewDataControl source)
        {
            var window = new Window()
            {
                Title = "Print Preview",
                Content = new DocumentViewer()
                {                    
                    Document = ToFixedDocument(ToPrintFriendlyGrid(source), new PrintDialog())
                }
            };

            window.ShowDialog();
        }

        public static void Print(this GridViewDataControl source, bool showDialog)
        {
            var dialog = new PrintDialog();
            var dialogResult = showDialog ? dialog.ShowDialog() : true;

            if (dialogResult == true)
            {
                var viewer = new DocumentViewer();
                viewer.Document = ToFixedDocument(ToPrintFriendlyGrid(source), dialog);
                dialog.PrintDocument(viewer.Document.DocumentPaginator, "");
            }
        }
    }
}
