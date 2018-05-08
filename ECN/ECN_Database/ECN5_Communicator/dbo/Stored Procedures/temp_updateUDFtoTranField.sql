CREATE PROCEDURE [dbo].[temp_updateUDFtoTranField] 
	@GroupID int, 
	@ShortName varchar(250) 	
AS
BEGIN
	declare @DataFieldSetID int
	set @DataFieldSetID = 0

	select @DataFieldSetID = DataFieldSetID from DatafieldSets where GroupID = @GroupID

	print @DataFieldSetID
	
	if(@DataFieldSetID > 0)
	begin
		update GroupDatafields set DatafieldSetID = @DataFieldSetID where ShortName = @ShortName and GroupID = @GroupID
	end
END
