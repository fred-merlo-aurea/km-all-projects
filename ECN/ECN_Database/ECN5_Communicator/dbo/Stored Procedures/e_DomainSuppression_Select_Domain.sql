CREATE PROCEDURE [dbo].[e_DomainSuppression_Select_Domain] 
(
@SearchString varchar(100) = NULL,
@BaseChannelID int = NULL,
@CustomerID int = NULL
)
AS

SET @SearchString = '''%' + UPPER(@SearchString) + '%'''

DECLARE @SelectSQL varchar(4000)
SET @SelectSQL =   'SELECT * FROM DomainSuppression WITH(NOLOCK) 
					WHERE UPPER(Domain) LIKE ' + convert(varchar,@SearchString) +
					' AND IsDeleted = 0 '
					
IF @CustomerID IS NOT NULL AND @BaseChannelID IS NOT NULL
BEGIN
	SET @SelectSQL = @SelectSQL + 'AND (BaseChannelID = ' + convert(varchar,@BaseChannelID) + ' OR CustomerID = ' + convert(varchar,@CustomerID) + ')'
END
ELSE IF @CustomerID IS NOT NULL 
BEGIN
	SET @SelectSQL = @SelectSQL + 'AND CustomerID = ' + convert(varchar,@CustomerID)
END
ELSE IF @BaseChannelID IS NOT NULL 
BEGIN
	SET @SelectSQL = @SelectSQL + 'AND BaseChannelID = ' + convert(varchar,@BaseChannelID)
END

EXEC(@SelectSQL)
