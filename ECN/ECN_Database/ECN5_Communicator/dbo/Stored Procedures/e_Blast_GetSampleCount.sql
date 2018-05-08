CREATE PROCEDURE [dbo].[e_Blast_GetSampleCount] 
	@SampleID INT
AS     
BEGIN 
	SELECT SUM(OverrideAmount) FROM Blast WHERE SampleID=@SampleID
END
