CREATE PROCEDURE [dbo].[e_MAControl_Delete]
	@MAControlID int
AS
	if exists(select top 1 * from MAControl m with(nolock) where m.MAControlID = @MAControlID)
	BEGIN
		Delete from MAControl
		where MAControlID = @MAControlID
	END

