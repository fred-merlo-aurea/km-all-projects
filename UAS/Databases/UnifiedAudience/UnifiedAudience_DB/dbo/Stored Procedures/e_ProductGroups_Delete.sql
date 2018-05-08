CREATE PROCEDURE [dbo].[e_ProductGroups_Delete]
	@PubID int
AS
BEGIN

	set nocount on

	DELETE PubGroups WHERE PubID = @PubID

END