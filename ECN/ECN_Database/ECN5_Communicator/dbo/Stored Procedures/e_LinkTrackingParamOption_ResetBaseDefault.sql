-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[e_LinkTrackingParamOption_ResetBaseDefault]
	
	@LTPID int,
	@BaseChannelID int
AS
BEGIN

    Update LinkTrackingParamOption
    set IsDefault = 0
    where LTPID = @LTPID and BaseChannelID = @BaseChannelID and IsDeleted = 0 and IsActive = 1
END