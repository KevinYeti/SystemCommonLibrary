﻿using SystemCommonLibrary.IoC.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SystemCommonLibrary.IoC.Extensions
{
    public static class TypeExtension
    {

        private readonly static Type baseType = typeof(object);
        private readonly static Type autowiredAttributeType = typeof(AutowiredAttribute);

        public static IList<MemberInfo> GetFullMembers(this Type type)
        {
            return GetFullMembers(type, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly, new List<MemberInfo>());
        }

        private static IList<MemberInfo> GetFullMembers(this Type type, BindingFlags bindingFlags, List<MemberInfo> memberInfos)
        {
            memberInfos.AddRange(type.GetMembers(bindingFlags).Where(memberInfo => (memberInfo.MemberType == MemberTypes.Field || memberInfo.MemberType == MemberTypes.Property) && memberInfo.GetCustomAttribute(autowiredAttributeType, false) != null));
            if (type.BaseType == baseType)
            {
                return memberInfos;
            }
            return GetFullMembers(type.BaseType, bindingFlags, memberInfos);
        }
    }
}
