create proc [dbo].[sp_getBlastSingleEmailStatus]
(
 @blastsingleID int
)
as
Begin

	declare @emailID int,
			@refBlastID int,
			@customerID int,
			@groupID int,
			@layoutPlanID int,
			@IsFormTrig bit,
			@campItemID int,
			@currBlastID int,
			@currentLayoutPlanID int

    select @emailID = emailID, @currBlastID = BlastID, @refBlastID = isnull(refblastID,0), @currentLayoutPlanID = LayoutPlanID, @layoutPlanID = LayoutPlanID from blastSingles where blastSingleID = @blastsingleID
	
	select @IsFormTrig = case when EventType in('abandon','submit') then 1 else 0 end, @campItemID = CampaignItemID 
	from LayoutPlans 
	where LayoutPlanID = @layoutPlanID and BlastID = @currBlastID and Status = 'y' and ISNULL(IsDeleted,0)= 0

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
			where tp.BlastID = @blastHolder and ISNULL(tp.IsDeleted,0) = 0 and tp.Status = 'y'
			
			select @campItemID = CampaignItemID
			from CampaignItemBlast cib with(nolock) 
			where cib.BlastID = @blastHolder
			
			select @groupID = GroupID from Blast b with(nolock) where b.BlastID = @blastHolder
		END
		
		SET @whileIndex = @whileIndex + 1
	END
	
	if (@refBlastID = 0 or len(@refBlastID) = 0 ) 
		select '1'
	else
	Begin
		if exists(select emailgroupID from emailgroups eg 
				  join groups g on eg.groupID = g.groupID 
				  where	customerID = @customerID and emailID = @emailID and mastersupression = 1	 				  
				  )
		Begin
			
			select '0'
		end 
		else
		Begin
			if exists(select emailgroupID 
					  from emailgroups 
					  where emailID = @emailID and groupID = @GroupID and subscribetypecode='S'					  
					  UNION
					  select emailgroupID from emailgroups eg with(nolock)
					  join groups g with(nolock) on eg.groupid = g.groupid
					  join LayoutPlans lp with(nolock) on g.GroupID = lp.GroupID
					  where lp.Criteria = eg.SubscribeTypeCode and eg.EmailID = @emailID and ISNULL(lp.IsDeleted,0) = 0 and lp.Status = 'Y' and lp.LayoutPlanID = @currentLayoutPlanID
					  
					  ) 
					  BEGIN
					  
				select '1'
				END
			else
			BEGIN				
			
				select '0'
				
				END
		End
	end


End