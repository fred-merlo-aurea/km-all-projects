CREATE PROCEDURE [dbo].[e_QuickTestBlastConfig_Select_BaseChannelID]
@BaseChannelID int
AS
	SELECT *
	FROM QuickTestBlastConfig WITH (NOLOCK)
	WHERE BaseChannelID = @BaseChannelID
