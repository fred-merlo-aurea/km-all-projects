CREATE PROCEDURE [dbo].[e_GroupDataFieldsDefault_GetByUDFID]
	@GDFID int
AS
	Select * 
	FROM GroupDataFieldsDefault gdfd with(nolock)
	WHERE gdfd.GDFID = @GDFID
