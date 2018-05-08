CREATE PROCEDURE [dbo].[e_Adhoc_Delete]
	@AdhocID int
AS
BEGIN

	set nocount on

	Delete from Adhoc Where AdhocID = @AdhocID

END