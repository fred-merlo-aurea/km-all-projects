CREATE PROCEDURE [dbo].[e_TransformationFieldMultiMap_Select]
AS
BEGIN

	set nocount on

	SELECT * 
	FROM TransformationFieldMultiMap With(NoLock)
	
END