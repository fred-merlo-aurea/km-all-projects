CREATE PROCEDURE [dbo].[e_Filter_Select]
	@ClientID int
AS
BEGIN

	set nocount on

	SELECT * 
	FROM Filter WITH(NOLOCK)
	WHERE ClientID = @ClientID

END
GO