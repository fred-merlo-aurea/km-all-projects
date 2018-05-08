CREATE PROCEDURE [dbo].[e_ProductSubscription_Clear_IMB]
	@ProductID int
AS
BEGIN

	set nocount on

	UPDATE PubSubscriptions
	SET IMBSEQ = '0'
	WHERE PubID = @ProductID

END