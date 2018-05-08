CREATE PROCEDURE [dbo].[spBlastUpdate]
@BlastID int,
@CustomerID int,
@EmailSubject varchar(255),
@EmailFrom varchar(100),
@EmailFromName varchar(100),
@SendTime datetime,
@AttemptTotal bigint,
@SendTotal bigint,
@SendBytes bigint,
@StatusCode varchar(50),
@BlastType varchar(50),
@CodeID int,
@LayoutID int,
@GroupID int,
@FinishTime datetime,
@SuccessTotal bigint,
@BlastLog varchar(2000),
@UserID int,
@FilterID int,
@Spinlock char(1),
@ReplyTo varchar(100),
@TestBlast varchar(1),
@BlastFrequency varchar(50),
@RefBlastID varchar(2000),
@blastsuppression varchar(2000),
@AddOptOuts_to_MS bit,
@DynamicFromName varchar(100),
@DynamicFromEmail varchar(100),
@DynamicReplyToEmail varchar(100)
AS

IF (@FinishTime = '1/1/1905')
	SET @FinishTime = null;
	
UPDATE [BLAST]
   SET [CustomerID] = @CustomerID
      ,[EmailSubject] = @EmailSubject
      ,[EmailFrom] = @EmailFrom
      ,[EmailFromName] = @EmailFromName
      ,[SendTime] = @SendTime
      ,[AttemptTotal] = @AttemptTotal
      ,[SendTotal] = @SendTotal
      ,[SendBytes] = @SendBytes
      ,[StatusCode] = @StatusCode
      ,[BlastType] = @BlastType
      ,[CodeID] = @CodeID
      ,[LayoutID] = @LayoutID
      ,[GroupID] = @GroupID
      ,[FinishTime] = @FinishTime
      ,[SuccessTotal] = @SuccessTotal
      ,[BlastLog] = @BlastLog
      ,[CreatedUserID] = @UserID
      ,[FilterID] = @FilterID
      ,[Spinlock] = @Spinlock
      ,[ReplyTo] = @ReplyTo
      ,[TestBlast] = @TestBlast
      ,[BlastFrequency] = @BlastFrequency
      ,[RefBlastID] = @RefBlastID
      ,[BlastSuppression] = @blastsuppression
      ,[AddOptOuts_to_MS] = @AddOptOuts_to_MS
      ,[DynamicFromName] = @DynamicFromName
      ,[DynamicFromEmail] = @DynamicFromEmail
      ,[DynamicReplyToEmail] = @DynamicReplyToEmail
 WHERE BlastID = @BlastID
