CREATE PROCEDURE  [dbo].[MovedToActivity_spGetSampleInfoForChampion]
(
	@SampleID INT,
	@JustWinningBlastID BIT
)
AS
BEGIN        
	SET NOCOUNT ON  

--set @SampleID = 9846
--set @JustWinningBlastID = 0

	declare @ResultSet TABLE (BlastID INT, EmailSubject VARCHAR(100), SendTotal INT, bounceTotal int, Delivered int, OpenTotal int, OpenPercent DECIMAL(12,8), ClickTotal INT, ClickPercent DECIMAL(12,8), Winner BIT)  
	
	insert into @ResultSet
	select b.blastID, b.emailsubject, b.sendtotal, 0, 0, 0, 0, 0, 0, 0
	from [BLAST] b join SampleBlasts sb on b.BlastID = sb.BlastID
	WHERE sb.SampleID = @SampleID

	update @ResultSet
	set bounceTotal = inn.counts
	from @ResultSet r join 
		(select r1.blastID, COUNT(DISTINCT EmailID) as counts from @ResultSet r1 join ECN_ACTIVITY.dbo.BlastActivityBounces bo WITH (NOLOCK) on r1.BlastID = bo.BlastID group by r1.BlastID) inn on r.BlastID = inn.BlastID

	update @ResultSet
	set OpenTotal = inn.counts
	from @ResultSet r join 
		(select r1.blastID, COUNT(DISTINCT EmailID) as counts from @ResultSet r1 join ECN_ACTIVITY.dbo.BlastActivityOpens bo  WITH (NOLOCK)on r1.BlastID = bo.BlastID group by r1.BlastID) inn on r.BlastID = inn.BlastID

	update @ResultSet
	set ClickTotal = inn.counts
	from @ResultSet r join 
		(select r1.blastID, COUNT(DISTINCT EmailID) as counts from @ResultSet r1 join ECN_ACTIVITY.dbo.BlastActivityClicks bc  WITH (NOLOCK) on r1.BlastID = bc.BlastID group by r1.BlastID) inn on r.BlastID = inn.BlastID 
	
	update @ResultSet 
	set Delivered  = SendTotal - bounceTotal,
		OpenPercent = CONVERT(DECIMAL(12,8),OpenTotal) / (SendTotal - bounceTotal),
		ClickPercent = CONVERT(DECIMAL(12,8),ClickTotal) / (SendTotal - bounceTotal)
	where
		(SendTotal - bounceTotal) > 0
	
	update @ResultSet 
	set Winner = 1
	where BlastID in (select top 1 blastID from @ResultSet order by (OpenPercent+ClickPercent) desc, BlastID asc)
	
	IF @JustWinningBlastID = 1
	BEGIN
		SELECT BlastID FROM @ResultSet WHERE Winner = 1
	END
	ELSE
	BEGIN
		SELECT * FROM @ResultSet ORDER BY Winner DESC
	END

	--CREATE TABLE #ResultSet (BlastID INT, SendTotal INT, ClickTotal INT, ClickPercent DECIMAL(8,8), EmailSubject VARCHAR(100), Winner BIT)  

	--DECLARE Sample_Cursor CURSOR FOR			
	--SELECT BlastID FROM SampleBlasts WHERE SampleID = @SampleID
					
	--OPEN Sample_Cursor   
	--FETCH NEXT FROM Sample_Cursor INTO @cursorBlastID

	--WHILE @@FETCH_STATUS = 0   
	--BEGIN   
	--	SET @SendTotal = 0
	--	SET @ClickTotal = 0
	--	SET @ClickPercent = 0
	--	SET @BounceUnique = 0
	--	SET @SuccessTotal = 0
	--	SET @EmailSubject = ''

	--	SELECT @ClickTotal = COUNT(DISTINCT EmailID) 
	--	FROM EmailActivityLog
	--	WHERE BlastID = @cursorBlastID AND ActionTypeCode='click'

	--	SELECT @BounceUnique = COUNT(DISTINCT EmailID) 
	--	FROM EmailActivityLog
	--	WHERE BlastID = @cursorBlastID AND ActionTypeCode='bounce'

	--	SELECT @SuccessTotal = SuccessTotal, @EmailSubject = EmailSubject
	--	FROM Blasts
	--	WHERE BlastID = @cursorBlastID

	--	SET @SendTotal = CONVERT(int,Convert(Decimal,@SuccessTotal) - @BounceUnique)

	--	IF @SendTotal > 0
	--	BEGIN
	--		SET @ClickPercent = CONVERT(DECIMAL(6,6),CONVERT(Decimal,@ClickTotal) / CONVERT(Decimal,@SendTotal))
	--	END
	--	INSERT INTO #ResultSet(BlastID, SendTotal, ClickTotal, ClickPercent, EmailSubject, Winner) VALUES (@cursorBlastID, @SendTotal, @ClickTotal, @ClickPercent, @EmailSubject, 0)

	--	FETCH NEXT FROM Sample_Cursor INTO @cursorBlastID
	--END 
	--CLOSE Sample_Cursor   
	--DEALLOCATE Sample_Cursor

	--UPDATE #ResultSet SET Winner = 1 WHERE BlastID = (SELECT TOP 1 BlastID from #ResultSet ORDER BY ClickPercent DESC, SendTotal ASC, BlastID DESC)

	--DROP TABLE #ResultSet
	


END
