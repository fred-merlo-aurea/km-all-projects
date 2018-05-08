CREATE proc [dbo].[spAssignBlastEngineFrameworkTesting]
as
BEGIN

DECLARE @CurrentBlast int
DECLARE @BlastEngineID int
DECLARE @LayoutID int
DECLARE @CustomerID int
DECLARE @BlastType varchar(50)
DECLARE @TestBlast varchar(1)
DECLARE @BaseChannelID int

DECLARE c_BlastCursor CURSOR FOR SELECT b.CustomerID, b.BlastID, b.BlastType, b.LayoutID, b.TestBlast, c.BaseChannelID
FROM Blast b
	JOIN ecn5_accounts..Customer c on b.CustomerID = c.CustomerID
WHERE 
c.ActiveFlag = 'Y' and 
	b.SendTime <  DATEADD(mi, 1, GETDATE()) AND 
	b.StatusCode = 'pending' AND 
	b.BlastEngineID IS NULL AND 
	(b.BlastType='html' OR b.BlastType='text' OR b.BlastType='sample' OR b.BlastType='champion') 
	and c.BlastConfigID is not null 
	and c.BlastConfigID in (3)
	--and c.BaseChannelID != 42
	--and c.BaseChannelID != 70
ORDER BY b.sendtime ASC, b.blastID ASC

OPEN c_BlastCursor  
FETCH NEXT FROM c_BlastCursor INTO @CustomerID, @CurrentBlast, @BlastType, @LayoutID, @TestBlast, @BaseChannelID

WHILE @@FETCH_STATUS = 0  
BEGIN 
	SET @BlastEngineID = 99999	
	IF(@CustomerID = 1)
		set @BlastEngineID = 101010
	
	IF(@CustomerID IN (1941,1797) AND @TestBlast = 'Y')
		SET @BlastEngineID = 101010

	UPDATE Blast
	SET BlastEngineID = @BlastEngineID
	WHERE BlastID = @CurrentBlast
	
	
	FETCH NEXT FROM c_BlastCursor INTO @CustomerID, @CurrentBlast, @BlastType, @LayoutID, @TestBlast, @BaseChannelID
END

CLOSE c_BlastCursor  
DEALLOCATE c_BlastCursor

END