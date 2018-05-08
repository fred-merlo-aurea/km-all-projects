CREATE proc [dbo].[sp_copysurvey]
(
	@NewsurveyID int,
	@OldSurveyID int
)
as
Begin

	set nocount on

	update survey
	set IntroHTML = inn.IntroHTML,
		ThankYouHTML = inn.ThankYouHTML
	from 
		survey s join
		(select @NewsurveyID as SurveyID, IntroHTML, ThankYouHTML from survey where SurveyID= @OldSurveyID) inn
		on s.SurveyID = inn.SurveyID
	where 
		s.SurveyID = @NewsurveyID

	delete from page where SurveyID= @NewsurveyID

	delete from [SURVEYSTYLES] where SurveyID= @NewsurveyID


	insert into [SURVEYSTYLES] (SurveyID,pWidth,pbgcolor,pAlign,pBorder,pBordercolor,pfontfamily,
		hImage,hAlign,hMargin,hbgcolor,phbgcolor,phfontsize,phcolor,phBold,pdbgcolor,pdfontsize,pdcolor,pdbold,
		bbgcolor,qcolor,qfontsize,qbold,acolor,abold,afontsize,fImage,fAlign,fMargin,fbgcolor,ShowQuestionNo)
	select @NewsurveyID,pWidth,pbgcolor,pAlign,pBorder,pBordercolor,pfontfamily,
	hImage,hAlign,hMargin,hbgcolor,phbgcolor,phfontsize,phcolor,phBold,pdbgcolor,pdfontsize,pdcolor,pdbold,
	bbgcolor,qcolor,qfontsize,qbold,acolor,abold,afontsize,fImage,fAlign,fMargin,fbgcolor, ShowQuestionNo 
	from [SURVEYSTYLES] 
	where SurveyID= @OldSurveyID

	insert into page (SurveyID, PageHeader, PageDesc, number)
	select @NewsurveyID, PageHeader, PageDesc, number 
	from page where SurveyID= @OldSurveyID

	insert into Question(SurveyID,number,PageID,format,grid_control_type,QuestionText,preface,maxlength,Required,GridValidation,ShowTextControl)
	select @NewsurveyID,q.number,p1.PageID,q.format,q.grid_control_type,q.QuestionText,q.preface,q.maxlength,q.Required,q.GridValidation,q.ShowTextControl
	from question q join page p on p.PageID = q.PageID join page p1 on p1.number = p.number
	where p.SurveyID= @OldSurveyID and p1.SurveyID = @NewsurveyID

	insert into response_options (QuestionID, OptionValue, score)
	select q1.QuestionID, OptionValue, score
	from response_options ro join question q on q.QuestionID = ro.QuestionID join
	question q1 on q1.number = q.number
	where q.SurveyID = @OldSurveyID and q1.SurveyID = @NewsurveyID

	insert into grid_statements (QuestionID, GridStatement)
	select q1.QuestionID, GridStatement
	from grid_statements gs join question q on q.QuestionID = gs.QuestionID join
	question q1 on q1.number = q.number
	where q.SurveyID = @OldSurveyID and q1.SurveyID = @NewsurveyID

	insert into [SURVEYBRANCHING] (SurveyID,QuestionID,OptionID,PageID,EndSurvey)
	select 
		@NewsurveyID, 
		(select q1.QuestionID from question q join question q1 on q.number = q1.number where q.QuestionID = sb.QuestionID and q1.SurveyID = @NewsurveyID),
		(select ro1.OptionID 
			from response_options ro join response_options ro1 on ro.OptionValue= ro1.OptionValue join
				 question q on q.QuestionID = ro.QuestionID join
				 question q1 on q1.QuestionID = ro1.QuestionID and q.number = q1.number
			where ro.OptionID  = sb.OptionID and q.SurveyID = @OldSurveyID and q1.SurveyID = @NewsurveyID
		),
		(select p1.PageID from page p join page p1 on p.number = p1.number where p.PageID = sb.PageID and p.SurveyID = @OldSurveyID and p1.SurveyID = @NewsurveyID),
		endsurvey
		
from [SURVEYBRANCHING] sb where SurveyID = @OldSurveyID

end
