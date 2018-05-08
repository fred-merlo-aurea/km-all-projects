CREATE  proc [dbo].[Schedule](@blastID as int, @days as int)
AS
Begin
declare @customerID as int
declare @emailSubject as varchar(255)
declare @emailFrom as varchar(255)
declare @emailFromName as varchar(255)
declare @sendTime as DateTime
declare @blastType as varchar(50)
declare @codeID as int
declare @layoutID as int
declare @groupID as int
declare @userID as int
declare @filterID as int
declare @replyTo as varchar(100)
declare @blastFrequency as varchar(50)
declare @refBlastID as int
declare CurBlasts cursor for select CustomerID, EmailSubject, EmailFrom, EmailFromName, SendTime, BlastType,
codeID, layoutID, groupID, CreateduserID, filterID, replyTo, BlastFrequency, RefBlastID from [BLAST] where BlastID = @blastID
open CurBlasts
Fetch next from CurBlasts into @customerID, @emailSubject, @emailFrom, @emailFromName, @sendTime, @blastType,@codeID, @layoutID, @groupID, @userID, @filterID, @replyTo, @blastFrequency, @refBlastID
if @@FETCH_STATUS = 0
BEGIN
	INSERT INTO [BLAST] 
(CustomerID, EmailSubject, EmailFrom, EmailFromName, SendTime, BlastType, codeID, layoutID, groupID, CreateduserID, filterID, replyTo, BlastFrequency, RefBlastID, TestBlast, StatusCode, Spinlock,AttemptTotal, SendTotal,SuccessTotal) VALUES 
(@customerID, @emailSubject, @emailFrom, @emailFromName, dateadd(day, @days, @sendtime), @blastType,@codeID, @layoutID, @groupID, @userID, @filterID, @replyTo, @blastFrequency, @refBlastID,'n','pending','n',0,0,0)
	UPDATE BlastPlans set BlastID = @@identity where BlastID = @blastID and CustomerID = @customerID
	print @@identity
END
ELSE
	Print 'Empty'
close CurBlasts
deallocate CurBlasts
End
