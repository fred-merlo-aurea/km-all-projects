CREATE PROCEDURE job_SetOneMaster
AS
BEGIN

	SET NOCOUNT ON

	Declare @ign uniqueidentifier

	declare c cursor
	for 
		select IGrp_No
		From SubscriberFinal with(nolock)
		where IGrp_Rank='M'
		Group By IGrp_No 
		having COUNT(*) > 1
      
	open c

	fetch next from c into @ign

	while @@FETCH_STATUS = 0
	begin
	
		declare @SFID int = (Select top 1 SubscriberFinalID From SubscriberFinal with(nolock) where IGrp_No=@ign)

		update SubscriberFinal  
		set IGrp_Rank= 'S',DateUpdated = GETDATE(), UpdatedByUserID=1
		where IGrp_No=@ign and SubscriberFinalID <> @SFID
	
		--update SubscriberTransformed  
		--set IGrp_Rank=sf.IGrp_Rank 
		--,DateUpdated = GETDATE(), UpdatedByUserID=1
		--From SubscriberFinal sf
		--where sf.STRecordIdentifier = SubscriberTransformed.STRecordIdentifier 
		--and sf.IGrp_No=@ign
	
		update SubscriberArchive  
		set IGrp_Rank=sf.IGrp_Rank 
		,DateUpdated = GETDATE(), UpdatedByUserID=1
		From SubscriberFinal sf
		where sf.SFRecordIdentifier = SubscriberArchive.SFRecordIdentifier 
		and SubscriberArchive.IGrp_No=@ign
		
		fetch next from c into @ign
	end
	close c
	deallocate c

END