using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;

namespace KMPS.MD.Objects
{
    public class ProfileFieldMask
    {
        public static object MaskData(KMPlatform.Object.ClientConnections clientconnection, object o, KMPlatform.Entity.User user)
        {
            if (o is DataTable)
            {
                DataTable dt = (DataTable) o;

                //if(!KM.Platform.User.IsAdministrator(user))
                //{
                    List<UserDataMask> udm = UserDataMask.GetByUserID(clientconnection, user.UserID);

                   foreach(UserDataMask u in udm)
                    {
                       if (u.MaskField.Equals("Address2",StringComparison.OrdinalIgnoreCase))
                       {
                           if (dt.Columns.Contains("MailStop"))
                           {
                               foreach (DataRow dr in dt.Rows)
                               {
                                   dr["MAILSTOP"] = dr["MAILSTOP"].ToString() == "" ? dr["MAILSTOP"].ToString() : "xxxxxx";
                               }
                           }
                       }

                        if (dt.Columns.Contains(u.MaskField))
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                dr[u.MaskField] = dr[u.MaskField].ToString() == "" ? dr[u.MaskField].ToString() : "xxxxxx";
                            }
                        }
                    }
                //}
            }
            else if (o is List<Subscriber>)
            {
                List<Subscriber> Subscriber = (List<Subscriber>)o;

                //if (!KM.Platform.User.IsAdministrator(user))
                //{
                    List<UserDataMask> udm = UserDataMask.GetByUserID(clientconnection, user.UserID);
                    string maskField = string.Empty;

                foreach (UserDataMask u in udm)
                {
                    switch (u.MaskField.ToUpper())
                    {
                        case "FIRSTNAME":
                            maskField = "FName";
                            break;
                        case "LASTNAME":
                            maskField = "LName";
                            break;
                        case "ADDRESS2":
                            maskField = "MailStop";
                            break;
                        default:
                            maskField = u.MaskField;
                            break;
                    }

                    Type t = typeof(Subscriber);
                    PropertyInfo xPropertyInfo = t.GetProperty(maskField);

                        Subscriber = Subscriber.Select(s =>
                        {
                            if ("".Equals(xPropertyInfo.GetValue(s)))
                                xPropertyInfo.SetValue(s, "");
                            else
                                xPropertyInfo.SetValue(s, "xxxxxx");

                            return s;
                        }).ToList();
                }
                //}
            }
            else if (o is Subscriber)
            {
                Subscriber s = (Subscriber)o;

                //if (!KM.Platform.User.IsAdministrator(user))
                //{
                    List<UserDataMask> udm = UserDataMask.GetByUserID(clientconnection, user.UserID);
                    string maskField = string.Empty;

                    foreach (UserDataMask u in udm)
                    {
                        switch (u.MaskField.ToUpper())
                        {
                            case "FIRSTNAME":
                                maskField = "FName";
                                break;
                            case "LASTNAME":
                                maskField = "LName";
                                break;
                            case "ADDRESS2":
                                maskField = "MailStop";
                                break;
                            default:
                                maskField = u.MaskField;
                                break;
                        }

                        Type t = typeof(Subscriber);
                        PropertyInfo xPropertyInfo = t.GetProperty(maskField);

                        if ("".Equals(xPropertyInfo.GetValue(s)))
                            xPropertyInfo.SetValue(s, "");
                        else
                            xPropertyInfo.SetValue(s, "xxxxxx");
                    }
                //}
            }

            return o;
        }
    }
}
