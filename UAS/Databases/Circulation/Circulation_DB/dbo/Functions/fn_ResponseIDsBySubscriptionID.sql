
CREATE FUNCTION [fn_ResponseIDsBySubscriptionID] ( @SubscriptionID int)
RETURNS VARCHAR(8000)
AS
BEGIN

DECLARE @Resp VARCHAR(8000)
Select @Resp = COALESCE(@Resp,'') + CAST(hrm.ResponseID as varchar(20)) + ',' 
From HistoryResponseMap hrm With(NoLock)
 Where hrm.SubscriptionID = @SubscriptionID

RETURN @Resp

END
