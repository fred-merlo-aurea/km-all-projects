CREATE PROCEDURE [dbo].[sp_PageSearch]  
 @EditionID int,  
 @searchText varchar(100)  
AS  
Begin
  
SELECT 1   as Tag,  
     NULL    as Parent,  
     Page.Pagenumber  as [Page!1!id],
	 Page.DisplayNumber  as [Page!1!displayno],  
     Page.Textcontent  as [Page!1!!cdata]  
 from Page  
where editionID = @EditionID and Convert(varchar(8000),Textcontent) like '%' + @searchText + '%' 
FOR XML EXPLICIT 

End
