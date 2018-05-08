CREATE  Proc [dbo].[spCreateNewBlastSchedule] (
@BlastID INT)
as 
BEGIN
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
	DECLARE @b VARCHAR(1000)
	DECLARE @Result VARCHAR(50)
	
	set @b = '<b>BlastID : </b>' + convert(varchar(10),@BlastID) + '<BR>'

	SET @BlastPlanID = NULL
	SET @blastscheduleID = NULL
	SELECT @PlanType = bp.PlanType, @Period = bp.Period, @BlastDay = bp.BlastDay, @SendTime = b.SendTime, @User = b.CreatedUserID, @BlastPlanID = bp.BlastPlanID, @blastscheduleID = b.BlastScheduleID
	FROM 
		[BLAST] b WITH (NOLOCK)
		join BlastPlans bp WITH (NOLOCK) on b.BlastID = bp.BlastID
	WHERE 
		b.BlastID = @BlastID
		
	--if we have the old blast plan add a new schedule
	IF @BlastPlanID IS NOT NULL
	BEGIN
		--if we already have a schedule, log and delete the schedule and start fresh as the blast plan may have changed
		set @b = @b + '<b>BlastPlanID : </b>' + convert(varchar(10),@BlastPlanID) + '<BR>'
		IF @blastscheduleID IS NOT NULL
		BEGIN
			set @b = @b + '<b>BlastScheduleID : </b>' + convert(varchar(10),@blastscheduleID) + '<BR>'
			DECLARE @Action VARCHAR(50)
			SET @Action = 'CHANGED BY BLAST ENGINE FOR BLASTID: ' + CONVERT(VARCHAR,@BlastID)
			EXEC spInsertBlastScheduleHistory @BlastID, @blastscheduleID, @Action
			DELETE BlastScheduleDays WHERE BlastScheduleID = @blastscheduleID
			--DELETE BlastSchedule WHERE BlastScheduleID = @blastscheduleID
		END
		
		--SET @blastscheduleID = NULL
		SET @SampleID = NULL
		SET @SampleAmount = NULL
		SET @IsAmount = NULL
		SELECT @SampleID = SampleID, @SampleAmount = Amount FROM SampleBlasts WHERE BlastID = @BlastID
		
		--see if it's a sample blast
		IF @SampleID IS NOT NULL
		BEGIN
			set @b = @b + '<b>SampleID : </b>' + convert(varchar(10),@SampleID) + '<BR>'
			SET @IsAmount = 1
		END
		
		IF LOWER(ISNULL(@PlanType, '')) = 'day'
		BEGIN
			set @b = @b + '<b>PlanType : </b>monthly<BR>'
			--monthly (plantype=day, blastday=dayofmonth, period=0)
			IF @blastscheduleID IS NULL
			BEGIN
				SET @Result = 'INSERT'
				INSERT INTO BlastSchedule
					(SchedTime, SchedStartDate, SchedEndDate, Period, CreatedBy, CreatedDate)
					VALUES
					(CONVERT(TIME,@SendTime), CONVERT(DATE,@SendTime), CONVERT(DATE,DATEADD(year, 25, @SendTime)), 'm', @User, GETDATE())
				SET @blastscheduleID = @@IDENTITY
				set @b = @b + '<b>BlastScheduleID : </b>' + convert(varchar(10),@blastscheduleID) + '<BR>'
			END
			ELSE
			BEGIN
				SET @Result = 'UPDATE'
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
				set @b = @b + '<b>PlanType : </b>daily<BR>'
				--daily (plantype=period, blastday=0, peri0d=1)
				IF @blastscheduleID IS NULL
				BEGIN
					SET @Result = 'INSERT'
					INSERT INTO BlastSchedule
						(SchedTime, SchedStartDate, SchedEndDate, Period, CreatedBy, CreatedDate)
						VALUES
						(CONVERT(TIME,@SendTime), CONVERT(DATE,@SendTime), CONVERT(DATE,DATEADD(YEAR, 25, @SendTime)), 'd', @User, GETDATE())
					SET @blastscheduleID = @@IDENTITY
					set @b = @b + '<b>BlastScheduleID : </b>' + convert(varchar(10),@blastscheduleID) + '<BR>'
				END
				ELSE
				BEGIN
					SET @Result = 'UPDATE'
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
				set @b = @b + '<b>PlanType : </b>weekly<BR>'
				--weekly (plantype=period, period=#of weeks * 7, blastday=0)
				IF @blastscheduleID IS NULL
				BEGIN
					SET @Result = 'INSERT'
					INSERT INTO BlastSchedule
						(SchedTime, SchedStartDate, SchedEndDate, Period, CreatedBy, CreatedDate)
						VALUES
						(CONVERT(TIME,@SendTime), CONVERT(DATE,@SendTime), CONVERT(DATE,DATEADD(YEAR, 25, @SendTime)), 'w', @User, GETDATE())
					SET @blastscheduleID = @@IDENTITY
					set @b = @b + '<b>BlastScheduleID : </b>' + convert(varchar(10),@blastscheduleID) + '<BR>'
				END
				ELSE
				BEGIN
					SET @Result = 'UPDATE'
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
		SET @b = @b + '<b>Result : </b>' + @Result + '<BR>'
		EXEC msdb.dbo.sp_send_dbmail
		@profile_name='SQLAdmin', 
		@recipients='sunil.theenathayalu@TeamKM.com,bill.hipps@TeamKM.com', 
		@importance='High',
		@body_format = 'HTML',
		@subject='Canon Blast Engine called spCreateNewBlastSchedule with a Blast Plan', 
		@body=@b
	END
	--ELSE
	--BEGIN
	--	set @Result = 'No blast plan so no schedule created'
	--END
	
	--SET @b = @b + '<b>Result : </b>' + @Result + '<BR>'
	--EXEC msdb.dbo.sp_send_dbmail
	--@profile_name='SQLAdmin', 
	--@recipients='sunil.theenathayalu@TeamKM.com,bill.hipps@TeamKM.com', 
	--@importance='High',
	--@body_format = 'HTML',
	--@subject='Canon Blast Engine called spCreateNewBlastSchedule with a Blast Plan', 
	--@body=@b
END
