CREATE PROCEDURE [dbo].[sp_RecordUpdate]
	@PubSubs text,	
	@Changes text,
	@IssueID int,
	@ProductID int,
	@UserID int
AS

	SET NOCOUNT ON

/* TESTING */
--DECLARE @PubSubs varchar(max) = '<XML><ID>24649836</ID><ID>24649837</ID><ID>24649838</ID><ID>24649802</ID></XML>'
--DECLARE @Changes varchar(max) = '<RecordUpdate>
--									<FieldName>
--									<Table>PubSubscriptions</Table>
--									<Column>County</Column>
--									<Values>
--										<Detail>
--											<Value>Stearns</Value>
--											<UseQDateForDemo>false</UseQDateForDemo>
--										</Detail>
--									</Values>
--									<OtherText />
--									<ResponseGroupID>0</ResponseGroupID>
--									</FieldName>
--									<FieldName>
--									<Table>PubSubscriptions</Table>
--									<Column>regioncode</Column>
--									<Values>										
--										<Detail>
--											<Value>CA</Value>
--											<UseQDateForDemo>false</UseQDateForDemo>
--										</Detail>
--									</Values>
--									<OtherText />
--									<ResponseGroupID>0</ResponseGroupID>
--								</FieldName>
--								<FieldName>
--									<Table>PubSubscriptions</Table>
--									<Column>Country</Column>
--									<Values>										
--										<Detail>
--											<Value>United States</Value>
--											<UseQDateForDemo>false</UseQDateForDemo>
--										</Detail>
--									</Values>
--									<OtherText />
--									<ResponseGroupID>0</ResponseGroupID>
--								</FieldName>
--								<FieldName>
--									<Table>PubSubscriptionsExtension</Table>
--									<Column>histbatch1</Column>
--									<Values>										
--										<Detail>
--											<Value>1</Value>
--											<UseQDateForDemo>false</UseQDateForDemo>
--										</Detail>
--									</Values>
--									<OtherText />
--									<ResponseGroupID>0</ResponseGroupID>
--								</FieldName>
--								<FieldName>
--									<Table>PubSubscriptionDetail</Table>
--									<Column>BUSINESS</Column>
--									<Values>										
--										<Detail>
--											<Value>1</Value>
--											<UseQDateForDemo>true</UseQDateForDemo>
--										</Detail>
--									</Values>
--									<OtherText />
--									<ResponseGroupID>1</ResponseGroupID>
--								</FieldName>
--								<FieldName>
--									<Table>PubSubscriptionDetail</Table>
--									<Column>ASSOCIATIONS</Column>
--									<Values>																				
--										<Detail>
--											<Value>13</Value>
--											<UseQDateForDemo>true</UseQDateForDemo>
--										</Detail>
--										<Detail>
--											<Value>14</Value>
--											<UseQDateForDemo>false</UseQDateForDemo>
--										</Detail>
--									</Values>
--									<OtherText />
--									<ResponseGroupID>3</ResponseGroupID>
--								</FieldName>                                                               
--							</RecordUpdate>'
--DECLARE @IssueID int = 0 --ZERO indicates current (PubSub, PubSubDetail), anything greater than zero indicates issue archive
--DECLARE @ProductID int = 1
--DECLARE @UserID int = 1
/* END TESTING */

	CREATE TABLE #PS
	(  
		[PubSubscriptionID] int Primary Key,
		[SubscriptionID] int null,
		[QDate] datetime null,
		[PubID] int null,
		[IsCirc] bit null
	)

	CREATE TABLE #Changes
	(  
		RowID int IDENTITY(1, 1),
		[Table] varchar(100),
		[Column] varchar(200),
		[Value] varchar(max),
		OtherText varchar(max),
		ResponseGroupID int,
		UseQDateForDemo bit
	)

	DECLARE @docHandle int

	EXEC sp_xml_preparedocument @docHandle OUTPUT, @PubSubs  

	INSERT INTO #PS (PubSubscriptionID)
	SELECT ID
	FROM OPENXML(@docHandle,N'/XML/ID')
	WITH
	(
			ID int '.'
	)
	EXEC sp_xml_removedocument @docHandle
	
       
	--CREATE NONCLUSTERED INDEX IDX_Changes_SubID ON #PS(SubID)
	DECLARE @docHandle2 int
	EXEC sp_xml_preparedocument @docHandle2 OUTPUT, @Changes  
	INSERT INTO #Changes 
	SELECT *
	FROM OPENXML(@docHandle2,N'/RecordUpdate/FieldName/Values/Detail')
	WITH
	(
			[Table] nvarchar(256) '../../Table',
			[Column] nvarchar(256) '../../Column',
			[Value] nvarchar(256) 'Value',
			[OtherText] varchar(256) '../../OtherText',
			[ResponseGroupID] int '../../ResponseGroupID',
			[UseQDateForDemo] bit 'UseQDateForDemo'
	)
	EXEC sp_xml_removedocument @docHandle2	

	BEGIN TRY
		BEGIN TRANSACTION;

		IF (@IssueID = 0)
			BEGIN
				Update t
				set SubscriptionID = ps.SubscriptionID,
					QDate = ps.QualificationDate,
					PubID = ps.PubID,
					IsCirc = p.isCirc
				from #PS t					
					join PubSubscriptions ps with(nolock) on t.PubSubscriptionID = ps.PubSubscriptionID
					join Pubs p with(nolock) on ps.PubID = p.PubID

				/* PubSubscriptions */
				declare @column varchar(256), 
						@value varchar(256),
						@execstring varchar(MAX) = ''

				DECLARE c_PS CURSOR FOR 
					select [Column], [Value]
					from #Changes
					where [Table] = 'PubSubscriptions'

					OPEN c_PS  
					FETCH NEXT FROM c_PS INTO @column, @value

					WHILE @@FETCH_STATUS = 0  
					BEGIN  
						if len(@execstring) =  0
								set @execstring += @column + ' = ''' + replace(@value,'''','''''') + ''' ' 
						else 
								set @execstring += ',' + @column + ' = ''' + replace(@value,'''','''''') + ''' ' 

						FETCH NEXT FROM c_PS INTO @column, @value
					END
					CLOSE c_PS  
					DEALLOCATE c_PS

					if (len(@execstring) > 0)
                    BEGIN
						set @execstring = 'update PS set ' + @execstring + ' from PubSubscriptions PS join #PS t on t.pubsubscriptionID = ps.pubsubscriptionID '
					
						exec (@execstring)
					END

					if exists (select top 1 1 from #Changes c where [Table] = 'PubSubscriptions' and [Column] = 'PubTransactionID')
					BEGIN
						DECLARE @IAFree int = (SELECT SubscriptionStatusID FROM UAD_Lookup..SubscriptionStatus WHERE StatusCode = 'IAFree')
						DECLARE @IAPAid int = (SELECT SubscriptionStatusID FROM UAD_Lookup..SubscriptionStatus WHERE StatusCode = 'IAPaid')
						DECLARE @AFree int = (SELECT SubscriptionStatusID FROM UAD_Lookup..SubscriptionStatus WHERE StatusCode = 'AFree')
						DECLARE @APAid int = (SELECT SubscriptionStatusID FROM UAD_Lookup..SubscriptionStatus WHERE StatusCode = 'APaid')
						DECLARE @APros int = (SELECT SubscriptionStatusID FROM UAD_Lookup..SubscriptionStatus WHERE StatusCode = 'AProsp')
						DECLARE @IAPros int = (SELECT SubscriptionStatusID FROM UAD_Lookup..SubscriptionStatus WHERE StatusCode = 'IAProsp')

						Update ps
						set SubscriptionStatusID =  
							CASE WHEN cc.CategoryCodeTypeID in (1,2) AND tc.TransactionCodeTypeID = 1 AND cc.CategoryCodeValue not in (70,71) THEN @AFree 
							WHEN cc.CategoryCodeTypeID in (3,4) AND tc.TransactionCodetypeID = 3 AND cc.CategoryCodeValue not in (70,71) THEN @APAid
							WHEN tc.TransactionCodetypeID = 2 AND cc.CategoryCodeValue not in (70,71) THEN @IAFree
							WHEN tc.TransactionCodeTypeID = 4 AND cc.CategoryCodeValue not in (70,71) THEN @IAPAid
							WHEN cc.CategoryCodeTypeID in (1,2) AND tc.TransactionCodeTypeID in (1) AND cc.CategoryCodeValue in (70,71) THEN @APros  
							WHEN tc.TransactionCodetypeID in (2,4) AND cc.CategoryCodeValue in (70,71) THEN @IAPros ELSE NULL END,
						IsSubscribed = CASE WHEN tc.TransactionCodeTypeID in (1,3) THEN 1 ELSE 0 END,
						IsPaid = CASE WHEN tc.TransactionCodeTypeID in (3,4) THEN 1 ELSE 0 END
						from #PS t
						join PubSubscriptions ps on t.PubSubscriptionID = ps.PubSubscriptionID
						left outer join UAD_LookUp..CategoryCode cc WITH (nolock) ON cc.CategoryCodeID = ps.PubCategoryID
						left outer join UAD_LookUp..TransactionCode tc WITH (nolock) ON tc.TransactionCodeID = ps.PubTransactionID
					END


				/* Delete Consensus */
				delete sd
				from #PS t
					join SubscriptionDetails sd on t.SubscriptionID = sd.SubscriptionID 
					join CodeSheet_Mastercodesheet_Bridge cmb  WITH (nolock) on sd.MasterID = cmb.MasterID 					           

				delete smv
				from #PS t
					join SubscriberMasterValues smv on t.SubscriptionID = smv.SubscriptionID            


				/* PubSubscriptionDetail */
				delete psd 
				from #PS t
					join PubSubscriptionDetail psd on t.pubsubscriptionID = psd.pubsubscriptionID
					join CodeSheet cs with(nolock) on psd.CodesheetID = cs.CodeSheetID
					join ResponseGroups rg with(nolock) on cs.ResponseGroupID = rg.ResponseGroupID					
					join #Changes c on c.ResponseGroupID = rg.ResponseGroupID
					where c.ResponseGroupID <> 0		

				insert into PubSubscriptionDetail (PubSubscriptionID,SubscriptionID,CodesheetID,DateCreated,CreatedByUserID)
				Select t.PubSubscriptionID, ps.SubscriptionID, c.[Value],
				CASE WHEN c.UseQDateForDemo = 1 THEN t.QDate ELSE GETDATE() END,1
				from #PS t
					join PubSubscriptions ps with(nolock) on t.PubSubscriptionID = ps.PubSubscriptionID
					cross join #Changes c
					where c.[Table] = 'PubSubscriptionDetail'

				/* Rebuild Consensus */
				insert into SubscriptionDetails (SubscriptionID, MasterID)
				select distinct psd.SubscriptionID, cmb.masterID 
					from #PS t
					join PubSubscriptionDetail psd on psd.PubSubscriptionID = t.PubSubscriptionID
					join CodeSheet_Mastercodesheet_Bridge cmb with (NOLOCK) on psd.CodesheetID = cmb.CodeSheetID 
					left outer join SubscriptionDetails sd WITH (nolock) on sd.SubscriptionID = psd.SubscriptionID and sd.MasterID = cmb.MasterID
					where sd.sdID is null


				insert into SubscriberMasterValues (MasterGroupID, SubscriptionID, MastercodesheetValues)
				SELECT 
					MasterGroupID, [SubscriptionID] , 
					STUFF((
					SELECT ',' + CAST([MasterValue] AS VARCHAR(MAX)) 
					FROM #PS t1
					join [dbo].[SubscriptionDetails] sd1  with (NOLOCK) on t1.SubscriptionID = sd1.SubscriptionID
					join Mastercodesheet mc1  with (NOLOCK) on sd1.MasterID = mc1.MasterID  					
					WHERE (sd1.SubscriptionID = Results.SubscriptionID and mc1.MasterGroupID = Results.MasterGroupID) 
					FOR XML PATH (''))
					,1,1,'') AS CombinedValues
				FROM 
					(
						SELECT distinct sd.SubscriptionID, mc.MasterGroupID
						FROM #PS t
						join SubscriptionDetails sd  with (NOLOCK) on t.SubscriptionID = sd.SubscriptionID 
						join Mastercodesheet mc  with (NOLOCK)  on sd.MasterID = mc.MasterID						                       
					)
					Results
				GROUP BY [SubscriptionID] , MasterGroupID
				order by SubscriptionID   


				/* PubSubscriptionsExtension */
				declare @psemcolumn varchar(256), 
						@psemvalue varchar(256),
						@execupdatepsemstring varchar(MAX) = '',
						@execinsertpsemstring varchar(MAX) = ''

				DECLARE c_PSEM CURSOR FOR 
					select [Column], [Value]
					from #Changes
					where [Table] = 'PubSubscriptionsExtension'

					OPEN c_PSEM  
					FETCH NEXT FROM c_PSEM INTO @psemcolumn, @psemvalue

					WHILE @@FETCH_STATUS = 0  
					BEGIN  
				
						declare @StandardField varchar(50)
						set @StandardField = (Select StandardField from PubSubscriptionsExtensionMapper where CustomField = @psemcolumn and PubID = @ProductID)

						set @execupdatepsemstring = 'Update pse
											set ' + @StandardField + ' = ''' + replace(@psemvalue,'''','''''') + ''' ,
												DateUpdated = GETDATE(),
												UpdatedByUserID = ' + cast(@UserID as varchar(10)) + '
											from #PS t
											left outer join PubSubscriptionsExtension pse on t.PubSubscriptionID = pse.PubSubscriptionID
											where pse.PubSubscriptionID is not null'

						exec(@execupdatepsemstring)

						set @execinsertpsemstring = 'Insert into PubSubscriptionsExtension (PubSubscriptionID, ' + @StandardField + ', DateCreated, CreatedByUserID) 
											Select t.PubSubscriptionID, ''' + replace(@psemvalue,'''','''''') + ''', GETDATE(), ' + cast(@UserID as varchar(10)) + ' 
											from #PS t
											left outer join PubSubscriptionsExtension pse on t.PubSubscriptionID = pse.PubSubscriptionID
											where pse.PubSubscriptionID is null'
			
						exec(@execinsertpsemstring)

						FETCH NEXT FROM c_PSEM INTO @psemcolumn, @psemvalue
					END
					CLOSE c_PSEM  
					DEALLOCATE c_PSEM

				--FOR CIRC PRODUCTS Create BATCH, UPDTE HISTORY, HISTORYSUBSCRIPTION, HISTORYRESPONSE
				--This is only for CIRC PRODUCTS.
		
				if exists (select top 1 1 from #PS t where isnull(t.IsCirc,0) = 1)
				Begin
					
					Create table #PubBatch (PubID int, BatchID int)
					Create table #HistoryResponseMap (HistoryResponseMapID int, PubSubscriptionID int)
					Create table #HistorySubscription (HistorySubscriptionID int, SubscriptionID int, PubSubscriptionID int)
					Create table #History  (HistoryID int, PubSubscriptionID int)

					CREATE INDEX idx_PubBatch_BatchID ON #PubBatch(BatchID)
					CREATE INDEX idx_HistoryResponseMap_HistoryResponseMapID ON #HistoryResponseMap(HistoryResponseMapID)
					CREATE INDEX idx_HistoryRespinseMap_PubSubscriptionID ON #HistoryResponseMap(PubSubscriptionID)
					CREATE INDEX idx_HistorySubscription_HistorySubscriptionID ON #HistorySubscription(HistorySubscriptionID)
					CREATE INDEX idx_HistorySubscription_SubscriptionID ON #HistorySubscription(SubscriptionID)
					CREATE INDEX idx_HistorySubscription_PubSubscriptionID ON #HistorySubscription(PubSubscriptionID)
					CREATE INDEX idx_History_HistoryID ON #History(HistoryID)
					CREATE INDEX idx_History_PubSubscriptionID ON #History(PubSubscriptionID)

						
					insert into Batch (PublicationID, UserID, BatchCount,IsActive, DateCreated, DateFinalized, BatchNumber)
					OUTPUT Inserted.PublicationID, Inserted.BatchID
					INTO #PubBatch
					select t.PubID, 1, COUNT(t.PubSubscriptionID), 1, GETDATE() , GETDATE() , (select isnull(MAX(batchnumber),0) + 1 from Batch b with (NOLOCK) where b.PublicationID = t.PubID)
					from 
							#PS t 
					where isnull(t.IsCirc,0) = 1
					group by t.pubID
			
								
					--PubSubscriptionDetail History
					Insert into HistoryResponseMap (PubSubscriptionDetailID,PubSubscriptionID,SubscriptionID,CodeSheetID,IsActive,DateCreated,CreatedByUserID,ResponseOther, HistorySubscriptionID)
					OUTPUT Inserted.HistoryResponseMapID, Inserted.PubSubscriptionID
					INTO #HistoryResponseMap
					Select PubSubscriptionDetailID, t.PubSubscriptionID, t.SubscriptionID, CodeSheetID, 'true', GETDATE(), 1, ResponseOther, -1 
					from 
							#PS t
							join PubSubscriptionDetail psd with (NOLOCK) on t.PubSubscriptionID = psd.PubSubscriptionID 
					where isnull(t.IsCirc,0) = 1 and psd.CodesheetID is not NULL
					
								
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
							#PS t
							join PubSubscriptions ps with (NOLOCK) on ps.PubSubscriptionID = t.PubSubscriptionID 
					where isnull(t.IsCirc,0) = 1 
			

					Insert Into History (BatchID,BatchCountItem, PublicationID, PubSubscriptionID, SubscriptionID,HistorySubscriptionID,DateCreated,CreatedByUserID)
							OUTPUT Inserted.HistoryID, Inserted.PubSubscriptionID
							INTO #History
						select pb.BatchID, (row_number() over (partition by t.PubSubscriptionID order by t.PubSubscriptionID)), t.PubID, t.PubSubscriptionID, t.SubscriptionID, hs.HistorySubscriptionID,GETDATE(),1
						from 
							#PS t
							join #PubBatch pb on t.PubID = pb.PubID
							join HistorySubscription hs with(nolock) on t.SubscriptionID = hs.SubscriptionID 
						where 
							hs.HistorySubscriptionID in (Select HistorySubscriptionID from #HistorySubscription)								
					 					

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

				End
				-- END - Batch update *******************************************

			END
		ELSE
			BEGIN
				Update t
				set SubscriptionID = iaps.SubscriptionID,
					QDate = iaps.QualificationDate,
					PubID = iaps.PubID
				from #PS t
					join IssueArchiveProductSubscription iaps with(nolock) on t.PubSubscriptionID = iaps.PubSubscriptionID
					where iaps.IssueID = @IssueID

				/* IssueArchiveProductSubscription */
				declare @archivecolumn varchar(256), 
						@archivevalue varchar(256),
						@archiveexecstring varchar(MAX) = ''

				DECLARE c_PS CURSOR FOR 
					select [Column], [Value]
					from #Changes
					where [Table] = 'IssueArchiveProductSubscription'

					OPEN c_PS  
					FETCH NEXT FROM c_PS INTO @archivecolumn, @archivevalue

					WHILE @@FETCH_STATUS = 0  
					BEGIN  
						if len(@archiveexecstring) =  0
								set @archiveexecstring += @archivecolumn + ' = ''' + replace(@archivevalue,'''','''''') + ''' ' 
						else 
								set @archiveexecstring += ',' + @archivecolumn + ' = ''' + replace(@archivevalue,'''','''''') + ''' ' 

						FETCH NEXT FROM c_PS INTO @archivecolumn, @archivevalue
					END
					CLOSE c_PS  
					DEALLOCATE c_PS

					if (len(@archiveexecstring) > 0)
                    BEGIN
						set @archiveexecstring = 'update IAPS set ' + @archiveexecstring + ' from IssueArchiveProductSubscription IAPS join #PS t on t.pubsubscriptionID = IAPS.pubsubscriptionID where IAPS.IssueID = ' + cast(@IssueID as varchar(10))

						exec (@archiveexecstring)
					END

				/* IssueArchiveProductSubscriptionDetail */
				delete iapsd from #PS t 
					join IssueArchiveProductSubscriptionDetail iapsd on t.pubsubscriptionID = iapsd.pubsubscriptionID
					join IssueArchiveProductSubscription iaps on iapsd.PubSubscriptionID = iaps.PubSubscriptionID
					join CodeSheet cs with(nolock) on iapsd.CodesheetID = cs.CodeSheetID
					join ResponseGroups rg with(nolock) on cs.ResponseGroupID = rg.ResponseGroupID					
					join #Changes c on c.ResponseGroupID = rg.ResponseGroupID
					where c.ResponseGroupID <> 0 and iaps.IssueID = @IssueID	

				insert into IssueArchiveProductSubscriptionDetail (IssueArchiveSubscriptionId,PubSubscriptionID,SubscriptionID,CodesheetID,DateCreated,CreatedByUserID)
				Select iaps.IssueArchiveSubscriptionId, t.PubSubscriptionID, iaps.SubscriptionID, c.[Value],
				CASE WHEN c.UseQDateForDemo = 1 THEN t.QDate ELSE GETDATE() END, @UserID
				from #PS t
					join IssueArchiveProductSubscription iaps with(nolock) on t.PubSubscriptionID = iaps.PubSubscriptionID
					cross join #Changes c
					where c.[Table] = 'IssueArchiveProductSubscriptionDetail' and iaps.IssueID = @IssueID

				/* IssueArchivePubSubscriptionsExtension */
				declare @apsemcolumn varchar(256), 
						@apsemvalue varchar(256),
						@execupdateapsemstring varchar(MAX) = '',
						@execinsertapsemstring varchar(MAX) = ''

				DECLARE c_PSEM CURSOR FOR 
					select [Column], [Value]
					from #Changes
					where [Table] = 'IssueArchivePubSubscriptionsExtension'

					OPEN c_PSEM  
					FETCH NEXT FROM c_PSEM INTO @apsemcolumn, @apsemvalue

					WHILE @@FETCH_STATUS = 0  
					BEGIN  
				
						declare @ArchiveStandardField varchar(50)
						set @ArchiveStandardField = (Select StandardField from PubSubscriptionsExtensionMapper where CustomField = @apsemcolumn and PubID = @ProductID)

						set @execupdateapsemstring = 'Update iapse
											set ' + @ArchiveStandardField + ' = ''' + replace(@apsemvalue,'''','''''') + ''' ,
												DateUpdated = GETDATE(),
												UpdatedByUserID = ' + cast(@UserID as varchar(10)) + '
											from #PS t
											join IssueArchiveProductSubscription iaps on t.PubSubscriptionID = iaps.PubSubscriptionID
											left outer join IssueArchivePubSubscriptionsExtension iapse on iaps.IssueArchiveSubscriptionID = iapse.IssueArchiveSubscriptionID
											where iaps.IssueID = ' + cast(@IssueID as varchar(10)) + ' and iapse.IssueArchiveSubscriptionID is not null'

						exec(@execupdateapsemstring)

						set @execinsertapsemstring = 'Insert into IssueArchivePubSubscriptionsExtension (IssueArchiveSubscriptionID, ' + @ArchiveStandardField + ', DateCreated, CreatedByUserID) 
											Select iaps.IssueArchiveSubscriptionID, ''' + replace(@apsemvalue,'''','''''') + ''', GETDATE(), ' + cast(@UserID as varchar(10)) + ' 
											from #PS t
											join IssueArchiveProductSubscription iaps on t.PubSubscriptionID = iaps.PubSubscriptionID
											left outer join IssueArchivePubSubscriptionsExtension iapse on iaps.IssueArchiveSubscriptionID = iapse.IssueArchiveSubscriptionID
											where iaps.IssueID = ' + cast(@IssueID as varchar(10)) + ' and iapse.IssueArchiveSubscriptionID is null'
			
						exec(@execinsertapsemstring)

						FETCH NEXT FROM c_PSEM INTO @apsemcolumn, @apsemvalue
					END
					CLOSE c_PSEM  
					DEALLOCATE c_PSEM
			END

		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
			
		ROLLBACK TRANSACTION;

		DECLARE @ErrorMessage NVARCHAR(4000);  
		DECLARE @ErrorSeverity INT;  
		DECLARE @ErrorState INT;  

		SET @ErrorMessage = ERROR_MESSAGE();  
		SET @ErrorSeverity = ERROR_SEVERITY();  
		SET @ErrorState = ERROR_STATE();  

		RAISERROR (@ErrorMessage, -- Message text.  
					  @ErrorSeverity, -- Severity.  
					  @ErrorState -- State.  
					  );  
		
	END CATCH;

	drop table #PS
	drop table #Changes