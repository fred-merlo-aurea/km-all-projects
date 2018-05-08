CREATE PROCEDURE [dbo].[e_HistorySubscription_Select_IsUadUpdated]
	@IsUadUpdated bit
AS
	SET NOCOUNT ON
	
	Select * from HistorySubscription where IsUadUpdated = @IsUadUpdated

