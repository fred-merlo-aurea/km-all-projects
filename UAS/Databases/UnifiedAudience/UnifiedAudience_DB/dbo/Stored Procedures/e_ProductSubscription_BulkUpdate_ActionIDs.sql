CREATE Proc [dbo].[e_ProductSubscription_BulkUpdate_ActionIDs]
(  
	@SubscriptionXML TEXT
) AS
BEGIN

	set nocount on

	CREATE TABLE #Subs
	(  
		RowID int IDENTITY(1, 1),
		[SubscriptionID] int,
		[PubCategoryID] int,
		[PubTransactionID] int
	)

	DECLARE @docHandle int
	DECLARE @IAFree int = (SELECT SubscriptionStatusID FROM UAD_Lookup..SubscriptionStatus WHERE StatusCode = 'IAFree')
	DECLARE @IAPAid int = (SELECT SubscriptionStatusID FROM UAD_Lookup..SubscriptionStatus WHERE StatusCode = 'IAPaid')
	DECLARE @AFree int = (SELECT SubscriptionStatusID FROM UAD_Lookup..SubscriptionStatus WHERE StatusCode = 'AFree')
	DECLARE @APAid int = (SELECT SubscriptionStatusID FROM UAD_Lookup..SubscriptionStatus WHERE StatusCode = 'APaid')
	--DECLARE @SubscriptionXML varchar(8000) --FOR TESTING PURPOSES
	--set @SubscriptionXML = '<XML><Subscription><SubscriptionID>372405</SubscriptionID><PubCategoryID>10</PubCategoryID><PubTransactionID>38</PubTransactionID></Subscription><Subscription><SubscriptionID>372860</SubscriptionID><PubCategoryID>10</PubCategoryID><PubTransactionID>38</PubTransactionID></Subscription></XML>'

	EXEC sp_xml_preparedocument @docHandle OUTPUT, @SubscriptionXML  
	INSERT INTO #Subs 
	(
		 SubscriptionID, PubCategoryID, PubTransactionID
	)  
	SELECT SubscriptionID, PubCategoryID,PubTransactionID
	FROM OPENXML(@docHandle,N'/XML/Subscription')
	WITH
	(
		[SubscriptionID] int 'SubscriptionID',
		[PubCategoryID] int 'PubCategoryID',
		[PubTransactionID] int 'PubTransactionID'
	)

	UPDATE PubSubscriptions
	SET PubCategoryID = (CASE WHEN s2.PubCategoryID = 1 THEN 101 ELSE s2.PubCategoryID END), PubTransactionID = (CASE WHEN s2.PubTransactionID = 9 THEN 109 WHEN s2.PubTransactionID = 1 THEN 101 ELSE s2.PubTransactionID END), DateUpdated = GETDATE(),
	IsSubscribed = (CASE WHEN s2.PubTransactionID in (1,101) OR s2.PubTransactionID in (11,111) THEN 1 ELSE 0 END),
	SubscriptionStatusID = (CASE WHEN s2.PubTransactionID in (1, 101) THEN @AFree
								 WHEN s2.PubTransactionID in (11,111) THEN @APAid
								 WHEN s2.PubTransactionID = 9 AND s.IsPaid = 1 THEN @IAPAid
								 WHEN s2.PubTransactionID = 9 AND s.IsPaid = 0 THEN @IAFree END)
	FROM PubSubscriptions s
	INNER JOIN #Subs s2
	ON s.PubSubscriptionID = s2.SubscriptionID

	INSERT INTO HistorySubscription (PubSubscriptionID,	SubscriptionID,	PubID,	Demo7,	QualificationDate,	PubQSourceID,	PubCategoryID,	PubTransactionID,	EmailStatusID,	StatusUpdatedDate,
		StatusUpdatedReason, Email,	DateCreated, DateUpdated, CreatedByUserID, UpdatedByUserID,	IsComp,	SubscriptionStatusID,	AccountNumber,	AddRemoveID, Copies,	
		GraceIssues, IMBSEQ, IsActive, IsPaid,	IsSubscribed, MemberGroup, OnBehalfOf,	OrigsSrc, Par3CID, SequenceID, [Status], SubscriberSourceCode, SubSrcID, Verify,
		ExternalKeyID,	FirstName,	LastName, Company, Title, Occupation, AddressTypeID, Address1, Address2, Address3, City, RegionCode, RegionID, ZipCode,	Plus4,
		CarrierRoute, County, Country, CountryID, Latitude,	Longitude,	IsAddressValidated,	AddressValidationDate,	AddressValidationSource, AddressValidationMessage, Phone,
		Fax, Mobile, Website, Birthdate, Age, Income, Gender, tmpSubscriptionID, IsLocked,	LockedByUserID,	LockDate,	LockDateRelease, PhoneExt,
		IsInActiveWaveMailing,	AddressTypeCodeId,	AddressLastUpdatedDate,	AddressUpdatedSourceTypeCodeId,	WaveMailingID,	IGrp_No, SFRecordIdentifier, SubGenSubscriberID,
		MailPermission, FaxPermission, PhonePermission, OtherProductsPermission, EmailRenewPermission, ThirdPartyPermission, TextPermission)
	SELECT ps.PubSubscriptionID,ps.SubscriptionID,	PubID,	Demo7,	QualificationDate,	PubQSourceID, ps.PubCategoryID, ps.PubTransactionID,	EmailStatusID,	StatusUpdatedDate,
		StatusUpdatedReason, Email,	DateCreated, DateUpdated, CreatedByUserID, UpdatedByUserID,	IsComp,	SubscriptionStatusID,	AccountNumber,	AddRemoveID, Copies,	
		GraceIssues, IMBSEQ, IsActive, IsPaid,	IsSubscribed, MemberGroup, OnBehalfOf,	OrigsSrc, Par3CID, SequenceID, [Status], SubscriberSourceCode, SubSrcID, Verify,
		ExternalKeyID,	FirstName,	LastName, Company, Title, Occupation, AddressTypeID, Address1, Address2, Address3, City, RegionCode, RegionID, ZipCode,	Plus4,
		CarrierRoute, County, Country, CountryID, Latitude,	Longitude,	IsAddressValidated,	AddressValidationDate,	AddressValidationSource, AddressValidationMessage, Phone,
		Fax, Mobile, Website, Birthdate, Age, Income, Gender, tmpSubscriptionID, IsLocked,	LockedByUserID,	LockDate,	LockDateRelease, PhoneExt,
		IsInActiveWaveMailing,	AddressTypeCodeId,	AddressLastUpdatedDate,	AddressUpdatedSourceTypeCodeId,	WaveMailingID,	IGrp_No, SFRecordIdentifier, SubGenSubscriberID,
		MailPermission, FaxPermission, PhonePermission, OtherProductsPermission, EmailRenewPermission, ThirdPartyPermission, TextPermission
		 FROM PubSubscriptions ps
	INNER JOIN #Subs s ON ps.PubSubscriptionID = s.SubscriptionID

	DROP TABLE #Subs

END