CREATE PROCEDURE [dbo].[e_GroupDataFields_ActiveByGDF]
	@CustomerID INT,
	@GroupID INT,
	@ShortName varchar(150)
AS     
BEGIN 
	IF EXISTS (
		SELECT 
			TOP 1 FilterID
		FROM 
			Filter with (NOLOCK)
		WHERE 
			CustomerID = @CustomerID AND 
			GroupID = @GroupID AND
			WhereClause LIKE '%' + @ShortName + '%'
	) 
	BEGIN
		SELECT 1 
	END	
	ELSE 
	BEGIN
		SELECT 0
	END
END
