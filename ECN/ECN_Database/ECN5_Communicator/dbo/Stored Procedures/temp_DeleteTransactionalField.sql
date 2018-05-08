CREATE PROCEDURE [dbo].[temp_DeleteTransactionalField]
	@GroupID int, 
	@ShortName varchar(250),  
	@LongName varchar(250)    
AS
BEGIN
	declare @GroupDataFieldsID int 	

	select @GroupDataFieldsID = GroupDatafieldsID from GroupDatafields where GroupID = @GroupID and 
			ShortName = @ShortName and LongName = @LongName

	if(@GroupDataFieldsID > 0)
	begin
		delete from GroupDatafields where GroupDatafieldsID = @GroupDataFieldsID
	end	
END
