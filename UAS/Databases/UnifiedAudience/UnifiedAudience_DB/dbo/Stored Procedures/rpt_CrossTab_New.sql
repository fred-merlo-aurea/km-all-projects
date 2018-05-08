CREATE PROCEDURE rpt_CrossTab_New
@ProductID int,
@Row varchar(100),
@Col varchar(100),
@RowID int,
@ColID int
AS
BEGIN
	
	SET NOCOUNT ON

	--DECLARE @ProductID int = 1
	--DECLARE @RowID int = 1904
	--DECLARE @ColID int = 1904
	--DECLARE @Row varchar(100) = 'PRODUCTS'
	--DECLARE @Col varchar(100) = 'PURCHASE'
	DECLARE @executeString varchar(2000) = ''
	DECLARE @ResponseFieldID int = (SELECT CodeId FROM UAD_Lookup..Code c JOIN UAD_Lookup..CodeType ct ON c.CodeTypeId = ct.CodeTypeId WHERE ct.CodeTypeName = 'Dimension' AND c.CodeName = 'Response Group')
	DECLARE @ProfileFieldID int = (SELECT CodeId FROM UAD_Lookup..Code c JOIN UAD_Lookup..CodeType ct ON c.CodeTypeId = ct.CodeTypeId WHERE ct.CodeTypeName = 'Dimension' AND c.CodeName = 'Profile Field')
	BEGIN
		IF @RowID = @ResponseFieldID AND @ColID = @ResponseFieldID
			BEGIN
				Set @executeString =
				'SELECT q1.PubSubscriptionID, q1.[' + @Row + '], q2.[' + @Col + '], q1.Copies, 1 as RecordCount, q1.Demo7, q1.DisplayOrder as RowDisplayOrder, q2.DisplayOrder as ColDisplayOrder
				FROM
				(SELECT ps.PubSubscriptionID, ps.Copies, ps.Demo7, ISNULL(cs.DisplayOrder,1000) as DisplayOrder, ISNULL(cs.Responsedesc, ''No Answer'') as ''' + @Row + '''
				FROM PubSubscriptionDetail psd 
				JOIN CodeSheet cs ON cs.CodeSheetID = psd.CodesheetID AND cs.IsActive = 1
				JOIN ResponseGroups rg ON rg.ResponseGroupID = cs.ResponseGroupID AND rg.DisplayName = ''' + @Row + '''
				RIGHT JOIN PubSubscriptions ps ON psd.PubSubscriptionID = ps.PubSubscriptionID
				WHERE ps.PubID = ' + CONVERT(varchar(50),@ProductID) + '
				) q1
				INNER JOIN
				(SELECT ps.PubSubscriptionID, ps.Copies, ps.Demo7, cs.DisplayOrder, cs.Responsedesc as ''' + @Col + '''
				FROM PubSubscriptions ps
				JOIN PubSubscriptionDetail psd ON psd.PubSubscriptionID = ps.PubSubscriptionID
				JOIN CodeSheet cs ON cs.CodeSheetID = psd.CodesheetID
				JOIN ResponseGroups rg ON rg.ResponseGroupID = cs.ResponseGroupID
				WHERE cs.IsActive = 1 AND rg.DisplayName = ''' + @Col + ''' AND ps.PubID = ' + CONVERT(varchar(50),@ProductID) + '
				) q2
				ON q1.PubSubscriptionID = q2.PubSubscriptionID ORDER BY q1.DisplayOrder, q2.DisplayOrder'
			END
		ELSE IF @RowID = @ResponseFieldID
			BEGIN
				SET @executeString =
					'SELECT ps.PubSubscriptionID, ps.Copies, ps.Demo7, ps.' + @Col + ',ISNULL(cs.DisplayOrder,1000) as RowDisplayOrder, ISNULL(cs.Responsedesc, ''No Answer'') as ''' + @Row + ''', 1 as RecordCount
					FROM PubSubscriptionDetail psd 
					JOIN CodeSheet cs ON cs.CodeSheetID = psd.CodesheetID AND cs.IsActive = 1
					JOIN ResponseGroups rg ON rg.ResponseGroupID = cs.ResponseGroupID AND rg.DisplayName = ''' + @Row + '''
					RIGHT JOIN PubSubscriptions ps ON psd.PubSubscriptionID = ps.PubSubscriptionID
					WHERE ps.PubID = ' + CONVERT(varchar(50),@ProductID)
			END
		ELSE IF @ColID = @ResponseFieldID
			BEGIN
				SET @executeString = 
					'SELECT ps.PubSubscriptionID, ps.Copies, ps.Demo7, cs.DisplayOrder as ColDisplayOrder, ps.' + @Row + ', cs.Responsedesc as ''' + @Col + ''', 1 as RecordCount
					FROM PubSubscriptions ps
					JOIN PubSubscriptionDetail psd ON psd.PubSubscriptionID = ps.PubSubscriptionID
					JOIN CodeSheet cs ON cs.CodeSheetID = psd.CodesheetID AND cs.IsActive = 1
					JOIN ResponseGroups rg ON rg.ResponseGroupID = cs.ResponseGroupID
					WHERE rg.DisplayName = ''' + @Col + ''' AND ps.PubID = ' + CONVERT(varchar(50),@ProductID)
			END
		ELSE
			BEGIN
				SET @executeString =
					'SELECT ps.PubSubscriptionID, ps.Copies, ps.Demo7, ps.' + @Row + ', ps.' + @Col + ', 1 as RecordCount
					FROM PubSubscriptions ps WHERE ps.PubID = ' + CONVERT(varchar(50),@ProductID)
			END
		exec(@executeString)
		--print @executeString
	END

END