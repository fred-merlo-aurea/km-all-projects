
-- USAGE of the included functions.  
-- These functions exist in the MASTER db on 147,148,191,198,225,241,251
--Select dbo.fn_GetDateDaysFromDate(CONVERT(Date, '1/1/2000'))
--Select dbo.fn_GetDateDaysFromYear(2000)
--Select dbo.fn_GetDateDaysFromMonthYear(1,2000)

--Select dbo.fn_GetDateDaysFromDate(CONVERT(Date, '12/31/2000'))
--Select dbo.fn_GetDateDaysFromYearEND(2000)
--Select dbo.fn_GetDateDaysFromMonthYear(12,2000)

--Select dbo.fn_GetDateDaysFromDate(CONVERT(Date, '2/29/2000'))
--Select dbo.fn_GetDateDaysFromYearEND(2000)
--Select dbo.fn_GetDateDaysFromMonthYearEND(2,2000)


CREATE FUNCTION dbo.fn_GetDateDaysFromDate (@input Date)
RETURNS Integer
AS BEGIN
Declare @DayNo int

    Select @DayNo = DateDiff(dd, '1/1/1900', @input)
    return @DayNo
END
GO

CREATE FUNCTION dbo.fn_GetDateDaysFromYear (@input int)
RETURNS Integer
AS BEGIN
Declare @DayNo int

    Select @DayNo = DateDiff(dd, '1/1/1900', Convert(Date, '1/1/' + CONVERT(varchar, @input)))
    return @DayNo
END
GO

CREATE FUNCTION dbo.fn_GetDateDaysFromMonthYear (@InputMonth int, @inputYear int)
RETURNS Integer
AS BEGIN
Declare @DayNo int

    Select @DayNo = DateDiff(dd, '1/1/1900', Convert(Date, Convert(varchar, @InputMonth) + '/1/' + CONVERT(varchar, @inputYear)))
    return @DayNo
END
GO

CREATE FUNCTION dbo.fn_GetDateDaysFromYearEND (@input int)
RETURNS Integer
AS BEGIN
Declare @DayNo int

    Select @DayNo = DateDiff(dd, '1/1/1900', Convert(Date, '12/31/' + CONVERT(varchar, @input)))
    return @DayNo
END
GO

CREATE FUNCTION dbo.fn_GetDateDaysFromMonthYearEND (@InputMonth int, @inputYear int)
RETURNS Integer
AS BEGIN
Declare @DayNo int
Declare @BuildDate date

	IF @InputMonth = 12
	BEGIN
		SET @InputMonth = 0
		SET @inputYear = @inputYear + 1
	END
	SET @InputMonth = @InputMonth + 1		-- increment to start of next month so when we subtract 1 day, it is the last day of the prev month
	Select @DayNo = DateDiff(dd, '1/1/1900', Convert(Date, DateAdd(dd,-1,Convert(varchar, @InputMonth) + '/1/' + CONVERT(varchar, @inputYear))) )
	return @DayNo
END
GO
