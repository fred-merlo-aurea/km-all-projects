CREATE PROCEDURE [dbo].[e_ApplicationLog_Save]
@LogID int = NULL,
@ApplicationID int = NULL,
@SeverityID int = NULL,
@SourceMethod varchar(250) = NULL,
@Exception text = NULL,
@LogNote text = NULL,
@IsBug bit = NULL,
@IsUserSubmitted bit = NULL,
@GDCharityID int = NULL,
@ECNCustomerID int = NULL,
@SubmittedBy nvarchar(250) = NULL,
@SubmittedByEmail nvarchar(100) = NULL,
@IsFixed bit = NULL,
@FixedDate date = NULL,
@FixedTime time = NULL,
@FixedBy nvarchar(50) = NULL,
@FixedNote nvarchar(750) = NULL,
@NotificationSent bit = NULL
AS
BEGIN
	IF @LogID = NULL or @LogID <= 0
	BEGIN
		INSERT INTO ApplicationLog
		(
			ApplicationID, SeverityID, SourceMethod, Exception, LogNote, IsBug, IsUserSubmitted, GDCharityID, ECNCustomerID, SubmittedBy,
			SubmittedByEmail, IsFixed, FixedDate, FixedTime, FixedBy, FixedNote, LogAddedDate, LogAddedTime, NotificationSent
		)
		VALUES
		(
			@ApplicationID, @SeverityID, @SourceMethod, @Exception, @LogNote, @IsBug, @IsUserSubmitted, @GDCharityID, @ECNCustomerID, @SubmittedBy,
			@SubmittedByEmail, @IsFixed, @FixedDate, @FixedTime, @FixedBy, @FixedNote, CONVERT(DATE,GETDATE()),CONVERT(TIME,GETDATE()), @NotificationSent
		)
		SET @LogID = @@IDENTITY
	END
	ELSE
	BEGIN
		UPDATE ApplicationLog
			SET ApplicationID=@ApplicationID, SeverityID=@SeverityID, SourceMethod=@SourceMethod, Exception=@Exception, LogNote=@LogNote, IsBug=@IsBug,
			IsUserSubmitted=@IsUserSubmitted, GDCharityID=@GDCharityID, ECNCustomerID=@ECNCustomerID, SubmittedBy=@SubmittedBy,
			SubmittedByEmail=@SubmittedByEmail, IsFixed=@IsFixed, FixedDate=@FixedDate, FixedTime=@FixedTime, FixedBy=@FixedBy, FixedNote=@FixedNote,
			LogUpdatedDate=CONVERT(DATE,GETDATE()), LogUpdatedTime=CONVERT(TIME,GETDATE()), NotificationSent=@NotificationSent
		WHERE
			LogID = @LogID
	END

	SELECT @LogID
END
