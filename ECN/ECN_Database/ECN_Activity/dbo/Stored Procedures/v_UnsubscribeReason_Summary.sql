CREATE PROCEDURE [dbo].[v_UnsubscribeReason_Summary]
	@SearchField varchar(100),
	@SearchCriteria varchar(100),
	@CustomerID int,
	@FromDate date,
	@ToDate date
AS


DECLARE @BaseChannelID int
SELECT @BaseChannelID = BaseChannelID FROM ECN5_Accounts..Customer where CustomerID = @CustomerID
DECLARE @LPAIDToUse int
IF EXISTS(
		SELECT TOP 1 LPAID 
		FROM ECN5_Accounts..LandingPageAssign with(nolock)
		WHERE BaseChannelID = @BaseChannelID AND
		ISNULL(BaseChannelDoesOverride, 0) = 1 AND
		LPID = 1
		)
	BEGIN
		IF EXISTS	
					(
					SELECT TOP 1 LPAID 
					FROM 
						ECN5_Accounts..LandingPageAssign WITH (NOLOCK) 
					WHERE
						CustomerID = @CustomerID AND
						IsNull(CustomerDoesOverride, 0) = 1 AND
						LPID = 1
					)
		BEGIN
			SELECT @LPAIDToUse = LPAID
			FROM 
				ECN5_Accounts..LandingPageAssign WITH (NOLOCK) 
			WHERE
				CustomerID = @CustomerID AND
				IsNull(CustomerDoesOverride, 0) = 1 AND
				LPID = 1
		END
		ELSE
		BEGIN
			SELECT @LPAIDToUse = LPAID
			FROM 
				ECN5_Accounts..LandingPageAssign WITH (NOLOCK) 
			WHERE
				BaseChannelID = @BaseChannelID AND
				IsNull(BaseChannelDoesOverride, 0) = 1 AND
				LPID = 1
		END
	END
	ELSE
	BEGIN
		SELECT @LPAIDToUse = LPAID
		FROM 
			ECN5_Accounts..LandingPageAssign WITH (NOLOCK) 
		WHERE
			LPID = 1 AND 
			IsNull(IsDefault, 0) = 1
	END

DECLARE @existingReasons table (Reason varchar(100))
INSERT INTO @existingReasons(Reason)
SELECT Display 
FROM ECN5_Accounts..LandingPageAssignContent lpac with(nolock)
JOIN ECN5_Accounts..LandingPageAssign lpa with(nolock) on lpac.LPAID = lpa.LPAID
WHERE lpac.LPOID = 14 and lpa.LPAID = @LPAIDToUse

SET @SearchCriteria = REPLACE(@SearchCriteria,'[','[[]')-- for escaping brackets

DECLARE @Summary table(SelectedReason varchar(500), UniqueCount int, TotalCount INT)
if(LEN(@SearchField) = 0)
BEGIN
	INSERT INTO @Summary
	select
		CASE WHEN LTRIM(Replace(SUBSTRING(bau.Comments, PATINDEX('%. Reason:%', bau.Comments), (LEN(bau.Comments) - PATINDEX('. Reason:', bau.Comments))), '. Reason: ', ''))  LIKE 'OTHER%' THEN 'Other'
		else Replace(SUBSTRING(bau.Comments, PATINDEX('%. Reason:%', bau.Comments), (LEN(bau.Comments) - PATINDEX('. Reason:', bau.Comments))), '. Reason: ', '') END,
		COUNT(distinct bau.EmailID) AS 'UniqueCount',
		COUNT(*) AS 'TotalCount'
	from 
		BlastActivityUnSubscribes bau with (nolock) 
		join ECN5_Communicator..Blast b with(nolock) on bau.BlastID = b.BlastID
		join ECN5_COMMUNICATOR..Emails e with(nolock) on bau.EmailID = e.EmailID
	where
		e.CustomerID = @CustomerID and
		bau.Comments like '%. Reason: %'
		AND CAST(bau.UnsubscribeTime as date) between @FromDate and @ToDate 
		and bau.UnsubscribeCodeID = 3
	group by
		CASE WHEN LTRIM(Replace(SUBSTRING(bau.Comments, PATINDEX('%. Reason:%', bau.Comments), (LEN(bau.Comments) - PATINDEX('. Reason:', bau.Comments))), '. Reason: ', ''))  LIKE 'OTHER%' THEN 'Other'
		else Replace(SUBSTRING(bau.Comments, PATINDEX('%. Reason:%', bau.Comments), (LEN(bau.Comments) - PATINDEX('. Reason:', bau.Comments))), '. Reason: ', '') END
END
ELSE IF(@SearchField = 'group')
BEGIN
	INSERT INTO @Summary
	select 
		CASE WHEN LTRIM(Replace(SUBSTRING(bau.Comments, PATINDEX('%. Reason:%', bau.Comments), (LEN(bau.Comments) - PATINDEX('. Reason:', bau.Comments))), '. Reason: ', ''))  LIKE 'OTHER%' THEN 'Other'
		else Replace(SUBSTRING(bau.Comments, PATINDEX('%. Reason:%', bau.Comments), (LEN(bau.Comments) - PATINDEX('. Reason:', bau.Comments))), '. Reason: ', '') END,
		COUNT(distinct bau.EmailID) AS 'UniqueCount',
		COUNT(*) AS 'TotalCount'
	from 
		BlastActivityUnSubscribes bau with (nolock) 
		join ECN5_COMMUNICATOR..Emails e with(nolock) on bau.EmailID = e.EmailID
		join ECN5_COMMUNICATOR..EmailGroups eg with(nolock) on e.EmailID = eg.EmailID
		join ECN5_COMMUNICATOR..Groups g with(nolock) on eg.GroupID = g.GroupID

	where
		e.CustomerID = @CustomerID and
		bau.Comments like '%' + CONVERT(varchar(20), eg.GroupID)+'. Reason: %'
		AND CAST(bau.UnsubscribeTime as date) between @FromDate and @ToDate 
		and g.GroupName like @SearchCriteria +'%'
		and bau.UnsubscribeCodeID = 3
	group by
		CASE WHEN LTRIM(Replace(SUBSTRING(bau.Comments, PATINDEX('%. Reason:%', bau.Comments), (LEN(bau.Comments) - PATINDEX('. Reason:', bau.Comments))), '. Reason: ', ''))  LIKE 'OTHER%' THEN 'Other'
		else Replace(SUBSTRING(bau.Comments, PATINDEX('%. Reason:%', bau.Comments), (LEN(bau.Comments) - PATINDEX('. Reason:', bau.Comments))), '. Reason: ', '') END
END
Else IF(@SearchField = 'emailsubject')
BEGIN
	INSERT INTO @Summary
	select 
		CASE WHEN LTRIM(Replace(SUBSTRING(bau.Comments, PATINDEX('%. Reason:%', bau.Comments), (LEN(bau.Comments) - PATINDEX('. Reason:', bau.Comments))), '. Reason: ', ''))  LIKE 'OTHER%' THEN 'Other'
		else Replace(SUBSTRING(bau.Comments, PATINDEX('%. Reason:%', bau.Comments), (LEN(bau.Comments) - PATINDEX('. Reason:', bau.Comments))), '. Reason: ', '') END,
		COUNT(distinct bau.EmailID) AS 'UniqueCount',
		COUNT(*) AS 'TotalCount'
	from 
		BlastActivityUnSubscribes bau with (nolock) 
		join ECN5_COMMUNICATOR..Blast b with (nolock) on bau.BlastID = b.BlastID
	where
		b.CustomerID = @CustomerID and
		bau.Comments like '%. Reason: %'
		AND CAST(bau.UnsubscribeTime as date) between @FromDate and @ToDate 
		and b.EmailSubject like '%' + @SearchCriteria +'%'
		and bau.UnsubscribeCodeID = 3
	group by
		CASE WHEN LTRIM(Replace(SUBSTRING(bau.Comments, PATINDEX('%. Reason:%', bau.Comments), (LEN(bau.Comments) - PATINDEX('. Reason:', bau.Comments))), '. Reason: ', ''))  LIKE 'OTHER%' THEN 'Other'
		else Replace(SUBSTRING(bau.Comments, PATINDEX('%. Reason:%', bau.Comments), (LEN(bau.Comments) - PATINDEX('. Reason:', bau.Comments))), '. Reason: ', '') END
END
ELSE IF(@SearchField = 'campaignitem')
BEGIN
	INSERT INTO @Summary
	select 
		CASE WHEN LTRIM(Replace(SUBSTRING(bau.Comments, PATINDEX('%. Reason:%', bau.Comments), (LEN(bau.Comments) - PATINDEX('. Reason:', bau.Comments))), '. Reason: ', ''))  LIKE 'OTHER%' THEN 'Other'
		else Replace(SUBSTRING(bau.Comments, PATINDEX('%. Reason:%', bau.Comments), (LEN(bau.Comments) - PATINDEX('. Reason:', bau.Comments))), '. Reason: ', '') END,
		COUNT(distinct bau.EmailID) AS 'UniqueCount',
		COUNT(*) AS 'TotalCount'
	from 
		BlastActivityUnSubscribes bau with (nolock) 
		join ECN5_COMMUNICATOR..Blast b with (nolock) on bau.BlastID = b.BlastID
		join ECN5_COMMUNICATOR..CampaignItemBlast cib with(nolock) on b.BlastID = cib.BlastID
		join ECN5_COMMUNICATOR..CampaignItem ci with(nolock) on cib.CampaignItemID = ci.CampaignItemID
	where
		b.CustomerID = @CustomerID and
		bau.Comments like '%. Reason: %'
		AND CAST(bau.UnsubscribeTime as date) between @FromDate and @ToDate 
		and ci.CampaignItemName like @SearchCriteria + '%'
		and bau.UnsubscribeCodeID = 3
	group by
		CASE WHEN LTRIM(Replace(SUBSTRING(bau.Comments, PATINDEX('%. Reason:%', bau.Comments), (LEN(bau.Comments) - PATINDEX('. Reason:', bau.Comments))), '. Reason: ', ''))  LIKE 'OTHER%' THEN 'Other'
		else Replace(SUBSTRING(bau.Comments, PATINDEX('%. Reason:%', bau.Comments), (LEN(bau.Comments) - PATINDEX('. Reason:', bau.Comments))), '. Reason: ', '') END
END

/*DECLARE @CursReason varchar(500), @CursCount int
DECLARE @FinalSummary table(SelectedReason varchar(500), Count int)
DECLARE mycursor cursor
FOR
SELECT 
	SelectedReason, 
	UniqueCount, 
	TotalCount 
FROM 
	@Summary

OPEN mycursor

FETCH NEXT FROM mycursor
INTO @CursReason, @CursCount
WHILE @@FETCH_STATUS = 0
BEGIN
	if(LTRIM(@CursReason) like 'Other%')
	BEGIN
		if not exists(Select top 1 * from @FinalSummary where SelectedReason = 'Other')
		BEGIN
			INSERT INTO @FinalSummary(SelectedReason, Count)
			VALUES('Other', @CursCount)
		END
		else
		BEGIN
			Update @FinalSummary
			Set Count = Count + @CursCount
			WHERE SelectedReason = 'Other'
		END

	END
	ELSE
	BEGIN
		Insert into @FinalSummary(SelectedReason, Count)
		VALUES(@CursReason, @CursCount)
	END
	FETCH NEXT FROM mycursor into @CursReason, @CursCount
END
CLOSE mycursor
DEALLOCATE mycursor

Select * from @FinalSummary
*/
Select * from @Summary
