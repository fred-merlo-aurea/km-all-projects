using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlClient.Fakes;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using FrameworkUAD.DataAccess;
using FrameworkUAD.DataAccess.Fakes;
using Microsoft.QualityTools.Testing.Fakes;
using NUnit.Framework;
using Shouldly;
using UAS.UnitTests.Helpers;
using Entity = FrameworkUAD.Object;
using KMFakes = KM.Common.Fakes;
using ClientConnections = KMPlatform.Object.ClientConnections;

namespace UAS.UnitTests.FrameworkUAD.DataAccess
{
    /// <summary>
    /// Unit Tests for <see cref="CircImportExport"/>
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class CircImportExportTest
    {
        private const int Rows = 5;
        private const int PublisherId = 1;
        private const int PublicationId = 2;
        private const int UserId = 4;
        private const string ProcSelect = "o_ExportData_Select";
        private const string ProcSelectPublisher = "o_ExportData_Select_Publisher_Publication";
        private const string ProcCircUpdateBulkSql = "o_ExportData_RunImportSave";
        private static readonly ClientConnections Client = new ClientConnections();

        private IDisposable _context;
        private DataTable _dataTable;
        private IList<Entity.CircImportExport> _list;
        private Entity.CircImportExport _objWithRandomValues;
        private Entity.CircImportExport _objWithDefaultValues;
        private SqlCommand _sqlCommand;

        [SetUp]
        public void SetUp()
        {
            _context = ShimsContext.Create();
            _dataTable = new DataTable();

            _objWithRandomValues = typeof(Entity.CircImportExport).CreateInstance();
            _objWithDefaultValues = typeof(Entity.CircImportExport).CreateInstance();

            _list = new List<Entity.CircImportExport>
            {
                _objWithRandomValues,
                _objWithDefaultValues
            };
            SetupFakes();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public void Select_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = CircImportExport.Select(Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelect),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectPublisher_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = CircImportExport.Select(PublisherId, PublicationId, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@PublisherID"].Value.ShouldBe(PublisherId),
                () => _sqlCommand.Parameters["@PublicationID"].Value.ShouldBe(PublicationId),
                () => result.ShouldNotBeNull(),
                () => result.IsListContentMatched(_list.ToList()).ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectPublisher),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void SelectDataTable_WhenCalled_VerifySqlParameters()
        {
            // Arrange, Act
            var result = CircImportExport.SelectDataTable(PublisherId, PublicationId, Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@PublisherID"].Value.ShouldBe(PublisherId),
                () => _sqlCommand.Parameters["@PublicationID"].Value.ShouldBe(PublicationId),
                () => result.ShouldBe(_dataTable),
                () => _sqlCommand.CommandText.ShouldBe(ProcSelectPublisher),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        [Test]
        public void Get_WhenCalled_VerifyReturnItem()
        {
            // Arrange, Act
            var result = typeof(CircImportExport).CallMethod("Get", new object[] { new SqlCommand() });

            // Assert
            result.ShouldBe(_objWithDefaultValues);
        }

        [Test]
        public void CircUpdateBulkSql_WhenCalled_VerifySqlParameters()
        {
            // Arrange
            var index = 1;
            var xml = new StringBuilder();
            xml.AppendLine("<XML>");
            foreach (var item in _list)
            {
                xml.AppendLine("<Subscriber>");
                AppendDemographicData(xml, index, item);
                AppendBusinessData(xml, item);
                AppendDemoData(xml, item);
                AppendAccountData(xml, item);
                xml.AppendLine("</Subscriber>");
                index++;
            }
            xml.AppendLine("</XML>");
            // Act
            var result = CircImportExport.CircUpdateBulkSql(UserId, _list.ToList(), Client);

            // Assert
            _sqlCommand.ShouldSatisfyAllConditions(
                () => _sqlCommand.Parameters["@UserID"].Value.ShouldBe(UserId),
                () => _sqlCommand.Parameters["@xml"].Value.ShouldBe(xml.ToString()),
                () => result.ShouldBeTrue(),
                () => _sqlCommand.CommandText.ShouldBe(ProcCircUpdateBulkSql),
                () => _sqlCommand.CommandType.ShouldBe(CommandType.StoredProcedure));
        }

        private static void AppendAccountData(StringBuilder xml, Entity.CircImportExport item)
        {
            xml.AppendLine($"<SECBUS>{item.SECBUS}</SECBUS>");
            xml.AppendLine($"<SECFUNC>{item.SECFUNC}</SECFUNC>");
            xml.AppendLine($"<Business1>{item.Business1}</Business1>");
            xml.AppendLine($"<Function1>{item.Function1}</Function1>");
            xml.AppendLine($"<Income1>{item.Income1}</Income1>");
            xml.AppendLine($"<Age1>{item.Age1}</Age1>");
            xml.AppendLine($"<Home_Value>{item.Home_Value}</Home_Value>");
            xml.AppendLine($"<JOBT1>{item.JOBT1}</JOBT1>");
            xml.AppendLine($"<JOBT1TEXT>{item.JOBT1TEXT}</JOBT1TEXT>");
            xml.AppendLine($"<JOBT2>{item.JOBT2}</JOBT2>");
            xml.AppendLine($"<JOBT3>{item.JOBT3}</JOBT3>");
            xml.AppendLine($"<TOE1>{item.TOE1}</TOE1>");
            xml.AppendLine($"<TOE2>{item.TOE2}</TOE2>");
            xml.AppendLine($"<AOI1>{item.AOI1}</AOI1>");
            xml.AppendLine($"<AOI2>{item.AOI2}</AOI2>");
            xml.AppendLine($"<AOI3>{item.AOI3}</AOI3>");
            xml.AppendLine($"<PROD1>{item.PROD1}</PROD1>");
            xml.AppendLine($"<PROD1TEXT>{item.PROD1TEXT}</PROD1TEXT>");
            xml.AppendLine($"<BUYAUTH>{item.BUYAUTH}</BUYAUTH>");
            xml.AppendLine($"<IND1>{item.IND1}</IND1>");
            xml.AppendLine($"<IND1TEXT>{item.IND1TEXT}</IND1TEXT>");
            xml.AppendLine($"<STATUS>{item.STATUS}</STATUS>");
            xml.AppendLine($"<PRICECODE>{item.PRICECODE}</PRICECODE>");
            xml.AppendLine($"<NUMISSUES>{item.NUMISSUES}</NUMISSUES>");
            xml.AppendLine($"<CPRATE>{item.CPRATE}</CPRATE>");
            xml.AppendLine($"<TERM>{item.TERM}</TERM>");
            xml.AppendLine($"<ISSTOGO>{item.ISSTOGO}</ISSTOGO>");
            xml.AppendLine($"<CARDTYPE>{item.CARDTYPE}</CARDTYPE>");
            xml.AppendLine($"<CARDTYPECC>{item.CARDTYPECC}</CARDTYPECC>");
            xml.AppendLine($"<CCNUM>{item.CCNUM}</CCNUM>");
            xml.AppendLine($"<CCEXPIRE>{item.CCEXPIRE}</CCEXPIRE>");
            xml.AppendLine($"<CCNAME>{item.CCNAME}</CCNAME>");
            xml.AppendLine($"<AMOUNTPD>{item.AMOUNTPD}</AMOUNTPD>");
            xml.AppendLine($"<AMOUNT>{item.AMOUNT}</AMOUNT>");
            xml.AppendLine($"<BALDUE>{item.BALDUE}</BALDUE>");
            xml.AppendLine($"<AMTEARNED>{item.AMTEARNED}</AMTEARNED>");
            xml.AppendLine($"<AMTDEFER>{item.AMTDEFER}</AMTDEFER>");
            xml.AppendLine($"<PAYDATE>{item.PAYDATE}</PAYDATE>");
            xml.AppendLine($"<STARTISS>{item.STARTISS}</STARTISS>");
            xml.AppendLine($"<EXPIRE>{item.EXPIRE}</EXPIRE>");
            xml.AppendLine($"<NWEXPIRE>{item.NWEXPIRE}</NWEXPIRE>");
            xml.AppendLine($"<DELIVERCODE>{item.DELIVERCODE}</DELIVERCODE>");
        }

        private static void AppendBusinessData(StringBuilder xml, Entity.CircImportExport item)
        {
            xml.AppendLine($"<ORIGSSRC>{item.ORIGSSRC}</ORIGSSRC>");
            xml.AppendLine($"<SUBSRC>{item.SUBSRC}</SUBSRC>");
            xml.AppendLine($"<Copies>{item.Copies}</Copies>");
            xml.AppendLine($"<NANQ>{item.NANQ}</NANQ>");
            xml.AppendLine($"<Qsource>{item.Qsource}</Qsource>");
            xml.AppendLine($"<Qdate>{item.Qdate}</Qdate>");
            xml.AppendLine($"<Cdate>{item.Cdate}</Cdate>");
            xml.AppendLine($"<Par3C>{item.Par3C}</Par3C>");
            xml.AppendLine($"<EmailID>{item.EmailID}</EmailID>");
            xml.AppendLine($"<Verify>{item.Verify}</Verify>");
            xml.AppendLine($"<Interview>{item.Interview}</Interview>");
            xml.AppendLine($"<Mail>{item.Mail}</Mail>");
            xml.AppendLine($"<Old_Date>{item.Old_Date}</Old_Date>");
            xml.AppendLine($"<Old_QSRC>{item.Old_QSRC}</Old_QSRC>");
            xml.AppendLine($"<MBR_ID>{item.MBR_ID}</MBR_ID>");
            xml.AppendLine($"<MBR_Flag>{item.MBR_Flag}</MBR_Flag>");
            xml.AppendLine($"<MBR_Reject>{item.MBR_Reject}</MBR_Reject>");
            xml.AppendLine($"<SPECIFY>{item.SPECIFY}</SPECIFY    >");
            xml.AppendLine($"<SIC>{item.SIC}</SIC>");
            xml.AppendLine($"<EMPLOY>{item.EMPLOY}</EMPLOY>");
            xml.AppendLine($"<SALES>{item.SALES}</SALES>");
            xml.AppendLine($"<IMB_SERIAL1>{item.IMB_SERIAL1}</IMB_SERIAL1>");
            xml.AppendLine($"<IMB_SERIAL2>{item.IMB_SERIAL2}</IMB_SERIAL2>");
            xml.AppendLine($"<IMB_SERIAL3>{item.IMB_SERIAL3}</IMB_SERIAL3>");
            xml.AppendLine($"<Business>{item.Business}</Business>");
            xml.AppendLine($"<BUSNTEXT>{item.BUSNTEXT}</BUSNTEXT>");
            xml.AppendLine($"<Function>{item.Function}</Function>");
            xml.AppendLine($"<FUNCTEXT>{item.FUNCTEXT}</FUNCTEXT>");
        }

        private static void AppendDemographicData(StringBuilder xml, int index, Entity.CircImportExport item)
        {
            xml.AppendLine($"<UniqueID>{index}</UniqueID>");
            xml.AppendLine($"<SubscriberID>{item.SubscriberID}</SubscriberID>");
            xml.AppendLine($"<PublicationID>{item.PublicationID}</PublicationID>");
            xml.AppendLine($"<SubscriptionID>{item.SubscriptionID}</SubscriptionID>");
            xml.AppendLine($"<Batch>{item.Batch}</Batch>");
            xml.AppendLine($"<Hisbatch>{item.Hisbatch}</Hisbatch>");
            xml.AppendLine($"<Hisbatch1>{item.Hisbatch1}</Hisbatch1>");
            xml.AppendLine($"<Hisbatch2>{item.Hisbatch2}</Hisbatch2>");
            xml.AppendLine($"<Hisbatch3>{item.Hisbatch3}</Hisbatch3>");
            xml.AppendLine($"<Pubcode>{item.Pubcode}</Pubcode>");
            xml.AppendLine($"<SequenceID>{item.SequenceID}</SequenceID>");
            xml.AppendLine($"<Cat>{item.Cat}</Cat>");
            xml.AppendLine($"<Xact>{item.Xact}</Xact>");
            xml.AppendLine($"<XactDate>{item.XactDate}</XactDate>");
            xml.AppendLine($"<Fname>{item.Fname}</Fname>");
            xml.AppendLine($"<Lname>{item.Lname}</Lname>");
            xml.AppendLine($"<Title>{item.Title}</Title>");
            xml.AppendLine($"<Company>{item.Company}</Company>");
            xml.AppendLine($"<Address>{item.Address}</Address>");
            xml.AppendLine($"<Mailstop>{item.Mailstop}</Mailstop>");
            xml.AppendLine($"<City>{item.City}</City>");
            xml.AppendLine($"<State>{item.State}</State>");
            xml.AppendLine($"<ZipCode>{item.ZipCode}</ZipCode>");
            xml.AppendLine($"<Plus4>{item.Plus4}</Plus4>");
            xml.AppendLine($"<County>{item.County}</County>");
            xml.AppendLine($"<Country>{item.Country}</Country>");
            xml.AppendLine($"<CountryID>{item.CountryID}</CountryID>");
            xml.AppendLine($"<Phone>{item.Phone}</Phone>");
            xml.AppendLine($"<Fax>{item.Fax}</Fax>");
            xml.AppendLine($"<Mobile>{item.Mobile}</Mobile>");
            xml.AppendLine($"<Email>{item.Email}</Email>");
            xml.AppendLine($"<Website>{item.Website}</Website>");
            xml.AppendLine($"<AcctNum>{item.AcctNum}</AcctNum>");
        }

        private static void AppendDemoData(StringBuilder xml, Entity.CircImportExport item)
        {
            xml.AppendLine($"<DEMO1>{item.DEMO1}</DEMO1>");
            xml.AppendLine($"<DEMO1TEXT>{item.DEMO1TEXT}</DEMO1TEXT>");
            xml.AppendLine($"<DEMO2>{item.DEMO2}</DEMO2>");
            xml.AppendLine($"<DEMO3>{item.DEMO3}</DEMO3>");
            xml.AppendLine($"<DEMO4>{item.DEMO4}</DEMO4>");
            xml.AppendLine($"<DEMO5>{item.DEMO5}</DEMO5>");
            xml.AppendLine($"<DEMO6>{item.DEMO6}</DEMO6>");
            xml.AppendLine($"<DEMO6TEXT>{item.DEMO6TEXT}</DEMO6TEXT>");
            xml.AppendLine($"<DEMO7>{item.DEMO7}</DEMO7>");
            xml.AppendLine($"<DEMO8>{item.DEMO8}</DEMO8>");
            xml.AppendLine($"<DEMO9>{item.DEMO9}</DEMO9>");
            xml.AppendLine($"<DEMO10>{item.DEMO10}</DEMO10>");
            xml.AppendLine($"<DEMO10TEXT>{item.DEMO10TEXT}</DEMO10TEXT>");
            xml.AppendLine($"<DEMO11>{item.DEMO11}</DEMO11>");
            xml.AppendLine($"<DEMO12>{item.DEMO12}</DEMO12>");
            xml.AppendLine($"<DEMO14>{item.DEMO14}</DEMO14>");
            xml.AppendLine($"<DEMO15>{item.DEMO15}</DEMO15>");
            xml.AppendLine($"<DEMO16>{item.DEMO16}</DEMO16>");
            xml.AppendLine($"<DEMO18>{item.DEMO18}</DEMO18>");
            xml.AppendLine($"<DEMO19>{item.DEMO19}</DEMO19>");
            xml.AppendLine($"<DEMO20>{item.DEMO20}</DEMO20>");
            xml.AppendLine($"<DEMO21>{item.DEMO21}</DEMO21>");
            xml.AppendLine($"<DEMO22>{item.DEMO22}</DEMO22>");
            xml.AppendLine($"<DEMO23>{item.DEMO23}</DEMO23>");
            xml.AppendLine($"<DEMO24>{item.DEMO24}</DEMO24>");
            xml.AppendLine($"<DEMO25>{item.DEMO25}</DEMO25>");
            xml.AppendLine($"<DEMO26>{item.DEMO26}</DEMO26>");
            xml.AppendLine($"<DEMO27>{item.DEMO27}</DEMO27>");
            xml.AppendLine($"<DEMO28>{item.DEMO28}</DEMO28>");
            xml.AppendLine($"<DEMO29>{item.DEMO29}</DEMO29>");
            xml.AppendLine($"<DEMO40>{item.DEMO40}</DEMO40>");
            xml.AppendLine($"<DEMO41>{item.DEMO41}</DEMO41>");
            xml.AppendLine($"<DEMO42>{item.DEMO42}</DEMO42>");
            xml.AppendLine($"<DEMO43>{item.DEMO43}</DEMO43>");
            xml.AppendLine($"<DEMO44>{item.DEMO44}</DEMO44>");
            xml.AppendLine($"<DEMO45>{item.DEMO45}</DEMO45>");
            xml.AppendLine($"<DEMO46>{item.DEMO46}</DEMO46>");
            xml.AppendLine($"<DEMO31>{item.DEMO31}</DEMO31>");
            xml.AppendLine($"<DEMO32>{item.DEMO32}</DEMO32>");
            xml.AppendLine($"<DEMO33>{item.DEMO33}</DEMO33>");
            xml.AppendLine($"<DEMO34>{item.DEMO34}</DEMO34>");
            xml.AppendLine($"<DEMO35>{item.DEMO35}</DEMO35>");
            xml.AppendLine($"<DEMO36>{item.DEMO36}</DEMO36>");
            xml.AppendLine($"<DEMO37>{item.DEMO37}</DEMO37>");
            xml.AppendLine($"<DEMO38>{item.DEMO38}</DEMO38>");
        }

        private void SetupFakes()
        {
            var connection = new ShimSqlConnection().Instance;
            ShimDataFunctions.GetClientSqlConnectionClientConnections = _ => connection;
            ShimDataFunctions.ExecuteScalarSqlCommand = cmd =>
            {
                _sqlCommand = cmd;
                return Rows;
            };

            KMFakes.ShimDataFunctions.GetDataTableSqlCommandString = (cmd, _) =>
            {
                _sqlCommand = cmd;
                return _dataTable;
            };

            KMFakes.ShimDataFunctions.ExecuteNonQuerySqlCommand = cmd =>
            {
                _sqlCommand = cmd;
                return true;
            };

            ShimSqlCommand.AllInstances.ConnectionGet = cmd => connection;
            ShimDataFunctions.ExecuteReaderSqlCommand = cmd =>
            {
                _sqlCommand = cmd;
                return _list.GetSqlDataReader();
            };
        }
    }
}