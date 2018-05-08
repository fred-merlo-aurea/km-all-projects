CREATE PROCEDURE [dbo].[o_Subscriber_Select_Email]
@Email varchar(100)
AS
BEGIN

	SET NOCOUNT ON

	SELECT distinct s.SubscriptionID,s.Sequence,s.FName,s.LName,s.Title,s.Company,s.Address,s.MailStop,s.City,State,s.Zip,s.Plus4,ForZip,s.County,s.Country,s.CountryID,s.Phone,PhoneExists,s.Fax,FaxExists,s.Email,EmailExists,
        s.CategoryID,s.TransactionID,s.TransactionDate,ps.QualificationDate,ps.PubQSourceID,s.RegCode,s.Verified,ps.SubSrcID,ps.OrigsSrc,ps.Par3CID,
		s.MailPermission as 'Demo31',
		s.FaxPermission as 'Demo32',
		s.PhonePermission as 'Demo33',
		s.OtherProductsPermission as 'Demo34',
		s.ThirdPartyPermission as 'Demo35',
		s.EmailRenewPermission as 'Demo36',
		Source,Priority,s.IGrp_No,
        IGrp_Cnt,CGrp_No,CGrp_Cnt,StatList,Sic,SicCode,s.Gender,IGrp_Rank,CGrp_Rank,s.Address3,Home_Work_Address,PubIDs,s.Demo7,IsExcluded,s.Mobile,s.Latitude,s.Longitude,IsLatLonValid,
        LatLonMsg,Score
     FROM Subscriptions s With(NoLock) 
		join PubSubscriptions ps on s.SubscriptionID = ps.SubscriptionID
	 WHERE ps.Email = @Email

END
GO