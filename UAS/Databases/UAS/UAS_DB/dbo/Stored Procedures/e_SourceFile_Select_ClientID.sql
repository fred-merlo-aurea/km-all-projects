CREATE PROCEDURE [dbo].[e_SourceFile_Select_ClientID] @ClientID int
AS
BEGIN

	set nocount on

	SELECT * 
	FROM SourceFile With(NoLock)
	Where ClientID = @ClientID

END