using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Reflection;
using KMEnums;
using KMModels.Controls;
using KMModels.Controls.Standard.Common;
using KMModels.Controls.Standard.Uncommon;

namespace KMModels.PostModels
{
    public class FormControlsPostModel : PostModelBase
    {
        private const string ControlPrefix = "KMModels.Controls.";
        private const string StandardPrefix = "Standard.";
        private const string SnippetMacros = "%%<span cid=\"{0}\">";
        public int CustomerID;
        private static string GetStandardControlPrefix(ControlType type)
        {
            string prefix = null;
            if (Enum.IsDefined(typeof(CommonControlType), (int)type))
            {
                prefix = "Common.";
            }
            else if (Enum.IsDefined(typeof(UncommonControlType), (int)type))
            {
                prefix = "Uncommon.";
            }
            return ControlPrefix + StandardPrefix + prefix;
        }

        public FormControlsPostModel()
        {
            dict = new Dictionary<ControlType, IEnumerable>();
            standart_dict = new Dictionary<ControlType, Control>();

            GetList<TextBox>();
            GetList<TextArea>();
            GetList<DropDown>();

            GetList<RadioButton>();
            GetList<CheckBox>();
            GetList<ListBox>();
            GetList<Grid>();
            GetList<Literal>();
            GetList<NewsLetter>();
            GetList<PageBreak>();
            GetList<Hidden>();
            GetList<Captcha>();

            //TextBox = new List<TextBox>();
            //DropDown = new List<DropDown>();
            //TextArea = new List<TextArea>();
            //RadioButton = new List<RadioButton>();
            //CheckBox = new List<CheckBox>();
            //ListBox = new List<ListBox>();
            //Grid = new List<Grid>();
            //Literal = new List<Literal>();
            //NewsLetter = new List<NewsLetter>();
            //PageBreak = new List<PageBreak>();
            //Hidden = new List<Hidden>();
            //Captcha = new List<Captcha>();
        }

        public int Id { get; set; }

        public FormType FormType { get; set; }

        public string Header { get; set; }
        public string Footer { get; set; }
        public string HeaderJs { get; set; }
        public string FooterJs { get; set; }

        #region Common

        public FirstName FirstName
        {
            get
            {
                return GetStandartControl<FirstName>();
            }
            set
            {
                AddStandartControl(value);
            }
        }

        public LastName LastName
        {
            get
            {
                return GetStandartControl<LastName>();
            }
            set
            {
                AddStandartControl(value);
            }
        }

        public Email Email
        {
            get
            {
                return GetStandartControl<Email>();
            }
            set
            {
                AddStandartControl(value);
            }
        }

        public Phone Phone
        {
            get
            {
                return GetStandartControl<Phone>();
            }
            set
            {
                AddStandartControl(value);
            }
        }

        public Fax Fax
        {
            get
            {
                return GetStandartControl<Fax>();
            }
            set
            {
                AddStandartControl(value);
            }
        }

        public Website Website
        {
            get
            {
                return GetStandartControl<Website>();
            }
            set
            {
                AddStandartControl(value);
            }
        }

        public Address1 Address1
        {
            get
            {
                return GetStandartControl<Address1>();
            }
            set
            {
                AddStandartControl(value);
            }
        }

        public Address2 Address2
        {
            get
            {
                return GetStandartControl<Address2>();
            }
            set
            {
                AddStandartControl(value);
            }
        }

        public City City
        {
            get
            {
                return GetStandartControl<City>();
            }
            set
            {
                AddStandartControl(value);
            }
        }

        public State State
        {
            get
            {
                return GetStandartControl<State>();
            }
            set
            {
                AddStandartControl(value);
            }
        }

        public Zip Zip
        {
            get
            {
                return GetStandartControl<Zip>();
            }
            set
            {
                AddStandartControl(value);
            }
        }

        public Country Country
        {
            get
            {
                return GetStandartControl<Country>();
            }
            set
            {
                AddStandartControl(value);
            }
        }

        #endregion Common

        #region Uncommon

        public Title Title
        {
            get
            {
                return GetStandartControl<Title>();
            }
            set
            {
                AddStandartControl(value);
            }
        }

        public FullName FullName
        {
            get
            {
                return GetStandartControl<FullName>();
            }
            set
            {
                AddStandartControl(value);
            }
        }

        public Company Company
        {
            get
            {
                return GetStandartControl<Company>();
            }
            set
            {
                AddStandartControl(value);
            }
        }

        public Occupation Occupation
        {
            get
            {
                return GetStandartControl<Occupation>();
            }
            set
            {
                AddStandartControl(value);
            }
        }

        public Mobile Mobile
        {
            get
            {
                return GetStandartControl<Mobile>();
            }
            set
            {
                AddStandartControl(value);
            }
        }

        public Age Age
        {
            get
            {
                return GetStandartControl<Age>();
            }
            set
            {
                AddStandartControl(value);
            }
        }

        public Income Income
        {
            get
            {
                return GetStandartControl<Income>();
            }
            set
            {
                AddStandartControl(value);
            }
        }

        public Gender Gender
        {
            get
            {
                return GetStandartControl<Gender>();
            }
            set
            {
                AddStandartControl(value);
            }
        }




        public User1 User1
        {
            get
            {
                return GetStandartControl<User1>();
            }
            set
            {
                AddStandartControl(value);
            }
        }

        public User2 User2
        {
            get
            {
                return GetStandartControl<User2>();
            }
            set
            {
                AddStandartControl(value);
            }
        }

        public User3 User3
        {
            get
            {
                return GetStandartControl<User3>();
            }
            set
            {
                AddStandartControl(value);
            }
        }

        public User4 User4
        {
            get
            {
                return GetStandartControl<User4>();
            }
            set
            {
                AddStandartControl(value);
            }
        }

        public User5 User5
        {
            get
            {
                return GetStandartControl<User5>();
            }
            set
            {
                AddStandartControl(value);
            }
        }

        public User6 User6
        {
            get
            {
                return GetStandartControl<User6>();
            }
            set
            {
                AddStandartControl(value);
            }
        }

        public Birthdate Birthdate
        {
            get
            {
                return GetStandartControl<Birthdate>();
            }
            set
            {
                AddStandartControl(value);
            }
        }

        public Notes Notes
        {
            get
            {
                return GetStandartControl<Notes>();
            }
            set
            {
                AddStandartControl(value);
            }
        }

        public Password Password
        {
            get
            {
                return GetStandartControl<Password>();
            }
            set
            {
                AddStandartControl(value);
            }
        }

        #endregion Uncommon

        private Dictionary<ControlType, IEnumerable> dict;
        private Dictionary<ControlType, Control> standart_dict;

        private T GetStandartControl<T>() where T : Control
        {
            T res = null;
            ControlType cType = new ControlType();

            cType = (ControlType)Enum.Parse(typeof(ControlType), typeof(T).Name);

            if (standart_dict.ContainsKey(cType))
            {
                res = (T)standart_dict[cType];
            }

            return res;
        }

        private void AddStandartControl(Control c)
        {
            if (c != null)
            {
                AddStandartControl((ControlType)Enum.Parse(typeof(ControlType), c.GetType().Name), c);
            }
        }

        private void AddStandartControl(ControlType cType, Control c)
        {
            if (standart_dict.ContainsKey(cType))
            {
                throw new Exception("control dublicate: cType is " + cType.ToString() + ", id " + c.Id);
                //standart_dict[cType] = c;
            }
            else
            {
                standart_dict.Add(cType, c);
            }
        }

        private List<T> GetList<T>() where T : Control
        {
            ControlType cType = (ControlType)Enum.Parse(typeof(ControlType), typeof(T).Name);
            if (!dict.ContainsKey(cType))
            {
                dict[cType] = new List<T>();
            }

            return (List<T>)dict[cType];
        }

        private IList GetList(ControlType cType)
        {
            return (IList)dict[cType];
        }

        public List<TextBox> TextBox
        {
            get
            {
                return GetList<TextBox>();
            }
        }

        public List<DropDown> DropDown
        {
            get
            {
                return GetList<DropDown>();
            }
        }

        public List<TextArea> TextArea
        {
            get
            {
                return GetList<TextArea>();
            }
        }

        public List<RadioButton> RadioButton
        {
            get
            {
                return GetList<RadioButton>();
            }
        }

        public List<CheckBox> CheckBox
        {
            get
            {
                return GetList<CheckBox>();
            }
        }

        public List<ListBox> ListBox
        {
            get
            {
                return GetList<ListBox>();
            }
        }

        public List<Grid> Grid
        {
            get
            {
                return GetList<Grid>();
            }
        }

        public List<Literal> Literal
        {
            get
            {
                return GetList<Literal>();
            }
        }

        public List<NewsLetter> NewsLetter
        {
            get
            {
                return GetList<NewsLetter>();
            }
        }

        public List<PageBreak> PageBreak
        {
            get
            {
                return GetList<PageBreak>();
            }
        }

        public List<Hidden> Hidden
        {
            get
            {
                return GetList<Hidden>();
            }
        }

        public List<Captcha> Captcha
        {
            get
            {
                return GetList<Captcha>();
            }
        }

        public List<string> OldGrids { get; set; }




        public IEnumerable<Control> GetCommonControls()
        {
            var controls = new List<Control>();

            if (FirstName != null) { controls.Add(FirstName); }
            if (LastName != null) { controls.Add(LastName); }
            if (Email != null) { controls.Add(Email); }
            if (Phone != null) { controls.Add(Phone); }
            if (Fax != null) { controls.Add(Fax); }
            if (Website != null) { controls.Add(Website); }
            if (Address1 != null) { controls.Add(Address1); }
            if (Address2 != null) { controls.Add(Address2); }
            if (City != null) { controls.Add(City); }
            if (State != null) { controls.Add(State); }
            if (Zip != null) { controls.Add(Zip); }
            if (Country != null) { controls.Add(Country); }

            return controls;
        }

        public IEnumerable<Control> GetUncommonControls()
        {
            var controls = new List<Control>();

            if (Title != null) { controls.Add(Title); }
            if (FullName != null) { controls.Add(FullName); }
            if (Company != null) { controls.Add(Company); }
            if (Occupation != null) { controls.Add(Occupation); }
            if (Mobile != null) { controls.Add(Mobile); }
            if (Age != null) { controls.Add(Age); }
            if (Income != null) { controls.Add(Income); }
            if (Gender != null) { controls.Add(Gender); }
            //if (UncommonDropDown!= null) { controls.Add(UncommonDropDown); }
            if (User1 != null) { controls.Add(User1); }
            if (User2 != null) { controls.Add(User2); }
            if (User3 != null) { controls.Add(User3); }
            if (User4 != null) { controls.Add(User4); }
            if (User5 != null) { controls.Add(User5); }
            if (User6 != null) { controls.Add(User6); }
            if (Birthdate != null) { controls.Add(Birthdate); }
            if (Notes != null) { controls.Add(Notes); }
            if (Password != null) { controls.Add(Password); }

            return controls;
        }

        public IEnumerable<Control> GetCustomControls()
        {
            var controls = new List<Control>();

            if (TextBox != null) controls.AddRange(TextBox);
            if (DropDown != null) controls.AddRange(DropDown);
            if (TextArea != null) controls.AddRange(TextArea);
            if (RadioButton != null) controls.AddRange(RadioButton);
            if (CheckBox != null) controls.AddRange(CheckBox);
            if (ListBox != null) controls.AddRange(ListBox);
            if (Grid != null) controls.AddRange(Grid);
            if (Literal != null) controls.AddRange(Literal);
            if (NewsLetter != null) controls.AddRange(NewsLetter);

            return controls;
        }

        public IEnumerable<Control> GetOtherControls()
        {
            var controls = new List<Control>();

            if (PageBreak != null) controls.AddRange(PageBreak);
            if (Hidden != null) controls.AddRange(Hidden);
            if (Captcha != null) controls.AddRange(Captcha);

            return controls;
        }

        public IEnumerable<Control> GetAllControls()
        {
            var controls = new List<Control>();

            controls.AddRange(GetCommonControls());
            controls.AddRange(GetUncommonControls());
            controls.AddRange(GetCustomControls());
            controls.AddRange(GetOtherControls());

            return controls;
        }



        public void FillData(KMEntities.Form form, IEnumerable<KMEntities.Control> controls, IEnumerable<KMEntities.ControlProperty> properties)
        {
            int lastId = controls.Last().Control_ID;
            foreach (var control in controls)
            {
                //fill
                bool isStandard = control.ControlType.MainType_ID.HasValue;
                ControlType cType = (ControlType)control.Type_Seq_ID;
                Type T = Assembly.GetExecutingAssembly().GetType((isStandard ? GetStandardControlPrefix(cType) : ControlPrefix) + cType.ToString());
                Control c = (Control)Activator.CreateInstance(T);
                c.Fill(control, properties);

                //set usage
                bool hasRules = control.Rules.Count > 0;
                bool hasNotifications = false;
                bool hasOutput = control.ThirdPartyQueryValues.Count > 0;
                foreach (var cond in control.Conditions)
                {
                    var gr = cond.ConditionGroup;
                    while (gr.ConditionGroup2 != null)
                    {
                        gr = gr.ConditionGroup2;
                    }
                    if (!hasRules)
                    {
                        hasRules = gr.Rules.Count > 0;
                    }
                    if (!hasNotifications)
                    {
                        hasNotifications = gr.Notifications.Count > 0;
                    }
                    if (hasRules && hasNotifications)
                    {
                        break;
                    }
                }
                bool hasNotificationTemplates = form.Notifications.Any(x => x.Message.Contains(GetSnippedByID(control.Control_ID)));
                if (!hasRules)
                    hasRules = control.RequestQueryValues.Count > 0;

                c.SetUsage(hasRules, hasNotifications, hasOutput, hasNotificationTemplates);

                //set default property
                c.Default = cType == ControlType.Email || c.Id == lastId;

                //add
                if (isStandard)
                {
                    AddStandartControl(cType, c);
                }
                else
                {
                    GetList(cType).Add(c);
                }
            }
        }

        public static string GetSnippedByID(int cid)
        {
            return string.Format(SnippetMacros, cid);
        }
    }
}
