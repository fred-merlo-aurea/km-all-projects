CREATE PROCEDURE [dbo].[o_TransformSplitInfo_Select]
	@SourceFileID int
AS
	SELECT DISTINCT tpm.PubID, fm.MAFField, ts.Delimiter
	FROM TransformationFieldMap tfm WITH(NOLOCK)
		JOIN TransformSplit ts WITH(NOLOCK) ON tfm.TransformationID = ts.TransformationID
		JOIN TransformationPubMap tpm WITH(NOLOCK) ON ts.TransformationID = tpm.TransformationID
		JOIN FieldMapping fm WITH(NOLOCK) ON tfm.FieldMappingID = fm.FieldMappingID
	WHERE tfm.SourceFileID = @SourceFileID
