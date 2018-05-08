CREATE PROCEDURE [dbo].[e_ApplicationLog_Save]
@ApplicationLogId int = 0,
@Application varchar(250) = '',
@Severity varchar(250) = '',
@SourceMethod varchar(250) = NULL,
@Exception varchar(max) = '',
@LogNote varchar(max) = null,
@IsBug bit = 'false',
@IsUserSubmitted bit = 'false',
@ClientId int = NULL,
@SubmittedBy varchar(250) = NULL,
@SubmittedByEmail varchar(100) = NULL,
@IsFixed bit = 'false',
@FixedDate date = NULL,
@FixedTime time = NULL,
@FixedBy varchar(50) = NULL,
@FixedNote varchar(750) = NULL,
@LogAddedDate date,
@LogAddedTime time,
@LogUpdatedDate date = null,
@LogUpdatedTime time = null,
@NotificationSent bit = 'false'
AS
	declare @codeTypeId int = (select codeTypeId from CodeType with(nolock) where CodeTypeName = 'Severity')
	declare @severityCodeId int = (select codeId from Code with(nolock) where CodeName = @Severity and CodetypeId = @codeTypeId)
	declare @applicationId int = (select applicationId from Application with(nolock) where ApplicationName = @Application)

	IF @ApplicationLogId > 0
		BEGIN
			IF @LogUpdatedDate IS NULL
				BEGIN
					SET @LogUpdatedDate = GETDATE();
				END
			IF @LogUpdatedTime IS NULL
				BEGIN
					SET @LogUpdatedTime = GETDATE();
				END
				
			UPDATE ApplicationLog
			SET ApplicationId = @applicationId,
				SeverityCodeId = @severityCodeId,
				SourceMethod = @SourceMethod,
				Exception = @Exception,
				LogNote = @LogNote,
				IsBug = @IsBug,
				IsUserSubmitted = @IsUserSubmitted,
				ClientId = @ClientId,
				SubmittedBy = @SubmittedBy,
				SubmittedByEmail = @SubmittedByEmail,
				IsFixed = @IsFixed,
				FixedDate = @FixedDate,
				FixedTime = @FixedTime,
				FixedBy = @FixedBy,
				FixedNote = @FixedNote,
				NotificationSent = @NotificationSent,
				LogUpdatedDate = @LogUpdatedDate,
				LogUpdatedTime = @LogUpdatedTime
			WHERE ApplicationLogId = @ApplicationLogId;
			
			SELECT @ApplicationLogId;
		END
	ELSE
		BEGIN
			IF @LogAddedDate IS NULL
				BEGIN
					SET @LogAddedDate = GETDATE();
				END
			IF @LogAddedTime IS NULL
				BEGIN
					SET @LogAddedTime = GETDATE();
				END
				
			INSERT INTO ApplicationLog (ApplicationId,SeverityCodeId,SourceMethod,Exception,LogNote,IsBug,IsUserSubmitted,ClientId,SubmittedBy,SubmittedByEmail,IsFixed,FixedDate,FixedTime,
										FixedBy,FixedNote,LogAddedDate,LogAddedTime,LogUpdatedDate,LogUpdatedTime,NotificationSent)
			VALUES(@applicationId,@severityCodeId,@SourceMethod,@Exception,@LogNote,@IsBug,@IsUserSubmitted,@ClientId,@SubmittedBy,@SubmittedByEmail,@IsFixed,@FixedDate,@FixedTime,
										@FixedBy,@FixedNote,@LogAddedDate,@LogAddedTime,@LogUpdatedDate,@LogUpdatedTime,@NotificationSent);SELECT @@IDENTITY;
		END
