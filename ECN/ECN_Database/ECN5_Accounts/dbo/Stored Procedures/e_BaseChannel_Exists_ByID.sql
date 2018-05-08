create  PROC [dbo].[e_BaseChannel_Exists_ByID] 
(
	@BaseChannelID int = NULL
)
AS 
BEGIN
	IF EXISTS (SELECT TOP 1 BaseChannelID from BaseChannel  with (nolock) where BaseChannelID = @BaseChannelID AND IsDeleted = 0) SELECT 1 ELSE SELECT 0
END
