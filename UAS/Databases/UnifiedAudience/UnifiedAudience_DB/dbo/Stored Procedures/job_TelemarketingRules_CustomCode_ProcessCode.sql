CREATE PROCEDURE [dbo].[job_TelemarketingRules_CustomCode_ProcessCode]
	@ProcessCode varchar(50),
	@FileType varchar(50)
as
BEGIN

	SET NOCOUNT ON

	declare @ActionTypeID int = (select CodeId from UAD_Lookup..Code where CodeName = 'System Generated')
	declare @PubSourceB int = (Select C.CodeId from UAD_Lookup..Code C join UAD_Lookup..CodeType CT on C.CodeTypeId = CT.CodeTypeId where CT.CodeTypeName = 'Qualification Source' and C.CodeValue = 'B')
	declare @DefaultSubSrc varchar(10) = 'TM' + CONVERT(varchar(2), GETDATE(), 101) + Right(Year(getDate()),2)

	Select PS.SubscriptionID , PS.PubSubscriptionID 
	into #KillSubs
	from PubSubscriptions PS WITH(NoLock)
		join SubscriberFinal SF WITH(NoLock) on PS.SequenceID = SF.Sequence
		join Pubs P WITH(NoLock) on SF.PubCode = P.PubCode
	where SF.ProcessCode = @ProcessCode and PS.PubID = P.PubID
		and SF.Sequence > 0
		and (PS.FirstNAME != SF.FNAME or PS.LastNAME != SF.LNAME)	


	if (@FileType = 'Web_Forms')
	BEGIN
		/* Ignore records where Subscription Field Value = 'N' will come in as a demographic field */
		Insert into #KillSubs
		Select PS.SubscriptionID, PS.PubSubscriptionID
		from PubSubscriptions PS WITH(NoLock)
			join Subscriptions S WITH(NoLock) on PS.SubscriptionID = S.SubscriptionID
			join SubscriberFinal SF WITH(NoLock) on S.IGrp_No = SF.IGrp_No
			join SubscriberDemographicFinal SDF With(NoLock) on SF.SFRecordIdentifier = SDF.SFRecordIdentifier
			join Pubs P WITH(NoLock) on SF.PubCode = P.PubCode
		where SF.ProcessCode = @ProcessCode and PS.PubID = P.PubID
			and SDF.MAFField = 'Subscription' and SDF.Value = 'N' and SF.Ignore = 'false'
	END


	/* Remove Subscribers to be killed who are Paid Active/Inactive */
	Delete #KillSubs
	from PubSubscriptions PS
		join #KillSubs KS on PS.PubSubscriptionID = KS.PubSubscriptionID	
	where PS.PubTransactionID in (Select tc.TransactionCodeID
								From UAD_Lookup..Action a
									join UAD_Lookup..CategoryCode cc on a.CategoryCodeID = cc.CategoryCodeID
									join UAD_Lookup..TransactionCode tc on a.TransactionCodeID = tc.TransactionCodeID
								where cc.CategoryCodeID = PS.PubCategoryID and tc.TransactionCodeID = PS.PubTransactionID
									and tc.TransactionCodeTypeID in (3,4)
									and a.IsActive = 'true'
									and a.ActionTypeID = @ActionTypeID)

	if exists (select top 1 1 from #KillSubs)
	Begin

		declare @killTransactionCodeID int,
				@SubscriptionStatusID int

		select @killTransactionCodeID = TransactionCodeID from uad_lookup..transactioncode
		where transactioncodevalue = 32

		select @SubscriptionStatusID = SubscriptionStatusID from uad_lookup..SubscriptionStatus where statuscode = 'IAFree'

	
		declare @BatchCount int = (Select count(*) from #KillSubs)
		declare @PubID int = (Select TOP 1 P.PubID from Pubs P join SubscriberFinal SF on P.PubCode = SF.PubCode where SF.ProcessCode = @ProcessCode and LEN(SF.PubCode ) > 0)


		--Update PubSubscription Record as Killed
		Update PS
		set PubTransactionID = @killTransactionCodeID,
			IsSubscribed = 'false',
			SubscriptionStatusID = (Select SubscriptionStatusID From UAD_Lookup..SubscriptionStatusMatrix 
										where CategoryCodeID = PS.PubCategoryID 
											and TransactionCodeID = @killTransactionCodeID
											and IsActive = 'true'),
			UpdatedByUserID = 1, DateUpdated = GETDATE()								
		from PubSubscriptions PS 
			join #KillSubs KS on PS.PubSubscriptionID = KS.PubSubscriptionID
		where PS.IsInActiveWaveMailing = 'false'


		--Update Subscription Record as Killed
		Update Sub
		set Sub.TransactionID = @killTransactionCodeID,
			Sub.UpdatedByUserID = 1, Sub.DateUpdated = GETDATE()
		from Subscriptions Sub
			join #KillSubs KS on Sub.SubscriptionID = KS.SubscriptionID	
			join PubSubscriptions PS on KS.PubSubscriptionID = PS.PubSubscriptionID
		where PS.IsInActiveWaveMailing = 'false'	


		--Update to Wave Mailing Detail 
		Update wmd
			set PubTransactionID = @killTransactionCodeID,
				SubscriptionStatusID = (Select SubscriptionStatusID 
											From UAD_Lookup..SubscriptionStatusMatrix 
											where CategoryCodeID = PS.PubCategoryID 
												and TransactionCodeID = @killTransactionCodeID and IsActive = 'true'),
				IsSubscribed = 'false',
				DateUpdated = GETDATE(),
				UpdatedByUserID = 1
			from PubSubscriptions PS 
				join #KillSubs KS on PS.PubSubscriptionID = KS.PubSubscriptionID
				left join WaveMailingDetail wmd on PS.PubSubscriptionID = wmd.PubSubscriptionID				
			where PS.IsInActiveWaveMailing = 'true' and wmd.PubSubscriptionID is not null


		--Insert to Wave Mailing Detail for those in wave mailing
		Insert into WaveMailingDetail (WaveMailingID,PubSubscriptionID,SubscriptionID,PubTransactionID,SubscriptionStatusID,IsSubscribed,DateCreated,CreatedByUserID)
		Select	
			(Select wm.WaveMailingID 
			From WaveMailing wm
				join Issue i ON i.PublicationId = PS.PubID and i.IssueId = wm.IssueID
			where wm.WaveMailingID = PS.WaveMailingID and i.IsComplete = 'false')
			,PS.PubSubscriptionID,KS.SubscriptionID,@killTransactionCodeID,
			(Select SubscriptionStatusID 
			From UAD_Lookup..SubscriptionStatusMatrix 
			where CategoryCodeID = PS.PubCategoryID 
				and TransactionCodeID = @killTransactionCodeID and IsActive = 'true'),
			'false',
			GETDATE(),
			1 
		from PubSubscriptions PS 
			join #KillSubs KS on PS.PubSubscriptionID = KS.PubSubscriptionID
			left join WaveMailingDetail wmd on PS.PubSubscriptionID = wmd.PubSubscriptionID
		where PS.IsInActiveWaveMailing = 'true' and wmd.PubSubscriptionID is null
	
			
		/* INSERT HISTORY */
		declare @PubBatch table (PubID int, BatchID int)
		declare @HistorySubscription table (HistorySubscriptionID int, SubscriptionID int)
		declare @History  table (HistoryID int, PubSubscriptionID int)


		insert into Batch (PublicationID, UserID, BatchCount,IsActive, DateCreated, DateFinalized, BatchNumber)
		OUTPUT Inserted.PublicationID, Inserted.BatchID
		INTO @PubBatch
		select @PubID, 1, @BatchCount, 1, GETDATE(), GETDATE() , (select isnull(MAX(batchnumber),0) + 1 from Batch b with (NOLOCK) where b.PublicationID = @PubID) 										


		--PubSubscriptionDetail History
		Insert into HistoryResponseMap (PubSubscriptionDetailID,PubSubscriptionID,SubscriptionID,CodeSheetID,IsActive,DateCreated,CreatedByUserID,ResponseOther, HistorySubscriptionID)
		Select PubSubscriptionDetailID, t.PubSubscriptionID,t.SubscriptionID,CodeSheetID,'true',GETDATE(),1,ResponseOther, t.PubSubscriptionID 
		from PubSubscriptions t With(nolock)
			join #KillSubs ks on t.PubSubscriptionID = ks.PubSubscriptionID
			join Pubs p with (NOLOCK) on t.pubID = p.PubID 
			join PubSubscriptionDetail psd with (NOLOCK) on t.PubSubscriptionID = psd.PubSubscriptionID 
		where isnull(p.IsCirc,0) = 1

				
		--PubSubscription History
		insert into HistorySubscription (PubSubscriptionID,SubscriptionID,PubID,Demo7,QualificationDate,PubQSourceID,PubCategoryID,PubTransactionID,EmailStatusID,StatusUpdatedDate, StatusUpdatedReason,Email,DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID,IsComp,SubscriptionStatusID,AccountNumber,AddRemoveID,Copies,
			GraceIssues,IMBSEQ,IsActive,IsPaid,IsSubscribed,MemberGroup,OnBehalfOf,OrigsSrc,Par3CID,SequenceID,Status,SubscriberSourceCode,SubSrcID,Verify,
			ExternalKeyID,FirstName,LastName,Company,Title,Occupation,AddressTypeID,Address1,Address2,Address3,City,RegionCode,RegionID,ZipCode,Plus4,
			CarrierRoute,County,Country,CountryID,Latitude,Longitude,IsAddressValidated,AddressValidationDate,AddressValidationSource,AddressValidationMessage,
			Phone,Fax,Mobile,Website,Birthdate,Age,Income,Gender,tmpSubscriptionID,IsLocked,LockedByUserID,LockDate,LockDateRelease,PhoneExt,
			IsInActiveWaveMailing,AddressTypeCodeId,AddressLastUpdatedDate,AddressUpdatedSourceTypeCodeId,WaveMailingID,IGrp_No,SFRecordIdentifier)                       
		OUTPUT Inserted.HistorySubscriptionID, Inserted.SubscriptionID
		INTO @HistorySubscription           
			select t.PubSubscriptionID,t.SubscriptionID,PubID,Demo7,QualificationDate,PubQSourceID,PubCategoryID,PubTransactionID,EmailStatusID,StatusUpdatedDate,StatusUpdatedReason,
				Email,GETDATE(),DateUpdated,1,UpdatedByUserID,IsComp,SubscriptionStatusID,AccountNumber,AddRemoveID,Copies,GraceIssues,IMBSEQ,IsActive,IsPaid,
				IsSubscribed,MemberGroup,OnBehalfOf,OrigsSrc,Par3CID,SequenceID,Status,SubscriberSourceCode,SubSrcID,Verify,ExternalKeyID,FirstName,LastName,Company,Title,Occupation,
				AddressTypeID,Address1,Address2,Address3,City,RegionCode,RegionID,ZipCode,Plus4,CarrierRoute,County,Country,CountryID,Latitude,Longitude,IsAddressValidated,
				AddressValidationDate,AddressValidationSource,AddressValidationMessage,Phone,Fax,Mobile,Website,Birthdate,Age,Income,Gender,tmpSubscriptionID,IsLocked,LockedByUserID,
				LockDate,LockDateRelease,PhoneExt,IsInActiveWaveMailing,AddressTypeCodeId,AddressLastUpdatedDate,AddressUpdatedSourceTypeCodeId,WaveMailingID,IGrp_No,SFRecordIdentifier       
			from PubSubscriptions t With(nolock)
				join #KillSubs ks on t.PubSubscriptionID = ks.PubSubscriptionID					


		Insert Into History (BatchID,BatchCountItem, PublicationID, PubSubscriptionID, SubscriptionID,HistorySubscriptionID,DateCreated,CreatedByUserID)
				OUTPUT Inserted.HistoryID, Inserted.PubSubscriptionID
				INTO @History
		select pb.BatchID, (row_number() over (partition by t.PubSubscriptionID order by t.PubSubscriptionID)), t.PubID, t.PubSubscriptionID, t.SubscriptionID, hs.HistorySubscriptionID,GETDATE(),1
		from PubSubscriptions t With(nolock)
			join #KillSubs ks on t.PubSubscriptionID = ks.PubSubscriptionID	
			join @PubBatch pb on t.PubID = pb.PubID
			join HistorySubscription hs with(nolock) on t.SubscriptionID = hs.SubscriptionID 
		where hs.HistorySubscriptionID in (Select HistorySubscriptionID from @HistorySubscription)
					
	End

	drop table #KillSubs

End