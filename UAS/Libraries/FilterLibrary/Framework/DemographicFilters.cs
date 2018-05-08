using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace FilterControls.Framework
{
    public class DemographicFilters : Filters
    {
        private FrameworkServices.ServiceClient<UAD_WS.Interface.IResponseGroup> responseGroupW = FrameworkServices.ServiceClient.UAD_ResponseGroupClient();
        private FrameworkServices.ServiceClient<UAD_WS.Interface.ICodeSheet> codeSheetW = FrameworkServices.ServiceClient.UAD_CodeSheetClient();
        private FrameworkServices.ServiceClient<UAD_Lookup_WS.Interface.ICode> codeW = FrameworkServices.ServiceClient.UAD_Lookup_CodeClient();
        private FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.ResponseGroup>> response = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.ResponseGroup>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.CodeSheet>> csResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD.Entity.CodeSheet>>();
        private FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>> codeResponse = new FrameworkUAS.Service.Response<List<FrameworkUAD_Lookup.Entity.Code>>();
        public override Framework.Enums.Filters FilterType { get; set; }
        public override string Title { get; set; }
        public override ObservableCollection<FilterObject> Objects { get; set; }

        public DemographicFilters(int pubID)
        {
            this.FilterType = Framework.Enums.Filters.Demographic;
            this.Title = this.FilterType.ToString();
            Objects = new ObservableCollection<FilterObject>();

            response = responseGroupW.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections,
                pubID);

            csResponse = codeSheetW.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, pubID, FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.User.CurrentClient.ClientConnections);

            List<int> ids = new List<int>();

            codeResponse = codeW.Proxy.Select(FrameworkUAS.Object.AppData.myAppData.AuthorizedUser.AuthAccessKey, FrameworkUAD_Lookup.Enums.CodeType.Response_Group);
            if(Common.CheckResponse(codeResponse.Result, codeResponse.Status))
            {
                ids = codeResponse.Result.Where(x => x.CodeName != FrameworkUAD_Lookup.Enums.ResponseGroupTypes.UAD_Only.ToString().Replace("_", " ")).ToList().Select(x => x.CodeId).ToList();
            }

            if(response.Result != null && response.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success && csResponse.Result != null &&
                csResponse.Status == FrameworkUAD_Lookup.Enums.ServiceResponseStatusTypes.Success)
            {
                foreach(FrameworkUAD.Entity.ResponseGroup rg in response.Result.Where(x=> ids.Contains(x.ResponseGroupTypeId) && x.IsActive == true))
                {
                    List<ListObject> options = new List<ListObject>();
                    List<FrameworkUAD.Entity.CodeSheet> answers = csResponse.Result.Where(x => x.ResponseGroupID == rg.ResponseGroupID && x.IsActive == true).OrderBy(x=> x.DisplayOrder).ToList();
                    if (answers != null)
                        answers.ForEach(x => options.Add(new ListObject(x.ResponseValue + ". " + x.ResponseDesc, x.CodeSheetID.ToString(), x.ResponseGroupID.ToString())));
                    ListObject anyResponse = new ListObject("YY. Any Response", "YY", rg.ResponseGroupID.ToString());
                    anyResponse.PropertyChanged += SpecialResponseChanged;
                    ListObject noResponse = new ListObject("ZZ. No Response", "ZZ", rg.ResponseGroupID.ToString());
                    noResponse.PropertyChanged += SpecialResponseChanged;
                    options.Add(anyResponse);
                    options.Add(noResponse);
                    Objects.Add(new ListFilterObject(this.FilterType, Enums.FilterObjects.Responses, rg.DisplayName, options));
                }
            }
        }

        private void SpecialResponseChanged(object sender, PropertyChangedEventArgs e)
        {
            //ListObject response = sender as ListObject;
            //if (response.Selected)
            //{
            //    foreach (ListObject lo in Objects.Cast<ListFilterObject>().Where(x=> x.Name == response.ParentValue).FirstOrDefault().Options.Where(x => x.ParentValue == response.ParentValue 
            //        && x.Value != response.Value))
            //    {
            //        if (lo.Selected != false)
            //            lo.Selected = false;
            //    }
            //}            
        }
    }
}
