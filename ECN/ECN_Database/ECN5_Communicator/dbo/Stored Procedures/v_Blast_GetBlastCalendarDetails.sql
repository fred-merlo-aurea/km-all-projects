CREATE PROCEDURE [dbo].[v_Blast_GetBlastCalendarDetails] 
	@CustomerID int = 0,
	@IsSummary bit = 0,
	@FromDate date = null,
	@ToDate  date = null,
	@campaignID int = 0,				
	@BlastType VARCHAR(1) = '',    --TESTblast or LIVE blast or all
	@SubjectSearch VARCHAR(50) = '',
	@GroupSearch VARCHAR(50) = '',
	@SentUserID int	= -1
	--set @BlastType = 'N'
	--set @FromDate = '3/1/2016'
	--set @ToDate = '3/31/2016'
	--set @CustomerID = 3570
AS
BEGIN

	if @FromDate is null
	BEGIN
		select @FromDate = convert(date, DATEADD(dd, -1 * DAY(getdate()-1), getdate()))
	END 
		
	if @ToDate is null
	BEGIN
		select @ToDate = convert(date, getdate())
	END

	if @BlastType='N'
	begin
		if @IsSummary = 1
		BEGIN
			SELECT
				 convert(date,b.SendTime) as 'SendDate', 
				 COUNT(case when b.StatusCode = 'sent' then ISNULL(b.BlastID,0) end) as 'SentTotal', 
				 COUNT(case when b.StatusCode = 'pending' then ISNULL(b.BlastID,0) end) as 'Pending',         
				 COUNT(case when b.StatusCode = 'active' then ISNULL(b.BlastID,0) end) as 'Active',
				 b.StatusCode   
			FROM 
				 ecn5_accounts..Customer cu WITH (NOLOCK)
				 join Blast b WITH (NOLOCK) on b.CustomerID = cu.CustomerID 
				 left outer join Groups g WITH (NOLOCK)  ON b.GroupID = g.GroupID	
				 left outer join CampaignItemBlast cib WITH (NOLOCK)  ON cib.BlastID =b.BlastID	
				 left outer join CampaignItem ci WITH (NOLOCK)  ON cib.CampaignItemID = ci.CampaignItemID
				 left outer join Campaign cm WITH (NOLOCK)  ON ci.CampaignID = cm.CampaignID
				 		 
			WHERE 
				 b.CustomerID = @CustomerID and 
				 b.StatusCode in ('sent','active','pending') and
				 convert(date,b.SendTime) >= @FromDate and 
				 convert(date,b.SendTime) <= @ToDate  and
				 (b.TestBlast = @BlastType or len(@BlastType) = 0) and
				 (cm.CampaignID = @campaignID   or @campaignID = 0) and
				 ((b.CreatedUserID = @SentUserID or b.UpdatedUserID = @SentUserID) or @SentUserID = -1) and
				 (b.EmailSubject like '%' + @SubjectSearch + '%' or len(@SubjectSearch) = 0) and 
				 (g.GroupName like '%' + @GroupSearch + '%' or len(@GroupSearch) = 0) 			 
			GROUP BY
				 convert(date,b.SendTime),b.StatusCode   	
			ORDER BY 
				 convert(date,b.SendTime),b.StatusCode 
			DESC
		END
		ELSE
		BEGIN
			SELECT
				 b.SendTime, Convert(varchar(10),b.SendTime,101) as 'SendDate', b.BlastID, g.GroupName, b.StatusCode, b.EmailSubject, b.SendTotal	
			FROM 
				ecn5_accounts..Customer cu WITH (NOLOCK)
				 join Blast b WITH (NOLOCK) on b.CustomerID = cu.CustomerID
				 left outer join  Groups g WITH (NOLOCK) on g.GroupID = b.GroupID
				 left outer join CampaignItemBlast cib WITH (NOLOCK)  ON cib.BlastID =b.BlastID	
				 left outer join CampaignItem ci WITH (NOLOCK)  ON cib.CampaignItemID = ci.CampaignItemID
				 left outer join Campaign cm WITH (NOLOCK)  ON ci.CampaignID = cm.CampaignID
			WHERE 
				 b.CustomerID = @CustomerID and 
				 b.StatusCode in ('sent','active','pending') and
				 convert(date,b.SendTime) >= @FromDate and 
				 convert(date,b.SendTime) <= @ToDate  and
				 (b.TestBlast = @BlastType or len(@BlastType) = 0) and
				 (cm.CampaignID = @campaignID   or @campaignID = 0) and
				 ((b.CreatedUserID = @SentUserID or b.UpdatedUserID = @SentUserID) or @SentUserID = -1) and 
				 (b.EmailSubject like '%' + @SubjectSearch + '%' or len(@SubjectSearch) = 0)  and 
				 (g.GroupName like '%' + @GroupSearch + '%' or len(@GroupSearch) = 0)  
			ORDER BY													
				b.SendTime,b.StatusCode 
			DESC
		END	
	end
	else
	BEGIN
		if @IsSummary = 1
			BEGIN
				SELECT
					 convert(date,b.SendTime) as 'SendDate', 
					 COUNT(case when b.StatusCode = 'sent' then ISNULL(b.BlastID,0) end) as 'SentTotal', 
					 COUNT(case when b.StatusCode = 'pending' then ISNULL(b.BlastID,0) end) as 'Pending',         
					 COUNT(case when b.StatusCode = 'active' then ISNULL(b.BlastID,0) end) as 'Active',
					 b.StatusCode   
				FROM 
					 ecn5_accounts..Customer cu WITH (NOLOCK)
					 join Blast b WITH (NOLOCK) on b.CustomerID = cu.CustomerID 
					 left outer join Groups g WITH (NOLOCK)  ON b.GroupID = g.GroupID	
					 left outer join CampaignItemTestBlast cib WITH (NOLOCK)  ON cib.BlastID =b.BlastID	
					 left outer join CampaignItem ci WITH (NOLOCK)  ON cib.CampaignItemID = ci.CampaignItemID
					 left outer join Campaign cm WITH (NOLOCK)  ON ci.CampaignID = cm.CampaignID
					 		 
				WHERE 
					 b.CustomerID = @CustomerID and 
					 b.StatusCode in ('sent','active','pending') and
					 convert(date,b.SendTime) >= @FromDate and 
					 convert(date,b.SendTime) <= @ToDate  and
					 (b.TestBlast = @BlastType or len(@BlastType) = 0) and
					  (cm.CampaignID = @campaignID   or @campaignID = 0) and
					 ((b.CreatedUserID = @SentUserID or b.UpdatedUserID = @SentUserID) or @SentUserID = -1) and
					 (b.EmailSubject like '%' + @SubjectSearch + '%' or len(@SubjectSearch) = 0) and 
					 (g.GroupName like '%' + @GroupSearch + '%' or len(@GroupSearch) = 0) 			 
				GROUP BY
					 convert(date,b.SendTime),b.StatusCode   	
				ORDER BY 
					 convert(date,b.SendTime),b.StatusCode 
				DESC
			END
		ELSE
			BEGIN
				SELECT
					 b.SendTime, Convert(varchar(10),b.SendTime,101) as 'SendDate', b.BlastID, g.GroupName, b.StatusCode, b.EmailSubject, b.SendTotal	
				FROM 
					 ecn5_accounts..Customer cu WITH (NOLOCK)
					 join Blast b WITH (NOLOCK) on b.CustomerID = cu.CustomerID
					 left outer join  Groups g WITH (NOLOCK) on g.GroupID = b.GroupID
					 left outer join CampaignItemTestBlast cib WITH (NOLOCK)  ON cib.BlastID =b.BlastID	
					 left outer join CampaignItem ci WITH (NOLOCK)  ON cib.CampaignItemID = ci.CampaignItemID
					 left outer join Campaign cm WITH (NOLOCK)  ON ci.CampaignID = cm.CampaignID
				WHERE 
					 b.CustomerID = @CustomerID and 
					 b.StatusCode in ('sent','active','pending') and
					 convert(date,b.SendTime) >= @FromDate and 
					 convert(date,b.SendTime) <= @ToDate  and
					 (b.TestBlast = @BlastType or len(@BlastType) = 0) and
					 (cm.CampaignID = @campaignID   or @campaignID = 0) and
					 ((b.CreatedUserID = @SentUserID or b.UpdatedUserID = @SentUserID) or @SentUserID = -1) and 
					 (b.EmailSubject like '%' + @SubjectSearch + '%' or len(@SubjectSearch) = 0)  and 
					 (g.GroupName like '%' + @GroupSearch + '%' or len(@GroupSearch) = 0)  
				ORDER BY													
					b.SendTime,b.StatusCode 
				DESC
			END
	
	END

END