using System;

namespace KMEnums
{
    public enum ControlType
    {
        TextBox = 1,
        TextArea = 2,
        RadioButton = 3,
        CheckBox = 4,
        DropDown = 5,
        ListBox = 6,
        Grid = 7,
        PageBreak = 8,
        Hidden = 9,
        NewsLetter = 10,
        Literal = 11,
        Captcha = 12,

        FirstName = 101,
        LastName = 102,
        Email = 103,
        Phone = 104,
        Fax = 105,
        Website = 106,
        Address1 = 107,
        Address2 = 108,
        City = 109,
        State = 110,
        Zip = 111,
        Country = 112,
        Button = 113,

        Title = 201,
        FullName = 202,
        Company = 203,
        Occupation = 204,
        Mobile = 205,
        Age = 206,
        Income = 207,
        Gender = 208,
        User1 = 209,
        User2 = 210,
        User3 = 211,
        User4 = 212,
        User5 = 213,
        User6 = 214,
        Birthdate = 215,
        Notes = 216,
        Password = 217,
        
    }

    public enum CommonControlType 
    {
        FirstName = ControlType.FirstName,
        LastName = ControlType.LastName,
        Email = ControlType.Email,
        Phone = ControlType.Phone,
        Fax = ControlType.Fax,
        Website = ControlType.Website,
        Address1 = ControlType.Address1,
        Address2 = ControlType.Address2,
        City = ControlType.City,
        State = ControlType.State,
        Zip = ControlType.Zip,
        Country = ControlType.Country
    }

    public enum UncommonControlType 
    {
        Title = ControlType.Title,
        FullName = ControlType.FullName,
        Company = ControlType.Company,
        Occupation = ControlType.Occupation,
        Mobile = ControlType.Mobile,
        Age = ControlType.Age,
        Income = ControlType.Income,
        Gender = ControlType.Gender,
        User1 = ControlType.User1,
        User2 = ControlType.User2,
        User3 = ControlType.User3,
        User4 = ControlType.User4,
        User5 = ControlType.User5,
        User6 = ControlType.User6,
        Birthdate = ControlType.Birthdate,
        Notes = ControlType.Notes,
        Password = ControlType.Password
    }

    public enum StandardControlType 
    {
        FirstName = CommonControlType.FirstName,
        LastName = CommonControlType.LastName,
        Email = CommonControlType.Email,
        Phone = CommonControlType.Phone,
        Fax = CommonControlType.Fax,
        Website = CommonControlType.Website,
        Address1 = CommonControlType.Address1,
        Address2 = CommonControlType.Address2,
        City = CommonControlType.City,
        State = CommonControlType.State,
        Zip = CommonControlType.Zip,
        Country = CommonControlType.Country,

        Title = UncommonControlType.Title,
        FullName = UncommonControlType.FullName,
        Company = UncommonControlType.Company,
        Occupation = UncommonControlType.Occupation,
        Mobile = UncommonControlType.Mobile,
        Age = UncommonControlType.Age,
        Income = UncommonControlType.Income,
        Gender = UncommonControlType.Gender,
        User1 = UncommonControlType.User1,
        User2 = UncommonControlType.User2,
        User3 = UncommonControlType.User3,
        User4 = UncommonControlType.User4,
        User5 = UncommonControlType.User5,
        User6 = UncommonControlType.User6,
        Birthdate = UncommonControlType.Birthdate,
        Notes = UncommonControlType.Notes,
        Password = UncommonControlType.Password
    }

    public enum CustomControlType 
    {
        TextBox = ControlType.TextBox,
        TextArea = ControlType.TextArea,
        RadioButton = ControlType.RadioButton,
        CheckBox = ControlType.CheckBox,
        DropDown = ControlType.DropDown,
        ListBox = ControlType.ListBox,
        Grid = ControlType.Grid,
        Literal = ControlType.Literal,
        NewsLetter = ControlType.NewsLetter,
        
    }

    public enum OtherControlType 
    {
        PageBreak = ControlType.PageBreak,
        Hidden = ControlType.Hidden,
        Captcha = ControlType.Captcha
    }

    public enum StyledControlType
    {
        TextBox = ControlType.TextBox,
        TextArea = ControlType.TextArea,
        RadioButton = ControlType.RadioButton,
        CheckBox = ControlType.CheckBox,
        DropDown = ControlType.DropDown,
        ListBox = ControlType.ListBox,
        Grid = ControlType.Grid,
        NewsLetter = ControlType.NewsLetter,
        Button = ControlType.Button
    }
}