CREATE  PROC dbo.e_BaseChannel_Select_SubDomain 
(
	@SubDomain varchar(50)
)
AS 
BEGIN
	select * from Basechannel where BrandSubDomain=@SubDomain
END