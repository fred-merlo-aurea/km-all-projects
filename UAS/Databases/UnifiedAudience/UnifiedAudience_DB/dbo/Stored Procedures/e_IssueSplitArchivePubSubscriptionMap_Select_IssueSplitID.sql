CREATE PROCEDURE [dbo].[e_IssueSplitArchivePubSubscriptionMap_Select_IssueSplitID]
	@IssueSplitID int
AS
BEGIN

	SET NOCOUNT ON

	SELECT IssueSplitPubSubscriptionId,IssueSplitId
	FROM IssueSplitArchivePubSubscriptionMap i With(NoLock)
	WHERE i.IssueSplitID =@IssueSplitID

END