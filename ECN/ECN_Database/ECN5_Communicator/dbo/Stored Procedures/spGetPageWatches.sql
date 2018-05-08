CREATE PROC [dbo].[spGetPageWatches] 
	@CustomerID int
AS 
BEGIN 
	select pw.PageWatchID, pw.name, substring(pw.url,0,68) as URL, g.GroupName, CONVERT(varchar(2),pw.FrequencyNo) + ' ' + pw.FrequencyType + 'S ' as "Frequency", u.UserName, ISNULL(SUM(Convert(int,pwt.IsChanged)), 0) as "UpdatedTags", pw.IsActive
	from PageWatch pw
		left outer join PageWatchTag pwt on pw.PageWatchID = pwt.PageWatchID
		join [ECN5_ACCOUNTS].[DBO].Users u on pw.AdminUserID = u.UserID
		join Groups g on pw.GroupID = g.GroupID
	where pw.CustomerID = @CustomerID
	group by pw.PageWatchID, pw.name, pw.url, g.GroupName, CONVERT(varchar(2),pw.FrequencyNo) + ' ' + pw.FrequencyType + 'S ', u.UserName, pw.IsActive
END
