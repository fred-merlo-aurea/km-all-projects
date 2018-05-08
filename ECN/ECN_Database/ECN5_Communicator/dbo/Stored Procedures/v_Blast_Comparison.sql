--select top 5 * from Blast where StatusCode = 'sent' order by BlastID desc
--select * from CampaignItemBlast where BlastID = 1568907
--select * from CampaignItem where CampaignItemID = 11410
CREATE PROCEDURE [dbo].[v_Blast_Comparison] 
(
	@CustomerID int,
	@UserID int = NULL,
	@StartTime datetime,
	@EndTime datetime,
	@GroupID int = NULL,
	@CampaignID int = NULL 
	--set @CustomerID = 1
	--set @UserID = 4496
	--set @StartTime = '2013-02-10 12:36:00.723'
	--set @EndTime = '2013-02-14 12:36:00.723'
	--set @GroupID = 49195
	--set @CampaignID = 469
)
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @Select VARCHAR(500)
	SET @Select = 'SELECT 
		b.BlastID, CONVERT(VARCHAR,b.SendTime,101)+'' - ''+ b.EmailSubject + '' [''+CONVERT(VARCHAR,b.BlastID)+'']'' AS EmailSubject
	FROM  
		[Blast] b with (nolock)
		JOIN [CampaignItemBlast] cib with (nolock) on b.BlastID = cib.BlastID
		JOIN [CampaignItem] ci with (nolock) on cib.CampaignItemID = ci.CampaignItemID
	WHERE '
	print @select
	DECLARE @Where VARCHAR(500)
	SET @Where = 'b.CustomerID = ' + Convert(varchar(20),@CustomerID) + ' AND 
		b.StatusCode = ''sent'' AND 
		b.SendTime BETWEEN ''' + Convert(varchar(20),@StartTime) + ''' AND ''' + Convert(varchar(20),@EndTime) + ''''
		
	IF @GroupID IS NOT NULL
	BEGIN
		SET @Where += ' AND b.GroupID = ' + Convert(varchar(20),@GroupID)
	END
	IF @CampaignID IS NOT NULL
	BEGIN
		SET @Where += ' AND ci.CampaignID = ' + Convert(varchar(20),@CampaignID)
	END
	IF @UserID IS NOT NULL
	BEGIN
		SET @Where += ' AND (b.CreatedUserID = ' + Convert(varchar(20),@UserID) + ' OR IsNull(b.UpdatedUserID, 0) = ' + Convert(varchar(20),@UserID) + ')'
	END
            
    EXEC(@Select + @Where + ' ORDER BY b.SendTime DESC')	

END