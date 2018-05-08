CREATE PROCEDURE [dbo].[e_BlastActivityClicks_GetUniqueByURL]
	@URL varchar(max),
	@CampaignItemID int
AS
	

declare @Blasts table(BlastID int, GroupID int)
insert into @Blasts(BlastID, GroupID)
select b.BlastID, b.GroupID
from ECN5_COMMUNICATOR..Blast b with(nolock)
join ECN5_COMMUNICATOR..CampaignItemBlast cib with(nolock) on b.BlastID = cib.BlastID
where cib.CampaignItemID = @CampaignItemID and b.StatusCode not in ('deleted','cancelled') and ISNULL(cib.IsDeleted,0) = 0

if exists(select top 1 * from @Blasts where GroupID is null)
BEGIN
		declare @currentBlastID int,
				 @refBlastID int,
					@customerID int,
					@groupID int,
					@layoutPlanID int,
					@IsFormTrig bit,
					@campItemID int,
					@index int = 0
		declare mycursor cursor
		FOR
		Select BlastID, GroupID from @Blasts where GroupID is null
		OPEN mycursor
		FETCH NEXT FROM mycursor into @currentBlastID, @groupID
		WHILE @@FETCH_STATUS = 0
		BEGIN
			SET  @refBlastID = null
			SET  @layoutPlanID = null
			SET  @IsFormTrig = null
			SET @campItemID = null
			SET @refBlastID = @currentBlastID
			while @groupID is null and @index < 10
			BEGIN
				
				select @IsFormTrig = case when EventType in('abandon','submit') then 1 else 0 end, @campItemID = CampaignItemID , @layoutPlanID = LayoutPlanID
				from ECN5_COMMUNICATOR..LayoutPlans lp with(nolock)
				where lp.BlastID = @refBlastID
				
				
				if ISNULL(@IsFormTrig,0) = 0
				BEGIN			
					if @layoutPlanID is not null
					BEGIN
						select @refBlastID = BlastID from ECN5_COMMUNICATOR..CampaignItemBlast cib with(nolock) where cib.CampaignItemID = @campItemID 
						select @groupID = GroupID from ECN5_COMMUNICATOR..Blast b where b.BlastID = @refBlastID
					END
					ELSE--must be a trigger plan
					BEGIN
						select @refBlastID = RefTriggerID from ECN5_COMMUNICATOR..TriggerPlans tp with(nolock) where tp.BlastID = @refBlastID
						select @groupID = GroupID from ECN5_COMMUNICATOR..Blast b where b.BlastID = @refBlastID
					END
				END
				ELSE
				BEGIN
					
					select @refBlastID = cib.BlastID from ECN5_COMMUNICATOR..CampaignItemBlast cib with(nolock)
												 where cib.CampaignItemID = @campItemID
					select @groupID = GroupID from ECN5_COMMUNICATOR..Blast b where b.BlastID = @refBlastID
				END
				SET @index = @index + 1
			END
			
			SET @index = 0
			if @groupID is not null
			BEGIN
				update @Blasts
				set GroupID = @groupID
				where BlastID = @currentBlastID
				SET @groupID = null
			END
			FETCH NEXT FROM mycursor into @refBlastID, @groupID
		END		
		close mycursor 
		deallocate mycursor
	END			


declare @SeedListIDs table(pKey int identity(1,1),EmailID int)
  
  insert into @SeedListIDs(EmailID)
  Select eg.EmailID
  from ECN5_COMMUNICATOR..EmailGroups eg with(nolock)
  join ECN5_COMMUNICATOR..Groups g with(nolock) on eg.groupid = g.GroupID
  left outer join ECN5_COMMUNICATOR..EmailGroups eg2 with(nolock) on eg.EmailID = eg2.EmailID and eg2.GroupID in (Select GroupID from @blasts)
  where g.CustomerID in (Select CustomerID from ECN5_COMMUNICATOR..Blast b with(nolock) where b.BlastID in (select BlastID from @blasts))
              and isnull(g.IsSeedList,0) = 1
              and eg2.EmailGroupID is null


select count(DISTINCT bac.EmailID) from BlastActivityClicks bac with (nolock)  
join @Blasts b on bac.BlastID = b.BlastID
JOIN ecn5_communicator..EmailGroups eg WITH (NOLOCK)  ON eg.EmailID = bac.EmailID and eg.GroupID =b.GroupID
left outer join @SeedListIDs s on eg.EmailID = s.EmailID 
where URL = @URL and s.pKey is null