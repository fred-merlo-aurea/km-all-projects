CREATE Proc [dbo].[e_Survey_Save]           
(            
        @SurveyID int ,        
        @SurveyTitle varchar(500) = NULL ,        
        @Description varchar(500) = NULL ,        
        @CustomerID int = NULL ,        
        @GroupID int ,        
        @EnableDate datetime = NULL ,        
        @DisableDate datetime = NULL ,        
        @IntroHTML varchar(MAX) = NULL ,        
        @ThankYouHTML varchar(MAX) = NULL , 
        @IsActive bit = NULL ,         
        @CompletedStep int = NULL ,        
        @UserID int = NULL 
)    
as    
Begin    

if @SurveyID>0
BEGIN
	update Survey
	set       
        SurveyTitle = @SurveyTitle,        
        [Description]=@Description, 
        EnableDate= @EnableDate,        
        DisableDate=@DisableDate, 
        IsActive=@IsActive,
        IntroHTML = @IntroHTML,        
        ThankYouHTML=@ThankYouHTML,        
        CompletedStep = @CompletedStep,        
        UpdatedUserID=@UserID,
        UpdatedDate= GETDATE()
    where 	      
        SurveyID = @SurveyID 
END
ELSE
BEGIN
	insert into Survey ( 
        SurveyTitle ,        
        [Description],        
        CustomerID  ,        
        GroupID  ,        
        EnableDate  ,        
        DisableDate ,
        IsActive,        
        IntroHTML  ,        
        ThankYouHTML  ,        
        CompletedStep ,        
        CreatedUserID,
        CreatedDate )
        values(        
        @SurveyTitle,        
        @Description,        
        @CustomerID,        
        @GroupID,        
        @EnableDate,        
        @DisableDate ,
        @IsActive,        
        @IntroHTML,        
        @ThankYouHTML,        
        @CompletedStep,        
        @UserID,
        GETDATE())
        
        select  @SurveyID = @@IDENTITY
        
        --create default page
			insert into page (SurveyID, PageHeader, PageDesc, number, CreatedDate) values (@SurveyID, '', '', 1, GETDATE())

			if exists (select templateID from templates where customerID = @customerID and IsDefault = 1)
			Begin
				insert into [SURVEYSTYLES] 
				(	
					SurveyID, pWidth, pbgcolor, pAlign, pBorder, pBordercolor, pfontfamily, 
					hImage, hAlign, hMargin, hbgcolor, 
					phbgcolor, phfontsize, phcolor, phBold, 
					pdbgcolor, pdfontsize, pdcolor, pdbold, 
					bbgcolor, 
					qcolor, qfontsize, qbold, 
					acolor, abold, afontsize,
					fImage, fAlign, fMargin, fbgcolor
				)
				select	@SurveyID, pWidth, pbgcolor, pAlign, pBorder, pBordercolor, pfontfamily, 
						hImage, hAlign, hMargin, hbgcolor, 
						phbgcolor, phfontsize, phcolor, phBold, 
						pdbgcolor, pdfontsize, pdcolor, pdbold, 
						bbgcolor, 
						qcolor, qfontsize, qbold, 
						acolor, abold, afontsize,
						fImage, fAlign, fMargin, fbgcolor
				from	
						Templates
				where
						customerID = @customerID and IsDefault = 1
			end
			else 
			begin

				if not exists (select templateID from templates where customerID = @customerID)
				Begin
					insert into Templates
					(	
						CustomerID,TemplateName,TemplateImage,IsDefault,pbgcolor,pAlign,pBorder,pBordercolor,pfontfamily,pWidth,
						hImage,hAlign,hMargin,hbgcolor,phbgcolor,phfontsize,phcolor,phBold,pdbgcolor,pdfontsize,pdcolor,
						pdbold,bbgcolor,qcolor,qfontsize,qbold,acolor,abold,afontsize,fImage,fAlign,fMargin,fbgcolor,IsActive
					)
					select	top 1 @customerID, TemplateName,TemplateImage,1,pbgcolor,pAlign,pBorder,pBordercolor,pfontfamily,pWidth,
								hImage,hAlign,hMargin,hbgcolor,phbgcolor,phfontsize,phcolor,phBold,pdbgcolor,pdfontsize,pdcolor,
							pdbold,bbgcolor,qcolor,qfontsize,qbold,acolor,abold,afontsize,fImage,fAlign,fMargin,fbgcolor,1
					from	Templates
					where	customerID = 1 
					order by TemplateID
				End
				
				insert into [SURVEYSTYLES]
				(	
					SurveyID, pWidth, pbgcolor, pAlign, pBorder, pBordercolor, pfontfamily, 
					hImage, hAlign, hMargin, hbgcolor, 
					phbgcolor, phfontsize, phcolor, phBold, 
					pdbgcolor, pdfontsize, pdcolor, pdbold, 
					bbgcolor, 
					qcolor, qfontsize, qbold, 
					acolor, abold, afontsize,
					fImage, fAlign, fMargin, fbgcolor
				)
				select	top 1 @SurveyID, pWidth, pbgcolor, pAlign, pBorder, pBordercolor, pfontfamily, 
						hImage, hAlign, hMargin, hbgcolor, 
						phbgcolor, phfontsize, phcolor, phBold, 
						pdbgcolor, pdfontsize, pdcolor, pdbold, 
						bbgcolor, 
						qcolor, qfontsize, qbold, 
						acolor, abold, afontsize,
						fImage, fAlign, fMargin, fbgcolor
				from	
						Templates
				where
						customerID = @customerID
			end
			
END

select  @SurveyID 
End