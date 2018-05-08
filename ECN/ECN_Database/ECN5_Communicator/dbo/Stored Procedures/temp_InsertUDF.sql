CREATE PROCEDURE [dbo].[temp_InsertUDF]
	@GroupID int
AS
BEGIN
	insert into GroupDatafields (GroupID,ShortName,LongName,SurveyID,DatafieldSetID,IsPublic,IsPrimaryKey)
		values (@GroupID, 'LASTDONATIONDATE', 'LAST DONATION DATE', null,null,'N',0)  
		
	SELECT * FROM GroupDatafields where GroupID = @GroupID		
END
