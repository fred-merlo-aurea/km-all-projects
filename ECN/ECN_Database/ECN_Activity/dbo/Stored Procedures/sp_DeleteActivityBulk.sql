
CREATE PROC sp_DeleteActivityBulk

AS

SET NOCOUNT ON

DECLARE 
	@dt1Year datetime,
	@dt2Years datetime,
	@blastID int,
	@sampleID int

      
SET @dt1Year = CONVERT(VARCHAR(10), DATEADD(MM, -12, DATEADD(DD, -1 * (DAY(GETDATE()) -1), GETDATE())), 101)
SET @dt2Years =  CONVERT(VARCHAR(10), DATEADD(MM, -24, DATEADD(DD, -1 * (DAY(GETDATE()) -1), GETDATE())), 101)


----------------------------
-- Prep for Deletes
-----------------------------

/*
IF NOT EXISTS (SELECT 1 FROM ECN_TEMP.sys.Tables WHERE Name = 'BlastDelete')
CREATE TABLE ECN_Temp..BlastDelete (
	BlastId INT,
	SampleId INT)


--TRUNCATE TABLE ECN_Temp..BlastDelete

     
INSERT INTO 
	ECN_Temp..BlastDelete
SELECT 
	b.blastID, 
	b.SampleID  
FROM 
	ECN5_Communicator..Blast b 
	INNER JOIN ecn5_accounts..Customer c ON b.CustomerID = c.CustomerID
WHERE 
	sendtime < @dt1Year 
	AND BaseChannelID not in (94, 31) -- do not delete GOODDONOR customer accounts blasts
	AND basechannelID not in (3,4,6,15,16,34,39,42,46,50,55,63,65,68,69,71,72,73,74,75,76,77,78,79,80,81,82,84,85,86,87,88,89,91,99,103,104,105,106,107,108,109,110,111,112,113,114,127,128,130,131,132)
	AND b.blastID not in (SELECT blastID FROM ECN5_Communicator..LayoutPlans)
	AND b.customerID <> 1735 

UNION

SELECT
	b.blastID, 
	b.SampleID  
FROM 
	ECN5_Communicator..Blast b 
	INNER JOIN ecn5_accounts..Customer c ON b.CustomerID = c.CustomerID
WHERE 
	BaseChannelID not in (94, 31)-- do not delete GOODDONOR customer accounts blasts
	AND sendtime < @dt2Years
	AND b.blastID not in (select blastID from ECN5_Communicator..LayoutPlans)
	AND b.customerID <> 1735 
ORDER BY 
	BlastID ASC
*/

----------------------------
-- DECLARE VARIABLES
-----------------------------

DECLARE @RowsToDelete INT
DECLARE @Batchsize INT  = 1000
DECLARE @Iterations	INT
DECLARE @i INT	= 0

----------------------------
-- Perform Deletes
-----------------------------

SET @RowsToDelete =0 
SET @Batchsize  = 1000
SET @Iterations	=0
SET @i = 0

SELECT @RowsToDelete = COUNT(*) FROM BlastActivityConversion ba WITH (NOLOCK) JOIN ECN_Temp..BlastDelete bd ON ba.BlastID = bd.BlastId
SELECT @Iterations = (@RowsToDelete / @Batchsize) + 1

SET ROWCOUNT @Batchsize

WHILE 
@i < @Iterations

BEGIN
SET @i = @i + 1

	DELETE 
		ba
	FROM
		BlastActivityConversion ba
		JOIN ECN_Temp..BlastDelete bd ON ba.BlastID = bd.BlastId
END
PRINT 'Deleted ' + CONVERT(VARCHAR(15),@RowsToDelete) + ' From BlastActivityConversion'


-------------------
--RESET VARIABLES
-------------------

SET @RowsToDelete =0 
SET @Batchsize  = 1000
SET @Iterations	=0
SET @i = 0

SELECT @RowsToDelete = COUNT(*) FROM BlastActivityExceptions ba WITH (NOLOCK) JOIN ECN_Temp..BlastDelete bd ON ba.BlastID = bd.BlastId
SELECT @Iterations = (@RowsToDelete / 1000) + 1

SET ROWCOUNT @Batchsize

WHILE 
@i < @Iterations

BEGIN
SET @i = @i + 1

	DELETE 
		ba
	FROM
		BlastActivityExceptions ba
		JOIN ECN_Temp..BlastDelete bd ON ba.BlastID = bd.BlastId
END
PRINT 'Deleted ' + CONVERT(VARCHAR(15),@RowsToDelete) + ' From BlastActivityExceptions'



-------------------
--RESET VARIABLES
-------------------

SET @RowsToDelete =0 
SET @Batchsize = 1000
SET @Iterations	=0
SET @i = 0

SELECT @RowsToDelete = COUNT(*) FROM BlastActivityReads ba WITH (NOLOCK) JOIN ECN_Temp..BlastDelete bd ON ba.BlastID = bd.BlastId
SELECT @Iterations = (@RowsToDelete / 1000) + 1

SET ROWCOUNT @Batchsize

WHILE 
@i < @Iterations

BEGIN
SET @i = @i + 1

	DELETE 
		ba
	FROM
		BlastActivityReads ba
		JOIN ECN_Temp..BlastDelete bd ON ba.BlastID = bd.BlastId
END
PRINT 'Deleted ' + CONVERT(VARCHAR(15),@RowsToDelete) + ' From BlastActivityReads'


-------------------
--RESET VARIABLES
-------------------

SET @RowsToDelete =0 
SET @Batchsize  = 1000
SET @Iterations	=0
SET @i = 0

SELECT @RowsToDelete = COUNT(*) FROM BlastActivityRefer ba WITH (NOLOCK) JOIN ECN_Temp..BlastDelete bd ON ba.BlastID = bd.BlastId
SELECT @Iterations = (@RowsToDelete / @Batchsize) + 1

SET ROWCOUNT @Batchsize

WHILE 
@i < @Iterations

BEGIN
SET @i = @i + 1

	DELETE 
		ba
	FROM
		BlastActivityRefer ba
		JOIN ECN_Temp..BlastDelete bd ON ba.BlastID = bd.BlastId
END
PRINT 'Deleted ' + CONVERT(VARCHAR(15),@RowsToDelete) + ' From BlastActivityRefer'



-------------------
--RESET VARIABLES
-------------------

SET @RowsToDelete =0 
SET @Batchsize   = 1000
SET @Iterations	=0
SET @i = 0

SELECT @RowsToDelete = COUNT(*) FROM BlastActivityResends ba WITH (NOLOCK) JOIN ECN_Temp..BlastDelete bd ON ba.BlastID = bd.BlastId
SELECT @Iterations = (@RowsToDelete / @Batchsize) + 1

SET ROWCOUNT @Batchsize

WHILE 
@i < @Iterations

BEGIN
SET @i = @i + 1

	DELETE 
		ba
	FROM
		BlastActivityResends ba
		JOIN ECN_Temp..BlastDelete bd ON ba.BlastID = bd.BlastId
END
PRINT 'Deleted ' + CONVERT(VARCHAR(15),@RowsToDelete) + ' From BlastActivityResends'

-------------------
--RESET VARIABLES
-------------------

SET @RowsToDelete =0 
SET @Batchsize = 1000
SET @Iterations	=0
SET @i = 0

SELECT @RowsToDelete = COUNT(*) FROM BlastActivitySocial ba WITH (NOLOCK) JOIN ECN_Temp..BlastDelete bd ON ba.BlastID = bd.BlastId
SELECT @Iterations = (@RowsToDelete / @Batchsize) + 1

SET ROWCOUNT @Batchsize

WHILE 
@i < @Iterations

BEGIN
SET @i = @i + 1

	DELETE 
		ba
	FROM
		BlastActivitySocial ba
		JOIN ECN_Temp..BlastDelete bd ON ba.BlastID = bd.BlastId
END
PRINT 'Deleted ' + CONVERT(VARCHAR(15),@RowsToDelete) + ' From BlastActivitySocial'


-------------------
--RESET VARIABLES
-------------------

SET @RowsToDelete =0 
SET @Batchsize = 1000
SET @Iterations	=0
SET @i = 0

SELECT @RowsToDelete = COUNT(*) FROM BlastActivityBounces ba WITH (NOLOCK) JOIN ECN_Temp..BlastDelete bd ON ba.BlastID = bd.BlastId
SELECT @Iterations = (@RowsToDelete / @Batchsize) + 1

SET ROWCOUNT @Batchsize

WHILE 
@i < @Iterations

BEGIN
SET @i = @i + 1

	DELETE 
		ba
	FROM
		BlastActivityBounces ba
		JOIN ECN_Temp..BlastDelete bd ON ba.BlastID = bd.BlastId
END
PRINT 'Deleted ' + CONVERT(VARCHAR(15),@RowsToDelete) + ' From BlastActivityBounces'

-------------------
--RESET VARIABLES
-------------------

SET @RowsToDelete =0 
SET @Batchsize = 1000
SET @Iterations	=0
SET @i = 0

SELECT @RowsToDelete = COUNT(*) FROM BlastActivitySuppressed ba WITH (NOLOCK) JOIN ECN_Temp..BlastDelete bd ON ba.BlastID = bd.BlastId
SELECT @Iterations = (@RowsToDelete / @Batchsize) + 1

SET ROWCOUNT @Batchsize

WHILE 
@i < @Iterations

BEGIN
SET @i = @i + 1

	DELETE 
		ba
	FROM
		BlastActivitySuppressed ba
		JOIN ECN_Temp..BlastDelete bd ON ba.BlastID = bd.BlastId
END
PRINT 'Deleted ' + CONVERT(VARCHAR(15),@RowsToDelete) + ' From BlastActivitySuppressed'

-------------------
--RESET VARIABLES
-------------------

SET @RowsToDelete =0 
SET @Batchsize = 1000
SET @Iterations	=0
SET @i = 0

SELECT @RowsToDelete = COUNT(*) FROM BlastActivityUnSubscribes ba WITH (NOLOCK) JOIN ECN_Temp..BlastDelete bd ON ba.BlastID = bd.BlastId
SELECT @Iterations = (@RowsToDelete / @Batchsize) + 1

SET ROWCOUNT @Batchsize

WHILE 
@i < @Iterations

BEGIN
SET @i = @i + 1

	DELETE 
		ba
	FROM
		BlastActivityUnSubscribes ba
		JOIN ECN_Temp..BlastDelete bd ON ba.BlastID = bd.BlastId
END
PRINT 'Deleted ' + CONVERT(VARCHAR(15),@RowsToDelete) + ' From BlastActivityUnSubscribes'


-------------------
--RESET VARIABLES
-------------------

SET @RowsToDelete =0 
SET @Batchsize = 1000
SET @Iterations	=0
SET @i = 0

SELECT @RowsToDelete = COUNT(*) FROM BlastActivitySends ba WITH (NOLOCK) JOIN ECN_Temp..BlastDelete bd ON ba.BlastID = bd.BlastId
SELECT @Iterations = (@RowsToDelete / @Batchsize) + 1

SET ROWCOUNT @Batchsize

WHILE 
@i < @Iterations

BEGIN
SET @i = @i + 1

	DELETE 
		ba
	FROM
		BlastActivitySends ba
		JOIN ECN_Temp..BlastDelete bd ON ba.BlastID = bd.BlastId
END
PRINT 'Deleted ' + CONVERT(VARCHAR(15),@RowsToDelete) + ' From BlastActivitySends'


-------------------
--RESET VARIABLES
-------------------

SET @RowsToDelete =0 
SET @Batchsize = 1000
SET @Iterations	=0
SET @i = 0

SELECT @RowsToDelete = COUNT(*) FROM BlastActivityClicks ba WITH (NOLOCK) JOIN ECN_Temp..BlastDelete bd ON ba.BlastID = bd.BlastId
SELECT @Iterations = (@RowsToDelete / @Batchsize) + 1

SET ROWCOUNT @Batchsize

WHILE 
@i < @Iterations

BEGIN
SET @i = @i + 1

	DELETE 
		ba
	FROM
		BlastActivityClicks ba
		JOIN ECN_Temp..BlastDelete bd ON ba.BlastID = bd.BlastId
END
PRINT 'Deleted ' + CONVERT(VARCHAR(15),@RowsToDelete) + ' From BlastActivityClicks'


-------------------
--RESET VARIABLES
-------------------

SET @RowsToDelete =0 
SET @Batchsize = 1000
SET @Iterations	=0
SET @i = 0

SELECT @RowsToDelete = COUNT(*) FROM BlastActivityOpens ba WITH (NOLOCK) JOIN ECN_Temp..BlastDelete bd ON ba.BlastID = bd.BlastId
SELECT @Iterations = (@RowsToDelete / @Batchsize) + 1

SET ROWCOUNT @Batchsize

WHILE 
@i < @Iterations

BEGIN
SET @i = @i + 1

	DELETE 
		ba
	FROM
		BlastActivityOpens ba
		JOIN ECN_Temp..BlastDelete bd ON ba.BlastID = bd.BlastId
END
PRINT 'Deleted ' + CONVERT(VARCHAR(15),@RowsToDelete) + ' From BlastActivityOpens'

