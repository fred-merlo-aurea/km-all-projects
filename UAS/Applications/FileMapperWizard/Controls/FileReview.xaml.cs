using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FileMapperWizard.Controls
{
    /// <summary>
    /// Interaction logic for FileReview.xaml
    /// </summary>
    public partial class FileReview : UserControl
    {
        #region Variables
        FileMapperWizard.Modules.FMUniversal thisContainer;
        #endregion

        public FileReview(FileMapperWizard.Modules.FMUniversal container, string incomingField, string mapField, FrameworkUAS.Entity.FieldMapping soloFieldMap, List<FrameworkUAS.Entity.Transformation> lstTransformations)
        {
            InitializeComponent();
            thisContainer = container;
            LoadData(incomingField, mapField, soloFieldMap, lstTransformations);
        }

        private void LoadData(string incomingField, string mapField, FrameworkUAS.Entity.FieldMapping soloFieldMap, List<FrameworkUAS.Entity.Transformation> lstTransformations)
        {

            tbIncoming.Text = incomingField.ToUpper();
            tbMapped.Text = mapField.ToUpper();
            #region Multi Maps
            foreach (FrameworkUAS.Entity.FieldMultiMap fmm in soloFieldMap.FieldMultiMappings)
            {
                TextBlock tbMulti = new TextBlock();                
                tbMulti.Margin = new Thickness(10,4,4,4);
                tbMulti.Text = "ADDITIONAL MAPPING: " + fmm.MAFField.ToUpper();
                tbMulti.FontSize = 10;
                var converter = new System.Windows.Media.BrushConverter();
                var brush = (Brush)converter.ConvertFromString("#4B87BC");                                    
                tbMulti.Foreground = brush;
                tbMulti.VerticalAlignment = System.Windows.VerticalAlignment.Center;                
                spMultiMap.Children.Add(tbMulti);
            }
            if (spMultiMap.Children.Count > 0)
                bMultiMap.Visibility = System.Windows.Visibility.Visible;

            #endregion
            #region Transformations
            FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICode> cWorker = FrameworkServices.ServiceClient.UAD_Lookup_CodeClient();
            List<FrameworkUAD_Lookup.Entity.Code> codeList = cWorker.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAD_Lookup.Enums.CodeType.Transformation).Result;

            foreach (FrameworkUAS.Entity.Transformation t in lstTransformations)
            {
                FrameworkUAD_Lookup.Entity.Code tran = codeList.Single(x => x.CodeId == t.TransformationTypeID);
                TextBlock tbTran = new TextBlock();                
                tbTran.Margin = new Thickness(10,4,4,4);                
                tbTran.Text = "TRANSFORMATION: " + tran.DisplayName.ToUpper() + " - " + t.TransformationName.ToUpper();
                tbTran.FontSize = 10;
                var converter = new System.Windows.Media.BrushConverter();
                var brush = (Brush)converter.ConvertFromString("#4B87BC");                                    
                tbTran.Foreground = brush;
                tbTran.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                spTransformation.Children.Add(tbTran);
            }
            if (spTransformation.Children.Count > 0)
                bTransformation.Visibility = System.Windows.Visibility.Visible;

            #endregion
        }
    }
}
