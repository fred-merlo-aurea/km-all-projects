CREATE PROCEDURE [dbo].[spFilterDetailInsert]
@FilterID int,
@FieldType varchar(50),
@CompareType varchar(50),
@FieldName varchar(100),
@Comparator varchar(100),
@CompareValue varchar(500)
AS

INSERT INTO [FiltersDetails]
           ([FilterID]
           ,[FieldType]
           ,[CompareType]
           ,[FieldName]
           ,[Comparator]
           ,[CompareValue])
     VALUES
           (@FilterID,@FieldType,@CompareType,@FieldName,@Comparator,@CompareValue);SELECT @@IDENTITY;
