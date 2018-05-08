Create PROCEDURE [dbo].[e_Suffix_Select]
AS
BEGIN

	set nocount on

	SELECT * 
	FROM Suffix With(NoLock)

END