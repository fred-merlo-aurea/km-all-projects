create procedure job_ConsensusUpdate
@SubscriptionId int,
@UpdateDetail bit = 'false'
as	
BEGIN   

	SET NOCOUNT ON 
	
	delete sd
	from SubscriptionDetails sd
		join CodeSheet_Mastercodesheet_Bridge cmb on sd.MasterID = cmb.MasterID 
	where sd.SubscriptionID = @SubscriptionId
			 
	Print ('Delete SubscriptionDetails COUNT : ' + convert(varchar(100),@@ROWCOUNT) + ' / ' + convert(varchar(20), getdate(), 114))	
	    
	delete smv
	from SubscriberMasterValues smv
	where smv.SubscriptionID = @SubscriptionId
				 
	Print ('Delete SubscriberMasterValues COUNT : ' + convert(varchar(100),@@ROWCOUNT) + ' / ' + convert(varchar(20), getdate(), 114))	
		
	insert into SubscriptionDetails (SubscriptionID, MasterID)
	select distinct psd.SubscriptionID, cmb.masterID 
	from PubSubscriptionDetail psd 
		join CodeSheet_Mastercodesheet_Bridge cmb with (NOLOCK) on psd.CodesheetID = cmb.CodeSheetID 
		left outer join SubscriptionDetails sd  on sd.SubscriptionID = psd.SubscriptionID and sd.MasterID = cmb.MasterID
	where psd.SubscriptionID = @SubscriptionId and sd.sdID is null	
		
	Print ('Insert SubscriptionDetails COUNT : ' + convert(varchar(100),@@ROWCOUNT) + ' / ' + convert(varchar(20), getdate(), 114))	
	    
	/***** Final Step *****/
	    
	insert into SubscriberMasterValues (MasterGroupID, SubscriptionID, MastercodesheetValues)
	SELECT MasterGroupID, [SubscriptionID] , 
		STUFF((
		SELECT ',' + CAST([MasterValue] AS VARCHAR(MAX)) 
		FROM [dbo].[SubscriptionDetails] sd1  with (NOLOCK)  
			join Mastercodesheet mc1  with (NOLOCK) on sd1.MasterID = mc1.MasterID  
		WHERE (sd1.SubscriptionID = Results.SubscriptionID and mc1.MasterGroupID = Results.MasterGroupID) 
		FOR XML PATH (''))
		,1,1,'') AS CombinedValues
	FROM 
		(
			SELECT distinct sd.SubscriptionID, mc.MasterGroupID
			FROM [dbo].[SubscriptionDetails] sd  with (NOLOCK)
				join Mastercodesheet mc  with (NOLOCK)  on sd.MasterID = mc.MasterID 
			where sd.SubscriptionID = @SubscriptionId			 
		)
		Results
	GROUP BY [SubscriptionID] , MasterGroupID
	order by SubscriptionID    
	    
	Print ('Insert into SubscriberMasterValues COUNT : ' + convert(varchar(100),@@ROWCOUNT)+ ' / ' + convert(varchar(20), getdate(), 114))
	    
	    
	-----------only do this for ProductSubscriptionDetail Insert/Updates
	if(@UpdateDetail = 'true')
		begin
			/*
			********** Insert/Update SubscriptionsExtension
			*/
			
			declare @StandardField varchar(255),
				@CustomField varchar(255),
				@AdhocisAlsoDimension bit = 0

			Create table #tblAdhoc (SubscriptionID int, AdhocValue varchar(max) )

			CREATE INDEX IDX_tblAdhoc_SubscriptionID ON #tblAdhoc(SubscriptionID)
			
			DECLARE curAdhoc CURSOR LOCAL FAST_FORWARD FOR SELECT CustomField, StandardField 
			FROM SubscriptionsExtensionMapper 
			where Active = 1

			OPEN curAdhoc
			FETCH NEXT FROM curAdhoc INTO @CustomField, @StandardField
			WHILE @@FETCH_STATUS = 0  
				BEGIN 
			
					if exists (select 1 from ResponseGroups where ResponseGroupName = @CustomField)
						set @AdhocisAlsoDimension = 1
			
					;WITH Adhoc_CTE (SubscriptionID,  Value)
					AS
					(
						Select distinct t.SubscriptionID,  Value
						from #tbl1 t 
							join SubscriberFinal s with(NOLOCK) on t.SubscriberFinalID = s.SubscriberFinalID
							join SubscriberDemographicFinal sdf  with(NOLOCK) on s.SFRecordIdentifier = sdf.SFRecordIdentifier
						where t.SubscriptionID is Not null and MAFField = @CustomField
						union
							Select distinct t.SubscriptionID,  Value
							from #tbl1 t 
								join SubscriberFinal s with(NOLOCK) on t.SubscriberFinalID = s.SubscriberFinalID
								join SubscriberDemographicTransformed sdt  with(NOLOCK) on s.STRecordIdentifier = sdt.STRecordIdentifier
							where @AdhocisAlsoDimension = 1 and t.SubscriptionID is Not null and MAFField = @CustomField and NotExists = 1
					)
					insert into #tblAdhoc (SubscriptionID, AdhocValue)		
				
					SELECT   DISTINCT SubscriptionID, STUFF((SELECT ',' + inn.Value FROM Adhoc_CTE AS inn WHERE inn.SubscriptionID = Adhoc_CTE.SubscriptionID FOR XML PATH('')), 1, 1, '') AS AdhocValue
					FROM      Adhoc_CTE
					ORDER BY   SubscriptionID		
			
					print @standardField + ' / ' + @CustomField + ' / counts : ' + convert(varchar(100), @@ROWCOUNT)
				
					EXEC ('Update s1 Set ' + @StandardField + ' = CASE 
							WHEN t.AdhocValue = ' + @StandardField + ' THEN ' + @StandardField + '
							WHEN ' + @StandardField + ' is null THEN t.AdhocValue
							ELSE CAST(LTRIM(RTRIM(ISNULL(' + @StandardField + ',''''))) + '','' + LTRIM(RTRIM(t.AdhocValue)) AS VARCHAR(2048)) END
					FROM SubscriptionsExtension s1 
					join #tblAdhoc t on s1.subscriptionID = t.subscriptionID
					where len(t.AdhocValue) > 0
					')
				
					Exec ('INSERT INTO SubscriptionsExtension (SubscriptionID, ' + @StandardField + ') 
					SELECT t.subscriptionID, left(AdhocValue,2048)
					FROM #tblAdhoc t  left outer join SubscriptionsExtension se on se.subscriptionID = t.subscriptionID
					WHERE se.subscriptionID is null')
					
					Truncate table #tblAdhoc
				
					FETCH NEXT FROM curAdhoc INTO @CustomField, @StandardField
				END 
			CLOSE curAdhoc
			DEALLOCATE curAdhoc		
			
			drop table #tblAdhoc
		end
END
go