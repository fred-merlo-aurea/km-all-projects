USE [ecn5_communicator]
GO
/****** Object:  StoredProcedure [dbo].[spGetPageWatches]    Script Date: 07/19/2011 09:57:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[spGetPageWatches] 
	@CustomerID int
AS 
BEGIN 
	select pw.PageWatchID, pw.name, substring(pw.url,0,68) as URL, g.GroupName, CONVERT(varchar(2),pw.FrequencyNo) + ' ' + pw.FrequencyType + 'S ' as "Frequency", u.UserName, ISNULL(SUM(Convert(int,pwt.IsChanged)), 0) as "UpdatedTags", pw.IsActive
	from PageWatch pw
		left outer join PageWatchTag pwt on pw.PageWatchID = pwt.PageWatchID
		join ecn5_accounts..Users u on pw.AdminUserID = u.UserID
		join Groups g on pw.GroupID = g.GroupID
	where pw.CustomerID = @CustomerID
	group by pw.PageWatchID, pw.name, pw.url, g.GroupName, CONVERT(varchar(2),pw.FrequencyNo) + ' ' + pw.FrequencyType + 'S ', u.UserName, pw.IsActive
END

