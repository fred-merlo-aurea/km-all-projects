CREATE PROCEDURE [dbo].[e_Code_Select_ByType]   
@CodeType varchar(50)
AS
	select * from Code where CodeType = @CodeType and IsDeleted = 0
