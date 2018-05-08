using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkUAD.Object
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
                List<FrameworkUAD.Entity.UserDataMask> udm = FrameworkUAD.BusinessLogic.UserDataMask.GetByUserID(clientconnection, user.UserID);

                foreach (FrameworkUAD.Entity.UserDataMask u in udm)
                {
                    if (u.MaskField.Equals("Address2", StringComparison.OrdinalIgnoreCase))
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
            else if (o is List<Object.Subscriber>)
            {
                List<Object.Subscriber> Subscriber = (List<Object.Subscriber>) o;

                //if (!KM.Platform.User.IsAdministrator(user))
                //{
                List<FrameworkUAD.Entity.UserDataMask> udm = FrameworkUAD.BusinessLogic.UserDataMask.GetByUserID(clientconnection, user.UserID);
                string maskField = string.Empty;

                foreach (FrameworkUAD.Entity.UserDataMask u in udm)
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

                    Type t = typeof(Object.Subscriber);
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
            else if (o is Object.Subscriber)
            {
                Object.Subscriber s = (Object.Subscriber) o;

                //if (!KM.Platform.User.IsAdministrator(user))
                //{
                List<FrameworkUAD.Entity.UserDataMask> udm = FrameworkUAD.BusinessLogic.UserDataMask.GetByUserID(clientconnection, user.UserID);
                string maskField = string.Empty;

                foreach (FrameworkUAD.Entity.UserDataMask u in udm)
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

                    Type t = typeof(Object.Subscriber);
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
