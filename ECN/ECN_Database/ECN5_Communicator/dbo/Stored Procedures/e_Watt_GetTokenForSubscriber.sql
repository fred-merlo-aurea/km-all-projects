CREATE PROCEDURE [dbo].[e_Watt_GetTokenForSubscriber]

	 @ShortName varchar(100),
	 @GroupID int,
	 @EmailAddress varchar(100),
	 @CustomerID int
AS
BEGIN
declare @returnToken varchar(500)
select @returnToken = edv.DataValue
from
      Emails e with (nolock)
      join EmailGroups eg with (nolock) on e.EmailID = eg.EmailID and eg.GroupID = @GroupID
      join GroupDatafields gdf with (nolock) on eg.GroupID = gdf.GroupID and gdf.ShortName = @ShortName
      join EmailDataValues edv with (nolock) on e.EmailID = edv.EmailID and gdf.GroupDatafieldsID = edv.GroupDatafieldsID
where
      e.EmailAddress = @EmailAddress and
      e.CustomerID = @CustomerID

if(@returnToken is null or LEN(@returnToken) = 0)
	Select ''
Else
	select @returnToken
END