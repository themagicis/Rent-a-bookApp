using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web;

namespace RentABook.Web.Helpers
{
    public static class DisplayEnum
    {
        public static string DisplayFor(this Enum value)
        {
            Type enumType = value.GetType();
            var enumValue = Enum.GetName(enumType, value);
            MemberInfo member = enumType.GetMember(enumValue)[0];
            string outString = "";

            var attrs = member.GetCustomAttributes(typeof(DisplayAttribute), false);
            if (attrs.Any())
            {
                var displayAttr = ((DisplayAttribute)attrs[0]);

                outString = displayAttr.Name;

                if (displayAttr.ResourceType != null)
                {
                    outString = displayAttr.GetName();
                }
            }
            else
            {
                outString = value.ToString();
            }

            return outString;
        }
    }
}