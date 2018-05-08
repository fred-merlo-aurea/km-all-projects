using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;

namespace ecn.MarketingAutomation.Models.PostModels
{
    public class MarketingAutomationPostModel
    {
        public MarketingAutomationPostModel()
        {
            controlDict = new Dictionary<ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType, IEnumerable>();
            standart_dict = new Dictionary<ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType, ControlBase>();

            GetList<PostModels.Controls.CampaignItem>();
            GetList<PostModels.Controls.Click>();
            GetList<PostModels.Controls.Direct_Click>();
            GetList<PostModels.Controls.Direct_NoOpen>();
            GetList<PostModels.Controls.Direct_Open>();
            GetList<PostModels.Controls.Group>();
            GetList<PostModels.Controls.NoClick>();
            GetList<PostModels.Controls.NoOpen>();
            GetList<PostModels.Controls.NotSent>();
            GetList<PostModels.Controls.Open>();
            GetList<PostModels.Controls.Open_NoClick>();
            GetList<PostModels.Controls.Sent>();
            GetList<PostModels.Controls.Subscribe>();
            GetList<PostModels.Controls.Suppressed>();
            GetList<PostModels.Controls.Unsubscribe>();
            GetList<PostModels.Controls.Form>();
            GetList<PostModels.Controls.FormSubmit>();
            GetList<PostModels.Controls.FormAbandon>();
            GetList<PostModels.Controls.Wait>();
            GetList<Controls.End>();

            Connectors = new List<Connector>();

        }

        public int MarketingAutomationID { get; set; }
        public string Name { get; set; }

        public int CustomerID { get; set; }

        public string CustomerName { get; set; }
        
        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }

        public int CreatedUserID { get; set; }
        
        public int UpdatedUserID { get; set; }

        public bool IsDeleted { get; set; }

        public string Goal { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string JSONDiagram { get; set; }

        public string JSONDeleted { get; set; }

        public ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationStatus State { get; set; }

        public List<PostModels.ControlBase> Controls { get; set; }

        public List<PostModels.Connector> Connectors { get; set; }

        private Dictionary<ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType, IEnumerable> controlDict;
        private Dictionary<ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType, ControlBase> standart_dict;


        public List<PostModels.Controls.CampaignItem> CampaignItem
        {
            get
            {
                return GetList<PostModels.Controls.CampaignItem>();
            }
        }
        public List<PostModels.Controls.Click> Click
        {
            get
            {
                return GetList<PostModels.Controls.Click>();
            }
        }
        public List<PostModels.Controls.Direct_Click> Direct_Click
        {
            get
            {
                return GetList<PostModels.Controls.Direct_Click>();
            }
        }
        public List<PostModels.Controls.Form> Form
        {
            get
            {
                return GetList<PostModels.Controls.Form>();
            }
        }
        public List<PostModels.Controls.FormSubmit> FormSubmit
        {
            get
            {
                return GetList<PostModels.Controls.FormSubmit>();
            }
        }
        public List<PostModels.Controls.FormAbandon> FormAbandon
        {
            get
            {
                return GetList<PostModels.Controls.FormAbandon>();
            }
        }
        public List<PostModels.Controls.Direct_NoOpen> Direct_NoOpen
        {
            get
            {
                return GetList<PostModels.Controls.Direct_NoOpen>();
            }
        }
        public List<PostModels.Controls.Direct_Open> Direct_Open
        {
            get
            {
                return GetList<PostModels.Controls.Direct_Open>();
            }
        }
        public List<PostModels.Controls.End> End
        {
            get
            {
                return GetList<PostModels.Controls.End>();
            }
            
        }

        public PostModels.Controls.Start Start
        {
            get
            {
                return GetStandartControl<PostModels.Controls.Start>();
            }
            set
            {
                AddStandartControl(value);
            }
        }
        public List<PostModels.Controls.Group> Group
        {
            get
            {
                return GetList<PostModels.Controls.Group>();
            }
        }
        public List<PostModels.Controls.NoClick> NoClick
        {
            get
            {
                return GetList<PostModels.Controls.NoClick>();
            }
        }
        public List<PostModels.Controls.NoOpen> NoOpen
        {
            get
            {
                return GetList<PostModels.Controls.NoOpen>();
            }
        }
        public List<PostModels.Controls.NotSent> NotSent
        {
            get
            {
                return GetList<PostModels.Controls.NotSent>();
            }
        }
        public List<PostModels.Controls.Open> Open
        {
            get
            {
                return GetList<PostModels.Controls.Open>();
            }
        }
        public List<PostModels.Controls.Open_NoClick> Open_NoClick
        {
            get
            {
                return GetList<PostModels.Controls.Open_NoClick>();
            }
        }
        public List<PostModels.Controls.Sent> Sent
        {
            get
            {
                return GetList<PostModels.Controls.Sent>();
            }
        }
        public List<PostModels.Controls.Subscribe> Subscribe
        {
            get
            {
                return GetList<PostModels.Controls.Subscribe>();
            }
        }

        public List<PostModels.Controls.Suppressed> Suppressed
        {
            get
            {
                return GetList<PostModels.Controls.Suppressed>();
            }
        }
        public List<PostModels.Controls.Unsubscribe> Unsubscribe
        {
            get
            {
                return GetList<PostModels.Controls.Unsubscribe>();
            }
        }
        public List<PostModels.Controls.Wait> Wait
        {
            get
            {
                return GetList<PostModels.Controls.Wait>();
            }
        }
        

        private List<T> GetList<T>() where T : ControlBase
        {
            ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType cType = (ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType)Enum.Parse(typeof(ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType), typeof(T).Name);
            if (!controlDict.ContainsKey(cType))
            {
                controlDict[cType] = new List<T>();
            }

            return (List<T>)controlDict[cType];
        }

        public IEnumerable<ControlBase> GetCustomControls()
        {
            var controls = new List<ControlBase>();

            if (CampaignItem != null) controls.AddRange(CampaignItem);
            if (Click != null) controls.AddRange(Click);
            if (Direct_Click != null) controls.AddRange(Direct_Click);
            if (Form != null) controls.AddRange(Form);
            if (Direct_NoOpen != null) controls.AddRange(Direct_NoOpen);
            if (Direct_Open != null) controls.AddRange(Direct_Open);
            if (Group != null) controls.AddRange(Group);
            if (NoClick != null) controls.AddRange(NoClick);
            if (NoOpen != null) controls.AddRange(NoOpen);
            if (NotSent != null) controls.AddRange(NotSent);
            if (Open != null) controls.AddRange(Open);
            if (Open_NoClick != null) controls.AddRange(Open_NoClick);
            if (Sent != null) controls.AddRange(Sent);
            if (Subscribe != null) controls.AddRange(Subscribe);
            if (Suppressed != null) controls.AddRange(Suppressed);
            if (Unsubscribe != null) controls.AddRange(Unsubscribe);
            if (Wait != null) controls.AddRange(Wait);
            if (End != null) controls.AddRange(End);
            if (FormSubmit != null) controls.AddRange(FormSubmit);
            if (FormAbandon != null) controls.AddRange(FormAbandon);
            return controls;
        }

        public IEnumerable<ControlBase> GetOneTimeControls()
        {
            var controls = new List<ControlBase>();

            if(Start != null) { controls.Add(Start); }
            //if(End != null) { controls.Add(End); }

            return controls;
        }

        private T GetStandartControl<T>() where T : ControlBase
        {
            T res = null;
            ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType cType = new ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType();

            cType = (ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType)Enum.Parse(typeof(ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType), typeof(T).Name);

            if (standart_dict.ContainsKey(cType))
            {
                res = (T)standart_dict[cType];
            }

            return res;
        }

        private void AddStandartControl(ControlBase c)
        {
            if (c != null)
            {
                AddStandartControl((ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType)Enum.Parse(typeof(ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType), c.GetType().Name), c);
            }
        }

        private void AddStandartControl(ECN_Framework_Common.Objects.Communicator.Enums.MarketingAutomationControlType cType, ControlBase c)
        {
            if (standart_dict.ContainsKey(cType))
            {
                throw new Exception("control dublicate: cType is " + cType.ToString() + ", id " + c.ControlID);
                //standart_dict[cType] = c;
            }
            else
            {
                standart_dict.Add(cType, c);
            }
        }

        public List<ControlBase> GetAllControls()
        {
            var controls = new List<ControlBase>();

            controls.AddRange(GetOneTimeControls());            
            controls.AddRange(GetCustomControls());            

            return controls;
        }
    }
}