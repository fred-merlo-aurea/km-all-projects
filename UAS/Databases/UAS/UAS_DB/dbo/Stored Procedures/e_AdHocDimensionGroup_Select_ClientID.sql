CREATE PROCEDURE e_AdHocDimensionGroup_Select_ClientID
@ClientID int
AS
BEGIN

	set nocount on

	Select * 
	from AdHocDimensionGroup WITH(NOLOCK)
	where ClientID = @ClientID

END
GO