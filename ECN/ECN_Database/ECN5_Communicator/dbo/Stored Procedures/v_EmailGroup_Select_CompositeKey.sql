CREATE PROCEDURE [dbo].[v_EmailGroup_Select_CompositeKey] 
(
@CustomerID int = NULL,
@EmailAddress varchar(255) = NULL,
@GroupID int = NULL,
@CompositeKey varchar(100) = NULL,
@CompositeKeyValue varchar(100) = NULL
)
AS
Begin
	DECLARE @EmailID int = null
	DECLARE @SelectSQL nvarchar(4000)
	SET @SelectSQL = '	SELECT TOP 1 @EmailID = e.EmailID 
						from emails e with (nolock) join emailgroups eg with (nolock) on e.emailID = eg.emailID 
						where customerID = ' + convert(varchar,@CustomerID) + ' and 
						eg.groupID = ' + convert(varchar,@GroupID) + ' and 
						e.emailaddress = ''' + convert(varchar,@EmailAddress) + ''' and
						e.' + @CompositeKey + ' = ''' + @CompositeKeyValue + ''''

	--EXEC(@SelectSQL) – modified to use sp_executesql – sunil – 4/7/2013

	exec sp_executesql @SelectSQL, N'@EmailID int out', @EmailID out


	IF @EmailID is NULL
	BEGIN
		SET @EmailID = 0
	END

	SELECT @EmailID

End