CREATE PROCEDURE [dbo].[spFilterInsert]
@CustomerID int,
@UserID int,
@GroupID int,
@FilterName varchar(50),
@WhereClause text,
@DynamicWhere text,
@CreateDate datetime
AS
INSERT INTO [Filters]
           ([CustomerID]
           ,[UserID]
           ,[GroupID]
           ,[FilterName]
           ,[WhereClause]
           ,[DynamicWhere]
           ,[CreateDate])
     VALUES
           (@CustomerID,@UserID,@GroupID,@FilterName,@WhereClause,@DynamicWhere,@CreateDate);SELECT @@IDENTITY;
