CREATE PROCEDURE [dbo].[e_Blast_Select_MasterRefBlast]
	@BlastID int ,
	@EmailID int
AS
	DECLARE @GroupID int = null
	
	declare @layoutPlanID int,
			@IsFormTrig bit,
			@campItemID int,
			@customerID int,
			@refBlastID int
	
	select @refBlastID = refblastid, @layoutPlanID = LayoutPlanID from [BlastSingles] where BlastID = @BlastID and EmailID = @EmailID
	
	select @IsFormTrig = case when EventType in('abandon','submit') then 1 else 0 end, @campItemID = CampaignItemID 
	from LayoutPlans 
	where LayoutPlanID = @layoutPlanID and BlastID = @BlastID and Status = 'y' and ISNULL(IsDeleted,0)= 0

	if @IsFormTrig is null--must be trigger plan
	BEGIN
		select @refBlastID = tp.RefTriggerID 
		from TriggerPlans tp with(nolock)
		where tp.TriggerPlanID = @layoutPlanID and tp.Status = 'y' and ISNULL(tp.IsDeleted,0) = 0
	END
	
	select @customerID = CustomerID, @groupID = groupID from [BLAST] where  blastID = @refBlastID 
	declare @blastHolder int
	set @blastHolder = @refBlastID
	declare @whileIndex int = 0
	
	while(@groupID is null and @whileIndex < 10)
	BEGIN
		SET @layoutPlanID = null
		SET @campItemID = null
		select @layoutPlanID = lp.LayoutPlanID, @campItemID = lp.CampaignItemID 
		from LayoutPlans lp with(nolock) where lp.BlastID = @blastHolder and lp.Status = 'y' and ISNULL(lp.isdeleted,0) = 0
		
		if @layoutPlanID is not null and @campItemID is not null
		BEGIN
			--must be a layout plan
			select @blastHolder = BlastID 
			from CampaignItemBlast cib with(nolock) 
			where cib.CampaignItemID = @campItemID
			
			select @groupID = GroupID from Blast b with(nolock) where b.BlastID = @blastHolder
		END
		ELSE IF @layoutPlanID is not null and @campItemID is null
		BEGIN
			select @layoutPlanID = LayoutPlanID, @blastHolder = bs.refblastID
			from BlastSingles bs with(nolock) 
			where bs.BlastID = @blastHolder and bs.EmailID = @emailID
			
			select @groupID = GroupID from Blast b with(nolock) where b.BlastID = @blastHolder
		END
		ELSE
		BEGIN		
			--must be a trigger plan
			select @blastHolder = tp.RefTriggerID 
			from TriggerPlans tp with(nolock)
			where tp.BlastID = @blastHolder
			
			select @campItemID = CampaignItemID
			from CampaignItemBlast cib with(nolock) 
			where cib.BlastID = @blastHolder
			
			select @groupID = GroupID from Blast b with(nolock) where b.BlastID = @blastHolder
		END
		
		SET @whileIndex = @whileIndex + 1
	END
	if(@GroupID is not null)
	BEGIN
		SELECT * FROM Blast b with(nolock) where b.blastid = @blastHolder
	END