CREATE PROCEDURE [dbo].[spBlastBWD_GetSendTotalByFilterName]
@FilterName varchar(50)
AS
DECLARE @FilterID int
SET @FilterID = (select FilterID from  [FILTER] where FilterName = @FilterName)

select SendTotal from [BLAST] where FilterID=@FilterID
