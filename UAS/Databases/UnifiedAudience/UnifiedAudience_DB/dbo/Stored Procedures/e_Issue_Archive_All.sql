
CREATE PROCEDURE [dbo].[e_Issue_Archive_All]
@ProductID int,
@IssueID int
AS
BEGIN

	Declare @Loc_ProductID int,
			@Loc_IssueID int

	SET NOCOUNT ON
	
	SET @Loc_ProductID = @ProductID
	SET @Loc_IssueID = @IssueID

	INSERT INTO IssueArchiveProductSubscription (IsComp, CompId, IssueID, IMBSEQ, PubSubscriptionID, SubscriptionID, PubID, Demo7, QualificationDate, PubQSourceID, PubCategoryID, PubTransactionID,
	EmailStatusID, StatusUpdatedDate, StatusUpdatedReason, Email, DateCreated, DateUpdated, CreatedByUserID, UpdatedByUserID, SubscriptionStatusID, AccountNumber, AddRemoveID, Copies, GraceIssues,
	IsActive, IsPaid, IsSubscribed, MemberGroup, OnBehalfOf, OrigsSrc, Par3CID, SequenceID, [Status], SubscriberSourceCode, SubSrcID, Verify, ExternalKeyID, FirstName, LastName,
	Company, Title, Occupation, AddressTypeID, Address1, Address2, Address3, City, RegionCode, RegionID, ZipCode, Plus4, CarrierRoute, County, Country, CountryID, Latitude, Longitude, IsAddressValidated,
	AddressValidationDate, AddressValidationSource, AddressValidationMessage, Phone, Fax, Mobile, Website, Birthdate, Age, Income, Gender, tmpSubscriptionID, IsLocked, LockedByUserID,
	LockDate, LockDateRelease, PhoneExt, IsInActiveWaveMailing, AddressTypeCodeId, AddressLastUpdatedDate, AddressUpdatedSourceTypeCodeId, WaveMailingID, IGrp_No, SFRecordIdentifier, ReqFlag, SubGenSubscriberID,
	MailPermission, FaxPermission, PhonePermission, OtherProductsPermission, EmailRenewPermission, ThirdPartyPermission, TextPermission)
	(SELECT 0, 0, @Loc_IssueID, ISNULL(IMBSEQ, '0'), ps.PubSubscriptionID, SubscriptionID, PubID, Demo7, QualificationDate, PubQSourceID, PubCategoryID, PubTransactionID,
	EmailStatusID, StatusUpdatedDate, StatusUpdatedReason, Email, DateCreated, DateUpdated, CreatedByUserID, UpdatedByUserID, SubscriptionStatusID, AccountNumber, AddRemoveID, Copies, GraceIssues,
	IsActive, IsPaid, IsSubscribed, MemberGroup, OnBehalfOf, OrigsSrc, Par3CID, SequenceID, [Status], SubscriberSourceCode, SubSrcID, Verify, ExternalKeyID, FirstName, LastName,
	Company, Title, Occupation, AddressTypeID, Address1, Address2, Address3, City, RegionCode, RegionID, ZipCode, Plus4, CarrierRoute, County, Country, CountryID, Latitude, Longitude, IsAddressValidated,
	AddressValidationDate, AddressValidationSource, AddressValidationMessage, Phone, Fax, Mobile, Website, Birthdate, Age, Income, Gender, tmpSubscriptionID, IsLocked, LockedByUserID,
	LockDate, LockDateRelease, PhoneExt, IsInActiveWaveMailing, AddressTypeCodeId, AddressLastUpdatedDate, AddressUpdatedSourceTypeCodeId, WaveMailingID, IGrp_No, SFRecordIdentifier, ReqFlag, SubGenSubscriberID,
	MailPermission, FaxPermission, PhonePermission, OtherProductsPermission, EmailRenewPermission, ThirdPartyPermission, TextPermission
	FROM PubSubscriptions ps with(nolock)
	WHERE ps.PubID = @Loc_ProductID AND ps.PubSubscriptionID NOT IN (Select PubSubscriptionID FROM IssueArchiveProductSubscription ias with(nolock) WHERE ias.IssueID = @Loc_IssueID))

	INSERT INTO IssueArchiveProductSubscriptionDetail (IssueArchiveSubscriptionId, PubSubscriptionID, SubscriptionID, CodesheetID, DateCreated, DateUpdated, 
		CreatedByUserID, UpdatedByUserID, ResponseOther)
	SELECT iap.IssueArchiveSubscriptionId, psd.PubSubscriptionID, psd.SubscriptionID, psd.CodesheetID, GETDATE(), psd.DateUpdated,
		psd.CreatedByUserID, psd.UpdatedByUserID, psd.ResponseOther FROM PubSubscriptionDetail psd with(nolock)
		JOIN IssueArchiveProductSubscription iap with(nolock) ON psd.PubSubscriptionID = iap.PubSubscriptionID
	WHERE iap.IssueID = @Loc_IssueID
	
	INSERT INTO IssueArchivePubSubscriptionsExtension (IssueArchiveSubscriptionID, Field1,Field2,Field3,Field4,Field5,Field6,Field7,Field8,Field9,Field10,Field11,Field12,Field13,Field14,Field15,Field16,Field17,
		Field18,Field19,Field20,Field21,Field22,Field23,Field24,Field25,Field26,Field27,Field28,Field29,Field30,Field31,Field32,Field33,Field34,Field35,Field36,Field37,Field38,Field39,Field40,Field41,Field42,
		Field43,Field44,Field45,Field46,Field47,Field48,Field49,Field50,Field51,Field52,Field53,Field54,Field55,Field56,Field57,Field58,Field59,Field60,Field61,Field62,Field63,Field64,Field65,Field66,Field67,
		Field68,Field69,Field70,Field71,Field72,Field73,Field74,Field75,Field76,Field77,Field78,Field79,Field80,Field81,Field82,Field83,Field84,Field85,Field86,Field87,Field88,Field89,Field90,Field91,Field92,
		Field93,Field94,Field95,Field96,Field97,Field98,Field99,Field100, DateCreated, DateUpdated, CreatedByUserID, UpdatedByUserID)
	SELECT ias.IssueArchiveSubscriptionId, Field1,Field2,Field3,Field4,Field5,Field6,Field7,Field8,Field9,Field10,Field11,Field12,Field13,Field14,Field15,Field16,Field17,Field18,Field19,Field20,Field21,
		Field22,Field23,Field24,Field25,Field26,Field27,Field28,Field29,Field30,Field31,Field32,Field33,Field34,Field35,Field36,Field37,Field38,Field39,Field40,Field41,Field42,Field43,Field44,Field45,Field46,
		Field47,Field48,Field49,Field50,Field51,Field52,Field53,Field54,Field55,Field56,Field57,Field58,Field59,Field60,Field61,Field62,Field63,Field64,Field65,Field66,Field67,Field68,Field69,Field70,Field71,
		Field72,Field73,Field74,Field75,Field76,Field77,Field78,Field79,Field80,Field81,Field82,Field83,Field84,Field85,Field86,Field87,Field88,Field89,Field90,Field91,Field92,Field93,Field94,Field95,Field96,
		Field97,Field98,Field99,Field100, GETDATE(), pe.DateUpdated, pe.CreatedByUserID, pe.UpdatedByUserID
	FROM PubSubscriptionsExtension pe with(nolock)
		JOIN IssueArchiveProductSubscription ias with(nolock) ON ias.PubSubscriptionID = pe.PubSubscriptionID
	WHERE ias.IssueID = @Loc_IssueID

END
GO


