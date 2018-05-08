CREATE PROCEDURE [dbo].[v_CampaignItem_Search]
	@CampaignID int,
	@Status varchar(20),
	@Name varchar(255),
	@PageSize int,
	@PageIndex int
AS

declare @validstatus table(Status varchar(20))
if @Status = 'sent'
BEGIN
insert into @validstatus(Status)
VALUES('sent'),('system'),('active')
END
ELSE if @Status = 'pending'
BEGIN
	INSERT INTO @validstatus(Status)
	VALUES('pending'),('system')
END

declare @NonTest table(CampaignItemID int, CampaignItemName varchar(255), EmailSubject varchar(255), GroupName varchar(255), SendTime DateTime, SendTotal int, BlastType varchar(20))
declare @Test table(CampaignItemID int, CampaignItemName varchar(255), EmailSubject varchar(255), GroupName varchar(255), SendTime DateTime, SendTotal int, BlastType varchar(20))
declare @Saved table(CampaignItemID int, CampaignItemName varchar(255), EmailSubject varchar(255), GroupName varchar(255), SendTime DateTime, SendTotal int, BlastType varchar(20))
if @Status in ('sent','pending')
BEGIN
	INSERT INTO @NonTest
	SELECT
	ci.CampaignItemID,ci.CampaignItemName, 
			case when COUNT(b.BlastID) > 1 and COUNT(distinct b.EmailSubject) > 1 then ' -- Multiple Subject Lines -- ' else MAX(b.EmailSubject) end as EmailSubject, 
			case when COUNT(b.BlastID) > 1 and ISNULL(b.BlastType,'') <> 'sample' then ' -- Multi Group Blast -- '
				when MAX(b.GroupID) is null and (ISNULL(b.BlastType,'') in ('layout','noopen') or ISNULL(b.StatusCode,'') = 'System') then ' -- Trigger Blast -- ' else MAX(g.GroupName)  end  as GroupName,
			MAX(b.SendTime) as SendTime,
			SUM(b.SendTotal) as SendTotal,
			case when b.BlastType = 'sample' then 'AB' when b.BlastType in ('HTML','text') then 'Regular' when b.BlastType in ('layout','noopen') then 'Trigger' else b.BlastType end as BlastType
	
	FROM CampaignItem ci with(nolock)
	left outer join CampaignItemBlast cib with(nolock) on ci.CampaignItemID = cib.CampaignItemID
	left outer join Blast b with(nolock) on cib.BlastID = b.BlastID
	left outer join Groups g with(nolock) on b.groupid = g.GroupID
	where ISNULL(b.StatusCode,'') in (SELECT Status from @validstatus) 
			and ci.CampaignID = @CampaignID 
			and ci.CampaignItemName like '%' + @Name + '%'
			and ISNULL(ci.IsDeleted,0) = 0
			--and b.StatusCode not in ('cancelled','deleted')
	group by ci.CampaignItemID,ci.CampaignItemName, g.GroupName,b.SendTime, b.BlastType,b.StatusCode

END
ELSE if @Status = 'saved'
BEGIN
	INSERT INTO @Saved
	SELECT ci.CampaignItemID, 
		   ci.CampaignItemName, 
		   case when COUNT(distinct ISNULL(b.EmailSubject,'')) > 1 then ' -- Multiple Subject Lines -- ' else ISNULL(MAX(b.EmailSubject),'') end,
		   case when COUNT(distinct ISNULL(g.GroupID,0)) > 1 then ' -- Multiple Groups -- ' else ISNULL(MAX(g.GroupName), '') end,
		   NULL,
		   0,
		   case when ISNULL(MAX(b.BlastType),'') = 'sample' then 'AB' when ISNULL(MAX(b.BlastType),'') in ('HTML','layout','noopen','text') then 'Regular' else ISNULL(MAX(b.BlastType),'') end as BlastType
	FROM CampaignItem ci with(nolock)
	left outer join CampaignItemBlast cib with(nolock) on ci.CampaignItemID = cib.CampaignItemID	
	left outer join Blast b with(nolock) on cib.BlastID = b.BlastID	
	left outer join Groups g with(nolock) on b.GroupID = g.GroupID	
	WHERE ci.CampaignID = @CampaignID
		and ci.CampaignItemName like '%' + @Name + '%'
		and ISNULL(ci.IsDeleted,0) = 0
		and ISNULL(b.statuscode,'') not in ('sent','pending','active','deleted','cancelled','error')
	GROUP By ci.CampaignItemID,ci.CampaignItemName
END

declare @FinalTable table(CampaignItemID int, CampaignItemName varchar(255), EmailSubject varchar(255), GroupName varchar(255), SendTime DateTime, SendTotal int, BlastType varchar(20))
insert into @FinalTable
select * from @NonTest

INsert into @FinalTable
select * from @Test;

insert into @FinalTable
select * from @Saved;

WITH Results


    AS (SELECT ROW_NUMBER() OVER (ORDER BY nt.CampaignItemName) 
	AS ROWNUM,Count(nt.CampaignItemID) over () AS TotalCount,nt.CampaignItemID,nt.CampaignItemName, 
			nt.EmailSubject,
			case when COUNT(distinct nt.GroupName) > 1 then ' -- Multiple Groups -- ' else MAX(nt.GroupName) end as 'GroupName',
			MAX(nt.SendTime) as SendTime,
			SUM(nt.SendTotal) as SendTotal,
			MAX(nt.BlastType) as BlastType
	
	FROM @FinalTable nt
	group by nt.CampaignItemID,nt.CampaignItemName,nt.EmailSubject
	)
	SeLECT ROWNUM,TotalCount,CampaignItemID,CampaignItemName, 
			EmailSubject, 
			GroupName,
			SendTime,
			SendTotal,
			BlastType
	FROM Results
    WHERE ROWNUM between ((@PageIndex - 1) * @PageSize + 1) and (@PageIndex * @PageSize)
    ORDER BY CampaignItemName