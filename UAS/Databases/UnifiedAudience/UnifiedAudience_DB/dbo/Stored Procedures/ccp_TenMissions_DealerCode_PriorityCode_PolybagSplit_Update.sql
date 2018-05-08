CREATE PROCEDURE [dbo].[ccp_TenMissions_DealerCode_PriorityCode_PolybagSplit_Update]   
(
	@ManualRun bit
)
as
BEGIN

	SET NOCOUNT ON

	declare @pubcode varchar(100) = 'FOB',
			@PubID int = 0,
			@DealerCodeFieldName varchar(100) = '',
			@dt date	
			
	set @dt = convert(date, dateadd(day, -1, getdate()))

	select @PubID = PubID from Pubs where pubcode = @pubcode
	
	Create table #tmpPS
	(
		PubSubscriptionID int, 
		Company varchar(100),
		ADDRESS1 Varchar(256),
		RegionID int,
		zipcode varchar(5),
		ExistingDealerCode int,
		newDealerCode int
		PRIMARY KEY (PubSubscriptionID)
	)

	select @DealerCodeFieldName = StandardField from PubSubscriptionsExtensionMapper where CustomField = 'DealerCode' and PubID = @PubID

	DECLARE @syncstatus int = 0;
	EXEC @syncstatus = [10.10.41.198].SSIS_Config.dbo.Job_Is_Sync_ECN_UAD_ON @DBName = 'TenMissionsmasterDB', @PubCode='FOB'

	if ( @syncstatus = 1 or @ManualRun = 1)
	Begin
		If (@PubID > 0 and LEN(@DealerCodeFieldName) > 0)	
		Begin
		
			-- Replace Multiple space to 1 space.
			update ps
			set 
				ADDRESS1 = 	 master.dbo.fnReplaceMultipleSpaces(ADDRESS1)
			From 
				PubSubscriptions ps with (NOLOCK) 
				join UAD_LOOKUP..categorycode cc on cc.categorycodeID = ps.PubCategoryID and cc.categorycodevalue in (10,70) 
				join UAD_LOOKUP..transactioncode tc on tc.transactioncodeID = ps.PubTransactionID and tc.transactioncodetypeID in (1,3) 
			WHERE
				PubID = @pubID and
				Convert(date, Isnull(ps.DateUpdated, ps.DateCreated)) >  @dt and
				ADDRESS1 <> master.dbo.fnReplaceMultipleSpaces(ADDRESS1)

			Insert into #tmpPS (PubSubscriptionID, Company, ADDRESS1, RegionID, zipcode, ExistingDealerCode)
			SELECT 
				ps.pubsubscriptionID,
				Company,
				substring(ISNULL(ADDRESS1,''),1,15) ADDRESS1
				,RegionID RegionID
				,LEFT(ZipCode,5) ZipCode
				,Field3 as ExstingDealerCode
			FROM 
				PubSubscriptions ps with (NOLOCK) 
				join UAD_LOOKUP..categorycode cc on cc.categorycodeID = ps.PubCategoryID and cc.categorycodevalue in (10,70) 
				join UAD_LOOKUP..transactioncode tc on tc.transactioncodeID = ps.PubTransactionID and tc.transactioncodetypeID in (1,3) 
				left outer join	PubSubscriptionsExtension pse with (NOLOCK) on ps.pubsubscriptionID = pse.pubsubscriptionID
			WHERE
				PubID = @pubID and
				Convert(date, Isnull(ps.DateUpdated, ps.DateCreated)) >  @dt and
				len(company) > 0 and len(ADDRESS1) > 0 and isnull(RegionID, 0) > 0 and len(zipcode) > 0 
			
			Update x
			set newDealerCode = pse.Field3
			from
				#tmpPS x  
				join Pubsubscriptions ps with (NOLOCK) on
					ps.pubsubscriptionID <> x.PubSubscriptionID and
						ps.company = x.Company and
						substring(ps.ADDRESS1,1,15)= x.ADDRESS1 and
						LEFT(ps.ZipCode,5)  = x.ZipCode and
						ps.RegionID = x.RegionID
				join UAD_LOOKUP..categorycode cc on cc.categorycodeID = ps.PubCategoryID and cc.categorycodevalue in (10,70) 
				join UAD_LOOKUP..transactioncode tc on tc.transactioncodeID = ps.PubTransactionID and tc.transactioncodetypeID in (1,3) 
				join PubSubscriptionsExtension pse with (NOLOCK) on ps.pubsubscriptionID = pse.pubsubscriptionID
			where
				PubID = @PubID and len(pse.field3) > 0

			--select * from #tmpPS
			--where isnull(existingdealercode, '') = '' and isnull(newdealercode, '') <> ''
			--order by existingdealercode 

			update pse
			set field3 = newDealerCode
			from PubSubscriptionsExtension pse join #tmpPS t on pse.pubsubscriptionID = t.pubsubscriptionID
			where isnull(pse.field3,'') = '' and isnull(t.existingdealercode, '') = '' and isnull(t.newdealercode, '') <> ''

			insert into PubSubscriptionsExtension (PubSubscriptionid, Field3)
			select t.pubsubscriptionID, newdealercode
			from #tmpPS t left outer join PubSubscriptionsExtension pse on pse.pubsubscriptionID = t.pubsubscriptionID
			where pse.pubsubscriptionID is null and isnull(t.existingdealercode, '') = '' and isnull(t.newdealercode, '') <> ''

			/* dealer code update completed */

			/*
			-- move data from pse to psd

			select ps.PubSubscriptionID, ps.subscriptionID, ps.Qualificationdate, x1.CodeSheetID as prID, x2.CodeSheetID as tcID
			into #tmp1
			from 
					PubSubscriptions ps with (NOLOCK)
					join PubSubscriptionsExtension pse with (NOLOCK) on ps.pubsubscriptionID = pse.pubsubscriptionID 
					left outer join (select codesheetID, responsevalue from CodeSheet where pubID = 63 and responsegroupID = 1280) x1 on x1.Responsevalue = pse.field1
					left outer join (select codesheetID, responsevalue from CodeSheet where pubID = 63 and responsegroupID = 1278) x2 on x2.Responsevalue = pse.field2
			where pubID = 63 and (isnull(field1,'') <> '' or isnull(field2,'') <> '' )


			insert into PubSubscriptionDetail
			select pubsubscriptionID, subscriptionID, prID, qualificationdate, null, null, null, null from #tmp1 where isnull(prID,'') <>''

			insert into PubSubscriptionDetail
			select pubsubscriptionID, subscriptionID, tcID, qualificationdate, null, null, null, null from #tmp1 where isnull(tcID,'') <>''

			drop table #tmp1

			*/

			set nocount on

			declare @rgSupplementsID int, 
					@rgPriorityCodeID int,
					@rgTotalCopiesID int,
					@rgAddBankID int,
					@RGFunctionID int,
					@RGareasID int,
					@dealercode int,
					@i int = 0

			declare @titlecoderank table (responsevalue varchar(10), codesheetID int, score decimal(10,2))

			insert into @titlecoderank (responsevalue, score ) values 
			('01',1),
			('02',2),
			('03',3),
			('04',5)

			declare @priority table (isDR bit, PriorityCode int, scoretotal int, TotalCopies int, drPriority int)

			insert into @priority values (0,1,11,4,1)
			insert into @priority values (0,2,6,3,1)
			insert into @priority values (0,3,9,3,1)
			insert into @priority values (0,4,8,3,1)
			insert into @priority values (0,5,10,4,2)
			insert into @priority values (0,6,3,2,1)
			insert into @priority values (0,7,6,2,1)
			insert into @priority values (0,8,4,2,1)
			insert into @priority values (0,9,5,3,2)
			insert into @priority values (0,10,8,3,3)
			insert into @priority values (0,11,7,3,2)
			insert into @priority values (0,12,1,1,1)
			insert into @priority values (0,13,2,2,2)
			insert into @priority values (0,14,3,2,3)
			insert into @priority values (0,15,5,2,4)
			insert into @priority values (1,1,11,4,1)
			insert into @priority values (1,16,11,4,2)
			insert into @priority values (1,17,11,4,3)
			insert into @priority values (1,18,11,4,4)
			insert into @priority values (1,19,10,4,3)
			insert into @priority values (1,20,10,4,4)
			insert into @priority values (1,21,6,3,2)
			insert into @priority values (1,22,6,3,3)
			insert into @priority values (1,23,9,3,3)
			insert into @priority values (1,24,9,3,4)
			insert into @priority values (1,25,8,3,2)
			insert into @priority values (1,26,8,3,4)
			insert into @priority values (1,27,3,2,2)
			insert into @priority values (1,28,4,2,3)
			insert into @priority values (1,29,6,2,4)
			insert into @priority values (1,5,10,4,2)

			insert into @priority values (1,9,5,3,2)
			insert into @priority values (1,10,8,3,4)
			insert into @priority values (1,11,7,2,2)
			insert into @priority values (1,30,5,3,2)
			insert into @priority values (1,31,8,3,4)
			insert into @priority values (1,32,7,3,2)

			insert into @priority values (1,12,1,1,1)
			insert into @priority values (1,13,2,2,2)
			insert into @priority values (1,14,3,2,3)
			insert into @priority values (1,15,5,2,4)

			select @rgSupplementsID = responsegroupID from responsegroups where pubID = @pubID and ResponseGroupName = 'SUPPLEMENTS'
			select @rgPriorityCodeID = responsegroupID from responsegroups where pubID = @pubID and ResponseGroupName = 'PRIORITYCODE'
			select @rgTotalCopiesID = responsegroupID from responsegroups where pubID = @pubID and ResponseGroupName = 'TOTALCOPIES'
			select @rgAddBankID = responsegroupID from responsegroups where pubID = @pubID and ResponseGroupName = 'ADDBANK'
			select @RGFunctionID = responsegroupID from responsegroups where PubID = @PubID and responsegroupname = 'FUNCTION' 
			select @RGareasID = responsegroupID from responsegroups where PubID = @PubID and responsegroupname = 'DEMO10'

			--select @rgPriorityCodeID,@rgSupplementsID,@rgTotalCopiesID,@rgAddBankID,@RGFunctionID,@RGareasID
	
			update @titlecoderank
			set codesheetID = c.codesheetID
			from @titlecoderank t join codesheet c on t.responsevalue = c.Responsevalue
			where (responsegroupID = @RGFunctionID or responsegroupID = @RGareasID)

			Create table #PS 
			(
				PubsubscriptionID int,
				SubscriptionID int,
				SequenceID int,
				FirstName varchar(50),
				LastName varchar(50),
				Company varchar(100),
				Address1 varchar(256),
				RegionID int,
				Zipcode varchar(50),
				CategoryCodeValue int,
				TransactionCodeValue int,
				QSource char(1),
				IsDr bit,
				isQualified bit,
				Qualificationdate date,
				lastchanged datetime,
				DealerCode int,
				FTCode int,
				score int,
				isHostCopy bit,
				Prioritycode int,
				supplements int,
				Totalcopies int,
				AddBank int,
				newPrioritycode int,
				newsupplements int,
				newTotalcopies int,
				newAddBank int
			)

			CREATE CLUSTERED INDEX IDX_PS_PubsubscriptionID ON #PS(PubsubscriptionID)
			CREATE INDEX IDX_PS_dealercode ON #PS(dealercode)

			Insert into #PS
			select ps.PubsubscriptionID, ps.SubscriptionID, ps.SequenceID, ps.FirstName, ps.LastName, ps.Company, ps.ADDRESS1, ps.RegionID, ps.zipcode, 
				cc.CategoryCodeValue, tc.TransactionCodeValue, 
				c.CodeValue as Qsource,  
				case when c.codevalue in ('A','B','Q') then 1 else 0 end as IsDR,
				case when cc.categorycodevalue in (10,70) and tc.transactioncodetypeID in (1,3) then 1 else 0 end as isQualified,
				ps.Qualificationdate, isnull(ps.dateupdated, ps.datecreated), isnull(pse.field3,'') dealercode, FunctionAreaCode, score,
				0, null as PRIORITYCODE, null as SUPPLEMENTS, null as TOTALCOPIES, null as ADDBANK,
				null as newPRIORITYCODE, null as newSUPPLEMENTS, null as newTOTALCOPIES, null as newADDBANK
			from 
	
					PubSubscriptions ps with (NOLOCK)
					join UAD_Lookup..CategoryCode cc with (NOLOCK)  on ps.PubCategoryID = cc.CategoryCodeID 
					join UAD_Lookup..TransactionCode tc with (NOLOCK)  on ps.PubTransactionID = tc.TransactionCodeID
					left outer join PubSubscriptionsExtension pse with (NOLOCK) on ps.pubsubscriptionID = pse.pubsubscriptionID 
					left outer join UAD_Lookup..Code c with (NOLOCK) on ps.PubQSourceID = c.codeID
					left outer join
					(
						select 
								ps.pubsubscriptionID, 
								min(t.responsevalue) FunctionAreaCode, 
								Min(t.score) score
						from 
								PubSubscriptions ps join 
								PubSubscriptionDetail psd on ps.pubsubscriptionID = psd.pubsubscriptionID join
								@titlecoderank t on t.CodeSheetID = psd.codesheetID
						where ps.pubID = @PubID
						group by ps.pubsubscriptionID, ps.Qualificationdate
					) x on ps.PubSubscriptionID = x.PubSubscriptionID
			where
				ps.pubID = @pubID  --and isnull(pse.field3,'') = 10006

			order by dealercode

			update t
			set t.prioritycode = c.responsevalue
			from 
					#PS t join 
					PubSubscriptionDetail psd on t.pubsubscriptionID = psd.pubsubscriptionID join
					codesheet c  with (NOLOCK) on psd.CodesheetID = c.CodeSheetID 
			where
				c.responsegroupID = @rgPriorityCodeID

			update t 
			set t.SUPPLEMENTS = c.responsevalue
			from 
					#PS t join 
					PubSubscriptionDetail psd on t.pubsubscriptionID = psd.pubsubscriptionID join
					codesheet c  with (NOLOCK) on psd.CodesheetID = c.CodeSheetID 
			where
				c.responsegroupID = @rgSupplementsID

			update t
			set t.TOTALCOPIES = c.responsevalue
			from 
					#PS t join 
					PubSubscriptionDetail psd on t.pubsubscriptionID = psd.pubsubscriptionID join
					codesheet c  with (NOLOCK) on psd.CodesheetID = c.CodeSheetID 
			where
				c.responsegroupID = @rgTotalCopiesID

			update t
			set t.ADDBANK = c.responsevalue
			from 
					#PS t join 
					PubSubscriptionDetail psd on t.pubsubscriptionID = psd.pubsubscriptionID join
					codesheet c  with (NOLOCK) on psd.CodesheetID = c.CodeSheetID 
			where
				c.responsegroupID = @rgAddBankID
	
			DECLARE c_Dealercode CURSOR 
			FOR select distinct dealercode from #PS

			OPEN c_Dealercode  
			FETCH NEXT FROM c_Dealercode INTO @dealercode

			WHILE @@FETCH_STATUS = 0  
			BEGIN  
				--print ('DR :' + convert(varchar,@i) + ' - ' +  convert(varchar(100),@dealercode) + ' - ' +  convert(varchar(100),getdate(), 109))

				if isnull(@dealerCode,0) > 0 
				Begin	
					if exists(select top 1 1 from #PS where dealercode = @dealercode and isDr = 1)
					Begin

						Update #PS 
						set isHostCopy = 1 
						where PubsubscriptionID = (select top 1 PubsubscriptionID from #PS where dealercode = @dealercode and isDR = 1 and isQualified=1 and isnull(score, 0) > 0 order  by Qualificationdate desc, lastchanged desc, pubsubscriptionID desc);

						--score asc, 
					End
					Else
					Begin
						Update #PS 
						set isHostCopy = 1 
						where PubsubscriptionID = (select top 1 PubsubscriptionID from #PS where dealercode = @dealercode  and isQualified=1  and isnull(score, 0) > 0 order  by score asc, CategoryCodeValue asc, Qualificationdate desc, lastchanged desc, pubsubscriptionID desc);
					End

					declare @TotalCopies int = 0, @scoretotal int = 0, @addbank int = 0, @isHostCopyFO bit = 0

					select	
							-- if hostcopy is not for "FO" and "FO" not exists in the dealer list then Add "1" to the total count - we need to send extra copy.
							@TotalCopies = count(distinct FTCode) + (case when max(case when FTCode = 1 then 1 else 0 end) = 0 then max(case when isHostCopy =1 and isnull(FTCode,0) > 1 then 1 else 0 end) else 0 end) , 
							@scoretotal = sum(distinct score), 
							@addbank = count(*) - count(distinct FTCode), 
							@isHostCopyFO =  max(case when isHostCopy =1 and isnull(FTCode,0) = 1 then 1 else 0 end)
					from #PS
					where dealercode = @dealercode  and isQualified=1
					group by dealercode

					if @isHostCopyFO = 1
					Begin
						update	ps
						set		ps.newPrioritycode = p.PriorityCode, 
								ps.newTotalcopies =  p.TotalCopies , 
								ps.newsupplements =  p.TotalCopies - 1, 
								ps.newAddbank = (case when @addbank > 25 then 25 else @addbank end)
						from 
								#PS ps join 
								@priority p on ps.FTCode = p.drPriority
						where ps.DealerCode = @dealercode and isHostCopy = 1 and p.isDR = 0 and p.TotalCopies = @TotalCopies and p.scoretotal = @scoretotal
					end
					else
					Begin
						update	ps
						set		ps.newPrioritycode = p.PriorityCode, 
								ps.newTotalcopies =  p.TotalCopies , 
								ps.newsupplements =  p.TotalCopies - 1, 
								ps.newAddbank = (case when @addbank > 25 then 25 else @addbank end)
						from 
								#PS ps join 
								@priority p  on ps.FTCode = p.drPriority
						where ps.DealerCode = @dealercode and isHostCopy = 1 and p.isDR = 1 and p.TotalCopies = @TotalCopies and p.scoretotal = @scoretotal
					end


					--if not exists 
					--(select top 1 1 from #PS ps join @tblcte c on ps.DealerCode = c.dealercode join @priority p on c.TotalCopies = p.TotalCopies - (case when ps.FTCode > 1 and c.HasFO = 0 then 1 else 0 end) and c.scoretotal = p.scoretotal and 
					--	(( ps.isDr=1 and ps.FTCode = 1 and p.IsDr = 0 and p.drPriority = 1) or (p.isdr = ps.IsDr and p.drPriority = ps.FTCode))
					--where isHostCopy = 1)
					--Begin
					--	select * from #PS where DealerCode = @dealercode
					--	select ps.DealerCode, ps.IsDr, ps.FTCode, c.TotalCopies, c.scoretotal  from #PS ps join @tblcte c on ps.DealerCode = c.dealercode where isHostCopy = 1
					--End

				End

				set @i = @i + 1

				FETCH NEXT FROM c_Dealercode INTO @dealercode

			END

			CLOSE c_Dealercode  
			DEALLOCATE c_Dealercode  

			--select * from #PS
			----where dealercode in (select DealerCode from #PS where DealerCode > 0 and isDR = 1) --ishostcopy = 0 and isnull(prioritycode,0) > 0
			--order by dealercode, isHostCopy desc

			--select * from #PS
			--where ishostcopy = 1 and isnull(newPriorityCode,0) = 0
			--order by dealercode, isHostCopy desc

			--select * from #PS
			--where ishostcopy = 0 and isnull(PriorityCode,0) > 0 and CategoryCodeValue <> 55
			--order by dealercode, isHostCopy desc


			/* Update Live */

			--Delete codesheet details for the records changed.

			delete psd
			from PubSubscriptionDetail psd join #PS ps on psd.PubSubscriptionID = ps.pubsubscriptionID
			where ishostcopy = 0 and isnull(PriorityCode,0) > 0 and CategoryCodeValue <> 55
			and psd.codesheetID in (select c.codesheetID from CodeSheet c where (responsegroupID =  @rgSupplementsID or responsegroupID =  @rgPriorityCodeID or responsegroupID =  @rgTotalCopiesID or responsegroupID =  @rgAddBankID))

			delete psd
			from PubSubscriptionDetail psd join #PS ps on psd.PubSubscriptionID = ps.pubsubscriptionID
			where   ishostcopy = 1 and CategoryCodeValue <> 55 and
					(
						isnull(Prioritycode,0) <> isnull(newPrioritycode,0) or
						isnull(Supplements,0) <> isnull(newSupplements,0) or
						isnull(TotalCopies,0) <> isnull(newTotalCopies,0) or
						isnull(AddBank,0) <> isnull(newAddBank,0) 
					)
					and psd.codesheetID in (select c.codesheetID from CodeSheet c where  (responsegroupID =  @rgSupplementsID or responsegroupID =  @rgPriorityCodeID or responsegroupID =  @rgTotalCopiesID or responsegroupID =  @rgAddBankID))

			/* insert updated/new records */
			--select  newPrioritycode, count(*)
			--from  #PS ps 
			--where   ishostcopy = 1 and CategoryCodeValue <> 55 and
			--		(
			--			isnull(Prioritycode,0) <> isnull(newPrioritycode,0) or
			--			isnull(Supplements,0) <> isnull(newSupplements,0) or
			--			isnull(TotalCopies,0) <> isnull(newTotalCopies,0) or
			--			isnull(AddBank,0) <> isnull(newAddBank,0) 
			--		)
			--group by newPrioritycode order by 1

			--select newSupplements, count(*)
			--from  #PS ps 
			--where   ishostcopy = 1 and CategoryCodeValue <> 55 and
			--		(
			--			isnull(Prioritycode,0) <> isnull(newPrioritycode,0) or
			--			isnull(Supplements,0) <> isnull(newSupplements,0) or
			--			isnull(TotalCopies,0) <> isnull(newTotalCopies,0) or
			--			isnull(AddBank,0) <> isnull(newAddBank,0) 
			--		)
			--group by newSupplements order by 1

			--select newTotalCopies, count(*)
			--from  #PS ps 
			--where   ishostcopy = 1 and CategoryCodeValue <> 55 and
			--		(
			--			isnull(Prioritycode,0) <> isnull(newPrioritycode,0) or
			--			isnull(Supplements,0) <> isnull(newSupplements,0) or
			--			isnull(TotalCopies,0) <> isnull(newTotalCopies,0) or
			--			isnull(AddBank,0) <> isnull(newAddBank,0) 
			--		)
			--group by newTotalCopies order by 1

			--select newAddBank, count(*)
			--from  #PS ps 
			--where   ishostcopy = 1 and CategoryCodeValue <> 55 and
			--		(
			--			isnull(Prioritycode,0) <> isnull(newPrioritycode,0) or
			--			isnull(Supplements,0) <> isnull(newSupplements,0) or
			--			isnull(TotalCopies,0) <> isnull(newTotalCopies,0) or
			--			isnull(AddBank,0) <> isnull(newAddBank,0) 
			--		)
			--group by newAddBank order by 1

			Insert into pubsubscriptionDetail (PubSubscriptionID,SubscriptionID,CodesheetID,DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID,ResponseOther)
			select ps.PubsubscriptionID, ps.subscriptionID, c.codesheetID, ps.Qualificationdate, null, null, null, null
			from #PS ps join Codesheet c on ps.newPrioritycode = c.Responsevalue and c.ResponseGroupID = @rgPriorityCodeID
			where   ishostcopy = 1 and CategoryCodeValue <> 55 and
					( isnull(Prioritycode,0) <> isnull(newPrioritycode,0) or isnull(Supplements,0) <> isnull(newSupplements,0) or isnull(TotalCopies,0) <> isnull(newTotalCopies,0) or isnull(AddBank,0) <> isnull(newAddBank,0)  )
			union
			select ps.PubsubscriptionID, ps.subscriptionID, c.codesheetID, ps.Qualificationdate, null, null, null, null
			from #PS ps join Codesheet c on ps.newSupplements = c.Responsevalue and c.ResponseGroupID = @rgSupplementsID
			where   ishostcopy = 1 and CategoryCodeValue <> 55 and
					( isnull(Prioritycode,0) <> isnull(newPrioritycode,0) or isnull(Supplements,0) <> isnull(newSupplements,0) or isnull(TotalCopies,0) <> isnull(newTotalCopies,0) or isnull(AddBank,0) <> isnull(newAddBank,0)  )
			union
			select ps.PubsubscriptionID, ps.subscriptionID, c.codesheetID, ps.Qualificationdate, null, null, null, null
			from #PS ps join Codesheet c on ps.newTotalCopies = c.Responsevalue and c.ResponseGroupID = @rgTotalCopiesID
			where   ishostcopy = 1 and CategoryCodeValue <> 55 and
					( isnull(Prioritycode,0) <> isnull(newPrioritycode,0) or isnull(Supplements,0) <> isnull(newSupplements,0) or isnull(TotalCopies,0) <> isnull(newTotalCopies,0) or isnull(AddBank,0) <> isnull(newAddBank,0)  )
			union
			select ps.PubsubscriptionID, ps.subscriptionID, c.codesheetID, ps.Qualificationdate, null, null, null, null
			from #PS ps join Codesheet c on ps.newAddBank = c.Responsevalue and c.ResponseGroupID = @rgAddBankID
			where   ishostcopy = 1 and CategoryCodeValue <> 55 and
					( isnull(Prioritycode,0) <> isnull(newPrioritycode,0) or isnull(Supplements,0) <> isnull(newSupplements,0) or isnull(TotalCopies,0) <> isnull(newTotalCopies,0) or isnull(AddBank,0) <> isnull(newAddBank,0)  )
			order by ps.pubSubscriptionID

			drop table #PS

		END
	End
	Else
	Begin
		Select ' Web sync is off'
	End	
	drop table #tmpPS
END

go


/* --mulitple dealercode for same company location report 
	
SET NOCOUNT ON
declare @dt date
set @dt = '1/1/2016'  -- convert(date, dateadd(day, -1, getdate()))

declare @pubcode varchar(100) = 'FOB'

declare @PubID int = 0,
	@DealerCodeFieldName varchar(100) = ''

select @PubID = PubID from Pubs where pubcode = @pubcode
	
Create table #tmpPS
(
	PubSubscriptionID int, 
	Company varchar(100),
	ADDRESS1 Varchar(256),
	RegionID int,
	zipcode varchar(5),
	ExistingDealerCode int,
	newDealerCode int
	PRIMARY KEY (PubSubscriptionID)
)

select @DealerCodeFieldName = StandardField from PubSubscriptionsExtensionMapper where CustomField = 'DealerCode' and PubID = @PubID


Insert into #tmpPS (PubSubscriptionID, Company, ADDRESS1, RegionID, zipcode, ExistingDealerCode)
SELECT 
	ps.pubsubscriptionID,
	Company,
	substring(ISNULL(ADDRESS1,''),1,15) ADDRESS1
	,RegionID RegionID
	,LEFT(ZipCode,5) ZipCode
	,Field3 as ExstingDealerCode
FROM 
	PubSubscriptions ps with (NOLOCK) left outer join
	PubSubscriptionsExtension pse with (NOLOCK) on ps.pubsubscriptionID = pse.pubsubscriptionID
WHERE
	PubID = @pubID and
	len(company) > 0 and len(ADDRESS1) > 0 and isnull(RegionID, 0) > 0 and len(zipcode) > 0 
			

--Update x
--set newDealerCode = pse.Field3
--from
--	#tmpPS x  
--	join Pubsubscriptions ps with (NOLOCK) on
--		ps.pubsubscriptionID <> x.PubSubscriptionID and
--			ps.company = x.Company and
--			substring(ps.ADDRESS1,1,15)= x.ADDRESS1 and
--			LEFT(ps.ZipCode,5)  = x.ZipCode and
--			ps.RegionID = x.RegionID
--	join PubSubscriptionsExtension pse with (NOLOCK) on ps.pubsubscriptionID = pse.pubsubscriptionID
--where
--	PubID = @PubID and len(pse.field3) > 0

select t.PubsubscriptionID, ps.FirstName, ps.LastName, t.Company, t.ADDRESS1, t.RegionID, t.zipcode, ps.SequenceID, cc.CategoryCodeValue, tc.TransactionCodeValue, c.CodeValue,  ps.Qualificationdate, ps.DateCreated, ps.DateUpdated, t.ExistingDealerCode dealercode  , x.MultipleDealerCodeCounts
from 
		#tmpps t join
		(
		select Company, address1, regionID, zipcode, count(distinct existingdealercode) as MultipleDealerCodeCounts from #tmpPS
		group by Company, address1, regionID, zipcode
		having count(distinct existingdealercode) > 1
		) x on t.Company = x.Company and t.ADDRESS1 = x.ADDRESS1 and t.regionID = x.regionID and t.zipcode = x.zipcode join
		PubSubscriptions ps with (NOLOCK) on ps.PubSubscriptionID = t.PubSubscriptionID join
		UAD_Lookup..CategoryCode cc with (NOLOCK)  on ps.PubCategoryID = cc.CategoryCodeID join
		UAD_Lookup..TransactionCode tc with (NOLOCK)  on ps.PubTransactionID = tc.TransactionCodeID left outer join
		UAD_Lookup..Code c with (NOLOCK) on ps.PubQSourceID = c.codeID 
order by t.Company, t.ADDRESS1, t.RegionID, t.zipcode, t.ExistingDealerCode

--drop table #tmpPS

*/

go


--exec ccp_TenMissions_DealerCode_PriorityCode_PolybagSplit_Update @ManualRun = 1