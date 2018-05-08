CREATE  Proc [dbo].[spCreateNewBlastScheduleForAll]
as 
BEGIN
	DECLARE @BlastID int
	DECLARE @PlanType VARCHAR(50)
	DECLARE @Period FLOAT
	DECLARE @BlastDay INT
	DECLARE @SendTime datetime
	DECLARE @User INT
	DECLARE @blastscheduleID INT
	DECLARE @BlastPlanID INT
	DECLARE @SampleID INT
	DECLARE @SampleAmount INT
	DECLARE @IsAmount BIT
	DECLARE @b VARCHAR(5000)
	
	
	DECLARE c_BlastCursor CURSOR FOR SELECT b.BlastID, bp.PlanType, bp.Period, bp.BlastDay, b.SendTime, b.CreatedUserID, bp.BlastPlanID, b.BlastScheduleID
	FROM 
		[BLAST] b WITH (NOLOCK)
		join BlastPlans bp WITH (NOLOCK) on b.BlastID = bp.BlastID
	WHERE 
		b.StatusCode = 'pending'
	ORDER BY b.sendtime ASC, b.blastID ASC
	
	SET @BlastID = NULL
	SET @BlastPlanID = NULL
	SET @blastscheduleID = NULL
	SET @PlanType = ''
	SET @Period = ''
	SET @BlastDay = NULL
	SET @SendTime = NULL
	SET @User = NULL	
	
	set @b = 'BlastID,BlastPlanID,BlastScheduleID|'
	
	OPEN c_BlastCursor  
	FETCH NEXT FROM c_BlastCursor INTO @BlastID, @PlanType, @Period, @BlastDay, @SendTime, @User, @BlastPlanID, @blastscheduleID

	WHILE @@FETCH_STATUS = 0  
	BEGIN		
		IF @blastscheduleID IS NOT NULL
		BEGIN
			DECLARE @Action VARCHAR(50)
			SET @Action = 'CHANGED BY BLAST ENGINE FOR BLASTID: ' + CONVERT(VARCHAR,@BlastID)
			EXEC spInsertBlastScheduleHistory @BlastID, @blastscheduleID, @Action
			DELETE BlastScheduleDays WHERE BlastScheduleID = @blastscheduleID
		END
			
		SET @SampleID = NULL
		SET @SampleAmount = NULL
		SET @IsAmount = NULL
		SELECT @SampleID = SampleID, @SampleAmount = Amount FROM SampleBlasts WHERE BlastID = @BlastID
		
		IF LOWER(ISNULL(@PlanType, '')) = 'day'
		BEGIN
			--monthly (plantype=day, blastday=dayofmonth, period=0)
			IF @blastscheduleID IS NULL
			BEGIN
				INSERT INTO BlastSchedule
					(SchedTime, SchedStartDate, SchedEndDate, Period, CreatedBy, CreatedDate)
					VALUES
					(CONVERT(TIME,@SendTime), CONVERT(DATE,@SendTime), CONVERT(DATE,DATEADD(year, 25, @SendTime)), 'm', @User, GETDATE())
				SET @blastscheduleID = @@IDENTITY
			END
			ELSE
			BEGIN
				UPDATE BlastSchedule
				SET SchedTime = CONVERT(TIME,@SendTime), SchedStartDate = CONVERT(DATE,@SendTime), SchedEndDate = CONVERT(DATE,DATEADD(year, 25, @SendTime)), Period = 'm', UpdatedBy = @User, UpdatedDate = GETDATE()
				WHERE BlastScheduleID = @blastscheduleID
			END
			INSERT INTO BlastScheduleDays
				(BlastScheduleID, DayToSend, IsAmount, Total)
				VALUES
				(@blastscheduleID, @BlastDay, @IsAmount, @SampleAmount)
			IF @SampleID IS NULL
			BEGIN
				UPDATE [BLAST] SET BlastScheduleID = @blastscheduleID WHERE BlastID = @BlastID
			END
			ELSE
			BEGIN
				UPDATE [BLAST] SET BlastScheduleID = @blastscheduleID WHERE BlastID IN (SELECT BlastID FROM [SAMPLE] WHERE SampleID = @SampleID)
				UPDATE [BLAST] SET BlastScheduleID = @blastscheduleID WHERE BlastID IN (SELECT BlastID FROM SampleBlasts WHERE SampleID = @SampleID)
			end
		end
		ELSE IF LOWER(ISNULL(@PlanType, '')) = 'period'
		BEGIN
			SET @PlanType = 'period'
			IF ISNULL(@Period, 0) = 1 and ISNULL(@BlastDay, 0) = 0
			BEGIN
				--daily (plantype=period, blastday=0, peri0d=1)
				IF @blastscheduleID IS NULL
				BEGIN
					INSERT INTO BlastSchedule
						(SchedTime, SchedStartDate, SchedEndDate, Period, CreatedBy, CreatedDate)
						VALUES
						(CONVERT(TIME,@SendTime), CONVERT(DATE,@SendTime), CONVERT(DATE,DATEADD(YEAR, 25, @SendTime)), 'd', @User, GETDATE())
					SET @blastscheduleID = @@IDENTITY
				END
				ELSE
				BEGIN
					UPDATE BlastSchedule
					SET SchedTime = CONVERT(TIME,@SendTime), SchedStartDate = CONVERT(DATE,@SendTime), SchedEndDate = CONVERT(DATE,DATEADD(year, 25, @SendTime)), Period = 'd', UpdatedBy = @User, UpdatedDate = GETDATE()
					WHERE BlastScheduleID = @blastscheduleID
				END
				IF @SampleID IS NULL
				BEGIN
					UPDATE [BLAST] SET BlastScheduleID = @blastscheduleID WHERE BlastID = @BlastID
				END
				ELSE
				BEGIN
					INSERT INTO BlastScheduleDays
						(BlastScheduleID, IsAmount, Total)
						VALUES
						(@blastscheduleID, @IsAmount, @SampleAmount)
					UPDATE [BLAST] SET BlastScheduleID = @blastscheduleID WHERE BlastID IN (SELECT BlastID FROM [SAMPLE] WHERE SampleID = @SampleID)
					UPDATE [BLAST] SET BlastScheduleID = @blastscheduleID WHERE BlastID IN (SELECT BlastID FROM SampleBlasts WHERE SampleID = @SampleID)
				END
			
			END
			ELSE IF ISNULL(@Period, 0) > 1 and ISNULL(@BlastDay, 0) = 0
			BEGIN
				--weekly (plantype=period, period=#of weeks * 7, blastday=0)
				IF @blastscheduleID IS NULL
				BEGIN
					INSERT INTO BlastSchedule
						(SchedTime, SchedStartDate, SchedEndDate, Period, CreatedBy, CreatedDate)
						VALUES
						(CONVERT(TIME,@SendTime), CONVERT(DATE,@SendTime), CONVERT(DATE,DATEADD(YEAR, 25, @SendTime)), 'w', @User, GETDATE())
					SET @blastscheduleID = @@IDENTITY
				END
				ELSE
				BEGIN
					UPDATE BlastSchedule
					SET SchedTime = CONVERT(TIME,@SendTime), SchedStartDate = CONVERT(DATE,@SendTime), SchedEndDate = CONVERT(DATE,DATEADD(year, 25, @SendTime)), Period = 'w', UpdatedBy = @User, UpdatedDate = GETDATE()
					WHERE BlastScheduleID = @blastscheduleID
				END
				IF @SampleID IS NULL
				BEGIN
					INSERT INTO BlastScheduleDays
						(BlastScheduleID, DayToSend, IsAmount, Total, Weeks)
						VALUES
						(@blastscheduleID, datepart(weekday,@SendTime) - 1, 0, 100, @Period / 7)
					UPDATE [BLAST] SET BlastScheduleID = @blastscheduleID WHERE BlastID = @BlastID
				END
				ELSE
				BEGIN
					INSERT INTO BlastScheduleDays
						(BlastScheduleID, DayToSend, IsAmount, Total, Weeks)
						VALUES
						(@blastscheduleID, datepart(weekday,@SendTime) - 1, @IsAmount, @SampleAmount, @Period / 7)
					UPDATE [BLAST] SET BlastScheduleID = @blastscheduleID WHERE BlastID IN (SELECT BlastID FROM [SAMPLE] WHERE SampleID = @SampleID)
					UPDATE [BLAST] SET BlastScheduleID = @blastscheduleID WHERE BlastID IN (SELECT BlastID FROM SampleBlasts WHERE SampleID = @SampleID)
				END
			END
		END
		
		set @b = @b + convert(varchar(10),@BlastID) + ',' + convert(varchar(10),@BlastPlanID) + ',' + convert(varchar(10),@blastscheduleID) + '|'
		
		SET @BlastID = NULL
		SET @BlastPlanID = NULL
		SET @blastscheduleID = NULL
		SET @PlanType = ''
		SET @Period = ''
		SET @BlastDay = NULL
		SET @SendTime = NULL
		SET @User = NULL	
		FETCH NEXT FROM c_BlastCursor INTO @BlastID, @PlanType, @Period, @BlastDay, @SendTime, @User, @BlastPlanID, @blastscheduleID
	END
	CLOSE c_BlastCursor  
	DEALLOCATE c_BlastCursor
	
	EXEC msdb.dbo.sp_send_dbmail
		@profile_name='SQLAdmin', 
		@recipients='sunil.theenathayalu@TeamKM.com,bill.hipps@TeamKM.com', 
		@importance='High',
		@body_format = 'HTML',
		@subject='[BLAST] modified with new schedule', 
		@body=@b
END
