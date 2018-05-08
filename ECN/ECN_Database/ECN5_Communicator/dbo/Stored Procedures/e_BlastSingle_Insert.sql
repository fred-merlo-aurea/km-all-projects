CREATE  PROC [dbo].[e_BlastSingle_Insert] 
(
	@BlastID int = NULL,
	@EmailID int = NULL,
	@SendTime datetime = NULL,
	@LayoutPlanID int = NULL,
	@RefBlastID int = NULL,
	@UserID int = NULL
)
AS 
BEGIN
	INSERT INTO BlastSingles
	(
		BlastID,EmailID,SendTime,Processed,LayoutPlanID,RefblastID,CreatedUserID,CreatedDate,IsDeleted
	)
	VALUES
	(
		@BlastID,@EmailID,@SendTime,'N',@LayoutPlanID,@RefblastID,@UserID,GetDate(),0
	)
	SELECT @@IDENTITY
END
