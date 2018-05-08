CREATE PROCEDURE [dbo].[e_PubCode_Select]	
AS
BEGIN

	SET NOCOUNT ON

	SELECT pubcode 
	FROM pubs With(NoLock)

END