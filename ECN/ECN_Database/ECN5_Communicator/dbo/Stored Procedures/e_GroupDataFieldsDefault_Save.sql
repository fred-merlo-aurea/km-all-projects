CREATE PROCEDURE [dbo].[e_GroupDataFieldsDefault_Save]
	@GDFID int,
	@DataValue varchar(500) = null,
	@SystemValue varchar(500) = null
AS
	IF exists(select top 1 * from GroupDataFieldsDefault g with(nolock) where g.GDFID = @GDFID)
	BEGIN
		UPDATE GroupDataFieldsDefault
		set DataValue = @DataValue, SystemValue = @SystemValue
		WHERE GDFID = @GDFID
	END
	ELSE
	BEGIN
		INSERT INTO GroupDatafieldsDefault(GDFID,DataValue,SystemValue)
		VALUES(@GDFID, @DataValue, @SystemValue)
	END
	