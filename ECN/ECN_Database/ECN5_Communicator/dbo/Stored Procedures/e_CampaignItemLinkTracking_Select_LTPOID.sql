-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[e_CampaignItemLinkTracking_Select_LTPOID]
	@LTPOID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * 
	FROM CampaignItemLinkTracking WITH(NOLOCK)
	WHERE LTPOID = @LTPOID
END