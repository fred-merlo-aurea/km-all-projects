CREATE PROCEDURE [dbo].[e_SourceFile_Select_ClientId_FileType]
	@ClientID int,
	@DatabaseFileType varchar(50)
AS
BEGIN

	SET NOCOUNT ON

	declare @ftid int = (select codeId 
							 from UAD_Lookup..Code with(nolock) 
							 where codetypeid=(select CodeTypeId 
												from UAD_Lookup..CodeType with(nolock) 
												where codetypename='Database File') 
									and codename=@DatabaseFileType)
	SELECT  * 
	FROM SourceFile WITH(NOLOCK)
	WHERE ClientID = @ClientID AND DatabaseFileTypeId = @ftid

END
