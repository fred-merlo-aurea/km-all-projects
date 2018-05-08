CREATE PROCEDURE [dbo].[e_ApiLog_Save]
@ApiLogId int,
@ClientID int, 
@AccessKey uniqueidentifier,
@RequestFromIP varchar(50),
@ApiId int,
@Entity varchar(100),
@Method varchar(100),
@ErrorMessage varchar(max),
@RequestData varchar(max),
@ResponseData varchar(max),
@RequestStartDate date,
@RequestStartTime time(7),
@RequestEndDate date,
@RequestEndTime time(7)
as
	IF @ApiLogId > 0
		BEGIN
			IF @RequestEndDate IS NULL
				BEGIN
					SET @RequestEndDate = GETDATE();
				END
			IF @RequestEndTime IS NULL
				BEGIN
					SET @RequestEndTime = GETDATE();
				END
				
			UPDATE ApiLog
			SET ClientID = @ClientID,
				AccessKey = @AccessKey,
				RequestFromIP = @RequestFromIP,
				ApiId = @ApiId,
				Entity = @Entity,
				Method = @Method,
				ErrorMessage = @ErrorMessage,
				RequestData = @RequestData,
				ResponseData = @ResponseData,
				RequestEndDate = @RequestEndDate,
				RequestEndTime = @RequestEndTime
			WHERE ApiLogId = @ApiLogId;
			
			SELECT @ApiLogId;
		END
	ELSE
		BEGIN
			IF @RequestStartDate IS NULL
				BEGIN
					SET @RequestStartDate = GETDATE();
				END
			IF @RequestStartTime IS NULL
				BEGIN
					SET @RequestStartTime = GETDATE();
				END
				
			INSERT INTO ApiLog (ClientID,AccessKey,RequestFromIP,ApiId,Entity,Method,ErrorMessage,RequestData,ResponseData,RequestStartDate,RequestStartTime,RequestEndDate,RequestEndTime)
			VALUES(@ClientID,@AccessKey,@RequestFromIP,@ApiId,@Entity,@Method,@ErrorMessage,@RequestData,@ResponseData,@RequestStartDate,@RequestStartTime,@RequestEndDate,@RequestEndTime);SELECT @@IDENTITY;
		END