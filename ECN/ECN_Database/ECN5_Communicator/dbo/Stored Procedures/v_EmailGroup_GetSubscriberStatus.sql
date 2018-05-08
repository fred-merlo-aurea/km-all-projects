CREATE PROCEDURE [dbo].[v_EmailGroup_GetSubscriberStatus] 
(
@CustomerID int = NULL,
@EmailAddress varchar(255) = NULL
)
AS

select g.GroupID, g.GroupName, max(eg.SubscribeTypeCode) as SubscribeTypeCode  
from Emails e WITH (NOLOCK)
	join EmailGroups eg WITH (NOLOCK) on eg.EmailID = e.EmailID
    join Groups g WITH (NOLOCK) on g.GroupID = eg.GroupID
where 
	g.CustomerID = @CustomerID AND
	e.EmailAddress = @EmailAddress
group by g.GroupID, g.GroupName
