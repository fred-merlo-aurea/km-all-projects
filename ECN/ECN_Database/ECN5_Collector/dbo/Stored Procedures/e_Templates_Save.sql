﻿CREATE Proc [dbo].[e_Templates_Save]           
(             
        @CustomerID int,     
        @TemplateName varchar(500) = NULL ,  
        @TemplateImage varchar(500) = NULL ,  
        @IsDefault bit = NULL ,    
        @pWidth varchar(500) = NULL ,        
        @pbgcolor varchar(500) = NULL ,        
        @pAlign varchar(500) = NULL ,        
        @pBorder bit ,        
        @pBordercolor varchar(500) = NULL ,        
        @pfontfamily varchar(500) = NULL ,        
        @hImage varchar(500) = NULL ,        
        @hAlign varchar(500) = NULL ,        
        @hMargin varchar(500) = NULL ,        
        @hbgcolor varchar(500) = NULL ,        
        @phbgcolor varchar(500) = NULL ,        
        @phfontsize varchar(500) = NULL ,        
        @phcolor varchar(500) = NULL ,        
        @phBold bit ,        
        @pdbgcolor varchar(500) = NULL ,        
        @pdfontsize varchar(500) = NULL ,        
        @pdcolor varchar(500) = NULL ,        
        @pdbold bit ,        
        @bbgcolor varchar(500) = NULL ,        
        @qcolor varchar(500) = NULL ,        
        @qfontsize varchar(500) = NULL ,        
        @qbold bit ,        
        @acolor varchar(500) = NULL ,        
        @abold bit ,        
        @afontsize varchar(500) = NULL ,        
        @fImage varchar(500) = NULL ,        
        @fAlign varchar(500) = NULL ,        
        @fMargin varchar(500) = NULL ,        
        @fbgcolor varchar(500) = NULL ,        
        @ShowQuestionNo bit,
        @IsActive bit
)    
as    
Begin    

if @IsDefault=1
BEGIN
	Update Templates set IsDefault=0 where CustomerID=@CustomerID 
END

insert into Templates(
					CustomerID,     
					TemplateName,  
					TemplateImage,  
					IsDefault,    
					 pAlign ,
					 pbgcolor,
					 pBorder, 
					 pBordercolor,
					 pfontfamily , 
					 pWidth,
					 hImage ,
					 hAlign ,
					 hbgcolor ,
					 hMargin,
					 phbgcolor ,
					 phBold ,
					 phcolor ,
					 phfontsize ,
					 pdbgcolor ,
					 pdBold ,
					 pdcolor ,
					 pdfontsize,
					 bbgcolor ,
                     ShowQuestionNo ,
					 qBold,
					 qcolor, 
					 qfontsize,
					 aBold ,
					 acolor ,
					 afontsize ,
					 fImage ,
					 fAlign ,
					 fbgcolor ,
					 fMargin,
					 CreatedDate)
					values 
					(
					@CustomerID,     
					@TemplateName,  
					@TemplateImage,  
					@IsDefault,    
					 @pAlign ,
					 @pbgcolor,
					 @pBorder, 
					 @pBordercolor,
					 @pfontfamily , 
					 @pWidth,
					 @hImage ,
					 @hAlign ,
					 @hbgcolor ,
					 @hMargin,
					 @phbgcolor ,
					 @phBold ,
					 @phcolor ,
					 @phfontsize ,
					 @pdbgcolor ,
					 @pdBold ,
					 @pdcolor ,
					 @pdfontsize,
					 @bbgcolor ,
                     @ShowQuestionNo ,
					 @qBold,
					 @qcolor, 
					 @qfontsize,
					 @aBold ,
					 @acolor ,
					 @afontsize ,
					 @fImage ,
					 @fAlign ,
					 @fbgcolor ,
					 @fMargin,
					 GETDATE()
					)
					select @@IDENTITY
End