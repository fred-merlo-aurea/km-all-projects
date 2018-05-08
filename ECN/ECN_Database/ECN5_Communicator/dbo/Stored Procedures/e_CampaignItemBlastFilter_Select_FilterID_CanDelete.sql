-- success (e.g. 1): The campaign item filter exists for the filter associated with a blast in Active, Send or Pending status
--fail - 0 
CREATE proc [dbo].[e_CampaignItemBlastFilter_Select_FilterID_CanDelete] 
	@FilterID INT
AS
BEGIN
	--if there is an associated Scheduled Report that uses the filterid - fail
	IF EXISTS(
	
		SELECT 
			TOP 1 rs.ReportScheduleID
		FROM 
			ReportSchedule rs WITH (NOLOCK) JOIN 
			ReportQueue rq WITH (NOLOCK) ON rs.ReportScheduleID = rq.ReportScheduleID 
		WHERE 
			rs.ReportParameters LIKE '<FilterID>%'+CONVERT(VARCHAR(MAX),@FilterID)+'%' AND
			rs.IsDeleted = 0 AND 
			rq.[Status] = 'pending'
	 
	UNION
	--if there is filter or suppression filter that uses the filterid - fail
	
		SELECT 
			TOP 1 CampaignItemBlastFilterID
		FROM 
			CampaignItemBlastFilter WITH (NOLOCK) 
		WHERE
			FilterID = @FilterID AND
			IsDeleted = 0
	)
	SELECT 0 ELSE SELECT 1
END
GO

