-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[e_BlastSingle_GetRefBlastID_BlastID] 
	@BlastID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT top 1 bs.refBlastID from BlastSingles bs with(nolock) where bs.BlastID = @BlastID
END