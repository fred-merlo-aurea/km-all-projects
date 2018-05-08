CREATE PROCEDURE [dbo].[e_InboundRequestLog_Save]
@InboundRequestLogID int,
@ClientID int,
@RequestStartDateTime datetime2(7),
@RequestEndDateTime datetime2(7),
@RequestFromIP varchar(15),
@ServiceMethodID int,
@ErrorMessage varchar,
@RequestData varchar,
@ResponseData varchar
AS
	IF (@InboundRequestLogID > 0)
		BEGIN
			UPDATE InboundRequestLog
			SET ClientID = @ClientID,
				RequestStartDateTime = @RequestStartDateTime,
				RequestEndDateTime = @RequestEndDateTime,
				RequestFromIP = @RequestFromIP,
				ServiceMethodID = @ServiceMethodID,
				ErrorMessage = @ErrorMessage,
				RequestData = @RequestData,
				ResponseData = @ResponseData
			WHERE InboundRequestLogID = @InboundRequestLogID
		END
	ELSE
		BEGIN
			INSERT INTO InboundRequestLog (ClientID,RequestStartDateTime,RequestEndDateTime,RequestFromIP,ServiceMethodID,ErrorMessage,RequestData,ResponseData)
			VALUES(@ClientID,@RequestStartDateTime,@RequestEndDateTime,@RequestFromIP,@ServiceMethodID,@ErrorMessage,@RequestData,@ResponseData)
		END