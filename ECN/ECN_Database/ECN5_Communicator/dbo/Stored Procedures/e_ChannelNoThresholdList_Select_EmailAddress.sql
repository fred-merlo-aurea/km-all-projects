CREATE PROCEDURE [dbo].[e_ChannelNoThresholdList_Select_EmailAddress]   
@BaseChannelID int,
@EmailAddress varchar(100) = NULL
AS
	DECLARE @SQLSelect varchar(1000)
	set @SQLSelect = 'SELECT * FROM ChannelNoThresholdList WITH (NOLOCK) WHERE BaseChannelID = ' + convert(varchar,@BaseChannelID) + ' and EmailAddress like ''%' + @EmailAddress + '%'' and IsDeleted = 0'
	EXEC (@SQLSelect)