using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using KMPS.MD.Objects;

namespace KMPS.MD
{
    public abstract class BrandsPageBase : WebPageBase
    {
        private const string BrandId = "BrandID";

        protected abstract Panel BrandPanel { get; }
        protected abstract DropDownList BrandDropDown { get; }
        protected abstract HiddenField BrandIdHiddenField { get; }
        protected abstract Label BrandNameLabel { get; }
        protected virtual string BrandDefaultEmptyDropDown => string.Empty;

        protected abstract void LoadPageFilters();

        protected new MasterPages.Site Master 
        {
            get 
            {
                return (MasterPages.Site)base.Master;
            }
        }

        protected void LoadBrands()
        {
            var brands = new List<Brand>();
            var isBrandAssignedUser = false;

            if (!KM.Platform.User.IsAdministrator(Master.UserSession.CurrentUser))
            {
                brands = Brand.GetByUserID(Master.clientconnections, Master.LoggedInUser);

                if (brands.Count > 0)
                {
                    isBrandAssignedUser = true;
                }
            }

            if (brands.Count == 0)
            {
                brands = Brand.GetAll(Master.clientconnections);
            }

            if (brands.Count > 0)
            {
                var brandId = 0;
                if (Request.QueryString[BrandId] != null)
                {
                    brandId = Convert.ToInt32(Request.QueryString[BrandId]);
                }
                
                BrandPanel.Visible = true;
                if (brands.Count > 1)
                {
                    BrandDropDown.Visible = true;
                    BrandDropDown.DataSource = brands;
                    BrandDropDown.DataBind();
                    if (KM.Platform.User.IsAdministrator(Master.UserSession.CurrentUser) || !isBrandAssignedUser)
                    {
                        BrandDropDown.Items.Insert(0, new ListItem("All Products", "0"));
                        
                        if (brandId > 0)
                        {
                            BrandDropDown.SelectedValue = brandId.ToString();
                        }

                        BrandIdHiddenField.Value = BrandDropDown.SelectedItem.Value;
                        LoadPageFilters();
                    }
                    else
                    {
                        BrandDropDown.Items.Insert(0, new ListItem(BrandDefaultEmptyDropDown, "-1"));

                        if (brandId > 0)
                        {
                            BrandDropDown.SelectedValue = brandId.ToString();
                        }

                        BrandIdHiddenField.Value = BrandDropDown.SelectedItem.Value;
                    }
                }
                else if (brands.Count == 1)
                {
                    if (KM.Platform.User.IsAdministrator(Master.UserSession.CurrentUser) || !isBrandAssignedUser)
                    {
                        BrandDropDown.Visible = true;
                        BrandDropDown.DataSource = brands;
                        BrandDropDown.DataBind();
                        BrandDropDown.Items.Insert(0, new ListItem("All Products", "0"));

                        if (brandId > 0)
                        {
                            BrandDropDown.SelectedValue = brandId.ToString();
                        }

                        BrandIdHiddenField.Value = BrandDropDown.SelectedItem.Value;
                    }
                    else
                    {
                        BrandIdHiddenField.Value = brands.FirstOrDefault().BrandID.ToString();
                        BrandNameLabel.Visible = true;
                        BrandNameLabel.Text = brands.FirstOrDefault().BrandName;

                        ShowBrandUI(brands.FirstOrDefault());
                    }

                    LoadPageFilters();
                }
            }
            else
            {
                LoadPageFilters();
            }
        }

        /// <summary>
        /// This method when overriden in derived classes to handle the UI logic of showing branding
        /// </summary>
        /// <param name="brand"></param>
        protected virtual void ShowBrandUI(Brand brand)
        {

        }

        protected int IntTryParse(string input)
        {
            int returnValue;

            if (!int.TryParse(input, out returnValue))
            {
                throw new InvalidOperationException($"{input} cannot be parsed to integer");
            }

            return returnValue;
        }
    }
}