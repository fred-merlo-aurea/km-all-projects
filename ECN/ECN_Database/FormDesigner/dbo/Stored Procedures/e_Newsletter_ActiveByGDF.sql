CREATE PROCEDURE [dbo].[e_Newsletter_ActiveByGDF]
	@CustomerID INT,
	@GroupDataFieldsID INT
AS     
BEGIN 
	IF EXISTS (
		SELECT 
			TOP 1 NewsletterGroupUDFID
		FROM 
			NewsletterGroupUDF ngUDF with (NOLOCK)
			join NewsletterGroup ng WITH (NOLOCK) on ngUDF.NewsletterGroupID = ng.NewsletterGroupID
			join [Control] c WITH (NOLOCK) on ng.Control_ID = c.Control_ID
			join Form f WITH (NOLOCK) on c.Form_Seq_ID = f.Form_Seq_ID
		WHERE 
			f.CustomerID = @CustomerID AND 
			ngUDF.NewsletterDataFieldID = @GroupDataFieldsID AND
			(
				(
					f.ActivationDateFrom is not null and
					f.ActivationDateTo is not null and
					GETDATE() between f.ActivationDateFrom and f.ActivationDateTo
				) or
				(
					f.ActivationDateFrom is not null and
					f.ActivationDateFrom >= GETDATE()
				) or
				(
					f.Active = 0
				)
			)
	) 
	BEGIN
		SELECT 1 
	END	
	ELSE 
	BEGIN
		SELECT 0
	END
END
