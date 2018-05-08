CREATE PROCEDURE [dbo].[e_RequestQueryValue_Delete_RuleID]
	@Rule_Seq_ID int
AS
	Delete from RequestQueryValue
	where Rule_Seq_ID = @Rule_Seq_ID