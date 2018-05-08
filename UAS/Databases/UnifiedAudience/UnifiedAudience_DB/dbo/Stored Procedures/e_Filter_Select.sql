CREATE PROCEDURE [dbo].[e_Filter_Select]
AS
BEGIN

	SET NOCOUNT ON

	Select * 
	from Filter With(NoLock)

END