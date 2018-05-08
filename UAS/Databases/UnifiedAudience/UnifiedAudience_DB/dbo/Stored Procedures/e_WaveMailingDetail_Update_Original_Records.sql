CREATE PROCEDURE e_WaveMailingDetail_Update_Original_Records
	@ProductID int,
	@UserID int
AS
BEGIN
	
	SET NOCOUNT ON
	
	UPDATE ps
	SET demo7 = (CASE WHEN LEN(wmd.Demo7) > 0 THEN wmd.Demo7 ELSE ps.demo7 END),
	PubCategoryID = (CASE WHEN wmd.PubCategoryID > 0 THEN wmd.PubCategoryID ELSE ps.PubCategoryID END),
	PubTransactionID = (CASE WHEN wmd.PubTransactionID > 0 THEN wmd.PubTransactionID ELSE ps.PubTransactionID END),
	Copies = (CASE WHEN LEN(wmd.Copies) > 0 THEN wmd.Copies ELSE ps.Copies END),
	SubscriptionStatusID = (CASE WHEN wmd.SubscriptionStatusID > 0 THEN wmd.SubscriptionStatusID ELSE ps.SubscriptionStatusID END),
	IsSubscribed = (CASE WHEN LEN(wmd.IsSubscribed) > 0 THEN wmd.IsSubscribed ELSE ps.IsSubscribed END),
	FirstName = (CASE WHEN ISNULL(wmd.FirstName, 'isnull') <> 'isnull' THEN wmd.FirstName ELSE ps.FirstName END),
	LastName = (CASE WHEN ISNULL(wmd.LastName, 'isnull') <> 'isnull' THEN wmd.LastName ELSE ps.LastName END),
	Company = (CASE WHEN ISNULL(wmd.Company, 'isnull') <> 'isnull' THEN wmd.Company ELSE ps.Company END),
	Title = (CASE WHEN ISNULL(wmd.Title, 'isnull') <> 'isnull' THEN wmd.Title ELSE ps.Title END),
	AddressTypeCodeId = (CASE WHEN LEN(wmd.AddressTypeID) > 0 THEN wmd.AddressTypeID ELSE ps.AddressTypeID END),
	Address1 = (CASE WHEN ISNULL(wmd.Address1, 'isnull') <> 'isnull' THEN wmd.Address1 ELSE ps.Address1 END),
	Address2 = (CASE WHEN ISNULL(wmd.Address2, 'isnull') <> 'isnull' THEN wmd.Address2 ELSE ps.Address2 END),
	Address3 = (CASE WHEN ISNULL(wmd.Address3, 'isnull') <> 'isnull' THEN wmd.Address3 ELSE ps.Address3 END),
	City = (CASE WHEN ISNULL(wmd.City, 'isnull') <> 'isnull' THEN wmd.City ELSE ps.City END),
	RegionCode = (CASE WHEN ISNULL(wmd.RegionCode, 'isnull') <> 'isnull' THEN wmd.RegionCode ELSE ps.RegionCode END),
	RegionID = (CASE WHEN LEN(wmd.RegionID) > 0 THEN wmd.RegionID ELSE ps.RegionID END),
	ZipCode = (CASE WHEN ISNULL(wmd.ZipCode, 'isnull') <> 'isnull' THEN wmd.ZipCode ELSE ps.ZipCode END),
	Plus4 = (CASE WHEN ISNULL(wmd.Plus4, 'isnull') <> 'isnull' THEN wmd.Plus4 ELSE ps.Plus4 END),
	County = (CASE WHEN ISNULL(wmd.County, 'isnull') <> 'isnull' THEN wmd.County ELSE ps.County END),
	Country = (CASE WHEN ISNULL(wmd.Country, 'isnull') <> 'isnull' THEN wmd.Country ELSE ps.Country END),
	CountryID = (CASE WHEN LEN(wmd.CountryID) > 0 THEN wmd.CountryID ELSE ps.CountryID END),
	Email = (CASE WHEN ISNULL(wmd.Email, 'isnull') <> 'isnull' THEN wmd.Email ELSE ps.Email END),
	Phone = (CASE WHEN ISNULL(wmd.Phone, 'isnull') <> 'isnull' THEN wmd.Phone ELSE ps.Phone END),
	Mobile = (CASE WHEN ISNULL(wmd.Mobile, 'isnull') <> 'isnull' THEN wmd.Mobile ELSE ps.Mobile END),
	Fax = (CASE WHEN ISNULL(wmd.Fax, 'isnull') <> 'isnull' THEN wmd.Fax ELSE ps.Fax END),
	IsPaid = (CASE WHEN LEN(wmd.IsPaid) > 0 THEN wmd.IsPaid ELSE ps.IsPaid END),
	PhoneExt = (CASE WHEN ISNULL(wmd.PhoneExt, 'isnull') <> 'isnull' THEN wmd.PhoneExt ELSE ps.PhoneExt END),
	DateUpdated = GETDATE()
	FROM PubSubscriptions ps
		JOIN WaveMailingDetail wmd ON wmd.PubSubscriptionID = ps.PubSubscriptionID
		JOIN WaveMailing wm ON wm.WaveMailingID = wmd.WaveMailingID
		JOIN Issue i ON i.IssueId = wm.IssueID
	WHERE i.PublicationId = @ProductID AND i.IsComplete = 0	
	
	BEGIN			
	INSERT INTO HistorySubscription (PubSubscriptionID,	SubscriptionID,	PubID,	Demo7,	QualificationDate,	PubQSourceID,	PubCategoryID,	PubTransactionID,	EmailStatusID,	StatusUpdatedDate,
		StatusUpdatedReason, Email,	DateCreated, DateUpdated, CreatedByUserID, UpdatedByUserID,	IsComp,	SubscriptionStatusID,	AccountNumber,	AddRemoveID, Copies,	
		GraceIssues, IMBSEQ, IsActive, IsPaid,	IsSubscribed, MemberGroup, OnBehalfOf,	OrigsSrc, Par3CID, SequenceID, [Status], SubscriberSourceCode, SubSrcID, Verify,
		ExternalKeyID,	FirstName,	LastName, Company, Title, Occupation, AddressTypeID, Address1, Address2, Address3, City, RegionCode, RegionID, ZipCode,	Plus4,
		CarrierRoute, County, Country, CountryID, Latitude,	Longitude,	IsAddressValidated,	AddressValidationDate,	AddressValidationSource, AddressValidationMessage, Phone,
		Fax, Mobile, Website, Birthdate, Age, Income, Gender, tmpSubscriptionID, IsLocked,	LockedByUserID,	LockDate,	LockDateRelease, PhoneExt,
		IsInActiveWaveMailing,	AddressTypeCodeId,	AddressLastUpdatedDate,	AddressUpdatedSourceTypeCodeId,	WaveMailingID,	IGrp_No, SFRecordIdentifier)
	SELECT ps.PubSubscriptionID, ps.SubscriptionID,	PubID,	ps.Demo7,	QualificationDate,	PubQSourceID, ps.PubCategoryID, ps.PubTransactionID,	EmailStatusID,	StatusUpdatedDate,
		StatusUpdatedReason, ps.Email,	GETDATE(), NULL, @UserID, NULL,	IsComp,	ps.SubscriptionStatusID,	AccountNumber,	AddRemoveID, ps.Copies,	
		GraceIssues, IMBSEQ, IsActive, ps.IsPaid,	ps.IsSubscribed, MemberGroup, OnBehalfOf,	OrigsSrc, Par3CID, SequenceID, [Status], SubscriberSourceCode, SubSrcID, Verify,
		ExternalKeyID,	ps.FirstName,	ps.LastName, ps.Company, ps.Title, Occupation, ps.AddressTypeID, ps.Address1, ps.Address2, ps.Address3, ps.City, ps.RegionCode, ps.RegionID, ps.ZipCode, ps.Plus4,
		CarrierRoute, ps.County, ps.Country, ps.CountryID, Latitude,	Longitude,	IsAddressValidated,	AddressValidationDate,	AddressValidationSource, AddressValidationMessage, ps.Phone,
		ps.Fax, ps.Mobile, Website, Birthdate, Age, Income, Gender, tmpSubscriptionID, IsLocked,	LockedByUserID,	LockDate,	LockDateRelease, ps.PhoneExt,
		IsInActiveWaveMailing,	AddressTypeCodeId,	AddressLastUpdatedDate,	AddressUpdatedSourceTypeCodeId,	ps.WaveMailingID,	IGrp_No, SFRecordIdentifier 
	FROM PubSubscriptions ps
		JOIN WaveMailingDetail wmd ON wmd.PubSubscriptionID = ps.PubSubscriptionID
		JOIN WaveMailing wm ON wm.WaveMailingID = wmd.WaveMailingID
		JOIN Issue i ON i.IssueId = wm.IssueID
	WHERE i.PublicationId = @ProductID AND i.IsComplete = 0	AND ps.IsInActiveWaveMailing = 1
	END			

	BEGIN
	UPDATE PubSubscriptions
	SET IsInActiveWaveMailing = 0, WaveMailingID = 0
	WHERE PubID = @ProductID AND IsInActiveWaveMailing = 1

	DELETE wmd 
	FROM WaveMailingDetail wmd
		JOIN WaveMailing wm ON wmd.WaveMailingID = wm.WaveMailingID
		JOIN Issue i ON i.IssueID = wm.IssueID
	WHERE i.IsComplete = 0 AND i.PublicationId = @ProductID
	END

END

--INSERT INTO HistorySubscription (PubSubscriptionID,	SubscriptionID,	PubID,	Demo7,	QualificationDate,	PubQSourceID,	PubCategoryID,	PubTransactionID,	EmailStatusID,	StatusUpdatedDate,
--	StatusUpdatedReason, Email,	DateCreated, DateUpdated, CreatedByUserID, UpdatedByUserID,	IsComp,	SubscriptionStatusID,	AccountNumber,	AddRemoveID, Copies,	
--	GraceIssues, IMBSEQ, IsActive, IsPaid,	IsSubscribed, MemberGroup, OnBehalfOf,	OrigsSrc, Par3CID, SequenceID, [Status], SubscriberSourceCode, SubSrcID, Verify,
--	ExternalKeyID,	FirstName,	LastName, Company, Title, Occupation, AddressTypeID, Address1, Address2, Address3, City, RegionCode, RegionID, ZipCode,	Plus4,
--	CarrierRoute, County, Country, CountryID, Latitude,	Longitude,	IsAddressValidated,	AddressValidationDate,	AddressValidationSource, AddressValidationMessage, Phone,
--	Fax, Mobile, Website, Birthdate, Age, Income, Gender, tmpSubscriptionID, IsLocked,	LockedByUserID,	LockDate,	LockDateRelease, PhoneExt,
--	IsInActiveWaveMailing,	AddressTypeCodeId,	AddressLastUpdatedDate,	AddressUpdatedSourceTypeCodeId,	WaveMailingID,	IGrp_No, SFRecordIdentifier)
--SELECT ps.PubSubscriptionID, ps.SubscriptionID,	PubID, (CASE WHEN LEN(wmd.Demo7) > 0 THEN wmd.Demo7 ELSE ps.demo7 END), 
--    QualificationDate,	
--    PubQSourceID, (CASE WHEN wmd.PubCategoryID > 0 THEN wmd.PubCategoryID ELSE ps.PubCategoryID END), 
--    (CASE WHEN wmd.PubTransactionID > 0 THEN wmd.PubTransactionID ELSE ps.PubTransactionID END),	
--    EmailStatusID,	StatusUpdatedDate, StatusUpdatedReason, 
--    (CASE WHEN ISNULL(wmd.Email, '') <> '' THEN wmd.Email ELSE ps.Email END),	GETDATE(), NULL, @UserID, NULL,	IsComp,	
--    (CASE WHEN wmd.SubscriptionStatusID > 0 THEN wmd.SubscriptionStatusID ELSE ps.SubscriptionStatusID END),	
--    AccountNumber,	AddRemoveID, (CASE WHEN LEN(wmd.Copies) > 0 THEN wmd.Copies ELSE ps.Copies END),	
--	GraceIssues, IMBSEQ, IsActive, (CASE WHEN LEN(wmd.IsPaid) > 0 THEN wmd.IsPaid ELSE ps.IsPaid END),	
--	(CASE WHEN LEN(wmd.IsSubscribed) > 0 THEN wmd.IsSubscribed ELSE ps.IsSubscribed END), MemberGroup, OnBehalfOf,	
--	OrigsSrc, Par3CID, SequenceID, [Status], SubscriberSourceCode, SubSrcID, Verify,
--	ExternalKeyID,	(CASE WHEN ISNULL(wmd.FirstName, '') <> '' THEN wmd.FirstName ELSE ps.FirstName END),
--	(CASE WHEN ISNULL(wmd.LastName, '') <> '' THEN wmd.LastName ELSE ps.LastName END), 
--	(CASE WHEN ISNULL(wmd.Company, '') <> '' THEN wmd.Company ELSE ps.Company END), 
--	(CASE WHEN ISNULL(wmd.Title, '') <> '' THEN wmd.Title ELSE ps.Title END), 
--	Occupation, (CASE WHEN LEN(wmd.AddressTypeID) > 0 THEN wmd.AddressTypeID ELSE ps.AddressTypeID END), 
--	(CASE WHEN ISNULL(wmd.Address1, '') <> '' THEN wmd.Address1 ELSE ps.Address1 END), 
--	(CASE WHEN ISNULL(wmd.Address2, '') <> '' THEN wmd.Address2 ELSE ps.Address2 END), 
--	(CASE WHEN ISNULL(wmd.Address3, '') <> '' THEN wmd.Address3 ELSE ps.Address3 END), 
--	(CASE WHEN ISNULL(wmd.City, '') <> '' THEN wmd.City ELSE ps.City END), 
--	(CASE WHEN ISNULL(wmd.RegionCode, '') <> '' THEN wmd.RegionCode ELSE ps.RegionCode END), 
--	(CASE WHEN LEN(wmd.RegionID) > 0 THEN wmd.RegionID ELSE ps.RegionID END), 
--	(CASE WHEN ISNULL(wmd.ZipCode, '') <> '' THEN wmd.ZipCode ELSE ps.ZipCode END), 
--	(CASE WHEN ISNULL(wmd.Plus4, '') <> '' THEN wmd.Plus4 ELSE ps.Plus4 END),
--	CarrierRoute, (CASE WHEN ISNULL(wmd.County, '') <> '' THEN wmd.County ELSE ps.County END),
--	(CASE WHEN ISNULL(wmd.Country, '') <> '' THEN wmd.Country ELSE ps.Country END), 
--	(CASE WHEN LEN(wmd.CountryID) > 0 THEN wmd.CountryID ELSE ps.CountryID END), 
--	Latitude,	Longitude,	IsAddressValidated,	AddressValidationDate,	AddressValidationSource, AddressValidationMessage, 
--	(CASE WHEN ISNULL(wmd.Phone, '') <> '' THEN wmd.Phone ELSE ps.Phone END), (CASE WHEN ISNULL(wmd.Fax, '') <> '' THEN wmd.Fax ELSE ps.Fax END), 
--	(CASE WHEN ISNULL(wmd.Mobile, '') <> '' THEN wmd.Mobile ELSE ps.Mobile END), Website, Birthdate, 
--	Age, Income, Gender, tmpSubscriptionID, IsLocked,	LockedByUserID,	LockDate,	LockDateRelease, ps.PhoneExt,
--	IsInActiveWaveMailing,	AddressTypeCodeId,	AddressLastUpdatedDate,	AddressUpdatedSourceTypeCodeId,	ps.WaveMailingID,	IGrp_No, SFRecordIdentifier 
--	FROM PubSubscriptions ps
--	JOIN WaveMailingDetail wmd ON wmd.PubSubscriptionID = ps.PubSubscriptionID
--	JOIN WaveMailing wm ON wm.WaveMailingID = wmd.WaveMailingID
--	JOIN Issue i ON i.IssueId = wm.IssueID
--	WHERE i.PublicationId = @ProductID AND i.IsComplete = 0	AND ps.IsInActiveWaveMailing = 1
