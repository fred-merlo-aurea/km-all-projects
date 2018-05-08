using System;

namespace ecn.communicator.classes
{
	
	/// A basic interface for the classes that behave like blasts. This will include the blast class and the sample class initially.
	/// Structurally, it probably isn't needed because no places use blastlike as a container class yet.
	
    public interface IBlastLike {
        void CustomerID(int cid);
        void UserID(int uid);
        void Subject(string sbj) ;
        void EmailFrom(string eml_frm);
        void EmailFromName(string eml_frm_nm);
        void ReplyTo(string rply);
        void Group(Groups gid);
        void Layout(Layouts lid);
        void FilterID(int fid);
        void StatusCode (string code);
        void SendTime(string time);
    }
}
