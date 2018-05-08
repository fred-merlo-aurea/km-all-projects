-- =============================================
-- Author:		Rohit Pooserla
-- Create date: 5/31/2012	
-- Description:	GetBlastIDfromBlastGroup
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetBlastID_fromBlastGroup]
	-- Add the parameters for the stored procedure here
	@BlastGroupID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	declare @BlastID varchar(50);

	select @BlastID=ecn5_communicator.dbo.BlastGrouping.BlastIDs from ecn5_communicator.dbo.BlastGrouping where BlastGroupID=@BlastGroupID;

	select * from ecn_Activity.dbo.fn_Split(@BlastID,',')
END
