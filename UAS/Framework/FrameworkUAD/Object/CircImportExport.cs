using System;
using System.Linq;
using System.Runtime.Serialization;

namespace FrameworkUAD.Object
{
    [Serializable]
    [DataContract]
    public class CircImportExport
    {
        public CircImportExport() { }
        #region Properties   
        [DataMember]
        public int SubscriberID { get; set; }
        [DataMember]
        public int PublicationID { get; set; }
        [DataMember]
        public int SubscriptionID { get; set; }
        [DataMember]
        public int Batch { get; set; }
        [DataMember]
        public string Hisbatch { get; set; }
        [DataMember]
        public string Hisbatch1 { get; set; }
        [DataMember]
        public string Hisbatch2 { get; set; }
        [DataMember]
        public string Hisbatch3 { get; set; }
        [DataMember]
        public string Pubcode { get; set; }
        [DataMember]
        public int SequenceID { get; set; }
        [DataMember]
        public int Cat { get; set; }
        [DataMember]
        public int Xact { get; set; }
        [DataMember]
        public DateTime XactDate { get; set; }
        [DataMember]
        public string Fname { get; set; }
        [DataMember]
        public string Lname { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public string Company { get; set; }
        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public string Mailstop { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public string State { get; set; }
        [DataMember]
        public string ZipCode { get; set; }
        [DataMember]
        public string Plus4 { get; set; }
        [DataMember]
        public string County { get; set; }
        [DataMember]
        public string Country { get; set; }
        [DataMember]
        public int CountryID { get; set; }
        [DataMember]
        public string Phone { get; set; }
        [DataMember]
        public string Fax { get; set; }
        [DataMember]
        public string Mobile { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string Website { get; set; }
        [DataMember]
        public string AcctNum { get; set; }
        [DataMember]
        public int ORIGSSRC { get; set; }
        [DataMember]
        public int SUBSRC { get; set; }
        [DataMember]
        public int Copies { get; set; }
        [DataMember]
        public int NANQ { get; set; }
        [DataMember]
        public int Qsource { get; set; }
        [DataMember]
        public DateTime Qdate { get; set; }
        [DataMember]
        public DateTime Cdate { get; set; }
        [DataMember]
        public int Par3C { get; set; }
        [DataMember]
        public int EmailID { get; set; }
        [DataMember]
        public string Verify { get; set; }
        [DataMember]
        public string Interview { get; set; }
        [DataMember]
        public string Mail { get; set; }
        [DataMember]
        public DateTime Old_Date { get; set; }
        [DataMember]
        public string Old_QSRC { get; set; }
        [DataMember]
        public int MBR_ID { get; set; }
        [DataMember]
        public string MBR_Flag { get; set; }
        [DataMember]
        public string MBR_Reject { get; set; }
        [DataMember]
        public string SPECIFY { get; set; }
        [DataMember]
        public string SIC { get; set; }
        [DataMember]
        public string EMPLOY { get; set; }
        [DataMember]
        public string SALES { get; set; }
        [DataMember]
        public int IMB_SERIAL1 { get; set; }
        [DataMember]
        public int IMB_SERIAL2 { get; set; }
        [DataMember]
        public int IMB_SERIAL3 { get; set; }
        [DataMember]
        public string Business { get; set; }
        [DataMember]
        public string BUSNTEXT { get; set; }
        [DataMember]
        public string Function { get; set; }
        [DataMember]
        public string FUNCTEXT { get; set; }
        [DataMember]
        public string DEMO1 { get; set; }
        [DataMember]
        public string DEMO1TEXT { get; set; }
        [DataMember]
        public string DEMO2 { get; set; }
        [DataMember]
        public string DEMO3 { get; set; }
        [DataMember]
        public string DEMO4 { get; set; }
        [DataMember]
        public string DEMO5 { get; set; }
        [DataMember]
        public string DEMO6 { get; set; }
        [DataMember]
        public string DEMO6TEXT { get; set; }
        [DataMember]
        public string DEMO7 { get; set; }
        [DataMember]
        public string DEMO8 { get; set; }
        [DataMember]
        public string DEMO9 { get; set; }
        [DataMember]
        public string DEMO10 { get; set; }
        [DataMember]
        public string DEMO10TEXT { get; set; }
        [DataMember]
        public string DEMO11 { get; set; }
        [DataMember]
        public string DEMO12 { get; set; }
        [DataMember]
        public string DEMO14 { get; set; }
        [DataMember]
        public string DEMO15 { get; set; }
        [DataMember]
        public string DEMO16 { get; set; }
        [DataMember]
        public string DEMO18 { get; set; }
        [DataMember]
        public string DEMO19 { get; set; }
        [DataMember]
        public string DEMO20 { get; set; }
        [DataMember]
        public string DEMO21 { get; set; }
        [DataMember]
        public string DEMO22 { get; set; }
        [DataMember]
        public string DEMO23 { get; set; }
        [DataMember]
        public string DEMO24 { get; set; }
        [DataMember]
        public string DEMO25 { get; set; }
        [DataMember]
        public string DEMO26 { get; set; }
        [DataMember]
        public string DEMO27 { get; set; }
        [DataMember]
        public string DEMO28 { get; set; }
        [DataMember]
        public string DEMO29 { get; set; }
        [DataMember]
        public string DEMO40 { get; set; }
        [DataMember]
        public string DEMO41 { get; set; }
        [DataMember]
        public string DEMO42 { get; set; }
        [DataMember]
        public string DEMO43 { get; set; }
        [DataMember]
        public string DEMO44 { get; set; }
        [DataMember]
        public string DEMO45 { get; set; }
        [DataMember]
        public string DEMO46 { get; set; }
        [DataMember]
        public string DEMO31 { get; set; }
        [DataMember]
        public string DEMO32 { get; set; }
        [DataMember]
        public string DEMO33 { get; set; }
        [DataMember]
        public string DEMO34 { get; set; }
        [DataMember]
        public string DEMO35 { get; set; }
        [DataMember]
        public string DEMO36 { get; set; }
        [DataMember]
        public string DEMO37 { get; set; }
        [DataMember]
        public string DEMO38 { get; set; }
        [DataMember]
        public string SECBUS { get; set; }
        [DataMember]
        public string SECFUNC { get; set; }
        [DataMember]
        public string Business1 { get; set; }
        [DataMember]
        public string Function1 { get; set; }
        [DataMember]
        public string Income1 { get; set; }
        [DataMember]
        public int Age1 { get; set; }
        [DataMember]
        public string Home_Value { get; set; }
        [DataMember]
        public string JOBT1 { get; set; }
        [DataMember]
        public string JOBT1TEXT { get; set; }
        [DataMember]
        public string JOBT2 { get; set; }
        [DataMember]
        public string JOBT3 { get; set; }
        [DataMember]
        public string TOE1 { get; set; }
        [DataMember]
        public string TOE2 { get; set; }
        [DataMember]
        public string AOI1 { get; set; }
        [DataMember]
        public string AOI2 { get; set; }
        [DataMember]
        public string AOI3 { get; set; }
        [DataMember]
        public string PROD1 { get; set; }
        [DataMember]
        public string PROD1TEXT { get; set; }
        [DataMember]
        public string BUYAUTH { get; set; }
        [DataMember]
        public string IND1 { get; set; }
        [DataMember]
        public string IND1TEXT { get; set; }
        [DataMember]
        public bool STATUS { get; set; }
        [DataMember]
        public int PRICECODE { get; set; }
        [DataMember]
        public int NUMISSUES { get; set; }
        [DataMember]
        public float CPRATE { get; set; }
        [DataMember]
        public int TERM { get; set; }
        [DataMember]
        public int ISSTOGO { get; set; }
        [DataMember]
        public int CARDTYPE { get; set; }
        [DataMember]
        public int CARDTYPECC { get; set; }
        [DataMember]
        public string CCNUM { get; set; }
        [DataMember]
        public string CCEXPIRE { get; set; }
        [DataMember]
        public string CCNAME { get; set; }
        [DataMember]
        public float AMOUNTPD { get; set; }
        [DataMember]
        public float AMOUNT { get; set; }
        [DataMember]
        public float BALDUE { get; set; }
        [DataMember]
        public float AMTEARNED { get; set; }
        [DataMember]
        public float AMTDEFER { get; set; }
        [DataMember]
        public DateTime PAYDATE { get; set; }
        [DataMember]
        public DateTime STARTISS { get; set; }
        [DataMember]
        public DateTime EXPIRE { get; set; }
        [DataMember]
        public DateTime NWEXPIRE { get; set; }
        [DataMember]
        public string DELIVERCODE { get; set; }
        #endregion
    }
}
