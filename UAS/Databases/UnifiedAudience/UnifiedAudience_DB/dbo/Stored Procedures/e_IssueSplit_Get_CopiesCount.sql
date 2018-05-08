CREATE PROCEDURE e_IssueSplit_Get_CopiesCount
	@TVP_IssueSplitIds IdListType READONLY 
AS
BEGIN
	
	DECLARE @copiescount int=0;
	
	SELECT @copiescount=SUM(ISNULL(Copies,0)) FROM PubSubscriptions ps(NOLOCK)
	JOIN @TVP_IssueSplitIds AS ids ON ps.PubSubscriptionID=ids.Id
	SELECT @copiescount;
END  
