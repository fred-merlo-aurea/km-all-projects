CREATE PROCEDURE [dbo].[e_UserAuthorizationLog_Save]
@UserAuthLogID int,
@AuthSource varchar(50),
@AuthMode varchar(50),
@AuthAttemptDate date,
@AuthAttemptTime time(7),
@IsAuthenticated bit,
@IpAddress varchar(15),
@AuthUserName varchar(50),
@AuthAccessKey uniqueidentifier,
@ServerVariables varchar(max),
@AppVersion varchar(50),
@UserID int,
@LogOutDate date,
@LogOutTime time(7)
AS

IF @UserAuthLogID > 0
	BEGIN
		IF @LogOutDate IS NULL
			BEGIN
				SET @LogOutDate = GETDATE();
			END
			
		IF @LogOutTime IS NULL
			BEGIN
				SET @LogOutTime = GETDATE();
			END

		UPDATE UserAuthorizationLog
		SET AuthSource = @AuthSource,
			AuthMode = @AuthMode,
			AuthAttemptDate = @AuthAttemptDate,
			AuthAttemptTime = @AuthAttemptTime,
			IsAuthenticated = @IsAuthenticated,
			IpAddress = @IpAddress,
			AuthUserName = @AuthUserName,
			AuthAccessKey = @AuthAccessKey,
			ServerVariables = @ServerVariables,
			AppVersion = @AppVersion,
			UserID = @UserID,
			LogOutDate = @LogOutDate,
			LogOutTime = @LogOutTime
		WHERE UserAuthLogID = @UserAuthLogID;

		SELECT @UserAuthLogID;
	END
ELSE
	BEGIN
		IF @AuthAttemptDate IS NULL
			BEGIN
				SET @AuthAttemptDate = GETDATE();
			END
			
		IF @AuthAttemptTime IS NULL
			BEGIN
				SET @AuthAttemptTime = GETDATE();
			END

		INSERT INTO UserAuthorizationLog (AuthSource,AuthMode,AuthAttemptDate,AuthAttemptTime,IsAuthenticated,IpAddress,AuthUserName,AuthAccessKey,ServerVariables,AppVersion,UserID,LogOutDate,LogOutTime)
		VALUES(@AuthSource,@AuthMode,@AuthAttemptDate,@AuthAttemptTime,@IsAuthenticated,@IpAddress,@AuthUserName,@AuthAccessKey,@ServerVariables,@AppVersion,@UserID,@LogOutDate,@LogOutTime);SELECT @@IDENTITY;
	END
