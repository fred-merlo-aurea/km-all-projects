
CREATE PROCEDURE [dbo].[ccp_NASFT_AfterDQM]
	@SourceFileId int,
	@ProcessCode varchar(50),
	@ClientId int = -1
AS
BEGIN

	set nocount on

	--BEGIN
	--		-- NASFT wants their data to be rolled up differently for DEMO fields.  They will not be using the same roll ups as the standard on used
	--		-- by the rest of the clients.  Phone, Fax and mobile will be used in the standard roll ups

	--		UPDATE SubscriberFinal
	--		SET DEMO31 = case when ISNULL(DEMO31,'')='' then 1 else DEMO31 end,
	--			DEMO32 = case when ISNULL(demo32,'')='' then 1 else DEMO32 end,
	--			DEMO33 = case when ISNULL(demo33,'')='' then 1 else DEMO33 end,
	--			DEMO34 = case when ISNULL(DEMO34,'')='' then 1 else DEMO34 end,
	--			DEMO35 = case when ISNULL(DEMO35,'')='' then 1 else DEMO35 end,
	--			DEMO36 = case when ISNULL(DEMO36,'')='' then 1 else DEMO36 end	
	--		where SourceFileId = @SourceFileId

	--		-- Rolls up DEMO31 from most recent source
	--		update sf
	--		set Demo31 = iSt.Demo31, IsUpdatedInLive = 0
	--		from SubscriberFinal sf
	--			inner join (select igrp_no,demo31
	--						from SubscriberTransformed
	--						where IGrp_Rank = 'S' and SourceFileID = @SourceFileId) as iSt
	--			on sf.IGrp_No = iSt.IGrp_No
	--		where sf.IGrp_Rank = 'M' 
		
	--		-- Rolls up DEMO32 from most recent source
	--		update sf
	--		set Demo32 = iSt.Demo32, IsUpdatedInLive = 0
	--		from SubscriberFinal sf
	--			inner join (select igrp_no,Demo32
	--						from SubscriberTransformed
	--						where IGrp_Rank = 'S' and SourceFileID = @SourceFileId) as iSt
	--			on sf.IGrp_No = iSt.IGrp_No
	--		where sf.IGrp_Rank = 'M' and sf.Demo32 = 1 

	--		-- Rolls up DEMO33 from most recent source
	--		update sf
	--		set Demo33 = iSt.Demo33, IsUpdatedInLive = 0
	--		from SubscriberFinal sf
	--			inner join (select igrp_no,Demo33
	--						from SubscriberTransformed
	--						where IGrp_Rank = 'S' and SourceFileID = @SourceFileId) as iSt
	--			on sf.IGrp_No = iSt.IGrp_No
	--		where sf.IGrp_Rank = 'M' and sf.Demo33 = 1 

	--		-- Rolls up DEMO34 from most recent source
	--		update sf
	--		set Demo34 = iSt.Demo34, IsUpdatedInLive = 0
	--		from SubscriberFinal sf
	--			inner join (select igrp_no,Demo34
	--						from SubscriberTransformed
	--						where IGrp_Rank = 'S' and SourceFileID = @SourceFileId) as iSt
	--			on sf.IGrp_No = iSt.IGrp_No
	--		where sf.IGrp_Rank = 'M' and sf.Demo34 = 1 
		
	--		-- Rolls up DEMO35 from most recent source
	--		update sf
	--		set Demo35 = iSt.Demo35, IsUpdatedInLive = 0
	--		from SubscriberFinal sf
	--			inner join (select igrp_no,Demo35
	--						from SubscriberTransformed
	--						where IGrp_Rank = 'S' and SourceFileID = @SourceFileId) as iSt
	--			on sf.IGrp_No = iSt.IGrp_No
	--		where sf.IGrp_Rank = 'M' and sf.Demo35 = 1 
		
	--		-- Rolls up DEMO36 from most recent source
	--		update sf
	--		set Demo36 = iSt.Demo36, IsUpdatedInLive = 0
	--		from SubscriberFinal sf
	--			inner join (select igrp_no,Demo36
	--						from SubscriberTransformed
	--						where IGrp_Rank = 'S' and SourceFileID = @SourceFileId) as iSt
	--			on sf.IGrp_No = iSt.IGrp_No
	--		where sf.IGrp_Rank = 'M' and sf.Demo36 = 1 
		
	--END

END