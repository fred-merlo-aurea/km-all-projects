CREATE PROCEDURE [dbo].[sp_FieldUpdateBlankProfileFields]
(
	@PubID int,
	@SequenceIDs varchar(max),
	@Profilefields varchar(1000)
)
as
Begin
	
	set nocount on

	declare @execString varchar(4000) = ''
	DECLARE @xml as xml,
			@delimiter as varchar(10) = ','

	SET @xml = cast(('<X>'+replace(@SequenceIDs,@delimiter ,'</X><X>')+'</X>') as xml)

	--print ('start : ' + convert(varchar(100), getdate(), 109))

	Create Table #tmpPS (PubID int, PubSubscriptionID int, SubscriptionID int)

	insert into #tmpPS
	select PubID, PubSubscriptionID, SubscriptionID from pubsubscriptions ps with (NOLOCK) join (SELECT N.value('.', 'int') as SequenceID FROM @xml.nodes('X') as T(N)) x on ps.SequenceID = x.SequenceID  where pubID = @pubID

	--print ('insert into #tmpPS : ' + convert(varchar(100), getdate(), 109))

	--select ps.PubSubscriptionID,Company,Email,Fax,Mobile,Phone,Title, datecreated, dateupdated from pubsubscriptions ps with (NOLOCK) join #tmpPS t on ps.PubSubscriptionID = t.PubSubscriptionID

	set @execString = 'update ps set ' + replace(@Profilefields, ',', ' = null, ') +  ' = null from pubsubscriptions ps join #tmpPS t on ps.PubSubscriptionID = t.PubSubscriptionID '

	----print ( @execString)
	exec ( @execString)
	--print ('exec update: ' + convert(varchar(100), getdate(), 109))
	-- update history tables.


	if exists (select top 1 1 from Pubs where pubID = @pubID and isnull(IsCirc,0) = 1)
	Begin

	
		Create table #PubBatch (PubID int, BatchID int)
		Create table #HistoryResponseMap (HistoryResponseMapID int, PubSubscriptionID int)
		Create table #HistorySubscription (HistorySubscriptionID int, SubscriptionID int, PubSubscriptionID int)
		Create table #History  (HistoryID int, PubSubscriptionID int)

		CREATE INDEX idx_#psdNewDimensions_PubSubscriptionID ON #History(PubSubscriptionID)

		CREATE INDEX idx_PubBatch_BatchID ON #PubBatch(BatchID)

		CREATE INDEX idx_History_HistoryID ON #History(HistoryID)
		CREATE INDEX idx_History_PubSubscriptionID ON #History(PubSubscriptionID)
	
		CREATE INDEX idx_HistoryResponseMap_HistoryResponseMapID ON #HistoryResponseMap(HistoryResponseMapID)
		CREATE INDEX idx_HistoryRespinseMap_PubSubscriptionID ON #HistoryResponseMap(PubSubscriptionID)
	
		CREATE INDEX idx_HistorySubscription_HistorySubscriptionID ON #HistorySubscription(HistorySubscriptionID)
		CREATE INDEX idx_HistorySubscription_SubscriptionID ON #HistorySubscription(SubscriptionID)
		CREATE INDEX idx_HistorySubscription_PubSubscriptionID ON #HistorySubscription(PubSubscriptionID)


		insert into Batch (PublicationID, UserID, BatchCount,IsActive, DateCreated, DateFinalized, BatchNumber)
		OUTPUT Inserted.PublicationID, Inserted.BatchID
		INTO #PubBatch
		values (@PubID, 1, (select COUNT(*) from #tmpPS), 1, GETDATE() , GETDATE() , (select isnull(MAX(batchnumber),0) + 1 from Batch b with (NOLOCK) where b.PublicationID = @PubID))

			
		--print ('insert into Batch COUNT : ' +  convert(varchar(100),@@ROWCOUNT) + ' / ' + convert(varchar(20), getdate(), 114))
			
		--PubSubscriptionDetail History
		Insert into HistoryResponseMap (PubSubscriptionDetailID,PubSubscriptionID,SubscriptionID,CodeSheetID,IsActive,DateCreated,CreatedByUserID,ResponseOther, HistorySubscriptionID)
		OUTPUT Inserted.HistoryResponseMapID, Inserted.PubSubscriptionID
		INTO #HistoryResponseMap
		Select PubSubscriptionDetailID, t.PubSubscriptionID, t.SubscriptionID, CodeSheetID, 'true', GETDATE(), 1, ResponseOther, -1 
		from 
				#tmpPS t join
				PubSubscriptionDetail psd with (NOLOCK) on t.PubSubscriptionID = psd.PubSubscriptionID 
		where psd.CodesheetID is not NULL

		--print ('Insert into HistoryResponseMap COUNT : ' +  convert(varchar(100),@@ROWCOUNT) + ' / ' + convert(varchar(20), getdate(), 114))

		--PubSubscription History
		insert into HistorySubscription (PubSubscriptionID,SubscriptionID,PubID,Demo7,QualificationDate,PubQSourceID,PubCategoryID,PubTransactionID,EmailStatusID,--StatusUpdatedDate, StatusUpdatedReason,
				Email,DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID,IsComp,SubscriptionStatusID,AccountNumber,AddRemoveID,Copies,
				GraceIssues,IMBSEQ,IsActive,IsPaid,IsSubscribed,MemberGroup,OnBehalfOf,OrigsSrc,Par3CID,SequenceID,Status,SubscriberSourceCode,SubSrcID,Verify,
				ExternalKeyID,FirstName,LastName,Company,Title,Occupation,AddressTypeID,Address1,Address2,Address3,City,RegionCode,RegionID,ZipCode,Plus4,
				CarrierRoute,County,Country,CountryID,Latitude,Longitude,IsAddressValidated,AddressValidationDate,AddressValidationSource,AddressValidationMessage,
				Phone,Fax,Mobile,Website,Birthdate,Age,Income,Gender,tmpSubscriptionID,IsLocked,LockedByUserID,LockDate,LockDateRelease,PhoneExt,
				IsInActiveWaveMailing,AddressTypeCodeId,AddressLastUpdatedDate,AddressUpdatedSourceTypeCodeId,WaveMailingID,IGrp_No,SFRecordIdentifier)                       
		OUTPUT Inserted.HistorySubscriptionID, Inserted.SubscriptionID, Inserted.PubSubscriptionID
		INTO #HistorySubscription           
				select ps.PubSubscriptionID,ps.SubscriptionID,ps.PubID,ps.Demo7,ps.QualificationDate,ps.PubQSourceID,ps.PubCategoryID,ps.PubTransactionID,ps.EmailStatusID,--StatusUpdatedDate,ps.StatusUpdatedReason,
				ps.Email,GETDATE(),ps.DateUpdated,1,ps.UpdatedByUserID,ps.IsComp,ps.SubscriptionStatusID,ps.AccountNumber,ps.AddRemoveID,ps.Copies,ps.GraceIssues,ps.IMBSEQ,ps.IsActive,ps.IsPaid,
				ps.IsSubscribed,ps.MemberGroup,ps.OnBehalfOf,ps.OrigsSrc,ps.Par3CID,ps.SequenceID,ps.Status,ps.SubscriberSourceCode,ps.SubSrcID,ps.Verify,ps.ExternalKeyID,ps.FirstName,ps.LastName,ps.Company,ps.Title,ps.Occupation,
				ps.AddressTypeID,ps.Address1,ps.Address2,ps.Address3,ps.City,ps.RegionCode,ps.RegionID,ps.ZipCode,ps.Plus4,ps.CarrierRoute,ps.County,ps.Country,ps.CountryID,ps.Latitude,ps.Longitude,ps.IsAddressValidated,
				ps.AddressValidationDate,ps.AddressValidationSource,ps.AddressValidationMessage,ps.Phone,ps.Fax,ps.Mobile,ps.Website,ps.Birthdate,ps.Age,ps.Income,ps.Gender,ps.tmpSubscriptionID,ps.IsLocked,ps.LockedByUserID,
				ps.LockDate,ps.LockDateRelease,ps.PhoneExt,ps.IsInActiveWaveMailing,ps.AddressTypeCodeId,ps.AddressLastUpdatedDate,ps.AddressUpdatedSourceTypeCodeId,ps.WaveMailingID,ps.IGrp_No,ps.SFRecordIdentifier       
			from 
				#tmpPS t join
				PubSubscriptions ps  with (NOLOCK) on t.PubSubscriptionID = ps.PubSubscriptionID 
			
		--print ('Insert into HistorySubscription COUNT : ' +  convert(varchar(100),@@ROWCOUNT) + ' / ' + convert(varchar(20), getdate(), 114))

		Insert Into History (BatchID,BatchCountItem, PublicationID, PubSubscriptionID, SubscriptionID,HistorySubscriptionID,DateCreated,CreatedByUserID)
				OUTPUT Inserted.HistoryID, Inserted.PubSubscriptionID
				INTO #History
			select pb.BatchID, (row_number() over (order by t.PubSubscriptionID)), t.PubID, t.PubSubscriptionID, t.SubscriptionID, hs.HistorySubscriptionID,GETDATE(),1
			from 
				#tmpPS t 
				join #PubBatch pb on t.PubID = pb.PubID
				join HistorySubscription hs with(nolock) on t.SubscriptionID = hs.SubscriptionID 
			where 
				hs.HistorySubscriptionID in (Select HistorySubscriptionID from #HistorySubscription)
					 
			--print ('Insert into History COUNT : ' +  convert(varchar(100),@@ROWCOUNT) + ' / ' + convert(varchar(20), getdate(), 114))

			Update hrm
			set HistorySubscriptionID = hs.HistorySubscriptionID
			from HistoryResponseMap hrm
			join #HistorySubscription hs on hrm.PubSubscriptionID = hs.PubSubscriptionID
			where 
				isnull(hrm.HistorySubscriptionID,0) = 0 or hrm.HistorySubscriptionID = -1 

		drop table #PubBatch 
		drop table #HistoryResponseMap 
		drop table #HistorySubscription 
		drop table #History

		--print ('end history: ' + convert(varchar(100), getdate(), 109))
	end

	drop table #tmpPS

End

