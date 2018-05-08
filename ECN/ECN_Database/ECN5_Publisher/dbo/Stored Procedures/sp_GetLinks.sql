--
CREATE PROCEDURE [dbo].[sp_GetLinks]  
 @EditionID int,  
 @Pageno varchar(20)  
AS  
Begin  
  SELECT 1   as Tag,  
     NULL    as Parent,  
     Page.PageID  as [Image!1!PageID],  
     Page.height  as [Image!1!height],  
     Page.PageNumber as [Image!1!PageNo],  
     null    as [Link!2!LinkID],  
     null    as [Link!2!type],
	 null    as [Link!2!Alias], 		  
     null    as [Link!2!x1],  
     null    as [Link!2!y1],  
     null    as [Link!2!x2],  
     null    as [Link!2!y2],  
     null    as [Link!2!]  
 from Page   
 where EditionID =@EditionID and Page.PageNumber in (select items from dbo.fn_Split(@Pageno,','))  
 union   
 SELECT distinct 2    as Tag,  
     1 as Parent,  
     Page.PageID,  
     Page.height,  
     Page.PageNumber,  
     LinkID,    
     lower(linktype),
	 isnull(alias,''),
     x1,  
     y1,  
     x2,  
     y2,       
     LinkURL  
 from Page join Link on Page.pageID = Link.pageID  
 where EditionID =@EditionID and  Page.PageNumber in (select items from dbo.fn_Split(@Pageno,','))  
 ORDER BY [Image!1!PageID],[Link!2!Alias],[Link!2!]    
 FOR XML EXPLICIT  
End
