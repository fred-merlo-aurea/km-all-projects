CREATE PROCEDURE [dbo].[e_BlastSingle_Delete_LayoutPlanID]
	@LayoutPlanID int,
	@UpdatedUserID int
AS
	UPDATE BlastSingles 
	set Processed = 'c',UpdatedUserID=@UpdatedUserID,UpdatedDate=GETDATE()
	where LayoutPlanID = @LayoutPlanID and Processed = 'n'