CREATE PROCEDURE [dbo].[spFilterSelectByFilterName]
@FilterName varchar(50)
AS
SELECT [FilterID]
      ,[CustomerID]
      ,[UserID]
      ,[GroupID]
      ,[FilterName]
      ,[WhereClause]
      ,[DynamicWhere]
      ,[CreateDate]
  FROM [Filters]
  WHERE FilterName = @FilterName
