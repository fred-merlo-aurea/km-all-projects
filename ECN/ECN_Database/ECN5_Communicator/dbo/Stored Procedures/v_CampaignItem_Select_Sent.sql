--sent (TEST or LIVE blast)
--this returns campaign items that have any part of them that match the criteria
CREATE PROCEDURE [dbo].[v_CampaignItem_Select_Sent] 
(  
	@CustomerID int,
	@CampaignName varchar(100) = NULL,
	@CampaignItemName varchar(100) = NULL,
	@EmailSubject varchar(255) = NULL,
	@LayoutName varchar(50) = NULL,
	@GroupName varchar(50) = NULL,
	@BlastID int = NULL,
	@FromDate datetime = NULL,
	@ToDate datetime = NULL,
	@UserID int = NULL,
	@TestBlast bit = NULL
)
as
Begin

	--declare
	--@CustomerID int,
	--@CampaignName varchar(100) = NULL,
	--@CampaignItemName varchar(100) = NULL,
	--@EmailSubject varchar(255) = NULL,
	--@LayoutName varchar(50) = NULL,
	--@GroupName varchar(50) = NULL,
	--@BlastID int = NULL,
	--@FromDate datetime = NULL,
	--@ToDate datetime = NULL,
	--@UserID int = NULL,
	--@TestBlast bit = NULL

	--set @customerID = 1797
	--set @FromDate = '1/1/2011'
	--set @ToDate = '3/31/2013'
	----set @CampaignName = 'PKG'
	--set @CampaignItemName = '1'
	--set @EmailSubject = 'plast'
	----set @LayoutName = '16'
	--set @groupname = 'plast'
	--set @UserID = 2471
	
	if @FromDate is null
		set @FromDate = DATEADD(dd, -14, getdate())

	if @ToDate is null
		set @ToDate = getdate()
	
	if (@TestBlast is null)
	
	Begin
	
	
		SELECT	b.CustomerID, c.CampaignID, c.CampaignName, ci.CampaignItemID,ci.CampaignItemName, MAX(b.SendTime) as 'SendTime', b.testblast,
				ci.CampaignItemType, ci.CampaignItemFormatType, ci.IsHidden, -- l.LayoutName,
				MAX(case when ci.CampaignItemType = 'AB' then '-- Multiple Subject lines --' else b.EmailSubject end) as emailsubject, 
				case when COUNT(b.BlastID) > 1 then ' -- Multi Group Blast --' else MAX(g.GroupName)  end  as GroupName,
				SUM(b.Sendtotal) as SendTotal, COUNT(b.BlastID) as TotalBlasts, MAX(b.BlastID) as 'BlastID'
				
		FROM 
				Campaign c WITH (NOLOCK) 
				JOIN CampaignItem ci WITH (NOLOCK) ON c.CampaignID = ci.CampaignID
				JOIN CampaignItemBlast cib WITH (NOLOCK) ON ci.CampaignItemID = cib.CampaignItemID
				JOIN Blast b WITH (NOLOCK) ON cib.BlastID = b.BlastID 
				JOIN Groups g WITH (NOLOCK) ON b.groupID = g.groupID
				JOIN Layout l WITH (NOLOCK) ON b.LayoutID = l.LayoutID	
		WHERE 
			c.CustomerID = @CustomerID and 
			b.CustomerID = @CustomerID and 
			b.statuscode = 'sent' and
			b.TestBlast = 'N' and
			Convert(date, b.SendTime) between @fromdate and @toDate and
			cib.IsDeleted = 0 and 
			ci.IsDeleted = 0 and
			(@CampaignName is null or c.CampaignName = @CampaignName) and
			(@CampaignItemName is null or ci.CampaignItemName like '%' + @CampaignItemName + '%') and
			(@EmailSubject is null or b.EmailSubject like '%' + @EmailSubject + '%') and
			(@LayoutName is null or l.LayoutName like '%' + @LayoutName + '%') and
			(@BlastID is null or b.BlastID = @BlastID) and
			(@GroupName is null or g.GroupName like '%' + @GroupName + '%') and
			(@UserID is null or b.CreatedUserID = @userID or b.UpdatedUserID = @UserID)
		group by
				b.CustomerID, c.CampaignID, c.CampaignName, ci.CampaignItemID,ci.CampaignItemName, b.testblast,
				 ci.CampaignItemType, ci.CampaignItemFormatType, ci.IsHidden--, b.SendTime, l.LayoutName
		union all
		SELECT	b.CustomerID, c.CampaignID, c.CampaignName, ci.CampaignItemID,ci.CampaignItemName, MAX(b.SendTime) , b.testblast,
				 ci.CampaignItemType, ci.CampaignItemFormatType, ci.IsHidden, --b.SendTime, l.LayoutName,
				MAX(case when ci.CampaignItemType = 'AB' then '-- Multiple Subject lines --' else b.EmailSubject end) as emailsubject,
				case when COUNT(b.BlastID) > 1 then ' -- Multi Group Blast --' else MAX(g.GroupName)  end  as GroupName,
				SUM(b.Sendtotal) as SendTotal, COUNT(b.BlastID) as TotalBlasts, MAX(b.BlastID) as 'BlastID'
		FROM 
				Campaign c WITH (NOLOCK) 
				JOIN CampaignItem ci WITH (NOLOCK) ON c.CampaignID = ci.CampaignID
				JOIN CampaignItemTestBlast cib WITH (NOLOCK) ON ci.CampaignItemID = cib.CampaignItemID
				JOIN Blast b WITH (NOLOCK) ON cib.BlastID = b.BlastID 
				JOIN Groups g WITH (NOLOCK) ON b.groupID = g.groupID
				JOIN Layout l WITH (NOLOCK) ON b.LayoutID = l.LayoutID	
		WHERE 
			c.CustomerID = @CustomerID and 
			b.CustomerID = @CustomerID and 
			b.statuscode = 'sent' and
			b.TestBlast = 'Y' and
			Convert(date, b.SendTime) between @fromdate and @toDate and
			cib.IsDeleted = 0 and 
			ci.IsDeleted = 0 and
			(@CampaignName is null or c.CampaignName = @CampaignName) and
			(@CampaignItemName is null or ci.CampaignItemName like '%' + @CampaignItemName + '%') and
			(@EmailSubject is null or b.EmailSubject like '%' + @EmailSubject + '%') and
			(@LayoutName is null or l.LayoutName like '%' + @LayoutName + '%') and
			(@GroupName is null or g.GroupName like '%' + @GroupName + '%') and
			(@BlastID is null or cib.BlastID = @BlastID) and 
			(@UserID is null or b.CreatedUserID = @userID or b.UpdatedUserID = @UserID)
		group by
				b.CustomerID, c.CampaignID, c.CampaignName, ci.CampaignItemID,ci.CampaignItemName,  b.testblast,
				 ci.CampaignItemType, ci.CampaignItemFormatType, ci.IsHidden--, b.SendTime, l.LayoutName
		order by
				6 desc			
	end
	
	else if (@TestBlast = 0)
	Begin
		SELECT	b.CustomerID, c.CampaignID, c.CampaignName, ci.CampaignItemID,ci.CampaignItemName, MAX(b.SendTime) as 'SendTime', b.testblast,
				 ci.CampaignItemType, ci.CampaignItemFormatType, ci.IsHidden, --b.SendTime, l.LayoutName,
				MAX(case when ci.CampaignItemType = 'AB' then '-- Multiple Subject lines --' else b.EmailSubject end) as emailsubject,
				case when COUNT(b.BlastID) > 1 then ' -- Multi Group Blast --' else MAX(g.GroupName)  end  as GroupName,
				SUM(b.Sendtotal) as SendTotal, COUNT(b.BlastID) as TotalBlasts, MAX(b.BlastID) as 'BlastID'
		FROM 
				Campaign c WITH (NOLOCK) 
				JOIN CampaignItem ci WITH (NOLOCK) ON c.CampaignID = ci.CampaignID
				JOIN CampaignItemBlast cib WITH (NOLOCK) ON ci.CampaignItemID = cib.CampaignItemID
				JOIN Blast b WITH (NOLOCK) ON cib.BlastID = b.BlastID 
				JOIN Groups g WITH (NOLOCK) ON b.groupID = g.groupID
				JOIN Layout l WITH (NOLOCK) ON b.LayoutID = l.LayoutID	
		WHERE 
			c.CustomerID = @CustomerID and 
			b.CustomerID = @CustomerID and 
			b.statuscode = 'sent' and
			b.TestBlast = 'N' and
			Convert(date, b.SendTime) between @fromdate and @toDate and
			cib.IsDeleted = 0 and 
			ci.IsDeleted = 0 and
			(@CampaignName is null or c.CampaignName = @CampaignName) and
			(@CampaignItemName is null or ci.CampaignItemName like '%' + @CampaignItemName + '%') and
			(@EmailSubject is null or b.EmailSubject like '%' + @EmailSubject + '%') and
			(@LayoutName is null or l.LayoutName like '%' + @LayoutName + '%') and
			(@GroupName is null or g.GroupName like '%' + @GroupName + '%') and
			(@BlastID is null or b.BlastID = @BlastID) and 
			(@UserID is null or b.CreatedUserID = @userID or b.UpdatedUserID = @UserID)
		group by
				b.CustomerID, c.CampaignID, c.CampaignName, ci.CampaignItemID,ci.CampaignItemName,  b.testblast,
				 ci.CampaignItemType, ci.CampaignItemFormatType, ci.IsHidden--, b.SendTime, l.LayoutName
		order by
				MAX(b.SendTime) desc
	End
	Else if (@TestBlast = 1)
	Begin
		SELECT	b.CustomerID, c.CampaignID, c.CampaignName, ci.CampaignItemID,ci.CampaignItemName, MAX(b.SendTime) as 'SendTime', b.testblast,
				  ci.CampaignItemType, ci.CampaignItemFormatType, ci.IsHidden, -- b.SendTime, l.LayoutName,
				MAX(case when ci.CampaignItemType = 'AB' then '-- Multiple Subject lines --' else b.EmailSubject end) as emailsubject,
				case when COUNT(b.BlastID) > 1 then ' -- Multi Group Blast --' else MAX(g.GroupName)  end  as GroupName,
				SUM(b.Sendtotal) as SendTotal, COUNT(b.BlastID) as TotalBlasts, MAX(b.BlastID) as 'BlastID'
		FROM 
				Campaign c WITH (NOLOCK) 
				JOIN CampaignItem ci WITH (NOLOCK) ON c.CampaignID = ci.CampaignID
				JOIN CampaignItemTestBlast cib WITH (NOLOCK) ON ci.CampaignItemID = cib.CampaignItemID
				JOIN Blast b WITH (NOLOCK) ON cib.BlastID = b.BlastID 
				JOIN Groups g WITH (NOLOCK) ON b.groupID = g.groupID
				JOIN Layout l WITH (NOLOCK) ON b.LayoutID = l.LayoutID	
		WHERE 
			c.CustomerID = @CustomerID and 
			b.CustomerID = @CustomerID and 
			b.statuscode = 'sent' and
			b.TestBlast = 'Y' and
			Convert(date, b.SendTime) between @fromdate and @toDate and
			cib.IsDeleted = 0 and 
			ci.IsDeleted = 0 and
			(@CampaignName is null or c.CampaignName = @CampaignName) and
			(@CampaignItemName is null or ci.CampaignItemName like '%' + @CampaignItemName + '%') and
			(@EmailSubject is null or b.EmailSubject like '%' + @EmailSubject + '%') and
			(@LayoutName is null or l.LayoutName like '%' + @LayoutName + '%') and
			(@GroupName is null or g.GroupName like '%' + @GroupName + '%') and
			(@BlastID is null or cib.BlastID = @BlastID) and
			(@UserID is null or b.CreatedUserID = @userID or b.UpdatedUserID = @UserID)
		group by
				b.CustomerID, c.CampaignID, c.CampaignName, ci.CampaignItemID,ci.CampaignItemName, b.testblast,
				 ci.CampaignItemType, ci.CampaignItemFormatType, ci.IsHidden--, b.SendTime, l.LayoutName
		order by
				Max(b.SendTime) desc
	end
	
end