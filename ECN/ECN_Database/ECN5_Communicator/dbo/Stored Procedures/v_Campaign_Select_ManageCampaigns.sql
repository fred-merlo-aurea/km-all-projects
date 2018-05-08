CREATE PROCEDURE [dbo].[v_Campaign_Select_ManageCampaigns]
	@CustomerID int,
	@CampaignName varchar(100) = '',
	@ArchiveFilter varchar(20) = 'active'
AS
				declare @totalNonTest table(CampaignID int, CampaignName varchar(100),CampaignItemID int, CISent int, CIPending int, CISaved int, IsArchived bit)
	declare @totalTest table(CampaignID int, CampaignName varchar(100),CampaignItemID int, CISent int, CIPending int, CISaved int, IsArchived bit)
	INSERT INTO @totalNonTest
		select c.CampaignID, 
			   c.CampaignName,
			   ci.CampaignitemID,
			   Count(distinct case when ISNULL(ci.IsDeleted,0) = 0 and ISNULL(cib.IsDeleted,0) = 0 and ISNULL(b.StatusCode,'') = 'sent' then ci.CampaignItemID end) as 'CISent', 
			   Count(distinct case when ISNULL(ci.IsDeleted,0) = 0 and  ISNULL(cib.IsDeleted,0) = 0 and ISNULL(b.StatusCode,'') in('pending','active','system') then ci.CampaignItemID end) as 'CIPending', 
			   COUNT(distinct case when ISNULL(ci.IsDeleted,0) = 0 and ((cib.CampaignItemBlastID is null or b.BlastID is null) and ISNULL(b.statuscode,'') not in ('sent','pending','active','deleted','cancelled')) then ci.CampaignitemID end) as 'CISaved',
			   ISNULL(c.IsArchived, 0) as IsArchived 
		from Campaign c with(nolock)
		left outer Join CampaignItem ci with(nolock) on c.CampaignID = ci.CampaignID 
		left outer Join CampaignItemBlast cib with(nolock) on ci.CampaignItemID = cib.CampaignItemID
		left outer join Blast b with(nolock) on cib.BlastID = b.BlastID
		WHERE c.CustomerID = @CustomerID 
				and ISNULL(c.IsDeleted,0) = 0 
				and c.CampaignName like '%' + @CampaignName + '%'
				and ISNULL(c.IsArchived,0) = case when @ArchiveFilter = 'all' then ISNULL(c.IsArchived,0) when @ArchiveFilter = 'active' then 0 when @ArchiveFilter = 'archived' then 1 end

		group by c.CampaignID, c.CampaignName,ci.CampaignItemID, c.IsArchived
		order by c.CampaignName asc
	
	
	declare @FinalTable table (CampaignID int, CampaignName varchar(100),CampaignItemID int, CISent int, CIPending int,CISaved int, IsArchived bit)
	
--select * from @totalNonTest
--where CISaved = 1
--order by CampaignItemID

	insert into @FinalTable
	select * from @totalNonTest

	

	select tnt.CampaignID, tnt.CampaignName, SUM(ISNULL(tnt.CISent,0)) as 'CISent', SUM(ISNULL(tnt.CIPending,0)) as 'CIPending', SUM(ISNULL(tnt.CISaved,0)) as 'CISaved', tnt.IsArchived 
	from @FinalTable tnt
	group by tnt.CampaignID, tnt.CampaignName,tnt.IsArchived
	order by tnt.CampaignName