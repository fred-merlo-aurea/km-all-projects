-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[e_LinkTrackingParamSettings_Select_BaseChannelID_LTPID]
	@BaseChannelID int,
	@LTPID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT * FROM LinkTrackingParamSettings with(nolock)
    WHERE BaseChannelID = @BaseChannelID and LTPID = @LTPID and IsDeleted = 0
END