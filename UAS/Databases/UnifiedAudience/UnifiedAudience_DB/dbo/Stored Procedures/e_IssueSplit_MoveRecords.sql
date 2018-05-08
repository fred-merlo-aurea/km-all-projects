CREATE PROCEDURE e_IssueSplit_MoveRecords
    @ToIssueSplitId int,
    @FromIssueSplitId int,
    @RecordMovedCount int,
	@TVP_IssueSplitIds IdListType READONLY 
AS
BEGIN

	DECLARE @FromIssueSplitCount int;
	DECLARE @ToIssueSplitCount int;
	
	SELECT @FromIssueSplitCount=MAX(IssueSplitCount) FROM IssueSplit WHERE IssueSplitId=@FromIssueSplitId
	SELECT @ToIssueSplitCount=MAX(IssueSplitCount) FROM IssueSplit WHERE IssueSplitId=@ToIssueSplitId
	
	SET @FromIssueSplitCount =@FromIssueSplitCount-@RecordMovedCount
	SET @ToIssueSplitCount =@ToIssueSplitCount+@RecordMovedCount
	
	UPDATE IssueSplit SET IssueSplitCount=@FromIssueSplitCount WHERE IssueSplitId=@FromIssueSplitId
	UPDATE IssueSplit SET IssueSplitCount=@ToIssueSplitCount WHERE IssueSplitId=@ToIssueSplitId
	
	UPDATE IssueSplitArchivePubSubscriptionMap SET IssueSplitId=@ToIssueSplitId, RecordMovedFrom=@FromIssueSplitId
	WHERE IssueSplitPubSubscriptionId IN( SELECT id FROM @TVP_IssueSplitIds )
	
END  
