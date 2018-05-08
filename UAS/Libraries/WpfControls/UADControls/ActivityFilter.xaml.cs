using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls;

namespace WpfControls.UADControls
{
    /// <summary>
    /// Interaction logic for ActivityFilter.xaml
    /// </summary>
    public partial class ActivityFilter : UserControl
    {
        //Needs DomainTracking object from UAD
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICode> codeData = FrameworkServices.ServiceClient.UAD_Lookup_CodeClient();
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>> codeResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>>();
        private List<FrameworkUAD_Lookup.Entity.Code> filterTypes = new List<FrameworkUAD_Lookup.Entity.Code>();
        private List<FrameworkUAD_Lookup.Entity.Code> filterGroups = new List<FrameworkUAD_Lookup.Entity.Code>();

        public ActivityFilter(FrameworkUAD_Lookup.Enums.FilterGroupTypes group)
        {
            InitializeComponent();

            codeResponse = codeData.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAD_Lookup.Enums.CodeType.Filter_Type);
            if (Helpers.Common.CheckResponse(codeResponse.Result, codeResponse.Status))
            {
                filterTypes = codeResponse.Result;
            }

            codeResponse = codeData.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAD_Lookup.Enums.CodeType.Filter_Group);
            if (Helpers.Common.CheckResponse(codeResponse.Result, codeResponse.Status))
            {
                filterGroups = codeResponse.Result;
            }

            #region Load ComboBoxes
            rcbOpenCriteria.ItemsSource = new List<string>()
            {
                "No Opens",
                "Opened 1+",
                "Opened 2+",
                "Opened 3+",
                "Opened 4+",
                "Opened 5+"
            };

            rcbClickCriteria.ItemsSource = new List<string>()
            {
                "No Clicks",
                "Clicked 1+",
                "Clicked 2+",
                "Clicked 3+",
                "Clicked 4+",
                "Clicked 5+",
                "Clicked 10+",
                "Clicked 15+",
                "Clicked 20+",
                "Clicked 30+",
            };
            #endregion

            if (group == FrameworkUAD_Lookup.Enums.FilterGroupTypes.Consensus_View)
            {
                HorizontalRadioButtons open = new HorizontalRadioButtons("OpenSearchType");
                open.Name = "hrbOpen";
                spOpenSearchType.Children.Add(open);
                HorizontalRadioButtons click = new HorizontalRadioButtons("ClickSearchType");
                click.Name = "hrbClick";
                spClickSearchType.Children.Add(click);
            }
        }

        #region UI Events
        private void rcbOpenCriteria_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RadComboBox rcb = sender as RadComboBox;
            if(rcb.SelectedItem != null)
            {
                if (rcb.SelectedIndex == 0)
                {
                    rdpOpenDateTo.IsEnabled = true;
                    rdpOpenDateFrom.IsEnabled = true;
                    tbxBlastID.IsEnabled = false;
                    tbxEmailSubject.IsEnabled = false;
                    rdpEmailDateFrom.IsEnabled = false;
                    rdpEmailDateTo.IsEnabled = false;
                }
                else
                {
                    rdpOpenDateTo.IsEnabled = true;
                    rdpOpenDateFrom.IsEnabled = true;
                    tbxBlastID.IsEnabled = true;
                    tbxEmailSubject.IsEnabled = true;
                    rdpEmailDateFrom.IsEnabled = true;
                    rdpEmailDateTo.IsEnabled = true;
                }
            }
            else
            {
                rdpOpenDateTo.IsEnabled = false;
                rdpOpenDateFrom.IsEnabled = false;
                tbxBlastID.IsEnabled = false;
                tbxEmailSubject.IsEnabled = false;
                rdpEmailDateFrom.IsEnabled = false;
                rdpEmailDateTo.IsEnabled = false;
            }
        }

        private void rcbClickCriteria_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RadComboBox rcb = sender as RadComboBox;
            if (rcb.SelectedItem != null)
            {
                if (rcb.SelectedIndex == 0)
                {
                    tbxClickURL.IsEnabled = false;
                    rdpClickDateFrom.IsEnabled = true;
                    rdpClickDateTo.IsEnabled = true;
                    tbxClickBlastID.IsEnabled = false;
                    tbxClickEmailSubject.IsEnabled = false;
                    rdpClickEmailDateFrom.IsEnabled = false;
                    rdpClickEmailDateTo.IsEnabled = false;
                }
                else
                {
                    tbxClickURL.IsEnabled = true;
                    rdpClickDateFrom.IsEnabled = true;
                    rdpClickDateTo.IsEnabled = true;
                    tbxClickBlastID.IsEnabled = true;
                    tbxClickEmailSubject.IsEnabled = true;
                    rdpClickEmailDateFrom.IsEnabled = true;
                    rdpClickEmailDateTo.IsEnabled = true;
                }
            }
            else
            {
                tbxClickURL.IsEnabled = false;
                rdpClickDateFrom.IsEnabled = false;
                rdpClickDateTo.IsEnabled = false;
                tbxClickBlastID.IsEnabled = false;
                tbxClickEmailSubject.IsEnabled = false;
                rdpClickEmailDateFrom.IsEnabled = false;
                rdpClickEmailDateTo.IsEnabled = false;
            }
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            ResetSelection();
        }
        #endregion

        public List<Helpers.FilterOperations.FilterDetailContainer> GetSelection()
        {
            List<Helpers.FilterOperations.FilterDetailContainer> retList = new List<Helpers.FilterOperations.FilterDetailContainer>();
            int typeID = filterTypes.Where(x => x.CodeName == FrameworkUAD_Lookup.Enums.FilterTypes.Activity.ToString().Replace("_", " ")).Select(x => x.CodeId).FirstOrDefault();

            #region Open Criteria
            if(rcbOpenCriteria.SelectedItem != null)
            {
                FrameworkUAS.Entity.FilterDetail fdTop = new FrameworkUAS.Entity.FilterDetail()
                {
                    CreatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID,
                    DateCreated = DateTime.Now,
                    FilterField = rcbOpenCriteria.Name,
                    FilterObjectType = rcbOpenCriteria.Tag.ToString(),
                    SearchCondition = "EQUALS",
                    //FilterGroupID = typeID,
                    FilterTypeId = typeID
                };
                FrameworkUAS.Entity.FilterDetailSelectedValue fdsvTop = new FrameworkUAS.Entity.FilterDetailSelectedValue()
                {
                    CreatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID,
                    DateCreated = DateTime.Now,
                    SelectedValue = rcbOpenCriteria.Text
                };

                List<FrameworkUAS.Entity.FilterDetailSelectedValue> fdsvListTop = new List<FrameworkUAS.Entity.FilterDetailSelectedValue>();
                fdsvListTop.Add(fdsvTop);
                retList.Add(new Helpers.FilterOperations.FilterDetailContainer() { FilterDetail = fdTop, Values = fdsvListTop });
                foreach (RadDatePicker rdp in grdOpenCriteria.ChildrenOfType<RadDatePicker>())
                {
                    if (rdp.SelectedDate != null)
                    {
                        FrameworkUAS.Entity.FilterDetail fd = new FrameworkUAS.Entity.FilterDetail()
                        {
                            CreatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID,
                            DateCreated = DateTime.Now,
                            FilterField = rdp.Name,
                            FilterObjectType = rdp.Tag.ToString(),
                            SearchCondition = "EQUALS",
                            FilterTypeId = typeID
                        };
                        FrameworkUAS.Entity.FilterDetailSelectedValue fdsv = new FrameworkUAS.Entity.FilterDetailSelectedValue()
                        {
                            CreatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID,
                            DateCreated = DateTime.Now,
                            SelectedValue = rdp.SelectedDate.Value.ToShortDateString().ToString()
                        };
                        List<FrameworkUAS.Entity.FilterDetailSelectedValue> fdsvList = new List<FrameworkUAS.Entity.FilterDetailSelectedValue>();
                        fdsvList.Add(fdsv);
                        retList.Add(new Helpers.FilterOperations.FilterDetailContainer() { FilterDetail = fd, Values = fdsvList });
                    }
                }
                foreach (RadWatermarkTextBox tbx in grdOpenCriteria.ChildrenOfType<RadWatermarkTextBox>())
                {
                    if (tbx.Text != null && tbx.Text != string.Empty && tbx.GetVisualParent<RadDatePicker>() == null)
                    {
                        FrameworkUAS.Entity.FilterDetail fd = new FrameworkUAS.Entity.FilterDetail()
                        {
                            CreatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID,
                            DateCreated = DateTime.Now,
                            FilterField = tbx.Name,
                            FilterObjectType = tbx.Tag.ToString(),
                            SearchCondition = "EQUALS",
                            FilterTypeId = typeID
                        };
                        FrameworkUAS.Entity.FilterDetailSelectedValue fdsv = new FrameworkUAS.Entity.FilterDetailSelectedValue()
                        {
                            CreatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID,
                            DateCreated = DateTime.Now,
                            SelectedValue = tbx.Text.ToString()
                        };
                        List<FrameworkUAS.Entity.FilterDetailSelectedValue> fdsvList = new List<FrameworkUAS.Entity.FilterDetailSelectedValue>();
                        fdsvList.Add(fdsv);
                        retList.Add(new Helpers.FilterOperations.FilterDetailContainer() { FilterDetail = fd, Values = fdsvList });
                    }
                }
                foreach (HorizontalRadioButtons hrb in grdOpenCriteria.ChildrenOfType<HorizontalRadioButtons>())
                {
                    if(hrb.SelectedText != null && hrb.SelectedText != string.Empty)
                    {
                        FrameworkUAS.Entity.FilterDetail fd = new FrameworkUAS.Entity.FilterDetail()
                        {
                            CreatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID,
                            DateCreated = DateTime.Now,
                            FilterField = (hrb.Parent as StackPanel).Name,
                            FilterObjectType = hrb.TagType.ToString(),
                            SearchCondition = "EQUALS",
                            FilterTypeId = typeID
                        };
                        FrameworkUAS.Entity.FilterDetailSelectedValue fdsv = new FrameworkUAS.Entity.FilterDetailSelectedValue()
                        {
                            CreatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID,
                            DateCreated = DateTime.Now,
                            SelectedValue = hrb.SelectedText.ToString()
                        };
                        List<FrameworkUAS.Entity.FilterDetailSelectedValue> fdsvList = new List<FrameworkUAS.Entity.FilterDetailSelectedValue>();
                        fdsvList.Add(fdsv);
                        retList.Add(new Helpers.FilterOperations.FilterDetailContainer() { FilterDetail = fd, Values = fdsvList });
                    }
                }
            }
            #endregion

            #region Click Criteria
            if (rcbClickCriteria.SelectedItem != null)
            {
                FrameworkUAS.Entity.FilterDetail fdTop = new FrameworkUAS.Entity.FilterDetail()
                {
                    CreatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID,
                    DateCreated = DateTime.Now,
                    FilterField = rcbClickCriteria.Name,
                    FilterObjectType = rcbClickCriteria.Tag.ToString(),
                    SearchCondition = "EQUALS",
                    FilterTypeId = typeID
                };
                FrameworkUAS.Entity.FilterDetailSelectedValue fdsvTop = new FrameworkUAS.Entity.FilterDetailSelectedValue()
                {
                    CreatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID,
                    DateCreated = DateTime.Now,
                    SelectedValue = rcbClickCriteria.Text
                };

                List<FrameworkUAS.Entity.FilterDetailSelectedValue> fdsvListTop = new List<FrameworkUAS.Entity.FilterDetailSelectedValue>();
                fdsvListTop.Add(fdsvTop);
                retList.Add(new Helpers.FilterOperations.FilterDetailContainer() { FilterDetail = fdTop, Values = fdsvListTop });
                foreach (RadDatePicker rdp in grdClickCriteria.ChildrenOfType<RadDatePicker>())
                {
                    if (rdp.SelectedDate != null)
                    {
                        FrameworkUAS.Entity.FilterDetail fd = new FrameworkUAS.Entity.FilterDetail()
                        {
                            CreatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID,
                            DateCreated = DateTime.Now,
                            FilterField = rdp.Name,
                            FilterObjectType = rdp.Tag.ToString(),
                            SearchCondition = "EQUALS",
                            FilterTypeId = typeID
                        };
                        FrameworkUAS.Entity.FilterDetailSelectedValue fdsv = new FrameworkUAS.Entity.FilterDetailSelectedValue()
                        {
                            CreatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID,
                            DateCreated = DateTime.Now,
                            SelectedValue = rdp.SelectedDate.Value.ToShortDateString().ToString()
                        };
                        List<FrameworkUAS.Entity.FilterDetailSelectedValue> fdsvList = new List<FrameworkUAS.Entity.FilterDetailSelectedValue>();
                        fdsvList.Add(fdsv);
                        retList.Add(new Helpers.FilterOperations.FilterDetailContainer() { FilterDetail = fd, Values = fdsvList });
                    }
                }
                foreach (RadWatermarkTextBox tbx in grdClickCriteria.ChildrenOfType<RadWatermarkTextBox>())
                {
                    if (tbx.Text != null && tbx.Text != string.Empty && tbx.GetVisualParent<RadDatePicker>() == null)
                    {
                        FrameworkUAS.Entity.FilterDetail fd = new FrameworkUAS.Entity.FilterDetail()
                        {
                            CreatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID,
                            DateCreated = DateTime.Now,
                            FilterField = tbx.Name,
                            FilterObjectType = tbx.Tag.ToString(),
                            SearchCondition = "EQUALS",
                            FilterTypeId = typeID
                        };
                        FrameworkUAS.Entity.FilterDetailSelectedValue fdsv = new FrameworkUAS.Entity.FilterDetailSelectedValue()
                        {
                            CreatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID,
                            DateCreated = DateTime.Now,
                            SelectedValue = tbx.Text.ToString()
                        };
                        List<FrameworkUAS.Entity.FilterDetailSelectedValue> fdsvList = new List<FrameworkUAS.Entity.FilterDetailSelectedValue>();
                        fdsvList.Add(fdsv);
                        retList.Add(new Helpers.FilterOperations.FilterDetailContainer() { FilterDetail = fd, Values = fdsvList });
                    }
                }
                foreach (HorizontalRadioButtons hrb in grdClickCriteria.ChildrenOfType<HorizontalRadioButtons>())
                {
                    if (hrb.SelectedText != null && hrb.SelectedText != string.Empty)
                    {
                        FrameworkUAS.Entity.FilterDetail fd = new FrameworkUAS.Entity.FilterDetail()
                        {
                            CreatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID,
                            DateCreated = DateTime.Now,
                            FilterField = (hrb.Parent as StackPanel).Name,
                            FilterObjectType = hrb.TagType.ToString(),
                            SearchCondition = "EQUALS",
                            FilterTypeId = typeID
                        };
                        FrameworkUAS.Entity.FilterDetailSelectedValue fdsv = new FrameworkUAS.Entity.FilterDetailSelectedValue()
                        {
                            CreatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID,
                            DateCreated = DateTime.Now,
                            SelectedValue = hrb.SelectedText.ToString()
                        };
                        List<FrameworkUAS.Entity.FilterDetailSelectedValue> fdsvList = new List<FrameworkUAS.Entity.FilterDetailSelectedValue>();
                        fdsvList.Add(fdsv);
                        retList.Add(new Helpers.FilterOperations.FilterDetailContainer() { FilterDetail = fd, Values = fdsvList });
                    }
                }
            }
            #endregion

            #region Website Visits
            if (rcbWebsiteVisits.SelectedItem != null)
            {
                FrameworkUAS.Entity.FilterDetail fdTop = new FrameworkUAS.Entity.FilterDetail()
                {
                    CreatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID,
                    DateCreated = DateTime.Now,
                    FilterField = rcbWebsiteVisits.Name,
                    FilterObjectType = rcbWebsiteVisits.Tag.ToString(),
                    SearchCondition = "EQUALS",
                    FilterTypeId = typeID
                };
                FrameworkUAS.Entity.FilterDetailSelectedValue fdsvTop = new FrameworkUAS.Entity.FilterDetailSelectedValue()
                {
                    CreatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID,
                    DateCreated = DateTime.Now,
                    SelectedValue = rcbWebsiteVisits.Text
                };
                List<FrameworkUAS.Entity.FilterDetailSelectedValue> fdsvListTop = new List<FrameworkUAS.Entity.FilterDetailSelectedValue>();
                fdsvListTop.Add(fdsvTop);
                retList.Add(new Helpers.FilterOperations.FilterDetailContainer() { FilterDetail = fdTop, Values = fdsvListTop });
            }
            foreach (RadDatePicker rdp in grdWebsiteVisits.ChildrenOfType<RadDatePicker>())
            {
                if (rdp.SelectedDate != null)
                {
                    FrameworkUAS.Entity.FilterDetail fd = new FrameworkUAS.Entity.FilterDetail()
                    {
                        CreatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID,
                        DateCreated = DateTime.Now,
                        FilterField = rdp.Name,
                        FilterObjectType = rdp.Tag.ToString(),
                        SearchCondition = "EQUALS",
                        FilterTypeId = typeID
                    };
                    FrameworkUAS.Entity.FilterDetailSelectedValue fdsv = new FrameworkUAS.Entity.FilterDetailSelectedValue()
                    {
                        CreatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID,
                        DateCreated = DateTime.Now,
                        SelectedValue = rdp.SelectedDate.Value.ToShortDateString().ToString()
                    };
                    List<FrameworkUAS.Entity.FilterDetailSelectedValue> fdsvList = new List<FrameworkUAS.Entity.FilterDetailSelectedValue>();
                    fdsvList.Add(fdsv);
                    retList.Add(new Helpers.FilterOperations.FilterDetailContainer() { FilterDetail = fd, Values = fdsvList });
                }
            }
            foreach (RadWatermarkTextBox tbx in grdWebsiteVisits.ChildrenOfType<RadWatermarkTextBox>())
            {
                if (tbx.Text != null && tbx.Text != string.Empty && tbx.GetVisualParent<RadDatePicker>() == null)
                {
                    FrameworkUAS.Entity.FilterDetail fd = new FrameworkUAS.Entity.FilterDetail()
                    {
                        CreatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID,
                        DateCreated = DateTime.Now,
                        FilterField = tbx.Name,
                        FilterObjectType = tbx.Tag.ToString(),
                        SearchCondition = "EQUALS",
                        FilterTypeId = typeID
                    };
                    FrameworkUAS.Entity.FilterDetailSelectedValue fdsv = new FrameworkUAS.Entity.FilterDetailSelectedValue()
                    {
                        CreatedByUserID = FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.UserID,
                        DateCreated = DateTime.Now,
                        SelectedValue = tbx.Text.ToString()
                    };
                    List<FrameworkUAS.Entity.FilterDetailSelectedValue> fdsvList = new List<FrameworkUAS.Entity.FilterDetailSelectedValue>();
                    fdsvList.Add(fdsv);
                    retList.Add(new Helpers.FilterOperations.FilterDetailContainer() { FilterDetail = fd, Values = fdsvList });
                }
            }
            #endregion

            return retList;
        }

        public void ResetSelection()
        {
            rdpOpenDateTo.SelectedDate = null;
            rdpOpenDateFrom.SelectedDate = null;
            tbxBlastID.Text = string.Empty;
            tbxEmailSubject.Text = string.Empty;
            rdpEmailDateFrom.SelectedDate = null;
            rdpEmailDateTo.SelectedDate = null;
            tbxClickURL.Text = string.Empty;
            rdpClickDateFrom.SelectedDate = null;
            rdpClickDateTo.SelectedDate = null;
            tbxClickBlastID.Text = string.Empty;
            tbxClickEmailSubject.Text = string.Empty;
            rdpClickEmailDateFrom.SelectedDate = null;
            rdpClickEmailDateTo.SelectedDate = null;
            rdpEmailDateFrom.SelectedDate = null;
            rdpEmailDateTo.SelectedDate = null;
            tbxVisitsURL.Text = string.Empty;
            rdpVisitDateFrom.SelectedDate = null;
            rdpVisitDateTo.SelectedDate = null;
            rcbClickCriteria.SelectedItem = null;
            rcbOpenCriteria.SelectedItem = null;
            rcbWebsiteVisits.SelectedItem = null;
            foreach(HorizontalRadioButtons hrb in this.ChildrenOfType<HorizontalRadioButtons>())
            {
                hrb.SelectedText = "Search All";
            }
        }
    }
}
