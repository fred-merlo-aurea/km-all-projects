CREATE PROCEDURE [dbo].[usp_GroupExport]

@GroupId INT ,
@DaysOld INT = 1

AS SET NOCOUNT ON 

--DECLARE @GroupId INT = 329071,
--@DaysOld INT = 1

DECLARE @loc_GroupId VARCHAR(10)
DECLARE @Loc_DaysOld VARCHAR(5)
DECLARE @Udfs VARCHAR(MAX)
DECLARE @SQL VARCHAR(MAX)
DECLARE @DateString VARCHAR(10)
DECLARE @TableName VARCHAR(100)

SET @loc_GroupId =  CONVERT(VARCHAR(10),@GroupId)
SET @Loc_DaysOld =  CONVERT(VARCHAR(5),@DaysOld )
SET @DateString = (SELECT REPLACE(CONVERT(CHAR(10),GETDATE(),110),'-',''))


SET @Udfs =(
SELECT  
	STUFF(( SELECT DISTINCT '],[' + ShortName 
	FROM groupdatafields 
	WHERE 
		GroupID = @loc_GroupId 
		AND IsDeleted = 0
		AND DatafieldSetID is null ORDER BY '],[' + ShortName FOR XML PATH('') ), 1, 2, '') + ']' )


SET @TableName = 'GroupExport' + @Loc_GroupId + @DateString 
IF EXISTS (SELECT 1 FROM ECN_TEMP.SYS.TABLES WHERE NAME = @TableName) 
EXEC ('DROP TABLE ECN_TEMP.DBO.'+ @TableName)


IF @Udfs IS NOT NULL
BEGIN
	SET @SQL = 
	'SELECT 
		GroupName,
		eg.GroupID,
		e.EmailID, 
		EmailAddress, 
		Title, 
		FirstName, 
		LastName, 
		FullName, 
		Company, 
		Occupation, 
		Address, 
		Address2,
		City, 
		State, 
		Zip, 
		Country, 
		Voice, 
		Mobile, 
		Fax, 
		Website, 
		Age, 
		Income, 
		Gender,
		User1, 
		User2, 
		User3, 
		User4, 
		User5, 
		User6, 
		Birthdate, 
		UserEvent1, 
		UserEvent1Date, 
		UserEvent2, 
		UserEvent2Date, 
		Convert(varchar,Notes) as Notes, 
		e.DateAdded		AS EmailDateAdded,
		e.DateUpdated	AS EmailDateUpdated,
		eg.CreatedOn	AS EmailGroupCreateOn, 
		eg.LastChanged	AS EmailGroupLastChanged, 
		FormatTypeCode, 
		SubscribeTypeCode, 
	--	SAUDFs.* 
		' + @Udfs + '
	INTO
		ECN_TEMP.DBO.GroupExport' + @Loc_GroupId + @DateString +'
	FROM 
		Emails E with (NOLOCK)
		INNER JOIN EmailGroups eg with (NOLOCK) ON e.EmailId = eg.EmailId
		INNER JOIN Groups g with (NOLOCK) ON g.GroupId = eg.GroupId
		LEFT OUTER JOIN			
					(
						SELECT *
						 FROM
						 ( 
							SELECT 
								gdf.groupID, 
								edv.emailID, 
								gdf.ShortName, 
								edv.DataValue
							from
								EmailDataValues edv with (NOLOCK)  
								join Groupdatafields gdf with (NOLOCK) on edv.GroupDatafieldsID = gdf.GroupDatafieldsID
							where 
								gdf.GroupID = ' + @loc_GroupId +'
								and datafieldsetID is null
						 ) u
						 PIVOT
						 (
						 MAX (DataValue)
						 FOR ShortName in (' + @udfs +' )) as pvt 			
					) 
					SAUDFs ON e.emailID = SAUDFs.EmailID and g.GroupID = SAUDFs.GroupId
	WHERE 
		g.GroupId = ' + @loc_GroupId + '
		AND (DATEDIFF(DAY,eg.CreatedOn,GETDATE())  =' + @Loc_DaysOld +'
			OR DATEDIFF(DAY,eg.LastChanged,GETDATE())= '+ @Loc_DaysOld + ')'
END

ELSE IF @Udfs IS NULL
BEGIN
	SET @SQL = 
	'SELECT 
		GroupName,
		eg.GroupID,
		e.EmailID, 
		EmailAddress, 
		Title, 
		FirstName, 
		LastName, 
		FullName, 
		Company, 
		Occupation, 
		Address, 
		Address2,
		City, 
		State, 
		Zip, 
		Country, 
		Voice, 
		Mobile, 
		Fax, 
		Website, 
		Age, 
		Income, 
		Gender,
		User1, 
		User2, 
		User3, 
		User4, 
		User5, 
		User6, 
		Birthdate, 
		UserEvent1, 
		UserEvent1Date, 
		UserEvent2, 
		UserEvent2Date, 
		Convert(varchar,Notes) as Notes, 
		e.DateAdded		AS EmailDateAdded,
		e.DateUpdated	AS EmailDateUpdated,
		eg.CreatedOn	AS EmailGroupCreateOn, 
		eg.LastChanged	AS EmailGroupLastChanged, 
		FormatTypeCode, 
		SubscribeTypeCode
	INTO
		ECN_TEMP.DBO.GroupExport' + @Loc_GroupId + @DateString +'
	FROM 
		Emails E with (NOLOCK)
		INNER JOIN EmailGroups eg with (NOLOCK) ON e.EmailId = eg.EmailId
		INNER JOIN Groups g with (NOLOCK) ON g.GroupId = eg.GroupId
	WHERE 
		g.GroupId = ' + @loc_GroupId + '
		AND (DATEDIFF(DAY,eg.CreatedOn,GETDATE())  =' + @Loc_DaysOld +'
			OR DATEDIFF(DAY,eg.LastChanged,GETDATE())= '+ @Loc_DaysOld + ')'
END

	
--PRINT @SQL
EXEC (@SQL)

SELECT  'GroupExport' + @Loc_GroupId + @DateString 
GO


