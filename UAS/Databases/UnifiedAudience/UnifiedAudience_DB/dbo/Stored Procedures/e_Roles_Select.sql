create PROCEDURE [dbo].[e_Roles_Select]   
AS
BEGIN

	SET NOCOUNT ON

	Select * 
	from Roles With(NoLock)

END