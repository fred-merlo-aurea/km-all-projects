CREATE PROCEDURE [dbo].[v_UnsubscribeReason_Detail_Report]
	@SearchField VARCHAR(100),
	@SearchCriteria VARCHAR(100),
	@CustomerID INT,
	@FromDate date,
	@ToDate date,
	@Reason VARCHAR(100)
AS

SET NOCOUNT ON 
--SET @FromDate = @FromDate + ' 00:00:00.000'
--SET @ToDate = @ToDate + ' 23:59:59.990'

CREATE TABLE #Results(
	ROWNUM INT IDENTITY(1,1),
	CampaignItemName  VARCHAR(255),
	EmailSubject  VARCHAR(255),
	GroupName  VARCHAR(100),
	EmailAddress  VARCHAR(255),
	UnsubscribeTime date,
	SelectedReason VARCHAR(255)
	)

SET @SearchCriteria = REPLACE(@SearchCriteria,'[','[[]')-- for escaping brackets

IF(LEN(@SearchField) = 0)
BEGIN
/* 
	2015-11-12 MTK 
	For all NON-ZERO Blasts:
	Changed Join to Groups criteria to default the Blast GroupId if no GroupId is found the comments string from BlastActivityUnsubscribes
	Old Join <g.GroupID = CASE WHEN...ELSE 0 END >
	New Join <g.GroupID = CASE WHEN...ELSE b.GroupId END >
	This Join is in all 4 sections, @SearchField = blank,group,emailsubject,campaignitem 
	The source of the missing data is being addressed and this should not be neccesary at some future point
*/
	INSERT INTO #Results (
		CampaignItemName,
		EmailSubject,
		GroupName,
		EmailAddress ,
		UnsubscribeTime ,
		SelectedReason
		)
	SELECT	
		ISNULL(ci.CampaignItemName,'') AS 'CampaignItemName', 
		ISNULL(b.EmailSubject,'') AS 'EmailSubject', 
		ISNULL(g.GroupName,'') AS 'GroupName',
		EmailAddress, 
		bau.UnsubscribeTime, 
		CASE WHEN LTRIM(REPLACE(SUBSTRING(bau.Comments, PATINDEX('%. Reason:%', bau.Comments), (LEN(bau.Comments) - PATINDEX('. Reason:', bau.Comments))), '. Reason: ', ''))  LIKE 'OTHER%' THEN 'Other'
		ELSE REPLACE(SUBSTRING(bau.Comments, PATINDEX('%. Reason:%', bau.Comments), (LEN(bau.Comments) - PATINDEX('. Reason:', bau.Comments))), '. Reason: ', '') END
	FROM 
		BlastActivityUnSubscribes bau WITH(NOLOCK)
		JOIN ECN5_COMMUNICATOR..Blast b WITH(NOLOCK) ON bau.BlastID = b.blastid
		JOIN ECN5_COMMUNICATOR..Emails e WITH(NOLOCK)on bau.EmailID = e.EmailID
		JOIN ECN5_COMMUNICATOR..Groups g WITH(NOLOCK)on  g.GroupID = CASE WHEN CHARINDEX('FOR GROUP:',bau.Comments)>0 AND CHARINDEX('. Reason: ', bau.Comments) > 1 THEN SUBSTRING(SUBSTRING( bau.Comments, PATINDEX('% FOR GROUP: %',bau.Comments) + 12, 17), 0, PATINDEX('% Reason: %',SUBSTRING( bau.Comments, PATINDEX('% FOR GROUP: %',bau.Comments) + 12, 17))-1) ELSE b.GroupId END
		JOIN ECN5_COMMUNICATOR..CampaignItemBlast cib WITH(NOLOCK) ON b.BlastID = cib.BlastID
		JOIN ECN5_COMMUNICATOR..CampaignItem ci WITH(NOLOCK) ON cib.CampaignItemID = ci.CampaignItemID
	WHERE
		e.CustomerID = @CustomerID 
		AND bau.Comments LIKE '%Reason: ' + @Reason + '%' 
		AND CAST(bau.UnsubscribeTime as date) between @FromDate and @ToDate 
		AND bau.UnsubscribeCodeID = 3 
		AND bau.BlastID > 0

	UNION 

	SELECT 
		'' AS 'CampaignItemName', 
		'' AS 'EmailSubject', 
		ISNULL(g.GroupName,'') AS 'GroupName', 
		EmailAddress, 
		bau.UnsubscribeTime, 
		CASE WHEN LTRIM(REPLACE(SUBSTRING(bau.Comments, PATINDEX('%. Reason:%', bau.Comments), (LEN(bau.Comments) - PATINDEX('. Reason:', bau.Comments))), '. Reason: ', ''))  LIKE 'OTHER%' THEN 'Other'
		ELSE REPLACE(SUBSTRING(bau.Comments, PATINDEX('%. Reason:%', bau.Comments), (LEN(bau.Comments) - PATINDEX('. Reason:', bau.Comments))), '. Reason: ', '') END
	FROM 
		BlastActivityUnSubscribes bau WITH(NOLOCK)
		JOIN ECN5_COMMUNICATOR..Emails e WITH(NOLOCK) ON bau.EmailID = e.EmailID
		JOIN ECN5_COMMUNICATOR..EmailGroups eg WITH(NOLOCK) ON e.EmailID = eg.EmailID
		JOIN ECN5_COMMUNICATOR..Groups g WITH(NOLOCK)on  g.GroupID = CASE WHEN CHARINDEX('FOR GROUP ID:',bau.Comments)>0 AND CHARINDEX('. Reason: ', bau.Comments) > 1 THEN SUBSTRING(SUBSTRING( bau.Comments, PATINDEX('% FOR GROUP ID: %',bau.Comments) + 15, 20), 0, PATINDEX('% Reason: %',SUBSTRING( bau.Comments, PATINDEX('% FOR GROUP ID: %',bau.Comments) + 15, 20))-1) ELSE 0 END
	WHERE
		e.CustomerID = @CustomerID 
		AND	bau.Comments LIKE '%' + CONVERT(varchar(20), eg.GroupID)+ '%Reason: ' + @Reason + '%' 
		AND CAST(bau.UnsubscribeTime as date) between @FromDate and @ToDate 
		AND bau.UnsubscribeCodeID = 3 
		AND bau.BlastID = 0
   
	SELECT 
		*
	FROM 
		#Results


END
ELSE IF(@SearchField = 'group')

BEGIN
	INSERT INTO #Results (
		CampaignItemName,
		EmailSubject,
		GroupName,
		EmailAddress ,
		UnsubscribeTime ,
		SelectedReason
		)
	SELECT	
		ISNULL(ci.CampaignItemName,'') AS 'CampaignItemName', 
		ISNULL(b.EmailSubject,'') AS 'EmailSubject', 
		ISNULL(g.GroupName,'') AS 'GroupName',
		EmailAddress, 
		bau.UnsubscribeTime, 
		CASE WHEN LTRIM(Replace(SUBSTRING(bau.Comments, PATINDEX('%. Reason:%', bau.Comments), (LEN(bau.Comments) - PATINDEX('. Reason:', bau.Comments))), '. Reason: ', ''))  LIKE 'OTHER%' THEN 'Other'
		else Replace(SUBSTRING(bau.Comments, PATINDEX('%. Reason:%', bau.Comments), (LEN(bau.Comments) - PATINDEX('. Reason:', bau.Comments))), '. Reason: ', '') END AS SelectedReason
	FROM 
		BlastActivityUnSubscribes bau WITH(NOLOCK)
		JOIN ECN5_COMMUNICATOR..Blast b WITH(NOLOCK) ON bau.BlastID = b.blastid
		JOIN ECN5_COMMUNICATOR..Emails e WITH(NOLOCK)on bau.EmailID = e.EmailID
		JOIN ECN5_COMMUNICATOR..Groups g WITH(NOLOCK)on  g.GroupID = CASE WHEN CHARINDEX('FOR GROUP:',bau.Comments)>0 AND CHARINDEX('. Reason: ', bau.Comments) > 1 THEN SUBSTRING(SUBSTRING( bau.Comments, PATINDEX('% FOR GROUP: %',bau.Comments) + 12, 17), 0, PATINDEX('% Reason: %',SUBSTRING( bau.Comments, PATINDEX('% FOR GROUP: %',bau.Comments) + 12, 17))-1) ELSE b.GroupId END
		JOIN ECN5_COMMUNICATOR..CampaignItemBlast cib WITH(NOLOCK) ON b.BlastID = cib.BlastID
		JOIN ECN5_COMMUNICATOR..CampaignItem ci WITH(NOLOCK) ON cib.CampaignItemID = ci.CampaignItemID
	WHERE
		e.CustomerID = @CustomerID 
		AND	bau.Comments LIKE '%' + CONVERT(varchar(20), b.GroupID)+ '. Reason: ' + @Reason + '%' 
		AND CAST(bau.UnsubscribeTime as date) between @FromDate and @ToDate 
		AND	g.GroupName LIKE @SearchCriteria + '%'
		AND bau.UnsubscribeCodeID = 3 
		AND bau.BlastID > 0

	UNION 

	SELECT 
		'' AS 'CampaignItemName', 
		'' AS 'EmailSubject', 
		ISNULL(g.GroupName,'') AS 'GroupName', 
		EmailAddress, 
		bau.UnsubscribeTime, 
		CASE WHEN LTRIM(REPLACE(SUBSTRING(bau.Comments, PATINDEX('%. Reason:%', bau.Comments), (LEN(bau.Comments) - PATINDEX('. Reason:', bau.Comments))), '. Reason: ', ''))  LIKE 'OTHER%' THEN 'Other'
		else REPLACE(SUBSTRING(bau.Comments, PATINDEX('%. Reason:%', bau.Comments), (LEN(bau.Comments) - PATINDEX('. Reason:', bau.Comments))), '. Reason: ', '') END AS SelectedReason
	from 
		BlastActivityUnSubscribes bau WITH(NOLOCK)
		JOIN ECN5_COMMUNICATOR..Emails e WITH(NOLOCK)on bau.EmailID = e.EmailID
		JOIN ECN5_COMMUNICATOR..EmailGroups eg WITH(NOLOCK) ON e.EmailID = eg.EmailID
		JOIN ECN5_COMMUNICATOR..Groups g WITH(NOLOCK)on  g.GroupID = CASE WHEN CHARINDEX('FOR GROUP ID:',bau.Comments)>0 AND CHARINDEX('. Reason: ', bau.Comments) > 1 THEN SUBSTRING(SUBSTRING( bau.Comments, PATINDEX('% FOR GROUP ID: %',bau.Comments) + 15, 20), 0, PATINDEX('% Reason: %',SUBSTRING( bau.Comments, PATINDEX('% FOR GROUP ID: %',bau.Comments) + 15, 20))-1) ELSE 0 END
	where
		e.CustomerID = @CustomerID 
		AND	bau.Comments LIKE '%' + CONVERT(varchar(20), eg.GroupID)+ '. Reason: ' + @Reason + '%' 
		AND CAST(bau.UnsubscribeTime as date) between @FromDate and @ToDate 
		AND	g.GroupName LIKE @SearchCriteria + '%'
		AND bau.UnsubscribeCodeID = 3 
		AND bau.BlastID = 0
	order by 
		'EmailSubject' asc, 
		'SelectedReason' asc
	
	SELECT 
		*
	FROM 
		#Results

END
ELSE IF(@SearchField = 'emailsubject')
BEGIN
	INSERT INTO #Results (
		CampaignItemName,
		EmailSubject,
		GroupName,
		EmailAddress ,
		UnsubscribeTime ,
		SelectedReason
		)
	SELECT	
		ISNULL(ci.CampaignItemName,'') AS 'CampaignItemName', 
		ISNULL(b.EmailSubject,'') AS 'EmailSubject', 
		ISNULL(g.GroupName,'') AS 'GroupName',
		EmailAddress, 
		bau.UnsubscribeTime, 
		CASE WHEN LTRIM(Replace(SUBSTRING(bau.Comments, PATINDEX('%. Reason:%', bau.Comments), (LEN(bau.Comments) - PATINDEX('. Reason:', bau.Comments))), '. Reason: ', ''))  LIKE 'OTHER%' THEN 'Other'
		else Replace(SUBSTRING(bau.Comments, PATINDEX('%. Reason:%', bau.Comments), (LEN(bau.Comments) - PATINDEX('. Reason:', bau.Comments))), '. Reason: ', '') END AS SelectedReason
	FROM 
		BlastActivityUnSubscribes bau WITH(NOLOCK)
		JOIN ECN5_COMMUNICATOR..Blast b WITH(NOLOCK) ON bau.BlastID = b.blastid
		JOIN ECN5_COMMUNICATOR..Emails e WITH(NOLOCK)on bau.EmailID = e.EmailID
		JOIN ECN5_COMMUNICATOR..Groups g WITH(NOLOCK)on  g.GroupID = CASE WHEN CHARINDEX('FOR GROUP:',bau.Comments)>0 AND CHARINDEX('. Reason: ', bau.Comments) > 1 THEN SUBSTRING(SUBSTRING( bau.Comments, PATINDEX('% FOR GROUP: %',bau.Comments) + 12, 17), 0, PATINDEX('% Reason: %',SUBSTRING( bau.Comments, PATINDEX('% FOR GROUP: %',bau.Comments) + 12, 17))-1) ELSE b.GroupId END
		JOIN ECN5_COMMUNICATOR..CampaignItemBlast cib WITH(NOLOCK) ON b.BlastID = cib.BlastID
		JOIN ECN5_COMMUNICATOR..CampaignItem ci WITH(NOLOCK) ON cib.CampaignItemID = ci.CampaignItemID
	WHERE
		b.CustomerID = @CustomerID 
		AND	bau.Comments LIKE '%Reason: ' + @Reason + '%' 
		AND CAST(bau.UnsubscribeTime as date) between @FromDate and @ToDate 
		AND b.EmailSubject LIKE '%' + @SearchCriteria +'%'
		AND bau.UnsubscribeCodeID = 3
	ORDER BY 
		ISNULL(b.EmailSubject,'') asc, 
		CASE WHEN LTRIM(Replace(SUBSTRING(bau.Comments, PATINDEX('%. Reason:%', bau.Comments), (LEN(bau.Comments) - PATINDEX('. Reason:', bau.Comments))), '. Reason: ', ''))  LIKE 'OTHER%' THEN 'Other'
		else Replace(SUBSTRING(bau.Comments, PATINDEX('%. Reason:%', bau.Comments), (LEN(bau.Comments) - PATINDEX('. Reason:', bau.Comments))), '. Reason: ', '') END ASC

	SELECT 
		*
	FROM 
		#Results

END
ELSE IF(@SearchField = 'campaignitem')
BEGIN
	INSERT INTO #Results (
		CampaignItemName,
		EmailSubject,
		GroupName,
		EmailAddress ,
		UnsubscribeTime ,
		SelectedReason
		)
      SELECT 
		ISNULL(ci.CampaignItemName,'') AS 'CampaignItemName', 
		ISNULL(b.EmailSubject,'') AS 'EmailSubject', 
		ISNULL(g.GroupName,'') AS 'GroupName', 
		EmailAddress, 
		bau.UnsubscribeTime, 
		CASE WHEN LTRIM(REPLACE(SUBSTRING(bau.Comments, PATINDEX('%. Reason:%', bau.Comments), (LEN(bau.Comments) - PATINDEX('. Reason:', bau.Comments))), '. Reason: ', ''))  LIKE 'OTHER%' THEN 'Other'
		ELSE REPLACE(SUBSTRING(bau.Comments, PATINDEX('%. Reason:%', bau.Comments), (LEN(bau.Comments) - PATINDEX('. Reason:', bau.Comments))), '. Reason: ', '') END AS SelectedReason
      FROM 
		BlastActivityUnSubscribes bau WITH(NOLOCK)
		JOIN ECN5_COMMUNICATOR..Blast b WITH(NOLOCK)on bau.BlastID = b.BlastID
		JOIN ECN5_COMMUNICATOR..Emails e WITH(NOLOCK)on bau.EmailID = e.EmailID
		JOIN ECN5_COMMUNICATOR..Groups g WITH(NOLOCK)on g.GroupID = CASE WHEN CHARINDEX('FOR GROUP:',bau.Comments)>0 AND CHARINDEX('. Reason: ', bau.Comments) > 1 THEN SUBSTRING(SUBSTRING( bau.Comments, PATINDEX('% FOR GROUP: %',bau.Comments) + 12, 17), 0, PATINDEX('% Reason: %',SUBSTRING( bau.Comments, PATINDEX('% FOR GROUP: %',bau.Comments) + 12, 17))-1) ELSE b.GroupId END
		JOIN ECN5_COMMUNICATOR..CampaignItemBlast cib WITH(NOLOCK) ON b.BlastID = cib.BlastID
		JOIN ECN5_COMMUNICATOR..CampaignItem ci WITH(NOLOCK) ON cib.CampaignItemID = ci.CampaignItemID
      WHERE
		b.CustomerID = @CustomerID 
		AND bau.Comments LIKE '%Reason: ' + @Reason + '%' 
		AND CAST(bau.UnsubscribeTime as date) between @FromDate and @ToDate 
		AND ci.CampaignItemName LIKE @SearchCriteria +'%'
		AND bau.UnsubscribeCodeID = 3
      ORDER BY 
		ISNULL(b.EmailSubject,'') asc, 
		CASE WHEN LTRIM(REPLACE(SUBSTRING(bau.Comments, PATINDEX('%. Reason:%', bau.Comments), (LEN(bau.Comments) - PATINDEX('. Reason:', bau.Comments))), '. Reason: ', ''))  LIKE 'OTHER%' THEN 'Other'
		ELSE REPLACE(SUBSTRING(bau.Comments, PATINDEX('%. Reason:%', bau.Comments), (LEN(bau.Comments) - PATINDEX('. Reason:', bau.Comments))), '. Reason: ', '') END ASC

	SELECT 
		*
	FROM 
		#Results
END

DROP TABLE #Results
