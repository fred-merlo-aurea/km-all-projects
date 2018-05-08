CREATE PROCEDURE [dbo].[e_Transformation_Select_ClientID] 
@ClientID int
AS
BEGIN

	set nocount on

	Select * from Transformation With(NoLock)
	where ClientID = @ClientID and IsActive = 'true'

END