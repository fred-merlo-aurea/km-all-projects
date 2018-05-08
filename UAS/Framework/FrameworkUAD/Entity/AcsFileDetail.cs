using System;
using System.Runtime.Serialization;

namespace FrameworkUAD.Entity
{
    [Serializable]
    [DataContract]
    public class AcsFileDetail
    {
        public AcsFileDetail()
        {
            AcsFileDetailId = 0;
            RecordType = string.Empty;
            FileVersion = string.Empty;
            SequenceNumber = 0;
            AcsMailerId = string.Empty;
            KeylineSequenceSerialNumber = string.Empty;
            MoveEffectiveDate = DateTime.Now;
            MoveType = string.Empty;
            DeliverabilityCode = string.Empty;
            UspsSiteID = 0;
            LastName = string.Empty;
            FirstName = string.Empty;
            Prefix = string.Empty;
            Suffix = string.Empty;
            OldAddressType = string.Empty;
            OldUrb = string.Empty;
            OldPrimaryNumber = string.Empty;
            OldPreDirectional = string.Empty;
            OldStreetName = string.Empty;
            OldSuffix = string.Empty;
            OldPostDirectional = string.Empty;
            OldUnitDesignator = string.Empty;
            OldSecondaryNumber = string.Empty;
            OldCity = string.Empty;
            OldStateAbbreviation = string.Empty;
            OldZipCode = string.Empty;
            NewAddressType = string.Empty;
            NewPmb = string.Empty;
            NewUrb = string.Empty;
            NewPrimaryNumber = string.Empty;
            NewPreDirectional = string.Empty;
            NewStreetName = string.Empty;
            NewSuffix = string.Empty;
            NewPostDirectional = string.Empty;
            NewUnitDesignator = string.Empty;
            NewSecondaryNumber = string.Empty;
            NewCity = string.Empty;
            NewStateAbbreviation = string.Empty;
            NewZipCode = string.Empty;
            Hyphen = string.Empty;
            NewPlus4Code = string.Empty;
            NewDeliveryPoint = string.Empty;
            NewAbbreviatedCityName = string.Empty;
            NewAddressLabel = string.Empty;
            FeeNotification = string.Empty;
            NotificationType = string.Empty;
            IntelligentMailBarcode = string.Empty;
            IntelligentMailPackageBarcode = string.Empty;
            IdTag = string.Empty;
            HardcopyToElectronicFlag = string.Empty;
            TypeOfAcs = string.Empty;
            FulfillmentDate = DateTime.Now;
            ProcessingType = string.Empty;
            CaptureType = string.Empty;
            MadeAvailableDate = DateTime.Now;
            ShapeOfMail = string.Empty;
            MailActionCode = string.Empty;
            NixieFlag = string.Empty;
            ProductCode1 = 0;
            ProductCodeFee1 = 0;
            ProductCode2 = 0;
            ProductCodeFee2 = 0;
            ProductCode3 = 0;
            ProductCodeFee3 = 0;
            ProductCode4 = 0;
            ProductCodeFee4 = 0;
            ProductCode5 = 0;
            ProductCodeFee5 = 0;
            ProductCode6 = 0;
            ProductCodeFee6 = 0;
            Filler = string.Empty;
            EndMarker = string.Empty;
            ProductCode = string.Empty;
            OldAddress1 = string.Empty;
            OldAddress2 = string.Empty;
            OldAddress3 = string.Empty;
            NewAddress1 = string.Empty;
            NewAddress2 = string.Empty;
            NewAddress3 = string.Empty;
            SequenceID = 0;
            TransactionCodeValue = 0;
            CategoryCodeValue = 0;
            IsIgnored = false;
            AcsActionId = 0;
            CreatedDate = DateTime.Now;
            CreatedTime = DateTime.Now.TimeOfDay;
            ProcessCode = Core_AMS.Utilities.StringFunctions.GenerateProcessCode();
        }
        #region Properties
        [DataMember]
        public int AcsFileDetailId { get; set; }
        [DataMember]
        public string RecordType { get; set; }
        [DataMember]
        public string FileVersion { get; set; }
        [DataMember]
        public int SequenceNumber { get; set; }
        [DataMember]
        public string AcsMailerId { get; set; } // Participant ID
        [DataMember]
        public string KeylineSequenceSerialNumber { get; set; }
        [DataMember]
        public DateTime MoveEffectiveDate { get; set; } //(CCYYMMDD)
        [DataMember]
        public string MoveType { get; set; } //need MoveTypeID ??
        [DataMember]
        public string DeliverabilityCode { get; set; }  //need Deliverability Code
        [DataMember]
        public int UspsSiteID { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string Prefix { get; set; }
        [DataMember]
        public string Suffix { get; set; }
        [DataMember]
        public string OldAddressType { get; set; }
        [DataMember]
        public string OldUrb { get; set; }
        [DataMember]
        public string OldPrimaryNumber { get; set; }
        [DataMember]
        public string OldPreDirectional { get; set; }
        [DataMember]
        public string OldStreetName { get; set; }
        [DataMember]
        public string OldSuffix { get; set; }
        [DataMember]
        public string OldPostDirectional { get; set; }
        [DataMember]
        public string OldUnitDesignator { get; set; }
        [DataMember]
        public string OldSecondaryNumber { get; set; }
        [DataMember]
        public string OldCity { get; set; }
        [DataMember]
        public string OldStateAbbreviation { get; set; }
        [DataMember]
        public string OldZipCode { get; set; }
        [DataMember]
        public string NewAddressType { get; set; }  //need AddressType
        [DataMember]
        public string NewPmb { get; set; }
        [DataMember]
        public string NewUrb { get; set; }
        [DataMember]
        public string NewPrimaryNumber { get; set; }
        [DataMember]
        public string NewPreDirectional { get; set; }
        [DataMember]
        public string NewStreetName { get; set; }
        [DataMember]
        public string NewSuffix { get; set; }
        [DataMember]
        public string NewPostDirectional { get; set; }
        [DataMember]
        public string NewUnitDesignator { get; set; }
        [DataMember]
        public string NewSecondaryNumber { get; set; }
        [DataMember]
        public string NewCity { get; set; }
        [DataMember]
        public string NewStateAbbreviation { get; set; }
        [DataMember]
        public string NewZipCode { get; set; }
        [DataMember]
        public string Hyphen { get; set; }
        [DataMember]
        public string NewPlus4Code { get; set; }
        [DataMember]
        public string NewDeliveryPoint { get; set; }
        [DataMember]
        public string NewAbbreviatedCityName { get; set; }
        [DataMember]
        public string NewAddressLabel { get; set; }
        [DataMember]
        public string FeeNotification { get; set; } //(E/1/2/3)
        [DataMember]
        public string NotificationType { get; set; } //(A-F)
        [DataMember]
        public string IntelligentMailBarcode { get; set; } //(IMb)
        [DataMember]
        public string IntelligentMailPackageBarcode { get; set; } //(IMpb)
        [DataMember]
        public string IdTag { get; set; } // UPU
        [DataMember]
        public string HardcopyToElectronicFlag { get; set; }
        [DataMember]
        public string TypeOfAcs { get; set; } //(T/O/F/I)
        [DataMember]
        public DateTime FulfillmentDate { get; set; }
        [DataMember]
        public string ProcessingType { get; set; } //(C/P/R/F)
        [DataMember]
        public string CaptureType { get; set; } //(I/C/F/Blank)
        [DataMember]
        public DateTime MadeAvailableDate { get; set; }
        [DataMember]
        public string ShapeOfMail { get; set; } //(L/F/P)
        [DataMember]
        public string MailActionCode { get; set; } //(F/R/D/X/U)
        [DataMember]
        public string NixieFlag { get; set; } //(C/N)
        [DataMember]
        public int ProductCode1 { get; set; }
        [DataMember]
        public decimal ProductCodeFee1 { get; set; }
        [DataMember]
        public int ProductCode2 { get; set; }
        [DataMember]
        public decimal ProductCodeFee2 { get; set; }
        [DataMember]
        public int ProductCode3 { get; set; }
        [DataMember]
        public decimal ProductCodeFee3 { get; set; }
        [DataMember]
        public int ProductCode4 { get; set; }
        [DataMember]
        public decimal ProductCodeFee4 { get; set; }
        [DataMember]
        public int ProductCode5 { get; set; }
        [DataMember]
        public decimal ProductCodeFee5 { get; set; }
        [DataMember]
        public int ProductCode6 { get; set; }
        [DataMember]
        public decimal ProductCodeFee6 { get; set; }
        [DataMember]
        public string Filler { get; set; }
        [DataMember]
        public string EndMarker { get; set; }
        [DataMember]
        public string ProductCode { get; set; }      
        [DataMember]
        public string OldAddress1 { get; set; }
        [DataMember]
        public string OldAddress2 { get; set; }        
        [DataMember]
        public string OldAddress3 { get; set; } 
        [DataMember]
        public string NewAddress1 { get; set; }
        [DataMember]
        public string NewAddress2 { get; set; }
        [DataMember]
        public string NewAddress3 { get; set; }
        [DataMember]
        public int SequenceID { get; set; }
        [DataMember]
        public int TransactionCodeValue { get; set; }
        [DataMember]
        public int CategoryCodeValue { get; set; }
        [DataMember]
        public bool IsIgnored { get; set; }
        [DataMember]
        public int AcsActionId { get; set; }
        [DataMember]
        public DateTime CreatedDate { get; set; }
        [DataMember]
        public TimeSpan CreatedTime { get; set; }
        [DataMember]
        public string ProcessCode { get; set; }
        #endregion
    }
}

//Move Type
//F - Family move (includes everyone with the same last name)
//I - Individual move (includes only the individual)
//B - Business move

//Deliverability – COA Notices
//Space or blank - New address information is present
//G - Post Office Box™ has been closed – created from a USPS-Filed COA – no new address present
//K - Customer has moved and left no forwarding address - created from a USPS-Filed COA – no new address present
//W - Temporary COA – no new address present – Temporarily Away is provided in the Parsed New Address field Code

//Deliverability Code – Nixie Notices
//Space or blank - New address information is present
//A - Attempted, not known
//E - In dispute
//I - Insufficient address
//L - Illegible
//M - No mail receptacle
//N - No such number
//P - Deceased
//Q - Not deliverable as addressed/unable to forward/forwarding order expired
//R - Refused
//S - No such street
//U - Unclaimed
//V - Vacant

//Address Type
//F - New address is Foreign
//G - Moved from or to a General Delivery
//H - Moved from or to a Highway Contract Route
//P - Moved from or to a P.O. BOX
//R - Moved from or to a Rural Route
//S - Moved from or to a Street Address
//V - Moved from or to a Highway Contract Route with a Box
//X - Moved from or to a Rural Route with Box

//Fee Notification
//E - Electronic Fee
//1 - Automated First Notice Fee
//2 - Automated Second Notice Fee
//3 - Automated Third or More Notice Fee

//Notification Type
//A - (Reserved for future use)
//B - First-Class Mail
//C - Periodicals, Initial Notice
//D - Standard Mail
//E - Package Services*
//F - Periodicals, Follow-Up Notice

//Intelligent Mail Barcode
/*Start End Length  FIELD NAME                  DATA TYPE
  441   471 31      INTELLIGENT MAIL BARCODE    NUM
  441   442 2       Barcode Identifier          NUM
  443   445 3       Service Type ID             NUM
  446   452 6       6-Digit Mailer ID           NUM 
  451   460 9       Serial Number 9 digits      NUM                
  446   455 9       9-Digit Mailer ID           NUM
  454   460 6       Serial Number 6 digits      NUM
  461   471 11      Delivery Point ZIP Code     NUM
 */

//Mail Action Code
//F - Forwarded to new address
//R - Returned to Sender
//D - Discarded
//X - Secure Destruction
//U - Unknown or disposition not available