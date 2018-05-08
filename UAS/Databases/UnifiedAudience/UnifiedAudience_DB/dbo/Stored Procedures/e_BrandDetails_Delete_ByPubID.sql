CREATE PROCEDURE [dbo].[e_BrandDetails_Delete_ByPubID]
@PubID int 
AS
BEGIN

	set nocount on

	delete from BrandDetails where PubID = @PubID

END