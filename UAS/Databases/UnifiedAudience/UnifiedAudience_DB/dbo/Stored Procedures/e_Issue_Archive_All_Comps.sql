CREATE PROCEDURE [dbo].[e_Issue_Archive_All_Comps]
@ProductID int,
@IssueID int,
@CompIMBSequences TEXT
AS
BEGIN

-- PER Bobby, removing writing comp  to archive, it was decided they should not be there
	SET NOCOUNT ON

	--BEGIN --SET UP
	
	--	CREATE TABLE #IMBComp
	--	(  
	--		RowID int IDENTITY(1, 1)
	--	  ,[IssueCompDetailId] int
	--	  ,[IMBSeq] nvarchar(256)
	--	)
	
	--	DECLARE @docHandle2 int
	
	--	EXEC sp_xml_preparedocument @docHandle2 OUTPUT, @CompIMBSequences  
	--	INSERT INTO #IMBComp 
	--	(
	--		 [IssueCompDetailId], [IMBSeq]
	--	)  
	--	SELECT [IssueCompDetailId], [IMBSeq]
	--	FROM OPENXML(@docHandle2,N'/XML/IS')
	--	WITH
	--	(
	--		[IssueCompDetailId] int 'ID',
	--		[IMBSeq] nvarchar(256) 'IMB'
	--	)

	--	EXEC sp_xml_removedocument @docHandle2
	
	--END
	
	--BEGIN --INSERTS
	
	--	BEGIN
	--		INSERT INTO IssueArchiveProductSubscription (IsComp, CompId, IssueID, IMBSEQ, PubSubscriptionID, SubscriptionID, PubID, Demo7, QualificationDate, PubQSourceID, PubCategoryID, PubTransactionID,
	--		EmailStatusID, StatusUpdatedDate, StatusUpdatedReason, Email, DateCreated, DateUpdated, CreatedByUserID, UpdatedByUserID, SubscriptionStatusID, AccountNumber, AddRemoveID, Copies, GraceIssues,
	--		IsActive, IsPaid, IsSubscribed, MemberGroup, OnBehalfOf, OrigsSrc, Par3CID, SequenceID, [Status], SubscriberSourceCode, SubSrcID, Verify, ExternalKeyID, FirstName, LastName,
	--		Company, Title, Occupation, AddressTypeID, Address1, Address2, Address3, City, RegionCode, RegionID, ZipCode, Plus4, CarrierRoute, County, Country, CountryID, Latitude, Longitude, IsAddressValidated,
	--		AddressValidationDate, AddressValidationSource, AddressValidationMessage, Phone, Fax, Mobile, Website, Birthdate, Age, Income, Gender, tmpSubscriptionID, IsLocked, LockedByUserID,
	--		LockDate, LockDateRelease, PhoneExt, IsInActiveWaveMailing, AddressTypeCodeId, AddressLastUpdatedDate, AddressUpdatedSourceTypeCodeId, WaveMailingID, IGrp_No, SFRecordIdentifier, ReqFlag)
	--		(SELECT 1, icd.IssueCompID, ic.IssueID, i.IMBSeq, 0, 0, PubID, Demo7, QualificationDate, PubQSourceID, PubCategoryID, PubTransactionID,
	--		EmailStatusID, StatusUpdatedDate, StatusUpdatedReason, Email, icd.DateCreated, icd.DateUpdated, icd.CreatedByUserID, icd.UpdatedByUserID, SubscriptionStatusID, AccountNumber, AddRemoveID, Copies, GraceIssues,
	--		icd.IsActive, IsPaid, IsSubscribed, MemberGroup, OnBehalfOf, OrigsSrc, Par3CID, SequenceID, [Status], SubscriberSourceCode, SubSrcID, Verify, ExternalKeyID, FirstName, LastName,
	--		Company, Title, Occupation, AddressTypeID, Address1, Address2, Address3, City, RegionCode, RegionID, ZipCode, Plus4, CarrierRoute, County, Country, CountryID, Latitude, Longitude, 0,
	--		AddressValidationDate, AddressValidationSource, AddressValidationMessage, Phone, Fax, Mobile, Website, Birthdate, Age, Income, Gender, tmpSubscriptionID, IsLocked, LockedByUserID,
	--		LockDate, LockDateRelease, PhoneExt, isnull(IsInActiveWaveMailing,0), AddressTypeCodeId, AddressLastUpdatedDate, AddressUpdatedSourceTypeCodeId, WaveMailingID, IGrp_No, SFRecordIdentifier, ReqFlag FROM IssueCompDetail icd
	--		JOIN IssueComp ic ON ic.IssueCompId = icd.IssueCompID
	--		LEFT JOIN #IMBComp i ON i.IssueCompDetailId = icd.IssueCompDetailId
	--		WHERE ic.IssueId = @IssueID AND ic.IsActive = 1)
	--	END
	
	--	DROP TABLE #IMBComp
	
	--END

END