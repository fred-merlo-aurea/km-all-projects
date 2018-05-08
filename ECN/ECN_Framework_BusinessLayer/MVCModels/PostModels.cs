using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web;

namespace ECN_Framework_BusinessLayer.MVCModels
{
    public class PostModels
    {
        public class Menu
        {
            public class PostMenu
            {
                public PostMenu() { }
                public PostMenu(string accountChange,int clientID, string application, string referringURL)
                {
                    AccountChange = accountChange;
                    Application = application;
                    cdd = new ClientDropDown();
                    ECN_Framework_BusinessLayer.Application.ECNSession session = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession();

                    KMPlatform.Entity.User currentUser = ECN_Framework_BusinessLayer.Application.ECNSession.CurrentSession().CurrentUser;

                    string selected = referringURL;
                    
                    int? securityGroupID = currentUser.CurrentSecurityGroup != null ? currentUser.CurrentSecurityGroup.SecurityGroupID : -1;
                    KMPlatform.BusinessLogic.Menu blMenu = new KMPlatform.BusinessLogic.Menu();
                    List<KMPlatform.Entity.Menu> menuList = blMenu.SelectForApplicationAndUser(Application, currentUser.UserID, clientID, securityGroupID.Value, true, true, true);
                    Menu = GetMenuList(menuList, selected);

                    cdd.SelectedClientGroupID = session.ClientGroupID;
                    cdd.SelectedClientID = session.ClientID;
                   
                    cdd.CurrentClientGroupID = cdd.SelectedClientGroupID;
                    cdd.CurrentClientID = cdd.SelectedClientID;

                    cdd.SelectedProductID = session.ProductID;
                    cdd.CurrentProductID = cdd.SelectedProductID;

                    cdd.AccountChange = AccountChange;
                    cdd = RepopulateDropDowns(cdd, session, application);
                   
                   
                    LogOffLink = "/EmailMarketing.Site/Login/Logout";
                }

                public string Application { get; set; }

                public string AccountChange { get; set; }
                 public MenuModel Menu { get; set; }

                public ClientDropDown cdd { get; set; }

                public string LogOffLink { get; set; }

                private ECN_Framework_BusinessLayer.MVCModels.PostModels.Menu.ClientDropDown RepopulateDropDowns(ECN_Framework_BusinessLayer.MVCModels.PostModels.Menu.ClientDropDown model, ECN_Framework_BusinessLayer.Application.ECNSession session,string app)
                {
                    model.SelectedClientGroupID = session.ClientGroupID;
                    model.CurrentClientGroupID = model.SelectedClientGroupID;
                    model.SelectedClientID = session.ClientID;
                    model.CurrentClientID = model.SelectedClientID;

                    List<ECN_Framework_Entities.Accounts.BaseChannel> bcList = ECN_Framework_BusinessLayer.Accounts.BaseChannel.GetAll();

                    var ClientGroups = session.CurrentUser.ClientGroups.Where(x => bcList.Exists(y => y.PlatformClientGroupID == x.ClientGroupID) && x.IsActive == true).OrderBy(x => x.ClientGroupName).ToList();
                    List<KMPlatform.Entity.Client> lstClient = new List<KMPlatform.Entity.Client>();
                    if (app.Equals("AMSCircMVC", StringComparison.InvariantCultureIgnoreCase))
                    {
                        lstClient = new KMPlatform.BusinessLogic.Client().SelectActiveForClientGroupLite(session.ClientGroupID)
                          .Where(x => x.IsAMS == true && x.IsActive == true)
                          .OrderBy(x => x.ClientName)
                          .ToList();

                        try
                        {
                            var CurrentClient = lstClient.FirstOrDefault(x => x.ClientID == model.SelectedClientID);
                            if (CurrentClient.Products == null || CurrentClient.Products.Count == 0)
                            {
                                //model.ProductItems = new KMPlatform.BusinessLogic.Client().SelectProducts(CurrentClient).Where(x => x.IsCirc == true).ToList();
                                var ProductItems = new KMPlatform.BusinessLogic.Client().SelectProducts(CurrentClient).Where(x => x.IsCirc == true).ToList();
                                ProductItems = ProductItems.OrderBy(x => x.ProductCode).ToList();
                                ProductItems.ForEach(x => model.ProductsSelectList.Add(new SelectListItem() { Text = x.ProductCode, Value = x.ProductID.ToString() }));
                            }
                            else
                            {
                                //model.ProductItems = CurrentClient.Products.Where(x => x.IsCirc == true).ToList();
                                var ProductItems = CurrentClient.Products.Where(x => x.IsCirc == true).ToList();
                                ProductItems = ProductItems.OrderBy(x => x.ProductCode).ToList();
                                ProductItems.ForEach(x => model.ProductsSelectList.Add(new SelectListItem() { Text = x.ProductCode, Value = x.ProductID.ToString() }));
                            }

                            if (session.ProductID > 0)
                                model.SelectedProductID = session.ProductID;
                            else
                            {
                                model.SelectedProductID = Convert.ToInt16(model.ProductsSelectList.FirstOrDefault().Value);
                                session.ProductID = model.SelectedProductID;
                            }
                        }
                        catch (Exception)
                        {
                            model.SelectedProductID = 0;
                            session.ProductID = 0;
                            model.ProductsSelectList = new List<SelectListItem>();
                        }
                        model.CurrentProductID = model.SelectedProductID;
                    }
                    else
                    {
                        if (KMPlatform.BusinessLogic.User.IsChannelAdministrator(session.CurrentUser))
                        {
                            lstClient = new KMPlatform.BusinessLogic.Client().SelectActiveForClientGroupLite(session.CurrentUser.ClientGroups.First(x => x.ClientGroupID == session.ClientGroupID).ClientGroupID).OrderBy(x => x.ClientName).ToList();
                        }
                        else 
                        {
                            lstClient = new KMPlatform.BusinessLogic.Client().SelectbyUserIDclientgroupID(session.CurrentUser.UserID, session.ClientGroupID, false);
                        }
                    }
                    //Creating SelectList
                    if (ClientGroups != null && ClientGroups.Count > 0)
                        ClientGroups.ForEach(x => model.ClientGroupsSelectList.Add(new SelectListItem() { Text = x.ClientGroupName, Value = x.ClientGroupID.ToString() }));
                    if (lstClient != null && lstClient.Count > 0)
                        lstClient.ForEach(x => model.ClientsSelectList.Add(new SelectListItem() { Text = x.ClientName, Value = x.ClientID.ToString() }));
                    
                    List<KMPlatform.Entity.User.EcnAccountsUserListGridViewModel> users = new List<KMPlatform.Entity.User.EcnAccountsUserListGridViewModel>();
                    KMPlatform.Entity.User.EcnAccountsUserListGridViewModel currentU = new KMPlatform.Entity.User.EcnAccountsUserListGridViewModel();
                    currentU.UserID = session.CurrentUser.UserID;
                    currentU.UserName = session.CurrentUser.UserName;
                    users.Add(currentU);
                    KMPlatform.Entity.User.EcnAccountsUserListGridViewModel editUser = new KMPlatform.Entity.User.EcnAccountsUserListGridViewModel();
                    editUser.UserID = -1;
                    editUser.UserName = "Edit Profile";
                    users.Add(editUser);

                    

                    model.UserDropDown = users;
                    model.CurrentUserID = session.CurrentUser.UserID;

                    return model;
                }

                private ECN_Framework_BusinessLayer.MVCModels.PostModels.Menu.MenuModel GetMenuList(List<KMPlatform.Entity.Menu> dtMenu, string selected)
                {
                    ECN_Framework_BusinessLayer.MVCModels.PostModels.Menu.MenuModel RootMenu = new ECN_Framework_BusinessLayer.MVCModels.PostModels.Menu.MenuModel();
                    RootMenu.Children = GetChildren(0, dtMenu, selected);
                    return RootMenu;
                }

                private List<ECN_Framework_BusinessLayer.MVCModels.PostModels.Menu.MenuModel> GetChildren(int MenuID, List<KMPlatform.Entity.Menu> dtChildren, string selected)
                {
                    List<ECN_Framework_BusinessLayer.MVCModels.PostModels.Menu.MenuModel> children = new List<ECN_Framework_BusinessLayer.MVCModels.PostModels.Menu.MenuModel>();

                    foreach (KMPlatform.Entity.Menu drChildren in dtChildren.Where(x => x.ParentMenuID == MenuID).OrderBy(x => x.MenuOrder))//.Select("ParentMenuID = '" + MenuID.ToString() == "0" ? "" : MenuID.ToString() + "'").OrderBy(x => x.Field<int>("MenuOrder")))
                    {
                        ECN_Framework_BusinessLayer.MVCModels.PostModels.Menu.MenuModel child = new ECN_Framework_BusinessLayer.MVCModels.PostModels.Menu.MenuModel();
                        child.MenuID = drChildren.MenuID;
                        child.MenuName = drChildren.MenuName;
                        child.URL = drChildren.URL;
                        child.ParentMenuID = MenuID;
                        if (child.URL.ToLower().Equals(selected.ToLower()) && !child.MenuName.ToLower().Equals("home"))
                            child.IsSelected = true;
                        else
                            child.IsSelected = false;

                        if (drChildren.IsParent)
                        {
                            child.Children = GetChildren(child.MenuID, dtChildren, selected);
                        }
                        children.Add(child);
                    }

                    return children;
                }
            }

            
            public partial class MenuModel
            {
                public MenuModel()
                {
                    MenuID = -1;
                    MenuName = "";
                    URL = "";
                    ParentMenuID = -1;
                    Children = new List<MenuModel>();
                    Customers = new List<ECN_Framework_Entities.Accounts.Customer>();
                    IsSelected = false;

                }

                public int MenuID { get; set; }
                public string MenuName { get; set; }
                public string URL { get; set; }
                public int ParentMenuID { get; set; }
                public List<MenuModel> Children { get; set; }
                public List<SelectListItem> CustomersSelectList { get; set; }
                public List<ECN_Framework_Entities.Accounts.Customer> Customers { get; set; }
                public bool IsSelected { get; set; }


            }

            public class ClientDropDown
            {
                public ClientDropDown()
                {
                    SelectedClientGroupID = -1;
                    SelectedClientID = -1;
                    CurrentClientGroupID = -1;
                    CurrentClientID = -1;
                    CurrentProductID = -1;
                    SelectedProductID = -1;
                    UserDropDown = null;
                    ClientsSelectList = new List<SelectListItem>();
                    ClientGroupsSelectList = new List<SelectListItem>();
                    ProductsSelectList = new List<SelectListItem>();
                    
                }
                public int ClientGroupID { get; set; }

                public int ClientID { get; set; }

                public int SelectedClientID { get; set; }

                public int SelectedClientGroupID { get; set; }

                public int CurrentClientID { get; set; }

                public int CurrentClientGroupID { get; set; }

                public int CurrentUserID { get; set; }

                public int SelectedProductID { get; set; }
                public int CurrentProductID { get; set; }

               

                public string AccountChange { get; set; }

                public List<SelectListItem> ClientsSelectList { get; set; }
                public List<SelectListItem> ClientGroupsSelectList { get; set; }
                public List<SelectListItem> ProductsSelectList { get; set; }


                //public List<KMPlatform.Entity.Client> Clients { get; set; }

                //public List<KMPlatform.Entity.ClientGroup> ClientGroups { get; set; }

                public List<KMPlatform.Entity.User.EcnAccountsUserListGridViewModel> UserDropDown
                {
                    get;
                    set;
                }


                //private SelectList clientGroupItems = new SelectList(new List<KMPlatform.Entity.ClientGroup>());
                //public List<KMPlatform.Entity.ClientGroup> ClientGroupItems
                //{
                //    get;
                //    set;
                //}

                //private SelectList clientItems = new SelectList(new List<KMPlatform.Entity.Client>());
                //public List<KMPlatform.Entity.Client> ClientItems
                //{
                //    get;
                //    set;

                //}

                //private SelectList Products = new SelectList(new List<KMPlatform.Object.Product>());
                //public List<KMPlatform.Object.Product> ProductItems { get; set; }
            }

            public class ClientDropDownViewModel
            {
                public string AccountChange { get; set; }
                public int CurrentClientGroupID { get; set; }
                public int CurrentClientID { get; set; }
                public int CurrentProductID { get; set; }
                public int SelectedClientGroupID { get; set; }
                public int SelectedClientID { get; set; }
                public int SelectedProductID { get; set; }

            }
        }
    }
}
