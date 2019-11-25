﻿using System;
using System.Collections.Concurrent;
using System.Reflection;

namespace SystemCommonLibrary.Reflect
{
    public class TypeInfo
    {
        public Type Type { get; set; }

        public Object Instance { get; set; }

        public Type[] ArgTypes { get; set; }

        public MethodInfo MethodInfo { get; set; }


        private static readonly object _syncLock = new object();

        private static readonly ConcurrentDictionary<string, TypeInfo> _instanceCache = new ConcurrentDictionary<string, TypeInfo>();

        public static readonly string[] ListTypes = { "List`1", "HashSet`1", "IList`1", "ISet`1", "ICollection`1", "IEnumerable`1" };

        public static readonly string[] DicTypes = { "Dictionary`2", "IDictionary`2" };

        /// <summary>
        /// 添加或获取实例
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static TypeInfo GetOrAddInstance(Type type, string methodName = "Add")
        {
            if (type.IsInterface)
            {
                throw new Exception("服务方法中不能包含接口内容！");
            }

            if (type.IsClass)
            {
                var fullName = type.FullName + methodName;

                TypeInfo typeInfo = _instanceCache.GetOrAdd(fullName, (v) =>
                {
                    Type[] argsTypes = null;

                    if (type.IsGenericType)
                    {
                        argsTypes = type.GetGenericArguments();
                        type = type.GetGenericTypeDefinition().MakeGenericType(argsTypes);
                    }

                    var mi = type.GetMethod(methodName);

                    return new TypeInfo()
                    {
                        Type = type,
                        MethodInfo = mi,
                        ArgTypes = argsTypes
                    };
                });
                typeInfo.Instance = Activator.CreateInstance(type);
                return typeInfo;
            }
            return null;
        }

        /// <summary>
        /// 添加或获取实例
        /// </summary>
        /// <param name="type"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        public static TypeInfo GetOrAddInstance(Type type, MethodInfo info)
        {
            lock (_syncLock)
            {
                if (type.IsInterface)
                {
                    throw new Exception("服务方法中不能包含接口内容！");
                }

                var fullName = type.FullName + info.Name;

                TypeInfo typeInfo = _instanceCache.GetOrAdd(fullName, (v) =>
                {
                    return new TypeInfo()
                    {
                        Type = type,
                        MethodInfo = info
                    };
                });
                typeInfo.Instance = Activator.CreateInstance(type);
                return typeInfo;
            }
        }
    }
}
