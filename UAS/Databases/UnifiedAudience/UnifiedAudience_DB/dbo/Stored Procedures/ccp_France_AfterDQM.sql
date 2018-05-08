CREATE PROCEDURE [dbo].[ccp_France_AfterDQM]
@SourceFileID int,
@ProcessCode varchar(50),
@ClientId int = 8
AS
BEGIN

	set nocount on

	--DECLARE @err_message VARCHAR(255)


	--/**
	--	Updates Regions from state
	--**/
	--IF EXISTS(select * from SubscriberDemographicTransformed sdt with(nolock) inner join SubscriberTransformed st with(nolock) on sdt.SORecordIdentifier = st.SORecordIdentifier where MAFFIeld in ('Region'))
	--BEGIN
	--	BEGIN TRY
	--		update sdt
	--		set value = St.[state]
	--		from SubscriberDemographicTransformed sdt with(nolock)
	--				inner join SubscriberTransformed st with(nolock)
	--					on sdt.SORecordIdentifier = st.SORecordIdentifier
	--		where ISNULL(st.[state],'')!='' and sdt.MAFField = 'Region'
	--	END TRY
	--	BEGIN CATCH
	--		set @err_message = ERROR_MESSAGE();
	--		RAISERROR(@err_message,12,1);
		
	--	END CATCH
	--END
	----ELSE
	----	BEGIN
	----		--set @err_message = 'REGION(DEMO90) column does not exist'
	----		--RAISERROR(@err_message,11,1);
	----	END

	--/** 
	--**	Updates the value if they are PaymentAmount or CollectedAmount to AmountCollected, can't have both
	--**/
	--IF EXISTS(select * from SubscriberDemographicTransformed sdt with(nolock) inner join SubscriberTransformed st with(nolock) on sdt.SORecordIdentifier = st.SORecordIdentifier where sdt.MAFField in ('CollectedAmount','PaymentAmount'))
	--BEGIN
	--	BEGIN TRY
	
	--		update sdt
	--		set value = ca.CollectedAmount
	--		from SubscriberDemographicTransformed sdt
	--				inner join SubscriberTransformed st
	--					on sdt.SORecordIdentifier = st.SORecordIdentifier
	--				inner join ( select SORecordIdentifier
	--									,value as CollectedAmount
	--							 from SubscriberDemographicTransformed with(nolock)
	--							 where MAFField = 'CollectedAmount') as ca
	--					on sdt.SORecordIdentifier = ca.SORecordIdentifier
	--				--inner join ( select SORecordIdentifier
	--				--					,value as PaymentAmount
	--				--			 from SubscriberDemographicTransformed with(nolock)
	--				--			 where MAFField = 'PaymentAmount') as pa
	--				--	on sdt.SORecordIdentifier = ca.SORecordIdentifier
	--		where MAFField = 'AmountCollected' 
		
	--		update sdt
	--		set value = pa.PaymentAmount
	--		from SubscriberDemographicTransformed sdt
	--				inner join SubscriberTransformed st
	--					on sdt.SORecordIdentifier = st.SORecordIdentifier
	--				--inner join ( select SORecordIdentifier
	--				--					,value as CollectedAmount
	--				--			 from SubscriberDemographicTransformed with(nolock)
	--				--			 where MAFField = 'CollectedAmount') as ca
	--				--	on sdt.SORecordIdentifier = ca.SORecordIdentifier
	--				inner join ( select SORecordIdentifier
	--									,value as PaymentAmount
	--							 from SubscriberDemographicTransformed with(nolock)
	--							 where MAFField = 'PaymentAmount') as pa
	--					on sdt.SORecordIdentifier = st.SORecordIdentifier
	--		where MAFField = 'AmountCollected' 
			
	--		-- All null values should be set to zero for AmountCollectedRanges can be set in next step
	--		update sdt
	--		set Value = 0
	--		from SubscriberDemographicTransformed sdt with(nolock)
	--				inner join SubscriberTransformed st with(nolock)
	--					on sdt.SORecordIdentifier = st.SORecordIdentifier
	--		where sdt.MAFField = 'AmountCollected' and ISNULL(sdt.value,'')='' 
		
	--	END TRY
	--	BEGIN CATCH
	--		set @err_message = ERROR_MESSAGE();
	--		RAISERROR(@err_message,12,1);
	--	END CATCH
	--END	
	----ELSE
	----	BEGIN
	----		--set @err_message = 'CollectedAmount or PaymentAmount column does not exist'
	----		--RAISERROR(@err_message,11,1);
	----	END

	--/**
	--** Sum AmountCollected if they are in the same igrp_no and appends it on to Master records
	--**/

	--		select st.IGrp_No
	--				,SUM(cast(ac.AmountCollected as decimal(12,0))) as TotalAmountCollected
	--				into #tmp_sumAmount
	--		from SubscriberDemographicTransformed sdt
	--				inner join ( select igrp_no
	--									,igrp_rank
	--									,SORecordIdentifier
									
	--							 from SubscriberTransformed with(nolock)) as st
	--					on sdt.SORecordIdentifier = st.SORecordIdentifier
	--				inner join ( select SORecordIdentifier
	--									,value as AmountCollected
	--							 from SubscriberDemographicTransformed with(nolock)
	--							 where MAFField = 'AmountCollected') as ac -- AmountCollected
	--					on sdt.SORecordIdentifier = ac.SORecordIdentifier
	--		where MAFField = 'Total Amount Collected' -- and cast(ac.AmountCollected as decimal(12,2)) != 0 and st.IGrp_No = 274767
	--		GROUP BY st.IGrp_No
		
	--		update sdt
	--		set value = sa.TotalAmountCollected
	--		from SubscriberDemographicTransformed sdt with(nolock)
	--			inner join SubscriberTransformed st with(nolock)
	--				on sdt.SORecordIdentifier = st.SORecordIdentifier
	--			inner join #tmp_sumAmount sa
	--				on st.IGrp_No = sa.igrp_no
	--		where st.IGrp_Rank = 'M' and sdt.MAFField = 'Total Amount Collected'
		
	--		update sdt
	--		set Value = 0.00
	--		from SubscriberDemographicTransformed sdt with(nolock)
	--			inner join SubscriberTransformed st with(nolock)
	--				on sdt.SORecordIdentifier = st.SORecordIdentifier
	--		where ISNULL(value,'')='' and MAFField = 'Total Amount Collected' 
		


	--/** 
	--Updates AmountCollectedRanges based on AmountCollected 
	--**/
	--IF EXISTS(select * from SubscriberDemographicTransformed sdt with(nolock) inner join SubscriberTransformed st with(nolock) on sdt.SORecordIdentifier = st.SORecordIdentifier where sdt.MAFField in ('AmountCollected','AmountCollectedRanges'))
	--BEGIN
	--	BEGIN TRY
		
	--		update sdt
	--		set value = case when amc.AmountCollected <= 99 then '0-99'
	--						when amc.AmountCollected >= 100 and amc.AmountCollected <=199 then '100-199'
	--						when amc.AmountCollected >= 200 and amc.AmountCollected <=299 then '200-299'
	--						when amc.AmountCollected >= 300 and amc.AmountCollected <=399 then '300-399'
	--						when amc.AmountCollected >= 400 and amc.AmountCollected <=499 then '400-499'
	--						when amc.AmountCollected >= 500 and amc.AmountCollected <=599 then '500-599'
	--						when amc.AmountCollected >= 600 and amc.AmountCollected <=699 then '600-699'
	--						when amc.AmountCollected >= 700 and amc.AmountCollected <=799 then '700-799'
	--						when amc.AmountCollected >= 800 and amc.AmountCollected <=899 then '800-899'
	--						when amc.AmountCollected >= 900 and amc.AmountCollected <=999 then '900-999'
	--						when amc.AmountCollected >= 1000 and amc.AmountCollected <=1099 then '1000-1099'
	--						when amc.AmountCollected >= 1100 and amc.AmountCollected <=1199 then '1100-1199'
	--						when amc.AmountCollected >= 1200 and amc.AmountCollected <=1299 then '1200-1299'
	--						when amc.AmountCollected >= 1300 and amc.AmountCollected <=1399 then '1300-1399'
	--						when amc.AmountCollected >= 1400 and amc.AmountCollected <=1499 then '1400-1499'
	--						when amc.AmountCollected >= 1500 then '1500+' else '' end--) as AmountCollectedRanges
	--		from SubscriberDemographicTransformed sdt
	--			inner join(	select SORecordIdentifier
	--								,cast(value as decimal(12,0)) as AmountCollected
	--								from SubscriberDemographicTransformed with(nolock)
	--								where MAFField = 'AmountCollected') as amc
	--				on sdt.SORecordIdentifier = amc.SORecordIdentifier
	--			inner join SubscriberTransformed st with(nolock)
	--				on sdt.SORecordIdentifier = st.SORecordIdentifier
	--		where sdt.MAFField = 'AmountCollectedRanges' 
		
	--	END TRY
	--	BEGIN CATCH
	    
	--		set @err_message = ERROR_MESSAGE();
	--		RAISERROR(@err_message,12,1);
	
	--	END CATCH
	--END
	----ELSE
	----	BEGIN
	----		--set @err_message = 'AmountCollected(DEMO91) or AmountCollectedRanges(DEMO92) column does not exist'
	----		--RAISERROR(@err_message,11,1);
	----	END

	--	-- Change cat17 to Cat 27
	--	update SubscriberTransformed
	--	set CategoryID = 27
	--	where CategoryID = 17 

END