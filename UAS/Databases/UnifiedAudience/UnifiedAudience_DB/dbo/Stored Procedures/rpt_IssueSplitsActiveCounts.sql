CREATE Proc [dbo].[rpt_IssueSplitsActiveCounts]
(  
	@ProductID int
) AS

BEGIN
	
	SET NOCOUNT ON

	Select * 
	FROM (
		Select Count(*) as FreeRecords, SUM(Copies) as FreeCopies
		FROM PubSubscriptions s
			JOIN UAD_Lookup..CategoryCode c on c.CategoryCodeID = s.PubCategoryID
			JOIN UAD_Lookup..TransactionCode t on t.TransactionCodeID = s.PubTransactionID
			JOIN UAD_Lookup..CategoryCodeType cg on cg.CategoryCodeTypeID = c.CategoryCodeTypeID
			JOIN UAD_Lookup..TransactionCodeType tg on tg.TransactionCodeTypeID = t.TransactionCodeTypeID
		WHERE s.PubID = @ProductID
			AND cg.CategoryCodeTypeID in (1,2) AND c.CategoryCodeValue not in (70,71)
			AND tg.TransactionCodeTypeID in (1)
			AND s.IsInActiveWaveMailing = 0) Q1
		CROSS JOIN
			(Select Count(*) as PaidRecords, SUM(Copies) as PaidCopies
			FROM PubSubscriptions s
				JOIN UAD_Lookup..CategoryCode c on c.CategoryCodeID = s.PubCategoryID
				JOIN UAD_Lookup..TransactionCode t on t.TransactionCodeID = s.PubTransactionID
				JOIN UAD_Lookup..CategoryCodeType cg on cg.CategoryCodeTypeID = c.CategoryCodeTypeID
				JOIN UAD_Lookup..TransactionCodeType tg on tg.TransactionCodeTypeID = t.TransactionCodeTypeID
			WHERE s.PubID = @ProductID
				AND cg.CategoryCodeTypeID in (3,4) AND c.CategoryCodeValue not in (70,71)
				AND tg.TransactionCodeTypeID in (3)
				AND s.IsInActiveWaveMailing = 0) Q2

END