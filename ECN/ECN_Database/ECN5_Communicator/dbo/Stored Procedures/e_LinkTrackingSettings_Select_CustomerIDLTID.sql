-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[e_LinkTrackingSettings_Select_CustomerIDLTID]
	@CustomerID int,
	@LTID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * FROM LinkTrackingSettings with(nolock)
	WHERE LTID = @LTID and CustomerID = @CustomerID and IsDeleted = 0
END