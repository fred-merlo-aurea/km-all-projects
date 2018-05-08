CREATE PROCEDURE [dbo].[rpt_TransformationCount_clientID]
	@ClientID int
AS
BEGIN

	set nocount on

	select COUNT(t.TransformationID) as 'Transformation Count'
	from Transformation t
	join TransformationFieldMap m on t.TransformationID = m.TransformationID  
	join SourceFile s on m.SourceFileID = s.SourceFileID 
	where t.IsActive='true' and m.IsActive='true' and s.IsDeleted = 'false' and t.ClientID = @ClientID

END
GO