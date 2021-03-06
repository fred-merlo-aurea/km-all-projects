
CREATE PROCEDURE [dbo].[e_Form_UpdateStaticstic] 
	@CurrentID INT,
	@NewID INT
AS     
BEGIN 
	
	SELECT FormStatistic_Seq_ID 
	INTO #FSIDS
	FROM FormStatistic f WITH (NOLOCK)
		WHERE f.Form_Seq_ID = @CurrentID
	DECLARE @TOBEUPDATED INT = (SELECT COUNT(1) FROM #FSIDS)
	IF @TOBEUPDATED > 0
	BEGIN
	 UPDATE FS
	   SET Form_Seq_ID=@NewID 
	    FROM FormStatistic AS FS
	    INNER JOIN #FSIDS AS TFS
	      ON TFS.FormStatistic_Seq_ID =FS.FormStatistic_Seq_ID
	END 
	--SELECT * FROM #FSIDS
	 IF(OBJECT_ID('tempdb..#FSIDS') IS NOT NULL)
          BEGIN
                DROP  TABLE #FSIDS
     END 
END