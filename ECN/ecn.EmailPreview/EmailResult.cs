using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EmailPreview
{
    /// <summary>
    /// EmailTestID - Test ID for EmailResult set
    /// ZipFile - URL for downloading EmailResult set
    /// ApplicationLongName = The longer, friendlier name of the client that you can show to your users
    /// ApplicationName = A unique identifier for each client, we guarantee that this name won't change for any given client (although we may deprecate one and announce another in its place, eg "IPHONE2" may be replaced by "IPHONE3", with an overlap inbetween, you will be notified of this)
    /// AverageTimeToProcess = To find the total time until a test will complete, select the highest value of all clients' AverageTimeToProcess. For example, if you perform a test on two clients and one returns 125, the other 180, the total time to process is 180 seconds (or 3 minutes). 
    /// IsBusinessClient = If Litmus currently considers this client a "business client", this value will be true. Business clients include Outlook and Lotus Notes.
    /// Completed = This is an important property as, when true, it indicates that the result has finished processing and the result is ready to be viewed. Please note that if a result fails for some reason, Completed is always set to true. It is therefore safe to use the Completed property to know when to stop polling for updates for a particular result, and when to show the result to your user.
    /// DesktopClient = Desktop clients are those that run locally, on the desktop. Examples include Outlook, Lotus Notes, Apple Mail and Thunderbird. Email clients such as Gmail, AOL and Hotmail would have a DesktopClient value of false.  
    /// FoundInSpam = Indicates if the email was found in this client's spam folder. Since not all clients support this property, it may always be false for some cilents.  
    /// PlatformLongName = The long, friendly name of the platform this client is running on. In this case of multi-platform applications (such as Thunderbird), the platform Litmus uses is used. The PlatformLongName includes the platform's manufacturer, as well as the platform name.
    ///                    In the case of web-based email clients, both PlatformLongName and PlatformName as set to "Web-based", if you wish to customise this, use the DesktopClient boolean property to detect if the client has an operating system in its platform name and act accordingly.
    /// PlatformName = The shorter name of the platform, usually excludes the manufacturer of the operating system. In the case of web-based email clients, both PlatformLongName and PlatformName as set to "Web-based
    /// ResultType  = Contains either "email", "spam" or "page". If you've selected any spam clients for your email test, you will have some results with a ResultType of "spam". Most resellers chose to display spam results in a separate section. 
    /// SpamScore = If ResultType was equal to "spam", this property may contain a score left by the spam filter this Client object represents. Understanding spam results is covered in greater detail later.
    /// Id = This property represents a unique identifier for this result (not this client). You can use this id when using GetResult.
    /// State = State is the current state of your result, it can be either "pending", "complete" or "error":
    /// Status = Represents a client's current status. Values are: 0 Client is performing well, no delays. 
    ///                                                            1 Client is running slower than usual, expect delays of up to 15 minutes. 
    ///                                                            2 Client is currently unavailable, you should avoid ordering new tests for this client, but any ordered tests will be honored when the client recovers.  
    /// SupportsContentBlocking = If a client supports both "images on" and "images off" captures, that is, the client has some kind of view where the user can't see the images in their email, this property will be true.
    ///                           Some clients, such as Apple Mail and some webmail clients, don't support external content blocking, and SupportsContentBlocking will be false.
    /// WindowImage = The uri of a capture of the client's inbox. You should only use this property if SupportsContentBlocking is false.
    ///               ****To make the link valid, you need to prepend a protocl to the uri. Litmus doesn't add the protocol for you because you may be linking to this (or embedding it in an iframe) in a page over SSL and this would lead to mixed content warnings in Internet Explorer
    /// WindowImageNoContentBlocking = The uri of a capture of the client's inbox with external content blocking disabled, this is the "images on" capture. You should only use this property if SupportsContentBlocking is true.
    ///                                **** To make the link valid, you need to prepend a protocl to the uri. Litmus doesn't add the protocol for you because you may be linking to this (or embedding it in an iframe) in a page over SSL and this would lead to mixed content warnings in Internet Explorer
    /// WindowImageContentBlocking = The uri of a capture of the client's inbox with external content blocking enabled, this is the "images off" capture. You should only use this property if SupportsContentBlocking is true.
    ///                               **** To make the link valid, you need to prepend a protocl to the uri. Litmus doesn't add the protocol for you because you may be linking to this (or embedding it in an iframe) in a page over SSL and this would lead to mixed content warnings in Internet Explorer
    /// WindowImageThumb, WindowImageThumbContentBlocking, WindowImageThumbNoContentBlocking - same as Images
    /// FullpageImage = The uri of a capture of the email opened in the client. You should only use this property if SupportsContentBlocking is false.
    ///                 ****To make the link valid, you need to prepend a protocl to the uri. Litmus doesn't add the protocol for you because you may be linking to this (or embedding it in an iframe) in a page over SSL and this would lead to mixed content warnings in Internet Explorer
    /// FullpageImageNoContentBlocking, FullpageImageContentBlocking
    /// FullpageImageThumb, FullpageImageThumbContentBlocking, FullpageImageThumbNoContentBlocking
    /// </summary>
    [Serializable]
    [DataContract]
    public class EmailResult
    {
        public EmailResult() { }
        #region Properties
        [Newtonsoft.Json.JsonProperty(PropertyName = "EmailTestID")]
        [DataMember]
        public int EmailTestID { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "ZipFile")]
        [DataMember]
        public string ZipFile { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "ApplicationLongName")]
        [DataMember]
        public string ApplicationLongName { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "ApplicationName")]
        [DataMember]
        public string ApplicationName { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "AverageTimeToProcess")]
        [DataMember]
        public int AverageTimeToProcess { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "IsBusinessClient")]
        [DataMember]
        public bool? IsBusinessClient { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "Completed")]
        [DataMember]
        public bool? Completed { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "DesktopClient")]
        [DataMember]
        public bool? DesktopClient { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "FoundInSpam")]
        [DataMember]
        public bool? FoundInSpam { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "PlatformLongName")]
        [DataMember]
        public string PlatformLongName { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "PlatformName")]
        [DataMember]
        public string PlatformName { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "ResultType")]
        [DataMember]
        public string ResultType { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "SpamScore")]
        [DataMember]
        public double SpamScore { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "Id")]
        [DataMember]
        public int Id { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "State")]
        [DataMember]
        public string State { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "Status")]
        [DataMember]
        public int Status { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "SupportsContentBlocking")]
        [DataMember]
        public bool? SupportsContentBlocking { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "WindowImage")]
        [DataMember]
        public string WindowImage { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "WindowImageContentBlocking")]
        [DataMember]
        public string WindowImageContentBlocking { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "WindowImageNoContentBlocking")]
        [DataMember]
        public string WindowImageNoContentBlocking { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "WindowImageThumb")]
        [DataMember]
        public string WindowImageThumb { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "WindowImageThumbContentBlocking")]
        [DataMember]
        public string WindowImageThumbContentBlocking { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "WindowImageThumbNoContentBlocking")]
        [DataMember]
        public string WindowImageThumbNoContentBlocking { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "FullpageImage")]
        [DataMember]
        public string FullpageImage { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "FullpageImageContentBlocking")]
        [DataMember]
        public string FullpageImageContentBlocking { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "FullpageImageNoContentBlocking")]
        [DataMember]
        public string FullpageImageNoContentBlocking { get; set; }

        [Newtonsoft.Json.JsonProperty(PropertyName = "FullpageImageThumb")]
        [DataMember]
        public string FullpageImageThumb { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "FullpageImageThumbContentBlocking")]
        [DataMember]
        public string FullpageImageThumbContentBlocking { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "FullpageImageThumbNoContentBlocking")]
        [DataMember]
        public string FullpageImageThumbNoContentBlocking { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "SpamHeaders")]
        [DataMember]
        public List<SpamHeader> SpamHeaders { get; set; }
        [Newtonsoft.Json.JsonProperty(PropertyName = "Summary")]
        [DataMember]
        public System.Text.StringBuilder Summary { get; set; }
        #endregion
    }

    public static class EmailResultEnum
    {
        public enum State
        {
            pending,
            complete,
            error
        }
        public enum Status : int
        {
            Good = 0, //Client is performing well, no delays. 
            Slow = 1, //Client is running slower than usual, expect delays of up to 15 minutes. 
            Unavailable = 2 //Client is currently unavailable, you should avoid ordering new tests for this client, but any ordered tests will be honored when the client recovers. 
        }
        public enum ResultType
        {
            email = 0,
            spam = 1
        }
        public enum EmailClient 
        {
            appmail4,
            appmail5,
            ol2000,
            ol2002,
            ol2003,
            ol2007,
            ol2010,
            ol2013,
            gmailnew,
            ffgmailnew,
            hotmail,
            ffhotmail,
            yahoo,
            ffyahoo,
            notes6,
            notes7,
            notes8,
            notes85,
            thunderbird3,
            thunderbirdlatest,
            plaintext,
            colorblind,
            windowsphone7,
            blackberryhtml,
            iphone3,
            iphone4,
            ipad3,
            android22,
            android4
        }
        public enum EmailSpam
        {
            messagelabs,
            postini,
            barracuda,
            outlookfilter,
            yahoospam,
            aolonlinespam,
            spamassassin3,
            gmailnewspam,
            spfcheck,
            dkimcheck,
            dkcheck,
            senderidcheck,
            gmxspam,
            mailcomspam,
            linkcheck,
            htmlvalidation
        }

        public static List<EmailClient> GetAllEmailClients()
        {
            List<EmailClient> list = new List<EmailClient>();

            list.Add(EmailClient.appmail4);
            list.Add(EmailClient.appmail5);
            list.Add(EmailClient.ol2000);
            list.Add(EmailClient.ol2002);
            list.Add(EmailClient.ol2003);
            list.Add(EmailClient.ol2007);
            list.Add(EmailClient.ol2010);
            list.Add(EmailClient.ol2013);
            list.Add(EmailClient.gmailnew);
            list.Add(EmailClient.ffgmailnew);
            list.Add(EmailClient.hotmail);
            list.Add(EmailClient.ffhotmail);
            list.Add(EmailClient.yahoo);
            list.Add(EmailClient.ffyahoo);
            list.Add(EmailClient.notes6);
            list.Add(EmailClient.notes7);
            list.Add(EmailClient.notes8);
            list.Add(EmailClient.notes85);
            list.Add(EmailClient.thunderbird3);
            list.Add(EmailClient.thunderbirdlatest);
            list.Add(EmailClient.plaintext);
            list.Add(EmailClient.colorblind);
            list.Add(EmailClient.windowsphone7);
            list.Add(EmailClient.blackberryhtml);
            list.Add(EmailClient.iphone3);
            list.Add(EmailClient.iphone4);
            list.Add(EmailClient.ipad3);
            list.Add(EmailClient.android22);
            list.Add(EmailClient.android4);

            return list;
        }
        public static List<EmailSpam> GetAllEmailSpams()
        {
            List<EmailSpam> list = new List<EmailSpam>();

            list.Add(EmailSpam.messagelabs);
            list.Add(EmailSpam.postini);
            list.Add(EmailSpam.barracuda);
            list.Add(EmailSpam.outlookfilter);
            list.Add(EmailSpam.yahoospam);
            list.Add(EmailSpam.aolonlinespam);
            list.Add(EmailSpam.spamassassin3);
            list.Add(EmailSpam.gmailnewspam);
            list.Add(EmailSpam.spfcheck);
            list.Add(EmailSpam.dkimcheck);
            list.Add(EmailSpam.dkcheck);
            list.Add(EmailSpam.senderidcheck);
            list.Add(EmailSpam.gmxspam);
            list.Add(EmailSpam.mailcomspam);
            list.Add(EmailSpam.linkcheck);
            list.Add(EmailSpam.htmlvalidation);

            return list;
        }
    }
}
