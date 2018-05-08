CREATE PROCEDURE [dbo].[e_Issue_Archive_IMB_PubSubscriptions]
@ProductID int,
@IssueID int,
@IMBSequences TEXT
AS
BEGIN

	SET NOCOUNT ON

	BEGIN --SET UP
		CREATE TABLE #IMB
		(  
			RowID int IDENTITY(1, 1)
		  ,[PubSubscriptionID] int
		  ,[IMBSeq] nvarchar(256)
		)
	
		DECLARE @docHandle int

		EXEC sp_xml_preparedocument @docHandle OUTPUT, @IMBSequences
		INSERT INTO #IMB 
		(
			 [PubSubscriptionID], [IMBSeq]
		)  
		SELECT [PubSubscriptionID], [IMBSeq]
		FROM OPENXML(@docHandle,N'/XML/PS')
		WITH
		(
			[PubSubscriptionID] int 'ID',
			[IMBSeq] nvarchar(256) 'IMB'
		)

		EXEC sp_xml_removedocument @docHandle
	
	END
	
	BEGIN --INSERTS
	
		IF EXISTS(Select 1 FROM #IMB)
			BEGIN
				INSERT INTO IssueArchiveProductSubscription (IsComp, CompId, IssueID, IMBSEQ, PubSubscriptionID, SubscriptionID, PubID, Demo7, QualificationDate, PubQSourceID, PubCategoryID, PubTransactionID,
				EmailStatusID, StatusUpdatedDate, StatusUpdatedReason, Email, DateCreated, DateUpdated, CreatedByUserID, UpdatedByUserID, SubscriptionStatusID, AccountNumber, AddRemoveID, Copies, GraceIssues,
				IsActive, IsPaid, IsSubscribed, MemberGroup, OnBehalfOf, OrigsSrc, Par3CID, SequenceID, [Status], SubscriberSourceCode, SubSrcID, Verify, ExternalKeyID, FirstName, LastName,
				Company, Title, Occupation, AddressTypeID, Address1, Address2, Address3, City, RegionCode, RegionID, ZipCode, Plus4, CarrierRoute, County, Country, CountryID, Latitude, Longitude, IsAddressValidated,
				AddressValidationDate, AddressValidationSource, AddressValidationMessage, Phone, Fax, Mobile, Website, Birthdate, Age, Income, Gender, tmpSubscriptionID, IsLocked, LockedByUserID,
				LockDate, LockDateRelease, PhoneExt, IsInActiveWaveMailing, AddressTypeCodeId, AddressLastUpdatedDate, AddressUpdatedSourceTypeCodeId, WaveMailingID, IGrp_No, SFRecordIdentifier, ReqFlag)
				(SELECT 0, 0, @IssueID, i.IMBSeq, ps.PubSubscriptionID, SubscriptionID, PubID, Demo7, QualificationDate, PubQSourceID, PubCategoryID, PubTransactionID,
				EmailStatusID, StatusUpdatedDate, StatusUpdatedReason, Email, DateCreated, DateUpdated, CreatedByUserID, UpdatedByUserID, SubscriptionStatusID, AccountNumber, AddRemoveID, Copies, GraceIssues,
				IsActive, IsPaid, IsSubscribed, MemberGroup, OnBehalfOf, OrigsSrc, Par3CID, SequenceID, [Status], SubscriberSourceCode, SubSrcID, Verify, ExternalKeyID, FirstName, LastName,
				Company, Title, Occupation, AddressTypeID, Address1, Address2, Address3, City, RegionCode, RegionID, ZipCode, Plus4, CarrierRoute, County, Country, CountryID, Latitude, Longitude, IsAddressValidated,
				AddressValidationDate, AddressValidationSource, AddressValidationMessage, Phone, Fax, Mobile, Website, Birthdate, Age, Income, Gender, tmpSubscriptionID, IsLocked, LockedByUserID,
				LockDate, LockDateRelease, PhoneExt, IsInActiveWaveMailing, AddressTypeCodeId, AddressLastUpdatedDate, AddressUpdatedSourceTypeCodeId, WaveMailingID, IGrp_No, SFRecordIdentifier, ReqFlag FROM PubSubscriptions ps
				JOIN #IMB i ON i.PubSubscriptionID = ps.PubSubscriptionID
				WHERE ps.PubID = @ProductID)
			END	
	
		DROP TABLE #IMB
	
	END
END