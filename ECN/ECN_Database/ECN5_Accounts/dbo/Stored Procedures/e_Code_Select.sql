CREATE PROCEDURE [dbo].[e_Code_Select]   

AS
	select * from Code where IsDeleted = 0