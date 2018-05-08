CREATE PROCEDURE [dbo].[e_BaseChannel_Select]
@BaseChannelID int = NULL
AS
BEGIN	
	SET NOCOUNT ON;
	if @BaseChannelID is not null
	Begin
		Select * from BaseChannel with (nolock) where BaseChannelID = @BaseChannelID and IsDeleted = 0
    End
    else
    Begin
		Select * from BaseChannel with (nolock) where IsDeleted = 0 
    End
END
