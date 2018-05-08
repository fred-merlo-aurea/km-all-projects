CREATE PROCEDURE [dbo].[sp_GetSubscriberIDByEmailAndGroup]
	@emailAddress varchar(250),
	@GroupID int,
	@CustID int
AS
BEGIN
	declare @SubscriberID varchar(100) 
	set @SubscriberID = '0' 

	SELECT @SubscriberID = ed.DataValue 
	from Emails e join EmailDataValues ed on e.EmailID = ed.EmailID  
	join GroupDatafields gdf on gdf.GroupDatafieldsID = ed.GroupDatafieldsID 
	where e.EmailAddress = @emailAddress and e.CustomerID = @CustID
	and gdf.GroupID = @GroupID and gdf.ShortName = 'SUBSCRIBERID' 
	
	SELECT case when @SubscriberID = '' then '' else @SubscriberID end as 'SUBSCRIBERID' 
END
