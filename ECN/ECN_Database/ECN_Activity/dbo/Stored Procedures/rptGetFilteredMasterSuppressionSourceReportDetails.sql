CREATE PROCEDURE [dbo].[rptGetFilteredMasterSuppressionSourceReportDetails]
	@CustomerID int,
	@UnsubscribeCode VARCHAR(50),
	@FromDate varchar(20),
	@ToDate varchar(20)
AS
--DECLARE
--@CustomerID int = 1,
--@UnsubscribeCode VARCHAR(50) = 'All',
--@FromDate varchar(20) = '1/27/2015',
--@ToDate varchar(20) = '4/27/2015'

BEGIN
	SET NOCOUNT ON
	Set @FromDate = @FromDate + ' 00:00:00'
	Set @ToDate = @ToDate + ' 23:59:59'
	
	SELECT emailaddress, isnull(UC.UnsubscribeCode, '') AS SuppressionCode, bs.Comments as 'Reason', MAX(isnull(bs.UnsubscribeTime,ISNULL(eg.LastChanged, eg.CreatedOn))) AS SuppressedDateTime
	
	FROM ECN5_COMMUNICATOR..emailgroups eg 
	JOIN ecn5_communicator..emails e 
		ON e.emailID = eg.emailID 
	LEFT OUTER JOIN BlastActivityUnSubscribes bs 
		ON eg.EmailID = bs.EmailID 
	LEFT OUTER JOIN UnsubscribeCodes uc 
		ON uc.UnsubscribeCodeID = bs.UnsubscribeCodeID 
	WHERE groupID = (SELECT groupID 
					FROM ECN5_COMMUNICATOR..Groups 
					WHERE CustomerID = @CustomerID 
					AND MasterSupression = 1) 
    and ISNULL(bs.UnsubscribeTime, ISNULL(eg.LastChanged, eg.CreatedOn)) between @FromDate and @ToDate
		AND ISNULL(UnsubscribeCode,'') = CASE WHEN @UnsubscribeCode = 'All' then ISNULL(UnsubscribeCode,'') WHEN @UnsubscribeCode = 'Blank' then '' ELSE @UnsubscribeCode END
	GROUP BY groupID, emailaddress, uc.UnsubscribeCode, bs.Comments
	ORDER BY SuppressedDateTime
	
	SET NOCOUNT OFF
END