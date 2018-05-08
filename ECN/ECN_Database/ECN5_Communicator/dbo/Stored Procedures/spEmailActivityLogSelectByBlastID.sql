CREATE PROCEDURE [dbo].[spEmailActivityLogSelectByBlastID]
@BlastID int
AS
SELECT [EAID]
      ,[EmailID]
      ,[BlastID]
      ,[ActionTypeCode]
      ,[ActionDate]
      ,[ActionValue]
      ,[ActionNotes]
      ,[Processed]
  FROM [EmailActivityLog]
  WHERE BlastID = @BlastID
