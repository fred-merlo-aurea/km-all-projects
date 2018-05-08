CREATE PROCEDURE e_AdHocDimensionGroup_Select_ClientID_AdHocDimensionGroupName
@ClientID int,
@AdHocDimensionGroupName varchar(50)
AS
BEGIN

	set nocount on

	Select * 
	from AdHocDimensionGroup WITH(NOLOCK)
	where ClientID = @ClientID and AdHocDimensionGroupName = @AdHocDimensionGroupName

END
GO