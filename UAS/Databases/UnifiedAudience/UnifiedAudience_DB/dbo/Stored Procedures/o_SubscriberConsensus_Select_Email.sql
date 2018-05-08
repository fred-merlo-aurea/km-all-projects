create procedure o_SubscriberConsensus_Select_Email
@Email varchar(100)
as
BEGIN

	SET NOCOUNT ON

	select Sequence,FName,LName,Title,Company,Address,MailStop,City,State,Zip,Plus4,ForZip,County,Country,Phone,Fax,
		MailPermission as 'Demo31',
		FaxPermission as 'Demo32',
		PhonePermission as 'Demo33',
		OtherProductsPermission as 'Demo34',
		ThirdPartyPermission as 'Demo35',
		EmailRenewPermission as 'Demo36',
		Gender,Address3,Home_Work_Address,Mobile,Score,Latitude,Longitude,Demo7,IGrp_No,Par3C,TransactionDate,
		QDate,Email,SubscriptionID,IsActive
	from Subscriptions with(nolock)
	where Email = @Email

END
go