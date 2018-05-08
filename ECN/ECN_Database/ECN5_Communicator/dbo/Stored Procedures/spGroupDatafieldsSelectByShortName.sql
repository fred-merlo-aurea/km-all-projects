CREATE PROCEDURE [dbo].[spGroupDatafieldsSelectByShortName]
@ShortName varchar(50)
AS
SELECT [GroupDatafieldsID]
      ,[GroupID]
      ,[ShortName]
      ,[LongName]
      ,[SurveyID]
      ,[DatafieldSetID]
      ,[IsPublic]
      ,[IsPrimaryKey]
  FROM [GroupDatafields]
  WHERE ShortName = @ShortName
