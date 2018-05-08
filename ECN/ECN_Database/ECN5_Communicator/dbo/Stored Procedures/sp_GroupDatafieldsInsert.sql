--select * from GroupDatafields where GroupID=40936

CREATE PROCEDURE [dbo].[sp_GroupDatafieldsInsert]
@GroupID int,
@ShortName varchar(50),
@LongName varchar(255),
@SurveyID int,
@DatafieldSetID int,
@IsPublic char(1),
@IsPrimaryKey bit
AS
INSERT INTO [ecn5_communicator].[dbo].[GroupDatafields]
           ([GroupID]
           ,[ShortName]
           ,[LongName]
           ,[SurveyID]
           ,[DatafieldSetID]
           ,[IsPublic]
           ,[IsPrimaryKey])
     VALUES
           (@GroupID,@ShortName,@LongName,@SurveyID,@DatafieldSetID,@IsPublic,@IsPrimaryKey); SELECT @@IDENTITY;
