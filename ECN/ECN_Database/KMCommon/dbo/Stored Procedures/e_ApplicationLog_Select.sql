CREATE PROCEDURE [dbo].[e_ApplicationLog_Select]
(
@ApplicationID int = NULL
)

AS
IF @ApplicationID is NULL
BEGIN
	SELECT *
	FROM [ApplicationLog] WITH(NOLOCK)
END
ELSE
BEGIN
	SELECT *
	FROM [ApplicationLog] WITH(NOLOCK)
	WHERE ApplicationID = @ApplicationID
END
