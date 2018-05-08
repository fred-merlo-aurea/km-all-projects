CREATE PROCEDURE [dbo].[e_SourceFile_Select_ClientName] 
@ClientID varchar(50)
AS
BEGIN

	set nocount on

	SELECT * 
	FROM SourceFile With(NoLock)
	Where ClientID = @ClientID

END