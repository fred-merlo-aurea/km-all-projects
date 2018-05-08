CREATE PROCEDURE [dbo].[e_BlastSingle_GetEmails_LayoutPlanID] 
	@LayoutPlanID INT,
	@Processed varchar(5)
AS     
     
BEGIN
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;
      declare @LPBlastID int 
      declare @LPGroupID int 
      declare @CustomerID int
      select @LPBlastID = BlastID, @LPGroupID = ISNULL(lp.GroupID,-1), @CustomerID = lp.CustomerID from LayoutPlans lp with(nolock) where lp.LayoutPlanID = @LayoutPlanID
	  declare @IsTriggerPlan bit = 0
	  if @LPBlastID is null
	  BEGIN
		
		SET @IsTriggerPlan = 1
		select @LPBlastID = BlastID, @LPGroupID = ISNULL(lp.GroupID,-1),@CustomerID = lp.CustomerID from TriggerPlans lp with(nolock) where lp.TriggerPlanID = @LayoutPlanID
	  END
	  
      DECLARE @GroupID int = null
      DECLARE @refBlastID int = 0
      declare @whileIndex int = 0
      declare @SeedListIDs table(pKey int identity(1,1),EmailID int)
      
      if @LPGroupID <= 0
      BEGIN
           SET @GroupID = null
			declare @IsFormTrig bit,
				@campItemID int,
				@index int = 0,
				@tempLayoutPlanID int
		
			SET @tempLayoutPlanID = @LayoutPlanID
			SET @refBlastID = @LPBlastID
			while @groupID is null and @index < 10
			BEGIN
				
				SET @campItemID = null
				SET @IsFormTrig = null
				SET @tempLayoutPlanID = null
				
				if ISNULL(@IsTriggerPlan,0) = 0
				BEGIN
					select @IsFormTrig = case when EventType in('abandon','submit') then 1 else 0 end, @campItemID = CampaignItemID , @tempLayoutPlanID = LayoutPlanID
					from ECN5_COMMUNICATOR..LayoutPlans lp with(nolock)
					where lp.BlastID = @refBlastID
					
				END
				ELSE
				BEGIN
					SET @IsFormTrig = 0
					SET @IsTriggerPlan = 0
				END
				
				if ISNULL(@IsFormTrig,0) = 0
				BEGIN			
					if @tempLayoutPlanID is not null
					BEGIN
						
						select @refBlastID = BlastID from ECN5_COMMUNICATOR..CampaignItemBlast cib with(nolock) where cib.CampaignItemID = @campItemID 
						select @GroupID = GroupID from ECN5_COMMUNICATOR..Blast b where b.BlastID = @refBlastID
					END
					ELSE--must be a trigger plan
					BEGIN
					
						select @refBlastID = RefTriggerID from ECN5_COMMUNICATOR..TriggerPlans tp with(nolock) where tp.BlastID = @refBlastID
						select @GroupID = GroupID from ECN5_COMMUNICATOR..Blast b where b.BlastID = @refBlastID
					END
				END
				ELSE
				BEGIN
					
					select @refBlastID = cib.BlastID from ECN5_COMMUNICATOR..CampaignItemBlast cib with(nolock)
												 where cib.CampaignItemID = @campItemID
					select @GroupID = GroupID from ECN5_COMMUNICATOR..Blast b where b.BlastID = @refBlastID
				END
				SET @index = @index + 1
			END

      END
      else
      BEGIN
            SET @GroupID = @LPGroupID
      END
      
      insert into @SeedListIDs(EmailID)
      Select eg.EmailID
      from ECN5_COMMUNICATOR..EmailGroups eg with(nolock)
      join ECN5_COMMUNICATOR..Groups g with(nolock) on eg.groupid = g.GroupID
      left outer join ECN5_COMMUNICATOR..EmailGroups eg2 with(nolock) on eg.EmailID = eg2.EmailID and eg2.GroupID = @GroupID
      where g.CustomerID = @CustomerID
                  and isnull(g.IsSeedList,0) = 1                  
                  and eg2.EmailGroupID is null

      SELECT EmailAddress from BlastSingles bs with(nolock) 
            Join Emails e WITH (NOLOCK) ON bs.EmailID=e.EmailID 
            left outer join @SeedListIDs sl on e.EmailID = sl.EmailID
          where bs.LayoutPlanID = @LayoutPlanID and bs.Processed = @Processed and sl.pKey is null
END


