-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[e_LinkTrackingParamOption_Select_BaseChannelID_LTPID]
	@LTPID int,
	@BaseChannelID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT * FROM LinkTrackingParamOption with(nolock)
    WHERE LTPID = @LTPID and BaseChannelID = @BaseChannelID and IsActive = 1 and IsDeleted = 0
END