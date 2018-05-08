CREATE PROC [dbo].[ListContentAndLayoutForCusomter] @customerID int
AS
select contentID from content where customerID = @customerID and UpdatedDate > '2005-2-2'
select LayoutID from [LAYOUT] where customerID = @customerID and UpdatedDate > '2005-2-2'
select count(*) AS FilterCount from [CONTENTFILTER] where LayoutID in (select LayoutID from [LAYOUT] where customerID = @customerID and UpdatedDate > '2005-2-2')
select count(*) AS FilterDetailCount from [CONTENTFILTERDETAIL] where FilterID in (select filterID from [CONTENTFILTER] where LayoutID in (select LayoutID from [LAYOUT] where customerID = @customerID and UpdatedDate > '2005-2-2'))
select count(*) AS LinkAliasCount from LinkAlias where ContentID in (select contentID from content where customerID = @customerID and UpdatedDate > '2005-2-2')
