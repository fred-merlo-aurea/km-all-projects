
CREATE FUNCTION [dbo].[fn_ResponseIDs_SubscriptionID_Response_Active] ( @SubscriptionID int, @ResponseName varchar(75))
RETURNS VARCHAR(8000)
AS
BEGIN

DECLARE @Resp VARCHAR(8000)
Select @Resp = COALESCE(@Resp,'') + CAST(r.ResponseCode as varchar(20)) + ',' 
From SubscriptionResponseMap srm With(NoLock)
JOIN Response r ON srm.ResponseID = r.ResponseID
 Where srm.SubscriptionID = @SubscriptionID and r.ResponseName = @ResponseName and srm.IsActive = 1

RETURN REPLACE(SUBSTRING(@Resp,1,DATALENGTH(@Resp)-1), ' ', '')

END