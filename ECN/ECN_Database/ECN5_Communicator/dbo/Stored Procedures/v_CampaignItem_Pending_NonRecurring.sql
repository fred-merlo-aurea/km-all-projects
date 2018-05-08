CREATE PROCEDURE [dbo].[v_CampaignItem_Pending_NonRecurring]
	@CustomerID int
AS
	Select distinct ci.CampaignItemID, ci.CampaignItemName, case when COUNT(b.GroupID) > 1 then '-- Multiple Groups --' else MAX(g.GroupName) end as 'Group', b.CustomerID, b.EmailSubject ,ci.CampaignItemType,ISNULL(ci.UpdatedDate,ci.CreatedDate) as 'UpdatedDate', b.SendTime as 'SendTime', b.LayoutID as 'LayoutID', l.LayoutName as 'LayoutName'
	from CampaignItem ci with(nolock)
	join CampaignItemBlast cib with(nolock) on ci.CampaignItemID = cib.CampaignItemID
	join Blast b with(nolock) on cib.BlastID = b.BlastID
	join Layout l with(nolock) on b.LayoutID = l.LayoutID
	left outer join BlastSchedule bs with(nolock) on b.BlastScheduleID = bs.BlastScheduleID
	join Groups g with(nolock) on b.GroupID = g.GroupID
	where b.CustomerID = @CustomerID
		  and b.StatusCode = 'Pending'
		  and b.BlastType = 'HTML'
		  and ISNULL(bs.Period, 'o') = 'o'
		  and ISNULL(ci.IsDeleted,0) = 0
		  and ISNULL(cib.IsDeleted,0) = 0
	GROUp BY ci.CampaignItemID, ci.CampaignItemName, b.CustomerID, b.EmailSubject,ci.CampaignItemType, ISNULL(ci.UpdatedDate,ci.CreatedDate) , b.SendTime, b.LayoutID, l.LayoutName