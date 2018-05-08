CREATE proc [dbo].[sp_charitySubscriberLogin] (
	@groupID int,
	@emailID int,
	@password varchar(50)
)
as
Begin
	SELECT	e.EmailID, e.EmailAddress
	FROM	
		Emails e 
		LEFT OUTER JOIN EmailDataValues edv ON (edv.EmailID = e.EmailID) 
		LEFT OUTER JOIN GroupDatafields gdf ON ( edv.GroupDatafieldsID = gdf.GroupDatafieldsID ) 
	WHERE 
		gdf.ShortName 	= 'PASSWORD' 	AND 
		edv.DataValue	= @password  	AND 
		gdf.GroupID 	= @groupID 	AND
		edv.EmailID	= @emailID
End
