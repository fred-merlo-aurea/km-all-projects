

CREATE FUNCTION [dbo].[fn_GetLastBatchID] ( @SubscriptionID int)
RETURNS VARCHAR(100)
AS
BEGIN

DECLARE @Resp VARCHAR(100)
Select @Resp = COALESCE(@Resp,'') + CAST(MAX(BatchID) as varchar(20)) + ',' 
From History hrm With(NoLock)
Where hrm.SubscriptionID = @SubscriptionID

RETURN SUBSTRING(@Resp,1,DATALENGTH(@Resp)-1)

END

