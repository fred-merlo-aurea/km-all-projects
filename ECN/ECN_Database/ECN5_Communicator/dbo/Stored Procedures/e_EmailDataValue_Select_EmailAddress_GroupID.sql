--2014-10-24 MK Added  WITH (NOLOCK) hints

CREATE PROCEDURE e_EmailDataValue_Select_EmailAddress_GroupID 
	@groupID int,
	@shortName varchar(200),
	@emailAddress varchar(200)
AS
BEGIN
	SET NOCOUNT ON;	   

	SELECT 
		DataValue 
	FROM 
		EmailDataValues edv WITH (NOLOCK)
		INNER JOIN GroupDatafields gdf WITH (NOLOCK) ON edv.GroupDatafieldsID = gdf.GroupDatafieldsID
	WHERE 
		gdf.ShortName=@shortName
		AND gdf.GroupID=@groupID
		AND EmailID IN (SELECT 
							e.EmailID 
						FROM 
							Emails e
							INNER JOIN EmailGroups eg WITH (NOLOCK) ON e.EmailID=eg.EmailID
						WHERE 
							eg.GroupID=@groupID 
							and e.EmailAddress=@emailAddress)
END

