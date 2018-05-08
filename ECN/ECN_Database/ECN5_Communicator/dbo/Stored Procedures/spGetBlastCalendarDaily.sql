CREATE PROCEDURE [dbo].[spGetBlastCalendarDaily]  
	@UserID int, 
	@FromDate date = null,
	@ToDate  date = null,
	@BlastCat int = 0,				
	@BlastType CHAR(1) = '',   
	@SubjectSearch VARCHAR(50) = '',
	@GroupSearch VARCHAR(50) = '',
	@SentUserID int	= -1
AS
BEGIN
	declare @CustomerID int, 
			@ChannelID int,
			@IsChannelAdmin bit

	if @FromDate is null
		select @FromDate = convert(date, DATEADD(dd, -1 * DAY(getdate()-1), getdate())) 
		
	if @ToDate is null
		select @ToDate = convert(date, getdate())

	select @CustomerID = c.CustomerID, @ChannelID = c.basechannelID,  @IsChannelAdmin = (case when substring(AccountsOptions,2,1) = 1 then 1 else 0 end)
	from ecn5_accounts..Users u join ecn5_accounts..Customer c on u.CustomerID = c.CustomerID where UserID = @UserID and u.IsDeleted = 0 and c.IsDeleted = 0 		
		
	SELECT
		 b.SendTime, b.BlastID, g.GroupName, b.StatusCode, b.EmailSubject, b.SendTotal	
	FROM 
		 ecn5_accounts..BaseChannel ch 
		 join ecn5_accounts..Customer cu on ch.BaseChannelID = cu.BaseChannelID
		 join Blast b on b.CustomerID = cu.CustomerID
		 left outer join  Groups g on g.GroupID = b.GroupID
	WHERE 
		 ((ch.BaseChannelID = @ChannelID AND @IsChannelAdmin=1) OR (b.CustomerID = @CustomerID)) and 
		 b.StatusCode in ('sent','active','pending') and
		 convert(date,b.SendTime) >= @FromDate and 
		 convert(date,b.SendTime) <= @ToDate  and
		 (b.TestBlast = @BlastType or len(@BlastType) = 0) and
		 (b.CodeID = @BlastCat or @BlastCat = 0) and
		 (b.CreatedUserID = @SentUserID or b.UpdatedUserID = @SentUserID or @SentUserID = -1) and 
		 (b.EmailSubject like '%' + @SubjectSearch + '%' or len(@SubjectSearch) = 0) and 
		 (g.GroupName like '%' + @GroupSearch + '%' or len(@GroupSearch) = 0)  
	ORDER BY													
		b.SendTime, b.StatusCode  
	DESC
END