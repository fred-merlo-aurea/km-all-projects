CREATE PROCEDURE [dbo].[v_EmailGroup_GetGroupStats]
	@GroupID int,
	@CustomerID int
AS
	SELECT COUNT(case when eg.SubscribeTypeCode = 'S' then 1 ELSE NULL END) as 'Subscribed',
			COUNT(case when eg.SubscribeTypeCode = 'U' then 1 ELSE null end) as 'Unsubscribed',
			COUNT(case when eg.SubscribeTypeCode = 'M' then 1 ELSE null end) as 'Suppressed'
	from EMailGroups eg with(nolock)
	join Groups g with(nolock) on eg.GroupID = g.groupID
	where g.GroupID = @GroupID and g.CustomerID = @CustomerID
