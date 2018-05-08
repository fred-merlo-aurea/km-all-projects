create proc [dbo].[spSaveDatafieldsets]
(
	@GroupID int,
	@name varchar(50)
	
)
as
Begin		
	declare @DatafieldSetID int
	set @DatafieldSetID = 0 
	select @DatafieldSetID =  DatafieldSetID from DatafieldSets where GroupID = @GroupID and name = @name
	
	if (@DatafieldSetID = 0)
	begin
		insert into DatafieldSets (GroupID,MultivaluedYN,Name) VALUES (@GroupID,'Y', @Name) 
		set @DatafieldSetID = @@IDENTITY
	end

	select @DatafieldSetID 
end
