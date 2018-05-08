CREATE PROCEDURE e_AggregateDimension_Select_ClientID
@ClientID int
AS
BEGIN

	set nocount on

	Select * 
	from AggregateDimension WITH(NOLOCK)
	where ClientID = @ClientID

END
GO