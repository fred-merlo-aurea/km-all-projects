--exec e_Blast_Select_Search 'api', 4496, 49195, 0, 'Pending', '1/1/2013', '1/1/2014', 1
CREATE PROCEDURE [dbo].[e_Blast_Select_Search] 
(
@EmailSubject varchar(255) = NULL,
@UserID int = NULL,
@GroupID int = NULL,
@IsTest bit = NULL,
@StatusCode varchar(50) = NULL,
@ModifiedFrom datetime = NULL,
@ModifiedTo datetime = NULL,
@CampaignID int = NULL,
@CampaignName varchar(100) = NULL,
@CampaignItemName varchar(255) = NULL,
@CustomerID int
)
AS


DECLARE @SelectSQL varchar(4000)
if @IsTest is null or @IsTest = 0
BEGIN
SET @SelectSQL = '	SELECT b.*
					FROM Blast b WITH(NOLOCK)
					JOIN CampaignItemBlast cib with(NOLOCK) on b.BlastID = cib.BlastID		
					JOIN CampaignItem ci with(Nolock) on cib.CampaignItemID = ci.CampaignItemID
					JOIN Campaign c with(NOLOCK) on ci.CampaignID = c.CampaignID		
					WHERE b.CustomerID = ' + convert(varchar,@CustomerID)
END
ELSE
BEGIN
SET @SelectSQL = '	SELECT b.*
					FROM Blast b WITH(NOLOCK)
					JOIN CampaignItemTestBlast cib with(NOLOCK) on b.BlastID = cib.BlastID	
					JOIN CampaignItem ci with(NOLOCK) on cib.CampaignItemID = ci.CampaignItemID
					JOIN Campaign c with(NOLOCK) on ci.CampaignID = c.CampaignID			
					WHERE b.CustomerID = ' + convert(varchar,@CustomerID)
END
IF @EmailSubject IS NOT NULL
BEGIN
	SET @EmailSubject = '''%' + UPPER(REPLACE(@EmailSubject,'''','''''')) + '%'''
	SET @SelectSQL = @SelectSQL + ' AND UPPER(b.EmailSubject) LIKE ' + @EmailSubject
END
IF @UserID IS NOT NULL
BEGIN
	SET @SelectSQL = @SelectSQL + ' AND (b.UpdatedUserID = ' + convert(varchar,@UserID) + ' OR b.CreatedUserID = ' + convert(varchar,@UserID) + ')'
END
IF @GroupID IS NOT NULL
BEGIN
	SET @SelectSQL = @SelectSQL + ' AND b.GroupID = ' + convert(varchar,@GroupID)
END
IF @IsTest IS NOT NULL
BEGIN
	IF @IsTest = 0
	BEGIN
		SET @SelectSQL = @SelectSQL + ' AND UPPER(ISNULL(b.TestBlast,''N'')) = ''N'''
	END
	ELSE
	BEGIN
		SET @SelectSQL = @SelectSQL + ' AND UPPER(ISNULL(b.TestBlast,''N'')) = ''Y'''
	END
END
IF @StatusCode IS NOT NULL
BEGIN
	SET @SelectSQL = @SelectSQL + ' AND UPPER(b.StatusCode) = ''' + UPPER(@StatusCode) + ''''
END
IF @ModifiedFrom IS NOT NULL
BEGIN
	SET @SelectSQL = @SelectSQL + ' AND (b.CreatedDate >= ''' + convert(varchar,@ModifiedFrom) +  ''' OR b.UpdatedDate >= ''' + convert(varchar,@ModifiedFrom) + ''')'
END
IF @ModifiedTo IS NOT NULL
BEGIN
	SET @SelectSQL = @SelectSQL + ' AND (b.CreatedDate <= ''' + convert(varchar,@ModifiedTo) +  ''' OR b.UpdatedDate <= ''' + convert(varchar,@ModifiedTo) + ''')'
END

IF @CampaignID IS NOT NULL
BEGIN
	SET @SelectSQL = @SelectSQL + ' AND c.CampaignID = ' + convert(varchar(20),@CampaignID) 
END

IF @CampaignName IS NOT NULL
BEGIN
	SET @SelectSQL = @SelectSQL + ' AND c.CampaignName like ''%' + @CampaignName + '%'''
END

IF @CampaignItemName Is Not Null
BEGIN
	SET @SelectSQL = @SelectSQL + ' AND ci.CampaignItemName like ''%' + @CampaignItemName + '%'''
END

EXEC(@SelectSQL)