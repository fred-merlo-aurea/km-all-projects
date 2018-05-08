CREATE PROCEDURE [dbo].[v_EmailDirectReport]
	@BaseChannelID int,
	@CustomerIDs varchar(1000),
	@StartDate varchar(30),
	@EndDate varchar(30)
AS
	SET @StartDate = @StartDate + ' 00:00:00.000'
	SET @EndDate = @EndDate + ' 23:59:59.999'
	select bc.BaseChannelID, bc.BaseChannelName, c.CustomerName,c.CustomerID, ed.Process, COUNT(ed.EmailDirectID) as 'Count'
	from ecn5_communicator..EmailDirect ed with(nolock)
	join Customer c with(nolock) on ed.CustomerID = c.CustomerID
	join Basechannel bc with(nolock) on c.BaseChannelID = bc.BaseChannelID
	where
	bc.BaseChannelID = CASE WHEN @BaseChannelID > 0 then @BaseChannelID else bc.BaseChannelID end
	and ((LEN(@CustomerIDs) > 0 and ed.CustomerID in (Select Convert(int,Items) from fn_Split(@CustomerIDs,','))) or (LEN(@CustomerIDs) = 0 and ed.CustomerID in (Select CustomerID from Customer c with(nolock) where c.IsDeleted = 0)))
	and ed.SendTime between @StartDate and @EndDate and ed.Status = 'sent'
	Group by bc.BaseChannelID, bc.BaseChannelName, c.CustomerName,c.CustomerID, ed.Process
	order by bc.BaseChannelID, c.CustomerName,ed.Process