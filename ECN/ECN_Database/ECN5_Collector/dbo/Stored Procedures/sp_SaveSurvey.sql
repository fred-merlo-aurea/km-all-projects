CREATE Proc [dbo].[sp_SaveSurvey]           
(              
	@SurveyID int,		
	@StepNo  int,    
	@SurveyTitle varchar(100),    
	@Description varchar(1000),    	
	@CustomerID int,    
	@CreatedUserID int,    
	@EnableDate varchar(10),    
	@DisableDate varchar(10),    
	@IntroHTML text,    
	@ThankyouHTML text,    
	@IsActive bit 
)    
as    
Begin    
	declare @groupID int

	set @groupID = 0

	if @StepNo = 1    
	Begin    
		if @SurveyID = 0    
		Begin    
			Insert into Survey     
				(SurveyTitle, Description, CustomerID, CreatedUserID, enabledate, disabledate, IntroHTML, ThankyouHTML, IsActive, CompletedStep, CreatedDate, UpdatedUserID)     
			values    
				(@SurveyTitle, @Description, @CustomerID, @CreatedUserID, 
				case when len(ltrim(rtrim(@EnableDate))) = 0 then NULL else @EnableDate end,     
				case when len(ltrim(rtrim(@DisableDate))) = 0 then NULL else @DisableDate end,
				@IntroHTML, @ThankyouHTML, 0, @StepNo, getdate(), @CreatedUserID)    

			set @SurveyID = @@IDENTITY   
			
			Insert into ecn5_communicator..Groups (CustomerID,FolderID,GroupName,GroupDescription,OwnerTypeCode,MasterSupression,PublicFolder,OptinHTML,OptinFields,AllowUDFHistory)
			values (@CustomerID, 0, replace(@SurveyTitle,' ', '_') + '_Group', replace(@SurveyTitle,' ', '_') + '_Group', 'customer', 0, 1, '', '', 'N')
			
			set @groupID = @@IDENTITY    

			update survey set GroupID = @groupID where SurveyID = @SurveyID

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
		End    
		Else    
		Begin    
			
			Update Survey    
			Set	SurveyTitle=@SurveyTitle, 
				CreatedUserID=@CreatedUserID,  
				Description=@Description,	   
				enabledate=case when len(ltrim(rtrim(@EnableDate))) = 0 then NULL else @EnableDate end,     
				disabledate=case when len(ltrim(rtrim(@DisableDate))) = 0 then NULL else @DisableDate end,
				UpdatedDate=getdate()
			where		 
				SurveyID = @SurveyID    


			select @groupID = GroupID from survey where SurveyID = @SurveyID

			if exists (select groupID from ecn5_communicator..Groups where groupID = @groupID)
			Begin
				update ecn5_communicator..Groups 
				set GroupName = replace(@SurveyTitle,' ', '_') + '_Group',
					GroupDescription= replace(@SurveyTitle,' ', '_') + '_Group'
				where 
					GroupID = @groupID
			end
			else
			Begin
				Insert into ecn5_communicator..Groups (CustomerID,FolderID,GroupName,GroupDescription,OwnerTypeCode,MasterSupression,PublicFolder,OptinHTML,OptinFields,AllowUDFHistory)
				values (@CustomerID, 0, replace(@SurveyTitle,' ', '_') + '_Group', replace(@SurveyTitle,' ', '_') + '_Group', 'customer', 0, 1, '', '', 'N')
				
				set @groupID = @@IDENTITY    

				update survey set GroupID = @groupID where SurveyID = @SurveyID
			end

		End    
	End    
	else if @StepNo = 2 or @StepNo = 6
	Begin    
		Update Survey    
		Set	CompletedStep = (case when completedStep > @StepNo then completedStep else @StepNo end),    
			UpdatedDate=getdate()  
		where		 
		SurveyID = @SurveyID    
	End    
	else if @StepNo = 3    
	Begin    
		Update Survey    
		Set	CompletedStep = (case when completedStep > @StepNo then completedStep else @StepNo end),    
			UpdatedDate=getdate()  
		where		 
				SurveyID = @SurveyID    
	End    
	else if @StepNo = 4    
	Begin    
		Update	Survey     
		Set		IntroHTML = @IntroHTML,
				ThankYouHTML = @ThankYouHTML,
				CompletedStep = (case when completedStep > @StepNo then completedStep else @StepNo end),    
				UpdatedDate=getdate()     
		where		 
				SurveyID = @SurveyID     
	End    
	else if @StepNo = 5    
	Begin    
		Update	Survey     
		Set		IsActive = @IsActive,
				CompletedStep = (case when completedStep > @StepNo then completedStep else @StepNo end),    
				UpdatedDate=getdate()     
		where		 
				SurveyID = @SurveyID     
	End 

	Select @SurveyID    
End
