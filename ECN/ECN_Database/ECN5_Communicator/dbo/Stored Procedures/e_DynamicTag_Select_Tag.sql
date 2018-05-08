CREATE PROCEDURE dbo.e_DynamicTag_Select_Tag
@CustomerID int,
@Tag varchar(200)
AS


SELECT dt.* FROM DynamicTag dt WITH (NOLOCK)
WHERE dt.CustomerID = @CustomerID and dt.IsDeleted = 0 and dt.Tag=@Tag