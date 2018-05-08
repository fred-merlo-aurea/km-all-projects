CREATE PROCEDURE [rpt_ReqFlagSummary]
@ProductID int
AS
BEGIN
	
	SET NOCOUNT ON

	SELECT CodeValue, CodeName, 1 as RecordCount, Copies, PubSubscriptionID, IsComp
	FROM PubSubscriptions ps
		JOIN UAD_Lookup..Code c ON c.CodeId = ps.ReqFlag
	WHERE ps.PubID = @ProductID
	UNION
		SELECT CodeValue, CodeName, 1 as RecordCount, Copies, ic.IssueCompDetailId, IsComp
		FROM IssueCompDetail ic
			JOIN UAD_Lookup..Code c ON c.CodeId = ic.ReqFlag
		WHERE ic.PubID = @ProductID
END