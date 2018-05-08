CREATE PROCEDURE [dbo].[e_Action_Select_CatCodeID_TranCodeID]
@CategoryCodeID int,
@TransactionCodeID int
AS    
BEGIN

	set nocount on

	SELECT *
	FROM [Action] With(NoLock)
	WHERE CategoryCodeID = @CategoryCodeID
		AND TransactionCodeID = @TransactionCodeID
		AND ActionTypeID = 1

END