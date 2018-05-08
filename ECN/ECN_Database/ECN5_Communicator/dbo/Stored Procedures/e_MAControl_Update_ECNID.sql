CREATE PROCEDURE [dbo].[e_MAControl_Update_ECNID]
	@NewID int,
	@CurrentID int
AS
	if exists(select top 1 * from MAControl m with(nolock) where m.[ECNID] = @CurrentID and m.ControlType='Form')
	BEGIN
		UPDATE [MAControl]
		SET ECNID = @NewID
		where ECNID = @CurrentID and ControlType='Form'
	END
