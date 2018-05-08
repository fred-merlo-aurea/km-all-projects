CREATE  PROC [dbo].[e_Blast_Save] 
(
	@BlastID int = null,
	@CustomerID int = null,
	@EmailSubject varchar(255) = null,
	@EmailFrom varchar(100) = null,
	@EmailFromName varchar(100) = null,
	@SendTime datetime = null,
	@AttemptTotal int = null,
	@SendTotal int = null,
	@SendBytes int = null,
	@StatusCode varchar(50) = null,
	@BlastType varchar(50) = null,
	@CodeID int = null,
	@LayoutID int = null,
	@GroupID int = null,
	@FinishTime datetime = null,
	@SuccessTotal int = null,
	@BlastLog varchar(2000) = null,
	@UserID int = null,
	@FilterID int = null,
	@Spinlock char(1),
	@ReplyTo varchar(100) = null,
	@TestBlast varchar(1) = null,
	@BlastFrequency varchar(50) = null,
	@RefBlastID varchar(2000) = null,
	@BlastSuppression varchar(2000) = null,
	@AddOptOuts_to_MS bit = null,
	@DynamicFromName varchar(100) = null,
	@DynamicFromEmail varchar(100) = null,
	@DynamicReplyToEmail varchar(100) = null,
	@BlastEngineID int = null,
	@HasEmailPreview bit = null,
	@BlastScheduleID int = null,
	@OverrideAmount int = null,
	@OverrideIsAmount bit = null,
	@StartTime datetime = null,
	@SMSOptInTotal int = null,
	@SmartSegmentID int = null,
	@NodeID varchar(50) = null,
	@SampleID int = null,
	@EnableCacheBuster bit = null,
	@IgnoreSuppression bit = null
)
AS 
BEGIN
	IF @BlastID is NULL or @BlastID <= 0
	BEGIN
		INSERT INTO Blast
		(
			CustomerID,EmailSubject,EmailFrom,EmailFromName,SendTime,AttemptTotal,SendTotal,SendBytes,
			StatusCode,BlastType,CodeID,LayoutID,GroupID,FinishTime,SuccessTotal,BlastLog,CreatedUserID,
			FilterID,Spinlock,ReplyTo,TestBlast,BlastFrequency,RefBlastID,BlastSuppression,AddOptOuts_to_MS,
			DynamicFromName,DynamicFromEmail,DynamicReplyToEmail,BlastEngineID,HasEmailPreview,BlastScheduleID,
			OverrideAmount,OverrideIsAmount,StartTime,SMSOptInTotal,SmartSegmentID,SampleID,NodeID,CreatedDate, EnableCacheBuster, IgnoreSuppression
		)
		VALUES
		(
			@CustomerID,@EmailSubject,@EmailFrom,@EmailFromName,@SendTime,@AttemptTotal,@SendTotal,@SendBytes,
			@StatusCode,@BlastType,@CodeID,@LayoutID,@GroupID,@FinishTime,@SuccessTotal,@BlastLog,@UserID,
			@FilterID,@Spinlock,@ReplyTo,@TestBlast,@BlastFrequency,@RefBlastID,@BlastSuppression,@AddOptOuts_to_MS,
			@DynamicFromName,@DynamicFromEmail,@DynamicReplyToEmail,@BlastEngineID,@HasEmailPreview,@BlastScheduleID,
			@OverrideAmount,@OverrideIsAmount,@StartTime,@SMSOptInTotal,@SmartSegmentID,@SampleID,@NodeID,GETDATE(),@EnableCacheBuster, @IgnoreSuppression
		)
		SET @BlastID = @@IDENTITY
	END
	ELSE
	BEGIN
		UPDATE Blast
			SET CustomerID=@CustomerID,EmailSubject=@EmailSubject,EmailFrom=@EmailFrom,EmailFromName=@EmailFromName,SendTime=@SendTime,AttemptTotal=@AttemptTotal,SendTotal=@SendTotal,SendBytes=@SendBytes,
				StatusCode=@StatusCode,BlastType=@BlastType,CodeID=@CodeID,LayoutID=@LayoutID,GroupID=@GroupID,FinishTime=@FinishTime,SuccessTotal=@SuccessTotal,BlastLog=@BlastLog,UpdatedUserID=@UserID,
				FilterID=@FilterID,Spinlock=@Spinlock,ReplyTo=@ReplyTo,TestBlast=@TestBlast,BlastFrequency=@BlastFrequency,RefBlastID=@RefBlastID,BlastSuppression=@BlastSuppression,AddOptOuts_to_MS=@AddOptOuts_to_MS,
				DynamicFromName=@DynamicFromName,DynamicFromEmail=@DynamicFromEmail,DynamicReplyToEmail=@DynamicReplyToEmail,BlastEngineID=@BlastEngineID,HasEmailPreview=@HasEmailPreview,BlastScheduleID=@BlastScheduleID,
				OverrideAmount=@OverrideAmount,OverrideIsAmount=@OverrideIsAmount,StartTime=@StartTime,SMSOptInTotal=@SMSOptInTotal,SmartSegmentID=@SmartSegmentID,SampleID=@SampleID,NodeID=@NodeID,UpdatedDate=GETDATE(), EnableCacheBuster = @EnableCacheBuster, IgnoreSuppression = @IgnoreSuppression
		WHERE
			BlastID = @BlastID
	END

	SELECT @BlastID
END