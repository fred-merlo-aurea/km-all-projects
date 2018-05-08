using System.Collections.Specialized;
using System.Data;
using System.Linq;

namespace ECN2FPImtrimDataExport.Tests.engine
{
    public partial class Ecn2FpIntrimDataExportTest
    {
        private DataTable _dataTable;
        private NameValueCollection _rows;

        private static readonly NameValueCollection CustomValues = new NameValueCollection
        {
            [CreatedOnColumn] = CreatedOn,
            [LastChangedColumn] = LastChanged,
            [ZipColumn] = Zip,
            [EmailIDColumn] = EmailID
        };

        private void SetupExportDataTable()
        {
            _dataTable = new DataTable();
            _rows = new NameValueCollection();

            AddDemographicColumns();
            AddFunctionColumns();
            AddDemoColumns();
            AddBusinessColumns();
            AddJobColumns();
            AddMarketColumns();
            AddOtherColumns();

            var row = _dataTable.NewRow();

            foreach (var key in _rows.AllKeys)
            {
                row[key] = _rows[key];
            }

            _dataTable.Rows.Add(row);
        }

        private void AddOtherColumns()
        {
            AddColumn(Paymentstatus);
            AddColumn(Plastics);
            AddColumn(Plasticstext);
            AddColumn(Prod1);
            AddColumn(Prod1Text);
            AddColumn(Products);
            AddColumn(Productstext);
            AddColumn(Publicationcode);
            AddColumn(Qsource);
            AddColumn(Sales);
            AddColumn(Salutation);
            AddColumn(Subscriberid);
            AddColumn(Subscription);
            AddColumn(Subsrc);
            AddColumn(Toe1);
            AddColumn(Toe1Text);
            AddColumn(Toe2);
            AddColumn(Transactiontype);
            AddColumn(Verify);
            AddColumn(Xact);
        }

        private void AddMarketColumns()
        {
            AddColumn(Mailstop);
            AddColumn(Marketcln);
            AddColumn(Marketfacl);
            AddColumn(Marketrail);
            AddColumn(Medium);
            AddColumn(Pa1Email);
            AddColumn(Pa1Fname);
            AddColumn(Pa1Function);
            AddColumn(Pa1Functxt);
            AddColumn(Pa1Lname);
            AddColumn(Pa1Title);
            AddColumn(Pa2Email);
            AddColumn(Pa2Fname);
            AddColumn(Pa2Function);
            AddColumn(Pa2Functxt);
            AddColumn(Pa2Lname);
            AddColumn(Pa2Title);
            AddColumn(Pa3Email);
            AddColumn(Pa3Fname);
            AddColumn(Pa3Function);
            AddColumn(Pa3Functxt);
            AddColumn(Pa3Lname);
            AddColumn(Pa3Title);
            AddColumn(Par3C);
        }

        private void AddJobColumns()
        {
            AddColumn(Aoi1);
            AddColumn(Aoi2);
            AddColumn(Aoi3);
            AddColumn(Batch);
            AddColumn(Buyauth);
            AddColumn(Headlineid);
            AddColumn(Histbatch);
            AddColumn(Ind1);
            AddColumn(Jobt1);
            AddColumn(Jobt1Text);
            AddColumn(Jobt2);
            AddColumn(Jobt2Text);
            AddColumn(Jobt3);
            AddColumn(LastChangedColumn);
        }

        private void AddFunctionColumns()
        {
            AddColumn(Functext6);
            AddColumn(Functext7);
            AddColumn(Functext8);
            AddColumn(Functext9);
            AddColumn(Function);
            AddColumn(Function1);
            AddColumn(Function1Txt);
            AddColumn(Function2);
            AddColumn(Function2Txt);
            AddColumn(Function3);
            AddColumn(Function3Txt);
            AddColumn(Function4);
            AddColumn(Function4Txt);
            AddColumn(Function5);
            AddColumn(Function5Txt);
            AddColumn(Functiontext);
        }

        private void AddDemoColumns()
        {
            AddColumn(Demo1);
            AddColumn(Demo10);
            AddColumn(Demo10Text);
            AddColumn(Demo11);
            AddColumn(Demo11Text);
            AddColumn(Demo11Textherbs);
            AddColumn(Demo11Textingr);
            AddColumn(Demo11Textpckg);
            AddColumn(Demo12);
            AddColumn(Demo13);
            AddColumn(Demo13Text);
            AddColumn(Demo14);
            AddColumn(Demo14Text);
            AddColumn(Demo15);
            AddColumn(Demo16);
            AddColumn(Demo2);
            AddColumn(Demo20);
            AddColumn(Demo21);
            AddColumn(Demo22);
            AddColumn(Demo23);
            AddColumn(Demo24);
            AddColumn(Demo25);
            AddColumn(Demo26);
            AddColumn(Demo27);
            AddColumn(Demo28);
            AddColumn(Demo29);
            AddColumn(Demo3);
            AddColumn(Demo31);
            AddColumn(Demo32);
            AddColumn(Demo33);
            AddColumn(Demo34);
            AddColumn(Demo34A);
            AddColumn(Demo34B);
            AddColumn(Demo35);
            AddColumn(Demo35A);
            AddColumn(Demo35B);
            AddColumn(Demo36);
            AddColumn(Demo36A);
            AddColumn(Demo36B);
            AddColumn(Demo37);
            AddColumn(Demo38);
            AddColumn(Demo38Text);
            AddColumn(Demo39);
            AddColumn(Demo4);
            AddColumn(Demo40);
            AddColumn(Demo41);
            AddColumn(Demo42);
            AddColumn(Demo43);
            AddColumn(Demo44);
            AddColumn(Demo45);
            AddColumn(Demo46);
            AddColumn(Demo5);
            AddColumn(Demo6);
            AddColumn(Demo6Text);
            AddColumn(Demo7);
            AddColumn(Demo8);
            AddColumn(Demo9);
            AddColumn(Demo9Text);
        }

        private void AddBusinessColumns()
        {
            AddColumn(Business);
            AddColumn(Business1);
            AddColumn(Business10);
            AddColumn(Business16);
            AddColumn(Business2);
            AddColumn(Business3);
            AddColumn(Business3Text);
            AddColumn(Business4);
            AddColumn(Business5);
            AddColumn(Business6);
            AddColumn(Business7);
            AddColumn(Business8);
            AddColumn(Business8Text);
            AddColumn(Business9);
            AddColumn(Business9Text);
            AddColumn(Businesstext);
            AddColumn(Busntext1);
            AddColumn(Busntext5);
        }

        private void AddDemographicColumns()
        {
            AddColumn(Address);
            AddColumn(Address2);
            AddColumn(AlternateEmail);
            AddColumn(ZipColumn);
            AddColumn(Urloi);
            AddColumn(Urlsu);
            AddColumn(Twitter);
            AddColumn(TwitterHandle);
            AddColumn(Webpgurl);
            AddColumn(Voice);
            AddColumn(State);
            AddColumn(StateInt);
            AddColumn(SecAddress);
            AddColumn(SecAddress2);
            AddColumn(SecCity);
            AddColumn(SecPostalcode);
            AddColumn(SecState);
            AddColumn(Source);
            AddColumn(MexState);
            AddColumn(Mobile);
            AddColumn(Occupation);
            AddColumn(Cat);
            AddColumn(City);
            AddColumn(Company);
            AddColumn(Companytext);
            AddColumn(Country);
            AddColumn(CreatedOnColumn);
            AddColumn(Campaign);
            AddColumn(EmailAddress);
            AddColumn(EmailIDColumn);
            AddColumn(Employ);
            AddColumn(Fax);
            AddColumn(FirstName);
            AddColumn(Formtype);
            AddColumn(Forzip);
            AddColumn(LastName);
        }

        private void AddColumn(string name)
        {
            _dataTable.Columns.Add(name + _invalidValue);

            var key = CustomValues.AllKeys.FirstOrDefault(x => x == name);
            _rows.Add(name + _invalidValue, string.IsNullOrWhiteSpace(key) ? name : CustomValues[key]);
        }
    }
}
