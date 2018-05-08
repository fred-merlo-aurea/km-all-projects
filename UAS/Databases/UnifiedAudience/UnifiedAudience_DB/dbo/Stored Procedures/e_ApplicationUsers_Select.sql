create PROCEDURE [dbo].[e_ApplicationUsers_Select]   
AS
BEGIN

	set nocount on

	Select * from ApplicationUsers With(NoLock)

END