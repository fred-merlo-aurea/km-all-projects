using KM.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace FrameworkUAD.DataAccess
{
    [Serializable]
    public class CircImportExport
    {
        public static List<Object.CircImportExport> Select(KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "o_ExportData_Select";
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return GetList(cmd);
        }
        public static List<Object.CircImportExport> Select(int PublisherID, int PublicationID, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "o_ExportData_Select_Publisher_Publication";
            cmd.Parameters.Add(new SqlParameter("@PublisherID", PublisherID));
            cmd.Parameters.Add(new SqlParameter("@PublicationID", PublicationID));
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);

            return GetList(cmd);
        }
        public static DataTable SelectDataTable(int publisherID, int publicationID, KMPlatform.Object.ClientConnections client)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "o_ExportData_Select_Publisher_Publication";
            cmd.Parameters.Add(new SqlParameter("@PublisherID", publisherID));
            cmd.Parameters.Add(new SqlParameter("@PublicationID", publicationID));
            cmd.Connection = DataFunctions.GetClientSqlConnection(client);
            return KM.Common.DataFunctions.GetDataTable(cmd, DataFunctions.ConnectionString.UAD_Master.ToString());
        }
        private static Object.CircImportExport Get(SqlCommand cmd)
        {
            Object.CircImportExport retItem = null;
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        retItem = new Object.CircImportExport();
                        DynamicBuilder<Object.CircImportExport> builder = DynamicBuilder<Object.CircImportExport>.CreateBuilder(rdr);
                        while (rdr.Read())
                        {
                            retItem = builder.Build(rdr);
                        }
                        rdr.Close();
                        rdr.Dispose();
                    }
                }
            }
            catch { }
            finally
            {
                cmd.Connection.Close();
                cmd.Dispose();
            }
            return retItem;
        }
        private static List<Object.CircImportExport> GetList(SqlCommand cmd)
        {
            List<Object.CircImportExport> retList = new List<Object.CircImportExport>();
            try
            {
                using (SqlDataReader rdr = DataFunctions.ExecuteReader(cmd))
                {
                    if (rdr != null)
                    {
                        Object.CircImportExport retItem = new Object.CircImportExport();
                        DynamicBuilder<Object.CircImportExport> builder = DynamicBuilder<Object.CircImportExport>.CreateBuilder(rdr);
                        while (rdr.Read())
                        {
                            retItem = builder.Build(rdr);
                            if (retItem != null)
                            {
                                retList.Add(retItem);
                            }
                        }
                        rdr.Close();
                        rdr.Dispose();
                    }
                }
            }
            catch { }
            finally
            {
                cmd.Connection.Close();
                cmd.Dispose();
            }
            return retList;
        }
        public static bool CircUpdateBulkSql(int UserID, List<Object.CircImportExport> list, KMPlatform.Object.ClientConnections client)
        {
            int userID = UserID;
            bool done = true;
            try
            {
                int i = 1;
                StringBuilder subscriberXML = new StringBuilder();
                subscriberXML.AppendLine("<XML>");
                foreach (Object.CircImportExport x in list)
                {
                    #region XML
                    subscriberXML.AppendLine("<Subscriber>");
                    subscriberXML.AppendLine("<UniqueID>" + i.ToString() + "</UniqueID>");
                    subscriberXML.AppendLine("<SubscriberID>" + x.SubscriberID.ToString() + "</SubscriberID>");
                    subscriberXML.AppendLine("<PublicationID>" + x.PublicationID.ToString() + "</PublicationID>");
                    subscriberXML.AppendLine("<SubscriptionID>" + x.SubscriptionID.ToString() + "</SubscriptionID>");
                    subscriberXML.AppendLine("<Batch>" + x.Batch.ToString() + "</Batch>");
                    subscriberXML.AppendLine("<Hisbatch>" + x.Hisbatch.ToString() + "</Hisbatch>");
                    subscriberXML.AppendLine("<Hisbatch1>" + x.Hisbatch1.ToString() + "</Hisbatch1>");
                    subscriberXML.AppendLine("<Hisbatch2>" + x.Hisbatch2.ToString() + "</Hisbatch2>");
                    subscriberXML.AppendLine("<Hisbatch3>" + x.Hisbatch3.ToString() + "</Hisbatch3>");
                    subscriberXML.AppendLine("<Pubcode>" + x.Pubcode.ToString() + "</Pubcode>");
                    subscriberXML.AppendLine("<SequenceID>" + x.SequenceID.ToString() + "</SequenceID>");
                    subscriberXML.AppendLine("<Cat>" + x.Cat.ToString() + "</Cat>");
                    subscriberXML.AppendLine("<Xact>" + x.Xact.ToString() + "</Xact>");
                    subscriberXML.AppendLine("<XactDate>" + x.XactDate.ToString() + "</XactDate>");
                    subscriberXML.AppendLine("<Fname>" + x.Fname.ToString() + "</Fname>");
                    subscriberXML.AppendLine("<Lname>" + x.Lname.ToString() + "</Lname>");
                    subscriberXML.AppendLine("<Title>" + x.Title.ToString() + "</Title>");
                    subscriberXML.AppendLine("<Company>" + x.Company.ToString() + "</Company>");
                    subscriberXML.AppendLine("<Address>" + x.Address.ToString() + "</Address>");
                    subscriberXML.AppendLine("<Mailstop>" + x.Mailstop.ToString() + "</Mailstop>");
                    subscriberXML.AppendLine("<City>" + x.City.ToString() + "</City>");
                    subscriberXML.AppendLine("<State>" + x.State.ToString() + "</State>");
                    subscriberXML.AppendLine("<ZipCode>" + x.ZipCode.ToString() + "</ZipCode>");
                    subscriberXML.AppendLine("<Plus4>" + x.Plus4.ToString() + "</Plus4>");
                    subscriberXML.AppendLine("<County>" + x.County.ToString() + "</County>");
                    subscriberXML.AppendLine("<Country>" + x.Country.ToString() + "</Country>");
                    subscriberXML.AppendLine("<CountryID>" + x.CountryID.ToString() + "</CountryID>");
                    subscriberXML.AppendLine("<Phone>" + x.Phone.ToString() + "</Phone>");
                    subscriberXML.AppendLine("<Fax>" + x.Fax.ToString() + "</Fax>");
                    subscriberXML.AppendLine("<Mobile>" + x.Mobile.ToString() + "</Mobile>");
                    subscriberXML.AppendLine("<Email>" + x.Email.ToString() + "</Email>");
                    subscriberXML.AppendLine("<Website>" + x.Website.ToString() + "</Website>");
                    subscriberXML.AppendLine("<AcctNum>" + x.AcctNum.ToString() + "</AcctNum>");
                    subscriberXML.AppendLine("<ORIGSSRC>" + x.ORIGSSRC.ToString() + "</ORIGSSRC>");
                    subscriberXML.AppendLine("<SUBSRC>" + x.SUBSRC.ToString() + "</SUBSRC>");
                    subscriberXML.AppendLine("<Copies>" + x.Copies.ToString() + "</Copies>");
                    subscriberXML.AppendLine("<NANQ>" + x.NANQ.ToString() + "</NANQ>");
                    subscriberXML.AppendLine("<Qsource>" + x.Qsource.ToString() + "</Qsource>");
                    subscriberXML.AppendLine("<Qdate>" + x.Qdate.ToString() + "</Qdate>");
                    subscriberXML.AppendLine("<Cdate>" + x.Cdate.ToString() + "</Cdate>");
                    subscriberXML.AppendLine("<Par3C>" + x.Par3C.ToString() + "</Par3C>");
                    subscriberXML.AppendLine("<EmailID>" + x.EmailID.ToString() + "</EmailID>");
                    subscriberXML.AppendLine("<Verify>" + x.Verify.ToString() + "</Verify>");
                    subscriberXML.AppendLine("<Interview>" + x.Interview.ToString() + "</Interview>");
                    subscriberXML.AppendLine("<Mail>" + x.Mail.ToString() + "</Mail>");
                    subscriberXML.AppendLine("<Old_Date>" + x.Old_Date.ToString() + "</Old_Date>");
                    subscriberXML.AppendLine("<Old_QSRC>" + x.Old_QSRC.ToString() + "</Old_QSRC>");
                    subscriberXML.AppendLine("<MBR_ID>" + x.MBR_ID.ToString() + "</MBR_ID>");
                    subscriberXML.AppendLine("<MBR_Flag>" + x.MBR_Flag.ToString() + "</MBR_Flag>");
                    subscriberXML.AppendLine("<MBR_Reject>" + x.MBR_Reject.ToString() + "</MBR_Reject>");
                    subscriberXML.AppendLine("<SPECIFY>" + x.SPECIFY.ToString() + "</SPECIFY    >");
                    subscriberXML.AppendLine("<SIC>" + x.SIC.ToString() + "</SIC>");
                    subscriberXML.AppendLine("<EMPLOY>" + x.EMPLOY.ToString() + "</EMPLOY>");
                    subscriberXML.AppendLine("<SALES>" + x.SALES.ToString() + "</SALES>");
                    subscriberXML.AppendLine("<IMB_SERIAL1>" + x.IMB_SERIAL1.ToString() + "</IMB_SERIAL1>");
                    subscriberXML.AppendLine("<IMB_SERIAL2>" + x.IMB_SERIAL2.ToString() + "</IMB_SERIAL2>");
                    subscriberXML.AppendLine("<IMB_SERIAL3>" + x.IMB_SERIAL3.ToString() + "</IMB_SERIAL3>");
                    subscriberXML.AppendLine("<Business>" + x.Business.ToString() + "</Business>");
                    subscriberXML.AppendLine("<BUSNTEXT>" + x.BUSNTEXT.ToString() + "</BUSNTEXT>");
                    subscriberXML.AppendLine("<Function>" + x.Function.ToString() + "</Function>");
                    subscriberXML.AppendLine("<FUNCTEXT>" + x.FUNCTEXT.ToString() + "</FUNCTEXT>");
                    subscriberXML.AppendLine("<DEMO1>" + x.DEMO1.ToString() + "</DEMO1>");
                    subscriberXML.AppendLine("<DEMO1TEXT>" + x.DEMO1TEXT.ToString() + "</DEMO1TEXT>");
                    subscriberXML.AppendLine("<DEMO2>" + x.DEMO2.ToString() + "</DEMO2>");
                    subscriberXML.AppendLine("<DEMO3>" + x.DEMO3.ToString() + "</DEMO3>");
                    subscriberXML.AppendLine("<DEMO4>" + x.DEMO4.ToString() + "</DEMO4>");
                    subscriberXML.AppendLine("<DEMO5>" + x.DEMO5.ToString() + "</DEMO5>");
                    subscriberXML.AppendLine("<DEMO6>" + x.DEMO6.ToString() + "</DEMO6>");
                    subscriberXML.AppendLine("<DEMO6TEXT>" + x.DEMO6TEXT.ToString() + "</DEMO6TEXT>");
                    subscriberXML.AppendLine("<DEMO7>" + x.DEMO7.ToString() + "</DEMO7>");
                    subscriberXML.AppendLine("<DEMO8>" + x.DEMO8.ToString() + "</DEMO8>");
                    subscriberXML.AppendLine("<DEMO9>" + x.DEMO9.ToString() + "</DEMO9>");
                    subscriberXML.AppendLine("<DEMO10>" + x.DEMO10.ToString() + "</DEMO10>");
                    subscriberXML.AppendLine("<DEMO10TEXT>" + x.DEMO10TEXT.ToString() + "</DEMO10TEXT>");
                    subscriberXML.AppendLine("<DEMO11>" + x.DEMO11.ToString() + "</DEMO11>");
                    subscriberXML.AppendLine("<DEMO12>" + x.DEMO12.ToString() + "</DEMO12>");
                    subscriberXML.AppendLine("<DEMO14>" + x.DEMO14.ToString() + "</DEMO14>");
                    subscriberXML.AppendLine("<DEMO15>" + x.DEMO15.ToString() + "</DEMO15>");
                    subscriberXML.AppendLine("<DEMO16>" + x.DEMO16.ToString() + "</DEMO16>");
                    subscriberXML.AppendLine("<DEMO18>" + x.DEMO18.ToString() + "</DEMO18>");
                    subscriberXML.AppendLine("<DEMO19>" + x.DEMO19.ToString() + "</DEMO19>");
                    subscriberXML.AppendLine("<DEMO20>" + x.DEMO20.ToString() + "</DEMO20>");
                    subscriberXML.AppendLine("<DEMO21>" + x.DEMO21.ToString() + "</DEMO21>");
                    subscriberXML.AppendLine("<DEMO22>" + x.DEMO22.ToString() + "</DEMO22>");
                    subscriberXML.AppendLine("<DEMO23>" + x.DEMO23.ToString() + "</DEMO23>");
                    subscriberXML.AppendLine("<DEMO24>" + x.DEMO24.ToString() + "</DEMO24>");
                    subscriberXML.AppendLine("<DEMO25>" + x.DEMO25.ToString() + "</DEMO25>");
                    subscriberXML.AppendLine("<DEMO26>" + x.DEMO26.ToString() + "</DEMO26>");
                    subscriberXML.AppendLine("<DEMO27>" + x.DEMO27.ToString() + "</DEMO27>");
                    subscriberXML.AppendLine("<DEMO28>" + x.DEMO28.ToString() + "</DEMO28>");
                    subscriberXML.AppendLine("<DEMO29>" + x.DEMO29.ToString() + "</DEMO29>");
                    subscriberXML.AppendLine("<DEMO40>" + x.DEMO40.ToString() + "</DEMO40>");
                    subscriberXML.AppendLine("<DEMO41>" + x.DEMO41.ToString() + "</DEMO41>");
                    subscriberXML.AppendLine("<DEMO42>" + x.DEMO42.ToString() + "</DEMO42>");
                    subscriberXML.AppendLine("<DEMO43>" + x.DEMO43.ToString() + "</DEMO43>");
                    subscriberXML.AppendLine("<DEMO44>" + x.DEMO44.ToString() + "</DEMO44>");
                    subscriberXML.AppendLine("<DEMO45>" + x.DEMO45.ToString() + "</DEMO45>");
                    subscriberXML.AppendLine("<DEMO46>" + x.DEMO46.ToString() + "</DEMO46>");
                    subscriberXML.AppendLine("<DEMO31>" + x.DEMO31.ToString() + "</DEMO31>");
                    subscriberXML.AppendLine("<DEMO32>" + x.DEMO32.ToString() + "</DEMO32>");
                    subscriberXML.AppendLine("<DEMO33>" + x.DEMO33.ToString() + "</DEMO33>");
                    subscriberXML.AppendLine("<DEMO34>" + x.DEMO34.ToString() + "</DEMO34>");
                    subscriberXML.AppendLine("<DEMO35>" + x.DEMO35.ToString() + "</DEMO35>");
                    subscriberXML.AppendLine("<DEMO36>" + x.DEMO36.ToString() + "</DEMO36>");
                    subscriberXML.AppendLine("<DEMO37>" + x.DEMO37.ToString() + "</DEMO37>");
                    subscriberXML.AppendLine("<DEMO38>" + x.DEMO38.ToString() + "</DEMO38>");
                    subscriberXML.AppendLine("<SECBUS>" + x.SECBUS.ToString() + "</SECBUS>");
                    subscriberXML.AppendLine("<SECFUNC>" + x.SECFUNC.ToString() + "</SECFUNC>");
                    subscriberXML.AppendLine("<Business1>" + x.Business1.ToString() + "</Business1>");
                    subscriberXML.AppendLine("<Function1>" + x.Function1.ToString() + "</Function1>");
                    subscriberXML.AppendLine("<Income1>" + x.Income1.ToString() + "</Income1>");
                    subscriberXML.AppendLine("<Age1>" + x.Age1.ToString() + "</Age1>");
                    subscriberXML.AppendLine("<Home_Value>" + x.Home_Value.ToString() + "</Home_Value>");
                    subscriberXML.AppendLine("<JOBT1>" + x.JOBT1.ToString() + "</JOBT1>");
                    subscriberXML.AppendLine("<JOBT1TEXT>" + x.JOBT1TEXT.ToString() + "</JOBT1TEXT>");
                    subscriberXML.AppendLine("<JOBT2>" + x.JOBT2.ToString() + "</JOBT2>");
                    subscriberXML.AppendLine("<JOBT3>" + x.JOBT3.ToString() + "</JOBT3>");
                    subscriberXML.AppendLine("<TOE1>" + x.TOE1.ToString() + "</TOE1>");
                    subscriberXML.AppendLine("<TOE2>" + x.TOE2.ToString() + "</TOE2>");
                    subscriberXML.AppendLine("<AOI1>" + x.AOI1.ToString() + "</AOI1>");
                    subscriberXML.AppendLine("<AOI2>" + x.AOI2.ToString() + "</AOI2>");
                    subscriberXML.AppendLine("<AOI3>" + x.AOI3.ToString() + "</AOI3>");
                    subscriberXML.AppendLine("<PROD1>" + x.PROD1.ToString() + "</PROD1>");
                    subscriberXML.AppendLine("<PROD1TEXT>" + x.PROD1TEXT.ToString() + "</PROD1TEXT>");
                    subscriberXML.AppendLine("<BUYAUTH>" + x.BUYAUTH.ToString() + "</BUYAUTH>");
                    subscriberXML.AppendLine("<IND1>" + x.IND1.ToString() + "</IND1>");
                    subscriberXML.AppendLine("<IND1TEXT>" + x.IND1TEXT.ToString() + "</IND1TEXT>");
                    subscriberXML.AppendLine("<STATUS>" + x.STATUS.ToString() + "</STATUS>");
                    subscriberXML.AppendLine("<PRICECODE>" + x.PRICECODE.ToString() + "</PRICECODE>");
                    subscriberXML.AppendLine("<NUMISSUES>" + x.NUMISSUES.ToString() + "</NUMISSUES>");
                    subscriberXML.AppendLine("<CPRATE>" + x.CPRATE.ToString() + "</CPRATE>");
                    subscriberXML.AppendLine("<TERM>" + x.TERM.ToString() + "</TERM>");
                    subscriberXML.AppendLine("<ISSTOGO>" + x.ISSTOGO.ToString() + "</ISSTOGO>");
                    subscriberXML.AppendLine("<CARDTYPE>" + x.CARDTYPE.ToString() + "</CARDTYPE>");
                    subscriberXML.AppendLine("<CARDTYPECC>" + x.CARDTYPECC.ToString() + "</CARDTYPECC>");
                    subscriberXML.AppendLine("<CCNUM>" + x.CCNUM.ToString() + "</CCNUM>");
                    subscriberXML.AppendLine("<CCEXPIRE>" + x.CCEXPIRE.ToString() + "</CCEXPIRE>");
                    subscriberXML.AppendLine("<CCNAME>" + x.CCNAME.ToString() + "</CCNAME>");
                    subscriberXML.AppendLine("<AMOUNTPD>" + x.AMOUNTPD.ToString() + "</AMOUNTPD>");
                    subscriberXML.AppendLine("<AMOUNT>" + x.AMOUNT.ToString() + "</AMOUNT>");
                    subscriberXML.AppendLine("<BALDUE>" + x.BALDUE.ToString() + "</BALDUE>");
                    subscriberXML.AppendLine("<AMTEARNED>" + x.AMTEARNED.ToString() + "</AMTEARNED>");
                    subscriberXML.AppendLine("<AMTDEFER>" + x.AMTDEFER.ToString() + "</AMTDEFER>");
                    subscriberXML.AppendLine("<PAYDATE>" + x.PAYDATE.ToString() + "</PAYDATE>");
                    subscriberXML.AppendLine("<STARTISS>" + x.STARTISS.ToString() + "</STARTISS>");
                    subscriberXML.AppendLine("<EXPIRE>" + x.EXPIRE.ToString() + "</EXPIRE>");
                    subscriberXML.AppendLine("<NWEXPIRE>" + x.NWEXPIRE.ToString() + "</NWEXPIRE>");
                    subscriberXML.AppendLine("<DELIVERCODE>" + x.DELIVERCODE.ToString() + "</DELIVERCODE>");
                    subscriberXML.AppendLine("</Subscriber>");
                    #endregion
                    i++;
                }
                subscriberXML.AppendLine("</XML>");

                #region SQL Commands
                SqlCommand cmds = new SqlCommand();
                cmds.CommandType = CommandType.StoredProcedure;
                cmds.CommandText = "o_ExportData_RunImportSave";
                cmds.Parameters.Add(new SqlParameter("@UserID", userID));
                cmds.Parameters.Add(new SqlParameter("@xml", subscriberXML.ToString()));
                cmds.Connection = DataFunctions.GetClientSqlConnection(client);

                done = KM.Common.DataFunctions.ExecuteNonQuery(cmds);
                #endregion
            }
            catch (Exception)
            {
                done = false;
                //string message = Core_AMS.Utilities.StringFunctions.FormatException(ex);
                //FrameworkUAS.BusinessLogic.FileLog fl = new BusinessLogic.FileLog();
                //fl.Save(new FrameworkUAS.Entity.FileLog(-99, -99, message));
            }
            return done;
        }
    }
}
