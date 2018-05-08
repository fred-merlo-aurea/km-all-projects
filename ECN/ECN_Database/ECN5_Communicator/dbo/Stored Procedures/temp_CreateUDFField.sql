CREATE PROCEDURE [dbo].[temp_CreateUDFField]
	@GroupID int, 
	@ShortName varchar(250),  
	@LongName varchar(250)    
AS
BEGIN	
	begin
		insert into GroupDatafields (GroupID, ShortName, LongName, DatafieldSetID,IsPublic,IsPrimaryKey) 
			values (@GroupID, @ShortName, @LongName, null, 'N',0) 
	end	
END
