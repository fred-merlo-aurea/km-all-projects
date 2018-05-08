create procedure o_SubscriberProduct_Select_Email_ProductCode
@Email varchar(100),
@ProductCode varchar(50) 
as
BEGIN

	SET NOCOUNT ON

	select 
			  s.IMBSEQ as 'Sequence',
			  s.FirstName as 'FName',
			  s.LastName as 'LName',
			  s.Title,
			  s.Company,
			  s.Address1 as 'Address',
			  s.Address2 as 'MailStop',
			  s.City,
			  s.RegionCode as 'State',
			  s.ZipCode as 'Zip',
			  s.Plus4,
			  '' as 'ForZip',
			  s.County,s.Country,s.Phone,s.Fax,
              s.MailPermission as 'Demo31',
              s.FaxPermission as 'Demo32',
              s.PhonePermission as 'Demo33',
              s.OtherProductsPermission as 'Demo34',
              s.ThirdPartyPermission as 'Demo35',
              s.EmailRenewPermission as 'Demo36',
              s.Gender,
			  s.Address3,
			  '' as 'Home_Work_Address',
			  s.Mobile,
			  0 as 'Score',
			  s.Latitude,
			  s.Longitude,
			  s.Demo7,
			  s.IGrp_No,
			  (select CodeValue from UAD_LOOKUP..Code with(nolock) where CodeId = s.Par3CID) as 'Par3C',
			  s.PubTransactionDate as 'TransactionDate',
			  s.Qualificationdate as 'QDate',
			  s.Email,
			  s.SubscriptionID,
			  s.IsActive
       from PubSubscriptions s with(nolock)  
       join Pubs p with(nolock) on s.PubID = p.PubID
       where s.Email = @Email and p.PubCode = @ProductCode

END
go