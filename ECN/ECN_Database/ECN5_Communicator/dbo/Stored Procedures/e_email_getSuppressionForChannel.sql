CREATE proc [dbo].[e_email_getSuppressionForChannel]
( @basechannelID int)
as
select emailaddress from ChannelMasterSuppressionList WITH (NOLOCK) where BaseChannelID = @basechannelID and IsDeleted = 0
union 
select e.emailaddress	
from [Emails] e  WITH (NOLOCK)
	join [EmailGroups] eg WITH (NOLOCK) on e.EmailID = eg.EmailID 
	join [Groups] g WITH (NOLOCK) on g.GroupID = eg.GroupID 
	join [ECN5_ACCOUNTS].[DBO].Customer c WITH (NOLOCK) on e.customeriD = c.CustomerID and g.customerID = c.CustomerID
where c.BaseChannelID = @basechannelID and g.MasterSupression = 1 and c.IsDeleted = 0 
order by 1