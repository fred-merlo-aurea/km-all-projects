CREATE PROCEDURE [dbo].[e_Blast_UsedAsSmartSegment]
	@BlastID int
AS
	select ci.* 
	from CampaignItemBlastFilter c with(nolock) 
	join CampaignItemBlast cib with(nolock) on c.CampaignItemBlastID = cib.CampaignItemBlastID
	join CampaignItem ci with(nolock) on cib.CampaignItemID = ci.CampaignItemID
	where ISNULL(c.SmartSegmentID,0) > 0 
		and(c.refBlastIDs = convert(varchar(10),@BlastID) or c.RefBlastIDs like '%,' + convert(varchar(10), @BlastID) + ',%' or c.RefBlastIDs like convert(varchar(10),@BlastID) + ',%' or c.RefBlastIDs like '%,' + convert(varchar(10),@BlastID)) 
		and ISNULL(c.IsDeleted,0) = 0 
		and ISNULL(cib.IsDeleted, 0) = 0 
		and ISNULL(ci.IsDeleted,0) = 0
	UNION
	select ci.* 
	from CampaignItemBlastFilter c with(nolock) 
	join CampaignItemSuppression cis with(nolock) on c.CampaignItemSuppressionID = cis.CampaignItemSuppressionID
	join CampaignItem ci with(nolock) on cis.CampaignItemID = ci.CampaignItemID
	where ISNULL(c.SmartSegmentID, 0) > 0 
		and (c.refBlastIDs = convert(varchar(10),@BlastID) or c.RefBlastIDs like '%,' + convert(varchar(10),@BlastID) + ',%' or c.RefBlastIDs like convert(varchar(10),@BlastID) + ',%' or c.RefBlastIDs like '%,' + convert(varchar(10),@BlastID)) 
		and ISNULL(c.IsDeleted,0) = 0  
		and ISNULL(cis.IsDeleted, 0) = 0 
		and ISNULL(ci.IsDeleted,0) = 0
	