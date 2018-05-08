CREATE PROCEDURE [e_CodeSheet_Select]
AS
BEGIN

	set nocount on

	SELECT  CodeSheetID,
			PubID,
			ResponseGroup,
			Responsevalue,
			Responsedesc,
			ResponseGroupID,
			DateCreated,
			DateUpdated,
			CreatedByUserID,
			UpdatedByUserID,
			DisplayOrder,
			ReportGroupID,
			IsActive,
			WQT_ResponseID,
			IsOther
	FROM CodeSheet With(NoLock)

END