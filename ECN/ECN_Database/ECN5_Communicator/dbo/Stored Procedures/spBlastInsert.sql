CREATE PROCEDURE [dbo].[spBlastInsert]
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
@FinishTime datetime = null,
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


INSERT INTO [BLAST]
           ([CustomerID]
           ,[EmailSubject]
           ,[EmailFrom]
           ,[EmailFromName]
           ,[SendTime]
           ,[AttemptTotal]
           ,[SendTotal]
           ,[SendBytes]
           ,[StatusCode]
           ,[BlastType]
           ,[CodeID]
           ,[LayoutID]
           ,[GroupID]
           ,[FinishTime]
           ,[SuccessTotal]
           ,[BlastLog]
           ,[CreatedUserID]
           ,[FilterID]
           ,[Spinlock]
           ,[ReplyTo]
           ,[TestBlast]
           ,[BlastFrequency]
           ,[RefBlastID]
           ,[BlastSuppression]
           ,[AddOptOuts_to_MS]
           ,[DynamicFromName]
           ,[DynamicFromEmail]
           ,[DynamicReplyToEmail])
     VALUES
           (@CustomerID,@EmailSubject,@EmailFrom,@EmailFromName,@SendTime,@AttemptTotal,@SendTotal,@SendBytes,@StatusCode,@BlastType,@CodeID,@LayoutID,@GroupID,@FinishTime,
			@SuccessTotal,@BlastLog,@UserID,@FilterID,@Spinlock,@ReplyTo,@TestBlast,@BlastFrequency,@RefBlastID,@blastsuppression,@AddOptOuts_to_MS,@DynamicFromName,
			@DynamicFromEmail,@DynamicReplyToEmail);SELECT @@IDENTITY;
