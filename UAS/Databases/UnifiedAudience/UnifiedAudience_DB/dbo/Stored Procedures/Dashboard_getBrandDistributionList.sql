CREATE PROCEDURE [dbo].[Dashboard_getBrandDistributionList]
(
	@userID int = 0,
	@brandID int = 0
)
AS
Begin

	declare @userbrand table (brandID int, BrandName varchar(50), logo  varchar(100), ProductCounts int)

	if exists (select top 1 1 from userbrand with (NOLOCK) where userID = @userID)
	Begin
		insert into @userbrand
		select b.BrandID,b.BrandName, b.Logo, COUNT(distinct bd.PubID) as ProductCounts 
		from 
				brand b with (NOLOCK) join 
				userbrand ub with (NOLOCK) on b.brandID = ub.BrandID  join
				brandDetails bd on b.brandID = bd.brandID
		where
			b.IsDeleted = 0 and userID = @userID and (@brandID = 0 or b.brandID = @brandID)
		group by
			b.BrandID,b.BrandName, b.Logo
	end
	Else
	Begin
		insert into @userbrand
		select b.BrandID,b.BrandName, b.Logo, COUNT(distinct bd.PubID) as ProductCounts 
		from 
				brand b with (NOLOCK) join 
				brandDetails bd on b.brandID = bd.brandID
		where
			b.IsDeleted = 0  and (@brandID = 0 or b.brandID = @brandID)
		group by
			b.BrandID,b.BrandName, b.Logo

	End

	select ub.BrandID, ub.BrandName, ub.Logo, ub.ProductCounts, Counts as [Counts] 
	FROM 
			dbo.[Summary_Data] s WITH (NOLOCK) join 
			@userbrand ub  on ub.brandID = s.brandID 
	WHERE 
			  entityName = 'Brand'
			   AND [Type] = 'Net'
	order by BrandName
end

go