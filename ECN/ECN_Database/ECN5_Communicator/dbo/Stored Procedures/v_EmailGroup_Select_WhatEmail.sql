
CREATE PROCEDURE [dbo].[v_EmailGroup_Select_WhatEmail] 
(
@CustomerID int = NULL,
@EmailAddress varchar(255) = NULL,
@GroupID int = NULL
)
AS

DECLARE @EmailID int = null

SELECT TOP 1 @EmailID = e.EmailID
FROM
	Emails e WITH (NOLOCK)
	JOIN EmailGroups eg WITH (NOLOCK) ON e.EmailID = eg.EmailID
WHERE
	e.CustomerID = @CustomerID AND
	eg.GroupID = @GroupID AND
	e.EmailAddress = @EmailAddress
	
IF @EmailID is NULL
BEGIN
	SELECT TOP 1 @EmailID = e.EmailID
	FROM
		Emails e WITH (NOLOCK)
	WHERE
		e.CustomerID = @CustomerID AND
		e.EmailAddress = @EmailAddress
END

SELECT Isnull(@EmailID,0)

