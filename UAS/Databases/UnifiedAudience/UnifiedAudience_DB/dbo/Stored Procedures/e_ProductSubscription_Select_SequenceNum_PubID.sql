CREATE PROC e_ProductSubscription_Select_SequenceNum_PubID
@SequenceNum VARCHAR(20),
@PubID INT
AS
BEGIN
	
	SET NOCOUNT ON
	SELECT TOP 1 *
	FROM PubSubscriptions WITH(NOLOCK)
	WHERE SequenceID =@SequenceNum AND PubID =@PubID

END
GO