CREATE PROCEDURE [dbo].[e_CampaignItem_UpdateSendTime]
	@CampaignItemID int,
	@SendTime datetime
AS
	Update ci
	set ci.SendTime = @SendTime
	from CampaignItem ci
	Join CampaignItemBlast cib on ci.CampaignItemID = cib.CampaignItemID
	join Blast b on cib.BlastID = b.BlastID
	where ci.CampaignItemID = @CampaignItemID and b.StatusCode in( 'pending','pendingcontent')

	Update b 
	set b.SendTime = @SendTime
	from CampaignItem ci
	Join CampaignItemBlast cib on ci.CampaignItemID = cib.CampaignItemID
	join Blast b on cib.BlastID = b.BlastID
	where ci.CampaignItemID = @CampaignItemID and b.StatusCode in( 'pending','pendingcontent')