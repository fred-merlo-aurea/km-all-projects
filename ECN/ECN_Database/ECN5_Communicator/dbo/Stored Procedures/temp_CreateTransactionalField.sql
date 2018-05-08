CREATE PROCEDURE [dbo].[temp_CreateTransactionalField]
	@GroupID int, 
	@ShortName varchar(250),  
	@LongName varchar(250)    
AS
BEGIN
	declare @DataFieldSetID int
	set @DataFieldSetID = 0

	select @DataFieldSetID = DataFieldSetID from DatafieldSets where GroupID = @GroupID

	if(@DataFieldSetID > 0)
	begin
		insert into GroupDatafields (GroupID, ShortName, LongName, DatafieldSetID,IsPublic,IsPrimaryKey) 
			values (@GroupID, @ShortName, @LongName, @DataFieldSetID, 'N',0)
	end	
END
