CREATE PROCEDURE [dbo].[e_Form_ActiveByGDF] 
	@CustomerID INT,
	@GroupDataFieldsID INT
AS     
BEGIN 
	IF EXISTS (
		SELECT 
			TOP 1 Control_ID
		FROM 
			Control c WITH (NOLOCK)
			join Form f WITH (NOLOCK) on c.Form_Seq_ID = f.Form_Seq_ID
		WHERE 
			f.CustomerID = @CustomerID AND 
			c.FieldID = @GroupDataFieldsID AND
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