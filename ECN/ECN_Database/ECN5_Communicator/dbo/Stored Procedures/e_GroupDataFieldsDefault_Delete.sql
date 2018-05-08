CREATE PROCEDURE [dbo].[e_GroupDataFieldsDefault_Delete]
	@GDFID int
AS
	Delete FROM GroupDataFieldsDefault
	where GDFID = @GDFID
