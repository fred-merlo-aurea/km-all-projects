CREATE PROCEDURE [dbo].[e_ProductSubscription_Update_Address]
@PubSubscriptionID	int,
@Address1 varchar(100) = '',
@Address2 varchar(100) = '',
@Address3 varchar(100) = '',
@City varchar(50) = '',
@RegionCode varchar(50) = '',
@ZipCode varchar(10) = '',
@CountryID int = 0,
@Phone varchar(50) = '',
@Fax varchar(50) = '',
@Email varchar(100),
@EmailStatusID int,
@UpdatedByUserID int
AS
BEGIN

	SET NOCOUNT ON

	declare @Country varchar(100)

	select @Country=ShortName  
	from UAD_Lookup..Country 
	where CountryID = @CountryID

	Update PubSubscriptions
	Set Address1 = @Address1,
		Address2 = @Address2,
		Address3 = @Address3,
		City = @City,
		RegionCode = @RegionCode,
		ZipCode = @ZipCode,
		CountryID = @CountryID,
		Country = @Country,
		Phone = @Phone,
		Fax = @Fax,
		Email = @Email,
		DateUpdated = GETDATE(),
		UpdatedByUserID = @UpdatedByUserID
	Where PubSubscriptionID = @PubSubscriptionID
		
	declare @OldEmailStatusID int
	
	Select @OldEmailStatusID = iSNULL(EmailStatusID,0) 
	from PubSubscriptions 
	where PubSubscriptionID = @PubSubscriptionID
		
	if (@EmailStatusID != @OldEmailStatusID) 	
		Begin
			update PubSubscriptions 
			set EmailStatusID = @EmailStatusID, StatusUpdatedDate =  GETDATE() 
			where  PubSubscriptionID = @PubSubscriptionID  
		end		
		
	--Update History table for circ products
		
	 IF EXISTS(Select ps.PubSubscriptionID From PubSubscriptions ps With(NoLock) join Pubs p With(NoLock) on ps.PubID = p.PubID where p.IsCirc = 1 and  pubsubscriptionID = @PubSubscriptionID)
	 Begin
		declare @pubID int = 0,
				@batchID int,
				@subscriptionID int,
				@HistorySubscriptionID int			
		
		Select @pubID = ps.pubID 
		From 
				PubSubscriptions ps With(NoLock) join 
				Pubs p With(NoLock) on ps.PubID = p.PubID 
		where 
				p.IsCirc = 1 and  pubsubscriptionID = @PubSubscriptionID
		
		if (@pubID > 0)
		Begin
		
			select @subscriptionID = subscriptionID 
			from 
					PubSubscriptions ps with (NOLOCK) 
			where  
					pubsubscriptionID = @PubSubscriptionID 
					
			insert into Batch (PublicationID, UserID, BatchCount,IsActive, DateCreated, DateFinalized, BatchNumber)
			Values(@pubID, @UpdatedByUserID, 1, 1, GETDATE() , GETDATE(), (select isnull(MAX(batchnumber),0) + 1 from Batch b with (NOLOCK) where b.PublicationID = @pubID))
			
			set @batchID = SCOPE_IDENTITY()
			
			--PubSubscription History
			insert into HistorySubscription (PubSubscriptionID,SubscriptionID,PubID,Demo7,QualificationDate,PubQSourceID,PubCategoryID,PubTransactionID,EmailStatusID,--StatusUpdatedDate, StatusUpdatedReason,
					Email,DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID,IsComp,SubscriptionStatusID,AccountNumber,AddRemoveID,Copies,
					GraceIssues,IMBSEQ,IsActive,IsPaid,IsSubscribed,MemberGroup,OnBehalfOf,OrigsSrc,Par3CID,SequenceID,Status,SubscriberSourceCode,SubSrcID,Verify,
					ExternalKeyID,FirstName,LastName,Company,Title,Occupation,AddressTypeID,Address1,Address2,Address3,City,RegionCode,RegionID,ZipCode,Plus4,
					CarrierRoute,County,Country,CountryID,Latitude,Longitude,IsAddressValidated,AddressValidationDate,AddressValidationSource,AddressValidationMessage,
					Phone,Fax,Mobile,Website,Birthdate,Age,Income,Gender,tmpSubscriptionID,IsLocked,LockedByUserID,LockDate,LockDateRelease,PhoneExt,
					IsInActiveWaveMailing,AddressTypeCodeId,AddressLastUpdatedDate,AddressUpdatedSourceTypeCodeId,WaveMailingID,IGrp_No,SFRecordIdentifier)                       
			select ps.PubSubscriptionID,ps.SubscriptionID,ps.PubID,Demo7,QualificationDate,PubQSourceID,PubCategoryID,PubTransactionID,EmailStatusID,--StatusUpdatedDate,StatusUpdatedReason,
					Email,GETDATE(),ps.DateUpdated,ps.createdByUserID,ps.UpdatedByUserID,IsComp,SubscriptionStatusID,AccountNumber,AddRemoveID,Copies,GraceIssues,IMBSEQ,ps.IsActive,IsPaid,
					IsSubscribed,MemberGroup,OnBehalfOf,OrigsSrc,Par3CID,SequenceID,Status,SubscriberSourceCode,SubSrcID,Verify,ExternalKeyID,FirstName,LastName,Company,Title,Occupation,
					AddressTypeID,Address1,Address2,Address3,City,RegionCode,RegionID,ZipCode,Plus4,CarrierRoute,County,Country,CountryID,Latitude,Longitude,IsAddressValidated,
					AddressValidationDate,AddressValidationSource,AddressValidationMessage,Phone,Fax,Mobile,Website,Birthdate,Age,Income,Gender,tmpSubscriptionID,IsLocked,LockedByUserID,
					LockDate,LockDateRelease,PhoneExt,IsInActiveWaveMailing,AddressTypeCodeId,AddressLastUpdatedDate,AddressUpdatedSourceTypeCodeId,WaveMailingID,ps.IGrp_No,SFRecordIdentifier       
				from 
				   PubSubscriptions ps with (NOLOCK) 
			where  pubsubscriptionID = @PubSubscriptionID 
			
			set @HistorySubscriptionID = SCOPE_IDENTITY()
			
			--PubSubscriptionDetail History
			Insert into HistoryResponseMap (PubSubscriptionDetailID,PubSubscriptionID,SubscriptionID,CodeSheetID,IsActive,DateCreated,CreatedByUserID,ResponseOther, HistorySubscriptionID)
			Select PubSubscriptionDetailID, psd.PubSubscriptionID, psd.SubscriptionID,CodeSheetID,'true',GETDATE(),1,ResponseOther, @HistorySubscriptionID
			from 
				PubSubscriptionDetail psd with (NOLOCK) 
			where psd.pubsubscriptionID = @PubSubscriptionID 
			
			Insert Into History (BatchID, BatchCountItem, PublicationID, PubSubscriptionID, SubscriptionID,HistorySubscriptionID,DateCreated,CreatedByUserID)
			values ( @BatchID, 1, @PubID, @PubSubscriptionID, @SubscriptionID, @HistorySubscriptionID,GETDATE(), @UpdatedByUserID )

		End
	End
END