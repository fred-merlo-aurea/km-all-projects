CREATE PROCEDURE  [dbo].[spGetSampleInfoForChampion]
(
	@SampleID INT,
	@JustWinningBlastID BIT,
	@CustomerID INT,
	@ABWinnerType varchar(10)
)
AS
BEGIN        
	SET NOCOUNT ON  

--set @SampleID = 6582
--set @JustWinningBlastID = 0

	declare @ResultSet TABLE (BlastID INT, CustomerID INT, EmailSubject VARCHAR(1000),FromEmail varchar(255), ReplyTo varchar(255), FromName varchar(255), SendTotal INT, bounceTotal int, Delivered int, OpenTotal int, OpenPercent DECIMAL(12,4), ClickTotal INT, ClickPercent DECIMAL(12,4), Winner BIT)  
	
	insert into @ResultSet
	select blastID, customerID, emailsubject, EmailFrom, ReplyTo, EmailFromName, sendtotal, 0, 0, 0, 0, 0, 0, 0
	from ecn5_communicator.dbo.Blast
	WHERE SampleID = @SampleID and BlastType = 'sample' and CustomerID = @CustomerID and StatusCode <> 'Deleted'

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
		OpenPercent = CONVERT(DECIMAL(12,4),OpenTotal * 100) / (SendTotal - bounceTotal),
		ClickPercent = CONVERT(DECIMAL(12,4),ClickTotal * 100) / (SendTotal - bounceTotal)
	where
		(SendTotal - bounceTotal) > 0
	
	if @ABWinnerType = 'opens'
	begin
		DECLARE @Blast1Perc DECIMAL(12,4),@Blast2Perc DECIMAL(12,4)
		Select top 1 @Blast1Perc = OpenPercent FROM @ResultSet order by OpenPercent, BlastID
		Select top 1 @Blast2Perc = OpenPercent FROM @ResultSet order by OpenPercent desc, BlastID desc
		if(@Blast1Perc = @Blast2Perc)
		BEGIN
		--Opens perc is the same so look at CLick perc
			Select top 1 @Blast1Perc = ClickPercent FROM @ResultSet order by ClickPercent, BlastID
			Select top 1 @Blast2Perc = ClickPercent FROM @ResultSet order by ClickPercent desc, BlastID desc
			if(@Blast1Perc = @Blast2Perc)
			BEGIN
			--Click perc is the same so look at Open totals
				DECLARE @Blast1Num int, @Blast2Num int
				Select top 1 @Blast1Num = OpenTotal FROM @ResultSet order by OpenTotal, BlastID
				Select top 1 @Blast2Num = OpenTotal FROM @ResultSet order by OpenTotal desc, BlastID desc
				if(@Blast1Num = @Blast2Num)
				BEGIN
				--Open Totals are the same so look at click totals
					Select top 1 @Blast1Num = ClickTotal FROM @ResultSet order by ClickTotal , BlastID
					Select top 1 @Blast2Num = ClickTotal FROM @ResultSet order by ClickTotal desc, BlastID desc
					if(@Blast1Num = @Blast2Num)
					BEGIN
						--Everything is the same so just grab BlastA
						update @ResultSet 
						set Winner = 1
						where BlastID = (Select top 1 BlastID from @ResultSet order by BlastID asc)
					END
					ELSE
					BEGIN
						update @ResultSet 
						set Winner = 1
						where BlastID = (Select top 1 BlastID from @ResultSet order by ClickTotal desc , BlastID)
					END

				END
				ELSE
				BEGIN
					update @ResultSet 
					set Winner = 1
					where BlastID = (Select top 1 BlastID from @ResultSet  order by OpenTotal desc, BlastID)
				END
			END
			ELSE
			BEGIN
				update @ResultSet 
				set Winner = 1
				where BlastID in (select top 1 BlastID FROM @ResultSet order by ClickPercent desc, BlastID)
			END

		END
		ELSE
		BEGIN
			update @ResultSet 
			set Winner = 1
			where BlastID in (select top 1 blastID from @ResultSet order by (OpenPercent) desc)	
		END
		
		
	end
	else
	begin
	--Determine by clicks perc
		SET @Blast1Perc = 0.0
		SET @Blast2Perc = 0.0
		Select top 1 @Blast1Perc = ClickPercent FROM @ResultSet order by ClickPercent, BlastID
		Select top 1 @Blast2Perc = ClickPercent FROM @ResultSet order by ClickPercent desc, BlastID desc
		if(@Blast1Perc = @Blast2Perc)
		BEGIN
			--Clicks perc is the same so look at Open Perc
			Select top 1 @Blast1Perc = OpenPercent FROM @ResultSet order by OpenPercent, BlastID
			Select top 1 @Blast2Perc = OpenPercent FROM @ResultSet order by OpenPercent desc, BlastID desc
			if(@Blast1Perc = @Blast2Perc)
			BEGIN
				--Opens perc is the same so look at click totals
				SET @Blast1Num = 0
				SET @Blast2Num = 0
				Select top 1 @Blast1Num = ClickTotal FROM @ResultSet order by ClickTotal, BlastID
				Select top 1 @Blast2Num = ClickTotal FROM @ResultSet order by ClickTotal desc, BlastID desc
				if(@Blast1Num = @Blast2Num)
				BEGIN
					--Click totals are the same so look at open totals
					Select top 1 @Blast1Num = OpenTotal FROM @ResultSet order by OpenTotal , BlastID
					Select top 1 @Blast2Num = OpenTotal FROM @ResultSet order by OpenTotal desc, BlastID desc
					if(@Blast1Num = @Blast2Num)
					BEGIN
						--Everything is the same so just grab BlastA
						update @ResultSet 
						set Winner = 1
						where BlastID = (Select top 1 BlastID from @ResultSet order by BlastID asc)
					END
					ELSE
					BEGIN
						update @ResultSet 
						set Winner = 1
						where BlastID = (Select top 1 BlastID from @ResultSet order by OpenTotal desc , BlastID)
					END

				END
				ELSE
				BEGIN
					update @ResultSet 
					set Winner = 1
					where BlastID = (Select top 1 BlastID from @ResultSet  order by ClickTotal desc, BlastID)
				END
			END
			ELSE
			BEGIN
				update @ResultSet 
				set Winner = 1
				where BlastID in (select top 1 BlastID FROM @ResultSet order by OpenPercent desc, BlastID)
			END
		END
		ELSE
		BEGIN
			update @ResultSet 
			set Winner = 1
			where BlastID in (select top 1 blastID from @ResultSet order by (ClickPercent) desc)	
		END	
	end
	
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