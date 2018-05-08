CREATE PROCEDURE [dbo].[e_BlastSingle_Delete_EmailID]
	@EmailID int,
	@LayoutPlanID int,
	@UpdatedUserID int
AS
	UPDATE BlastSingles 
	set Processed = 'c',UpdatedUserID=@UpdatedUserID,UpdatedDate=GETDATE()
	where EmailID = @EmailID and LayoutPlanID=@LayoutPlanID and Processed = 'n'
