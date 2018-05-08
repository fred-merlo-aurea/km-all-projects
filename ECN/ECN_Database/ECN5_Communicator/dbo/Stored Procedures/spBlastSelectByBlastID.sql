﻿CREATE PROCEDURE [dbo].[spBlastSelectByBlastID]
@BlastID int
AS
SELECT [BlastID]
      ,[CustomerID]
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
      ,[DynamicReplyToEmail]
  FROM [BLAST]
  WHERE BlastID = @BlastID