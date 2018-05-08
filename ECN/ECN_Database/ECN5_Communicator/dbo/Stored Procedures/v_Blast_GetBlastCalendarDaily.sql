CREATE PROCEDURE [dbo].[v_Blast_GetBlastCalendarDaily]  
	@CustomerID int, 
	@FromDate date = null,
	@ToDate  date = null,
	@campaignID int = 0,				
	@BlastType VARCHAR(1) = '',   
	@SubjectSearch VARCHAR(50) = '',
	@GroupSearch VARCHAR(50) = '',
	@SentUserID int	= -1
	--set @BlastType = 'N'
	--set @FromDate = '3/1/2016'
	--set @ToDate = '3/1/2016'
	--set @CustomerID = 3570
AS
BEGIN


	if @FromDate is null
		select @FromDate = convert(date, DATEADD(dd, -1 * DAY(getdate()-1), getdate())) 
		
	if @ToDate is null
		select @ToDate = convert(date, getdate())

	if @BlastType='N'
	begin
		SELECT
			 b.SendTime, b.BlastID, g.GroupName, b.StatusCode, b.EmailSubject, b.SendTotal	
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
			 (b.EmailSubject like '%' + @SubjectSearch + '%' or len(@SubjectSearch) = 0) and 
			 (g.GroupName like '%' + @GroupSearch + '%' or len(@GroupSearch) = 0)  
		ORDER BY													
			b.SendTime, b.StatusCode  
		DESC
	end
	else
	begin
		SELECT
			 b.SendTime, b.BlastID, g.GroupName, b.StatusCode, b.EmailSubject, b.SendTotal	
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
			 (b.EmailSubject like '%' + @SubjectSearch + '%' or len(@SubjectSearch) = 0) and 
			 (g.GroupName like '%' + @GroupSearch + '%' or len(@GroupSearch) = 0)  
		ORDER BY													
			b.SendTime, b.StatusCode  
		DESC
	end
END