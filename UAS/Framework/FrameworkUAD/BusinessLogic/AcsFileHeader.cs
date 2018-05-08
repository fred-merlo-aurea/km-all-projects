using System;
using System.Transactions;
using KM.Common.Functions;

namespace FrameworkUAD.BusinessLogic
{
    public class AcsFileHeader
    {
        public Entity.AcsFileHeader Parse(string headerRecord)
        {
            Entity.AcsFileHeader x = new Entity.AcsFileHeader();

            x.RecordType = headerRecord.Substring(0, 1);
            x.FileVersion = headerRecord.Substring(1, 2);
            int customerID = 0;
            int.TryParse(headerRecord.Substring(3, 6).ToString(), out customerID);
            x.CustomerID = customerID;
            x.CreateDate = DateTimeFunctions.ParseDate(DateFormat.YYYYMMDD, headerRecord.Substring(9, 8));
            x.ShipmentNumber = Convert.ToInt64(headerRecord.Substring(17, 10));
            x.TotalAcsRecordCount = Convert.ToInt32(headerRecord.Substring(27, 9));
            x.TotalCoaCount = Convert.ToInt32(headerRecord.Substring(36, 9));
            x.TotalNixieCount = Convert.ToInt32(headerRecord.Substring(45, 9));
            x.TrdRecordCount = Convert.ToInt32(headerRecord.Substring(54, 9));
            x.TrdAcsFeeAmount = Convert.ToDecimal(headerRecord.Substring(63, 11)) / 100;
            x.TrdCoaCount = Convert.ToInt32(headerRecord.Substring(74, 9));
            x.TrdCoaAcsFeeAmount = Convert.ToDecimal(headerRecord.Substring(83, 11)) / 100;
            x.TrdNixieCount = Convert.ToInt32(headerRecord.Substring(94, 9));
            x.TrdNixieAcsFeeAmount = Convert.ToDecimal(headerRecord.Substring(103, 11)) / 100;
            x.OcdRecordCount = Convert.ToInt32(headerRecord.Substring(114, 9));
            x.OcdAcsFeeAmount = Convert.ToDecimal(headerRecord.Substring(123, 11)) / 100;
            x.OcdCoaCount = Convert.ToInt32(headerRecord.Substring(134, 9));
            x.OcdCoaAcsFeeAmount = Convert.ToDecimal(headerRecord.Substring(143, 11)) / 100;
            x.OcdNixieCount = Convert.ToInt32(headerRecord.Substring(154, 9));
            x.OcdNixieAcsFeeAmount = Convert.ToDecimal(headerRecord.Substring(163, 11)) / 100;
            x.FsRecordCount = Convert.ToInt32(headerRecord.Substring(174, 9));
            x.FsAcsFeeAmount = Convert.ToDecimal(headerRecord.Substring(183, 11)) / 100;
            x.FsCoaCount = Convert.ToInt32(headerRecord.Substring(194, 9));
            x.FsCoaAcsFeeAmount = Convert.ToDecimal(headerRecord.Substring(203, 11)) / 100;
            x.FsNixieCount = Convert.ToInt32(headerRecord.Substring(214, 9));
            x.FsNixieAcsFeeAmount = Convert.ToDecimal(headerRecord.Substring(223, 11)) / 100;
            x.ImpbRecordCount = Convert.ToInt32(headerRecord.Substring(234, 9));
            x.ImpbAcsFeeAmount = Convert.ToDecimal(headerRecord.Substring(243, 11)) / 100;
            x.ImpbCoaCount = Convert.ToInt32(headerRecord.Substring(254, 9));
            x.ImpbCoaAcsFeeAmount = Convert.ToDecimal(headerRecord.Substring(263, 11)) / 100;
            x.ImpbNixieCount = Convert.ToInt32(headerRecord.Substring(274, 9));
            x.ImpbNixieAcsFeeAmount = Convert.ToDecimal(headerRecord.Substring(283, 11)) / 100;
            x.Filler = headerRecord.Substring(294, 405);
            x.EndMarker = headerRecord.Substring(699, 1);

            return x;
        }
        public int Save(Entity.AcsFileHeader header, KMPlatform.Object.ClientConnections client)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                header.AcsFileHeaderId = DataAccess.AcsFileHeader.Save(header, client);
                scope.Complete();
            }

            return header.AcsFileHeaderId;
        }
    }
}
