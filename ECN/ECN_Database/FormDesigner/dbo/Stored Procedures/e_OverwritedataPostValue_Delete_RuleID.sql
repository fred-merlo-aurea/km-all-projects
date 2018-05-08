CREATE PROCEDURE [dbo].[e_OverwritedataPostValue_Delete_RuleID]
	@Rule_Seq_ID int
AS
	Delete from OverwritedataPostValue
	where Rule_Seq_ID = @Rule_Seq_ID
