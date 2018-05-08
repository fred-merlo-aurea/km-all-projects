CREATE PROCEDURE [dbo].[e_Category_Select_All]   

AS
	select * from Category where IsDeleted=0