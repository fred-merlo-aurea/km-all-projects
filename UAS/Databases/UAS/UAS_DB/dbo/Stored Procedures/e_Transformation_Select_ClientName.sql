CREATE PROCEDURE [dbo].[e_Transformation_Select_ClientName] 
@ClientID int
AS
BEGIN

	set nocount on

	Select * from Transformation With(NoLock)
	where ClientId = @ClientID and IsActive = 'true'

END