CREATE PROCEDURE [dbo].[e_Form_ActiveByGroup] 
	@CustomerID INT,
	@GroupID INT
AS     
BEGIN 
	IF EXISTS (
		SELECT 
			TOP 1 Form_Seq_ID
		FROM 
			Form WITH (NOLOCK)
		WHERE 
			CustomerID = @CustomerID AND 
			GroupID = @GroupID AND
			(
				(
					ActivationDateFrom is not null and
					ActivationDateTo is not null and
					GETDATE() between ActivationDateFrom and ActivationDateTo
				) or
				(
					ActivationDateFrom is not null and
					ActivationDateFrom >= GETDATE()
				) or
				(
					Active = 0
				)
			)
	) 
	BEGIN
		SELECT 1 
	END	
	ELSE 
	BEGIN
		IF EXISTS(SELECT TOP 1 c.Control_ID
					FROM Control c with(nolock) 
						join Form f with(nolock) on c.Form_Seq_ID = f.Form_Seq_ID
						join NewsletterGroup ng WITH (NOLOCK) on c.Control_ID = ng.Control_ID
					WHERE ng.GroupID = @GroupID
						and f.CustomerID = @CustomerID 
						and 
						(
							(
								ActivationDateFrom is not null and
								ActivationDateTo is not null and
								GETDATE() between ActivationDateFrom and ActivationDateTo
							) or
							(
								ActivationDateFrom is not null and
								ActivationDateFrom >= GETDATE()
							) or
							(
								Active = 0
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
END