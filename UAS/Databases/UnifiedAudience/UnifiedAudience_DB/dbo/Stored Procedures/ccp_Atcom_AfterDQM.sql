CREATE PROCEDURE [dbo].[ccp_Atcom_AfterDQM]
@SourceFileID int,
@ClientID int = 4 
AS
BEGIN
	SET NOCOUNT ON;
--	DECLARE @err_message VARCHAR(255)

--	-- Calculate REG_TYPE_ADHOC with STUFF
--	IF EXISTS(select * from SubscriberDemographicTransformed sdt with(nolock) inner join SubscriberTransformed st with(nolock) on sdt.STRecordIdentifier = st.STRecordIdentifier 
--	where MAFFIeld in ('country_of_licensure') )
--	BEGIN TRY
--		select DISTINCT STRecordIdentifier, STUFF((SELECT ',' + value
--										FROM  (select STRecordIdentifier, a2.Value from SubscriberTransformed a1 
--												inner join SubscriberDemographicTransformed a2 
--												on a1.SORecordIdentifier = a2.SORecordIdentifier
--												where a2.MAFField='REG_TYPE') t1
--										WHERE t1.STRecordIdentifier = t2.STRecordIdentifier and value in
--										('DG','D1','DC','VIP','WS','WL','LN','EH','EHL','EX','DE','ST','GT','MF','BD','SP','VR','GF','OP','CR','OR','CS','PR','CD','CE','CN','INA','OD','AB')
--										FOR XML PATH('')),1,1,'') as REG_TYPE_ADHOC
--			into #tmp_REGTYPE
--            from SubscriberTransformed t2
		
--		update t1
--		set value = t3.REG_TYPE_ADHOC
--		from SubscriberDemographicTransformed as t1
--			inner join SubscriberTransformed as t2
--			on t1.SORecordIdentifier = t2.SORecordIdentifier
--			inner join #tmp_REGTYPE as t3
--			on t2.STRecordIdentifier=t3.STRecordIdentifier
--		where t1.MAFField='REG_TYPE_ADHOC' and t2.IGrp_Rank='M'

--	END TRY
--	BEGIN CATCH
--		set @err_message = ERROR_MESSAGE();
--		PRINT(@err_message);
--	END CATCH
END